#!/bin/bash

# Complete Terraform Setup Script for Team-5 Project
# This script helps you set up all required variables and run Terraform

set -e

echo "================================================"
echo "  Team-5 K3s Cluster - Terraform Setup"
echo "================================================"
echo ""

cd ~/team-5/IaC

# Check if terraform.tfvars exists
if [ -f "terraform.tfvars" ]; then
    echo "⚠️  terraform.tfvars already exists."
    read -p "Do you want to recreate it? (yes/no): " recreate
    if [ "$recreate" != "yes" ]; then
        echo "Skipping variable setup..."
        echo ""
    else
        rm terraform.tfvars
    fi
fi

if [ ! -f "terraform.tfvars" ]; then
    echo "📝 Setting up terraform.tfvars..."
    echo ""
    
    # Get Hetzner API Token
    echo "1️⃣  Hetzner Cloud API Token"
    echo "   Get from: https://console.hetzner.cloud/ → Security → API Tokens"
    echo ""
    read -p "   Enter your Hetzner API token: " hcloud_token
    echo ""
    
    # Get SSH Public Key
    echo "2️⃣  SSH Public Key"
    if [ -f ~/.ssh/id_rsa.pub ]; then
        echo "   Found existing SSH key: ~/.ssh/id_rsa.pub"
        ssh_key=$(cat ~/.ssh/id_rsa.pub)
        echo "   Key: ${ssh_key:0:50}..."
        read -p "   Use this key? (yes/no): " use_key
        if [ "$use_key" != "yes" ]; then
            echo ""
            echo "   Please create an SSH key first:"
            echo "   ssh-keygen -t rsa -b 4096"
            echo "   Then run this script again."
            exit 1
        fi
    else
        echo "   ⚠️  No SSH key found at ~/.ssh/id_rsa.pub"
        read -p "   Create one now? (yes/no): " create_key
        if [ "$create_key" = "yes" ]; then
            ssh-keygen -t rsa -b 4096 -N "" -f ~/.ssh/id_rsa
            ssh_key=$(cat ~/.ssh/id_rsa.pub)
            echo "   ✅ SSH key created!"
        else
            echo "   Please create an SSH key manually and run this script again."
            exit 1
        fi
    fi
    echo ""
    
    # Get Technitium DNS Token (optional)
    echo "3️⃣  Technitium DNS API Token"
    echo "   This is for DNS management. If you don't have it, try pressing Enter."
    echo ""
    read -p "   Enter DNS token (or press Enter to skip): " dns_token
    if [ -z "$dns_token" ]; then
        dns_token="optional-token-not-set"
    fi
    echo ""
    
    # Create terraform.tfvars
    echo "📄 Creating terraform.tfvars..."
    cat > terraform.tfvars << EOF
# Terraform Variables for Team-5 K3s Cluster
# Created: $(date)

# Hetzner Cloud API Token
hcloud_token = "$hcloud_token"

# SSH Public Key for server access
ssh_public_key = "$ssh_key"

# Technitium DNS API Token (for DNS management)
technitium_api_token = "$dns_token"

# Optional: Uncomment and modify if needed
# node_count = 4
# server_type = "cx21"
# location = "fsn1"
EOF
    
    echo "✅ terraform.tfvars created!"
    echo ""
    
    # Make sure it's in .gitignore
    if ! grep -q "terraform.tfvars" .gitignore 2>/dev/null; then
        echo "terraform.tfvars" >> .gitignore
        echo "*.tfvars" >> .gitignore
        echo "🔒 Added terraform.tfvars to .gitignore"
    fi
    echo ""
fi

# Show current directory and files
echo "================================================"
echo "📂 Current Setup:"
echo "   Directory: $(pwd)"
echo "   Variables file: terraform.tfvars $([ -f terraform.tfvars ] && echo '✅' || echo '❌')"
echo "================================================"
echo ""

# Ask if user wants to run terraform now
read -p "🚀 Ready to run Terraform? (yes/no): " run_terraform

if [ "$run_terraform" = "yes" ]; then
    echo ""
    echo "================================================"
    echo "  Running Terraform"
    echo "================================================"
    echo ""
    
    # Initialize Terraform
    echo "1️⃣  Initializing Terraform..."
    terraform init
    echo ""
    
    # Run plan
    echo "2️⃣  Creating execution plan..."
    terraform plan
    echo ""
    
    # Ask for confirmation
    read -p "Apply this plan? (yes/no): " apply_confirm
    if [ "$apply_confirm" = "yes" ]; then
        echo ""
        echo "3️⃣  Applying configuration..."
        terraform apply -auto-approve
        
        if [ $? -eq 0 ]; then
            echo ""
            echo "================================================"
            echo "✅ Terraform Apply Successful!"
            echo "================================================"
            echo ""
            
            # Get kubeconfig
            echo "📦 Getting kubeconfig..."
            mkdir -p ~/.kube
            terraform output -raw kubeconfig > ~/.kube/config-k3s 2>/dev/null || echo "⚠️  Kubeconfig not available yet (may need time for cluster setup)"
            
            if [ -f ~/.kube/config-k3s ]; then
                export KUBECONFIG=~/.kube/config-k3s
                echo "✅ Kubeconfig saved to ~/.kube/config-k3s"
                echo ""
                echo "📋 Next steps:"
                echo "   export KUBECONFIG=~/.kube/config-k3s"
                echo "   kubectl get nodes"
                echo ""
            fi
            
            echo "🎉 Setup complete! Follow /workspace/DEVOPS-TESTING-GUIDE.md for testing."
        else
            echo ""
            echo "❌ Terraform apply failed. Check errors above."
        fi
    else
        echo "Apply cancelled."
    fi
else
    echo ""
    echo "📋 To run Terraform manually:"
    echo "   cd ~/team-5/IaC"
    echo "   terraform init"
    echo "   terraform plan"
    echo "   terraform apply"
    echo ""
fi

echo ""
echo "Done! 🚀"
