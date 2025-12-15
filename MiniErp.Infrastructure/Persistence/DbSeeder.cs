using MiniErp.Domain.Entities;

namespace MiniErp.Infrastructure.Persistence;

public static class DbSeeder
{
    public static void Seed(AppDbContext context)
    {
        if (context.Roles.Any())
            return;

        // ROLES
        var adminRole = new Role("Admin");
        var sellerRole = new Role("Seller");
        var viewerRole = new Role("Viewer");

        context.Roles.AddRange(adminRole, sellerRole, viewerRole);

        // PERMISSIONS (simples)
        var permissions = new List<Permission>
        {
            new Permission("users.read"),
            new Permission("users.write"),
            new Permission("products.read"),
            new Permission("products.write"),
            new Permission("sales.read"),
            new Permission("sales.write")
        };

        context.Permissions.AddRange(permissions);

        context.SaveChanges();
    }
}
