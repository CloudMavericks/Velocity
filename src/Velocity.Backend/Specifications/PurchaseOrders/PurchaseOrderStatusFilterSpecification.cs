using Velocity.Shared.Entities;
using Velocity.Shared.Enums;

namespace Velocity.Backend.Specifications.PurchaseOrders;

public class PurchaseOrderStatusFilterSpecification : BaseSpecification<PurchaseOrder>
{
    public PurchaseOrderStatusFilterSpecification(PurchaseOrderStatus? purchaseOrderStatus)
    {
        if (purchaseOrderStatus == null)
        {
            FilterCondition = p => true;
        }
        else
        {
            FilterCondition = p => p.Status == purchaseOrderStatus;
        }
    }
}