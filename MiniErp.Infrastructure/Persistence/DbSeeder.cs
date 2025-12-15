using MiniErp.Domain.Entities;
using MiniErp.Domain.Enums;

namespace MiniErp.Infrastructure.Persistence;

public static class DbSeeder
{
    public static void Seed(AppDbContext context)
    {
        if (context.Users.Any()) return;

        var canCreateSale = new Permission { Name = "CanCreateSale" };
        var canViewAllSales = new Permission { Name = "CanViewAllSales" };
        var canManageRoles = new Permission { Name = "CanManageRoles" };

        var sellerRole = new Role
        {
            Name = RoleName.seller,
            Permissions = new() { canCreateSale }
        };

        var adminRole = new Role
        {
            Name = RoleName.admin,
            Permissions = new() { canViewAllSales }
        };

        var managerRole = new Role
        {
            Name = RoleName.manager,
            Permissions = new() { canCreateSale, canViewAllSales, canManageRoles }
        };

        context.Roles.AddRange(sellerRole, adminRole, managerRole);

        context.Users.AddRange(
            new User { Email = "seller@erp.test", Password = "seller", Role = RoleName.seller },
            new User { Email = "admin@erp.test", Password = "admin", Role = RoleName.admin },
            new User { Email = "manager@erp.test", Password = "manager", Role = RoleName.manager }
        );

        context.SaveChanges();
    }
}
