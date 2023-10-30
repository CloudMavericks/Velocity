namespace Velocity.Shared.Requests.PurchaseOrders;

public class GetPurchaseOrdersRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string SearchString { get; set; } = "";
    public string OrderNumber { get; set; }
    public DateTime? OrderDate { get; set; }
    public Guid? SupplierId { get; set; }
}