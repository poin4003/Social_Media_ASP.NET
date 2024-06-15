using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data;

public class ApplicationDBContext : DbContext 
{
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
    : base(options)
    {
        
    }

    public DbSet<User> Users { get; set; }

    public DbSet<Post> Posts { get; set; }

}