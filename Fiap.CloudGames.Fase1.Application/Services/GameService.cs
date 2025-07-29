using Fiap.CloudGames.Fase1.Application.DTOs.Games;
using Fiap.CloudGames.Fase1.Application.DTOs.Shared;
using Fiap.CloudGames.Fase1.Application.DTOs.Shared.ValueObjects;
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

    public async Task<ResultDto<GameDto>> CreateAsync(CreateGameDto dto)
    {
        var game = new Game(dto.Title, dto.Description, dto.ReleaseDate);

        _context.Games.Add(game);
        var result = await _context.SaveChangesAsync();

        if (result is 0)
        {
            return ResultDto<GameDto>.Fail(Error.InternalServerError("Houve uma falha e não foi possível cadastrar o jogo"));
        }

        return ResultDto<GameDto>.Ok(GameMapper.ToDto(game));
    }

    public async Task<ResultDto<ListGamesDto>> GetAllAsync(PaginationDto pagination)
    {
        var games = await _context.Games
                    .AsNoTracking()
                    .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                    .Take(pagination.PageSize)
                    .ToListAsync();

        var total = await _context.Games.AsNoTracking().CountAsync();
        pagination.SetTotalPages(total);
        return ResultDto<ListGamesDto>.Ok(GameMapper.ToListDto(games, pagination));
    }

    public async Task<ResultDto<GameDto>> GetByIdAsync(Guid gameId)
    {
        var game = await _context.Games.AsNoTracking().FirstOrDefaultAsync(game => game.Id == gameId);
        return ResultDto<GameDto>.Ok(GameMapper.ToDto(game));
    }

    public async Task<ResultDto> RemoveGameAsync(Guid gameId)
    {
        var game = await _context.Games.AsNoTracking().FirstOrDefaultAsync(game => game.Id == gameId);

        if (game is null)
            return ResultDto.Fail(Error.BadRequest($"O jogo com ID {gameId} não encontrado para remoção."));

        _context.Games.Remove(game);
        var result = await _context.SaveChangesAsync();

        if (result is 0)
        {
            return ResultDto.Fail(Error.InternalServerError("Algum erro aconteceu ao remover o jogo."));
        }

        return ResultDto.Ok("Jogo removido");
    }

    public async Task<ResultDto<GameDto>> UpdateAsync(CreateGameDto dto, Guid gameId)
    {
        var game = await _context.Games.FirstOrDefaultAsync(game => game.Id == gameId);

        if (game is null)
            return ResultDto<GameDto>.Fail(Error.BadRequest($"O jogo com ID {gameId} não encontrado para atualização."));

        game.Update(dto.Title, dto.Description, dto.ReleaseDate);

        var result = await _context.SaveChangesAsync();

        if (result is 0)
        {
            return ResultDto<GameDto>.Fail(Error.InternalServerError("Houve uma falha e não foi possível atualizar o jogo"));
        }

        return ResultDto<GameDto>.Ok(GameMapper.ToDto(game));
    }
}
