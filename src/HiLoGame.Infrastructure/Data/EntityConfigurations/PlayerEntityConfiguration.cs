using HiLoGame.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HiLoGame.Infrastructure.Data.EntityConfigurations;

public class PlayerEntityConfiguration : IEntityTypeConfiguration<PlayerEntity>
{
    public void Configure(EntityTypeBuilder<PlayerEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name);
    }
}
