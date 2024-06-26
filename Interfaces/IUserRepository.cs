
using api.Dtos.User;
using api.Models;

namespace api.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task<User> CreateAsync(User userModel);
    Task<User?> UpdateAsync(int id, UpdateUserRequestDto userDto);
    Task<User?> DeleteAsync(int id);
    Task<bool> UserExists(int id);
}