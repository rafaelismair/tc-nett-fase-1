using Fiap.CloudGames.Fase1.Application.DTOs.Games;
using Fiap.CloudGames.Fase1.Application.DTOs.Shared;
using Fiap.CloudGames.Fase1.Domain.Entities;

namespace Fiap.CloudGames.Fase1.Application.Interfaces;

public interface IGameService
{
    Task<GameDto> CreateAsync(CreateGameDto dto);
    Task<ListGamesDto> GetAllAsync(PaginationDto pagination);
    Task<GameDto> GetByIdAsync(Guid gameId);
    Task RemoveGameAsync(Guid gameId);
}
