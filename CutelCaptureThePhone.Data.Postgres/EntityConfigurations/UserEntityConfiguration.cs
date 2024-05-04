using CutelCaptureThePhone.Data.Postgres.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CutelCaptureThePhone.Data.Postgres.EntityConfigurations
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            //Indexes
            builder.HasKey(e => e.Id);
            
            builder.HasIndex(e => e.Username).IsUnique();
            
            //Columns
            builder.Property(e => e.Id).IsRequired();
            
            builder.Property(e => e.Username).HasColumnType("citext").IsRequired();
            builder.Property(e => e.HashedPassword).IsRequired();

            builder.Property(e => e.Created).IsRequired();
            builder.Property(e => e.Updated).IsRequired();
        }
    }
}