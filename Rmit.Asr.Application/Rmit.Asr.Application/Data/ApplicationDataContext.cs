using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rmit.Asr.Application.Models;

namespace Rmit.Asr.Application.Data
{
    public class ApplicationDataContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        
        public DbSet<Room> Room { get; set; }
        
        public DbSet<Staff> Staff { get; set; }
        
        public DbSet<Student> Student { get; set; }
        
        public DbSet<Slot> Slot { get; set; }

        public ApplicationDataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Slot>().HasKey(x => new { x.RoomId, x.StartTime });
            
            modelBuilder.Entity<Room>()
                .HasIndex(u => u.Name)
                .IsUnique();
        }
    }
}
