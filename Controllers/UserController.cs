using api.Data;
using api.Dtos.User;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
   private readonly ApplicationDBContext _context;
   public UserController(ApplicationDBContext context)
   {
        _context = context;
   } 

   [HttpGet]
   public async Task<IActionResult> GetAll() 
   {
      var users = await _context.Users.ToListAsync();

      var userDto = users.Select(s => s.ToUserDto());

      return Ok(users);
   }

   [HttpGet("{id}")]
   public async Task<IActionResult> GetById([FromRoute] int id)
   {
      var user = await _context.Users.FindAsync(id);

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
      await _context.Users.AddAsync(userModel);
      await _context.SaveChangesAsync();
      return CreatedAtAction(nameof(GetById), new { id = userModel.Id }, userModel.ToUserDto());
   }

   [HttpPut]
   [Route("{id}")]
   public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateUserRequestDto userDto)
   {
      var userModel = await _context.Users.FirstOrDefaultAsync(p => p.Id == id);

      if (userModel == null) 
      {
         return NotFound();
      }

      userModel.Name = userDto.Name;
      userModel.BirthDay = userDto.BirthDay;

      await _context.SaveChangesAsync();

      return Ok(userModel.ToUserDto());
   }

   [HttpDelete]
   [Route("{id}")]
   public async Task<IActionResult> Delete([FromRoute] int id)
   {
      var userModel = await _context.Users.FirstOrDefaultAsync(p => p.Id == id);

      if (userModel == null)
      {
         return NotFound();
      }

      _context.Users.Remove(userModel);

      await _context.SaveChangesAsync();

      return NoContent();
   }
}
