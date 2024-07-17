namespace api.Helpers.ApiResponseObject;

public class ApiResponseObject<T> 
{
    public T Record { get; set; } = default!;
    public string Message { get; set; } = string.Empty;
}

public class ApiResponseObjectWithPaging<T>
{
    public List<T> Record { get; set; } = new List<T>();
    public PaginationMeta Meta { get; set; } = null!;
    public string Message { get; set; } = string.Empty;
}

public class PaginationMeta
{
    public int TotalCount { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public int TotalPages { get; set; }
}

