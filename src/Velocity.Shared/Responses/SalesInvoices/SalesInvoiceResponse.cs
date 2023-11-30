namespace Velocity.Shared.Responses.SalesInvoices;

public class SalesInvoiceResponse
{
    public Guid Id { get; set; }
    
    public string InvoiceNumber { get; set; }
    public DateTime InvoiceDate { get; set; }
    
    public IList<SalesInvoiceItemResponse> Items { get; set; } 
    
    public decimal SubTotal => Items.Sum(x => x.TotalPrice);
    
    public decimal Discount => Items.Sum(x => x.Discount);
    
    public decimal NetTotal => Items.Sum(x => x.NetPrice);
    
    public decimal Tax => Items.Sum(x => x.TaxAmount);
    
    public decimal Total => Items.Sum(x => x.Total);
    
    public Guid? CustomerId { get; set; }
    public string Customer { get; set; }
}

public class SalesInvoiceItemResponse
{
    public Guid Id { get; set; }
    
    public Guid ProductId { get; set; }
    public string Product { get; set; }
    
    public Guid PurchaseInvoiceItemId { get; set; }
    public string PurchaseInvoiceItem { get; set; }
    
    public int Quantity { get; set; }
    
    public decimal UnitPrice { get; set; }
    
    /// <summary>
    /// Quantity * UnitPrice
    /// </summary>
    public decimal TotalPrice => Quantity * UnitPrice;
    
    public decimal Discount { get; set; }
    
    /// <summary>
    /// TotalPrice - Discount
    /// </summary>
    public decimal NetPrice => TotalPrice - Discount;
    
    /// <summary>
    /// TaxAmount = NetPrice * TaxPercentage / 100
    /// </summary>
    public decimal TaxAmount => NetPrice * TaxPercentage / 100;
    
    public decimal TaxPercentage { get; set; }
    
    /// <summary>
    /// NetPrice + TaxAmount
    /// </summary>
    public decimal Total => NetPrice + TaxAmount;
}