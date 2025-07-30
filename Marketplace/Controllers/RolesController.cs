using AutoMapper;
using Marketplace.API.DTOs;
using Marketplace.Domain.Entities;
using Marketplace.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly MarketplaceDbContext _context;
    private readonly IMapper _mapper;
    public RolesController(MarketplaceDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<Role>>> GetAllRoles()
    {
        var roles = await _context.Roles.ToListAsync();
        return Ok(roles);
    }
    
    [HttpGet("api/[controller]/name/{name}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Role>> GetRoleByName(string name)
    {
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == name);
        if (role == null) return NotFound();
        return Ok(role);
    }
    [HttpGet("api/[controller]/id/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Role>> GetRoleById(Guid id)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role == null) return NotFound();
        return Ok(role);
    }
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Role>> CreateRole(RoleCreateDto dto)
    {
        var role = _mapper.Map<Role>(dto);
        _context.Roles.Add(role);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetRoleById), new { id = role.Id }, role);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, RoleCreateDto dto)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role == null) return NotFound();

        _mapper.Map(dto, role);
        await _context.SaveChangesAsync();
        return Ok(role);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role == null) return NotFound();

        _context.Roles.Remove(role);
        await _context.SaveChangesAsync();
        return NoContent();
    }

}
