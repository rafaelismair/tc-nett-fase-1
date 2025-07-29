using Fiap.CloudGames.Fase1.Application.DTOs.Auth;

namespace Fiap.CloudGames.Fase1.Application.Interfaces;
public interface IAuthService
{
    Task<string> RegisterAsync(RegisterUserDto dto, bool isAdmin = false);
    Task<string> LoginAsync(LoginDto dto);
}
