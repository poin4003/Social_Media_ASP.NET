using System.Net.NetworkInformation;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class FriendRepository : IFriendRepository
{
    private ApplicationDBContext _context;
    public FriendRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<Friend> CreateFriendAsync(Friend friendModel)
    {
        await _context.Friends.AddAsync(friendModel);
        await _context.SaveChangesAsync();
        return friendModel;
    }

    public async Task<Friend> DeleteFriendAsync(ApplicationUser applicationUser, string friendId)
    {
        var friendModel = await _context.Friends.FirstOrDefaultAsync(x => x.ApplicationUserId == applicationUser.Id 
        && x.FriendId == friendId);
        if (friendModel == null)
            return null;
        
        _context.Friends.Remove(friendModel);
        await _context.SaveChangesAsync();
        return friendModel;
    }

    public async Task<List<ApplicationUser>> GetUserFriends(ApplicationUser user)
    {
        return await _context.Friends.Where(friend => friend.ApplicationUserId == user.Id)
        .Select(friend => new ApplicationUser
        {
            Id = friend.FriendId,
            Email = friend.FriendUser.Email,
            UserName = friend.FriendUser.UserName,
            BirthDay = friend.FriendUser.BirthDay,
            AccessTime = friend.FriendUser.AccessTime
        }).ToListAsync();
    }
}