namespace api.Dtos.Comments;

public class CommentDto
{
    public string Id { get; set; } = string.Empty;
    public string PostId { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreateAt { get; set; } = DateTime.Today;
    public int LikesCount { get; set; } = 0;
    public string CreatedBy { get; set; } = string.Empty;
}