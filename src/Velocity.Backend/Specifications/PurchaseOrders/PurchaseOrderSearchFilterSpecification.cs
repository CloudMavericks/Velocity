using Velocity.Shared.Entities;

namespace Velocity.Backend.Specifications.PurchaseOrders;

public class PurchaseOrderSearchFilterSpecification : BaseSpecification<PurchaseOrder>
{
    public PurchaseOrderSearchFilterSpecification(string searchString)
    {
        if (string.IsNullOrWhiteSpace(searchString))
        {
            FilterCondition = x => true;
        }
        else
        {
            searchString = searchString.ToLower();
            FilterCondition = x => x.OrderNumber.ToLower().Contains(searchString) 
                                   || x.SupplierReferenceNumber.ToLower().Contains(searchString)
                                   || x.Supplier.Name.ToLower().Contains(searchString);
        }
    }
}