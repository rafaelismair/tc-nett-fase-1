namespace Fiap.CloudGames.Fase1.Application.DTOs.Games;

public record GamePromotionDto(Guid Id, Guid GameId, Guid PromotionId, DateTime StartDate, DateTime? EndDate)
{
}
