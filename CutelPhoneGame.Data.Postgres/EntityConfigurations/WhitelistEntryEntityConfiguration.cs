using CutelPhoneGame.Data.Postgres.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CutelPhoneGame.Data.Postgres.EntityConfigurations
{
    public class WhitelistEntryEntityConfiguration : IEntityTypeConfiguration<WhitelistEntry>
    {
        public void Configure(EntityTypeBuilder<WhitelistEntry> builder)
        {
            //Indexes
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => new { e.Value, e.Interpretation }).IsUnique();

            //Columns
            builder.Property(e => e.Id).IsRequired();

            builder.Property(e => e.Value).IsRequired();
            builder.Property(e => e.Interpretation).IsRequired();

            builder.Property(e => e.Created).IsRequired();
            builder.Property(e => e.Updated).IsRequired();
        }
    }
}