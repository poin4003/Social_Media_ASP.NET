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
using api.Utils.ApiResponseMethod;

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
        {
            var badRequestResponse = ApiResponseMethod.ToApiResponseObject<PostDto>(ModelState);
            return BadRequest(badRequestResponse);
        }

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
        {
            var badRequestResponse = ApiResponseMethod.ToApiResponseObject<PostDto>(ModelState);
            return BadRequest(badRequestResponse);
        }

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
            Message = "Success: Get post!"
        };

        return Ok(response);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreatePostRequestDto postDto)
    {
        if (!ModelState.IsValid)
        {
            var badRequestResponse = ApiResponseMethod.ToApiResponseObject<PostDto>(ModelState);
            return BadRequest(badRequestResponse);
        }

        var userId = User.GetId();

        var postModel = postDto.ToPostFromCreateDto(userId);
        await _postRepository.CreateAsync(postModel);

        var response = new ApiResponseObject<PostDto>
        {
            Record = postModel.ToPostDto(),
            Message = "Success: Post created!"
        };

        return CreatedAtAction(nameof(GetById), new { id = postModel.Id }, response);
    }
    
    [HttpPut]
    [Route("{id:Guid}")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdatePostRequestDto postDto) 
    {
        if (!ModelState.IsValid)
        {
            var badRequestResponse = ApiResponseMethod.ToApiResponseObject<PostDto>(ModelState);
            return BadRequest(badRequestResponse);
        }

        var postModel = await _postRepository.UpdateAsync(id, postDto.ToPostFromUpdateDto());

        if (postModel == null) 
        {
            var badRequestResponse = new ApiResponseObject<PostDto>
            {
                Message = "Failed: Post does not exist!"
            };
            return BadRequest(badRequestResponse);
        }

        var response = new ApiResponseObject<PostDto>
        {
            Record = postModel.ToPostDto(),
            Message = "Success: Post updated!"
        };

        return Ok(response);
    }

    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        if (!ModelState.IsValid)
        {
            var badRequestResponse = ApiResponseMethod.ToApiResponseObject<PostDto>(ModelState);
            return BadRequest(badRequestResponse);
        }

        var postModel = await _postRepository.DeleteAsync(id);

        if (postModel == null)
        {
            var badRequestResponse = new ApiResponseObject<PostDto>
            {
                Message = "Failed: Post does not exist!"
            };
            return BadRequest(badRequestResponse);
        }

        var response = new ApiResponseObject<PostDto>
        {
            Message = "Success: Post deleted!"
        };

        return Ok(response);
    }
}
