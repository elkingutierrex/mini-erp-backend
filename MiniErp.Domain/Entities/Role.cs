using MiniErp.Domain.Enums;

namespace MiniErp.Domain.Entities;

public class Role
{
    public Guid Id { get; set; }
    public RoleName Name { get; set; }
    public List<Permission> Permissions { get; set; } = new();
}
