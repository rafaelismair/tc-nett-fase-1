using Fiap.CloudGames.Fase1.Application.DTOs;
using Fiap.CloudGames.Fase1.Application.Interfaces;
using Fiap.CloudGames.Fase1.Domain.Entities;
using Fiap.CloudGames.Fase1.Infrastructure.Data;
using Fiap.CloudGames.Fase1.Infrastructure.LogService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fiap.CloudGames.Fase1.Application.Services;
public class GameService : IGameService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogService<GameService> _logger;

    public GameService(ApplicationDbContext context, ILogService<GameService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Game> CreateAsync(CreateGameDto dto)
    {
        var game = new Game
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Description = dto.Description,
            ReleaseDate = dto.ReleaseDate
        };

        _context.Games.Add(game);
        await _context.SaveChangesAsync();

        return game;
    }

    public async Task<IEnumerable<Game>> GetAllAsync()
    {
        return await _context.Games.ToListAsync();
    }

    public async Task AcquireGameAsync(Guid userId, Guid gameId)
    {
        var exists = await _context.UserGames.AnyAsync(x => x.UserId == userId && x.GameId == gameId);
        if (exists)
        {
            _logger.LogInformation("Jogo já adquirido.");
            throw new Exception("Jogo já adquirido.");
        }

        _context.UserGames.Add(new UserGame
        {
            UserId = userId,
            GameId = gameId,
            AcquiredAt = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Game>> GetUserGamesAsync(Guid userId)
    {
        return await _context.UserGames
            .Where(x => x.UserId == userId)
            .Include(x => x.Game)
            .Select(x => x.Game)
            .ToListAsync();
    }

    public async Task<Game> GetByIdAsync(Guid gameId)
    {
        return await _context.Games.FirstOrDefaultAsync(game => game.Id == gameId);
    }
}
