# Fix SSH Key Uniqueness Error (409 Conflict)

## The Problem
The SSH key already exists in Hetzner Cloud from a previous Terraform run, but it's not in your current Terraform state file.

## Error Message
```
Error: API request failed
SSH key not unique
Error code: uniqueness_error
Status code: 409
```

---

## Solution Options

### ⭐ Option 1: Import Existing SSH Key (RECOMMENDED)

This tells Terraform to use the existing key instead of creating a new one.

#### Step 1: Get the SSH Key ID from Hetzner
```bash
# You need your Hetzner API token
export HCLOUD_TOKEN="your-hetzner-api-token"

# List all SSH keys
curl -H "Authorization: Bearer $HCLOUD_TOKEN" \
  https://api.hetzner.cloud/v1/ssh_keys | jq '.ssh_keys[] | {id, name, fingerprint}'
```

OR use hcloud CLI:
```bash
# Install hcloud CLI if not installed
wget https://github.com/hetznercloud/cli/releases/download/v1.42.0/hcloud-linux-amd64.tar.gz
tar xvzf hcloud-linux-amd64.tar.gz
sudo mv hcloud /usr/local/bin/
rm hcloud-linux-amd64.tar.gz

# Configure with your token
hcloud context create myproject

# List SSH keys
hcloud ssh-key list
```

#### Step 2: Import the SSH Key into Terraform State
```bash
cd ~/team-5/IaC

# Import using the key ID (replace 103535668 with your actual ID)
terraform import hcloud_ssh_key.me 103535668
```

#### Step 3: Verify Import
```bash
terraform state list | grep ssh_key
# Should show: hcloud_ssh_key.me

terraform state show hcloud_ssh_key.me
# Should show the key details
```

#### Step 4: Continue with Terraform Apply
```bash
# Now apply normally
terraform apply -target=hcloud_server.node
```

---

### Option 2: Delete and Recreate (If Import Doesn't Work)

#### Using hcloud CLI:
```bash
# List keys
hcloud ssh-key list

# Delete the key (use the name or ID)
hcloud ssh-key delete <key-name-or-id>

# Or delete all keys (careful!)
hcloud ssh-key list -o noheader -o columns=id | xargs -n1 hcloud ssh-key delete
```

#### Using API:
```bash
# Get key ID
KEY_ID=$(curl -H "Authorization: Bearer $HCLOUD_TOKEN" \
  https://api.hetzner.cloud/v1/ssh_keys | jq -r '.ssh_keys[0].id')

# Delete key
curl -X DELETE -H "Authorization: Bearer $HCLOUD_TOKEN" \
  https://api.hetzner.cloud/v1/ssh_keys/$KEY_ID
```

#### Then rerun Terraform:
```bash
terraform apply -target=hcloud_server.node
```

---

### Option 3: Use Data Source Instead of Resource

If you want to keep using the existing key without managing it in Terraform:

#### Edit main.tf (around line 27):
```bash
nano ~/team-5/IaC/main.tf
```

**Change FROM:**
```hcl
resource "hcloud_ssh_key" "me" {
  name       = "my-ssh-key"
  public_key = file("~/.ssh/id_rsa.pub")
}
```

**Change TO:**
```hcl
data "hcloud_ssh_key" "me" {
  name = "my-ssh-key"  # Use the exact name from Hetzner
}
```

Then update any references from `hcloud_ssh_key.me.id` to `data.hcloud_ssh_key.me.id`.

---

## 🎯 RECOMMENDED WORKFLOW

```bash
# 1. Get your Hetzner token (from main.tf or env vars)
cd ~/team-5/IaC
grep -r "HCLOUD_TOKEN" . || grep -r "hcloud_token" .

# Or check if it's in environment
echo $HCLOUD_TOKEN

# 2. Install hcloud CLI (easier than API)
wget https://github.com/hetznercloud/cli/releases/download/v1.42.0/hcloud-linux-amd64.tar.gz
tar xvzf hcloud-linux-amd64.tar.gz
sudo mv hcloud /usr/local/bin/
rm hcloud-linux-amd64.tar.gz

# 3. Configure hcloud
hcloud context create team5
# Paste your API token when prompted

# 4. List SSH keys to find the ID
hcloud ssh-key list

# 5. Import the key (use ID from step 4)
terraform import hcloud_ssh_key.me <SSH_KEY_ID>

# 6. Verify import worked
terraform state show hcloud_ssh_key.me

# 7. Continue with two-stage apply
terraform apply -target=hcloud_server.node

# If successful, continue
terraform apply
```

---

## 🔍 Finding Your Hetzner API Token

Your token should be in one of these places:

### 1. Environment variable:
```bash
env | grep -i hcloud
env | grep -i hetzner
```

### 2. Terraform variables:
```bash
cd ~/team-5/IaC
cat terraform.tfvars
cat variables.tf
grep -r "hcloud_token" .
```

### 3. GitLab CI/CD variables:
Check your GitLab project: Settings → CI/CD → Variables

### 4. Get a new token:
1. Login to https://console.hetzner.cloud/
2. Select your project
3. Go to Security → API Tokens
4. Generate new token (Read & Write)

---

## 🐛 Troubleshooting

### "Cannot import: resource already in state"
```bash
# Remove from state first
terraform state rm hcloud_ssh_key.me

# Then import again
terraform import hcloud_ssh_key.me <SSH_KEY_ID>
```

### "Cannot find SSH key ID"
```bash
# Use hcloud CLI
hcloud ssh-key list

# Or API with your token
curl -H "Authorization: Bearer $HCLOUD_TOKEN" \
  https://api.hetzner.cloud/v1/ssh_keys | jq '.ssh_keys'
```

### "I don't have the hcloud CLI"
Use the API method:
```bash
export HCLOUD_TOKEN="your-token-here"

# Get all keys
curl -H "Authorization: Bearer $HCLOUD_TOKEN" \
  https://api.hetzner.cloud/v1/ssh_keys

# Delete a specific key
curl -X DELETE -H "Authorization: Bearer $HCLOUD_TOKEN" \
  https://api.hetzner.cloud/v1/ssh_keys/<KEY_ID>
```

---

## ✅ After Fixing SSH Key Error

Once the SSH key is imported or recreated, continue with the two-stage apply:

```bash
# Stage 1: Create servers
terraform apply -target=hcloud_server.node

# Stage 2: Create everything else
terraform apply
```

Then follow the rest of the testing guide!
