using System.Net.NetworkInformation;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class ApplicationUserPostRepository : IApplicationUserPostRepository
{
    private ApplicationDBContext _context;
    public ApplicationUserPostRepository(ApplicationDBContext context)
    {
        _context = context;
    }
    public async Task<List<Post>> GetUserPosts(ApplicationUser user)
    {
        return await _context.ApplicationUserPosts.Where(aup => aup.ApplicationUserId == user.Id)
            .Select(post => new Post
            {
                Id = post.PostId,
                Title = post.Post.Title,
                Content = post.Post.Content,
                MediaUrl = post.Post.MediaUrl,
                CreateAt = post.Post.CreateAt,
                LikesCount = post.Post.LikesCount,
                Privacy = post.Post.Privacy,
                Location = post.Post.Location
            }).ToListAsync();
    }
}