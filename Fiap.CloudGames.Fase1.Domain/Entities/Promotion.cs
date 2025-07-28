namespace Fiap.CloudGames.Fase1.Domain.Entities;

public class Promotion
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string PromotionTitle { get; set; } = default!;
    public string? PromotionDescription { get; set; }
    public decimal DiscountPercentage { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool Active { get; set; }
    public ICollection<GamePromotion> GamePromotions { get; set; } = new List<GamePromotion>();

    public Promotion() { }

    public Promotion(string promotionTitle, string? promotionDescription, decimal discountPercentage, bool active)
    {
        PromotionTitle = promotionTitle;
        PromotionDescription = promotionDescription;
        DiscountPercentage = discountPercentage;
        Active = active;
    }

    public void SetInactive()
    {
        if (Active)
        {
            Active = false;
        }
    }

    public void Update(string promotionTitle, string promotionDescription, decimal discountPercentage)
    {
        PromotionTitle = promotionTitle;
        PromotionDescription = promotionDescription;
        DiscountPercentage = discountPercentage;
    }
}
