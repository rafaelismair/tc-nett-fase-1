using Fiap.CloudGames.Fase1.Application.DTOs;
using Fiap.CloudGames.Fase1.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.CloudGames.Fase1.API.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
    {
        var token = await _authService.RegisterAsync(dto);
        return Ok(new { token });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var token = await _authService.LoginAsync(dto);
        return Ok(new { token });
    }

    [HttpPost("register-admin")]
    public async Task<IActionResult> RegisterAdmin([FromBody] RegisterUserDto dto)
    {
        var token = await _authService.RegisterAsync(dto, isAdmin: true);
        return Ok(new { token });
    }
}
