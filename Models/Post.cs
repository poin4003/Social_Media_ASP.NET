using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models;

public partial class Post
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    [Required]
    [StringLength(50)]
    public string Title { get; set; } = string.Empty;
    [Required]
    public string Content {get; set; } = string.Empty;
    public string? MediaUrl { get; set; }
    [Required]
    public DateTime CreateAt { get; set; } = DateTime.Now;
    [Required]
    public int LikesCount { get; set; } = 0;
    [Required]
    [StringLength(50)]
    public string? Privacy { get; set; } = "public";
    public string? Location { get; set; }
    public virtual ICollection<Post> Posts { get;} = new List<Post>();
}
