using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rmit.Asr.Application.Models;
using System;
using System.Linq;

namespace Rmit.Asr.Application.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDataContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDataContext>>()))
            {
                // Look for any rooms.
                if (context.Rooms.Any())
                {
                    return; // DB has been seeded.
                }
                // Look for any staff members.
                if (context.Staffs.Any())
                {
                    return; // DB has been seeded.
                }

                var room = new[]
                {
                    new Room
                    {
                        RoomID = "A"
                    },
                    new Room
                    {
                        RoomID = "B"
                    },
                    new Room
                    {
                        RoomID = "C"
                    },
                    new Room
                    {
                        RoomID = "D"
                    },

                };

                var staff = new[]
                {
                    new Staff
                    {
                        StaffID = "e12345",
                        Name = "shawn",
                        Email = "e12345@rmit.edu.au"
                    },
                    new Staff
                    {
                        StaffID = "e54321",
                        Name = "bob",
                        Email = "e54321@rmit.edu.au"
                    }

                };

                context.Rooms.AddRange(room);
                context.Staffs.AddRange(staff);
                context.SaveChanges();
            }
        }
    }
}
