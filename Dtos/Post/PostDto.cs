
using api.Models;

namespace api.Dtos.Posts;

public class PostDto
{
    public string? Id { get; set; }
    public string? Title { get; set; }
    public string? Content {get; set; }
    public string? MediaUrl { get; set; }
    public DateTime CreateAt { get; set; }
    public int LikesCount { get; set; }
    public string? Privacy { get; set; }
    public string? Location { get; set; }
    public List<Comment>? Comments { get; set; }
}