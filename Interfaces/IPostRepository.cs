using api.Helpers;
using api.Models;

namespace api.Interfaces;

public interface IPostRepository
{
    Task<List<Post>> GetAllAsync(PostQueryObject query);
    Task<Post?> GetByIdAsync(string id);
    Task<Post> CreateAsync(Post postModel);
    Task<Post?> UpdateAsync(string id, Post postModel);
    Task<Post?> DeleteAsync(string id);
    Task<bool> PostExists(string id);
}