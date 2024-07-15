using api.Extenstions;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers.v1;

[Route("api/v1/[Controller]")]
[ApiController]
public class FriendController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IFriendRepository _friendRepository;
    public FriendController(UserManager<ApplicationUser> userManager, 
    IFriendRepository friendRepository)
    {
        _userManager = userManager;
        _friendRepository = friendRepository;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUserFriends()
    {
        var applicationUserId = User.GetId();

        if (string.IsNullOrEmpty(applicationUserId))
            return BadRequest("NameIdentitifer claims is missing!");

        var appUser = await _userManager.FindByIdAsync(applicationUserId);

        if (appUser == null) return BadRequest("User not found!");
      
        var friends = await _friendRepository.GetUserFriends(appUser);
        return Ok(friends);
    }

    [HttpPost]
    [Route("{friendId:Guid}")]
    [Authorize]
    public async Task<IActionResult> AddFriend([FromRoute] string friendId)
    {
        var applicationUserId = User.GetId();
        var appUser = await _userManager.FindByIdAsync(applicationUserId);
        var friend = await _userManager.FindByIdAsync(friendId);

        if (appUser == null) return BadRequest("User not found!");
        if (friend == null) return BadRequest("Friend (user) not found!");

        var userFriend = await _friendRepository.GetUserFriends(appUser);

        if (userFriend.Any(e => e.Id == friendId)) return BadRequest("Cannot add same friend to friendTable");

        var friendModel = new Friend 
        {
            ApplicationUserId = appUser.Id,
            FriendId = friendId,
        };

        await _friendRepository.CreateFriendAsync(friendModel);

        if (friendModel == null)
        {
            return StatusCode(500, "Could not create");
        }
        else 
        {
            return Created();
        }
    }

    [HttpDelete]
    [Route("{friendId:Guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteFriend([FromRoute] string friendId)
    {
        var applicationUserId = User.GetId();
        var appUser = await _userManager.FindByIdAsync(applicationUserId);

        if (appUser == null) return BadRequest("User not found!");

        var userFriend = await _friendRepository.GetUserFriends(appUser);

        var filteredFriend = userFriend.Where(s => s.Id == friendId).ToList();

        if (filteredFriend.Count() == 1)
        {
            await _friendRepository.DeleteFriendAsync(appUser, friendId);
        }
        else 
        {
            return BadRequest("Friend (user) is not in your friendList");
        }

        return NoContent();
    }
}