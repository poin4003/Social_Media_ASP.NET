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
    public DbSet<ApplicationUserPost> ApplicationUserPosts { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUserPost>()
            .HasKey(aup => new { aup.ApplicationUserId, aup.PostId });

        builder.Entity<ApplicationUserPost>()
            .HasOne(aup => aup.ApplicationUser)
            .WithMany(au => au.ApplicationUserPosts)
            .HasForeignKey(aup => aup.ApplicationUserId);

        builder.Entity<ApplicationUserPost>()
            .HasOne(aup => aup.Post)
            .WithMany(p => p.ApplicationUserPosts)
            .HasForeignKey(aup => aup.PostId);

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