using Velocity.Shared.Entities;

namespace Velocity.Backend.Specifications.PurchaseOrders;

public class PurchaseOrderSupplierFilterSpecification : BaseSpecification<PurchaseOrder>
{
    public PurchaseOrderSupplierFilterSpecification(Guid? supplierId)
    {
        if(supplierId.HasValue)
        {
            FilterCondition = po => po.SupplierId == supplierId;
        }
        else
        {
            FilterCondition = po => true;
        }
    }
}