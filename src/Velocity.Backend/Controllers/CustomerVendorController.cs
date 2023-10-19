using Microsoft.EntityFrameworkCore;
using Velocity.Backend.DbContexts;
using Velocity.Shared.Responses.CustomerVendorDetails;
using Velocity.Shared.Wrapper;

namespace Velocity.Backend.Controllers;

[ApiController]
[Route("customer-vendor")]
public class CustomerVendorController : ControllerBase
{
    private readonly AppDbContext _appDbContext;

    public CustomerVendorController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get(int pageNumber = 1, int pageSize = 10, string searchString = "")
    {
        if(pageNumber < 1 || pageSize < 1)
        {
            return BadRequest(PaginatedResult<CustomerVendorResponse>.Failure("Page number and page size must be greater than 0"));
        }
        var customerVendorDetails = await _appDbContext
            .CustomerVendorDetails
            .Where(x => x.Name.Contains(searchString))
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new CustomerVendorResponse()
            {
                Id = x.Id,
                Name = x.Name,
                FullName = x.FullName,
                Address = x.Address,
                City = x.City,
                State = x.State,
                PinCode = x.PinCode,
                Phone = x.Phone,
                Type = x.Type,
            })
            .ToListAsync();
        var count = await _appDbContext
            .CustomerVendorDetails
            .Where(x => x.Name.Contains(searchString))
            .CountAsync();
        return Ok(PaginatedResult<CustomerVendorResponse>.Success(customerVendorDetails, pageNumber, pageSize, count));
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var customerVendorDetail = await _appDbContext
            .CustomerVendorDetails
            .Where(x => x.Id == id)
            .Select(x => new CustomerVendorResponse()
            {
                Id = x.Id,
                Name = x.Name,
                FullName = x.FullName,
                Address = x.Address,
                City = x.City,
                State = x.State,
                PinCode = x.PinCode,
                Phone = x.Phone,
                Type = x.Type,
            })
            .FirstOrDefaultAsync();
        if(customerVendorDetail == null)
        {
            return NotFound(await Result<CustomerVendorResponse>.FailAsync("Customer or vendor not found"));
        }
        return Ok(await Result<CustomerVendorResponse>.SuccessAsync(customerVendorDetail));
    }
}