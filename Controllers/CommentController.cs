using api.Dtos.Comments;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/[Controller]")]
[ApiController]
public class CommentController : ControllerBase
{
    private readonly ICommentRepository _commentReposity;
    private readonly IPostRepository _postRepository;
    public CommentController(ICommentRepository commentRepository, IPostRepository postRepository) 
    {
        _commentReposity = commentRepository;
        _postRepository = postRepository;
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
    public async Task<IActionResult> Create([FromRoute] string postId, [FromBody] CreateCommentRequestDto commentDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!await _postRepository.PostExists(postId))
            return BadRequest("User does not exist!");

        var commentModel = commentDto.ToCommentFromCreate(postId);
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