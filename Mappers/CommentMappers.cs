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
            // UserId = commentModel.UserId,
            Content = commentModel.Content,
            CreateAt = commentModel.CreateAt,
            LikesCount = commentModel.LikesCount,
        };
    }

    public static Comment ToCommentFromCreate(this CreateCommentRequestDto commentDto, string postId) 
    {
        return new Comment 
        {
            PostId = postId,
            // UserId = userId,
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