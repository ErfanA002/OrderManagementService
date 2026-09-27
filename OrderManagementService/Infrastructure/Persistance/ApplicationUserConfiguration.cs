using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagementService.Infrastructure.Identity;

namespace OrderManagementService.Infrastructure.Persistance;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    private static readonly Guid AdminUserId =
        new("A1B2C3D4-E5F6-4789-ABCD-123456789001");

    private static readonly Guid SampleUserId =
        new("A1B2C3D4-E5F6-4789-ABCD-123456789002");

    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        var passwordHasher = new PasswordHasher<ApplicationUser>();

        var adminUser = new ApplicationUser
        {
            Id = AdminUserId,
            UserName = "admin",
            NormalizedUserName = "ADMIN",
            Email = "admin@example.com",
            NormalizedEmail = "ADMIN@EXAMPLE.COM",
            EmailConfirmed = true,
            SecurityStamp = "A1B2C3D4-E5F6-4789-ABCD-123456789011",
            ConcurrencyStamp = "A1B2C3D4-E5F6-4789-ABCD-123456789012",
        };

        var sampleUser = new ApplicationUser
        {
            Id = SampleUserId,
            UserName = "sample",
            NormalizedUserName = "SAMPLE",
            Email = "sample@example.com",
            NormalizedEmail = "SAMPLE@EXAMPLE.COM",
            EmailConfirmed = true,
            SecurityStamp = "A1B2C3D4-E5F6-4789-ABCD-123456789021",
            ConcurrencyStamp = "A1B2C3D4-E5F6-4789-ABCD-123456789022",
        };

        adminUser.PasswordHash =
            passwordHasher.HashPassword(adminUser, "Admin@123");

        sampleUser.PasswordHash =
            passwordHasher.HashPassword(sampleUser, "Sample@123");

        builder.HasData(
            adminUser,
            sampleUser
        );
    }
}