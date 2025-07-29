namespace Fiap.CloudGames.Fase1.Application.DTOs.Games;
public class CreateGameDto
{
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime ReleaseDate { get; set; }
}
