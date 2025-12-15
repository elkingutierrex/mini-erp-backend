using MiniErp.Domain.Enums;

namespace MiniErp.Domain.Entities;

public class Role
{
    public Guid Id { get; private set; }
    public RoleName Name { get; private set; }

    private readonly List<Permission> _permissions = new();
    public IReadOnlyCollection<Permission> Permissions => _permissions.AsReadOnly();

    protected Role() { }

    public Role(RoleName name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }

    public void AddPermission(Permission permission)
    {
        if (_permissions.Any(p => p.Name == permission.Name))
            return;

        _permissions.Add(permission);
    }
}
