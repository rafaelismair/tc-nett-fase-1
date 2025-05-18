using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fiap.CloudGames.Fase1.Application.DTOs;
using Fiap.CloudGames.Fase1.Domain.Entities;

namespace Fiap.CloudGames.Fase1.Application.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserAsync(Guid userId);
        Task<IEnumerable<User>> GetAllAsync();
        Task PutUserAsync(Guid userId, PatchUserDto dto);
    }
}
