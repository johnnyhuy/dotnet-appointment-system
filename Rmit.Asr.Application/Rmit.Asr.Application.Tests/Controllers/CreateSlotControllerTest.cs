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
    public class CreateSlotControllerTest : ControllerBaseTest
    {
        [Fact]
        public async Task CreateSlot_WithValidParameters_ReturnSuccess()
        {
            // Arrange
            UserLoggedIn(StaffUsername);
            
            var slot = new CreateSlot
            {
                RoomId = RoomC.Id,
                Room = RoomC,
                StaffId = Staff.Id,
                StartTime = DateTimeNow.AddHours(1)
            };

            // Act
            IActionResult result = await SlotController.Create(slot);

            // Assert
            Assert.Empty(SlotController.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage));
            Assert.True(SlotController.ModelState.IsValid);
            
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", viewResult.ActionName);

            Assert.True(Context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime));
        }
        
        [Fact]
        public async Task CreateSlot_WithNonExistentRoom_ReturnSuccess()
        {
            // Arrange
            UserLoggedIn(StaffUsername);
            
            var slot = new CreateSlot
            {
                RoomId = "Z",
                StaffId = Staff.Id,
                StartTime = new DateTime(2019, 1, 1, 8, 0, 0)
            };

            // Act
            IActionResult result = await SlotController.Create(slot);

            // Assert
            IEnumerable<string> errorMessages = SlotController.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage);
            
            Assert.Contains(errorMessages, e => e == "Room does not exist.");
            Assert.False(SlotController.ModelState.IsValid);
            
            Assert.IsType<ViewResult>(result);

            // Slot does not exist in the mock database
            Assert.False(Context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime));
        }
        
        [Fact]
        public async Task CreateSlot_WithOverMaximumRoomBooking_ReturnFail()
        {
            // Arrange
            UserLoggedIn(StaffUsername);
            
            var slot = new CreateSlot
            {
                RoomId = RoomA.Id,
                Room = RoomA,
                StaffId = Staff.Id,
                StartTime = new DateTime(2019, 1, 1, 9, 0, 0)
            };

            Context.Slot.AddRange(
                new Slot
                {
                    RoomId = RoomA.Id,
                    Room = RoomA,
                    StaffId = Staff.Id,
                    StartTime = new DateTime(2019, 1, 1, 12, 0, 0)
                },
                new Slot
                {
                    RoomId = RoomA.Id,
                    Room = RoomA,
                    StaffId = Staff.Id,
                    StartTime = new DateTime(2019, 1, 1, 14, 0, 0)
                }
            );

            Context.SaveChanges();

            // Act
            IActionResult result = await SlotController.Create(slot);

            // Assert
            IEnumerable<string> errorMessages = SlotController.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage);
            
            Assert.Contains(errorMessages, e => e == $"Room has reached a maximum booking of {Room.MaxRoomBookingPerDay} per day.");
            Assert.False(SlotController.ModelState.IsValid);
            
            Assert.IsType<ViewResult>(result);

            // Slot does not exist in the mock database
            Assert.False(Context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime));
        }
        
        [Fact]
        public async Task CreateSlot_WithStaffOverMaximumBooking_ReturnFail()
        {
            // Arrange
            UserLoggedIn(StaffUsername);
            
            var slot = new CreateSlot
            {
                RoomId = "D",
                StaffId = Staff.Id,
                StartTime = new DateTime(2019, 1, 1, 13, 0,  0)
            };

            Context.Slot.AddRange(
                new Slot
                {
                    RoomId = RoomA.Id,
                    Room = RoomA,
                    StaffId = Staff.Id,
                    StartTime = new DateTime(2019, 1, 1, 12, 0, 0)
                },
                new Slot
                {
                    RoomId = RoomA.Id,
                    Room = RoomA,
                    StaffId = Staff.Id,
                    StartTime = new DateTime(2019, 1, 1, 14, 0, 0)
                },
                new Slot
                {
                    RoomId = "B",
                    StaffId = Staff.Id,
                    StartTime = new DateTime(2019, 1, 1, 10, 0, 0)
                },
                new Slot
                {
                    RoomId = "B",
                    StaffId = Staff.Id,
                    StartTime = new DateTime(2019, 1, 1, 9, 0, 0)
                }
            );

            await Context.SaveChangesAsync();

            // Act
            IActionResult result = await SlotController.Create(slot);

            // Assert
            IEnumerable<string> errorMessages = SlotController.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage);
            
            Assert.Contains(errorMessages, e => e == $"Staff has a maximum of {Staff.MaxBookingPerDay} bookings.");
            Assert.False(SlotController.ModelState.IsValid);
            
            Assert.IsType<ViewResult>(result);

            // Slot does not exist in the mock database
            Assert.False(Context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime));
        }
        
        [Fact]
        public async Task CreateSlot_WithStaffAlreadyTakenSlot_ReturnFail()
        {
            // Arrange
            UserLoggedIn(StaffUsername);
            
            var slot = new CreateSlot
            {
                RoomId = RoomA.Id,
                Room = RoomA,
                StaffId = Staff.Id,
                Staff = new Staff
                {
                    Id = Staff.Id,
                    StaffId = Staff.StaffId
                },
                StartTime = new DateTime(2019, 1, 1, 12, 0, 0)
            };

            Context.Slot.AddRange(
                new Slot
                {
                    RoomId = RoomA.Id,
                    Room = RoomA,
                    StaffId = "e54321",
                    Staff = new Staff
                    {
                        StaffId = "e54321"
                    },
                    StartTime = new DateTime(2019, 1, 1, 12, 0, 0)
                }
            );

            await Context.SaveChangesAsync();

            // Act
            IActionResult result = await SlotController.Create(slot);

            // Assert
            IEnumerable<string> errorMessages = SlotController.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage);
            
            Assert.Contains(errorMessages, e => e == "Staff has already taken slot.");
            Assert.False(SlotController.ModelState.IsValid);
            
            Assert.IsType<ViewResult>(result);

            // Slot does not exist in the mock database
            Assert.False(Context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime && s.StaffId == slot.StaffId));
        }
        
        [Fact]
        public async Task CreateSlot_WithSlotAlreadyExists_ReturnFail()
        {
            // Arrange
            UserLoggedIn(StaffUsername);
            
            var slot = new CreateSlot
            {
                RoomId = RoomA.Id,
                Room = RoomA,
                StaffId = Staff.Id,
                StartTime = new DateTime(2019, 1, 1, 12, 0, 0)
            };

            Context.Slot.AddRange(
                new Slot
                {
                    RoomId = RoomA.Id,
                    Room = RoomA,
                    StaffId = Staff.Id,
                    Staff = new Staff
                    {
                        Id = Staff.Id,
                        StaffId = Staff.StaffId
                    },
                    StartTime = new DateTime(2019, 1, 1, 12, 0, 0)
                }
            );

            await Context.SaveChangesAsync();

            // Act
            IActionResult result = await SlotController.Create(slot);

            // Assert
            IEnumerable<string> errorMessages = SlotController.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage);
            
            Assert.Contains(errorMessages, e => e == "Slot already exists.");
            Assert.False(SlotController.ModelState.IsValid);
            
            Assert.IsType<ViewResult>(result);
        }
        
        [Fact]
        public async Task CreateSlot_WithStaffAlreadyCreatedSlot_ReturnFail()
        {
            // Arrange
            UserLoggedIn(StaffUsername);
            
            var slot = new CreateSlot
            {
                RoomId = "D",
                StaffId = Staff.Id,
                StartTime = new DateTime(2019, 1, 1, 13, 0, 0)
            };

            var createdSlot = new Slot
            {
                RoomId = RoomA.Id,
                Room = RoomA,
                StaffId = Staff.Id,
                StartTime = new DateTime(2019, 1, 1, 13, 0, 0)
            };

            Context.Slot.Add(createdSlot);

            await Context.SaveChangesAsync();

            // Act
            IActionResult result = await SlotController.Create(slot);

            // Assert
            IEnumerable<string> errorMessages = SlotController.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage);
            
            Assert.Contains(errorMessages, e => e == "You have already created a slot on the same time with a different room.");
            Assert.False(SlotController.ModelState.IsValid);
            
            Assert.IsType<ViewResult>(result);

            // Slot does not exist in the mock database
            Assert.False(Context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime));
        }
        
        [Fact]
        public async Task CreateSlot_WithStartTimeInThePast_ReturnFail()
        {
            // Arrange
            UserLoggedIn(StaffUsername);
            
            var slot = new CreateSlot
            {
                RoomId = "D",
                StaffId = Staff.Id,
                StartTime = DateTimeNow.Subtract(TimeSpan.FromDays(1))
            };

            // Act
            IActionResult result = await SlotController.Create(slot);

            // Assert
            IEnumerable<string> errorMessages = SlotController.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage);
            
            Assert.Contains(errorMessages, e => e == "Slot cannot be created in the past.");
            Assert.False(SlotController.ModelState.IsValid);
            
            Assert.IsType<ViewResult>(result);

            // Slot does not exist in the mock database
            Assert.False(Context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime));
        }
    }
}