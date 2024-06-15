using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models;

public partial class Post
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
    public int Id { get; set; }
    [Required]
    [StringLength(50)]
    public string Title { get; set; } = string.Empty;
    [Required]
    public string Content {get; set; } = string.Empty;
    public int? UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public virtual User IdUserNavigation { get; set; } = null!;
}
