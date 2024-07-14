using api.Models;

namespace api.Interfaces;

public interface IFriendRepository
{
    Task<List<ApplicationUser>> GetFriends(ApplicationUser user);
    // Task<List<ApplicationUser>> CreateFriend(string ApplicationUserId, string FriendId);
}