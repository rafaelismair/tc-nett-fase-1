using Fiap.CloudGames.Fase1.Application.DTOs.Shared;

namespace Fiap.CloudGames.Fase1.Application.DTOs.Promotions;

public class ListPromotionDto
{
    public List<PromotionDto> Promotions { get; set; } = new();
    public PaginationDto Pagination { get; set; } = new PaginationDto();
}
