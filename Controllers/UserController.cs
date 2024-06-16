using api.Dtos.User;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
   private readonly IUserRepository _userRepo;
   public UserController(IUserRepository userRepo)
   {
        _userRepo = userRepo;
   } 

   [HttpGet]
   public async Task<IActionResult> GetAll() 
   {
      var users = await _userRepo.GetAllAsync();

      var userDto = users.Select(s => s.ToUserDto());

      return Ok(users);
   }

   [HttpGet("{id}")]
   public async Task<IActionResult> GetById([FromRoute] int id)
   {
      var user = await _userRepo.GetByIdAsync(id);

      if (user == null)
      {
         return NotFound();
      }

      return Ok(user.ToUserDto());
   }

   [HttpPost]
   public async Task<IActionResult> Create([FromBody] CreateUserRequestDto userDto)
   {
      var userModel = userDto.ToUserFromCreateDTO();
      await _userRepo.CreateAsync(userModel);
      return CreatedAtAction(nameof(GetById), new { id = userModel.Id }, userModel.ToUserDto());
   }

   [HttpPut]
   [Route("{id}")]
   public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateUserRequestDto userDto)
   {
      var userModel = await _userRepo.UpdateAsync(id, userDto);

      if (userModel == null) 
      {
         return NotFound();
      }

      return Ok(userModel.ToUserDto());
   }

   [HttpDelete]
   [Route("{id}")]
   public async Task<IActionResult> Delete([FromRoute] int id)
   {
      var userModel = await _userRepo.DeleteAsync(id);

      if (userModel == null)
      {
         return NotFound();
      }

      return NoContent();
   }
}
