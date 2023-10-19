namespace Velocity.Shared.Contracts;

public interface IAuditableEntity : IEntity
{
    DateTime CreatedAt { get; set; }
    Guid CreatedBy { get; set; }
    string CreatedByName { get; set; }
    
    DateTime? UpdatedAt { get; set; }
    Guid? UpdatedBy { get; set; }
    string UpdatedByName { get; set; }
    
    DateTime? DeletedAt { get; set; }
    Guid? DeletedBy { get; set; }
    string DeletedByName { get; set; }
}

public interface IAuditableEntity<T> : IAuditableEntity, IEntity<T>
{
    
}