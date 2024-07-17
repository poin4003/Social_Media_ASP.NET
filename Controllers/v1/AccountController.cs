using api.Dtos.Account;
using api.Helpers.ApiResponseObject;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers.v1;

[Route("api/v1/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly SignInManager<ApplicationUser> _signInManager;
    public AccountController(UserManager<ApplicationUser> userManager, ITokenService tokenService, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _signInManager = signInManager;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {        
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == loginDto.Email.ToLower());

        if (user == null)
        {
            var unauthorizedReponse = new ApiResponseObject<ApplicationUser>
            {
                Message = "Failed: Invalid Email!"
            };
            return Unauthorized(unauthorizedReponse);
        }
        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (!result.Succeeded)
        {
            var unauthorizedReponse = new ApiResponseObject<ApplicationUser>
            {
                Message = "Failed: Email does not exits or password incorrect!"
            };
            return Unauthorized(unauthorizedReponse);
        } 

        var response = new ApiResponseObject<NewUserDto>
        {
            Record = new NewUserDto
            {
                UserName = user.UserName,
                Email = user.Email,
                Token = await _tokenService.CreateToken(user)
            },
            Message = "Success: Login!"
        };

        return Ok(response);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try 
        {
            var appUser = new ApplicationUser 
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
                BirthDay = registerDto.BirthDay,
            };

            var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);

            if (createdUser.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                if (roleResult.Succeeded)
                {
                    var response = new ApiResponseObject<NewUserDto>
                    {
                        Record = new NewUserDto
                        {
                            UserName = appUser.UserName,
                            Email = appUser.Email,
                            Token = await _tokenService.CreateToken(appUser)
                        },
                        Message = "Success: Account created!"
                    };
                    return Ok(response);
                }
                else 
                {
                    return StatusCode(500, roleResult.Errors);
                }
            }
            else 
            {
                return StatusCode(500, createdUser.Errors);
            }
        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }
}