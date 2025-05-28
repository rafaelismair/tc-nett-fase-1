using Fiap.CloudGames.Fase1.API.Controllers.Base;
using Fiap.CloudGames.Fase1.API.Middleware.Logging;
using Fiap.CloudGames.Fase1.Application.DTOs;
using Fiap.CloudGames.Fase1.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Fiap.CloudGames.Fase1.Infrastructure.LogService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Fiap.CloudGames.Fase1.API.Controllers;

[ApiController]
[Route("auth")]
[Produces("application/json")]
[Consumes("application/json")]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class AuthController : CustomControllerBase<AuthController>
{
    private readonly IAuthService _authService;
    private readonly ILogService<AuthController> _logger;

    public AuthController(IAuthService authService, ILogService<AuthController> logger) : base(logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary> Registro de um usuário </summary>
    [HttpPost("register")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
    {
        var token = await _authService.RegisterAsync(dto);
        return Ok(new { token });
    }

    /// <summary> Login de um usuário </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var token = await _authService.LoginAsync(dto);
        return Ok(new { token });
    }

    /// <summary> Registro de admins</summary>
    /// <remarks>Requer permissão de Admin</remarks>
    [HttpPost("register-admin")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> RegisterAdmin([FromBody] RegisterUserDto dto)
    {
        var token = await _authService.RegisterAsync(dto, isAdmin: true);
        return Ok(new { token });
    }
}
