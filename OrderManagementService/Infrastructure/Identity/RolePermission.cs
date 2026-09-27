namespace OrderManagementService.Infrastructure.Identity;

public class RolePermission
{
    public Guid RoleId { get; set; }

    public Permission Permission { get; set; }

    public ApplicationRole Role { get; set; } = null!;
}