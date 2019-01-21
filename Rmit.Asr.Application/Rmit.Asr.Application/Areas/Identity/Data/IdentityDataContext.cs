using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rmit.Asr.Application.Areas.Identity.Models;

namespace Rmit.Asr.Application.Areas.Identity.Data
{
    public class IdentityDataContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Student> Students { get; set; }
        
        public DbSet<Staff> Staff { get; set; }
        
        public IdentityDataContext(DbContextOptions<IdentityDataContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
