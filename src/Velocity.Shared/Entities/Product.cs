using Velocity.Shared.Contracts;

namespace Velocity.Shared.Entities;

public class Product : IEntity<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int QuantityOnHand { get; set; }
    public int AlertQuantity { get; set; }
    
    public Guid SupplierId { get; set; }
    public Supplier Supplier { get; set; }
}
