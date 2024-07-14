using api.Dtos.Comments;
using api.Models;

namespace api.Mappers;

public static class CommentMapper
{
    public static CommentDto ToCommentDto(this Comment commentModel)
    {
        return new CommentDto
        {
            Id = commentModel.Id,
            PostId = commentModel.PostId,
            ApplicationUserId = commentModel.ApplicationUserId,
            Content = commentModel.Content,
            CreateAt = commentModel.CreateAt,
            LikesCount = commentModel.LikesCount,
        };
    }

    public static Comment ToCommentFromCreate(this CreateCommentRequestDto commentDto, string postId, string applicationUserId) 
    {
        return new Comment 
        {
            PostId = postId,
            ApplicationUserId = applicationUserId,
            Content = commentDto.Content
        };
    }

    public static Comment ToCommentFromUpdate(this UpdateCommentRequestDto commentDto)
    {
        return new Comment
        {
            Content = commentDto.Content,
            LikesCount = commentDto.LikesCount,
        };
    }
}