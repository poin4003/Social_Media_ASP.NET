using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models;

[Table("Friends")]
public class Friend
{
    public string ApplicationUserId { get; set; } = string.Empty;
    public string FriendId { get; set; } = string.Empty;
    public virtual ApplicationUser ApplicationUser { get; set; } = null!;
    public virtual ApplicationUser FriendUser { get; set; } = null!;
}