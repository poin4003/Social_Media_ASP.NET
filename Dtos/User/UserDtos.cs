using api.Models;
namespace api.Dtos.User;

public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime BirthDay { get; set; }
}