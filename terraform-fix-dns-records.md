# Fix for dns-records.tf - Invalid for_each argument

## Problem
The error occurs because `local.dns_records` depends on resource attributes that aren't known until after apply.

## Solution Options

### Option A: Two-Stage Apply (Quick Fix)
```bash
cd ~/team-5/IaC
terraform apply -target=hcloud_server.node
terraform apply
```

### Option B: Refactor to Use Static Keys (Proper Fix)

Find your `locals` block that defines `dns_records` and change it to use static keys:

**Before:**
```hcl
locals {
  dns_records = {
    for server in hcloud_server.node : 
      server.name => {
        ip = server.ipv4_address
        name = server.name
      }
  }
}
```

**After:**
```hcl
locals {
  # Define the number of nodes (should match your var.node_count or actual count)
  node_names = [
    "control-plane-1",
    "control-plane-2", 
    "control-plane-3",
    "worker-1"
  ]
  
  # Use static keys with index-based lookup
  dns_records = {
    for idx, name in local.node_names :
      name => {
        # The IP will be determined at apply time (in values, not keys)
        ip   = try(hcloud_server.node[idx].ipv4_address, null)
        name = name
      }
  }
}
```

Or if you're using count/index directly:

```hcl
locals {
  dns_records = {
    for idx in range(4) :  # 3 control plane + 1 worker = 4 total
      "node-${idx}" => {
        ip   = hcloud_server.node[idx].ipv4_address
        name = "node-${idx}"
        type = idx < 3 ? "control-plane" : "worker"
      }
  }
}
```

Then use it:

```hcl
resource "null_resource" "dns_records" {
  for_each = local.dns_records
  
  triggers = {
    ip   = each.value.ip
    name = each.value.name
  }
  
  # Your provisioner or other configuration here
}
```

## Key Point
The **keys** of the for_each map must be statically determinable.
Only the **values** can contain dynamic resource attributes.
