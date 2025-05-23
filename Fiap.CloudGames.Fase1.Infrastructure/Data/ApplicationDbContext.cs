using Fiap.CloudGames.Fase1.Domain.Entities;
using Fiap.CloudGames.Fase1.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Fiap.CloudGames.Fase1.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Game> Games => Set<Game>();
    public DbSet<UserGame> UserGames => Set<UserGame>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        modelBuilder.Entity<User>().HasData(new User
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Name = "Administrador",
            Email = "admin@fiap.com",
            PasswordHash = "$2a$11$KB4AoC8oR1WsGZQHRUwCVeYxTHykOwnkD2Dk.g9sEPT0VsUo4jksi", // admin123
            Role = UserRole.Admin
        });

    }
}
