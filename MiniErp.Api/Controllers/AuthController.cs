using Microsoft.AspNetCore.Mvc;
using MiniErp.Api.Contracts.Auth;
using MiniErp.Infrastructure.Persistence;
using MiniErp.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;

namespace MiniErp.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly JwtTokenService _jwtService;

    public AuthController(AppDbContext context, JwtTokenService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthLoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new { message = "Email and password are required" });
        }

        var user = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u =>
                u.Email == request.Email &&
                u.Password == request.Password
            );

        if (user == null)
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }

        var token = _jwtService.GenerateToken(user);

        var response = new AuthLoginResponse
        {
            AccessToken = token,
            ExpiresIn = 3600,
            User = new AuthUserResponse
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role.Name
            }
        };

        return Ok(response);
    }
}
