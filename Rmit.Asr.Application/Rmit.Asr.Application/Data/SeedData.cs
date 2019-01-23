using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Rmit.Asr.Application.Data;

namespace Rmit.Asr.Application.Models
{
    public static class SeedData
    {

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDataContext( serviceProvider.GetRequiredService< DbContextOptions<ApplicationDataContext>>()))
            {

                if (context.Room.Any() || context.Slot.Any())
                {
                    return;   // DB has been seeded
                }

                context.Room.AddRange(
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
                    }
                );

                // Doesn't work - no staff table in database or somthing
                //context.Staff.AddRange(
                //    new Staff
                //    {
                //        StaffID = "e12345",
                //        Name = "shawn",
                //        Email = "e12345@rmit.edu.au"
                //    },

                //    new Staff
                //    {
                //        StaffID = "e54321",
                //        Name = "bob",
                //        Email = "e54321@rmit.edu.au"
                //    }
                //);

                context.SaveChanges();
            }
        }
    }
}
