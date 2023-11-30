namespace Velocity.Shared.Requests.PurchaseInvoices;

public class PurchaseInvoiceItemRequest
{
    public Guid Id { get; set; }
    
    public Guid ProductId { get; set; }
    public string Product { get; set; }
    
    public int Quantity { get; set; }
    
    public decimal UnitPrice { get; set; }
    
    /// <summary>
    /// Quantity * UnitPrice
    /// </summary>
    public decimal TotalPrice => Quantity * UnitPrice;
    
    public decimal DiscountAmount { get; set; }
    
    /// <summary>
    /// TotalPrice - Discount
    /// </summary>
    public decimal NetPrice => TotalPrice - DiscountAmount;
    
    /// <summary>
    /// TaxAmount = NetPrice * TaxPercentage / 100
    /// </summary>
    public decimal TaxAmount => NetPrice * TaxPercentage / 100;
    
    public decimal TaxPercentage { get; set; }
    
    /// <summary>
    /// NetPrice + TaxAmount
    /// </summary>
    public decimal Total => NetPrice + TaxAmount;
    
    public decimal UnitSellingPrice { get; set; }
    
    public decimal ProfitAmount => UnitSellingPrice - UnitPrice;
}