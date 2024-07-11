using api.Models;

namespace api.Interfaces;

public interface IApplicationUserPostRepository
{
    Task<List<Post>> GetUserPosts(ApplicationUser user);
}