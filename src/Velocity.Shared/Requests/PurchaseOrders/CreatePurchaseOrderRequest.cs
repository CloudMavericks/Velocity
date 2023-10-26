﻿namespace Velocity.Shared.Requests.PurchaseOrders;

public class CreatePurchaseOrderRequest
{
    /// <summary>
    /// Generated by the system
    /// </summary>
    public string OrderNumber { get; set; }
    
    /// <summary>
    /// Provided by the supplier from the quotation
    /// </summary>
    public string SupplierReferenceNumber { get; set; }
    
    public DateTime OrderDate { get; set; }

    public ICollection<CreatePurchaseOrderItemRequest> Items { get; set; }
    
    public decimal SubTotal => Items.Sum(x => x.TotalPrice);
    
    public decimal TotalDiscountAmount => Items.Sum(x => x.DiscountAmount);
    
    public decimal NetTotal => Items.Sum(x => x.NetPrice);
    
    public decimal TotalTaxAmount => Items.Sum(x => x.TaxAmount);
    
    public decimal Total => Items.Sum(x => x.Total);

    public Guid SupplierId { get; set; }
}

public class CreatePurchaseOrderItemRequest
{
    public Guid ProductId { get; set; }
    
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
}