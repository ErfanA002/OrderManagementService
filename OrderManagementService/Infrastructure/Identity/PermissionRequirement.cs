using Microsoft.AspNetCore.Authorization;

namespace OrderManagementService.Infrastructure.Identity;

public sealed class PermissionRequirement
    : IAuthorizationRequirement
{
    public PermissionRequirement(
        Permission permission)
    {
        Permission = permission;
    }

    public Permission Permission { get; }
}