using Microsoft.EntityFrameworkCore;
using Velocity.Backend.DbContexts;
using Velocity.Shared.Entities;
using Velocity.Shared.Requests.Products;
using Velocity.Shared.Responses.Products;
using Velocity.Shared.Wrapper;

namespace Velocity.Backend.Controllers;

[ApiController]
[Route("products")]
public class ProductController : ControllerBase
{
    private readonly AppDbContext _appDbContext;

    public ProductController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get(int pageNumber = 1, int pageSize = 10, string searchString = "")
    {
        if(pageNumber < 1 || pageSize < 1)
        {
            return BadRequest(PaginatedResult<ProductResponse>.Failure("Page number and page size must be greater than 0"));
        }
        var products = await _appDbContext
            .Products
            .Where(x => x.Name.Contains(searchString))
            .OrderBy(x => x.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Include(x => x.Supplier)
            .Select(x => new ProductResponse()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                QuantityOnHand = x.QuantityOnHand,
                AlertQuantity = x.AlertQuantity,
                SupplierId = x.SupplierId,
                SupplierName = x.Supplier.Name,
            })
            .ToListAsync();
        var count = await _appDbContext
            .Products
            .Where(x => x.Name.Contains(searchString))
            .CountAsync();
        return Ok(PaginatedResult<ProductResponse>.Success(products, pageNumber, pageSize, count));
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var product = await _appDbContext
            .Products
            .Where(x => x.Id == id)
            .Include(x => x.Supplier)
            .Select(x => new ProductResponse()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                QuantityOnHand = x.QuantityOnHand,
                AlertQuantity = x.AlertQuantity,
                SupplierId = x.SupplierId,
                SupplierName = x.Supplier.Name,
            })
            .FirstOrDefaultAsync();
        if(product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }
    
    [HttpPost]
    public async Task<IActionResult> Post(CreateProductRequest request)
    {
        var product = new Product()
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            QuantityOnHand = 0,
            AlertQuantity = request.AlertQuantity,
            SupplierId = request.SupplierId,
        };
        await _appDbContext.Products.AddAsync(product);
        await _appDbContext.SaveChangesAsync();
        return Created(nameof(Get), new { id = product.Id });
        // return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
    }
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, UpdateProductRequest request)
    {
        var product = await _appDbContext
            .Products
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
        if(product == null)
        {
            return NotFound();
        }
        product.Name = request.Name;
        product.Description = request.Description;
        product.AlertQuantity = request.AlertQuantity;
        product.SupplierId = request.SupplierId;
        await _appDbContext.SaveChangesAsync();
        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var product = await _appDbContext
            .Products
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
        if(product == null)
        {
            return NotFound();
        }
        if(product.QuantityOnHand > 0)
        {
            return BadRequest("Cannot delete product with quantity on hand");
        }
        if(await _appDbContext.PurchaseOrders.AnyAsync(x => x.Items.Any(y => y.ProductId == id)))
        {
            return BadRequest("Cannot delete product present in purchase orders");
        }
        if(await _appDbContext.PurchaseInvoices.AnyAsync(x => x.Items.Any(y => y.ProductId == id)))
        {
            return BadRequest("Cannot delete product present in purchase invoices");
        }
        if(await _appDbContext.SalesInvoices.AnyAsync(x => x.Items.Any(y => y.ProductId == id)))
        {
            return BadRequest("Cannot delete product that has been sold");
        }
        _appDbContext.Products.Remove(product);
        await _appDbContext.SaveChangesAsync();
        return NoContent();
    }
}