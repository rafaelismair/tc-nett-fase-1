namespace Fiap.CloudGames.Fase1.Domain.Entities;

public class GamePromotion
{
    public Guid GamePromotionId { get; set; }

    public Guid GameId { get; set; }
    public Game Game { get; set; }

    public Guid PromotionId { get; set; }
    public Promotion Promotion { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
