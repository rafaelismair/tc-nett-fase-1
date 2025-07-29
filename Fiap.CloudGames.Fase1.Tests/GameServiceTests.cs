using Fiap.CloudGames.Fase1.Application.DTOs.Games;
using Fiap.CloudGames.Fase1.Application.DTOs.Shared;
using Fiap.CloudGames.Fase1.Application.Services;
using Fiap.CloudGames.Fase1.Domain.Entities;
using Fiap.CloudGames.Fase1.Infrastructure.Data;
using Fiap.CloudGames.Fase1.Infrastructure.LogService.Services;
using Microsoft.EntityFrameworkCore;

namespace Fiap.CloudGames.Fase1.Tests;

public class GameServiceTests
{
    private readonly ApplicationDbContext _context;
    private readonly GameService _service;
    private readonly LogService<GameService> _logger;

    public GameServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("GameTestDb")
            .Options;
            
        _logger = new LogService<GameService>();
        _context = new ApplicationDbContext(options);
        _service = new GameService(_context, _logger);
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

        var result = await _service.CreateAsync(dto);

        Assert.True(result.Success);
        Assert.Equal(dto.Title, result?.Data?.Title);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnPaginatedGames()
    {
        for (int i = 1; i <= 10; i++)
        {
            _context.Games.Add(new Game($"Jogo {i}", $"Descrição {i}", DateTime.UtcNow.AddDays(-i)));
        }

        await _context.SaveChangesAsync();

        var pagination = new PaginationDto() { PageNumber = 1, PageSize = 5};

        var result = await _service.GetAllAsync(pagination);

        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal(5, result.Data.Games.Count); 
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnGame_WhenExists()
    {
        var game = new Game("Found Game", "Description", DateTime.Today);
        _context.Games.Add(game);
        await _context.SaveChangesAsync();

        var result = await _service.GetByIdAsync(game.Id);

        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal(game.Id, result.Data.Id);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateGame_WhenExists()
    {
        var game = new Game("Old Title", "Old Desc", DateTime.Today);
        _context.Games.Add(game);
        await _context.SaveChangesAsync();

        var dto = new CreateGameDto()
        {
            Title = "New Title", 
            Description = "New Desc", 
            ReleaseDate = DateTime.Today.AddDays(1)
        };

        var result = await _service.UpdateAsync(dto, game.Id);

        Assert.True(result.Success);
        Assert.Equal("New Title", result.Data.Title);
        Assert.Equal("New Desc", result.Data.Description);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoGamesExist()
    {
        var localContext = new ApplicationDbContext(
        new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase("NewDb")
        .Options
        );

        var pagination = new PaginationDto();

        var result = await _service.GetAllAsync(pagination);

        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Empty(result.Data.Games);
        Assert.Equal(0, pagination.TotalPages);
    }
}
