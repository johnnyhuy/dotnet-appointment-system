using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rmit.Asr.Application.Models;
using Rmit.Asr.Application.Models.ViewModels;
using Xunit;

namespace Rmit.Asr.Application.Tests.Controllers
{
    public class CancelSlotControllerTest : ControllerBaseTest
    {
        [Fact]
        public async Task CancelSlot_WithValidParameters_ReturnSuccess()
        {
            // Arrange
            UserLoggedIn(StudentUsername);
            
            var createdSlot = new Slot
            {
                RoomId = RoomA.Id,
                Room = RoomA,
                StaffId = Staff.Id,
                StudentId = Student.Id,
                StartTime = new DateTime(2019, 1, 1, 13, 0, 0),
                Student = new Student
                {
                    Id = Student.Id,
                    StudentId = StudentId,
                    FirstName = "Johnny",
                    LastName = "Doe",
                    Email = StudentEmail
                }
            };

            Context.Slot.Add(createdSlot);
            
            await Context.SaveChangesAsync();
            
            var slot = new CancelSlot
            {
                RoomId = createdSlot.RoomId,
                StartTime = createdSlot.StartTime
            };

            // Act
            IActionResult result = await SlotController.Cancel(slot);

            // Assert
            Assert.Empty(SlotController.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage));
            Assert.True(SlotController.ModelState.IsValid);
            
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", viewResult.ActionName);

            Assert.True(Context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime && string.IsNullOrEmpty(s.StudentId)));
        }
        
        [Fact]
        public async Task CancelSlot_WithNoStudentBooked_ReturnFail()
        {
            // Arrange
            UserLoggedIn(StudentUsername);
            
            var createdSlot = new Slot
            {
                RoomId = RoomA.Id,
                Room = RoomA,
                StaffId = Staff.Id,
                StartTime = new DateTime(2019, 1, 1, 13, 0, 0),
            };
            
            var slot = new CancelSlot
            {
                RoomId = createdSlot.RoomId,
                StartTime = createdSlot.StartTime
            };

            Context.Slot.Add(createdSlot);

            await Context.SaveChangesAsync();

            // Act
            IActionResult result = await SlotController.Cancel(slot);

            // Assert
            IEnumerable<string> errorMessages = SlotController.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage);
            
            Assert.Contains(errorMessages, e => e == "No student is booked in room.");
            Assert.False(SlotController.ModelState.IsValid);
            
            Assert.IsType<ViewResult>(result);

            Assert.False(Context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime && s.StudentId == slot.StudentId));
        }
        
        [Fact]
        public async Task CancelSlot_WithNonExistentRoom_ReturnFail()
        {
            // Arrange
            UserLoggedIn(StudentUsername);
            
            var createdSlot = new Slot
            {
                RoomId = RoomA.Id,
                Room = RoomA,
                StaffId = Staff.Id,
                StudentId = null,
                StartTime = new DateTime(2019, 1, 1, 13, 0, 0)
            };

            Context.Slot.Add(createdSlot);
            
            await Context.SaveChangesAsync();
            
            var slot = new CancelSlot
            {
                RoomId = "YEET",
                StartTime = createdSlot.StartTime
            };

            // Act
            IActionResult result = await SlotController.Cancel(slot);

            // Assert
            IEnumerable<string> errorMessages = SlotController.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage);
            
            Assert.Contains(errorMessages, e => e == "Room does not exist.");
            Assert.False(SlotController.ModelState.IsValid);
            
            Assert.IsType<ViewResult>(result);

            Assert.False(Context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime && s.StudentId == slot.StudentId));
        }
        
        [Fact]
        public async Task CancelSlot_WithNonExistentSlot_ReturnFail()
        {
            // Arrange
            UserLoggedIn(StudentUsername);
            
            var createdSlot = new Slot
            {
                RoomId = RoomA.Id,
                Room = RoomA,
                StaffId = Staff.Id,
                StudentId = null,
                StartTime = new DateTime(2019, 1, 1, 13, 0, 0)
            };

            Context.Slot.Add(createdSlot);
            
            await Context.SaveChangesAsync();
            
            var slot = new CancelSlot
            {
                RoomId = "B",
                StaffId = createdSlot.StaffId,
                StartTime = new DateTime(2019, 1, 1, 9, 0, 0)
            };

            // Act
            IActionResult result = await SlotController.Cancel(slot);

            // Assert
            IEnumerable<string> errorMessages = SlotController.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage);
            
            Assert.Contains(errorMessages, e => e == "Slot does not exist.");
            Assert.False(SlotController.ModelState.IsValid);
            
            Assert.IsType<ViewResult>(result);

            Assert.False(Context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime && s.StudentId == slot.StudentId));
        }
    }
}