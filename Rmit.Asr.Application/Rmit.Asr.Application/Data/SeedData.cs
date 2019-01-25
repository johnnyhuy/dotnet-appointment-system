using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Rmit.Asr.Application.Models;

namespace Rmit.Asr.Application.Data
{
    /// <summary>
    /// This is the data seeding class used to populate data upon startup.
    /// </summary>
    public static class SeedData
    {
        /// <summary>
        /// General seeding of data
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static async Task Seed(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = { "Student", "Staff" };

            foreach (string roleName in roleNames)
            {
                bool roleExist = await roleManager.RoleExistsAsync(roleName);
                
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            using (var context = new ApplicationDataContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDataContext>>()))
            {
                if (context.Room.Any() || context.Slot.Any())
                {
                    return;
                }

                context.Room.AddRange(
                    new Room
                    {
                        RoomId = "A"
                    },

                    new Room
                    {
                        RoomId = "B"
                    },

                    new Room
                    {
                        RoomId = "C"
                    },

                    new Room
                    {
                        RoomId = "D"
                    }
                );

                // Doesn't work - no staff table in database or somthing
                context.Staff.AddRange(
                    new Staff
                    {
                        Id = "e12345",
                        FirstName = "Shawn",
                        LastName = "Taylor",
                        Email = "e12345@rmit.edu.au"
                    },

                    new Staff
                    {
                        Id = "e54321",
                        FirstName = "Bob",
                        LastName = "Doe",
                        Email = "e54321@rmit.edu.au"
                    }
                );

                context.SaveChanges();
            }
        }
    }
}