using api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using api.Mappers;
using api.Dtos.Posts;

namespace api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostController : ControllerBase
{
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;
    public PostController(IPostRepository postRepository, IUserRepository userRepository)
    {
        _postRepository = postRepository;
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var posts = await _postRepository.GetAllAsync();

        var postDto = posts.Select(p => p.ToPostDto());

        return Ok(postDto);
    } 

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var post = await _postRepository.GetByIdAsync(id);

        if (post == null) 
        {
            return NotFound();
        }

        return Ok(post.ToPostDto());
    }

    [HttpPost]
    [Route("{userId}")]
    public async Task<IActionResult> Create([FromRoute] int userId, CreatePostDto postDto)
    {
        if (!await _userRepository.UserExists(userId)) 
        {
            return BadRequest("User does not exits");
        }

        var postModel = postDto.ToPostFromCreate(userId);
        await _postRepository.CreateAsync(postModel);
        return CreatedAtAction(nameof(GetById), new { id = postModel}, postModel.ToPostDto());
    }
}
