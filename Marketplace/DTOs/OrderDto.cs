namespace Marketplace.API.DTOs;

public class OrderDto
{
    public Guid Id { get; set; }
    public string ProductName { get; set; }
    public string SellerName { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime CreatedAt { get; set; }
}
