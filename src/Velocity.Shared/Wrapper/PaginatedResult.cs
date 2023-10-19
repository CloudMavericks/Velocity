namespace Velocity.Shared.Wrapper;

public class PaginatedResult<T> : Result
{
    public PaginatedResult(List<T> data)
    {
        Data = data;
    }
    
    public List<T> Data { get; }
    
    public int PageNumber { get; set; }
    
    public int PageSize { get; set; }
    
    public long TotalCount { get; set; }
    
    public long TotalPages { get; set; }
    
    public bool HasPreviousPage => PageNumber > 1;
    
    public bool HasNextPage => PageNumber < TotalPages;
    
    internal PaginatedResult(List<T> data, int pageNumber, int pageSize, long totalCount, bool succeeded = true, List<string> messages = null)
    {
        Data = data;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        Succeeded = succeeded;
        Messages = messages ?? new List<string>();
    }
    
    public static PaginatedResult<T> Success(List<T> data, int page, int pageSize, long totalCount)
    {
        return new PaginatedResult<T>(data, page, pageSize, totalCount);
    }
    
    public static PaginatedResult<T> Success(List<T> data, int page, int pageSize, long totalCount, string message)
    {
        return new PaginatedResult<T>(data, page, pageSize, totalCount)
        {
            Messages = new List<string> { message }
        };
    }
    
    public static PaginatedResult<T> Success(List<T> data, int page, int pageSize, long totalCount, List<string> messages)
    {
        return new PaginatedResult<T>(data, page, pageSize, totalCount)
        {
            Messages = messages
        };
    }
    
    public static PaginatedResult<T> Failure(List<string> errors)
    {
        return new PaginatedResult<T>(default!, 0, 0, 0)
        {
            Messages = errors,
            Succeeded = false
        };
    }
    
    public static PaginatedResult<T> Failure(string error)
    {
        return new PaginatedResult<T>(default!, 0, 0, 0)
        {
            Messages = new List<string> { error },
            Succeeded = false
        };
    }
    
    public static PaginatedResult<T> Failure()
    {
        return new PaginatedResult<T>(default!, 0, 0, 0)
        {
            Messages = new List<string> { "An error occurred" },
            Succeeded = false
        };
    }
    
    public static implicit operator PaginatedResult<T>(string error) => Failure(error);
    
    public static implicit operator PaginatedResult<T>(List<string> errors) => Failure(errors);
}