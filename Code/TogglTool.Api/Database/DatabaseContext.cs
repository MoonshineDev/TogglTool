using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TogglTool.Api.Models;
using TogglTool.Api.Properties;

namespace TogglTool.Api.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TimeEntry> TimeEntries { get; set; }
        public DbSet<Workspace> Workspaces { get; set; }

        public DatabaseContext()
            : this(GetDefaultConnectionString())
        {
        }

        public DatabaseContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        { }

        private static string GetDefaultConnectionString()
        { return Settings.Default.ConnectionString; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            ResetIdIdentityAttribute<Client>(modelBuilder);
            ResetIdIdentityAttribute<Project>(modelBuilder);
            ResetIdIdentityAttribute<TimeEntry>(modelBuilder);
            ResetIdIdentityAttribute<Workspace>(modelBuilder);
        }

        private void ResetIdIdentityAttribute<T>(DbModelBuilder modelBuilder)
            where T : TogglEntity
        {
            modelBuilder
                .Entity<T>()
                .Property(x => x.id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}
