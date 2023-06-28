using HiLoGame.Domain.Entities;
using HiLoGame.Infrastructure.Data.Context;
using HiLoGame.Infrastructure.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HiLoGame.Infrastructure.Data.Repositories;

public class GameRepository : RepositoryBase<GameEntity>, IGameRepository
{
    public GameRepository(AppDbContext context) : base(context) { }

    public async Task<GameEntity> GetGameByIdAsync(Guid id)
    {
        return await _context.Games
            .Include(x => x.Players)
            .ThenInclude(x => x.Guesses)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}
