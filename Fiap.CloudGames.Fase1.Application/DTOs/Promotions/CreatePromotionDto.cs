using System.ComponentModel.DataAnnotations;

namespace Fiap.CloudGames.Fase1.Application.DTOs.Promotions;

public record CreatePromotionDto(
    [Required(ErrorMessage = "O título da promoção é obrigatório.")]
    string PromotionTitle, 
    string PromotionDescription,
    [Range(0, 100, ErrorMessage = "O percentual de desconto deve estar entre 0% e 100%.")]
    decimal DiscountPercentage, 
    bool Active)
{
}
