using MiniErp.Domain.Entities;
using MiniErp.Domain.Enums;

namespace MiniErp.Infrastructure.Persistence;

public static class DbSeeder
{
    public static void Seed(AppDbContext context)
    {
        if (context.Users.Any())
            return;

        // --------------------
        // Permissions
        // --------------------
        var canCreateSale = new Permission("CanCreateSale");
        var canViewAllSales = new Permission("CanViewAllSales");
        var canManageRoles = new Permission("CanManageRoles");

        context.Permissions.AddRange(
            canCreateSale,
            canViewAllSales,
            canManageRoles
        );

        // --------------------
        // Roles
        // --------------------
        var sellerRole = new Role(RoleName.seller);
        sellerRole.AddPermission(canCreateSale);

        var adminRole = new Role(RoleName.admin);
        adminRole.AddPermission(canCreateSale);
        adminRole.AddPermission(canViewAllSales);

        var managerRole = new Role(RoleName.manager);
        managerRole.AddPermission(canCreateSale);
        managerRole.AddPermission(canViewAllSales);
        managerRole.AddPermission(canManageRoles);

        context.Roles.AddRange(sellerRole, adminRole, managerRole);

        // --------------------
        // Users
        // --------------------
        var sellerUser = new User(
            "seller@demo.com",
            "seller123",
            sellerRole
        );

        var adminUser = new User(
            "admin@demo.com",
            "admin123",
            adminRole
        );

        var managerUser = new User(
            "manager@demo.com",
            "manager123",
            managerRole
        );

        context.Users.AddRange(
            sellerUser,
            adminUser,
            managerUser
        );

        // --------------------
        // Products
        // --------------------
        var products = new List<Product>
        {
            new Product("Laptop", 3500),
            new Product("Mouse", 80),
            new Product("Keyboard", 150),
            new Product("Monitor", 900),
            new Product("Headphones", 200),
            new Product("USB Cable", 30)
        };

        context.Products.AddRange(products);

        context.SaveChanges();
    }
}
