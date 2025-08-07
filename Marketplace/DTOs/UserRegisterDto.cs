using System.ComponentModel.DataAnnotations;

namespace Marketplace.API.DTOs;

public class UserRegisterDto
{
    [Required]
    [MinLength(3)]
    public string Username { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [MinLength(6)]
    public string Password { get; set; }
    

}
