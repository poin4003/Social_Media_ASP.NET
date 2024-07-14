using api.Data;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class CommentRepository : ICommentRepository
{
    private readonly ApplicationDBContext _context;
    public CommentRepository(ApplicationDBContext context)
    {
        _context = context;
    }
    public Task<bool> CommentExist(string id)
    {
        return _context.Comments.AnyAsync(comment => comment.Id == id);
    }

    public async Task<Comment> CreateAsync(Comment commentModel)
    {
        await _context.Comments.AddAsync(commentModel);
        await _context.SaveChangesAsync();
        return commentModel;
    }

    public async Task<Comment> DeleteAsync(string id)
    {
        var commentModel = await _context.Comments.FirstOrDefaultAsync(comment => comment.Id == id);

        if (commentModel == null)
            return null;

        _context.Comments.Remove(commentModel);
        await _context.SaveChangesAsync();
        return commentModel;
    }

    public async Task<List<Comment>> GetAllAsync(CommentQueryObject query)
    {
        var comments = _context.Comments.Include(a => a.ApplicationUser).AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Content))
        {
            comments = comments.Where(comment => comment.Content == query.Content);
        }

        if (query.CreateAt.HasValue)
        {
            var createAt = query.CreateAt.Value.Date;
            comments = comments.Where(comment => comment.CreateAt == createAt);
        }

        if (!string.IsNullOrWhiteSpace(query.SortBy))
        {
            switch (query.SortBy.ToLower())
            {
                case "id":
                    comments = query.IsDecsending ? comments.OrderByDescending(comment => comment.Id) : comments.OrderBy(comment => comment.Id);
                    break;
                case "content":
                    comments = query.IsDecsending ? comments.OrderByDescending(comment => comment.Content) : comments.OrderBy(comment => comment.Content);
                    break;
                case "createat":
                    comments = query.IsDecsending ? comments.OrderByDescending(comment => comment.CreateAt) : comments.OrderBy(comment => comment.CreateAt);
                    break;
                case "likescount":
                    comments = query.IsDecsending ? comments.OrderByDescending(comment => comment.LikesCount) : comments.OrderBy(comment => comment.LikesCount);
                    break;
                case "postid":
                    comments = query.IsDecsending ? comments.OrderByDescending(comment => comment.PostId) : comments.OrderBy(comment => comment.PostId);
                    break;
                case "applicationuserid":
                    comments = query.IsDecsending ? comments.OrderByDescending(comment => comment.ApplicationUserId) : comments.OrderBy(comment => comment.ApplicationUserId);
                    break;
                default:
                    break;
            }
        }

        var skipNumber = (query.PageNumber - 1) * query.PageSize;

        return await comments.Skip(skipNumber).Take(query.PageSize).ToListAsync();
    }

    public async Task<Comment> GetByIdAsync(string id)
    {
        return await _context.Comments.Include(a => a.ApplicationUser).FirstOrDefaultAsync(comment => comment.Id == id);
    }

    public async Task<Comment> UpdateAsync(string id, Comment commentModel)
    {
        var existingComment = await _context.Comments.FirstOrDefaultAsync(comment => comment.Id == id);

        if (existingComment == null)
            return null;
        
        existingComment.Content = commentModel.Content;
        existingComment.LikesCount = commentModel.LikesCount;

        await _context.SaveChangesAsync();

        return existingComment;
    }
}