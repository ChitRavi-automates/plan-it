# Setting Up Terraform Variables

## Problem
Terraform keeps prompting for these variables:
- `hcloud_token` - Hetzner Cloud API token
- `ssh_public_key` - Your SSH public key
- `technitium_api_token` - DNS API token

## Solution: Create terraform.tfvars File

### Step 1: Create terraform.tfvars
```bash
cd ~/team-5/IaC

# Create the file
nano terraform.tfvars
```

### Step 2: Add Your Variables

```hcl
# Hetzner Cloud API Token
# Get from: https://console.hetzner.cloud/ → Security → API Tokens
hcloud_token = "your-hetzner-api-token-here"

# Your SSH Public Key
# Get from: cat ~/.ssh/id_rsa.pub
ssh_public_key = "ssh-rsa AAAAB3NzaC1yc2EAAAADAQABAAAB... your-key-here"

# Technitium DNS API Token (if you're using Technitium for DNS)
# This might be optional depending on your setup
technitium_api_token = "your-dns-api-token-here"
```

### Step 3: Get Your Values

#### Get Hetzner API Token:
1. Go to https://console.hetzner.cloud/
2. Select your project
3. Go to **Security** → **API Tokens**
4. Click "Generate API Token"
5. Name: `terraform-team5`
6. Permissions: **Read & Write**
7. Copy the token (you only see it once!)

#### Get Your SSH Public Key:
```bash
# If you have an SSH key
cat ~/.ssh/id_rsa.pub

# If you don't have one, create it
ssh-keygen -t rsa -b 4096 -C "your.email@example.com"
# Press Enter for all prompts (default location, no passphrase)

# Then get the public key
cat ~/.ssh/id_rsa.pub
```

#### Technitium DNS Token:
- If you're using Technitium DNS for DNS management, get the token from your Technitium setup
- If you're NOT using it, you might need to check your IaC code to see if it's optional

---

## Alternative: Use Environment Variables

Instead of terraform.tfvars, you can export environment variables:

```bash
# Set environment variables
export TF_VAR_hcloud_token="your-hetzner-token"
export TF_VAR_ssh_public_key="$(cat ~/.ssh/id_rsa.pub)"
export TF_VAR_technitium_api_token="your-dns-token"

# Now run terraform
terraform plan
terraform apply
```

To make these permanent, add to ~/.bashrc:
```bash
echo 'export TF_VAR_hcloud_token="your-token"' >> ~/.bashrc
echo 'export TF_VAR_ssh_public_key="$(cat ~/.ssh/id_rsa.pub)"' >> ~/.bashrc
source ~/.bashrc
```

---

## Security Note

**IMPORTANT**: Never commit terraform.tfvars to Git!

Make sure it's in .gitignore:
```bash
cd ~/team-5/IaC
echo "terraform.tfvars" >> .gitignore
echo "*.tfvars" >> .gitignore
```

---

## Example terraform.tfvars

```hcl
# terraform.tfvars
hcloud_token = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"

ssh_public_key = "ssh-rsa AAAAB3NzaC1yc2EAAAADAQABAAABgQC8example...rest-of-key user@laptop"

# Optional - only if using Technitium DNS
technitium_api_token = "your-token-here"

# You might also have these variables:
# node_count = 4
# server_type = "cx21"
# location = "fsn1"
```

Save the file and Terraform will automatically use these values!
