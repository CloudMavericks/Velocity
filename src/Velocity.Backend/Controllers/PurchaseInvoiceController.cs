using Microsoft.EntityFrameworkCore;
using Velocity.Backend.DbContexts;
using Velocity.Backend.Extensions;
using Velocity.Backend.PrintObjects;
using Velocity.Backend.Specifications.PurchaseInvoices;
using Velocity.Shared.Entities;
using Velocity.Shared.Enums;
using Velocity.Shared.Requests.PurchaseInvoices;
using Velocity.Shared.Responses.PurchaseInvoices;
using Velocity.Shared.Responses.Suppliers;
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
                Status = x.Status,
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
                Status = x.Status,
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
            Status = PurchaseInvoiceStatus.Pending,
            SupplierId = request.SupplierId,
            Items = new List<PurchaseInvoiceItem>()
        };
        var purchaseOrder = await _appDbContext.PurchaseOrders
            .FirstOrDefaultAsync(x => x.OrderNumber == request.ReferenceNumber);
        if (purchaseOrder != null)
        {
            switch (purchaseOrder.Status)
            {
                case PurchaseOrderStatus.Completed:
                    return BadRequest("Selected Purchase Order Reference Number is already completed!");
                case PurchaseOrderStatus.Cancelled:
                    return BadRequest("Selected Purchase Order Reference Number is already cancelled!");
                case PurchaseOrderStatus.Placed:
                default:
                    purchaseOrder.Status = PurchaseOrderStatus.Completed;
                    break;
            }
        }
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
        }
        await _appDbContext.PurchaseInvoices.AddAsync(purchaseInvoice);
        await _appDbContext.SaveChangesAsync();
        return Created(nameof(GetPurchaseInvoice), new { id = purchaseInvoice.Id });
        // return CreatedAtAction(nameof(GetPurchaseInvoice), new { id = purchaseInvoice.Id }, purchaseInvoice);
    }
    
    [HttpGet("{id:guid}/complete")]
    public async Task<IActionResult> Complete(Guid id)
    {
        var purchaseInvoice = await _appDbContext.PurchaseInvoices
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (purchaseInvoice == null)
        {
            return NotFound();
        }
        switch (purchaseInvoice.Status)
        {
            case PurchaseInvoiceStatus.Completed:
                return BadRequest("Selected Purchase Invoice is already completed!");
            case PurchaseInvoiceStatus.Cancelled:
                return BadRequest("Cancelled Invoice cannot be completed!");
        }

        purchaseInvoice.Status = PurchaseInvoiceStatus.Completed;
        foreach (var item in purchaseInvoice.Items)
        {
            var product = await _appDbContext.Products.FirstOrDefaultAsync(x => x.Id == item.ProductId);
            if (product != null)
            {
                product.QuantityOnHand += item.Quantity;
            }
        }
        await _appDbContext.SaveChangesAsync();
        return NoContent();
    }
    
    [HttpGet("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        var purchaseInvoice = await _appDbContext.PurchaseInvoices
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (purchaseInvoice == null)
        {
            return NotFound();
        }
        switch (purchaseInvoice.Status)
        {
            case PurchaseInvoiceStatus.Completed:
                return BadRequest("Completed Invoice cannot be cancelled!");
            case PurchaseInvoiceStatus.Cancelled:
                return BadRequest("Selected Purchase Invoice is already cancelled!");
        }

        purchaseInvoice.Status = PurchaseInvoiceStatus.Cancelled;
        foreach (var item in purchaseInvoice.Items)
        {
            var product = await _appDbContext.Products.FirstOrDefaultAsync(x => x.Id == item.ProductId);
            if (product != null)
            {
                product.QuantityOnHand -= item.Quantity;
            }
        }
        await _appDbContext.SaveChangesAsync();
        return NoContent();
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
        switch (purchaseInvoice.Status)
        {
            case PurchaseInvoiceStatus.Completed:
                return BadRequest("Selected Purchase Invoice is already completed!");
            case PurchaseInvoiceStatus.Cancelled:
                return BadRequest("Selected Purchase Invoice is already cancelled!");
        }

        if (purchaseInvoice.ReferenceNumber != request.ReferenceNumber)
        {
            var originalPurchaseOrder = await _appDbContext.PurchaseOrders
                .FirstOrDefaultAsync(x => x.OrderNumber == purchaseInvoice.ReferenceNumber);
            Console.WriteLine($"Original Purchase Order: {originalPurchaseOrder?.Status}");
            if (originalPurchaseOrder is { Status: PurchaseOrderStatus.Completed })
            {
                originalPurchaseOrder.Status = PurchaseOrderStatus.Placed;
            }

            var newPurchaseOrder = await _appDbContext.PurchaseOrders
                .FirstOrDefaultAsync(x => x.OrderNumber == request.ReferenceNumber);
            Console.WriteLine($"New Purchase Order: {newPurchaseOrder?.Status}");
            if (newPurchaseOrder != null)
            {
                switch (newPurchaseOrder.Status)
                {
                    case PurchaseOrderStatus.Completed:
                        return BadRequest("Selected Purchase Order Reference Number is already completed!");
                    case PurchaseOrderStatus.Cancelled:
                        return BadRequest("Selected Purchase Order Reference Number is already cancelled!");
                    case PurchaseOrderStatus.Placed:
                    default:
                        newPurchaseOrder.Status = PurchaseOrderStatus.Completed;
                        break;
                }
            }
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
        switch (purchaseInvoice.Status)
        {
            case PurchaseInvoiceStatus.Completed:
                return BadRequest("Completed Invoice cannot be deleted!");
            case PurchaseInvoiceStatus.Cancelled:
                return BadRequest("Cancelled Invoice cannot be deleted!");
        }
        var purchaseOrder = await _appDbContext.PurchaseOrders
            .FirstOrDefaultAsync(x => x.OrderNumber == purchaseInvoice.ReferenceNumber);
        if (purchaseOrder is { Status: PurchaseOrderStatus.Completed })
        {
            purchaseOrder.Status = PurchaseOrderStatus.Placed;
        }

        foreach (var item in purchaseInvoice.Items)
        {
            _appDbContext.PurchaseInvoiceItems.Remove(item);
        }
        _appDbContext.PurchaseInvoices.Remove(purchaseInvoice);
        await _appDbContext.SaveChangesAsync();
        return NoContent();
    }
    
    [HttpGet("{id:guid}/print")]
    public async Task<IActionResult> Print(Guid id)
    {
        var purchaseOrder = await _appDbContext.PurchaseInvoices
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
                Status = x.Status,
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
        var supplierResponse = await _appDbContext.Suppliers
            .Select(x => new SupplierResponse
            {
                Id = x.Id,
                Name = x.Name,
                ContactName = x.ContactName,
                ContactEmail = x.ContactEmail,
                ContactPhone = x.ContactPhone,
                Address = x.Address,
                City = x.City,
                State = x.State,
                ZipCode = x.ZipCode,
                Gstn = x.Gstn
            })
            .FirstOrDefaultAsync(x => x.Id == purchaseOrder.SupplierId);
        var bytes = PurchaseInvoiceDocument.Generate(purchaseOrder, supplierResponse);
        return File(bytes, "application/pdf", $"{purchaseOrder.InvoiceNumber}.pdf");
    }
}