using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Rmit.Asr.Application.Models;
using Xunit;

namespace Rmit.Asr.Application.Tests.Controllers.Api
{
    public class IndexSlotControllerTest : ControllerBaseTest
    {
        [Fact]
        public async void IndexSlot_WithValidParameters_ReturnSlots()
        {
            // Arrange
            var slots = new List<Slot>
            {
                new Slot
                {
                    RoomId = RoomA.Id,
                    Room = RoomA,
                    StaffId = Staff.Id,
                    StudentId = null,
                    StartTime = new DateTime(2019, 1, 1, 13, 0, 0)
                },
                new Slot
                {
                    RoomId = RoomB.Name,
                    Room = RoomB,
                    StaffId = Staff.Id,
                    StudentId = Student.Id,
                    StartTime = new DateTime(2019, 1, 1, 9, 0, 0)
                },
                new Slot
                {
                    RoomId = RoomB.Name,
                    Room = RoomB,
                    StaffId = Staff.Id,
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
                    RoomId = RoomA.Id,
                    Room = RoomA,
                    StaffId = Staff.Id,
                    StudentId = null,
                    StartTime = new DateTime(2019, 1, 1, 13, 0, 0)
                },
                new Slot
                {
                    RoomId = RoomB.Name,
                    Room = RoomB,
                    StaffId = Staff.Id,
                    StudentId = Student.Id,
                    StartTime = new DateTime(2019, 1, 1, 9, 0, 0)
                },
                new Slot
                {
                    RoomId = RoomB.Name,
                    Room = RoomB,
                    StaffId = Staff.Id,
                    StudentId = null,
                    StartTime = new DateTime(2019, 1, 2, 13, 0, 0)
                }
            };

            await Context.Slot.AddRangeAsync(slots);
            
            await Context.SaveChangesAsync();    
            
            var student = new Student
            {
                StudentId = Student.StudentId
            };

            // Act
            ActionResult<IEnumerable<Slot>> result = ApiSlotController.StudentIndex(student);

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
                    RoomId = RoomA.Id,
                    Room = RoomA,
                    StaffId = Staff.Id,
                    StudentId = null,
                    StartTime = new DateTime(2019, 1, 1, 13, 0, 0)
                },
                new Slot
                {
                    RoomId = RoomB.Name,
                    Room = RoomB,
                    StaffId = "e54321",
                    StudentId = Student.Id,
                    StartTime = new DateTime(2019, 1, 1, 9, 0, 0)
                },
                new Slot
                {
                    RoomId = RoomB.Name,
                    Room = RoomB,
                    StaffId = Staff.Id,
                    StudentId = Student.Id,
                    StartTime = new DateTime(2019, 1, 2, 13, 0, 0)
                }
            };

            var staff = new Staff
            {
                StaffId = Staff.StaffId
            };

            await Context.Slot.AddRangeAsync(slots);
            
            await Context.SaveChangesAsync();    

            // Act
            ActionResult<IEnumerable<Slot>> result = ApiSlotController.StaffIndex(staff);

            // Assert
            Assert.Equal(2, result.Value.Count());
        }
        
        [Fact]
        public async void StaffIndexSlot_WithNonExistentStaff_ReturnBadRequest()
        {
            // Arrange
            var slots = new List<Slot>
            {
                new Slot
                {
                    RoomId = RoomA.Id,
                    Room = RoomA,
                    StaffId = Staff.Id,
                    StudentId = null,
                    StartTime = new DateTime(2019, 1, 1, 13, 0, 0)
                },
                new Slot
                {
                    RoomId = RoomB.Name,
                    Room = RoomB,
                    StaffId = "e54321",
                    StudentId = Student.Id,
                    StartTime = new DateTime(2019, 1, 1, 9, 0, 0)
                },
                new Slot
                {
                    RoomId = RoomB.Name,
                    Room = RoomB,
                    StaffId = Staff.Id,
                    StudentId = Student.Id,
                    StartTime = new DateTime(2019, 1, 2, 13, 0, 0)
                }
            };

            var staff = new Staff
            {
                StaffId = "I should not exist"
            };

            await Context.Slot.AddRangeAsync(slots);
            
            await Context.SaveChangesAsync();    

            // Act
            ActionResult<IEnumerable<Slot>> result = ApiSlotController.StaffIndex(staff);

            // Assert
            var badRequest = Assert.IsAssignableFrom<BadRequestObjectResult>(result.Result);
            var serializableError = (SerializableError) badRequest.Value;
            string[] errors = serializableError.Values.Select(e => (string[]) e).First();
            Assert.Contains("Staff does not exist.", errors);
        }
        
        [Fact]
        public async void StudentIndexSlot_WithNonExistentStudent_ReturnBadRequest()
        {
            // Arrange
            var slots = new List<Slot>
            {
                new Slot
                {
                    RoomId = RoomA.Id,
                    Room = RoomA,
                    StaffId = Staff.Id,
                    StudentId = null,
                    StartTime = new DateTime(2019, 1, 1, 13, 0, 0)
                },
                new Slot
                {
                    RoomId = RoomB.Name,
                    Room = RoomB,
                    StaffId = "e54321",
                    StudentId = Student.Id,
                    StartTime = new DateTime(2019, 1, 1, 9, 0, 0)
                },
                new Slot
                {
                    RoomId = RoomB.Name,
                    Room = RoomB,
                    StaffId = Staff.Id,
                    StudentId = Student.Id,
                    StartTime = new DateTime(2019, 1, 2, 13, 0, 0)
                }
            };

            var student = new Student
            {
                StudentId = "I should not exist"
            };

            await Context.Slot.AddRangeAsync(slots);
            
            await Context.SaveChangesAsync();    

            // Act
            ActionResult<IEnumerable<Slot>> result = ApiSlotController.StudentIndex(student);

            // Assert
            var badRequest = Assert.IsAssignableFrom<BadRequestObjectResult>(result.Result);
            var serializableError = (SerializableError) badRequest.Value;
            string[] errors = serializableError.Values.Select(e => (string[]) e).First();
            Assert.Contains("Student does not exist.", errors);
        }
    }
}