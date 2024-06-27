using api.Data;
using api.Dtos.User;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDBContext _context;
    public UserRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<User> CreateAsync(User userModel)
    {
       await _context.AddAsync(userModel);
       await _context.SaveChangesAsync();
       return userModel;
    }

    public async Task<User?> DeleteAsync(int id)
    {
        var userModel = await _context.Users.FirstOrDefaultAsync(p => p.Id == id);

        if (userModel == null)
        {
            return null;
        }

        _context.Users.Remove(userModel);
        await _context.SaveChangesAsync();
        return userModel;
    }

    public async Task<List<User>> GetAllAsync(QueryObject query)
    {
        var users = _context.Users.Include(p => p.Posts).AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Name))
        {
            users = users.Where(p => p.Name.Contains(query.Name));
        }

        if (query.BirthDay.HasValue)
        {
            var birthday = query.BirthDay.Value.Date;
            users = users.Where(p => p.BirthDay.Date == birthday);
        }

        return await users.ToListAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.Include(p => p.Posts).FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<User?> UpdateAsync(int id, UpdateUserRequestDto userDto)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(p => p.Id == id);

        if (existingUser == null)
        {
            return null;
        }

        existingUser.Name = userDto.Name;
        existingUser.BirthDay = userDto.BirthDay;

        await _context.SaveChangesAsync();

        return existingUser;
    }

    public Task<bool> UserExists(int id)
    {
        return _context.Users.AnyAsync(p => p.Id == id);
    }
}