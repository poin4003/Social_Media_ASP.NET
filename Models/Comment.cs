using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models;

[Table("Comments")]
public class Comment 
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    [Required]
    public string PostId { get; set; } = string.Empty;
    [Required]
    public string ApplicationUserId { get; set; } = string.Empty;
    [Required]
    public string Content { get; set; } = string.Empty;
    [Required]
    public DateTime CreateAt { get; set; } = DateTime.Now;
    [Required]
    public int LikesCount { get; set; } = 0;
    [ForeignKey(nameof(PostId))]
    public virtual Post Post{ get; set; } = null!;
    [ForeignKey(nameof(ApplicationUserId))]
    public virtual ApplicationUser ApplicationUser { get; set;} = null!;
}


