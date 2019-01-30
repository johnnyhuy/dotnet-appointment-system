using System;
using System.Linq;
using System.Net;
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
                RoomId = "A",
                StaffId = LoggedInStaff.Id,
                StudentId = LoggedInStudent.Id,
                StartTime = new DateTime(2019, 1, 1, 13, 0, 0)
            };

            Context.Slot.Add(slot);

            await Context.SaveChangesAsync();

            var updateSlot = new Slot
            {
                StudentId = "s3604367"
            };

            // Act
            dynamic result = ApiSlotController.Put(slot.RoomId, slot.StartTime.Value.Date, slot.StartTime.Value, updateSlot);

            // Assert
            Assert.IsAssignableFrom<OkResult>(result);
            
            Assert.True(Context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime && s.StudentId == "s3604367"));
        }
        
        [Fact]
        public async void PutSlot_CancelStudent_ReturnOk()
        {
            // Arrange
            var slot = new Slot
            {
                RoomId = "A",
                StaffId = LoggedInStaff.Id,
                StudentId = LoggedInStudent.Id,
                StartTime = new DateTime(2019, 1, 1, 13, 0, 0)
            };

            Context.Slot.Add(slot);

            await Context.SaveChangesAsync();

            var updateSlot = new Slot
            {
                StudentId = null
            };

            // Act
            dynamic result = ApiSlotController.Put(slot.RoomId, slot.StartTime.Value.Date, slot.StartTime.Value, updateSlot);

            // Assert
            Assert.IsAssignableFrom<OkResult>(result);
            
            Assert.True(Context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime && string.IsNullOrEmpty(s.StudentId)));
        }
        
        [Fact]
        public async void PutSlot_WithNonExistentSlot_ReturnNotFound()
        {
            // Arrange
            var slot = new Slot
            {
                RoomId = "A",
                StaffId = LoggedInStaff.Id,
                StudentId = LoggedInStudent.Id,
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
            dynamic result = ApiSlotController.Put(slot.RoomId, slot.StartTime.Value.Date, slot.StartTime.Value.AddHours(1), updateSlot);

            // Assert
            Assert.IsAssignableFrom<JsonResult>(result);
            Assert.Equal("Slot does not exist.", result.Value.Message);
            Assert.Equal((int) HttpStatusCode.NotFound, result.StatusCode);
            
            Assert.True(Context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime && s.StudentId == slot.StudentId));
        }
        
        [Fact]
        public async void PutSlot_WithNonExistentRoom_ReturnNotFound()
        {
            // Arrange
            var slot = new Slot
            {
                RoomId = "A",
                StaffId = LoggedInStaff.Id,
                StudentId = LoggedInStudent.Id,
                StartTime = new DateTime(2019, 1, 1, 13, 0, 0)
            };

            Context.Slot.Add(slot);

            await Context.SaveChangesAsync();

            var updateSlot = new Slot
            {
                StudentId = LoggedInStudent.StudentId,
                StartTime = slot.StartTime
            };

            // Act
            dynamic result = ApiSlotController.Put("Z", slot.StartTime.Value.Date, slot.StartTime.Value, updateSlot);

            // Assert
            Assert.IsAssignableFrom<JsonResult>(result);
            Assert.Equal("Room does not exist.", result.Value.Message);
            Assert.Equal((int) HttpStatusCode.NotFound, result.StatusCode);
            
            Assert.True(Context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime && s.StudentId == slot.StudentId));
        }
        
        [Fact]
        public async void PutSlot_WithNonExistentStudent_ReturnNotFound()
        {
            // Arrange
            var slot = new Slot
            {
                RoomId = "A",
                StaffId = LoggedInStaff.Id,
                StudentId = LoggedInStudent.Id,
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
            dynamic result = ApiSlotController.Put(slot.RoomId, slot.StartTime.Value.Date, slot.StartTime.Value, updateSlot);

            // Assert
            Assert.IsAssignableFrom<JsonResult>(result);
            Assert.Equal("Student does not exist.", result.Value.Message);
            Assert.Equal((int) HttpStatusCode.NotFound, result.StatusCode);
            
            Assert.True(Context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime && s.StudentId == slot.StudentId));
        }
    }
}