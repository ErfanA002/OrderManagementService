using Microsoft.AspNetCore.Identity;

namespace OrderManagementService.Infrastructure.Identity;

public class ApplicationRole : IdentityRole<Guid>
{
    public ICollection<RolePermission> Permissions { get; set; }
}