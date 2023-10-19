using Velocity.Shared.Enums;

namespace Velocity.Shared.Requests.CustomerVendorDetails;

public class UpdateCustomerVendorDetailRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string FullName { get; set; }
    public CustomerVendorType Type { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string PinCode { get; set; }
    public string Phone { get; set; }
}