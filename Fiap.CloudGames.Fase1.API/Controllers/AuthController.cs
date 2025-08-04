using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prometheus;
using Fiap.CloudGames.Fase1.Application.Interfaces;
using Fiap.CloudGames.Fase1.API.Controllers.Base;
using Fiap.CloudGames.Fase1.Infrastructure.LogService.Interfaces;
using Fiap.CloudGames.Fase1.Application.DTOs.Auth;
using Microsoft.Extensions.Logging;

namespace Fiap.CloudGames.Fase1.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : CustomControllerBase<AuthController>
    {
        private readonly IAuthService _authService;

        // Prometheus metrics with labels for enhanced observability
        private static readonly Counter AuthCounter = Metrics
            .CreateCounter("cloudgames_auth_requests_total", "Total number of auth requests", new[] { "action", "status" });

        private static readonly Histogram AuthProcessingHistogram = Metrics
            .CreateHistogram("cloudgames_auth_processing_duration_seconds", "Duration of auth operations in seconds", new[] { "action" });

        public AuthController(IAuthService authService, ILogService<AuthController> logger)
            : base(logger)
        {
            _authService = authService;
        }

        /// <summary>
        /// Endpoint to register a new user.
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            var action = "register";
            using (AuthProcessingHistogram.WithLabels(action).NewTimer())
            {
                try
                {
                    var token = await _authService.RegisterAsync(dto);
                    AuthCounter.WithLabels(action, "success").Inc();
                    return HandleResult(token);
                }
                catch (Exception ex)
                {
                    AuthCounter.WithLabels(action, "error").Inc();
                    return HandleException(ex, "Erro ao registrar usuário.");
                }
            }
        }

        /// <summary>
        /// Endpoint to login a user.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var action = "login";
            using (AuthProcessingHistogram.WithLabels(action).NewTimer())
            {
                try
                {
                    var token = await _authService.LoginAsync(dto);
                    AuthCounter.WithLabels(action, "success").Inc();
                    return HandleResult(token);
                }
                catch (Exception ex)
                {
                    AuthCounter.WithLabels(action, "error").Inc();
                    return HandleException(ex, "Erro ao efetuar login.");
                }
            }
        }

        /// <summary>
        /// Endpoint to register an admin user. Requires admin role.
        /// </summary>
        [HttpPost("register-admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterUserDto dto)
        {
            var action = "register_admin";
            using (AuthProcessingHistogram.WithLabels(action).NewTimer())
            {
                try
                {
                    var token = await _authService.RegisterAsync(dto, isAdmin: true);
                    AuthCounter.WithLabels(action, "success").Inc();
                    return HandleResult(token);
                }
                catch (Exception ex)
                {
                    AuthCounter.WithLabels(action, "error").Inc();
                    return HandleException(ex, "Erro ao registrar administrador.");
                }
            }
        }
    }
}
