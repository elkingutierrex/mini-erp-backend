using System;
using System.Collections.Generic;

namespace MiniErp.Domain.Entities;

public class Role
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public string Name { get; private set; }

    public ICollection<Permission> Permissions { get; private set; } = new List<Permission>();

    private Role() { } // EF Core

    public Role(string name)
    {
        Name = name;
    }

    public void SetPermissions(IEnumerable<Permission> permissions)
    {
        Permissions.Clear();

        foreach (var permission in permissions)
        {
            Permissions.Add(permission);
        }
    }
}
