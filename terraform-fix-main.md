# Fix for main.tf (Line 388) - Invalid count argument

## Problem
The error occurs because `count = length(hcloud_server.node)` depends on a resource that doesn't exist yet.

## Solution: Use Variable Instead of length()

### Find line 388 in main.tf:

**Before:**
```hcl
resource "hcloud_server_network" "node" {
  count      = length(hcloud_server.node)
  server_id  = hcloud_server.node[count.index].id
  network_id = hcloud_network.main.id
  # ... other config
}
```

**After (Option 1 - Use variable):**
```hcl
resource "hcloud_server_network" "node" {
  count      = var.node_count  # or whatever variable defines your server count
  server_id  = hcloud_server.node[count.index].id
  network_id = hcloud_network.main.id
  # ... other config
}
```

**After (Option 2 - Use a local value):**

First, define this in your locals block:
```hcl
locals {
  node_count = 4  # 3 control plane + 1 worker
}
```

Then use it:
```hcl
resource "hcloud_server_network" "node" {
  count      = local.node_count
  server_id  = hcloud_server.node[count.index].id
  network_id = hcloud_network.main.id
  # ... other config
}
```

**After (Option 3 - If using for_each in hcloud_server):**

If your `hcloud_server.node` uses `for_each` instead of `count`:
```hcl
resource "hcloud_server_network" "node" {
  for_each   = hcloud_server.node
  server_id  = each.value.id
  network_id = hcloud_network.main.id
  # ... other config
}
```

## Key Point
Use the **same count/for_each pattern** that you used in the `hcloud_server.node` resource.
Never use `length()` on a resource that doesn't exist yet.

## To Check Your Current Setup

Look for `resource "hcloud_server" "node"` in your main.tf and check if it uses:
- `count = var.node_count` → Use Option 1
- `count = 4` or `count = local.node_count` → Use Option 2
- `for_each = var.nodes` → Use Option 3
