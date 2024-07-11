using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Comments;

public class CreateCommentRequestDto
{
    [Required]
    [MaxLength(280, ErrorMessage = "Content must be letter than 280 character!")]
    public string Content { get; set; } = string.Empty;
}