using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Rmit.Asr.Application.Models;
using Xunit;

namespace Rmit.Asr.Application.Tests.Controllers.Api
{
    public class SlotControllerTest : ControllerBaseTest
    {
        [Fact]
        public async void IndexSlot_WithValidParameters_ReturnSlots()
        {
            // Arrange
            var slots = new List<Slot>
            {
                new Slot
                {
                    RoomId = "A",
                    StaffId = StaffId,
                    StudentId = null,
                    StartTime = new DateTime(2019, 1, 1, 13, 0, 0)
                },
                new Slot
                {
                    RoomId = "B",
                    StaffId = StaffId,
                    StudentId = StudentId,
                    StartTime = new DateTime(2019, 1, 1, 9, 0, 0)
                },
                new Slot
                {
                    RoomId = "B",
                    StaffId = StaffId,
                    StudentId = null,
                    StartTime = new DateTime(2019, 1, 2, 13, 0, 0)
                }
            };

            await Context.Slot.AddRangeAsync(slots);
            
            await Context.SaveChangesAsync();    

            // Act
            ActionResult<IEnumerable<Slot>> result = ApiSlotController.Index();

            // Assert
            Assert.True(result.Value.Count() == slots.Count);
        }
        
        [Fact]
        public async void StudentIndexSlot_WithValidParameters_ReturnSlots()
        {
            // Arrange
            var slots = new List<Slot>
            {
                new Slot
                {
                    RoomId = "A",
                    StaffId = StaffId,
                    StudentId = null,
                    StartTime = new DateTime(2019, 1, 1, 13, 0, 0)
                },
                new Slot
                {
                    RoomId = "B",
                    StaffId = StaffId,
                    StudentId = StudentId,
                    StartTime = new DateTime(2019, 1, 1, 9, 0, 0)
                },
                new Slot
                {
                    RoomId = "B",
                    StaffId = StaffId,
                    StudentId = null,
                    StartTime = new DateTime(2019, 1, 2, 13, 0, 0)
                }
            };

            await Context.Slot.AddRangeAsync(slots);
            
            await Context.SaveChangesAsync();    

            // Act
            ActionResult<IEnumerable<Slot>> result = ApiSlotController.StudentIndex(StudentId);

            // Assert
            Assert.Single(result.Value);
        }
        
        [Fact]
        public async void StaffIndexSlot_WithValidParameters_ReturnSlots()
        {
            // Arrange
            var slots = new List<Slot>
            {
                new Slot
                {
                    RoomId = "A",
                    StaffId = StaffId,
                    StudentId = null,
                    StartTime = new DateTime(2019, 1, 1, 13, 0, 0)
                },
                new Slot
                {
                    RoomId = "B",
                    StaffId = "e54321",
                    StudentId = StudentId,
                    StartTime = new DateTime(2019, 1, 1, 9, 0, 0)
                },
                new Slot
                {
                    RoomId = "B",
                    StaffId = StaffId,
                    StudentId = StudentId,
                    StartTime = new DateTime(2019, 1, 2, 13, 0, 0)
                }
            };

            await Context.Slot.AddRangeAsync(slots);
            
            await Context.SaveChangesAsync();    

            // Act
            ActionResult<IEnumerable<Slot>> result = ApiSlotController.StaffIndex(StaffId);

            // Assert
            Assert.Equal(2, result.Value.Count());
        }
        
        [Fact]
        public async void PutSlot_WithValidParameters_ReturnOk()
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
            values.StudentId.Value = "s3604367";
            values.StartTime.Value = slot.StartTime.ToString();

            // Act
            dynamic result = ApiSlotController.Put(slot.RoomId, values);

            // Assert
            Assert.IsAssignableFrom<OkResult>(result);
            
            Assert.True(Context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime && s.StudentId == "s3604367"));
        }
        
        [Fact]
        public async void PutSlot_WithNonExistentSlot_ReturnNotFound()
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
            values.StudentId.Value = "s3604367";
            values.StartTime.Value = new DateTime(2019, 1, 1, 14, 0, 0).ToString(CultureInfo.InvariantCulture);

            // Act
            dynamic result = ApiSlotController.Put(slot.RoomId, values);

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