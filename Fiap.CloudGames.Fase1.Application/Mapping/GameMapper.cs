using Fiap.CloudGames.Fase1.Application.DTOs.Games;
using Fiap.CloudGames.Fase1.Application.DTOs.Shared;
using Fiap.CloudGames.Fase1.Domain.Entities;

namespace Fiap.CloudGames.Fase1.Application.Mapping;

public static class GameMapper
{
    public static GameDto? ToDto(Game? game)
    {
        if (game is null)
        {
            return null;
        }

        return new GameDto
        {
            Id = game.Id,
            Title = game.Title,
            Description = game.Description,
            ReleaseDate = game.ReleaseDate
        };
    }

    public static ListGamesDto ToListDto(IList<Game> games, PaginationDto pagination)
    {
        return new ListGamesDto
        {
            Games = games?.Select(ToDto).ToList() ?? new List<GameDto>(),
            Pagination = pagination
        };
    }
}
