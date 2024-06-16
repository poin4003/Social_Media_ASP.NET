

namespace api.Dtos.User;

public class UpdateUserRequestDto
{
    public string Name { get; set; } = string.Empty;
    public DateTime BirthDay { get; set; }
}