using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace OrderManagementService.Infrastructure.Identity;

public sealed class PermissionPolicyProvider
    : DefaultAuthorizationPolicyProvider
{
    public const string PolicyPrefix = "Permission:";

    public PermissionPolicyProvider(
        IOptions<AuthorizationOptions> options)
        : base(options)
    {
    }

    public override async Task<AuthorizationPolicy?>
        GetPolicyAsync(string policyName)
    {
        if (!policyName.StartsWith(
                PolicyPrefix,
                StringComparison.Ordinal))
        {
            return await base.GetPolicyAsync(policyName);
        }

        var permissionName =
            policyName[PolicyPrefix.Length..];

        if (!Enum.TryParse<Permission>(
                permissionName,
                ignoreCase: false,
                out var permission))
        {
            return null;
        }

        return new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .AddRequirements(
                new PermissionRequirement(permission))
            .Build();
    }
}