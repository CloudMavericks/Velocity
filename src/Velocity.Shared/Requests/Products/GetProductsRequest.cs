namespace Velocity.Shared.Requests.Products;

public class GetProductsRequest
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string SearchString { get; set; }
    public Guid? SupplierId { get; set; }
}