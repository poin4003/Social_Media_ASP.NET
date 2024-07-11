using api.Extenstions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/[Controller]")]
[ApiController]
public class ApplicationUserPostController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IPostRepository _postRepository;
    private readonly IApplicationUserPostRepository _applicationUserPostRepository;
    public ApplicationUserPostController(UserManager<ApplicationUser> userManager, 
    IPostRepository postRepository, IApplicationUserPostRepository applicationUserPostRepository)
    {
        _userManager = userManager;
        _postRepository = postRepository;
        _applicationUserPostRepository = applicationUserPostRepository;
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

        var applicationUserPost = await _applicationUserPostRepository.GetUserPosts(appUser);
        return Ok(applicationUserPost);
    }
}