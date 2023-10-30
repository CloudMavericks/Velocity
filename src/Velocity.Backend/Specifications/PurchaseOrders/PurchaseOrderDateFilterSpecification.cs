using Velocity.Shared.Entities;

namespace Velocity.Backend.Specifications.PurchaseOrders;

public class PurchaseOrderDateFilterSpecification : BaseSpecification<PurchaseOrder>
{
    public PurchaseOrderDateFilterSpecification(DateTime? orderDate)
    {
        if(orderDate.HasValue)
        {
            FilterCondition = po => po.OrderDate == orderDate;
        }
        else
        {
            FilterCondition = po => true;
        }
    }
}