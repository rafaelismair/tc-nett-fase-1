using Fiap.CloudGames.Fase1.Domain.Enums;

namespace Fiap.CloudGames.Fase1.Domain.Entities;
public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public UserRole Role { get; set; } = UserRole.User;

    public ICollection<UserGame> UserGames { get; set; } = new List<UserGame>();
}