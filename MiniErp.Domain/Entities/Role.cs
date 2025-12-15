namespace MiniErp.Domain.Entities;

public class Role
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }

    protected Role() { }

    public Role(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }

    public void Update(string name)
    {
        Name = name;
    }
}
