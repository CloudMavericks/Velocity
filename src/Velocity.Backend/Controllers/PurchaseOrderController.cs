using Microsoft.EntityFrameworkCore;
using Velocity.Backend.DbContexts;
using Velocity.Shared.Entities;
using Velocity.Shared.Requests.PurchaseOrders;
using Velocity.Shared.Responses.PurchaseOrders;
using Velocity.Shared.Wrapper;

namespace Velocity.Backend.Controllers;

[ApiController]
[Route("purchase-orders")]
public class PurchaseOrderController : ControllerBase
{
    private readonly AppDbContext _appDbContext;

    public PurchaseOrderController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetPurchaseOrders(int pageNumber = 1, int pageSize = 10, string searchString = "")
    {
        var purchaseOrders = await _appDbContext.PurchaseOrders
            .Include(x => x.Supplier)
            .Include(x => x.Items)
            .ThenInclude(x => x.Product)
            .OrderByDescending(x => x.OrderDate)
            .Where(x => x.OrderNumber.Contains(searchString) || x.SupplierReferenceNumber.Contains(searchString))
            .Select(x => new PurchaseOrderResponse
            {
                Id = x.Id,
                OrderNumber = x.OrderNumber,
                SupplierReferenceNumber = x.SupplierReferenceNumber,
                OrderDate = x.OrderDate,
                SupplierId = x.SupplierId,
                SupplierName = x.Supplier.Name,
                Items = x.Items.Select(y => new PurchaseOrderItemResponse
                {
                    Id = y.Id,
                    ProductId = y.ProductId,
                    Product = y.Product.Name,
                    DiscountAmount = y.DiscountAmount,
                    Quantity = y.Quantity,
                    TaxPercentage = y.TaxPercentage,
                    UnitPrice = y.UnitPrice
                }).ToList()
            })
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        var totalRecords = await _appDbContext.PurchaseOrders.CountAsync();
        return Ok(PaginatedResult<PurchaseOrderResponse>.Success(purchaseOrders, pageNumber, pageSize, totalRecords));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetPurchaseOrder(Guid id)
    {
        var purchaseOrder = await _appDbContext.PurchaseOrders
            .Include(x => x.Supplier)
            .Include(x => x.Items)
            .ThenInclude(x => x.Product)
            .Select(x => new PurchaseOrderResponse
            {
                Id = x.Id,
                OrderNumber = x.OrderNumber,
                SupplierReferenceNumber = x.SupplierReferenceNumber,
                OrderDate = x.OrderDate,
                SupplierId = x.SupplierId,
                SupplierName = x.Supplier.Name,
                Items = x.Items.Select(y => new PurchaseOrderItemResponse
                {
                    Id = y.Id,
                    ProductId = y.ProductId,
                    Product = y.Product.Name,
                    DiscountAmount = y.DiscountAmount,
                    Quantity = y.Quantity,
                    TaxPercentage = y.TaxPercentage,
                    UnitPrice = y.UnitPrice
                }).ToList()
            })
            .FirstOrDefaultAsync();
        if (purchaseOrder == null)
        {
            return NotFound();
        }

        return Ok(purchaseOrder);
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreatePurchaseOrderRequest purchaseRequest)
    {
        var purchaseOrder = new PurchaseOrder()
        {
            Id = Guid.NewGuid(),
            OrderDate = purchaseRequest.OrderDate,
            OrderNumber = purchaseRequest.OrderNumber,
            SupplierReferenceNumber = purchaseRequest.SupplierReferenceNumber,
            SupplierId = purchaseRequest.SupplierId
        };
        foreach (var item in purchaseOrder.Items)
        {
            var orderItem = new PurchaseOrderItem()
            {
                Id = Guid.NewGuid(),
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                TaxPercentage = item.TaxPercentage,
                UnitPrice = item.UnitPrice,
                DiscountAmount = item.DiscountAmount
            };
            purchaseOrder.Items.Add(orderItem);
        }
        await _appDbContext.PurchaseOrders.AddAsync(purchaseOrder);
        await _appDbContext.SaveChangesAsync();
        return Created(nameof(GetPurchaseOrder), new { id = purchaseOrder.Id });
        // return CreatedAtAction(nameof(Get), new { id = customer.Id }, customer);
    }
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, UpdatePurchaseOrderRequest purchaseRequest)
    {
        var purchaseOrder = await _appDbContext
            .PurchaseOrders
            .Where(x => x.Id == id)
            .Include(x => x.Items)
            .FirstOrDefaultAsync();
        if(purchaseOrder == null)
        {
            return NotFound();
        }
        purchaseOrder.OrderDate = purchaseRequest.OrderDate;
        purchaseOrder.OrderNumber = purchaseRequest.OrderNumber;
        purchaseOrder.SupplierReferenceNumber = purchaseRequest.SupplierReferenceNumber;
        purchaseOrder.SupplierId = purchaseRequest.SupplierId;
        purchaseOrder.Items.Clear();
        foreach (var item in purchaseRequest.Items)
        {
            if (item.Id == Guid.Empty)
            {
                item.Id = Guid.NewGuid();                
            }
            var orderItem = new PurchaseOrderItem()
            {
                Id = item.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                TaxPercentage = item.TaxPercentage,
                UnitPrice = item.UnitPrice,
                DiscountAmount = item.DiscountAmount
            };
            purchaseOrder.Items.Add(orderItem);
        }
        await _appDbContext.SaveChangesAsync();
        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var purchaseOrder = await _appDbContext
            .PurchaseOrders
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
        if(purchaseOrder == null)
        {
            return NotFound();
        }
        _appDbContext.PurchaseOrders.Remove(purchaseOrder);
        await _appDbContext.SaveChangesAsync();
        return NoContent();
    }
}