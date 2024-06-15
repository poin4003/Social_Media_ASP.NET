using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models;

public partial class User
{
    public User()
    {
        Posts = new HashSet<Post>();
    }
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
    public int Id { get; set; }
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;
    [Column(TypeName = "datetime")]
    public DateTime BirthDay { get; set; }
    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
