using System;
using System.Dynamic;
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
        public async void PutSlot_WithNonExistentRoom_ReturnNotFound()
        {
            // Arrange
            var slot = new Slot
            {
                RoomId = "A",
                StaffId = StaffId,
                StudentId = StudentId,
                StartTime = new DateTime(2019, 1, 1, 13, 0, 0)
            };

            Context.Slot.Add(slot);

            await Context.SaveChangesAsync();

            dynamic values = new ExpandoObject();
            values.StudentId = new ExpandoObject();
            values.StartTime = new ExpandoObject();
            values.StudentId.Value = slot.StudentId;
            values.StartTime.Value = slot.StartTime.ToString();

            // Act
            dynamic result = ApiSlotController.Put("Z", values);

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
                StaffId = StaffId,
                StudentId = StudentId,
                StartTime = new DateTime(2019, 1, 1, 13, 0, 0)
            };

            Context.Slot.Add(slot);

            await Context.SaveChangesAsync();

            dynamic values = new ExpandoObject();
            values.StudentId = new ExpandoObject();
            values.StartTime = new ExpandoObject();
            values.StudentId.Value = "asd";
            values.StartTime.Value = slot.StartTime.ToString();

            // Act
            dynamic result = ApiSlotController.Put(slot.RoomId, values);

            // Assert
            Assert.IsAssignableFrom<JsonResult>(result);
            Assert.Equal("Student does not exist.", result.Value.Message);
            Assert.Equal((int) HttpStatusCode.NotFound, result.StatusCode);
            
            Assert.True(Context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime && s.StudentId == slot.StudentId));
        }
    }
}