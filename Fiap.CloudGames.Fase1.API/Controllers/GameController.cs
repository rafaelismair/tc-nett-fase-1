using Fiap.CloudGames.Fase1.API.Controllers.Base;
using Fiap.CloudGames.Fase1.Application.DTOs.Games;
using Fiap.CloudGames.Fase1.Application.DTOs.Shared;
using Fiap.CloudGames.Fase1.Application.Interfaces;
using Fiap.CloudGames.Fase1.Infrastructure.LogService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Fiap.CloudGames.Fase1.API.Controllers;

[ApiController]
[Route("games/[action]")]
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
        var result = await _gameService.CreateAsync(dto);

        if (!result.Success)
        {
            return HandleError(result.Error.StatusCode, result.Error.ErrorMessage);
        }

        return HandleResult(result);
    }

    /// <summary> Listagem dos jogos </summary>
    [HttpGet]
    [Authorize(Roles = "User,Admin")]
    public async Task<IActionResult> GetAll(PaginationDto pagination)
    {
        var result = await _gameService.GetAllAsync(pagination);

        if (!result.Success)
        {
            return HandleError(result.Error.StatusCode, result.Error.ErrorMessage);
        }

        return HandleResult(result);
    }

    /// <summary> Listar detalhes de um jogo específico </summary>
    [HttpGet("{gameId}")]
    [Authorize(Roles = "User,Admin")]
    public async Task<IActionResult> GetById(Guid gameId)
    {
        var result = await _gameService.GetByIdAsync(gameId);

        if (!result.Success)
        {
            return HandleError(result.Error.StatusCode, result.Error.ErrorMessage);
        }

        return HandleResult(result);
    }

    /// <summary> Remove um jogo específico </summary>
    [HttpGet("{gameId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RemoveById(Guid gameId)
    {
        var result = await _gameService.RemoveGameAsync(gameId);

        if (!result.Success)
        {
            return HandleError(result.Error.StatusCode, result.Error.ErrorMessage);
        }

        return HandleResult(result);
    }

    /// <summary> Atualiza um jogo específico </summary>
    [HttpGet("{gameId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(CreateGameDto dto, Guid gameId)
    {
        var result = await _gameService.UpdateAsync(dto, gameId);

        if (!result.Success)
        {
            return HandleError(result.Error.StatusCode, result.Error.ErrorMessage);
        }

        return HandleResult(result);
    }

    ///// <summary> Aquisição de um jogo do catálogo </summary>
    //[HttpPost("{gameId}/acquire")]
    //[Authorize(Roles = "User,Admin")]
    //public async Task<IActionResult> Acquire(Guid gameId)
    //{
    //    var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.Name)!);
    //    await _gameService.AcquireGameAsync(userId, gameId);
    //    return NoContent();
    //}

    ///// <summary> Biblioteca de jogos adquiridos </summary>
    //[HttpGet("my-library")]
    //[Authorize(Roles = "User,Admin")]
    //public async Task<IActionResult> GetMyGames()
    //{
    //    var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.Name)!);
    //    var games = await _gameService.(userId);
    //    return HandleResult(games);
    //}

    #region Private Methods
    private IActionResult HandleError(HttpStatusCode statusCode, string errorMessage)
    {
        return statusCode switch
        {
            HttpStatusCode.BadRequest => HandleBadRequest(errorMessage),
            HttpStatusCode.InternalServerError => HandleException(errorMessage),
            _ => StatusCode((int)statusCode, new { error = errorMessage })
        };
    }
    #endregion
}