using Velocity.Shared.Entities;

namespace Velocity.Backend.Specifications.Products;

public class ProductSupplierFilterSpecification : BaseSpecification<Product>
{
    public ProductSupplierFilterSpecification(Guid? supplierId)
    {
        if (supplierId.HasValue)
        {
            FilterCondition = x => x.SupplierId == supplierId;
        }
        else
        {
            FilterCondition = x => true;
        }
    }
}