#!/bin/bash

# Quick Fix Script for Terraform Errors
# This script uses the two-stage apply method to work around for_each and count errors

set -e  # Exit on error

echo "====================================="
echo "Terraform Quick Fix - Two-Stage Apply"
echo "====================================="
echo ""

# Check if IaC directory exists
if [ ! -d "$HOME/team-5/IaC" ]; then
    echo "❌ Error: IaC directory not found at $HOME/team-5/IaC"
    echo "Please clone the repository first:"
    echo "  cd ~"
    echo "  git clone https://gitlab.com/team-59632738/team-5.git"
    exit 1
fi

cd "$HOME/team-5/IaC"

echo "📂 Current directory: $(pwd)"
echo ""

# Initialize Terraform
echo "🔧 Step 1: Initializing Terraform..."
terraform init
echo "✅ Terraform initialized"
echo ""

# Stage 1: Create servers first
echo "🚀 Step 2: Creating servers (Stage 1)..."
echo "This will create the hcloud_server.node resources first."
echo ""

terraform plan -target=hcloud_server.node -out=stage1.tfplan

read -p "Apply Stage 1? (yes/no): " confirm
if [ "$confirm" != "yes" ]; then
    echo "❌ Aborted by user"
    rm -f stage1.tfplan
    exit 1
fi

terraform apply stage1.tfplan
echo "✅ Stage 1 complete - Servers created"
echo ""

# Stage 2: Apply everything else
echo "🚀 Step 3: Applying remaining resources (Stage 2)..."
echo "This will create DNS records, networks, and all other resources."
echo ""

terraform plan -out=stage2.tfplan

read -p "Apply Stage 2? (yes/no): " confirm
if [ "$confirm" != "yes" ]; then
    echo "❌ Aborted by user"
    rm -f stage2.tfplan
    exit 1
fi

terraform apply stage2.tfplan
echo "✅ Stage 2 complete - All resources created"
echo ""

# Cleanup
rm -f stage1.tfplan stage2.tfplan

echo "====================================="
echo "✅ Terraform Apply Complete!"
echo "====================================="
echo ""
echo "Next steps:"
echo "1. Export kubeconfig:"
echo "   terraform output -raw kubeconfig > ~/.kube/config-k3s"
echo "   export KUBECONFIG=~/.kube/config-k3s"
echo ""
echo "2. Verify nodes:"
echo "   kubectl get nodes"
echo ""
echo "3. Check services:"
echo "   kubectl get pods -A"
echo ""
echo "4. Follow the full guide in /workspace/DEVOPS-TESTING-GUIDE.md"
echo ""
