
using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Posts;

public class UpdatePostRequestDto
{
    [Required]
    [MinLength(5, ErrorMessage = "Title must be more than 5 characters!")]
    [MaxLength(280, ErrorMessage = "Title must be letter than 280 character!")]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MinLength(5, ErrorMessage = "Content must be more than 5 characters!")]
    [MaxLength(280, ErrorMessage = "Content must be letter than 280 character!")]
    public string Content {get; set; } = string.Empty;

    [Required]
    public DateTime CreateAt { get; set; } = DateTime.Today;

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "LikeCount must be a non-negative number!")]
    public int LikesCount { get; set; } = 0;

    [Url(ErrorMessage = "MediaUrl must be a valid URL")]
    [StringLength(2048, ErrorMessage = "MediaUrl must be less than 2048 characters!")]
    public string MediaUrl { get; set; } = string.Empty;

    [Required]
    [EnumDataType(typeof(PrivacyOptions), ErrorMessage = "Privacy must be Public, Private or FriendOnly")]
    public string Privacy { get; set; } = "Public";

    [StringLength(100, ErrorMessage = "Location must be less than 100 character!")]    
    public string Location { get; set; } = string.Empty;
}