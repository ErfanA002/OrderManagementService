using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OrderManagementService.Infrastructure.Persistance;

public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<Guid>>
{
    private static readonly Guid AdminUserId =
        new("A1B2C3D4-E5F6-4789-ABCD-123456789001");

    private static readonly Guid SampleUserId =
        new("A1B2C3D4-E5F6-4789-ABCD-123456789002");

    private static readonly Guid SuperAdminRoleId =
        new("8A5B5D7E-7D3D-4C5A-9B21-123456789001");

    private static readonly Guid UserRoleId =
        new("8A5B5D7E-7D3D-4C5A-9B21-123456789002");

    public void Configure(EntityTypeBuilder<IdentityUserRole<Guid>> builder)
    {
        builder.HasData(
            new IdentityUserRole<Guid>
            {
                UserId = AdminUserId,
                RoleId = SuperAdminRoleId
            },
            new IdentityUserRole<Guid>
            {
                UserId = SampleUserId,
                RoleId = UserRoleId
            }
        );
    }
}