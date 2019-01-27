using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rmit.Asr.Application.Controllers;
using Rmit.Asr.Application.Models;
using Xunit;

namespace Rmit.Asr.Application.Tests.Controllers
{
    public class StaffControllerTest : ControllerBaseTest
    {
        [Fact]
        public async Task CreateSlot_WithValidParameters_ReturnSuccess()
        {
            // Arrange
            var controller = new StaffMenuController(Context);
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
            Assert.True(Context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime));
        }
        
        [Fact]
        public async Task CreateSlot_WithNonExistentRoom_ReturnSuccess()
        {
            // Arrange
            var controller = new StaffMenuController(Context);
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
            Assert.False(Context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime));
        }
        
        [Fact]
        public async Task CreateSlot_WithNonExistentStaff_ReturnFail()
        {
            // Arrange
            var controller = new StaffMenuController(Context);
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
            Assert.False(Context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime));
        }
        
        [Fact]
        public async Task CreateSlot_WithOverMaximumRoomBooking_ReturnFail()
        {
            // Arrange
            var controller = new StaffMenuController(Context);
            var slot = new Slot
            {
                RoomId = "A",
                StaffId = "e12345",
                StartTime = new DateTime(2019, 1, 1, 9, 0, 0)
            };

            Context.Slot.AddRange(
                new Slot
                {
                    RoomId = "A",
                    StaffId = "e12345",
                    StartTime = new DateTime(2019, 1, 1, 12, 0, 0)
                },
                new Slot
                {
                    RoomId = "A",
                    StaffId = "e12345",
                    StartTime = new DateTime(2019, 1, 1, 14, 0, 0)
                }
            );

            Context.SaveChanges();

            // Act
            IActionResult result = await controller.CreateSlot(slot);

            // Assert
            IEnumerable<string> errorMessages = controller.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage);
            
            // No validation errors have occured
            Assert.Contains(errorMessages, e => e == $"Room {slot.RoomId} has reached a maximum booking of {Room.MaxRoomBookingPerDay} per day.");
            Assert.False(controller.ModelState.IsValid);
            
            // Controller returns a view
            Assert.IsType<ViewResult>(result);

            // Slot does not exist in the mock database
            Assert.False(Context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime));
        }
        
        [Fact]
        public async Task CreateSlot_WithStaffOverMaximumBooking_ReturnFail()
        {
            // Arrange
            var controller = new StaffMenuController(Context);
            var slot = new Slot
            {
                RoomId = "D",
                StaffId = "e12345",
                StartTime = new DateTime(2019, 1, 1, 7, 2, 2)
            };

            Context.Slot.AddRange(
                new Slot
                {
                    RoomId = "A",
                    StaffId = "e12345",
                    StartTime = new DateTime(2019, 1, 1, 12, 0, 0)
                },
                new Slot
                {
                    RoomId = "A",
                    StaffId = "e12345",
                    StartTime = new DateTime(2019, 1, 1, 14, 0, 0)
                },
                new Slot
                {
                    RoomId = "B",
                    StaffId = "e12345",
                    StartTime = new DateTime(2019, 1, 1, 10, 0, 0)
                },
                new Slot
                {
                    RoomId = "B",
                    StaffId = "e12345",
                    StartTime = new DateTime(2019, 1, 1, 9, 0, 0)
                }
            );

            Context.SaveChanges();

            // Act
            IActionResult result = await controller.CreateSlot(slot);

            // Assert
            IEnumerable<string> errorMessages = controller.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage);
            
            // No validation errors have occured
            Assert.Contains(errorMessages, e => e == $"Staff {slot.StaffId} has a maximum of {Staff.MaxBookingPerDay} bookings at {slot.StartTime:dd-MM-yyyy}.");
            Assert.False(controller.ModelState.IsValid);
            
            // Controller returns a view
            Assert.IsType<ViewResult>(result);

            // Slot does not exist in the mock database
            Assert.False(Context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime));
        }
    }
}