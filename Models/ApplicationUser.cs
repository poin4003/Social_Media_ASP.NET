using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace api.Models;

public class ApplicationUser : IdentityUser
{
    [Column(TypeName = "datetime")]
    public DateTime BirthDay { get; set; } = DateTime.Today;
    [Required]
    public int AccessTime {get; set; } = 0;
    public virtual ICollection<Friend> Friends { get; set; } = new List<Friend>();
    public virtual ICollection<Friend> FriendOf { get; set; } = new List<Friend>();
}
