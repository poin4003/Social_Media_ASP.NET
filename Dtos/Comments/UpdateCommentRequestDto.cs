using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Comments;

public class UpdateCommentRequestDto
{
    [Required]
    [MaxLength(280, ErrorMessage = "Content must be letter than 280 character!")]
    public string Content { get; set; } = string.Empty;
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "LikeCount must be a non-negative number!")]
    public int LikesCount { get; set; }
}