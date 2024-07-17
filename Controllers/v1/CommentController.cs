using api.Dtos.Comments;
using api.Extenstions;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using api.Helpers.ApiResponseObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers.v1;

[Route("api/v1/[Controller]")]
[ApiController]
public class CommentController : ControllerBase
{
    private readonly ICommentRepository _commentReposity;
    private readonly IPostRepository _postRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    public CommentController(ICommentRepository commentRepository, IPostRepository postRepository, 
    UserManager<ApplicationUser> userManager) 
    {
        _commentReposity = commentRepository;
        _postRepository = postRepository;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] CommentQueryObject query) 
    {
        var comments = await _commentReposity.GetAllAsync(query);

        var totalCount = comments.Count;
        var skipNumber = (query.PageNumber - 1) * query.PageSize;
        var commentDtos = comments.Select(comment => comment.ToCommentDto()).ToList();

        var response = new ApiResponseObjectWithPaging<CommentDto>
        {
            Record = commentDtos,
            Meta = new PaginationMeta
            {
                TotalCount = totalCount,
                PageSize = query.PageSize,
                PageNumber = query.PageNumber,
                TotalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize)
            },
            Message = "Success: Get comment list!"
        };

        return Ok(response);
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {     
        var comment = await _commentReposity.GetByIdAsync(id);
        
        if (comment == null) 
        {
            var notFoundResponse = new ApiResponseObject<CommentDto>
            {
                Message = "Failed: Comment not found!"
            };
            return NotFound(notFoundResponse);
        }    

        var response = new ApiResponseObject<CommentDto>
        {
            Record = comment.ToCommentDto(),
            Message = "Success: Get comment!"
        };

        return Ok(response);
    }

    [HttpPost]
    [Route("{postId:Guid}")]
    [Authorize]
    public async Task<IActionResult> Create([FromRoute] string postId,
    [FromBody] CreateCommentRequestDto commentDto)
    {
        var existingPost = await _postRepository.PostExists(postId);

        if (!existingPost) 
        {
            var BadRequestResponse = new ApiResponseObject<CommentDto>
            {
                Message = "Failed: Post does not exist!"
            };
            return BadRequest(BadRequestResponse);
        }    

        var applicationUserId = User.GetId();
        var existingAppUser = await _userManager.FindByIdAsync(applicationUserId);

        if (existingAppUser == null) 
        {
            var BadRequestResponse = new ApiResponseObject<CommentDto>
            {
                Message = "Failed: User does not exist!"
            };
            return BadRequest(BadRequestResponse);
        }
        
        var commentModel = commentDto.ToCommentFromCreate(postId, existingAppUser.Id);
        await _commentReposity.CreateAsync(commentModel);
        
        var response = new ApiResponseObject<CommentDto>
        {
            Record = commentModel.ToCommentDto(),
            Message = "Success: Comment created!" 
        };

        return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, response);
    }

    [HttpPut]
    [Route("{id:Guid}")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateCommentRequestDto commentDto)
    {
        var commentModel = await _commentReposity.UpdateAsync(id, commentDto.ToCommentFromUpdate());

        if (commentModel == null)
        {
            var badRequestResponse = new ApiResponseObject<CommentDto>
            {
                Message = "Failed: Comment does not exist!"
            };
            return BadRequest(badRequestResponse);
        }

        var response = new ApiResponseObject<CommentDto>
        {
            Record = commentModel.ToCommentDto(),
            Message = "Success: Comment updated!"
        };

        return Ok(response);
    }

    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        var commentModel = await _commentReposity.DeleteAsync(id);

        if (commentModel == null)
        {
            var badRequestResponse = new ApiResponseObject<CommentDto>
            {
                Message = "Failed: Comment does not exist!"
            };
            return BadRequest(badRequestResponse);
        }

        var response = new ApiResponseObject<CommentDto>
        {
            Message = "Success: Comment deleted!"
        };

        return Ok(response);
    }
}