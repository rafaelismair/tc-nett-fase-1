using Fiap.CloudGames.Fase1.API.Controllers.Base;
using Fiap.CloudGames.Fase1.Application.DTOs.Games;
using Fiap.CloudGames.Fase1.Application.DTOs.Shared;
using Fiap.CloudGames.Fase1.Application.Interfaces;
using Fiap.CloudGames.Fase1.Infrastructure.LogService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prometheus;
using System.Diagnostics;
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

    private static readonly Counter GameRequestsTotal = Metrics
        .CreateCounter("cloudgames_games_requests_total", "Total de requisições para os endpoints de jogo", new[] { "action", "status" });

    private static readonly Histogram GameRequestsDuration = Metrics
        .CreateHistogram("cloudgames_games_request_duration_seconds", "Duração das requisições para endpoints de jogo", new[] { "action" });

    public GameController(IGameService gameService, ILogService<GameController> logger) : base(logger)
    {
        _gameService = gameService;
        _logger = logger;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateGameDto dto)
    {
        const string action = "create";
        using (GameRequestsDuration.WithLabels(action).NewTimer())
        {
            try
            {
                var result = await _gameService.CreateAsync(dto);
                GameRequestsTotal.WithLabels(action, result.Success ? "success" : "error").Inc();
                return result.Success ? HandleResult(result) : HandleError(result.Error.StatusCode, result.Error.ErrorMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex}, Erro ao criar jogo");
                GameRequestsTotal.WithLabels(action, "exception").Inc();
                return HandleException(ex, "Erro inesperado ao criar jogo.");
            }
        }
    }

    [HttpGet]
    [Authorize(Roles = "User,Admin")]
    public async Task<IActionResult> GetAll([FromQuery] PaginationDto pagination)
    {
        const string action = "get_all";
        using (GameRequestsDuration.WithLabels(action).NewTimer())
        {
            try
            {
                var result = await _gameService.GetAllAsync(pagination);
                GameRequestsTotal.WithLabels(action, result.Success ? "success" : "error").Inc();
                return result.Success ? HandleResult(result) : HandleError(result.Error.StatusCode, result.Error.ErrorMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex} Erro ao listar jogos");
                GameRequestsTotal.WithLabels(action, "exception").Inc();
                return HandleException(ex, "Erro inesperado ao listar jogos.");
            }
        }
    }

    [HttpGet("{gameId}")]
    [Authorize(Roles = "User,Admin")]
    public async Task<IActionResult> GetById(Guid gameId)
    {
        const string action = "get_by_id";
        using (GameRequestsDuration.WithLabels(action).NewTimer())
        {
            try
            {
                var result = await _gameService.GetByIdAsync(gameId);
                GameRequestsTotal.WithLabels(action, result.Success ? "success" : "error").Inc();
                return result.Success ? HandleResult(result) : HandleError(result.Error.StatusCode, result.Error.ErrorMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex} Erro ao buscar jogo por ID");
                GameRequestsTotal.WithLabels(action, "exception").Inc();
                return HandleException(ex, "Erro inesperado ao buscar jogo.");
            }
        }
    }

    [HttpDelete("{gameId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RemoveById(Guid gameId)
    {
        const string action = "remove_by_id";
        using (GameRequestsDuration.WithLabels(action).NewTimer())
        {
            try
            {
                var result = await _gameService.RemoveGameAsync(gameId);
                GameRequestsTotal.WithLabels(action, result.Success ? "success" : "error").Inc();
                return result.Success ? HandleResult(result) : HandleError(result.Error.StatusCode, result.Error.ErrorMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex} Erro ao remover jogo");
                GameRequestsTotal.WithLabels(action, "exception").Inc();
                return HandleException(ex, "Erro inesperado ao remover jogo.");
            }
        }
    }

    [HttpPut("{gameId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update([FromBody] CreateGameDto dto, Guid gameId)
    {
        const string action = "update";
        using (GameRequestsDuration.WithLabels(action).NewTimer())
        {
            try
            {
                var result = await _gameService.UpdateAsync(dto, gameId);
                GameRequestsTotal.WithLabels(action, result.Success ? "success" : "error").Inc();
                return result.Success ? HandleResult(result) : HandleError(result.Error.StatusCode, result.Error.ErrorMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex} Erro ao atualizar jogo");
                GameRequestsTotal.WithLabels(action, "exception").Inc();
                return HandleException(ex, "Erro inesperado ao atualizar jogo.");
            }
        }
    }

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
