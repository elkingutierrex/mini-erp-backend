using MiniErp.Domain.Entities;

namespace MiniErp.Infrastructure.Persistence;

public static class DbSeeder
{
    public static void Seed(AppDbContext context)
    {
        if (context.Users.Any())
            return;

        // =======================
        // ROLES
        // =======================

        var adminRole = new Role("admin");
        var sellerRole = new Role("seller");
        var managerRole = new Role("manager");

        context.Roles.AddRange(adminRole, sellerRole, managerRole);

        // =======================
        // PERMISSIONS
        // =======================

        var permissions = new List<Permission>
        {
            new Permission("users.read"),
            new Permission("users.write"),
            new Permission("products.read"),
            new Permission("products.write"),
            new Permission("sales.read"),
            new Permission("sales.write"),
            new Permission("roles.manage")
        };

        context.Permissions.AddRange(permissions);

        context.SaveChanges();

        // =======================
        // USERS
        // =======================

        var users = new List<User>
        {
            new User("seller1@erp.test", "123", sellerRole),
            new User("seller2@erp.test", "123", sellerRole),
            new User("seller3@erp.test", "123", sellerRole),

            new User("admin@erp.test", "123", adminRole),
            new User("manager@erp.test", "123", managerRole)
        };

        context.Users.AddRange(users);

        context.SaveChanges();
    }
}
