using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Rmit.Asr.Application.Models;
using Xunit;

namespace Rmit.Asr.Application.Tests.Controllers.Api
{
    public class DeleteSlotControllerTest : ControllerBaseTest
    {
        [Fact]
        public async void DeleteSlot_WithValidParameters_ReturnOk()
        {
            // Arrange
            var slot = new Slot
            {
                RoomId = RoomA.Id,
                Room = RoomA,
                StaffId = Staff.Id,
                StudentId = Student.Id,
                StartTime = new DateTime(2019, 1, 1, 13, 0, 0)
            };

            Context.Slot.Add(slot);

            await Context.SaveChangesAsync();

            // Act
            ActionResult result = ApiSlotController.Delete(RoomA.Name, slot.StartTime.Value.Date, slot.StartTime.Value);

            // Assert
            Assert.IsAssignableFrom<OkResult>(result);
            
            Assert.False(Context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime));
        }
        
        [Fact]
        public async void DeleteSlot_WithNonExistentRoom_ReturnNotFound()
        {
            // Arrange
            var slot = new Slot
            {
                RoomId = RoomA.Id,
                Room = RoomA,
                StaffId = Staff.Id,
                StudentId = Student.Id,
                StartTime = new DateTime(2019, 1, 1, 13, 0, 0)
            };

            Context.Slot.Add(slot);

            await Context.SaveChangesAsync();

            // Act
            ActionResult result = ApiSlotController.Delete("ZZZ", slot.StartTime.Value, slot.StartTime.Value);

            // Assert
            var badRequest = Assert.IsAssignableFrom<BadRequestObjectResult>(result);
            var serializableError = (SerializableError) badRequest.Value;
            string[] errors = serializableError.Values.Select(e => (string[]) e).First();
            Assert.Contains("Room does not exist.", errors);
            
            Assert.True(Context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime && s.StudentId == slot.StudentId));
        }
        
        [Fact]
        public async void DeleteSlot_WithNonExistentSlot_ReturnNotFound()
        {
            // Arrange
            var slot = new Slot
            {
                RoomId = RoomA.Id,
                Room = RoomA,
                StaffId = Staff.Id,
                StudentId = Student.Id,
                StartTime = new DateTime(2019, 1, 1, 13, 0, 0)
            };

            Context.Slot.Add(slot);

            await Context.SaveChangesAsync();

            // Act
            ActionResult result = ApiSlotController.Delete("A", slot.StartTime.Value, slot.StartTime.Value.AddHours(1));

            // Assert
            var badRequest = Assert.IsAssignableFrom<BadRequestObjectResult>(result);
            var serializableError = (SerializableError) badRequest.Value;
            string[] errors = serializableError.Values.Select(e => (string[]) e).First();
            Assert.Contains("Slot does not exist.", errors);
            
            Assert.True(Context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime && s.StudentId == slot.StudentId));
        }
    }
}