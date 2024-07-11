using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models;

[Table("ApplicationUserPosts")]
public class ApplicationUserPost
{
    public string ApplicationUserId { get; set; } = string.Empty;
    public string PostId { get; set; } = string.Empty;
    public ApplicationUser? ApplicationUser { get; set; } = null;
    public Post? Post { get; set; } = null; 
}