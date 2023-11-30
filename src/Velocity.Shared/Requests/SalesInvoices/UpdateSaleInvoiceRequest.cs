namespace Velocity.Shared.Requests.SalesInvoices;

public class UpdateSaleInvoiceRequest
{
    public Guid Id { get; set; }
    
    public string InvoiceNumber { get; set; }
    public DateTime? InvoiceDate { get; set; }
    
    public IList<SalesInvoiceItemRequest> Items { get; set; } = new List<SalesInvoiceItemRequest>(); 
    
    public decimal SubTotal => Items.Sum(x => x.TotalPrice);
    
    public decimal Discount => Items.Sum(x => x.Discount);
    
    public decimal NetTotal => Items.Sum(x => x.NetPrice);
    
    public decimal Tax => Items.Sum(x => x.TaxAmount);
    
    public decimal Total => Items.Sum(x => x.Total);
    
    public Guid? CustomerId { get; set; }
    public string Customer { get; set; }
}