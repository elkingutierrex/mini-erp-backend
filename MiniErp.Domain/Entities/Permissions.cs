using System;
using System.Collections.Generic;

namespace MiniErp.Domain.Entities;

public class Permission
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public string Name { get; private set; }

    public ICollection<Role> Roles { get; private set; } = new List<Role>();

    private Permission() { } // EF Core

    public Permission(string name)
    {
        Name = name;
    }
}
