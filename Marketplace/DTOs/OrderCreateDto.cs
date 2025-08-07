using System.ComponentModel.DataAnnotations;

namespace Marketplace.API.DTOs;

public class OrderCreateDto
{
    [Required]
    public Guid ProductId { get; set; }
    [Required]
    [Range(1,double.MaxValue)]
    public int Quantity { get; set; }
}
