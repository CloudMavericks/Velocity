namespace Velocity.Shared.Responses.Suppliers;

public record SupplierResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string ContactName { get; init; }
    public string ContactEmail { get; init; }
    public string ContactPhone { get; init; }
    public string Address { get; init; }
    public string City { get; init; }
    public string State { get; init; }
    public string ZipCode { get; init; }
    public string Gstn { get; init; }

    public override string ToString()
    {
        return Name;
    }
}