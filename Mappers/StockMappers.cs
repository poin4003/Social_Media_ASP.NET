using api.Dtos.User;
using api.Models;

namespace api.Mappers;

public static class UserMappers
{
    public static UserDto ToUserDto(this User userModel)
    {
        return new UserDto
        {
            Id = userModel.Id,
            Name = userModel.Name,
            BirthDay = userModel.BirthDay
        };
    }
}