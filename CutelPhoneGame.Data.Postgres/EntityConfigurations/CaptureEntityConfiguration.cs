using CutelPhoneGame.Data.Postgres.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CutelPhoneGame.Data.Postgres.EntityConfigurations
{
    public class CaptureEntityConfiguration : IEntityTypeConfiguration<Capture>
    {
        public void Configure(EntityTypeBuilder<Capture> builder)
        {
            //Indexes
            builder.HasKey(e => e.Id);
            
            //Columns 
            builder.Property(e => e.Id).IsRequired();

            builder.Property(e => e.PlayerId).IsRequired();

            builder.Property(e => e.FromNumber).IsRequired();

            builder.Property(e => e.Created).IsRequired();
            builder.Property(e => e.Updated).IsRequired();
            
            //Relationships
            builder.HasOne(e => e.Player).WithMany(f => f.Captures).HasForeignKey(e => e.PlayerId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}