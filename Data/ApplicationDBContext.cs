using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data;

public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
    : base(options)
    {
        
    }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Friend> Friends { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Friend>()
            .HasKey(friend => new { friend.ApplicationUserId, friend.FriendId });

        builder.Entity<Friend>()
            .HasOne(friend => friend.ApplicationUser)
            .WithMany(au => au.Friends)
            .HasForeignKey(friend => friend.ApplicationUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Friend>()
            .HasOne(friend => friend.FriendUser)
            .WithMany(au => au.FriendOf)
            .HasForeignKey(friend => friend.FriendId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Comment>()
            .HasOne(c => c.Post)
            .WithMany(p => p.Comments)
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Comment>()
            .HasOne(c => c.ApplicationUser)
            .WithMany()
            .HasForeignKey(c => c.ApplicationUserId)
            .OnDelete(DeleteBehavior.Restrict);

        List<IdentityRole> roles = new List<IdentityRole>
        {
            new IdentityRole
            {
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new IdentityRole
            {
                Name = "User",
                NormalizedName = "USER"
            }
        };
        builder.Entity<IdentityRole>().HasData(roles);
        
    }
}