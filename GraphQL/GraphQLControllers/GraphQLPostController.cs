using api.Dtos.Posts;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;
using GraphQL.AspNet.Interfaces.Controllers;
using Microsoft.AspNetCore.Identity;

namespace api.GraphQL.Controllers;

public class GraphQLPostController : GraphController
{
    private readonly IPostRepository _postRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    public GraphQLPostController(IPostRepository postRepository,
    UserManager<ApplicationUser> userManager)
    {
        _postRepository = postRepository;
        _userManager = userManager;
    }

    [QueryRoot("post", typeof(PostDto))]
    public async Task<IGraphActionResult> RetrivePost(string id)
    {
        var post = await _postRepository.GetByIdAsync(id);
        return Ok(post.ToPostDto());
    }

    [QueryRoot("posts", typeof(List<PostDto>))]
    public async Task<IGraphActionResult> RetriveAllPosts(PostQueryObject postQueryObject)
    {
        var posts = await _postRepository.GetAllAsync(postQueryObject);
        var postDto = posts.Select(p => p.ToPostDto()).ToList();
        return Ok(postDto);
    }

    [MutationRoot("createPost", typeof(PostDto))]
    public async Task<IGraphActionResult> CreatePost(CreatePostRequestDto postDto, string userId)
    {
        var userModel = await _userManager.FindByIdAsync(userId);

        if (userModel == null) 
        {
            return BadRequest("Failed: User does not exist");
        }

        var postModel = postDto.ToPostFromCreateDto(userId);
        await _postRepository.CreateAsync(postModel);
        return Ok(postModel.ToPostDto());
    }

    [MutationRoot("updatePost", typeof(Post))]
    public async Task<IGraphActionResult> UpdatePost(UpdatePostRequestDto postDto, string id)
    {
        var postModel = await _postRepository.UpdateAsync(id, postDto.ToPostFromUpdateDto());

        if (postModel == null) 
        {
            return BadRequest("Failed: Post does not exist!");
        }

        return Ok(postModel);
    }

    [MutationRoot("deletePost", typeof(Post))]
    public async Task<IGraphActionResult> DeletePost(string id)
    {
        var postModel = await _postRepository.DeleteAsync(id);

        if (postModel == null)
        {
            return BadRequest("Failed: Post does not exist!");
        }

        return Ok(postModel);
    }
}

