using System.ComponentModel.DataAnnotations;

namespace Marketplace.API.DTOs;

public class RoleCreateDto
{
    [Required]
    public string Name { get; set; }
}
