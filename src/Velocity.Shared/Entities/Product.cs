using Velocity.Shared.Contracts;

namespace Velocity.Shared.Entities;

public class Product : IEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Code { get; set; }
}
