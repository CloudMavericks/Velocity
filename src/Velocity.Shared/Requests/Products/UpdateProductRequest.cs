namespace Velocity.Shared.Requests.Products;

public class UpdateProductRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int AlertQuantity { get; set; }
    
    public Guid SupplierId { get; set; }
}