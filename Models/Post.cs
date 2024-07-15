using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models;

[Table("Posts")]
public partial class Post
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    [Required]
    [StringLength(50)]
    public string Title { get; set; } = string.Empty;
    [Required]
    public string Content {get; set; } = string.Empty;
    public string MediaUrl { get; set; } = string.Empty;
    [Required]
    public DateTime CreateAt { get; set; } = DateTime.Now;
    [Required]
    public int LikesCount { get; set; } = 0;
    [Required]
    [StringLength(50)]
    public string Privacy { get; set; } = "Public";
    public string Location { get; set; } = string.Empty;
    [Required]
    public string ApplicationUserId { get; set; } = string.Empty;
    [ForeignKey(nameof(ApplicationUserId))]
    public virtual ApplicationUser ApplicationUser { get; set; } = null!;
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
