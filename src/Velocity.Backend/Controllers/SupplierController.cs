using Microsoft.EntityFrameworkCore;
using Velocity.Backend.DbContexts;
using Velocity.Shared.Entities;
using Velocity.Shared.Requests.Suppliers;
using Velocity.Shared.Responses.Suppliers;
using Velocity.Shared.Wrapper;

namespace Velocity.Backend.Controllers;

[ApiController]
[Route("suppliers")]
public class SupplierController : ControllerBase
{
    private readonly AppDbContext _appDbContext;

    public SupplierController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get(int pageNumber = 1, int pageSize = 10, string searchString = "")
    {
        if(pageNumber < 1 || pageSize < 1)
        {
            return BadRequest(PaginatedResult<SupplierResponse>.Failure("Page number and page size must be greater than 0"));
        }
        var suppliers = await _appDbContext
            .Suppliers
            .Where(x => x.Name.Contains(searchString))
            .OrderBy(x => x.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new SupplierResponse()
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
                Gstn = x.Gstn,
            })
            .ToListAsync();
        var count = await _appDbContext
            .Suppliers
            .Where(x => x.Name.Contains(searchString))
            .CountAsync();
        return Ok(PaginatedResult<SupplierResponse>.Success(suppliers, pageNumber, pageSize, count));
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var supplier = await _appDbContext
            .Suppliers
            .Where(x => x.Id == id)
            .Select(x => new SupplierResponse()
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
                Gstn = x.Gstn,
            })
            .FirstOrDefaultAsync();
        if(supplier == null)
        {
            return NotFound();
        }
        return Ok(supplier);
    }
    
    [HttpPost]
    public async Task<IActionResult> Post(CreateSupplierRequest request)
    {
        var supplier = new Supplier()
        {
            Name = request.Name,
            ContactName = request.ContactName,
            ContactEmail = request.ContactEmail,
            ContactPhone = request.ContactPhone,
            Address = request.Address,
            City = request.City,
            State = request.State,
            ZipCode = request.ZipCode,
            Gstn = request.Gstn,
        };
        await _appDbContext.Suppliers.AddAsync(supplier);
        await _appDbContext.SaveChangesAsync();
        return Created(nameof(Get), new { id = supplier.Id });
        // return CreatedAtAction(nameof(Get), new { id = supplier.Id }, supplier);
    }
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, UpdateSupplierRequest request)
    {
        var supplier = await _appDbContext
            .Suppliers
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
        if(supplier == null)
        {
            return NotFound();
        }
        supplier.Name = request.Name;
        supplier.ContactName = request.ContactName;
        supplier.ContactEmail = request.ContactEmail;
        supplier.ContactPhone = request.ContactPhone;
        supplier.Address = request.Address;
        supplier.City = request.City;
        supplier.State = request.State;
        supplier.ZipCode = request.ZipCode;
        supplier.Gstn = request.Gstn;
        await _appDbContext.SaveChangesAsync();
        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var supplier = await _appDbContext
            .Suppliers
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
        if(supplier == null)
        {
            return NotFound();
        }
        _appDbContext.Suppliers.Remove(supplier);
        await _appDbContext.SaveChangesAsync();
        return NoContent();
    }
}