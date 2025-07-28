using Fiap.CloudGames.Fase1.Application.DTOs.Promotions;
using Fiap.CloudGames.Fase1.Application.DTOs.Shared;
using Fiap.CloudGames.Fase1.Application.DTOs.Shared.ValueObjects;
using Fiap.CloudGames.Fase1.Application.Interfaces;
using Fiap.CloudGames.Fase1.Application.Mapping;
using Fiap.CloudGames.Fase1.Domain.Entities;
using Fiap.CloudGames.Fase1.Infrastructure.Data;
using Fiap.CloudGames.Fase1.Infrastructure.LogService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fiap.CloudGames.Fase1.Application.Services;

public class PromotionService : IPromotionService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogService<PromotionService> _logger;

    public PromotionService(ApplicationDbContext context, ILogService<PromotionService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ResultDto<PromotionDto>> CreateAsync(CreatePromotionDto dto)
    {
        var promotion = new Promotion(dto.PromotionTitle, dto.PromotionDescription, dto.DiscountPercentage, dto.Active);

        _context.Promotions.Add(promotion);
        var result = await _context.SaveChangesAsync();

        if (result is 0)
        {
            return ResultDto<PromotionDto>.Fail(Error.InternalServerError("Failed to create the promotion."));
        }

        return ResultDto<PromotionDto>.Ok(PromotionMapper.ToDto(promotion));
    }

    public async Task<ResultDto<ListPromotionDto>> GetAllAsync(PaginationDto pagination)
    {
        var promotions = await _context.Promotions
                    .AsNoTracking()
                    .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                    .Take(pagination.PageSize)
                    .ToListAsync();

        var total = await _context.Promotions.AsNoTracking().CountAsync();
        pagination.SetTotalPages(total);
        return ResultDto<ListPromotionDto>.Ok(PromotionMapper.ToListDto(promotions, pagination));
    }

    public async Task<ResultDto<PromotionDto>> GetByIdAsync(Guid promotionId)
    {
        var promotion = await _context.Promotions.AsNoTracking().FirstOrDefaultAsync(promotion => promotion.Id == promotionId);
        return ResultDto<PromotionDto>.Ok(PromotionMapper.ToDto(promotion));
    }

    public async Task<ResultDto> RemovePromotionAsync(Guid promotionId)
    {
        var promotion = await _context.Promotions.AsNoTracking().FirstOrDefaultAsync(promotion => promotion.Id == promotionId);

        if (promotion is null)
            return ResultDto.Fail(Error.BadRequest($"Promotion with ID {promotionId} not found."));

        _context.Promotions.Remove(promotion);
        var result = await _context.SaveChangesAsync();

        if (result is 0)
        {
            return ResultDto.Fail(Error.InternalServerError("Failed to remove the promotion."));
        }

        return ResultDto.Ok("Promotion successfully removed.");
    }

    public async Task<ResultDto> SetPromotionInactive(Guid promotionId)
    {
        var promotion = await _context.Promotions.FirstOrDefaultAsync(promotion => promotion.Id == promotionId);

        if (promotion is null)
            return ResultDto.Fail(Error.BadRequest($"Promotion with ID {promotionId} not found."));

        promotion.SetInactive();
        var result = await _context.SaveChangesAsync();

        if (result is 0)
        {
            return ResultDto.Fail(Error.InternalServerError("Failed to inactivate the promotion."));
        }

        return ResultDto.Ok("Promotion successfully inactivated");
    }

    public async Task<ResultDto<PromotionDto>> UpdateAsync(CreatePromotionDto dto, Guid promotionId)
    {
        var promotion = await _context.Promotions.FirstOrDefaultAsync(promotion => promotion.Id == promotionId);

        if (promotion is null)
            return ResultDto<PromotionDto>.Fail(Error.BadRequest($"Promotion with ID {promotionId} not found."));

        promotion.Update(dto.PromotionTitle, dto.PromotionDescription, dto.DiscountPercentage);

        var result = await _context.SaveChangesAsync();

        if (result is 0)
        {
            return ResultDto<PromotionDto>.Fail(Error.InternalServerError("Failed to update the promotion."));
        }

        return ResultDto<PromotionDto>.Ok(PromotionMapper.ToDto(promotion));
    }
}
