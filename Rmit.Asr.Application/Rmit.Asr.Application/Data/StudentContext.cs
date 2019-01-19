using Microsoft.EntityFrameworkCore;
using Rmit.Asr.Application.Models;

namespace Rmit.Asr.Application.Data
{
    public class StudentContext : DbContext
    {
        public StudentContext (DbContextOptions<StudentContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Student { get; set; }
    }
}
