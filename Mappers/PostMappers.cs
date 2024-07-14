using api.Dtos.Posts;
using api.Models;

namespace api.Mappers;

public static class PostMapper
{
    public static PostDto ToPostDto(this Post postModel) 
    {
        return new PostDto
        {
            Id = postModel.Id,
            Title = postModel.Title,
            Content = postModel.Content,
            MediaUrl = postModel.MediaUrl,
            CreateAt = postModel.CreateAt,
            LikesCount = postModel.LikesCount,
            Privacy = postModel.Privacy,
            Location = postModel.Location,
            ApplicationUserId = postModel.ApplicationUserId,
            Comments = postModel.Comments.Select(comment => comment.ToCommentDto()).ToList(),
        };
    }

    public static Post ToPostFromCreateDto(this CreatePostRequestDto postDto, string applicationUserId) 
    {
        return new Post
        {
            Title = postDto.Title,
            Content = postDto.Content,
            MediaUrl = postDto.MediaUrl,
            Privacy = postDto.Privacy,
            Location = postDto.Location,
            ApplicationUserId = applicationUserId,
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