using Velocity.Shared.Contracts;

namespace Velocity.Shared.Entities;

public class Supplier : IEntity<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string ContactName { get; set; }
    public string ContactEmail { get; set; }
    public string ContactPhone { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public string Gstn { get; set; }

    public ICollection<Product> Products { get; set; }
}
