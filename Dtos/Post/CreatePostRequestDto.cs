using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Posts;

public enum PrivacyOptions
{
    Public,
    Private,
    FriendOnly
}

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

    [Url(ErrorMessage = "MediaUrl must be a valid URL")]
    [StringLength(2048, ErrorMessage = "MediaUrl must be less than 2048 characters!")]
    public string? MediaUrl { get; set; }

    [Required]
    [EnumDataType(typeof(PrivacyOptions), ErrorMessage = "Privacy must be Public, Private or FriendOnly")]
    public string? Privacy { get; set; }

    [StringLength(100, ErrorMessage = "Location must be less than 100 character!")]    
    public string? Location { get; set; }
}