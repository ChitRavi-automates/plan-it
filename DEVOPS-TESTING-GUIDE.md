# DevOps Group Project - Local Testing Guide
## K3s Cluster with Full Automation

Based on your requirements:
- ✅ K3s cluster (3 control plane + 1 worker)
- ✅ Control plane NoSchedule taint
- ✅ IaC + CMT automation
- ✅ ArgoCD, cert-manager, monitoring, alarms, logs, secrets

---

## 📋 STEP-BY-STEP GUIDE

### **Prerequisites**

```bash
# Install required tools
sudo apt update
sudo apt install -y \
  terraform \
  kubectl \
  helm \
  git \
  curl \
  jq

# Verify installations
terraform --version
kubectl version --client
helm version
```

---

## **STEP 1: Fix Terraform Errors**

### Navigate to IaC directory:
```bash
cd ~/team-5/IaC
```

### Quick Fix Method (Two-Stage Apply):
```bash
# Initialize Terraform
terraform init

# Stage 1: Create servers first
terraform plan -target=hcloud_server.node -out=stage1.tfplan
terraform apply stage1.tfplan

# Stage 2: Apply everything else
terraform plan -out=stage2.tfplan
terraform apply stage2.tfplan
```

### Proper Fix Method (Edit Files):

**Fix 1: Edit main.tf line 388**
```bash
nano ~/team-5/IaC/main.tf
```
- Find line 388: `count = length(hcloud_server.node)`
- Change to: `count = var.node_count` (or the variable/local you're using)
- See `/workspace/terraform-fix-main.md` for detailed options

**Fix 2: Edit dns-records.tf line 36**
```bash
nano ~/team-5/IaC/dns-records.tf
```
- Find the `locals` block that defines `dns_records`
- Change to use static keys (see `/workspace/terraform-fix-dns-records.md`)

**After editing, apply:**
```bash
terraform init
terraform plan
terraform apply
```

---

## **STEP 2: Verify K3s Cluster Structure**

### Check that you have the correct node setup:
```bash
# Get kubeconfig from Terraform output
terraform output -raw kubeconfig > ~/.kube/config-k3s

# Set kubectl to use this config
export KUBECONFIG=~/.kube/config-k3s

# Verify nodes
kubectl get nodes -o wide

# Expected output:
# NAME              STATUS   ROLES                  AGE   VERSION
# control-plane-1   Ready    control-plane,master   1h    v1.27.x
# control-plane-2   Ready    control-plane,master   1h    v1.27.x
# control-plane-3   Ready    control-plane,master   1h    v1.27.x
# worker-1          Ready    <none>                 1h    v1.27.x
```

### Verify NoSchedule taint on control plane:
```bash
kubectl describe nodes | grep -A 5 "Taints"

# Expected output should show:
# Taints: node-role.kubernetes.io/control-plane:NoSchedule
```

---

## **STEP 3: Verify ArgoCD Installation**

```bash
# Check if ArgoCD is installed
kubectl get namespace argocd

# Get ArgoCD pods
kubectl get pods -n argocd

# Get ArgoCD admin password
kubectl -n argocd get secret argocd-initial-admin-secret -o jsonpath="{.data.password}" | base64 -d
echo

# Port forward to access ArgoCD UI
kubectl port-forward svc/argocd-server -n argocd 8080:443

# Access in browser: https://localhost:8080
# Username: admin
# Password: (from command above)
```

### Check ArgoCD Applications:
```bash
# List all applications
kubectl get applications -n argocd

# Check application status
kubectl describe application <app-name> -n argocd
```

---

## **STEP 4: Verify cert-manager with Wildcard Certificates**

```bash
# Check cert-manager installation
kubectl get pods -n cert-manager

# Expected: cert-manager, cert-manager-cainjector, cert-manager-webhook

# Check ClusterIssuer for LetsEncrypt
kubectl get clusterissuer

# Check certificates
kubectl get certificates --all-namespaces

# Verify wildcard certificate
kubectl describe certificate -n <namespace> | grep "DNS Names"
# Should show: *.yourdomain.com
```

### Test certificate issuance:
```bash
# Check certificate status
kubectl get certificate -A

# Check certificate details
kubectl describe certificate <cert-name> -n <namespace>

# Should show:
# Status: True
# Type: Ready
```

---

## **STEP 5: Verify Monitoring Stack**

### Check Prometheus:
```bash
# Get Prometheus pods
kubectl get pods -n monitoring | grep prometheus

# Port forward to Prometheus
kubectl port-forward -n monitoring svc/prometheus-k8s 9090:9090

# Access: http://localhost:9090
```

### Check Grafana:
```bash
# Get Grafana pods
kubectl get pods -n monitoring | grep grafana

# Get Grafana password
kubectl get secret -n monitoring grafana-admin -o jsonpath="{.data.admin-password}" | base64 -d
echo

# Port forward to Grafana
kubectl port-forward -n monitoring svc/grafana 3000:3000

# Access: http://localhost:3000
```

### Verify metrics are being collected:
```bash
# Test Prometheus query
curl -s "http://localhost:9090/api/v1/query?query=up" | jq
```

---

## **STEP 6: Verify Alerting Configuration**

```bash
# Check AlertManager
kubectl get pods -n monitoring | grep alertmanager

# Get AlertManager configuration
kubectl get secret -n monitoring alertmanager-main -o json | jq

# Check PrometheusRules
kubectl get prometheusrules -A

# Describe a rule to see alert conditions
kubectl describe prometheusrule <rule-name> -n monitoring
```

### Test alert firing:
```bash
# Port forward to AlertManager
kubectl port-forward -n monitoring svc/alertmanager-main 9093:9093

# Access: http://localhost:9093
# Check if alerts are visible here
```

---

## **STEP 7: Verify Log Collection**

### Check logging stack (Loki/ELK):
```bash
# If using Loki:
kubectl get pods -n logging | grep loki

# Check log aggregation
kubectl get pods -n logging

# Port forward to Loki
kubectl port-forward -n logging svc/loki 3100:3100
```

### Verify logs are being collected:
```bash
# Query Loki for logs
curl -G -s "http://localhost:3100/loki/api/v1/query" \
  --data-urlencode 'query={namespace="default"}' | jq

# Or check in Grafana:
# Grafana → Explore → Select Loki as datasource → Run query
```

---

## **STEP 8: Verify Secrets Management**

### Check Sealed Secrets (if used):
```bash
# Check sealed-secrets controller
kubectl get pods -n kube-system | grep sealed-secrets

# List sealed secrets
kubectl get sealedsecrets -A
```

### Check External Secrets (if used):
```bash
# Check external-secrets operator
kubectl get pods -n external-secrets-system

# List external secrets
kubectl get externalsecrets -A
```

### Verify secrets are properly synced:
```bash
# Check if secrets exist
kubectl get secrets -A | grep -v "kubernetes.io/service-account-token"

# Describe a secret (without revealing content)
kubectl describe secret <secret-name> -n <namespace>
```

---

## **STEP 9: Test Application Deployment**

### Deploy your application via ArgoCD:
```bash
# Create ArgoCD application manifest
cat <<EOF | kubectl apply -f -
apiVersion: argoproj.io/v1alpha1
kind: Application
metadata:
  name: team-app
  namespace: argocd
spec:
  project: default
  source:
    repoURL: https://gitlab.com/team-59632738/team-5.git
    targetRevision: HEAD
    path: k8s-manifests  # or your manifest path
  destination:
    server: https://kubernetes.default.svc
    namespace: default
  syncPolicy:
    automated:
      prune: true
      selfHeal: true
EOF
```

### Verify deployment:
```bash
# Check ArgoCD application status
kubectl get application team-app -n argocd

# Sync the application
kubectl argocd app sync team-app -n argocd

# Check pods
kubectl get pods -n default

# Check ingress
kubectl get ingress -A
```

---

## **STEP 10: Verify Ingress with TLS**

```bash
# Check ingress resources
kubectl get ingress -A

# Verify certificate is attached
kubectl describe ingress <ingress-name> -n <namespace> | grep -A 10 "TLS"

# Test HTTPS endpoint
curl -k https://your-app.yourdomain.com

# Verify certificate
openssl s_client -connect your-app.yourdomain.com:443 -servername your-app.yourdomain.com < /dev/null 2>&1 | openssl x509 -noout -text | grep -A 2 "Subject Alternative Name"
```

---

## **STEP 11: Run CI/CD Pipeline Tests**

### Test GitLab CI/CD:
```bash
# Make a test commit
cd ~/team-5
git checkout -b test-pipeline
echo "test" > test-file.txt
git add test-file.txt
git commit -m "Test CI/CD pipeline"
git push origin test-pipeline
```

### Monitor pipeline:
- Go to GitLab: https://gitlab.com/team-59632738/team-5/-/pipelines
- Verify all stages run:
  1. ✅ pre-build
  2. ✅ check
  3. ✅ build
  4. ✅ test
  5. ✅ deploy
  6. ✅ teardown (manual)

---

## **STEP 12: Comprehensive Health Check**

### Run this complete health check:
```bash
#!/bin/bash
echo "=== K3s Cluster Health Check ==="

echo -e "\n1. Nodes:"
kubectl get nodes

echo -e "\n2. Control Plane Taints:"
kubectl describe nodes | grep -A 2 "Taints" | grep control-plane

echo -e "\n3. ArgoCD Status:"
kubectl get pods -n argocd

echo -e "\n4. cert-manager Status:"
kubectl get pods -n cert-manager

echo -e "\n5. Certificates:"
kubectl get certificates -A

echo -e "\n6. Monitoring Status:"
kubectl get pods -n monitoring

echo -e "\n7. Alerting Rules:"
kubectl get prometheusrules -A

echo -e "\n8. Logging Status:"
kubectl get pods -n logging

echo -e "\n9. Secrets Management:"
kubectl get sealedsecrets -A || kubectl get externalsecrets -A

echo -e "\n10. Applications:"
kubectl get applications -n argocd

echo -e "\n11. Ingresses:"
kubectl get ingress -A

echo -e "\n=== Health Check Complete ==="
```

Save this as `health-check.sh` and run:
```bash
chmod +x health-check.sh
./health-check.sh
```

---

## **🐛 TROUBLESHOOTING**

### Terraform Issues:
```bash
# Reset Terraform state
cd ~/team-5/IaC
rm -rf .terraform .terraform.lock.hcl
terraform init

# Check for syntax errors
terraform validate

# See what will change
terraform plan
```

### K3s Node Issues:
```bash
# Check node logs
kubectl logs -n kube-system <node-problem-detector-pod>

# Restart kubelet (SSH to node)
systemctl restart k3s

# Check node conditions
kubectl describe node <node-name> | grep -A 10 "Conditions"
```

### ArgoCD Sync Issues:
```bash
# Force refresh
kubectl argocd app get <app-name>

# Hard refresh
kubectl argocd app sync <app-name> --force

# Check sync status
kubectl describe application <app-name> -n argocd
```

### Certificate Issues:
```bash
# Check cert-manager logs
kubectl logs -n cert-manager deployment/cert-manager

# Delete and recreate certificate
kubectl delete certificate <cert-name> -n <namespace>
# It will be recreated automatically

# Check ACME challenge
kubectl get challenges -A
kubectl describe challenge <challenge-name> -n <namespace>
```

### Monitoring/Alerting Issues:
```bash
# Check Prometheus targets
kubectl port-forward -n monitoring svc/prometheus-k8s 9090:9090
# Visit: http://localhost:9090/targets

# Reload Prometheus config
kubectl delete pod -n monitoring -l app.kubernetes.io/name=prometheus
```

---

## **📊 VERIFICATION CHECKLIST**

- [ ] 3 control plane nodes + 1 worker node running
- [ ] Control plane has NoSchedule taint
- [ ] Terraform applies successfully
- [ ] ArgoCD deployed and accessible
- [ ] Applications deployed via ArgoCD
- [ ] cert-manager installed
- [ ] Wildcard certificate from LetsEncrypt working
- [ ] Ingress using TLS certificates
- [ ] Prometheus collecting metrics
- [ ] Grafana dashboards working
- [ ] AlertManager configured
- [ ] Alerts trigger somewhere (Slack/email/etc)
- [ ] Logs being collected (Loki/ELK)
- [ ] Logs queryable via Grafana
- [ ] Secrets management working (Sealed Secrets/Vault)
- [ ] CI/CD pipeline runs on commit
- [ ] Application auto-deploys via GitOps

---

## **🎯 EXPECTED OUTCOMES**

After following this guide, you should have:

1. **Infrastructure**: 4-node K3s cluster on Hetzner Cloud
2. **GitOps**: All applications deployed via ArgoCD
3. **Security**: TLS wildcard certificates, secrets encrypted
4. **Observability**: Full monitoring, alerting, and logging stack
5. **Automation**: Complete CI/CD pipeline with IaC + CMT
6. **Resilience**: HA control plane, automated backups

---

## **📚 USEFUL COMMANDS**

```bash
# Quick status check
kubectl get all -A

# Resource usage
kubectl top nodes
kubectl top pods -A

# Events (debugging)
kubectl get events -A --sort-by='.lastTimestamp'

# Logs from all pods in namespace
kubectl logs -n <namespace> -l app=<app-name> --tail=100 -f

# Shell into pod
kubectl exec -it <pod-name> -n <namespace> -- /bin/bash

# Copy files from pod
kubectl cp <namespace>/<pod-name>:/path/to/file ./local-file
```

---

## **🔗 DOCUMENTATION LINKS**

- K3s: https://docs.k3s.io/
- ArgoCD: https://argo-cd.readthedocs.io/
- cert-manager: https://cert-manager.io/docs/
- Prometheus: https://prometheus.io/docs/
- Grafana: https://grafana.com/docs/
- Sealed Secrets: https://github.com/bitnami-labs/sealed-secrets
- Terraform Hetzner: https://registry.terraform.io/providers/hetznercloud/hcloud/

Good luck with your DevOps project! 🚀
