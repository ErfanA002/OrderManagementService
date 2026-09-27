using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OrderManagementService.Domain.Entities;
using OrderManagementService.Infrastructure.Identity;

namespace OrderManagementService.Infrastructure.Persistance;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>(options)
{
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ApplicationDbContext).Assembly);

        modelBuilder.Entity<ApplicationRole>(entity =>
        {
            entity
                .HasMany(x => x.Permissions)
                .WithOne(x => x.Role)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.HasKey(x => new
            {
                x.RoleId,
                x.Permission
            });

            entity.Property(x => x.Permission)
                .HasConversion<string>()
                .HasMaxLength(100);

            entity.HasIndex(x => x.Permission);
        });
    }
}