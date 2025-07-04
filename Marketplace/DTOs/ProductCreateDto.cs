namespace Marketplace.API.DTOs;

public class ProductCreateDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
