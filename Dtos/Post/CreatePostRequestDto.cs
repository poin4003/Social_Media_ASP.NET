
using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Posts;

public class CreatePostRequestDto
{
    [Required]
    [MinLength(5, ErrorMessage = "Title must be more than 5 characters!")]
    [MaxLength(280, ErrorMessage = "Title must be letter than 280 character!")]
    public string Title { get; set; } = string.Empty;
    [Required]
    [MinLength(5, ErrorMessage = "Content must be more than 5 characters!")]
    [MaxLength(280, ErrorMessage = "Content must be letter than 280 character!")]
    public string Content {get; set; } = string.Empty;
}