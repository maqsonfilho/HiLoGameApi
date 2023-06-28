using HiLoGame.Domain.Entities;

namespace HiLoGame.Infrastructure.Data.Interfaces
{
    public interface IGameRepository : IRepository<GameEntity>
    {

        // Additional methods specific to GameEntity
        Task<GameEntity> GetGameByIdAsync(Guid id);
    }
}
