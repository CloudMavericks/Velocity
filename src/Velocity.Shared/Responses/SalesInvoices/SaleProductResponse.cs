namespace Velocity.Shared.Responses.SalesInvoices;

public class SaleProductResponse
{
    public Guid ProductId { get; set; }
    
    public string Product { get; set; }
    
    public int AvailableQuantity { get; set; }
    
    public decimal UnitSellingPrice { get; set; }
    
    public decimal TaxPercentage { get; set; }
    
    public Guid PurchaseInvoiceItemId { get; set; }
}