using System;
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
            // Check no validation errors have occured
            Assert.Empty(controller.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage));
            Assert.True(controller.ModelState.IsValid);
            
            // Check the controller redirected
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", viewResult.ActionName);

            // Check slot exists in mock database
            Assert.True(_context.Slot.Any(s => s == slot));
        }
        
//        [Fact]
//        public async Task CreateSlot_WithNonExistentRoom_ReturnSuccess()
//        {
//            // Arrange
//            var controller = new StaffMenuController(_context);
//            var slot = new Slot
//            {
//                RoomId = "Z",
//                StaffId = "e12345",
//                StartTime = new DateTime(2019, 1, 1, 8, 0, 0)
//            };
//
//            // Act
//            IActionResult result = await controller.CreateSlot(slot);
//
//            // Assert
//            // Check no validation errors have occured
//            Assert.Empty(controller.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage));
//            Assert.True(controller.ModelState.IsValid);
//            
//            // Check the controller redirected
//            var viewResult = Assert.IsType<RedirectToActionResult>(result);
//            Assert.Equal("Index", viewResult.ActionName);
//
//            // Check slot exists in mock database
//            Assert.True(_context.Slot.Any(s => s != slot));
//        }

        public void Dispose()
        {
        }
    }
}