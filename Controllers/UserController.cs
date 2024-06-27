using api.Dtos.User;
using api.Helpers;
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
   public async Task<IActionResult> GetAll([FromQuery] QueryObject query) 
   {
      if (!ModelState.IsValid)
         return BadRequest(ModelState);

      var users = await _userRepo.GetAllAsync(query);

      var userDto = users.Select(s => s.ToUserDto());

      return Ok(users);
   }

   [HttpGet("{id:int}")]
   public async Task<IActionResult> GetById([FromRoute] int id)
   {
      if (!ModelState.IsValid)
         return BadRequest(ModelState);
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
      if (!ModelState.IsValid)
         return BadRequest(ModelState);

      var userModel = userDto.ToUserFromCreateDTO();
      await _userRepo.CreateAsync(userModel);
      return CreatedAtAction(nameof(GetById), new { id = userModel.Id }, userModel.ToUserDto());
   }

   [HttpPut]
   [Route("{id:int}")]
   public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateUserRequestDto userDto)
   {
      if (!ModelState.IsValid)
         return BadRequest(ModelState);

      var userModel = await _userRepo.UpdateAsync(id, userDto);

      if (userModel == null) 
      {
         return NotFound();
      }

      return Ok(userModel.ToUserDto());
   }

   [HttpDelete]
   [Route("{id:int}")]
   public async Task<IActionResult> Delete([FromRoute] int id)
   {
      if (!ModelState.IsValid)
         return BadRequest(ModelState);
         
      var userModel = await _userRepo.DeleteAsync(id);

      if (userModel == null)
      {
         return NotFound();
      }

      return NoContent();
   }
}
