namespace Fiap.CloudGames.Fase1.Domain.Entities;

public class Promotion
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string PromotionTitle { get; set; } = default!;
    public string? PromotionDescription { get; set; }
    public decimal DiscountPercentage { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool Active { get; set; }
    public ICollection<GamePromotion> GamePromotions { get; set; } = new List<GamePromotion>();
}
