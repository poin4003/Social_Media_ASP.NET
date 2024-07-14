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

    public async Task<Friend> CreateAsync(Friend friendModel)
    {
        await _context.Friends.AddAsync(friendModel);
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