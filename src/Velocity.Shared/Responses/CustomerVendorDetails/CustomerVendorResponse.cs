using Velocity.Shared.Enums;

namespace Velocity.Shared.Responses.CustomerVendorDetails;

public record CustomerVendorResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string FullName { get; init; }
    public CustomerVendorType Type { get; init; }
    public string Address { get; init; }
    public string City { get; init; }
    public string State { get; init; }
    public string PinCode { get; init; }
    public string Phone { get; init; }
}