using api.Extenstions;
using api.Helpers.ApiResponseObject;
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
            return BadRequest("Failed: NameIdentitifer claims is missing!");

        var appUser = await _userManager.FindByIdAsync(applicationUserId);

        if (appUser == null) 
        {
            var notFoundResponse = new ApiResponseObjectWithPaging<ApplicationUser> 
            {
                Record = [],
                Message = "Failed: User not found!"
            };
            return BadRequest(notFoundResponse);
        } 

        var friends = await _friendRepository.GetUserFriends(appUser);

        var response = new ApiResponseObjectWithPaging<ApplicationUser>
        {
            Record = friends,
            Message = "Success: Get friend list!"
        };

        return Ok(response);
    }

    [HttpPost]
    [Route("{friendId:Guid}")]
    [Authorize]
    public async Task<IActionResult> AddFriend([FromRoute] string friendId)
    {
        var applicationUserId = User.GetId();
        var appUser = await _userManager.FindByIdAsync(applicationUserId);
        var friend = await _userManager.FindByIdAsync(friendId);

        if (appUser == null) 
        {
            var badRequestResponse = new ApiResponseObject<ApplicationUser>
            {
                Message = "Failed: User does not exist!"
            };
            return BadRequest(badRequestResponse);
        }
        if (friend == null) 
        {
            var badRequestResponse = new ApiResponseObject<ApplicationUser>
            {
                Message = "Failed: Friend (user) does not exist!"
            };
            return BadRequest(badRequestResponse);
        }

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
            var response = new ApiResponseObject<Friend>
            {
                Message = "Success: Friend created!"
            };
            return Ok(response);
        }
    }

    [HttpDelete]
    [Route("{friendId:Guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteFriend([FromRoute] string friendId)
    {
        var applicationUserId = User.GetId();
        var appUser = await _userManager.FindByIdAsync(applicationUserId);

        if (appUser == null) 
        {
            var badRequestResponse = new ApiResponseObject<ApplicationUser>
            {
                Message = "Failed: User does not exist!"
            };
            return BadRequest(badRequestResponse);
        }

        var userFriend = await _friendRepository.GetUserFriends(appUser);

        var filteredFriend = userFriend.Where(s => s.Id == friendId).ToList();

        if (filteredFriend.Count() == 1)
        {
            await _friendRepository.DeleteFriendAsync(appUser, friendId);
        }
        else 
        {
            var badRequestResponse = new ApiResponseObject<Friend>
            {
                Message = "Failed: Friend (user) is not in your friendList"
            };
            return BadRequest(badRequestResponse);
        }

        var response = new ApiResponseObject<Friend>
        {
            Message = "Success: Friend deleted!"
        };

        return Ok(response);
    }
}
