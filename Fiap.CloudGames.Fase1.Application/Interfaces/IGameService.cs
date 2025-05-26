using Fiap.CloudGames.Fase1.Application.DTOs;
using Fiap.CloudGames.Fase1.Domain.Entities;

namespace Fiap.CloudGames.Fase1.Application.Interfaces;

public interface IGameService
{
    Task<Game> CreateAsync(CreateGameDto dto);
    Task<IEnumerable<Game>> GetAllAsync();
    Task<Game> GetByIdAsync(Guid gameId);
    Task AcquireGameAsync(Guid userId, Guid gameId);
    Task<IEnumerable<Game>> GetUserGamesAsync(Guid userId);
}
