namespace Fiap.CloudGames.Fase1.Domain.Entities;
public class Game
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Title { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public DateTime ReleaseDate { get; private set; }
    public ICollection<UserGame> UserGames { get; set; } = new List<UserGame>();
    public ICollection<GamePromotion> GamePromotions { get; set; } = new List<GamePromotion>();
    public Game() { }

    public Game(string title, string description, DateTime releaseDate)
    {
        Title = title;
        Description = description;
        ReleaseDate = releaseDate;
    }

    public void Update(string title, string description, DateTime releaseDate)
    {
        Title = title;
        Description = description;
        ReleaseDate = releaseDate;
    }
}

