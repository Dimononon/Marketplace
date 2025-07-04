using AutoMapper;
using Marketplace.API.DTOs;
using Marketplace.Domain.Entities;
using Marketplace.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : Controller
{
    private readonly MarketplaceDbContext _context;
    private readonly IMapper _mapper;
    public ProductsController(MarketplaceDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductReadDto>>> GetAll()
    {
        var products = await _context.Products.ToListAsync();
        return Ok(_mapper.Map<IEnumerable<ProductReadDto>>(products));
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductReadDto>> GetById(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();
        return Ok(_mapper.Map<ProductReadDto>(product));
    }
    [HttpPost]
    public async Task<ActionResult<ProductReadDto>> Create(ProductCreateDto dto)
    {
        var product = _mapper.Map<Product>(dto);
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, _mapper.Map<ProductReadDto>(product));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, ProductCreateDto dto)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();

        _mapper.Map(dto, product);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
