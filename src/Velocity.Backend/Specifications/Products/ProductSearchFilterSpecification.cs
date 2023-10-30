using Velocity.Shared.Entities;

namespace Velocity.Backend.Specifications.Products;

public class ProductSearchFilterSpecification : BaseSpecification<Product>
{
    public ProductSearchFilterSpecification(string searchString)
    {
        if (string.IsNullOrEmpty(searchString))
        {
            FilterCondition = x => true;
        }
        else
        {
            searchString = searchString.ToLower();
            FilterCondition = x => x.Name.ToLower().Contains(searchString) 
                                   || x.Description.ToLower().Contains(searchString)
                                   || x.Supplier.Name.ToLower().Contains(searchString);
        }
    }
}