using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rmit.Asr.Application.Controllers;
using Rmit.Asr.Application.Data;
using Rmit.Asr.Application.Models;
using Xunit;

namespace Rmit.Asr.Application.Tests.Controllers
{
    public class StaffControllerTest : IDisposable
    {
        private readonly ApplicationDataContext _context;

        public StaffControllerTest()
        {
            DbContextOptions options = new DbContextOptionsBuilder<DbContext>()
                .UseInMemoryDatabase("SomeDatabase")
                .Options;

            _context = new ApplicationDataContext(options);

            Seed();
        }
        
        public void Dispose()
        {
            _context.Dispose();
        }

        private void Seed()
        {
            if (_context.Room.Any() || _context.Slot.Any() || _context.Users.Any())
            {
                return;
            }
            
            _context.Room.AddRange(
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
            
            _context.Staff.AddRange(
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
            
            _context.Student.AddRange(
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

            _context.SaveChanges();
        }

        [Fact]
        public async Task CreateSlot_WithValidParameters_ReturnSuccess()
        {
            // Arrange
            var controller = new StaffMenuController(_context);
            var slot = new Slot
            {
                RoomId = "A",
                StaffId = "e12345",
                StartTime = new DateTime(2019, 1, 1, 8, 0, 0)
            };

            // Act
            IActionResult result = await controller.CreateSlot(slot);

            // Assert
            // No validation errors have occured
            Assert.Empty(controller.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage));
            Assert.True(controller.ModelState.IsValid);
            
            // Controller redirected
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", viewResult.ActionName);

            // Slot exists in mock database
            Assert.True(_context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime));
        }
        
        [Fact]
        public async Task CreateSlot_WithNonExistentRoom_ReturnSuccess()
        {
            // Arrange
            var controller = new StaffMenuController(_context);
            var slot = new Slot
            {
                RoomId = "Z",
                StaffId = "e12345",
                StartTime = new DateTime(2019, 1, 1, 8, 0, 0)
            };

            // Act
            IActionResult result = await controller.CreateSlot(slot);

            // Assert
            IEnumerable<string> errorMessages = controller.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage);
            
            // No validation errors have occured
            Assert.Contains(errorMessages, e => e == $"Room {slot.RoomId} does not exist.");
            Assert.False(controller.ModelState.IsValid);
            
            // Controller returns a view
            Assert.IsType<ViewResult>(result);

            // Slot does not exist in the mock database
            Assert.False(_context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime));
        }
        
        [Fact]
        public async Task CreateSlot_WithNonExistentStaff_ReturnSuccess()
        {
            // Arrange
            var controller = new StaffMenuController(_context);
            var slot = new Slot
            {
                RoomId = "A",
                StaffId = "e11111",
                StartTime = new DateTime(2019, 1, 1, 8, 0, 0)
            };

            // Act
            IActionResult result = await controller.CreateSlot(slot);

            // Assert
            IEnumerable<string> errorMessages = controller.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage);
            
            // No validation errors have occured
            Assert.Contains(errorMessages, e => e == $"Staff {slot.StaffId} does not exist.");
            Assert.False(controller.ModelState.IsValid);
            
            // Controller returns a view
            Assert.IsType<ViewResult>(result);

            // Slot does not exist in the mock database
            Assert.False(_context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime));
        }
    }
}