using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rmit.Asr.Application.Models;

namespace Rmit.Asr.Application.Data
{
    public class ApplicationDataContext : IdentityDbContext<ApplicationUser>
    {
        
        public DbSet<Room> Room { get; set; }
        
        public DbSet<Staff> Staff { get; set; }
        
        public DbSet<Student> Student { get; set; }
        
        public DbSet<Slot> Slot { get; set; }

        public ApplicationDataContext(DbContextOptions<ApplicationDataContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Slot>().HasKey(x => new { x.RoomID, x.StartTime });
        }

    }
}
