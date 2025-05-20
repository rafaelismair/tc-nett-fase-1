using Fiap.CloudGames.Fase1.Application.DTOs;
using Fiap.CloudGames.Fase1.Application.Services;
using Fiap.CloudGames.Fase1.Domain.Entities;
using Fiap.CloudGames.Fase1.Domain.Enums;
using Fiap.CloudGames.Fase1.Infrastructure.Data;
using Fiap.CloudGames.Fase1.Infrastructure.LogService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Fiap.CloudGames.Fase1.Tests;

public class AuthServiceTests
{
    private readonly ApplicationDbContext _context;
    private readonly AuthService _service;
    private readonly LogService<AuthService> _logger;

    public AuthServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("AuthTestDb")
            .Options;

        _context = new ApplicationDbContext(options);

        _logger = new LogService<AuthService>();

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "Jwt:Key", "12345678901234567890123456789012" },
                { "Jwt:Issuer", "TestIssuer" }
            })
            .Build();

        _service = new AuthService(_context, config, _logger);
    }

    [Fact]
    public async Task RegisterAsync_ShouldCreateUser()
    {
        var dto = new RegisterUserDto
        {
            Name = "João",
            Email = "joao@fiap.com",
            Password = "Senha123!"
        };

        var token = await _service.RegisterAsync(dto);

        Assert.NotNull(token);
        Assert.True(_context.Users.Any(u => u.Email == dto.Email));
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnToken_WhenCredentialsAreCorrect()
    {
        var email = "maria@fiap.com";
        var password = "Senha123!";
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "Maria",
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Role = UserRole.User
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var token = await _service.LoginAsync(new LoginDto
        {
            Email = email,
            Password = password
        });

        Assert.NotNull(token);
    }
}

