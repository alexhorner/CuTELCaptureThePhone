using CutelCaptureThePhone.Data.Postgres.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CutelCaptureThePhone.Data.Postgres.EntityConfigurations
{
    public class MapPinEntityConfiguration : IEntityTypeConfiguration<MapPin>
    {
        public void Configure(EntityTypeBuilder<MapPin> builder)
        {
            //Indexes
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => e.Number).IsUnique();

            //Columns
            builder.Property(e => e.Id).IsRequired();

            builder.Property(e => e.Lat).IsRequired();
            builder.Property(e => e.Long).IsRequired();
            builder.Property(e => e.Name).IsRequired();
            builder.Property(e => e.Number).IsRequired();

            builder.Property(e => e.Created).IsRequired();
            builder.Property(e => e.Updated).IsRequired();
        }
    }
}