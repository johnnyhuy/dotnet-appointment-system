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
    }
}
