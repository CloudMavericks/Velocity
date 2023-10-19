namespace Velocity.Shared.Wrapper;

public interface IResult
{
    bool Succeeded { get; set; }
    
    List<string> Messages { get; set; }
}

public interface IResult<T> : IResult
{
    T Data { get; set; }
}