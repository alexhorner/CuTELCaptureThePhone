using CutelCaptureThePhone.Core.Models;
using CutelCaptureThePhone.Data.Postgres.Entities;
using CutelCaptureThePhone.Data.Postgres.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CutelCaptureThePhone.Data.Postgres
{
    public class CutelCaptureThePhoneDbContext : DbContext
    {
        public CutelCaptureThePhoneDbContext() { }
        
        public CutelCaptureThePhoneDbContext(DbContextOptions<CutelCaptureThePhoneDbContext> options) : base(options) { }
        
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Player> Players { get; set; } = null!;
        public virtual DbSet<Capture> Captures { get; set; } = null!;
        public virtual DbSet<BlacklistEntry> NumberBlacklist { get; set; } = null!;
        public virtual DbSet<WhitelistEntry> NumberWhitelist { get; set; } = null!;
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasPostgresExtension("citext");

            builder.ApplyConfiguration(new UserEntityConfiguration());
            builder.ApplyConfiguration(new PlayerEntityConfiguration());
            builder.ApplyConfiguration(new CaptureEntityConfiguration());
            builder.ApplyConfiguration(new BlacklistEntryEntityConfiguration());
            builder.ApplyConfiguration(new WhitelistEntryEntityConfiguration());

            base.OnModelCreating(builder);
        }
        
        public override int SaveChanges()
        {
            SetAutomaticValues();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetAutomaticValues();
            return base.SaveChangesAsync(cancellationToken);
        }
        
        private void SetAutomaticValues()
        {
            DateTime sharedNow = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            
            foreach (EntityEntry entity in ChangeTracker.Entries().Where(p => p.State == EntityState.Added))
            {
                // ReSharper disable once ConvertIfStatementToSwitchStatement - ReSharper did not consider if the entity was both IDateCreated and IDateUpdated
                if (entity.Entity is IDateCreated created && (created.Created == default || created.Created == DateTime.MinValue)) created.Created = sharedNow;

                if (entity.Entity is IDateUpdated updated) updated.Updated = sharedNow;

                //if (entity.Entity is IKeyable keyable) keyable.Key = Guid.NewGuid();
            }

            foreach (EntityEntry entity in ChangeTracker.Entries().Where(p => p.State == EntityState.Modified))
            {
                if (entity.Entity is IDateCreated created) Entry(created).Property(e => e.Created).IsModified = false;
                
                if (entity.Entity is IDateUpdated updated) updated.Updated = sharedNow;
                
                if (entity.Entity is IIdentifiable identifiable) Entry(identifiable).Property(e => e.Id).IsModified = false;
                
                //if (entity.Entity is IKeyable keyable) Entry(keyable).Property(e => e.Key).IsModified = false;
            }
        }
    }
}