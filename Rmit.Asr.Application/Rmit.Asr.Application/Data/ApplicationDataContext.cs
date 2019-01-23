using System;
using Microsoft.EntityFrameworkCore;
using Rmit.Asr.Application.Models;

namespace Rmit.Asr.Application.Data
{
    public class ApplicationDataContext : DbContext
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Student> Staffs { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Slot> Slots { get; set; }

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
