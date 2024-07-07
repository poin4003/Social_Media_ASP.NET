using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace api.Models;

public partial class ApplicationUser : IdentityUser
{
    // public ApplicationUser()
    // {
    //     Posts = new HashSet<Post>();
    // } 
    [Column(TypeName = "datetime")]
    public DateTime BirthDay { get; set; } = DateTime.Today;
    [Required]
    public int AccessTime {get; set; } = 0;
    // public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
