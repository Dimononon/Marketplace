using AutoMapper;
using AutoMapper.QueryableExtensions;
using Marketplace.API.DTOs;
using Marketplace.Domain.Entities;
using Marketplace.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Marketplace.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly MarketplaceDbContext _context;
    private readonly IMapper _mapper;
    public ProductsController(MarketplaceDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] ProductQueryParameters query)
    {
        var productsQuery = _context.Products
            .Include(p => p.Seller)
            .AsQueryable();

        if (!string.IsNullOrEmpty(query.Search))
        {
            productsQuery = productsQuery.Where(p =>
                p.Title.Contains(query.Search) || p.Description.Contains(query.Search));
        }

        if (query.MinPrice.HasValue)
        {
            productsQuery = productsQuery.Where(p => p.Price >= query.MinPrice.Value);
        }

        if (query.MaxPrice.HasValue)
        {
            productsQuery = productsQuery.Where(p => p.Price <= query.MaxPrice.Value);
        }

        var products = await productsQuery
        .ProjectTo<ProductReadDto>(_mapper.ConfigurationProvider)
        .ToListAsync();

        return Ok(products);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductReadDto>> GetById(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();
        return Ok(_mapper.Map<ProductReadDto>(product));
    }
    [HttpPost]
    [Authorize(Roles = "Seller")]
    public async Task<ActionResult<ProductReadDto>> Create(ProductCreateDto dto)
    {
        var product = _mapper.Map<Product>(dto);
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();
        product.SellerId = Guid.Parse(userId);
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, _mapper.Map<ProductReadDto>(product));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Seller")]
    public async Task<IActionResult> Update(Guid id, ProductCreateDto dto)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id && p.SellerId == userId);

        if (product == null)
            return NotFound("Product not found or you are not the owner.");

        _mapper.Map(dto, product);
        await _context.SaveChangesAsync();
        return Ok(dto);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Seller")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id && p.SellerId == userId);
        if (product == null)
            return NotFound("Product not found or you are not the owner.");

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
