using Microsoft.EntityFrameworkCore;
using Velocity.Backend.DbContexts;
using Velocity.Backend.Extensions;
using Velocity.Backend.Specifications.SalesInvoices;
using Velocity.Shared.Entities;
using Velocity.Shared.Requests.SalesInvoices;
using Velocity.Shared.Responses.SalesInvoices;
using Velocity.Shared.Wrapper;

namespace Velocity.Backend.Controllers;

[ApiController]
[Route("sales-invoices")]
public class SalesInvoiceController : ControllerBase
{
    private readonly AppDbContext _appDbContext;

    public SalesInvoiceController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10, string searchString = "")
    {
        if(pageNumber < 1 || pageSize < 1)
        {
            return BadRequest(PaginatedResult<SalesInvoiceResponse>.Failure("Page number and page size must be greater than 0"));
        }
        var salesInvoices = await _appDbContext.SalesInvoices
            .Include(x => x.Customer)
            .Include(x => x.Items).ThenInclude(x => x.Product)
            .OrderByDescending(x => x.InvoiceDate)
            .Specify(new SalesInvoiceSearchFilterSpecification(searchString))
            .Select(x => new SalesInvoiceResponse()
            {
                Id = x.Id,
                InvoiceNumber = x.InvoiceNumber,
                InvoiceDate = x.InvoiceDate,
                CustomerId = x.CustomerId,
                Customer = x.Customer.Name,
                Items = x.Items.Select(y => new SalesInvoiceItemResponse()
                {
                    Id = y.Id,
                    ProductId = y.ProductId,
                    Product = y.Product.Name,
                    PurchaseInvoiceItemId = y.PurchaseInvoiceItemId,
                    UnitPrice = y.UnitPrice,
                    Quantity = y.Quantity,
                    Discount = y.Discount,
                    TaxPercentage = y.TaxPercentage,
                }).ToList()
            })
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        var totalRecords = await _appDbContext.SalesInvoices.CountAsync();
        return Ok(PaginatedResult<SalesInvoiceResponse>.Success(salesInvoices, pageNumber, pageSize, totalRecords));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var salesInvoice = await _appDbContext.SalesInvoices
            .Include(x => x.Customer)
            .Include(x => x.Items).ThenInclude(x => x.Product)
            .Select(x => new SalesInvoiceResponse()
            {
                Id = x.Id,
                InvoiceNumber = x.InvoiceNumber,
                InvoiceDate = x.InvoiceDate,
                CustomerId = x.CustomerId,
                Customer = x.Customer.Name,
                Items = x.Items.Select(y => new SalesInvoiceItemResponse()
                {
                    Id = y.Id,
                    ProductId = y.ProductId,
                    Product = y.Product.Name,
                    PurchaseInvoiceItemId = y.PurchaseInvoiceItemId,
                    UnitPrice = y.UnitPrice,
                    Quantity = y.Quantity,
                    Discount = y.Discount,
                    TaxPercentage = y.TaxPercentage,
                }).ToList()
            })
            .FirstOrDefaultAsync(x => x.Id == id);
        if (salesInvoice == null)
        {
            return NotFound();
        }

        return Ok(salesInvoice);
    }

    [HttpGet("product/{id:guid}")]
    public async Task<IActionResult> GetProductFromPurchaseInvoiceItemId(Guid id)
    {
        var purchaseInvoiceItem = await _appDbContext.PurchaseInvoiceItems
            .Include(x => x.Product)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (purchaseInvoiceItem == null)
        {
            return BadRequest("Requested Invoice item doesn't exist!");
        }

        var response = new SaleProductResponse()
        {
            ProductId = purchaseInvoiceItem.ProductId,
            Product = purchaseInvoiceItem.Product.Name,
            AvailableQuantity = purchaseInvoiceItem.Quantity,
            UnitSellingPrice = purchaseInvoiceItem.UnitSellingPrice,
            TaxPercentage = purchaseInvoiceItem.TaxPercentage,
            PurchaseInvoiceItemId = purchaseInvoiceItem.Id,
        };
        return Ok(response);
    }

    [HttpGet("generate")]
    public async Task<IActionResult> GenerateNewInvoiceNumber(long saleDate)
    {
        var invoiceDateTime = new DateTime(saleDate);
        var countPattern = await _appDbContext.SalesInvoices.CountAsync(x => x.InvoiceDate == invoiceDateTime);
        var newInvoiceNumber = $"SI/{invoiceDateTime:ddMMyyyy}/{countPattern + 1:00000}";
        return Ok(new { invoiceNumber = newInvoiceNumber });
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreateSaleInvoiceRequest request)
    {
        var salesInvoice = new SalesInvoice()
        {
            Id = Guid.NewGuid(),
            InvoiceNumber = request.InvoiceNumber,
            InvoiceDate = request.InvoiceDate.GetValueOrDefault(),
            CustomerId = request.CustomerId,
            Items = request.Items.Select(x => new SalesInvoiceItem()
            {
                Id = Guid.NewGuid(),
                ProductId = x.ProductId,
                PurchaseInvoiceItemId = x.PurchaseInvoiceItemId,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice,
                Discount = x.Discount,
                TaxPercentage = x.TaxPercentage,
            }).ToList()
        };
        foreach (var item in request.Items)
        {
            var product = await _appDbContext.Products.FirstOrDefaultAsync(x => x.Id == item.ProductId);
            if (product == null)
            {
                return BadRequest($"Product with id {item.ProductId} not found");
            }
            if(product.QuantityOnHand < item.Quantity)
            {
                return BadRequest($"Product with id {item.ProductId} doesn't have enough quantity on hand");
            }
            product.QuantityOnHand -= item.Quantity;
        }
        await _appDbContext.SalesInvoices.AddAsync(salesInvoice);
        await _appDbContext.SaveChangesAsync();
        return Created(nameof(Get), new { id = salesInvoice.Id });
        // return CreatedAtAction(nameof(Get), new { id = salesInvoice.Id }, salesInvoice);
    }
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, UpdateSaleInvoiceRequest request)
    {
        var salesInvoice = await _appDbContext.SalesInvoices
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (salesInvoice == null)
        {
            return NotFound();
        }

        salesInvoice.InvoiceNumber = request.InvoiceNumber;
        salesInvoice.InvoiceDate = request.InvoiceDate.GetValueOrDefault();
        salesInvoice.CustomerId = request.CustomerId;
        salesInvoice.Items = request.Items.Select(x => new SalesInvoiceItem()
        {
            Id = Guid.NewGuid(),
            ProductId = x.ProductId,
            Quantity = x.Quantity,
            UnitPrice = x.UnitPrice,
            Discount = x.Discount,
            TaxPercentage = x.TaxPercentage,
        }).ToList();
        await _appDbContext.SaveChangesAsync();
        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var salesInvoice = await _appDbContext.SalesInvoices
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (salesInvoice == null)
        {
            return NotFound();
        }
        foreach (var item in salesInvoice.Items)
        {
            _appDbContext.SalesInvoiceItems.Remove(item);
            var product = await _appDbContext.Products.FirstOrDefaultAsync(x => x.Id == item.ProductId);
            if (product == null)
            {
                return BadRequest($"Product with id {item.ProductId} not found");
            }
            product.QuantityOnHand += item.Quantity;
        }

        _appDbContext.SalesInvoices.Remove(salesInvoice);
        await _appDbContext.SaveChangesAsync();
        return NoContent();
    }
}