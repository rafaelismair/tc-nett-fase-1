using Fiap.CloudGames.Fase1.Application.DTOs.Shared;

namespace Fiap.CloudGames.Fase1.Application.DTOs.Games;

public class ListGamesDto
{
    public List<GameDto> Games { get; set; } = new();
    public PaginationDto Pagination { get; set; } = new PaginationDto();
}
