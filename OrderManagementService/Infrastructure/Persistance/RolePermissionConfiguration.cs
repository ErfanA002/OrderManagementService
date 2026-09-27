using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagementService.Infrastructure.Identity;

namespace OrderManagementService.Infrastructure.Persistance;

public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    private static readonly Guid AdminRoleId =
        new("8A5B5D7E-7D3D-4C5A-9B21-123456789001");

    private static readonly Guid UserRoleId =
        new("8A5B5D7E-7D3D-4C5A-9B21-123456789002");

    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.HasKey(x => new
        {
            x.RoleId,
            x.Permission
        });

        builder.HasOne(x => x.Role)
            .WithMany()
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData(
            new RolePermission
            {
                RoleId = AdminRoleId,
                Permission = Permission.CreateOrder
            },
            new RolePermission
            {
                RoleId = AdminRoleId,
                Permission = Permission.OrderDetail
            },
            new RolePermission
            {
                RoleId = AdminRoleId,
                Permission = Permission.SearchOrder
            },
            new RolePermission
            {
                RoleId = AdminRoleId,
                Permission = Permission.OrderChangeStatus
            },
            new RolePermission
            {
                RoleId = AdminRoleId,
                Permission = Permission.OrderBulkInsert
            },
            new RolePermission
            {
                RoleId = AdminRoleId,
                Permission = Permission.OrderDelete
            },

            new RolePermission
            {
                RoleId = UserRoleId,
                Permission = Permission.OrderDetail
            },
            new RolePermission
            {
                RoleId = UserRoleId,
                Permission = Permission.SearchOrder
            },
            new RolePermission
            {
                RoleId = UserRoleId,
                Permission = Permission.CreateOrder
            }
        );
    }
}