using HiLoGame.Domain.Entities;
using HiLoGame.Infrastructure.Data.Context;
using HiLoGame.Infrastructure.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HiLoGame.Infrastructure.Data.Repositories;

public class PlayerRepository : RepositoryBase<PlayerEntity>, IPlayerRepository
{
    public PlayerRepository(AppDbContext context) : base(context) { }

    public async Task<PlayerEntity> GetByNameAsync(string playerName)
    {
        return await _dbSet.FirstOrDefaultAsync(p => p.Name == playerName);
    }
}
