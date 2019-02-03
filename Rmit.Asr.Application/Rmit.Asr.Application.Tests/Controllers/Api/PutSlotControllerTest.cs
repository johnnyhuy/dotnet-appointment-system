using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Rmit.Asr.Application.Models;
using Xunit;

namespace Rmit.Asr.Application.Tests.Controllers.Api
{
    public class PutSlotControllerTest : ControllerBaseTest
    {
        [Fact]
        public async void PutSlot_BookStudent_ReturnOk()
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

            var updateSlot = new Slot
            {
                StudentId = OtherStudent.StudentId
            };

            // Act
            ActionResult result = ApiSlotController.Put(RoomA.Name, slot.StartTime.Value.Date, slot.StartTime.Value, updateSlot);

            // Assert
            Assert.IsAssignableFrom<OkResult>(result);
            
            Assert.True(Context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime && s.StudentId == OtherStudent.Id));
        }
        
        [Fact]
        public async void PutSlot_CancelStudent_ReturnOk()
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

            var updateSlot = new Slot
            {
                StudentId = null
            };

            // Act
            ActionResult result = ApiSlotController.Put(RoomA.Name, slot.StartTime.Value.Date, slot.StartTime.Value, updateSlot);

            // Assert
            Assert.IsAssignableFrom<OkResult>(result);
            
            Assert.True(Context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime && string.IsNullOrEmpty(s.StudentId)));
        }
        
        [Fact]
        public async void PutSlot_WithNonExistentSlot_ReturnBadRequest()
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

            var updateSlot = new Slot
            {
                StudentId = "s3604367",
                StartTime = new DateTime(2019, 1, 1, 14, 0, 0)
            };

            // Act
            ActionResult result = ApiSlotController.Put(RoomA.Name, slot.StartTime.Value.Date, slot.StartTime.Value.AddHours(1), updateSlot);

            // Assert
            var badRequest = Assert.IsAssignableFrom<BadRequestObjectResult>(result);
            var serializableError = (SerializableError) badRequest.Value;
            string[] errors = serializableError.Values.Select(e => (string[]) e).First();
            Assert.Contains("Slot does not exist.", errors);
            
            Assert.True(Context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime && s.StudentId == slot.StudentId));
        }
        
        [Fact]
        public async void PutSlot_WithNonExistentRoom_ReturnBadRequest()
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

            var updateSlot = new Slot
            {
                StudentId = Student.StudentId,
                StartTime = slot.StartTime
            };

            // Act
            ActionResult result = ApiSlotController.Put("Z", slot.StartTime.Value.Date, slot.StartTime.Value, updateSlot);

            // Assert
            var badRequest = Assert.IsAssignableFrom<BadRequestObjectResult>(result);
            var serializableError = (SerializableError) badRequest.Value;
            string[] errors = serializableError.Values.Select(e => (string[]) e).First();
            Assert.Contains("Room does not exist.", errors);
            
            Assert.True(Context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime && s.StudentId == slot.StudentId));
        }
        
        [Fact]
        public async void PutSlot_WithNonExistentStudent_ReturnBadRequest()
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

            var updateSlot = new Slot
            {
                StudentId = "asd",
                StartTime = slot.StartTime
            };

            // Act
            ActionResult result = ApiSlotController.Put(RoomA.Name, slot.StartTime.Value.Date, slot.StartTime.Value, updateSlot);

            // Assert
            var badRequest = Assert.IsAssignableFrom<BadRequestObjectResult>(result);
            var serializableError = (SerializableError) badRequest.Value;
            string[] errors = serializableError.Values.Select(e => (string[]) e).First();
            Assert.Contains("Student does not exist.", errors);
            
            Assert.True(Context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime && s.StudentId == slot.StudentId));
        }
    }
}