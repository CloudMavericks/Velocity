namespace Velocity.Shared.Responses.Customers;

public class CustomerResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string ContactName { get; set; }
    public string ContactEmail { get; set; }
    public string ContactPhone { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public string Gstn { get; set; }

    public override string ToString()
    {
        return Name;
    }
}