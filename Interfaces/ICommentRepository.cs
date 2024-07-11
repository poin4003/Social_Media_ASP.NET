using api.Helpers;
using api.Models;

namespace api.Interfaces;

public interface ICommentRepository
{
    Task<List<Comment>> GetAllAsync(CommentQueryObject query);
    Task<Comment> GetByIdAsync(string id);
    Task<Comment> CreateAsync(Comment comment);
    Task<Comment> UpdateAsync(string id, Comment comment);
    Task<Comment> DeleteAsync(string id);
    Task<bool> CommentExist(string id);
}