using Fiap.CloudGames.Fase1.Application.DTOs.Games;
using Fiap.CloudGames.Fase1.Application.DTOs.Shared;

namespace Fiap.CloudGames.Fase1.Application.Interfaces;

public interface IGameService
{
    Task<ResultDto<GameDto>> CreateAsync(CreateGameDto dto);
    Task<ResultDto<GameDto>> UpdateAsync(CreateGameDto dto, Guid gameId);
    Task<ResultDto<ListGamesDto>> GetAllAsync(PaginationDto pagination);
    Task<ResultDto<GameDto>> GetByIdAsync(Guid gameId);
    Task<ResultDto> RemoveGameAsync(Guid gameId);
}
