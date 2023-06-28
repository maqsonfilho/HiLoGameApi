using HiLoGame.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HiLoGame.Infrastructure.Data.EntityConfigurations;

public class GameEntityConfiguration : IEntityTypeConfiguration<GameEntity>
{
    public void Configure(EntityTypeBuilder<GameEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.MinNumber);
        builder.Property(x => x.MaxNumber);
        builder.Property(x => x.MysteryNumber);
        builder.Property(x => x.IsGameStarted);
        builder.Property(x => x.IsGameFinished);;
        
        builder.HasMany(g => g.Guesses)
            .WithOne(g => g.Game)
            .HasForeignKey(g => g.GameId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(g => g.Players)
            .WithOne(p => p.Game)
            .HasForeignKey(g => g.GameId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
