using Velocity.Shared.Contracts;
using Velocity.Shared.Enums;

namespace Velocity.Shared.Entities;

public class CustomerVendorDetail : IAuditableEntity<Guid>
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
    
    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public string CreatedByName { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public string UpdatedByName { get; set; }
    
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedBy { get; set; }
    public string DeletedByName { get; set; }
}