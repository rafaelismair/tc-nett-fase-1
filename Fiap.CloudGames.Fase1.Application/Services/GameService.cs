using Fiap.CloudGames.Fase1.Application.DTOs.Games;
using Fiap.CloudGames.Fase1.Application.DTOs.Shared;
using Fiap.CloudGames.Fase1.Application.Interfaces;
using Fiap.CloudGames.Fase1.Application.Mapping;
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

    public async Task<GameDto> CreateAsync(CreateGameDto dto)
    {
        var game = new Game(dto.Title, dto.Description, dto.ReleaseDate);

        _context.Games.Add(game);
        var result = await _context.SaveChangesAsync();

        if (result is 0)
        {
            throw new Exception("Algum erro aconteceu ao cadastrar o jogo.");
        }

        return GameMapper.ToDto(game);
    }

    public async Task<ListGamesDto> GetAllAsync(PaginationDto pagination)
    {
        var games = await _context.Games
                    .AsNoTracking()
                    .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                    .Take(pagination.PageSize)
                    .ToListAsync();

        var total = await _context.Games.AsNoTracking().CountAsync();
        pagination.SetTotalPages(total);
        return GameMapper.ToListDto(games, pagination);
    }

    public async Task<GameDto> GetByIdAsync(Guid gameId)
    {
        var game = await _context.Games.FirstOrDefaultAsync(game => game.Id == gameId);
        return GameMapper.ToDto(game);
    }

    public async Task RemoveGameAsync(Guid gameId)
    {
        var game = await _context.Games.FirstOrDefaultAsync(game => game.Id == gameId);

        if (game is null)
            throw new KeyNotFoundException($"Jogo com ID {gameId} não encontrado para remoção.");

        _context.Games.Remove(game);
        var result = await _context.SaveChangesAsync();

        if (result is 0)
        {
            throw new Exception("Algum erro aconteceu ao remover o jogo.");
        }
    }
}
