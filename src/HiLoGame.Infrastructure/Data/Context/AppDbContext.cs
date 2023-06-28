using HiLoGame.Domain.Entities;
using HiLoGame.Infrastructure.Data.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace HiLoGame.Infrastructure.Data.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<GameEntity> Games { get; set; }
    public DbSet<PlayerEntity> Players { get; set; }
    public DbSet<GuessEntity> Guesses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new GameEntityConfiguration());
        modelBuilder.ApplyConfiguration(new PlayerEntityConfiguration());
        modelBuilder.ApplyConfiguration(new GuessEntityConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
}
