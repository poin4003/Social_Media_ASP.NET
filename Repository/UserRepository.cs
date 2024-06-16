using api.Data;
using api.Dtos.User;
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

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
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
}