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
    public async Task<IActionResult> GetUserPost()
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

        var friends = await _friendRepository.GetFriends(appUser);
        return Ok(friends);
    }

    // [HttpPost]
    // [Authorize]
    // public async Task<IActionResult> AddApplicationUserPost()
    // {

    // }
}