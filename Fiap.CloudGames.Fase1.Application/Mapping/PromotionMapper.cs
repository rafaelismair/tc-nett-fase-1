using Fiap.CloudGames.Fase1.Application.DTOs.Games;
using Fiap.CloudGames.Fase1.Application.DTOs.Promotions;
using Fiap.CloudGames.Fase1.Application.DTOs.Shared;
using Fiap.CloudGames.Fase1.Domain.Entities;

namespace Fiap.CloudGames.Fase1.Application.Mapping
{
    public static class PromotionMapper
    {
        public static PromotionDto? ToDto(Promotion? promotion)
        {
            if (promotion is null)
            {
                return null;
            }

            return new PromotionDto(promotion.Id, promotion.PromotionTitle, promotion.PromotionDescription, promotion.DiscountPercentage, promotion.Active, promotion.CreatedAt);
        }

        public static ListPromotionDto ToListDto(IList<Promotion> promotions, PaginationDto pagination)
        {
            return new ListPromotionDto
            {
                Promotions = promotions?.Select(ToDto).ToList() ?? new List<PromotionDto>(),
                Pagination = pagination
            };
        }
    }
}
