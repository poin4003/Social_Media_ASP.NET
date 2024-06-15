using api.Data;
using api.Mappers;
using api.Models;
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
}
