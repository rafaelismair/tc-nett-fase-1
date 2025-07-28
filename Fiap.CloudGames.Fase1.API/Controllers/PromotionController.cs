using Fiap.CloudGames.Fase1.API.Controllers.Base;
using Fiap.CloudGames.Fase1.Application.DTOs.Promotions;
using Fiap.CloudGames.Fase1.Application.DTOs.Shared;
using Fiap.CloudGames.Fase1.Application.Interfaces;
using Fiap.CloudGames.Fase1.Infrastructure.LogService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Fiap.CloudGames.Fase1.API.Controllers;

[ApiController]
[Route("promotion")]
[Produces("application/json")]
[Consumes("application/json")]
[Authorize(Roles = "Admin")]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class PromotionController : CustomControllerBase<PromotionController>
{
    private readonly IPromotionService _promotionService;
    private readonly ILogService<PromotionController> _logger;

    public PromotionController(IPromotionService promotionService, ILogService<PromotionController> logger) : base(logger)
    {
        _promotionService = promotionService;
        _logger = logger;
    }

    /// <summary> Cria uma nova promoção </summary>
    /// <remarks>Requer permissão de Admin</remarks>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePromotionDto dto)
    {
        var result = await _promotionService.CreateAsync(dto);

        if (!result.Success)
        {
            return HandleError(result.Error.StatusCode, result.Error.ErrorMessage);
        }

        return HandleResult(result);
    }

    /// <summary> Listagem das promoções </summary>
    /// <remarks>Requer permissão de Admin</remarks>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PaginationDto pagination)
    {
        var result = await _promotionService.GetAllAsync(pagination);

        if (!result.Success)
        {
            return HandleError(result.Error.StatusCode, result.Error.ErrorMessage);
        }

        return HandleResult(result);
    }

    /// <summary> Listar detalhes de uma promoção específica </summary>
    /// <remarks>Requer permissão de Admin</remarks>
    [HttpGet("{promotionId}")]
    public async Task<IActionResult> GetById(Guid promotionId)
    {
        var result = await _promotionService.GetByIdAsync(promotionId);

        if (!result.Success)
        {
            return HandleError(result.Error.StatusCode, result.Error.ErrorMessage);
        }

        return HandleResult(result);
    }

    /// <summary> Remove uma promoção específica </summary>
    /// <remarks>Requer permissão de Admin</remarks>
    [HttpDelete("{promotionId}")]
    public async Task<IActionResult> RemoveById(Guid promotionId)
    {
        var result = await _promotionService.RemovePromotionAsync(promotionId);

        if (!result.Success)
        {
            return HandleError(result.Error.StatusCode, result.Error.ErrorMessage);
        }

        return HandleResult(result);
    }

    /// <summary> Atualiza uma promoção específica </summary>
    /// <remarks>Requer permissão de Admin</remarks>
    [HttpPut("{promotionId}")]
    public async Task<IActionResult> Update(CreatePromotionDto dto, Guid promotionId)
    {
        var result = await _promotionService.UpdateAsync(dto, promotionId);

        if (!result.Success)
        {
            return HandleError(result.Error.StatusCode, result.Error.ErrorMessage);
        }

        return HandleResult(result);
    }

    /// <summary> Inativa uma promoção específica </summary>
    /// <remarks>Requer permissão de Admin</remarks>
    [HttpPut("{promotionId}/inactive")]
    public async Task<IActionResult> InactivePromotion(Guid promotionId)
    {
        var result = await _promotionService.SetPromotionInactive(promotionId);

        if (!result.Success)
        {
            return HandleError(result.Error.StatusCode, result.Error.ErrorMessage);
        }

        return HandleResult(result);
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