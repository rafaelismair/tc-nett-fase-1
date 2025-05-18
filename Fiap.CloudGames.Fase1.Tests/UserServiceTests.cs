using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fiap.CloudGames.Fase1.Application.DTOs;
using Fiap.CloudGames.Fase1.Application.Services;
using Fiap.CloudGames.Fase1.Domain.Entities;
using Fiap.CloudGames.Fase1.Infrastructure.Data;
using Fiap.CloudGames.Fase1.Infrastructure.LogService.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Fiap.CloudGames.Fase1.Tests;

    public class UserServiceTests
    {
        private readonly ApplicationDbContext _context;
        private readonly UserService _service;
        private readonly Mock<ILogService<UserService>> _loggerMock;

        public UserServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // banco novo a cada execução
                .Options;

            _context = new ApplicationDbContext(options);
            _loggerMock = new Mock<ILogService<UserService>>();
            _service = new UserService(_context, _loggerMock.Object);

            SeedData().Wait();
        }

        private async Task SeedData()
        {
            await _context.Users.AddAsync(new User
            {
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Name = "Usuário Inicial"
            });

            await _context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnUsers()
        {
            var users = await _service.GetAllAsync();

            Assert.NotNull(users);
            Assert.Single(users);
        }

        [Fact]
        public async Task GetUserAsync_ValidId_ReturnsUser()
        {
            var userId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

            var user = await _service.GetUserAsync(userId);

            Assert.NotNull(user);
            Assert.Equal("Usuário Inicial", user.Name);
        }

        [Fact]
        public async Task GetUserAsync_InvalidId_ThrowsException()
        {
            var invalidId = Guid.NewGuid();

            var ex = await Assert.ThrowsAsync<Exception>(() => _service.GetUserAsync(invalidId));

            Assert.Equal("Usuário não encontrado", ex.Message);
        }

        [Fact]
        public async Task PutUserAsync_ValidId_UpdatesUser()
        {
            var userId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
            var dto = new PatchUserDto { Name = "Nome Atualizado" };

            await _service.PutUserAsync(userId, dto);

            var updatedUser = await _context.Users.FindAsync(userId);
            Assert.Equal("Nome Atualizado", updatedUser.Name);
        }

        [Fact]
        public async Task PutUserAsync_InvalidId_ThrowsException()
        {
            var invalidId = Guid.NewGuid();
            var dto = new PatchUserDto { Name = "Teste" };

            var ex = await Assert.ThrowsAsync<Exception>(() => _service.PutUserAsync(invalidId, dto));

            Assert.Equal("Usuário não encontrado", ex.Message);
        }
    }

