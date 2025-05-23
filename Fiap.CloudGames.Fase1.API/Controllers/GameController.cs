using Fiap.CloudGames.Fase1.API.Controllers.Base;
using Fiap.CloudGames.Fase1.Application.DTOs;
using Fiap.CloudGames.Fase1.Application.Interfaces;
using Fiap.CloudGames.Fase1.Infrastructure.LogService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fiap.CloudGames.Fase1.API.Controllers;

[ApiController]
[Route("games")]
[Produces("application/json")]
[Consumes("application/json")]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]

public class GameController : CustomControllerBase<GameController>
{
    private readonly IGameService _gameService;
    private readonly ILogService<GameController> _logger;

    public GameController(IGameService gameService, ILogService<GameController> logger) : base(logger)
    {
        _gameService = gameService;
        _logger = logger;
    }

    /// <summary> Adiciona um jogo ao catálogo </summary>
    /// <remarks>Requer permissão de Admin</remarks>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateGameDto dto)
    {
        var game = await _gameService.CreateAsync(dto);
        return Ok(game);
    }

    /// <summary> Listagem dos jogos </summary>
    [HttpGet]
    [Authorize(Roles = "User,Admin")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var games = await _gameService.GetAllAsync();
            return HandleResult(games);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary> Aquisição de um jogo do catálogo </summary>
    [HttpPost("{gameId}/acquire")]
    [Authorize(Roles = "User,Admin")]
    public async Task<IActionResult> Acquire(Guid gameId)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.Name)!);
        await _gameService.AcquireGameAsync(userId, gameId);
        return NoContent();
    }

    /// <summary> Biblioteca de jogos adquiridos </summary>
    [HttpGet("my-library")]
    [Authorize(Roles = "User,Admin")]
    public async Task<IActionResult> GetMyGames()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.Name)!);
        var games = await _gameService.GetUserGamesAsync(userId);
        return Ok(games);
    }
}