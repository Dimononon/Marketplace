using Marketplace.Domain.Entities;

namespace Marketplace.API.DTOs;

public class OrderStatusUpdateDto
{
    public OrderStatus Status { get; set; }
}
