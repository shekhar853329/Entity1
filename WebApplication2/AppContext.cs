using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace WebApplication2
{
    public class AppContext : DbContext
    {
        public AppContext(DbContextOptions<AppContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserHistory> UserHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            ApplyAudit(this);

            return base.SaveChangesAsync(true, cancellationToken);
        }

        private void ApplyAudit(AppContext appContext)
        {
            var entries = appContext.ChangeTracker.Entries();

            var userRoles = entries.Where(x => x.State == EntityState.Deleted || x.State == EntityState.Added).Select(x => x.Entity).OfType<UserHistory>().ToArray();

            if (userRoles.Length > 0)
            {
                var now = DateTime.UtcNow;

                var historyRecords = new List<UserHistory>();

                foreach (var ur in userRoles)
                {
                    historyRecords.Add(new UserHistory
                    {
                        CreatedAt = now,
                    });
                }

                appContext.UserHistories.AddRange(historyRecords);
            }
        }
    }
}
