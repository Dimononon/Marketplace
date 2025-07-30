using AutoMapper;
using Marketplace.API.DTOs;
using Marketplace.Application.Services;
using Marketplace.Domain.Entities;
using Marketplace.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace Marketplace.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly MarketplaceDbContext _context;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    public AuthController(MarketplaceDbContext context, ITokenService tokenService, IMapper mapper)
    {
        _context = context;
        _tokenService = tokenService;
        _mapper = mapper;
    }
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<ProductReadDto>>> GetAll()
    {
        var users = await _context.Users
        .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
        .ToListAsync();

        var userDtos = _mapper.Map<List<UserDto>>(users);
        return Ok(userDtos);
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterDto dto)
    {
        if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            return BadRequest("Email already exists");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = ComputeHash(dto.Password),

        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var buyerRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Buyer");
        if (buyerRole == null)
            return StatusCode(500, "Default role 'Buyer' not found in database");

        var userRole = new UserRole
        {
            UserId = user.Id,
            RoleId = buyerRole.Id,
        };

        _context.UserRoles.Add(userRole);
        await _context.SaveChangesAsync();

        var userWithRoles = await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == user.Id);


        var token = _tokenService.CreateToken((userWithRoles != null) ? userWithRoles : user);


        return Ok(_mapper.Map<UserDto>(userWithRoles));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginDto dto)
    {
        var user = await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null || user.PasswordHash != ComputeHash(dto.Password))
            return Unauthorized();

        var token = _tokenService.CreateToken(user);

        return Ok(new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,

            Token = token
        });
    }
    [HttpPost("assign-role")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignRole([FromBody] UserAssignRoleDto dto)
    {
        var user = await _context.Users
            .Include(u => u.UserRoles)
            .FirstOrDefaultAsync(u => u.Id == dto.UserId);

        if (user == null)
            return NotFound("User not found");

        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == dto.RoleName);
        if (role == null)
            return NotFound("Role not found");

        var alreadyHasRole = user.UserRoles.Any(ur => ur.RoleId == role.Id);
        if (alreadyHasRole)
            return BadRequest("User already has this role");

        _context.UserRoles.Add(new UserRole
        {
            UserId = user.Id,
            RoleId = role.Id,
        });

        await _context.SaveChangesAsync();
        var userWithRoles = await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == user.Id);
        return Ok(_mapper.Map<UserDto>(userWithRoles));
    }
    private static string ComputeHash(string input)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToBase64String(bytes);
    }
}
