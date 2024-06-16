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
   public IActionResult GetAll() 
   {
      var users = _context.Users.ToList()
         .Select(s => s.ToUserDto());

      return Ok(users);
   }

   [HttpGet("{id}")]
   public IActionResult GetById([FromRoute] int id)
   {
      var user = _context.Users.Find(id);

      if (user == null)
      {
         return NotFound();
      }

      return Ok(user.ToUserDto());
   }

   [HttpPost]
   public IActionResult Create([FromBody] CreateUserRequestDto userDto)
   {
      var userModel = userDto.ToUserFromCreateDTO();
      _context.Users.Add(userModel);
      _context.SaveChanges();
      return CreatedAtAction(nameof(GetById), new { id = userModel.Id }, userModel.ToUserDto());
   }

   [HttpPut]
   [Route("{id}")]
   public IActionResult Update([FromRoute] int id, [FromBody] UpdateUserRequestDto userDto)
   {
      var userModel = _context.Users.FirstOrDefault(p => p.Id == id);

      if (userModel == null) 
      {
         return NotFound();
      }

      userModel.Name = userDto.Name;
      userModel.BirthDay = userDto.BirthDay;

      _context.SaveChanges();

      return Ok(userModel.ToUserDto());
   }

   [HttpDelete]
   [Route("{id}")]
   public IActionResult Delete([FromRoute] int id)
   {
      var userModel = _context.Users.FirstOrDefault(p => p.Id == id);

      if (userModel == null)
      {
         return NotFound();
      }

      _context.Users.Remove(userModel);

      _context.SaveChanges();

      return NoContent();
   }
}
