using Microsoft.EntityFrameworkCore;
using Velocity.Backend.DbContexts;
using Velocity.Backend.Extensions;
using Velocity.Backend.Specifications.PurchaseInvoices;
using Velocity.Shared.Entities;
using Velocity.Shared.Requests.PurchaseInvoices;
using Velocity.Shared.Responses.PurchaseInvoices;
using Velocity.Shared.Wrapper;

namespace Velocity.Backend.Controllers;

[ApiController]
[Route("purchase-invoices")]
public class PurchaseInvoiceController : ControllerBase
{
    private readonly AppDbContext _appDbContext;

    public PurchaseInvoiceController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(int pageNumber, int pageSize, string searchString)
    {
        if(pageNumber < 1 || pageSize < 1)
        {
            return BadRequest(PaginatedResult<PurchaseInvoiceResponse>.Failure("Page number and page size must be greater than 0"));
        }
        var purchaseInvoices = await _appDbContext.PurchaseInvoices
            .Include(x => x.Supplier)
            .Include(x => x.Items)
            .ThenInclude(x => x.Product)
            .OrderByDescending(x => x.PurchaseDate)
            .Specify(new PurchaseInvoiceSearchFilterSpecification(searchString))
            .Select(x => new PurchaseInvoiceResponse()
            {
                Id = x.Id,
                PurchaseNumber = x.PurchaseNumber,
                PurchaseDate = x.PurchaseDate,
                InvoiceNumber = x.InvoiceNumber,
                InvoiceDate = x.InvoiceDate,
                ReferenceNumber = x.ReferenceNumber,
                SupplierId = x.SupplierId,
                Supplier = x.Supplier.Name,
                Items = x.Items.Select(y => new PurchaseInvoiceItemResponse()
                {
                    Id = y.Id,
                    ProductId = y.ProductId,
                    Product = y.Product.Name,
                    Quantity = y.Quantity,
                    UnitPrice = y.UnitPrice,
                    DiscountAmount = y.DiscountAmount,
                    TaxPercentage = y.TaxPercentage,
                    UnitSellingPrice = y.UnitSellingPrice
                }).ToList()
            })
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        var totalRecords = await _appDbContext.PurchaseInvoices
            .Specify(new PurchaseInvoiceSearchFilterSpecification(searchString))
            .CountAsync();
        return Ok(PaginatedResult<PurchaseInvoiceResponse>.Success(purchaseInvoices, pageNumber, pageSize, totalRecords));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetPurchaseInvoice(Guid id)
    {
        var purchaseInvoice = await _appDbContext.PurchaseInvoices

            .Include(x => x.Supplier)
            .Include(x => x.Items)
            .ThenInclude(x => x.Product)
            .Select(x => new PurchaseInvoiceResponse()
            {
                Id = x.Id,
                PurchaseNumber = x.PurchaseNumber,
                PurchaseDate = x.PurchaseDate,
                InvoiceNumber = x.InvoiceNumber,
                InvoiceDate = x.InvoiceDate,
                ReferenceNumber = x.ReferenceNumber,
                SupplierId = x.SupplierId,
                Supplier = x.Supplier.Name,
                Items = x.Items.Select(y => new PurchaseInvoiceItemResponse()
                {
                    Id = y.Id,
                    ProductId = y.ProductId,
                    Product = y.Product.Name,
                    Quantity = y.Quantity,
                    UnitPrice = y.UnitPrice,
                    DiscountAmount = y.DiscountAmount,
                    TaxPercentage = y.TaxPercentage,
                    UnitSellingPrice = y.UnitSellingPrice
                }).ToList()
            })
            .FirstOrDefaultAsync(x => x.Id == id);
        if (purchaseInvoice == null)
        {
            return NotFound();
        }
        return Ok(purchaseInvoice);
    }

    [HttpGet("generate")]
    public async Task<IActionResult> GenerateNewPurchaseNumber(long purchaseDate)
    {
        var purchaseDateTime = new DateTime(purchaseDate);
        var countPattern = await _appDbContext.PurchaseInvoices.Where(x => x.PurchaseDate == purchaseDateTime).CountAsync();
        var newPurchaseNumber = $"PI/{purchaseDateTime:ddMMyyyy}/{countPattern + 1:00000}";
        return Ok(new { purchaseNumber = newPurchaseNumber });
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreatePurchaseInvoiceRequest request)
    {
        var purchaseInvoice = new PurchaseInvoice()
        {
            Id = Guid.NewGuid(),
            InvoiceDate = request.InvoiceDate.GetValueOrDefault(),
            InvoiceNumber = request.InvoiceNumber,
            PurchaseDate = request.PurchaseDate.GetValueOrDefault(),
            PurchaseNumber = request.PurchaseNumber,
            ReferenceNumber = request.ReferenceNumber,
            SupplierId = request.SupplierId,
            Items = new List<PurchaseInvoiceItem>()
        };
        foreach (var item in request.Items)
        {
            purchaseInvoice.Items.Add(new PurchaseInvoiceItem()
            {
                Id = Guid.NewGuid(),
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                DiscountAmount = item.DiscountAmount,
                TaxPercentage = item.TaxPercentage,
                UnitSellingPrice = item.UnitSellingPrice
            });
            var product = await _appDbContext.Products.FirstOrDefaultAsync(x => x.Id == item.ProductId);
            if (product != null)
            {
                product.QuantityOnHand += item.Quantity;
            }
        }
        await _appDbContext.PurchaseInvoices.AddAsync(purchaseInvoice);
        await _appDbContext.SaveChangesAsync();
        return Created(nameof(GetPurchaseInvoice), new { id = purchaseInvoice.Id });
        // return CreatedAtAction(nameof(GetPurchaseInvoice), new { id = purchaseInvoice.Id }, purchaseInvoice);
    }
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, UpdatePurchaseInvoiceRequest request)
    {
        var purchaseInvoice = await _appDbContext.PurchaseInvoices
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (purchaseInvoice == null)
        {
            return NotFound();
        }
        purchaseInvoice.InvoiceDate = request.InvoiceDate.GetValueOrDefault();
        purchaseInvoice.InvoiceNumber = request.InvoiceNumber;
        purchaseInvoice.PurchaseDate = request.PurchaseDate.GetValueOrDefault();
        purchaseInvoice.PurchaseNumber = request.PurchaseNumber;
        purchaseInvoice.ReferenceNumber = request.ReferenceNumber;
        purchaseInvoice.SupplierId = request.SupplierId;
        var purchaseInvoiceItems = await _appDbContext.PurchaseInvoiceItems
            .Where(x => x.PurchaseInvoiceId == purchaseInvoice.Id)
            .ToListAsync();
        foreach (var invoiceItem in purchaseInvoiceItems)
        {
            _appDbContext.PurchaseInvoiceItems.Remove(invoiceItem);
        }
        await _appDbContext.SaveChangesAsync();
        foreach (var item in request.Items)
        {
            var invoiceItem = new PurchaseInvoiceItem()
            {
                Id = item.Id == Guid.Empty ? Guid.NewGuid() : item.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                DiscountAmount = item.DiscountAmount,
                TaxPercentage = item.TaxPercentage,
                UnitSellingPrice = item.UnitSellingPrice
            };
            purchaseInvoice.Items.Add(invoiceItem);
        }
        await _appDbContext.SaveChangesAsync();
        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var purchaseInvoice = await _appDbContext.PurchaseInvoices
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (purchaseInvoice == null)
        {
            return NotFound();
        }
        foreach (var item in purchaseInvoice.Items)
        {
            _appDbContext.PurchaseInvoiceItems.Remove(item);
        }
        _appDbContext.PurchaseInvoices.Remove(purchaseInvoice);
        await _appDbContext.SaveChangesAsync();
        return NoContent();
    }
}