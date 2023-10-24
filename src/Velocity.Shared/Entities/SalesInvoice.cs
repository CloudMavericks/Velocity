using Velocity.Shared.Contracts;

namespace Velocity.Shared.Entities;

public class SalesInvoice : IEntity<Guid>
{
    public Guid Id { get; set; }
    
    public string InvoiceNumber { get; set; }
    public DateTime InvoiceDate { get; set; }
    
    public ICollection<SalesInvoiceItem> Items { get; set; } 
    
    public decimal SubTotal => Items.Sum(x => x.TotalPrice);
    
    public decimal Discount => Items.Sum(x => x.Discount);
    
    public decimal NetTotal => Items.Sum(x => x.NetPrice);
    
    public decimal Tax => Items.Sum(x => x.TaxAmount);
    
    public decimal Total => Items.Sum(x => x.Total);
    
    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; }
}

public class SalesInvoiceItem : IEntity<Guid>
{
    public Guid Id { get; set; }
    
    public Guid SalesInvoiceId { get; set; }
    public SalesInvoice SalesInvoice { get; set; }
    
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
    
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