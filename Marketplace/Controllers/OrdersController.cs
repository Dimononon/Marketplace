using AutoMapper;
using Marketplace.API.DTOs;
using Marketplace.Domain.Entities;
using Marketplace.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Marketplace.API.Controllers;

[ApiController]
[Route("api/Orders")]
[Authorize(Roles = "Buyer")]
public class OrdersController : ControllerBase
{
    private readonly MarketplaceDbContext _context;
    private readonly IMapper _mapper;

    public OrdersController(MarketplaceDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(OrderCreateDto dto)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var product = await _context.Products
            .Include(p => p.Seller)
            .FirstOrDefaultAsync(p => p.Id == dto.ProductId);

        if (product == null)
            return NotFound("Product not found");

        if (product.SellerId == userId)
            return BadRequest("You cannot buy your own product");

        if (dto.Quantity <= 0)
            return BadRequest("Quantity must be greater than 0");

        var order = new Order
        {
            Id = Guid.NewGuid(),
            ProductId = product.Id,
            BuyerId = userId,
            Quantity = dto.Quantity,
            TotalPrice = product.Price * dto.Quantity,
            CreatedAt = DateTime.UtcNow
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return Ok(_mapper.Map<OrderDto>(order));
    }

    [HttpGet("My")]
    public async Task<IActionResult> GetMyOrders()
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var orders = await _context.Orders
            .Where(o => o.BuyerId == userId)
            .Include(o => o.Product)
                .ThenInclude(p => p.Seller)
            .ToListAsync();

        var result = _mapper.Map<List<OrderDto>>(orders);
        return Ok(result);
    }
    [HttpGet("ForMyProducts")]
    [Authorize(Roles = "Seller")]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersForMyProducts()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var orders = await _context.Orders
            .Where(o => o.Product.SellerId == Guid.Parse(userId))
            .Include(o => o.Product)
                .ThenInclude(p => p.Seller)
            .ToListAsync();

        var result = _mapper.Map<List<OrderDto>>(orders);
        return Ok(result);
    }
    [HttpPut("{id}/status")]
    [Authorize(Roles = "Seller")]
    public async Task<IActionResult> UpdateOrderStatus(Guid id, [FromBody] OrderStatusUpdateDto dto)
    {
        var order = await _context.Orders
            .Include(o => o.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
            return NotFound("Order not found");

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (order.Product.SellerId.ToString() != userId)
            return Forbid("You can only change status for your own product orders");

        order.Status = dto.Status;
        await _context.SaveChangesAsync();

        return Ok("Order status updated");
    }
}
