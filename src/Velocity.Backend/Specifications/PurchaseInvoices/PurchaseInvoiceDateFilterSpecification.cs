using Velocity.Shared.Entities;

namespace Velocity.Backend.Specifications.PurchaseInvoices;

public class PurchaseInvoiceDateFilterSpecification : BaseSpecification<PurchaseInvoice>
{
    public PurchaseInvoiceDateFilterSpecification(DateTime dateTime)
    {
        if(dateTime != DateTime.MinValue)
        {
            FilterCondition = pi => pi.InvoiceDate == dateTime;
        }
        else
        {
            FilterCondition = pi => true;
        }
    }
}