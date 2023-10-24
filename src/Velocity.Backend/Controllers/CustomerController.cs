using Microsoft.EntityFrameworkCore;
using Velocity.Backend.DbContexts;
using Velocity.Shared.Entities;
using Velocity.Shared.Requests.Customers;
using Velocity.Shared.Responses.Customers;
using Velocity.Shared.Wrapper;

namespace Velocity.Backend.Controllers;

[ApiController]
[Route("customers")]
public class CustomerController : ControllerBase
{
    private readonly AppDbContext _appDbContext;

    public CustomerController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get(int pageNumber = 1, int pageSize = 10, string searchString = "")
    {
        if(pageNumber < 1 || pageSize < 1)
        {
            return BadRequest(PaginatedResult<CustomerResponse>.Failure("Page number and page size must be greater than 0"));
        }
        var customers = await _appDbContext
            .Customers
            .Where(x => x.Name.Contains(searchString))
            .OrderBy(x => x.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new CustomerResponse()
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
            .Customers
            .Where(x => x.Name.Contains(searchString))
            .CountAsync();
        return Ok(PaginatedResult<CustomerResponse>.Success(customers, pageNumber, pageSize, count));
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var customer = await _appDbContext
            .Customers
            .Where(x => x.Id == id)
            .Select(x => new CustomerResponse()
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
        if(customer == null)
        {
            return NotFound();
        }
        return Ok(customer);
    }
    
    [HttpPost]
    public async Task<IActionResult> Post(CreateCustomerRequest customerResponse)
    {
        var customer = new Customer()
        {
            Id = Guid.NewGuid(),
            Name = customerResponse.Name,
            ContactName = customerResponse.ContactName,
            ContactEmail = customerResponse.ContactEmail,
            ContactPhone = customerResponse.ContactPhone,
            Address = customerResponse.Address,
            City = customerResponse.City,
            State = customerResponse.State,
            ZipCode = customerResponse.ZipCode,
            Gstn = customerResponse.Gstn,
        };
        await _appDbContext.Customers.AddAsync(customer);
        await _appDbContext.SaveChangesAsync();
        return Created(nameof(Get), new { id = customer.Id });
        // return CreatedAtAction(nameof(Get), new { id = customer.Id }, customer);
    }
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, UpdateCustomerRequest customerResponse)
    {
        var customer = await _appDbContext
            .Customers
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
        if(customer == null)
        {
            return NotFound();
        }
        customer.Name = customerResponse.Name;
        customer.ContactName = customerResponse.ContactName;
        customer.ContactEmail = customerResponse.ContactEmail;
        customer.ContactPhone = customerResponse.ContactPhone;
        customer.Address = customerResponse.Address;
        customer.City = customerResponse.City;
        customer.State = customerResponse.State;
        customer.ZipCode = customerResponse.ZipCode;
        customer.Gstn = customerResponse.Gstn;
        await _appDbContext.SaveChangesAsync();
        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var customer = await _appDbContext
            .Customers
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
        if(customer == null)
        {
            return NotFound();
        }
        if(await _appDbContext.SalesInvoices.AnyAsync(x => x.CustomerId == id))
        {
            return BadRequest("Customer has sales invoices.");
        }
        _appDbContext.Customers.Remove(customer);
        await _appDbContext.SaveChangesAsync();
        return NoContent();
    }
}