using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Rmit.Asr.Application.Controllers;
using Rmit.Asr.Application.Models;
using Xunit;

namespace Rmit.Asr.Application.Tests.Controllers
{
    public class StaffControllerTest : ControllerBaseTest
    {
        private readonly StaffMenuController _controller;

        public StaffControllerTest()
        {
            var loggedInUser = new Staff
            {
                UserName = "e12345@rmit.edu.au"
            };
            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, loggedInUser.UserName)
            }));
            
            var mockUserStore = new Mock<IUserStore<Staff>>();
            mockUserStore.Setup(x => x.FindByIdAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(loggedInUser);
            
            var mockUserManager = new UserManager<Staff>(mockUserStore.Object, null, null, null, null, null, null, null, null);
            _controller = new StaffMenuController(Context, mockUserManager)
            {
                ControllerContext =  new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = userPrincipal }
                }
            };
        }
        
        [Fact]
        public async Task CreateSlot_WithValidParameters_ReturnSuccess()
        {
            // Arrange
            var slot = new Slot
            {
                RoomId = "C",
                StaffId = "e12345",
                StartTime = new DateTime(2019, 1, 1, 8, 0, 0)
            };

            // Act
            IActionResult result = await _controller.CreateSlot(slot);

            // Assert
            // No validation errors have occured
            Assert.Empty(_controller.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage));
            Assert.True(_controller.ModelState.IsValid);
            
            // Controller redirected
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", viewResult.ActionName);

            // Slot exists in mock database
            Assert.True(Context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime));
        }
        
        [Fact]
        public async Task CreateSlot_WithNonExistentRoom_ReturnSuccess()
        {
            // Arrange
            var slot = new Slot
            {
                RoomId = "Z",
                StaffId = "e12345",
                StartTime = new DateTime(2019, 1, 1, 8, 0, 0)
            };

            // Act
            IActionResult result = await _controller.CreateSlot(slot);

            // Assert
            IEnumerable<string> errorMessages = _controller.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage);
            
            // No validation errors have occured
            Assert.Contains(errorMessages, e => e == $"Room {slot.RoomId} does not exist.");
            Assert.False(_controller.ModelState.IsValid);
            
            // Controller returns a view
            Assert.IsType<ViewResult>(result);

            // Slot does not exist in the mock database
            Assert.False(Context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime));
        }
        
        [Fact]
        public async Task CreateSlot_WithOverMaximumRoomBooking_ReturnFail()
        {
            // Arrange
            var slot = new Slot
            {
                RoomId = "A",
                StaffId = "e12345",
                StartTime = new DateTime(2019, 1, 1, 9, 0, 0)
            };

            Context.Slot.AddRange(
                new Slot
                {
                    RoomId = "A",
                    StaffId = "e12345",
                    StartTime = new DateTime(2019, 1, 1, 12, 0, 0)
                },
                new Slot
                {
                    RoomId = "A",
                    StaffId = "e12345",
                    StartTime = new DateTime(2019, 1, 1, 14, 0, 0)
                }
            );

            Context.SaveChanges();

            // Act
            IActionResult result = await _controller.CreateSlot(slot);

            // Assert
            IEnumerable<string> errorMessages = _controller.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage);
            
            // No validation errors have occured
            Assert.Contains(errorMessages, e => e == $"Room {slot.RoomId} has reached a maximum booking of {Room.MaxRoomBookingPerDay} per day.");
            Assert.False(_controller.ModelState.IsValid);
            
            // Controller returns a view
            Assert.IsType<ViewResult>(result);

            // Slot does not exist in the mock database
            Assert.False(Context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime));
        }
        
        [Fact]
        public async Task CreateSlot_WithStaffOverMaximumBooking_ReturnFail()
        {
            // Arrange
            var slot = new Slot
            {
                RoomId = "D",
                StaffId = "e12345",
                StartTime = new DateTime(2019, 1, 1, 13, 2, 2)
            };

            Context.Slot.AddRange(
                new Slot
                {
                    RoomId = "A",
                    StaffId = "e12345",
                    StartTime = new DateTime(2019, 1, 1, 12, 0, 0)
                },
                new Slot
                {
                    RoomId = "A",
                    StaffId = "e12345",
                    StartTime = new DateTime(2019, 1, 1, 14, 0, 0)
                },
                new Slot
                {
                    RoomId = "B",
                    StaffId = "e12345",
                    StartTime = new DateTime(2019, 1, 1, 10, 0, 0)
                },
                new Slot
                {
                    RoomId = "B",
                    StaffId = "e12345",
                    StartTime = new DateTime(2019, 1, 1, 9, 0, 0)
                }
            );

            await Context.SaveChangesAsync();

            // Act
            IActionResult result = await _controller.CreateSlot(slot);

            // Assert
            IEnumerable<string> errorMessages = _controller.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage);
            
            // No validation errors have occured
            Assert.Contains(errorMessages, e => e == $"Staff {slot.StaffId} has a maximum of {Staff.MaxBookingPerDay} bookings at {slot.StartTime:dd-MM-yyyy}.");
            Assert.False(_controller.ModelState.IsValid);
            
            // Controller returns a view
            Assert.IsType<ViewResult>(result);

            // Slot does not exist in the mock database
            Assert.False(Context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime));
        }
        
        [Fact]
        public async Task CreateSlot_WithStaffAlreadyTakenSlot_ReturnFail()
        {
            // Arrange
            var slot = new Slot
            {
                RoomId = "A",
                StaffId = "e12345",
                StartTime = new DateTime(2019, 1, 1, 12, 0, 0)
            };

            Context.Slot.AddRange(
                new Slot
                {
                    RoomId = "A",
                    StaffId = "e54321",
                    StartTime = new DateTime(2019, 1, 1, 12, 0, 0)
                }
            );

            await Context.SaveChangesAsync();

            // Act
            IActionResult result = await _controller.CreateSlot(slot);

            // Assert
            IEnumerable<string> errorMessages = _controller.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage);
            
            // No validation errors have occured
            Assert.Contains(errorMessages, e => e == $"Staff e54321 has already taken slot at room {slot.RoomId} {slot.StartTime:dd-MM-yyyy H:mm}.");
            Assert.False(_controller.ModelState.IsValid);
            
            // Controller returns a view
            Assert.IsType<ViewResult>(result);

            // Slot does not exist in the mock database
            Assert.False(Context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime && s.StaffId == slot.StaffId));
        }
        
        [Fact]
        public async Task CreateSlot_WithSlotAlreadyExists_ReturnFail()
        {
            // Arrange
            var slot = new Slot
            {
                RoomId = "A",
                StaffId = "e12345",
                StartTime = new DateTime(2019, 1, 1, 12, 0, 0)
            };

            Context.Slot.AddRange(
                new Slot
                {
                    RoomId = "A",
                    StaffId = "e12345",
                    StartTime = new DateTime(2019, 1, 1, 12, 0, 0)
                }
            );

            await Context.SaveChangesAsync();

            // Act
            IActionResult result = await _controller.CreateSlot(slot);

            // Assert
            IEnumerable<string> errorMessages = _controller.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage);
            
            // No validation errors have occured
            Assert.Contains(errorMessages, e => e == $"Slot at room {slot.RoomId} {slot.StartTime:dd-MM-yyyy H:mm} already exists.");
            Assert.False(_controller.ModelState.IsValid);
            
            // Controller returns a view
            Assert.IsType<ViewResult>(result);
        }
        
        [Fact]
        public async Task CreateSlot_WithStaffAlreadyCreatedSlot_ReturnFail()
        {
            // Arrange
            var slot = new Slot
            {
                RoomId = "D",
                StaffId = "e12345",
                StartTime = new DateTime(2019, 1, 1, 13, 0, 0)
            };

            var createdSlot = new Slot
            {
                RoomId = "A",
                StaffId = "e12345",
                StartTime = new DateTime(2019, 1, 1, 13, 0, 0)
            };

            Context.Slot.Add(createdSlot);

            await Context.SaveChangesAsync();

            // Act
            IActionResult result = await _controller.CreateSlot(slot);

            // Assert
            IEnumerable<string> errorMessages = _controller.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage);
            
            // No validation errors have occured
            Assert.Contains(errorMessages, e => e == $"You have already created a slot at room {createdSlot.RoomId} {createdSlot.StartTime:dd-MM-yyyy H:mm}.");
            Assert.False(_controller.ModelState.IsValid);
            
            // Controller returns a view
            Assert.IsType<ViewResult>(result);

            // Slot does not exist in the mock database
            Assert.False(Context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime));
        }
    }
}