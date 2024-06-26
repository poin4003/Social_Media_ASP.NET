

using System.ComponentModel.DataAnnotations;

namespace api.Dtos.User;

public class UpdateUserRequestDto
{
    [Required]
    [MinLength(10, ErrorMessage = "Name must be over 10 character!")]
    public string Name { get; set; } = string.Empty;
    [Required]
    public DateTime BirthDay { get; set; }
}