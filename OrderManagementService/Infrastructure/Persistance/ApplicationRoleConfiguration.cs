using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagementService.Infrastructure.Identity;

namespace OrderManagementService.Infrastructure.Persistance;

public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.HasData(
            new ApplicationRole
            {
                Id = new Guid("8A5B5D7E-7D3D-4C5A-9B21-123456789001"),
                Name = "SuperAdmin",
                NormalizedName = "SUPERADMIN",
            },
            new ApplicationRole
            {
                Id = new Guid("8A5B5D7E-7D3D-4C5A-9B21-123456789002"),
                Name = "User",
                NormalizedName = "USER",
            }
        );
    }
}