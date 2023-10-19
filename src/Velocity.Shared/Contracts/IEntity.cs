namespace Velocity.Shared.Contracts;

public interface IEntity
{
    
}

public interface IEntity<T> : IEntity
{
    T Id { get; set; }
}