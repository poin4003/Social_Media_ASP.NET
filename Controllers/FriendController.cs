using api.Extenstions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/[Controller]")]
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
        var email = User.GetEmail();

        if (string.IsNullOrEmpty(email))
        {
            return BadRequest("Email claims is missing!");
        }

        var appUser = await _userManager.FindByEmailAsync(email);
        if (appUser == null)
        {
            return NotFound("User not found!");
        }

        var friends = await _friendRepository.GetUserFriends(appUser);
        return Ok(friends);
    }

    [HttpPost]
    [Route("{friendId}")]
    [Authorize]
    public async Task<IActionResult> AddFriend([FromRoute] string friendId)
    {
        var email = User.GetEmail();
        var appUser = await _userManager.FindByEmailAsync(email);
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

        await _friendRepository.CreateAsync(friendModel);

        if (friendModel == null)
        {
            return StatusCode(500, "Could not create");
        }
        else 
        {
            return Created();
        }
    }
}