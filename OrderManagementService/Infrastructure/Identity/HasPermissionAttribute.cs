using Microsoft.AspNetCore.Authorization;

namespace OrderManagementService.Infrastructure.Identity;

[AttributeUsage(
    AttributeTargets.Class | AttributeTargets.Method,
    AllowMultiple = true,
    Inherited = true)]
public sealed class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(Permission permission)
    {
        Policy =
            $"{PermissionPolicyProvider.PolicyPrefix}{permission}";
    }
}