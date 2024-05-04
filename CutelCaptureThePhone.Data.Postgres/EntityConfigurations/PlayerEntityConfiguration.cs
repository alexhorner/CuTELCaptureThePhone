using CutelCaptureThePhone.Data.Postgres.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CutelCaptureThePhone.Data.Postgres.EntityConfigurations
{
    public class PlayerEntityConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            //Indexes
            builder.HasKey(e => e.Id);

            builder.HasIndex(e => e.Pin).IsUnique();
            builder.HasIndex(e => e.Name).IsUnique();
            
            //Columns
            builder.Property(e => e.Id).IsRequired();

            builder.Property(e => e.Pin).IsRequired();
            builder.Property(e => e.Name).IsRequired();
            builder.Property(e => e.RegisteredFromNumber).IsRequired();

            builder.Property(e => e.Created).IsRequired();
            builder.Property(e => e.Updated).IsRequired();
        }
    }
}