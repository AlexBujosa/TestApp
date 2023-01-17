using System.ComponentModel.DataAnnotations;

namespace EndPointProject.Dto;


public class UserDto
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}