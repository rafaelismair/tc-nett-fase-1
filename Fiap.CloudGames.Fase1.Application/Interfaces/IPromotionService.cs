using Fiap.CloudGames.Fase1.Application.DTOs.Games;
using Fiap.CloudGames.Fase1.Application.DTOs.Promotions;
using Fiap.CloudGames.Fase1.Application.DTOs.Shared;

namespace Fiap.CloudGames.Fase1.Application.Interfaces;

public interface IPromotionService
{
    Task<ResultDto<PromotionDto>> CreateAsync(CreatePromotionDto dto);
    Task<ResultDto<PromotionDto>> UpdateAsync(CreatePromotionDto dto, Guid promotionId);
    Task<ResultDto<ListPromotionDto>> GetAllAsync(PaginationDto pagination);
    Task<ResultDto<PromotionDto>> GetByIdAsync(Guid promotionId);
    Task<ResultDto> RemovePromotionAsync(Guid promotionId);
    Task<ResultDto> SetPromotionInactive(Guid promotionId);
}
