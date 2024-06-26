using api.Dtos.Posts;
using api.Models;

namespace api.Mappers;

public static class PostMapper
{
    public static PostDto ToPostDto(this Post postmodel) 
    {
        return new PostDto
        {
            Id = postmodel.Id,
            Title = postmodel.Title,
            Content = postmodel.Content,
            UserId = postmodel.UserId
        };
    }

    public static Post ToPostFromCreate(this CreatePostRequestDto postDto, int userId) 
    {
        return new Post
        {
            Title = postDto.Title,
            Content = postDto.Content,
            UserId = userId
        };
    }

    public static Post ToPostFromUpdate(this UpdatePostRequestDto postDto)
    {
        return new Post
        {
            Title = postDto.Title,
            Content = postDto.Content
        };
    }
}