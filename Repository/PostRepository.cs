using api.Data;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class PostRepository : IPostRepository
{
    private readonly ApplicationDBContext _context;

    public PostRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<Post> CreateAsync(Post postModel)
    {
        await _context.Posts.AddAsync(postModel);
        await _context.SaveChangesAsync();
        return postModel;
    }

    public async Task<List<Post>> GetAllAsync(PostQueryObject query) 
    {
        var posts = _context.Posts.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Title))
        {
            posts = posts.Where(post => post.Title.Contains(query.Title));
        }

        if (!string.IsNullOrWhiteSpace(query.Content))
        {
            posts = posts.Where(post => post.Content.Contains(query.Content));
        }

        if (query.CreateAt.HasValue)
        {
            var createAt = query.CreateAt.Value.Date;
            posts = posts.Where(post => post.CreateAt == createAt);
        }

        if (!string.IsNullOrEmpty(query.Privacy))
        {
            posts = posts.Where(post => post.Privacy == query.Privacy);
        }

        if (!string.IsNullOrEmpty(query.Location))
        {
            posts = posts.Where(post => post.Location.Contains(query.Location));
        }

        if (!string.IsNullOrEmpty(query.SortBy))
        {
            switch (query.SortBy.ToLower())
            {
                case "id":
                    posts = query.IsDecsending ? posts.OrderByDescending(post => post.Id) : posts.OrderBy(post => post.Id);
                    break;
                case "title":
                    posts = query.IsDecsending ? posts.OrderByDescending(post => post.Title) : posts.OrderBy(post => post.Title);
                    break;
                case "content":
                    posts = query.IsDecsending ? posts.OrderByDescending(post => post.Content) : posts.OrderBy(post => post.Content);
                    break;
                case "createat":
                    posts = query.IsDecsending ? posts.OrderByDescending(posts => posts.CreateAt) : posts.OrderBy(posts => posts.CreateAt);
                    break;
                case "privacy":
                    posts = query.IsDecsending ? posts.OrderByDescending(post => post.Privacy) : posts.OrderBy(post => post.Privacy);
                    break;
                case "location":
                    posts = query.IsDecsending ? posts.OrderByDescending(post => post.Location) : posts.OrderBy(post => post.Location);
                    break;
                case "likescount":
                    posts = query.IsDecsending ? posts.OrderByDescending(post => post.LikesCount) : posts.OrderBy(post => post.LikesCount);
                    break;
                case "applicationuserid":
                    posts = query.IsDecsending ? posts.OrderByDescending(post => post.ApplicationUser) : posts.OrderBy(post => post.ApplicationUser);
                    break;
                default: 
                    break;
            }
        }

        var skipNumber = (query.PageNumber - 1) * query.PageSize;

        return await posts.Skip(skipNumber).Take(query.PageSize).ToListAsync();
    }

    public async Task<Post?> GetByIdAsync(string id)
    {
        return await _context.Posts.Include(a => a.Comments).ThenInclude(a => a.ApplicationUser).FirstOrDefaultAsync(post => post.Id == id);
    }
    
    public async Task<Post?> DeleteAsync(string id)
    {
        var postModel = await _context.Posts.FirstOrDefaultAsync(post => post.Id == id);

        if (postModel == null) 
            return null;

        _context.Posts.Remove(postModel);
        await _context.SaveChangesAsync();
        return postModel;
    }

    public async Task<Post?> UpdateAsync(string id, Post postModel)
    {
        var existingPost = await _context.Posts.FirstOrDefaultAsync(post => post.Id == id);

        if (existingPost == null)
            return null;

        existingPost.Title = postModel.Title;
        existingPost.Content = postModel.Content;
        existingPost.CreateAt = postModel.CreateAt;
        existingPost.LikesCount = postModel.LikesCount;
        existingPost.MediaUrl = postModel.MediaUrl;
        existingPost.Privacy = postModel.Privacy;
        existingPost.Location = postModel.Location;

        await _context.SaveChangesAsync();

        return existingPost;
    }

    public Task<bool> PostExists(string id)
    {
        return _context.Posts.AnyAsync(post => post.Id == id);
    }
}