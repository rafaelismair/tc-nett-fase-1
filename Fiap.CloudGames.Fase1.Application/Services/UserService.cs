using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fiap.CloudGames.Fase1.Application.DTOs;
using Fiap.CloudGames.Fase1.Application.Interfaces;
using Fiap.CloudGames.Fase1.Domain.Entities;
using Fiap.CloudGames.Fase1.Infrastructure.Data;
using Fiap.CloudGames.Fase1.Infrastructure.LogService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fiap.CloudGames.Fase1.Application.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogService<UserService> _logger;


        public UserService(ApplicationDbContext context, ILogService<UserService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserAsync(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) 
            {
                throw new Exception("Usuário não encontrado");
            }

            return user;
        }

        public async Task PutUserAsync(Guid userId, PatchUserDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new Exception("Usuário não encontrado");
            }

            user.Name = dto.Name;
            _context.Users.Update(user);
            _context.SaveChanges();
        }
    }
}
