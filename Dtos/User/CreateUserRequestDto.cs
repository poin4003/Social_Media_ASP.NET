
namespace api.Dtos.User;

public class CreateUserRequestDto
{
    public string Name { get; set; } = string.Empty;
    public DateTime BirthDay { get; set; }
}