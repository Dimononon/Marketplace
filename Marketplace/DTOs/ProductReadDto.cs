namespace Marketplace.API.DTOs;

public class ProductReadDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
}
