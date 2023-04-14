using DataTech.System.Versioning.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace DataTech.System.Versioning.Data
{
    public class DataContext : DbContext
    {

        public DataContext()
        {

        }

        public DataContext(DbContextOptions<DataContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            PrepareInitialValues(modelBuilder);
        }


        private void PrepareInitialValues(ModelBuilder modelBuilder)
        {

            
        }


        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppSystem> AppSystems { get; set; }
        public DbSet<AppModule> AppModules { get; set; }
        public DbSet<AppSystemLog> AppSystemLogs { get; set; }
        public DbSet<AppModuleLog> AppModuleLogs { get; set; }


        #region SaveChanges overrides 
        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e =>
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified);

            foreach (var entityEntry in entries)
            {
                var modificationTimeProp = entityEntry.Properties.FirstOrDefault(x => x.Metadata.Name == "UpdateTime");
                if (modificationTimeProp != null)
                {
                    if (entityEntry.State != EntityState.Added)
                    {
                        modificationTimeProp.CurrentValue = DateTimeOffset.Now;
                    }
                }

                if (entityEntry.State == EntityState.Added)
                {
                    var createTimeProp = entityEntry.Properties.FirstOrDefault(x => x.Metadata.Name == "CreationTime");
                    if (createTimeProp != null)
                    {
                        createTimeProp.CurrentValue = DateTimeOffset.Now;

                    }
                }
            }

            return base.SaveChanges();
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            var entries = ChangeTracker
            .Entries()
            .Where(e =>
                    e.State == EntityState.Added
                    || e.State == EntityState.Modified);

            foreach (var entityEntry in entries)
            {

                var modificationTimeProp = entityEntry.Properties.FirstOrDefault(x => x.Metadata.Name == "UpdateTime");

                if (modificationTimeProp != null)
                {
                    if (entityEntry.State != EntityState.Added)
                    {
                        modificationTimeProp.CurrentValue = DateTimeOffset.Now;
                    }
                }

                if (entityEntry.State == EntityState.Added)
                {
                    var createTimeProp = entityEntry.Properties.FirstOrDefault(x => x.Metadata.Name == "CreationTime");
                    if (createTimeProp != null)
                    {
                        createTimeProp.CurrentValue = DateTimeOffset.Now;

                    }
                }


            }




            return await base.SaveChangesAsync(cancellationToken);
        }
        #endregion
    }
}
