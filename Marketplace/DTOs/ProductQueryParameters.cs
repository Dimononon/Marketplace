namespace Marketplace.API.DTOs;

public class ProductQueryParameters
{
    public string? Search { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }

}
