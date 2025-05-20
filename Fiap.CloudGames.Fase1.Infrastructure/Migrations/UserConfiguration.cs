using Fiap.CloudGames.Fase1.Domain.Entities;
using Fiap.CloudGames.Fase1.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fiap.CloudGames.Fase1.Infrastructure.Migrations;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.PasswordHash)
            .IsRequired();

        builder.Property(u => u.Role)
                    .HasConversion<int>()
                    .IsRequired();

        builder.HasMany(u => u.UserGames)
            .WithOne(ug => ug.User)
            .HasForeignKey(ug => ug.UserId);
    }
}
