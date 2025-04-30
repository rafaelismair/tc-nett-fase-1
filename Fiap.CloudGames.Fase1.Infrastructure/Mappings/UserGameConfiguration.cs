using Fiap.CloudGames.Fase1.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Fiap.CloudGames.Fase1.Infrastructure.Mappings;
public class UserGameConfiguration : IEntityTypeConfiguration<UserGame>
{
    public void Configure(EntityTypeBuilder<UserGame> builder)
    {
        builder.HasKey(ug => new { ug.UserId, ug.GameId });

        builder.Property(ug => ug.AcquiredAt)
            .IsRequired();
    }
}
