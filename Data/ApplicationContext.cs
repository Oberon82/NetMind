using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetMind.Models;
using Microsoft.AspNetCore.Identity;
using NetMind.Areas.Admin.Models;
using NetMind.Areas.Site.Models;

namespace NetMind.Data
{
    public class ApplicationContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Tracker> Trackers { get; set; }
        public DbSet<Priority> Priorities { get; set; }
        public DbSet<Issue> Issue { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            if (Database.EnsureCreated())
            {
                Trackers.Add(new Tracker() { Name = "Bug", Order = 1 });
                Priorities.AddRange(
                    new Priority() { Active = true, IsDefault = false, Name = "Low", Order = 1 },
                    new Priority() { Active = true, IsDefault = true, Name = "Normal", Order = 2 },
                    new Priority() { Active = true, IsDefault = false, Name = "High", Order = 3 }
                    );
                //Projects.Add(new Project() { Name = "Default", Public = true });
                Statuses.AddRange(
                    new Status() { IsClosed = false, IsDefault = true, Name = "Open", Order = 1 },
                    new Status() { IsClosed = false, IsDefault = false, Name = "In Progress", Order = 2 },
                    new Status() { IsClosed = false, IsDefault = false, Name = "To be tested", Order = 3 },
                    new Status() { IsClosed = true, IsDefault = false, Name = "Closed", Order = 4 },
                    new Status() { IsClosed = false, IsDefault = false, Name = "Reopen", Order = 5 },
                    new Status() { IsClosed = true, IsDefault = false, Name = "Rejected", Order = 6 }
                    );
                SaveChanges();
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
