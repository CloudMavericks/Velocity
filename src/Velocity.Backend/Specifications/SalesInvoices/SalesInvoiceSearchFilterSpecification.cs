using Velocity.Shared.Entities;

namespace Velocity.Backend.Specifications.SalesInvoices;

public class SalesInvoiceSearchFilterSpecification : BaseSpecification<SalesInvoice>
{
    public SalesInvoiceSearchFilterSpecification(string searchString)
    {
        if (string.IsNullOrWhiteSpace(searchString))
        {
            FilterCondition = p => true;
        }
        else
        {
            searchString = searchString.Trim().ToLower();
            FilterCondition = p =>
                p.InvoiceNumber.ToLower().Contains(searchString)
                || p.Customer.Name.ToLower().Contains(searchString);
        }
    }
}