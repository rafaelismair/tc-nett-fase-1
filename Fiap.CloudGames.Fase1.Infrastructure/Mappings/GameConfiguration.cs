using Fiap.CloudGames.Fase1.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fiap.CloudGames.Fase1.Infrastructure.Mappings;
public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.HasKey(g => g.Id);

        builder.Property(g => g.Title)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(g => g.Description)
            .IsRequired();

        builder.Property(g => g.ReleaseDate)
            .IsRequired();

        builder.HasMany(g => g.UserGames)
            .WithOne(ug => ug.Game)
            .HasForeignKey(ug => ug.GameId);
    }
}
