using Microsoft.AspNetCore.Mvc;
using ArithmeticChat.DTOs;
using ArithmeticChat.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace ArithmeticChat.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;
    public AuthController(IAuthService auth) { _auth = auth; }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto, CancellationToken ct)
    {
        var id = await _auth.RegisterAsync(dto, ct);
        return CreatedAtAction(null, new { id });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto dto, CancellationToken ct)
    {
        var res = await _auth.LoginAsync(dto, ct);
        return Ok(res);
    }

    [AllowAnonymous]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto, CancellationToken ct)
    {
        var sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(sub, out var userId)) return Unauthorized();
        await _auth.ChangePasswordAsync(userId, dto, ct);
        return NoContent();
    }

    [AllowAnonymous]
    [HttpGet("me")]
    public IActionResult Me() => Ok(new { Username = User.Identity?.Name });
}
