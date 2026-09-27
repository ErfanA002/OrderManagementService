using Microsoft.AspNetCore.Authorization;

namespace OrderManagementService.Infrastructure.Identity;

public sealed class PermissionAuthorizationHandler
    : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        if (context.User.Identity?.IsAuthenticated != true)
        {
            return Task.CompletedTask;
        }

        if (context.User.IsInRole("SuperAdmin"))
        {
            context.Succeed(requirement);

            return Task.CompletedTask;
        }

        var requiredPermission =
            requirement.Permission.ToString();

        var hasPermission =
            context.User
                .FindAll(PermissionClaimTypes.Permission)
                .Any(x =>
                    x.Value == requiredPermission);

        if (hasPermission)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}