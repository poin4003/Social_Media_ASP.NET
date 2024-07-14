using api.Dtos.Comments;
using api.Extenstions;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/[Controller]")]
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
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var comment = await _commentReposity.GetAllAsync(query);

        var commentDto = comment.Select(comment => comment.ToCommentDto());

        return Ok(commentDto);
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var comment = await _commentReposity.GetByIdAsync(id);

        if (comment == null)
            return NotFound();

        return Ok(comment.ToCommentDto());
    }

    [HttpPost]
    [Route("{postId:Guid}")]
    [Authorize]
    public async Task<IActionResult> Create([FromRoute] string postId,
    [FromBody] CreateCommentRequestDto commentDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!await _postRepository.PostExists(postId))
            return BadRequest("User does not exist!");

        var email = User.GetEmail();
        var appUser = await _userManager.FindByEmailAsync(email);
        

        var commentModel = commentDto.ToCommentFromCreate(postId, appUser.Id);
        await _commentReposity.CreateAsync(commentModel);
        return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.ToCommentDto());
    }

    [HttpPut]
    [Route("{id:Guid}")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateCommentRequestDto commentDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var commentModel = await _commentReposity.UpdateAsync(id, commentDto.ToCommentFromUpdate());

        if (commentModel == null)
            return NotFound();

        return Ok(commentModel.ToCommentDto());
    }

    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var commentModel = await _commentReposity.DeleteAsync(id);

        if (commentModel == null)
            return NotFound();

        return NoContent();
    }
}