using Fiap.CloudGames.Fase1.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fiap.CloudGames.Fase1.Infrastructure.Mappings;

public class GamePromotionConfiguration : IEntityTypeConfiguration<GamePromotion>
{
    public void Configure(EntityTypeBuilder<GamePromotion> builder)
    {
        builder.HasKey(g => g.GamePromotionId);

        builder.HasOne(gp => gp.Game)
            .WithMany(g => g.GamePromotions)
            .HasForeignKey(gp => gp.GameId);

        builder.HasOne(gp => gp.Promotion)
            .WithMany(p => p.GamePromotions)
            .HasForeignKey(gp => gp.PromotionId);
    }
}