using Fiap.CloudGames.Fase1.Application.DTOs;
using Fiap.CloudGames.Fase1.Application.Services;
using Fiap.CloudGames.Fase1.Domain.Entities;
using Fiap.CloudGames.Fase1.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fiap.CloudGames.Fase1.Tests;

public class GameServiceTests
{
    private readonly ApplicationDbContext _context;
    private readonly GameService _service;

    public GameServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("GameTestDb")
            .Options;

        _context = new ApplicationDbContext(options);
        _service = new GameService(_context);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddGame()
    {
        var dto = new CreateGameDto
        {
            Title = "Jogo Teste",
            Description = "Teste de criação",
            ReleaseDate = DateTime.UtcNow
        };

        var game = await _service.CreateAsync(dto);

        Assert.NotNull(game);
        Assert.Equal(dto.Title, game.Title);
    }

    [Fact]
    public async Task AcquireGameAsync_ShouldAddUserGame()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "Carlos",
            Email = "carlos@fiap.com",
            PasswordHash = "hashed",
            Role = Domain.Enums.UserRole.User
        };
        var game = new Game
        {
            Id = Guid.NewGuid(),
            Title = "Game A",
            Description = "Descrição",
            ReleaseDate = DateTime.UtcNow
        };

        _context.Users.Add(user);
        _context.Games.Add(game);
        await _context.SaveChangesAsync();

        await _service.AcquireGameAsync(user.Id, game.Id);

        Assert.True(_context.UserGames.Any(ug => ug.UserId == user.Id && ug.GameId == game.Id));
    }
}
