using Microsoft.EntityFrameworkCore;
using Velocity.Backend.DbContexts;
using Velocity.Backend.Extensions;
using Velocity.Backend.Specifications.PurchaseOrders;
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
        if(pageNumber < 1 || pageSize < 1)
        {
            return BadRequest(PaginatedResult<PurchaseOrderResponse>.Failure("Page number and page size must be greater than 0"));
        }
        var purchaseOrders = await _appDbContext.PurchaseOrders
            .Include(x => x.Supplier)
            .Include(x => x.Items)
            .ThenInclude(x => x.Product)
            .OrderByDescending(x => x.OrderDate)
            .Specify(new PurchaseOrderSearchFilterSpecification(searchString))
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
        var totalRecords = await _appDbContext.PurchaseOrders
            .Specify(new PurchaseOrderSearchFilterSpecification(searchString))
            .CountAsync();
        return Ok(PaginatedResult<PurchaseOrderResponse>.Success(purchaseOrders, pageNumber, pageSize, totalRecords));
    }

    [HttpPost("get")]
    public async Task<IActionResult> GetWithFilter(GetPurchaseOrdersRequest request)
    {
        var purchaseOrders = await _appDbContext.PurchaseOrders
            .Include(x => x.Supplier)
            .Include(x => x.Items)
            .ThenInclude(x => x.Product)
            .OrderByDescending(x => x.OrderDate)
            .Specify(new PurchaseOrderSearchFilterSpecification(request.SearchString))
            .Specify(new PurchaseOrderSupplierFilterSpecification(request.SupplierId))
            .Specify(new PurchaseOrderDateFilterSpecification(request.OrderDate))
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
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();
        var totalRecords = await _appDbContext.PurchaseOrders
            .Specify(new PurchaseOrderSearchFilterSpecification(request.SearchString))
            .Specify(new PurchaseOrderSupplierFilterSpecification(request.SupplierId))
            .Specify(new PurchaseOrderDateFilterSpecification(request.OrderDate))
            .CountAsync();
        return Ok(PaginatedResult<PurchaseOrderResponse>.Success(purchaseOrders, request.PageNumber, request.PageSize,
            totalRecords));
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
            .FirstOrDefaultAsync(x => x.Id == id);
        if (purchaseOrder == null)
        {
            return NotFound();
        }

        return Ok(purchaseOrder);
    }

    [HttpGet("generate")]
    public async Task<IActionResult> GenerateNewOrderNumber(long orderDate)
    {
        var orderDateTime = new DateTime(orderDate);
        var countPattern = await _appDbContext.PurchaseOrders.CountAsync(x => x.OrderDate == orderDateTime);
        var newOrderNumber = $"PO/{orderDateTime:ddMMyyyy}/{countPattern + 1:00000}";
        return Ok(new { orderNumber = newOrderNumber });
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreatePurchaseOrderRequest purchaseRequest)
    {
        var purchaseOrder = new PurchaseOrder()
        {
            Id = Guid.NewGuid(),
            OrderDate = purchaseRequest.OrderDate.GetValueOrDefault(),
            OrderNumber = purchaseRequest.OrderNumber,
            SupplierReferenceNumber = purchaseRequest.SupplierReferenceNumber,
            SupplierId = purchaseRequest.SupplierId,
            Items = new List<PurchaseOrderItem>()
        };
        foreach (var item in purchaseRequest.Items)
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
            .FirstOrDefaultAsync(x => x.Id == id);
        if(purchaseOrder == null)
        {
            return NotFound();
        }
        purchaseOrder.OrderDate = purchaseRequest.OrderDate.GetValueOrDefault();
        purchaseOrder.OrderNumber = purchaseRequest.OrderNumber;
        purchaseOrder.SupplierReferenceNumber = purchaseRequest.SupplierReferenceNumber;
        purchaseOrder.SupplierId = purchaseRequest.SupplierId;
        var purchaseOrderItems = await _appDbContext.PurchaseOrderItems.Where(x => x.PurchaseOrderId == purchaseOrder.Id).ToListAsync();
        var purchaseRequestItemIds = purchaseRequest.Items.Select(x => x.Id).Distinct().ToList();
        foreach (var item in purchaseOrderItems.Where(x => purchaseRequestItemIds.Any(y => y == x.Id)))
        {
            _appDbContext.PurchaseOrderItems.Remove(item);
        }
        await _appDbContext.SaveChangesAsync();
        foreach (var item in purchaseRequest.Items)
        {
            var orderItem = new PurchaseOrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                TaxPercentage = item.TaxPercentage,
                UnitPrice = item.UnitPrice,
                DiscountAmount = item.DiscountAmount,
                PurchaseOrderId = purchaseOrder.Id,
                Id = item.Id == Guid.Empty ? Guid.NewGuid() : item.Id
            };
            _appDbContext.PurchaseOrderItems.Add(orderItem);
        }
        await _appDbContext.SaveChangesAsync();
        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var purchaseOrder = await _appDbContext
            .PurchaseOrders
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == id);
        if(purchaseOrder == null)
        {
            return NotFound();
        }
        foreach (var item in purchaseOrder.Items)
        {
            _appDbContext.PurchaseOrderItems.Remove(item);
        }
        _appDbContext.PurchaseOrders.Remove(purchaseOrder);
        await _appDbContext.SaveChangesAsync();
        return NoContent();
    }
}