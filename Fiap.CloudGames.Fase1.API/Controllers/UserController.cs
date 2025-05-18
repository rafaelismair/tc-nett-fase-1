using System.Security.Claims;
using Fiap.CloudGames.Fase1.Application.DTOs;
using Fiap.CloudGames.Fase1.Application.Interfaces;
using Fiap.CloudGames.Fase1.Application.Services;
using Fiap.CloudGames.Fase1.Infrastructure.LogService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.CloudGames.Fase1.API.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogService<UserController> _logger;

        public UserController(IUserService userService, ILogService<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }


        [HttpGet("{userId}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetUser(Guid userId) 
        {
            var user = await _userService.GetUserAsync(userId);

            return Ok(user); 
        }

        [HttpGet]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetMyUser()
        {
            try
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.Name)!);
                var user = await _userService.GetUserAsync(userId);

                return Ok(user);
            }
            catch (Exception ex) 
            { 
                return BadRequest(ex.Message);            
            }
           
        }

        [HttpGet]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpPatch]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Patch([FromBody] PatchUserDto dto)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.Name)!);
                await _userService.PutUserAsync(userId, dto);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
