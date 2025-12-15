namespace MiniErp.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }

    public Guid RoleId { get; private set; }
    public Role Role { get; private set; }

    protected User() { }

    public User(string email, string password, Role role)
    {
        Id = Guid.NewGuid();
        Email = email;
        Password = password;
        Role = role;
        RoleId = role.Id;
    }
}
