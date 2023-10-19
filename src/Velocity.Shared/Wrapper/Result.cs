namespace Velocity.Shared.Wrapper;

public class Result : IResult
{
    public bool Succeeded { get; set; }
    
    public List<string> Messages { get; set; }
    
    public static Result Success()
    {
        return new Result
        {
            Succeeded = true
        };
    }
    
    public static Result Success(string message)
    {
        return new Result
        {
            Succeeded = true,
            Messages = new List<string> { message }
        };
    }
    
    public static Result Success(List<string> messages)
    {
        return new Result
        {
            Succeeded = true,
            Messages = messages
        };
    }
    
    public static Result Fail()
    {
        return new Result
        {
            Succeeded = false
        };
    }
    
    public static Result Fail(string message)
    {
        return new Result
        {
            Succeeded = false,
            Messages = new List<string> { message }
        };
    }
    
    public static Result Fail(List<string> messages)
    {
        return new Result
        {
            Succeeded = false,
            Messages = messages
        };
    }
    
    public static ValueTask<Result> SuccessAsync()
    {
        return ValueTask.FromResult(Success());
    }
    
    public static ValueTask<Result> SuccessAsync(string message)
    {
        return ValueTask.FromResult(Success(message));
    }
    
    public static ValueTask<Result> SuccessAsync(List<string> messages)
    {
        return ValueTask.FromResult(Success(messages));
    }
    
    public static ValueTask<Result> FailAsync()
    {
        return ValueTask.FromResult(Fail());
    }
    
    public static ValueTask<Result> FailAsync(string message)
    {
        return ValueTask.FromResult(Fail(message));
    }
    
    public static ValueTask<Result> FailAsync(List<string> messages)
    {
        return ValueTask.FromResult(Fail(messages));
    }
    
    public static implicit operator Result(bool succeeded)
    {
        return succeeded ? Success() : Fail();
    }
    
    public static implicit operator Result(string message)
    {
        return Success(message);
    }
    
    public static implicit operator Result(List<string> messages)
    {
        return Success(messages);
    }
}

public class Result<T> : Result, IResult<T>
{
    public T Data { get; set; }
    
    public static Result<T> Success(T data)
    {
        return new Result<T>()
        {
            Succeeded = true,
            Data = data
        };
    }
    
    public static Result<T> Success(T data, string message)
    {
        return new Result<T>()
        {
            Succeeded = true,
            Messages = new List<string> { message },
            Data = data
        };
    }
    
    public static Result<T> Success(T data, List<string> messages)
    {
        return new Result<T>()
        {
            Succeeded = true,
            Messages = messages,
            Data = data
        };
    }
    
    public new static Result<T> Fail(string message)
    {
        return new Result<T>
        {
            Succeeded = false,
            Messages = new List<string> { message }
        };
    }
    
    public new static Result<T> Fail(List<string> messages)
    {
        return new Result<T>
        {
            Succeeded = false,
            Messages = messages
        };
    }
    
    public static ValueTask<Result<T>> SuccessAsync(T data)
    {
        return ValueTask.FromResult(Success(data));
    }
    
    public static ValueTask<Result<T>> SuccessAsync(T data, string message)
    {
        return ValueTask.FromResult(Success(data, message));
    }
    
    public static ValueTask<Result<T>> SuccessAsync(T data, List<string> messages)
    {
        return ValueTask.FromResult(Success(data, messages));
    }
    
    public new static ValueTask<Result<T>> FailAsync(string message)
    {
        return ValueTask.FromResult(Fail(message));
    }
    
    public new static ValueTask<Result<T>> FailAsync(List<string> messages)
    {
        return ValueTask.FromResult(Fail(messages));
    }
    
    public static implicit operator Result<T>(T data)
    {
        return Success(data);
    }
    
    public static implicit operator Result<T>(string message)
    {
        return Fail(message);
    }
    
    public static implicit operator Result<T>(List<string> messages)
    {
        return Fail(messages);
    }
} 