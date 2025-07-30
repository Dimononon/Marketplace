namespace Marketplace.API.DTOs;

public class OrderCreateDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
