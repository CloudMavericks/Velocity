using Velocity.Shared.Entities;

namespace Velocity.Backend.Specifications.PurchaseInvoices;

public class PurchaseInvoiceSearchFilterSpecification : BaseSpecification<PurchaseInvoice>
{
    public PurchaseInvoiceSearchFilterSpecification(string searchString)
    {
        if (string.IsNullOrWhiteSpace(searchString))
        {
            FilterCondition = p => true;
        }
        else
        {
            searchString = searchString.Trim().ToLower();
            FilterCondition = p => p.PurchaseNumber.ToLower().Contains(searchString) ||
                                   p.InvoiceNumber.ToLower().Contains(searchString) ||
                                   p.ReferenceNumber.ToLower().Contains(searchString) ||
                                   p.Supplier.Name.ToLower().Contains(searchString);
        }
    }
}