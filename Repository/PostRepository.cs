using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Mvc;
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

    public async Task<List<Post>> GetAllAsync() 
    {
        return await _context.Posts.ToListAsync();
    }

    public async Task<Post?> GetByIdAsync(int id)
    {
        var post = await _context.Posts.FindAsync(id);
        return post;
    }
}