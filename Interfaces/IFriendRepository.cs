using api.Models;

namespace api.Interfaces;

public interface IFriendRepository
{
    Task<List<ApplicationUser>> GetUserFriends(ApplicationUser user);
    Task<Friend> CreateAsync(Friend friendModel);
}