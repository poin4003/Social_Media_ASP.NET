using api.Models;

namespace api.Interfaces;

public interface ITokenService
{
    Task<string> CreateToken(ApplicationUser user);
}