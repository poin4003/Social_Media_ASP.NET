using api.Dtos.Comments;

namespace api.Dtos.Posts;

public class PostDto
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Content {get; set; } = string.Empty;
    public string MediaUrl { get; set; } = string.Empty;
    public DateTime CreateAt { get; set; } = DateTime.Today;
    public int LikesCount { get; set; } = 0;
    public string Privacy { get; set; } = "Public";
    public string Location { get; set; } = string.Empty;
    public string ApplicationUserId { get; set; } = string.Empty;
    public List<CommentDto> Comments { get; set; } = [];
}