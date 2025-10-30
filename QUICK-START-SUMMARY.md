# 🚀 Quick Start Summary - Terraform Fix & DevOps Testing

## Your Project
**DevOps Group Project**: K3s cluster with full automation
- 3 control plane nodes + 1 worker node
- ArgoCD, cert-manager, monitoring, alerting, logging, secrets management
- Hetzner Cloud infrastructure via Terraform

## The Errors You're Facing

### Error 1: Invalid for_each argument (dns-records.tf:36)
**Cause**: The for_each map uses dynamic keys that depend on resource attributes not known until apply.

### Error 2: Invalid count argument (main.tf:388)  
**Cause**: Using `length(hcloud_server.node)` before the resource exists.

---

## ⚡ QUICKEST FIX (Recommended)

Run the automated script:

```bash
# Copy script from workspace to your system
cp /workspace/quick-fix-terraform.sh ~/team-5/

# Run it
cd ~/team-5
bash quick-fix-terraform.sh
```

This uses a **two-stage Terraform apply**:
1. Stage 1: Creates servers first
2. Stage 2: Creates everything else that depends on servers

---

## 📖 ALTERNATIVE: Manual Fix

### Option A: Two-Stage Apply (No code changes needed)

```bash
cd ~/team-5/IaC

# Initialize
terraform init

# Stage 1: Create servers
terraform plan -target=hcloud_server.node -out=stage1.tfplan
terraform apply stage1.tfplan

# Stage 2: Create everything else  
terraform plan -out=stage2.tfplan
terraform apply stage2.tfplan
```

### Option B: Code Refactoring (Proper fix)

**Fix main.tf line 388:**
```hcl
# Change this:
count = length(hcloud_server.node)

# To this:
count = var.node_count  # or local.node_count, or 4
```

**Fix dns-records.tf line 36:**
```hcl
# Change local.dns_records to use static keys
locals {
  dns_records = {
    for idx in range(4) :  # Static range
      "node-${idx}" => {
        ip = hcloud_server.node[idx].ipv4_address  # Dynamic value OK
      }
  }
}
```

See detailed guides:
- `/workspace/terraform-fix-main.md`
- `/workspace/terraform-fix-dns-records.md`

---

## ✅ After Fixing Terraform

### 1. Get kubeconfig
```bash
cd ~/team-5/IaC
terraform output -raw kubeconfig > ~/.kube/config-k3s
export KUBECONFIG=~/.kube/config-k3s
```

### 2. Verify cluster
```bash
kubectl get nodes
# Should show: 3 control-plane + 1 worker

kubectl get pods -A
# Should show: ArgoCD, cert-manager, monitoring, etc.
```

### 3. Follow complete testing guide
See: `/workspace/DEVOPS-TESTING-GUIDE.md`

This includes verification of:
- ✅ K3s cluster setup
- ✅ ArgoCD installation
- ✅ cert-manager with wildcard certificates
- ✅ Monitoring (Prometheus/Grafana)
- ✅ Alerting configuration
- ✅ Log collection
- ✅ Secrets management
- ✅ CI/CD pipeline
- ✅ Application deployment

---

## 📁 Files Created for You

| File | Purpose |
|------|---------|
| `DEVOPS-TESTING-GUIDE.md` | Complete step-by-step testing guide (all 12 steps) |
| `terraform-fix-main.md` | Detailed fix for main.tf count error |
| `terraform-fix-dns-records.md` | Detailed fix for dns-records.tf for_each error |
| `quick-fix-terraform.sh` | Automated script for two-stage apply |
| `QUICK-START-SUMMARY.md` | This file - quick reference |

---

## 🎯 Recommended Workflow

```bash
# 1. Fix Terraform and create infrastructure
cd ~/team-5/IaC
bash ../quick-fix-terraform.sh

# 2. Get kubeconfig
terraform output -raw kubeconfig > ~/.kube/config-k3s
export KUBECONFIG=~/.kube/config-k3s

# 3. Verify everything
kubectl get nodes
kubectl get pods -A

# 4. Access services
kubectl port-forward -n argocd svc/argocd-server 8080:443 &
kubectl port-forward -n monitoring svc/grafana 3000:3000 &

# 5. Open in browser
# ArgoCD: https://localhost:8080
# Grafana: http://localhost:3000

# 6. Deploy your application via ArgoCD
kubectl apply -f ~/team-5/k8s-manifests/argocd-app.yaml
```

---

## 🆘 If You Get Stuck

### Terraform won't apply?
```bash
cd ~/team-5/IaC
rm -rf .terraform .terraform.lock.hcl
terraform init
terraform validate  # Check for syntax errors
```

### Can't connect to cluster?
```bash
# Re-export kubeconfig
export KUBECONFIG=~/.kube/config-k3s

# Test connection
kubectl cluster-info
```

### Services not working?
```bash
# Check all pods
kubectl get pods -A

# Check failed pods
kubectl get pods -A | grep -v Running

# Debug specific pod
kubectl describe pod <pod-name> -n <namespace>
kubectl logs <pod-name> -n <namespace>
```

---

## 📚 Key Terraform Concepts

### Why these errors happen:

**for_each error**: Terraform needs to know all resource instances *before* it creates anything. When you use resource attributes as keys, Terraform can't determine this.

**count error**: Similar issue - `length()` needs to evaluate a list that doesn't exist yet.

**Solution**: Use static values for `count` and for_each `keys`, put dynamic values in for_each `values`.

### Good Practice:
```hcl
# ✅ Good - static keys
for_each = { 
  node1 = { ip = resource.x.ip }  # Dynamic value OK
  node2 = { ip = resource.y.ip }
}

# ❌ Bad - dynamic keys  
for_each = {
  for x in resource.list :
    x.name => x.id  # x.name is unknown
}
```

---

## 🎓 Understanding Your Project Stack

### Infrastructure Layer (IaC)
- **Terraform**: Creates 4 Hetzner Cloud VMs
- **Ansible/k3s**: Configures K3s cluster (CMT)

### Kubernetes Layer
- **K3s**: Lightweight Kubernetes
- **ArgoCD**: GitOps continuous deployment
- **cert-manager**: Automated TLS certificate management

### Observability Layer  
- **Prometheus**: Metrics collection
- **Grafana**: Visualization dashboards
- **AlertManager**: Alert routing and grouping
- **Loki/ELK**: Log aggregation

### Security Layer
- **Sealed Secrets/Vault**: Secret encryption
- **LetsEncrypt**: Free TLS certificates
- **Network Policies**: Pod-to-pod security

### Application Layer
- **Backend**: .NET Core API
- **Frontend**: React app
- **Database**: SQL Server (optional)

All deployed via **GitOps** (ArgoCD watches Git repo and auto-deploys).

---

## 🏆 Success Criteria

Your project is complete when:
- [ ] `terraform apply` succeeds without errors
- [ ] `kubectl get nodes` shows 3 control-plane + 1 worker
- [ ] Control plane nodes have NoSchedule taint
- [ ] ArgoCD is accessible and syncing applications
- [ ] Wildcard TLS certificate is issued and valid
- [ ] Prometheus is collecting metrics
- [ ] Grafana dashboards show data
- [ ] Alerts are configured and can trigger
- [ ] Logs are being collected and queryable
- [ ] Secrets are encrypted at rest
- [ ] Git push triggers CI/CD pipeline
- [ ] Application auto-deploys via ArgoCD
- [ ] Application is accessible via HTTPS with valid cert

---

## 💡 Pro Tips

1. **Always use two-stage apply for complex dependencies**
2. **Keep Terraform state backed up** (use remote state)
3. **Test in a separate branch** before applying to main
4. **Monitor resource costs** in Hetzner console
5. **Document your changes** in Git commits
6. **Use GitOps** - don't `kubectl apply` manually
7. **Check ArgoCD UI** for sync status regularly
8. **Set up Slack/email** for alerts

---

## 📞 Next Steps

1. ✅ Run `quick-fix-terraform.sh` to fix the errors
2. ✅ Verify cluster with `kubectl get nodes`
3. ✅ Follow DEVOPS-TESTING-GUIDE.md for complete testing
4. ✅ Deploy your application via ArgoCD
5. ✅ Set up monitoring dashboards
6. ✅ Configure alerting rules
7. ✅ Test the complete CI/CD flow
8. ✅ Document your setup for the course

Good luck with your DevOps project! 🚀

---

**Questions?** Re-read the guides or check Terraform/K3s documentation.
