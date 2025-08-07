using Marketplace.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Marketplace.API.DTOs;

public class OrderStatusUpdateDto
{
    [Required]
    public OrderStatus Status { get; set; }
}
