namespace api.Helpers;

public class PostQueryObject
{
    public string? Title { get; set; } = null;
    public string? Content { get; set; } = null;
    public DateTime? CreateAt { get; set; } = null;
    public string? Privacy { get; set; } = null;
    public string? Location { get; set; } = null;
    public string? SortBy { get; set; } = null;
    public bool IsDecsending { get; set; } = false;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set;} = 10;
}