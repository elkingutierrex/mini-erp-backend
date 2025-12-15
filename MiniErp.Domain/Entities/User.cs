using MiniErp.Domain.Enums;

namespace MiniErp.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public RoleName Role { get; set; }
}
