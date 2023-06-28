using HiLoGame.Domain.Entities;

namespace HiLoGame.Infrastructure.Data.Interfaces
{
    public interface IPlayerRepository : IRepository<PlayerEntity>
    {
        // Additional methos speficic to PlayerEntity
        Task<PlayerEntity> GetByNameAsync(string playerName);
    }
}
