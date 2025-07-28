namespace Fiap.CloudGames.Fase1.Application.DTOs.Promotions;

public record PromotionDto(Guid Id, string PromotionTitle, string? PromotionDescription, decimal DiscountPercentage, bool Active, DateTime CreatedAt)
{
}
