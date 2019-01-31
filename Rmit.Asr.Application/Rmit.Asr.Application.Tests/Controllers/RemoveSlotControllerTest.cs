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
    public class RemoveSlotControllerTest : ControllerBaseTest
    {
        [Fact]
        public async Task RemoveSlot_WithStudentBookedInSlot_ReturnFail()
        {
            // Arrange
            UserLoggedIn(StaffUsername);
            
            var slot = new RemoveSlot
            {
                RoomId = RoomA.Id,
                Room = RoomA,
                StartTime = new DateTime(2019, 1, 1, 13, 0, 0)
            };

            var createdSlot = new Slot
            {
                RoomId = RoomA.Id,
                Room = RoomA,
                StaffId = Staff.Id,
                StudentId = "s1234567",
                StartTime = new DateTime(2019, 1, 1, 13, 0, 0)
            };

            Context.Slot.Add(createdSlot);

            await Context.SaveChangesAsync();

            // Act
            IActionResult result = await SlotController.Remove(slot);

            // Assert
            IEnumerable<string> errorMessages = SlotController.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage);
            
            Assert.Contains(errorMessages, e => e == "Cannot remove slot as a student has been booked into it.");
            Assert.False(SlotController.ModelState.IsValid);
            
            Assert.IsType<ViewResult>(result);

            Assert.True(Context.Slot.Any(s => s.RoomId == createdSlot.RoomId && s.StartTime == createdSlot.StartTime));
        }
        
        [Fact]
        public async Task RemoveSlot_WithANonExistentSlot_ReturnFail()
        {
            // Arrange
            UserLoggedIn(StaffUsername);
            
            var slot = new RemoveSlot
            {
                RoomId = "ASS",
                StartTime = new DateTime(2019, 1, 1, 13, 0, 0)
            };

            var createdSlot = new Slot
            {
                RoomId = RoomA.Id,
                Room = RoomA,
                StaffId = Staff.Id,
                StudentId = "s1234567",
                StartTime = new DateTime(2019, 1, 1, 13, 0, 0)
            };

            Context.Slot.Add(createdSlot);

            await Context.SaveChangesAsync();

            // Act
            IActionResult result = await SlotController.Remove(slot);

            // Assert
            IEnumerable<string> errorMessages = SlotController.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage);
            
            Assert.Contains(errorMessages, e => e == "Slot does not exist.");
            Assert.False(SlotController.ModelState.IsValid);
            
            Assert.IsType<ViewResult>(result);

            Assert.True(Context.Slot.Any(s => s.RoomId == createdSlot.RoomId && s.StartTime == createdSlot.StartTime));
        }
    }
}