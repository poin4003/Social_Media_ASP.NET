namespace api.Dtos.Comments;

public class CommentDto
{
    public string? Id { get; set; }
    public string? PostId { get; set; }
    // public string? UserId { get; set; }
    public string? Content { get; set; }
    public DateTime? CreateAt { get; set; }
    public int? LikesCount { get; set; }
}