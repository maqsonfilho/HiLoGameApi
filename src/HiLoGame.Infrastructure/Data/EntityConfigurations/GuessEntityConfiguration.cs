using HiLoGame.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HiLoGame.Infrastructure.Data.EntityConfigurations;

public class GuessEntityConfiguration : IEntityTypeConfiguration<GuessEntity>
{
    public void Configure(EntityTypeBuilder<GuessEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.GuessNumber);
        builder.Property(x => x.Feedback);

        builder.HasOne(x => x.Player)
            .WithMany(p => p.Guesses)
            .HasForeignKey(g => g.PlayerId)
            .OnDelete(DeleteBehavior.NoAction);
            
}
}
