namespace Fiap.CloudGames.Fase1.Domain.Entities;
public class Game
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime ReleaseDate { get; set; }

    public ICollection<UserGame> UserGames { get; set; } = new List<UserGame>();
}

