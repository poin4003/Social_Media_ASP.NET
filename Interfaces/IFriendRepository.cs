using api.Models;

namespace api.Interfaces;

public interface IFriendRepository
{
    Task<List<ApplicationUser>> GetUserFriends(ApplicationUser user);
    Task<Friend> CreateFriendAsync(Friend friendModel);
    Task<Friend> DeleteFriendAsync(ApplicationUser applicationUser, string friendId);
}