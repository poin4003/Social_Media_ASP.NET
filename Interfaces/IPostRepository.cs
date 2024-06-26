
using api.Models;

namespace api.Interfaces;

public interface IPostRepository
{
    Task<List<Post>> GetAllAsync();
    Task<Post?> GetByIdAsync(int id);
    Task<Post> CreateAsync(Post postModel);
}