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
            MediaUrl = postmodel.MediaUrl,
            CreateAt = postmodel.CreateAt,
            LikesCount = postmodel.LikesCount,
            Privacy = postmodel.Privacy,
            Location = postmodel.Location,
            // Comments = postmodel.Posts.Select(p => p.ToCommentDto().ToList()),
        };
    }

    public static Post ToPostFromCreateDto(this CreatePostRequestDto postDto) 
    {
        return new Post
        {
            Title = postDto.Title,
            Content = postDto.Content,
            MediaUrl = postDto.MediaUrl,
            Privacy = postDto.Privacy,
            Location = postDto.Location,
        };
    }

    public static Post ToPostFromUpdateDto(this UpdatePostRequestDto postDto)
    {
        return new Post
        {
            Title = postDto.Title,
            Content = postDto.Content,
            MediaUrl = postDto.MediaUrl,
            CreateAt = postDto.CreateAt,
            LikesCount = postDto.LikesCount,
            Privacy = postDto.Privacy,
            Location = postDto.Location,
        };
    }
}