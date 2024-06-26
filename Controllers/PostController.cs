using api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using api.Mappers;
using api.Dtos.Posts;
using api.Dtos.User;

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
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var posts = await _postRepository.GetAllAsync();

        var postDto = posts.Select(p => p.ToPostDto());

        return Ok(postDto);
    } 

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var post = await _postRepository.GetByIdAsync(id);

        if (post == null) 
        {
            return NotFound();
        }

        return Ok(post.ToPostDto());
    }

    [HttpPost]
    [Route("{userId:int}")]
    public async Task<IActionResult> Create([FromRoute] int userId, CreatePostRequestDto postDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!await _userRepository.UserExists(userId)) 
        {
            return BadRequest("User does not exits!");
        }

        var postModel = postDto.ToPostFromCreate(userId);
        await _postRepository.CreateAsync(postModel);
        return CreatedAtAction(nameof(GetById), new { id = postModel.Id }, postModel.ToPostDto());
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdatePostRequestDto updateDto) 
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var post = await _postRepository.UpdateAsync(id, updateDto.ToPostFromUpdate());

        if (post == null) 
        {
            return NotFound("Post not found!");
        }

        return Ok(post.ToPostDto());
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var postModel = await _postRepository.DeleteAsync(id);

        if (postModel == null)
        {
            return NotFound("Post does not exist!");
        }

        return Ok(postModel);
    }
}
