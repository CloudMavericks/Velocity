namespace Velocity.Shared.Responses.Products;

public class ProductResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int QuantityOnHand { get; set; }
    public int AlertQuantity { get; set; }
    
    public Guid SupplierId { get; set; }
    
    public string SupplierName { get; set; }

    public override string ToString()
    {
        return $"{Name} - {SupplierName}";
    }
}