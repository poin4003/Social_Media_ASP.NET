using api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using api.Mappers;
using api.Dtos.Posts;
using api.Helpers;
using Microsoft.AspNetCore.Authorization;
using api.Models;
using Microsoft.AspNetCore.Identity;
using api.Extenstions;
using api.Helpers.ApiResponseObject;

namespace api.Controllers.v1;

[Route("api/v1/[controller]")]
[ApiController]
public class PostController : ControllerBase
{
    private readonly IPostRepository _postRepository;
    private readonly UserManager<ApplicationUser> _userManger;
    public PostController(IPostRepository postRepository, UserManager<ApplicationUser> userManger)
    {
        _postRepository = postRepository;
        _userManger = userManger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PostQueryObject query)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var posts = await _postRepository.GetAllAsync(query);

        var totalCount = posts.Count;
        var skipNumber = (query.PageNumber - 1) * query.PageSize;
        var postDtos = posts.Select(post => post.ToPostDto()).ToList();

        var response = new ApiResponseObjectWithPaging<PostDto>
        {
            Record = postDtos,
            Meta = new PaginationMeta
            {
                TotalCount = totalCount,
                PageSize = query.PageSize,
                PageNumber = query.PageNumber,
                TotalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize)
            },
            Message = "Success: Get post list!"
        };

        return Ok(response);
    } 

    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var post = await _postRepository.GetByIdAsync(id);

        if (post == null) 
        {
            var notFoundResponse = new ApiResponseObject<PostDto>
            {
                Record = null,
                Message = "Failed: Post not found!"
            };
            return NotFound(notFoundResponse);
        }    

        var response = new ApiResponseObject<PostDto>
        {
            Record = post.ToPostDto(),
            Message = "Success: Get comment list!"
        };

        return Ok(response);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreatePostRequestDto postDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = User.GetId();

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
