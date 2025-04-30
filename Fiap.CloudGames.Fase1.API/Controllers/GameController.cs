using Fiap.CloudGames.Fase1.Application.DTOs;
using Fiap.CloudGames.Fase1.Application.Interfaces;
using Fiap.CloudGames.Fase1.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fiap.CloudGames.Fase1.API.Controllers;

[ApiController]
[Route("games")]
public class GameController : ControllerBase
{
    private readonly IGameService _gameService;

    public GameController(IGameService gameService)
    {
        _gameService = gameService;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateGameDto dto)
    {
        var game = await _gameService.CreateAsync(dto);
        return Ok(game);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var games = await _gameService.GetAllAsync();
        return Ok(games);
    }

    [HttpPost("{gameId}/acquire")]
    [Authorize(Roles = "User,Admin")]
    public async Task<IActionResult> Acquire(Guid gameId)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.Name)!);
        await _gameService.AcquireGameAsync(userId, gameId);
        return NoContent();
    }

    [HttpGet("my-library")]
    [Authorize(Roles = "User,Admin")]
    public async Task<IActionResult> GetMyGames()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.Name)!);
        var games = await _gameService.GetUserGamesAsync(userId);
        return Ok(games);
    }
}