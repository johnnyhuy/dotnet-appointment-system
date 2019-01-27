using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Rmit.Asr.Application.Data;
using Rmit.Asr.Application.Models;

namespace Rmit.Asr.Application.Tests.Controllers
{
    public class ControllerBaseTest : IDisposable
    {
        protected readonly ApplicationDataContext Context;

        protected ControllerBaseTest()
        {
            DbContextOptions options = new DbContextOptionsBuilder<DbContext>()
                .UseInMemoryDatabase("SomeDatabase")
                .Options;

            Context = new ApplicationDataContext(options);

            Seed();
        }
        
        public void Dispose()
        {
            Context.Dispose();
        }

        private void Seed()
        {
            if (Context.Room.Any() || Context.Slot.Any() || Context.Users.Any())
            {
                return;
            }
            
            Context.Room.AddRange(
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
            
            Context.Staff.AddRange(
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
            
            Context.Student.AddRange(
                new Student
                {
                    Id = "s1234567",
                    FirstName = "Shawn",
                    LastName = "Taylor",
                    Email = "s1234567@student.rmit.edu.au"
                },
                new Student
                {
                    Id = "s3604367",
                    FirstName = "Johnny",
                    LastName = "Doe",
                    Email = "s3604367@student.rmit.edu.au"
                }
            );

            Context.SaveChanges();
        }
    }
}