namespace Fiap.CloudGames.Fase1.Domain.Entities;

public class UserGame
{
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public Guid GameId { get; set; }
    public Game Game { get; set; } = default!;

    public DateTime AcquiredAt { get; set; } = DateTime.UtcNow;
}
