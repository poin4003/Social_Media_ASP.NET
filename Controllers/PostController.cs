using api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using api.Mappers;
using api.Dtos.Posts;
using api.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostController : ControllerBase
{
    private readonly IPostRepository _postRepository;
    public PostController(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PostQueryObject query)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var posts = await _postRepository.GetAllAsync(query);

        var postDto = posts.Select(p => p.ToPostDto());

        return Ok(postDto);
    } 

    [HttpGet]
    [Route("{id:Guid}")]
    [Authorize]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var post = await _postRepository.GetByIdAsync(id);

        if (post == null) 
            return NotFound("Post not found!");

        return Ok(post.ToPostDto());
    }

    [HttpPost]
    [Route("{userId:Guid}")]
    public async Task<IActionResult> Create([FromRoute] string userId,
    [FromBody] CreatePostRequestDto postDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var postModel = postDto.ToPostFromCreateDto(userId);
        await _postRepository.CreateAsync(postModel);
        return CreatedAtAction(nameof(GetById), new { id = postModel.Id }, postModel.ToPostDto());
    }
    
    [HttpPut]
    [Route("{id:Guid}")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdatePostRequestDto postDto) 
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var post = await _postRepository.UpdateAsync(id, postDto.ToPostFromUpdateDto());

        if (post == null) 
            return NotFound("Post not found!");

        return Ok(post.ToPostDto());
    }

    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var postModel = await _postRepository.DeleteAsync(id);

        if (postModel == null)
            return NotFound("Post not found!");

        return NoContent();
    }
}
