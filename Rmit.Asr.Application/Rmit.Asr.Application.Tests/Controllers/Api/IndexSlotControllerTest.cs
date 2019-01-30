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
    }
}