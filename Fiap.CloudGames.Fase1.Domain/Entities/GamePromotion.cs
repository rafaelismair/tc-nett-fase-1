namespace Fiap.CloudGames.Fase1.Domain.Entities;

public class GamePromotion
{
    public Guid GamePromotionId { get; set; } = Guid.NewGuid();

    public Guid GameId { get; set; }
    public Game Game { get; set; }

    public Guid PromotionId { get; set; }
    public Promotion Promotion { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public GamePromotion(Guid gameId, Guid promotionId, DateTime startDate, DateTime? endDate)
    {
        GameId = gameId;
        PromotionId = promotionId;
        StartDate = startDate;
        EndDate = endDate;
    }
}
