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
    public class SlotControllerTest : ControllerBaseTest
    {
        [Fact]
        public async Task CreateSlot_WithValidParameters_ReturnSuccess()
        {
            // Arrange
            UserLoggedIn(StaffUsername);
            var slot = new CreateSlot
            {
                RoomId = "C",
                StaffId = StaffId,
                StartTime = new DateTime(2019, 1, 1, 8, 0, 0)
            };

            // Act
            IActionResult result = await Controller.Create(slot);

            // Assert
            Assert.Empty(Controller.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage));
            Assert.True(Controller.ModelState.IsValid);
            
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("StaffIndex", viewResult.ActionName);

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
                StaffId = StaffId,
                StartTime = new DateTime(2019, 1, 1, 8, 0, 0)
            };

            // Act
            IActionResult result = await Controller.Create(slot);

            // Assert
            IEnumerable<string> errorMessages = Controller.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage);
            
            Assert.Contains(errorMessages, e => e == $"Room {slot.RoomId} does not exist.");
            Assert.False(Controller.ModelState.IsValid);
            
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
                RoomId = "A",
                StaffId = StaffId,
                StartTime = new DateTime(2019, 1, 1, 9, 0, 0)
            };

            Context.Slot.AddRange(
                new Slot
                {
                    RoomId = "A",
                    StaffId = StaffId,
                    StartTime = new DateTime(2019, 1, 1, 12, 0, 0)
                },
                new Slot
                {
                    RoomId = "A",
                    StaffId = StaffId,
                    StartTime = new DateTime(2019, 1, 1, 14, 0, 0)
                }
            );

            Context.SaveChanges();

            // Act
            IActionResult result = await Controller.Create(slot);

            // Assert
            IEnumerable<string> errorMessages = Controller.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage);
            
            Assert.Contains(errorMessages, e => e == $"Room {slot.RoomId} has reached a maximum booking of {Room.MaxRoomBookingPerDay} per day.");
            Assert.False(Controller.ModelState.IsValid);
            
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
                StaffId = StaffId,
                StartTime = new DateTime(2019, 1, 1, 13, 0,  0)
            };

            Context.Slot.AddRange(
                new Slot
                {
                    RoomId = "A",
                    StaffId = StaffId,
                    StartTime = new DateTime(2019, 1, 1, 12, 0, 0)
                },
                new Slot
                {
                    RoomId = "A",
                    StaffId = StaffId,
                    StartTime = new DateTime(2019, 1, 1, 14, 0, 0)
                },
                new Slot
                {
                    RoomId = "B",
                    StaffId = StaffId,
                    StartTime = new DateTime(2019, 1, 1, 10, 0, 0)
                },
                new Slot
                {
                    RoomId = "B",
                    StaffId = StaffId,
                    StartTime = new DateTime(2019, 1, 1, 9, 0, 0)
                }
            );

            await Context.SaveChangesAsync();

            // Act
            IActionResult result = await Controller.Create(slot);

            // Assert
            IEnumerable<string> errorMessages = Controller.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage);
            
            Assert.Contains(errorMessages, e => e == $"Staff {LoggedInStaff.StaffId} has a maximum of {Staff.MaxBookingPerDay} bookings at {slot.StartTime:dd-MM-yyyy}.");
            Assert.False(Controller.ModelState.IsValid);
            
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
                RoomId = "A",
                StaffId = StaffId,
                Staff = new Staff
                {
                    StaffId = StaffId
                },
                StartTime = new DateTime(2019, 1, 1, 12, 0, 0)
            };

            Context.Slot.AddRange(
                new Slot
                {
                    RoomId = "A",
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
            IActionResult result = await Controller.Create(slot);

            // Assert
            IEnumerable<string> errorMessages = Controller.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage);
            
            Assert.Contains(errorMessages, e => e == $"Staff e54321 has already taken slot at room {slot.RoomId} {slot.StartTime:dd-MM-yyyy H:mm}.");
            Assert.False(Controller.ModelState.IsValid);
            
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
                RoomId = "A",
                StaffId = StaffId,
                StartTime = new DateTime(2019, 1, 1, 12, 0, 0)
            };

            Context.Slot.AddRange(
                new Slot
                {
                    RoomId = "A",
                    StaffId = StaffId,
                    Staff = new Staff
                    {
                        StaffId = StaffId
                    },
                    StartTime = new DateTime(2019, 1, 1, 12, 0, 0)
                }
            );

            await Context.SaveChangesAsync();

            // Act
            IActionResult result = await Controller.Create(slot);

            // Assert
            IEnumerable<string> errorMessages = Controller.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage);
            
            Assert.Contains(errorMessages, e => e == $"Slot at room {slot.RoomId} {slot.StartTime:dd-MM-yyyy H:mm} already exists.");
            Assert.False(Controller.ModelState.IsValid);
            
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
                StaffId = StaffId,
                StartTime = new DateTime(2019, 1, 1, 13, 0, 0)
            };

            var createdSlot = new Slot
            {
                RoomId = "A",
                StaffId = StaffId,
                StartTime = new DateTime(2019, 1, 1, 13, 0, 0)
            };

            Context.Slot.Add(createdSlot);

            await Context.SaveChangesAsync();

            // Act
            IActionResult result = await Controller.Create(slot);

            // Assert
            IEnumerable<string> errorMessages = Controller.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage);
            
            Assert.Contains(errorMessages, e => e == $"You have already created a slot at room {createdSlot.RoomId} {createdSlot.StartTime:dd-MM-yyyy H:mm}.");
            Assert.False(Controller.ModelState.IsValid);
            
            Assert.IsType<ViewResult>(result);

            // Slot does not exist in the mock database
            Assert.False(Context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime));
        }
        
        [Fact]
        public async Task RemoveSlot_WithStudentBookedInSlot_ReturnFail()
        {
            // Arrange
            UserLoggedIn(StaffUsername);
            var slot = new RemoveSlot
            {
                RoomId = "A",
                StartTime = new DateTime(2019, 1, 1, 13, 0, 0)
            };

            var createdSlot = new Slot
            {
                RoomId = "A",
                StaffId = StaffId,
                StudentId = "s1234567",
                StartTime = new DateTime(2019, 1, 1, 13, 0, 0)
            };

            Context.Slot.Add(createdSlot);

            await Context.SaveChangesAsync();

            // Act
            IActionResult result = await Controller.Remove(slot);

            // Assert
            IEnumerable<string> errorMessages = Controller.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage);
            
            Assert.Contains(errorMessages, e => e == "Cannot remove slot as a student has been booked into it.");
            Assert.False(Controller.ModelState.IsValid);
            
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
                RoomId = "A",
                StaffId = StaffId,
                StudentId = "s1234567",
                StartTime = new DateTime(2019, 1, 1, 13, 0, 0)
            };

            Context.Slot.Add(createdSlot);

            await Context.SaveChangesAsync();

            // Act
            IActionResult result = await Controller.Remove(slot);

            // Assert
            IEnumerable<string> errorMessages = Controller.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage);
            
            Assert.Contains(errorMessages, e => e == $"Slot at room {slot.RoomId} {slot.StartTime:dd-MM-yyyy H:mm} does not exist.");
            Assert.False(Controller.ModelState.IsValid);
            
            Assert.IsType<ViewResult>(result);

            Assert.True(Context.Slot.Any(s => s.RoomId == createdSlot.RoomId && s.StartTime == createdSlot.StartTime));
        }
        
        [Fact]
        public async Task BookSlot_WithValidParameters_ReturnSuccess()
        {
            // Arrange
            UserLoggedIn(StudentUsername);
            
            var createdSlot = new Slot
            {
                RoomId = "A",
                StaffId = StaffId,
                StudentId = null,
                StartTime = new DateTime(2019, 1, 1, 13, 0, 0)
            };

            Context.Slot.Add(createdSlot);
            
            await Context.SaveChangesAsync();
            
            var slot = new BookSlot
            {
                RoomId = createdSlot.RoomId,
                StaffId = createdSlot.StaffId,
                StartTime = createdSlot.StartTime
            };

            // Act
            IActionResult result = await Controller.Book(slot);

            // Assert
            Assert.Empty(Controller.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage));
            Assert.True(Controller.ModelState.IsValid);
            
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("StudentIndex", viewResult.ActionName);

            Assert.True(Context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime && s.StudentId == slot.StudentId));
        }
        
        [Fact]
        public async Task BookSlot_WithStudentAlreadyBookedOnDay_ReturnFail()
        {
            // Arrange
            UserLoggedIn(StudentUsername);
            
            var createdSlot = new Slot
            {
                RoomId = "A",
                StaffId = StaffId,
                StudentId = "s1234567",
                StartTime = new DateTime(2019, 1, 1, 13, 0, 0)
            };
            
            var bookSlot = new Slot
            {
                RoomId = "B",
                StaffId = StaffId,
                StartTime = new DateTime(2019, 1, 1, 13, 0, 0)
            };
            
            var slot = new BookSlot
            {
                RoomId = bookSlot.RoomId,
                StartTime = bookSlot.StartTime,
                StudentId = StudentId
            };

            Context.Slot.Add(createdSlot);

            await Context.SaveChangesAsync();

            // Act
            IActionResult result = await Controller.Book(slot);

            // Assert
            IEnumerable<string> errorMessages = Controller.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage);
            
            Assert.Contains(errorMessages, e => e == $"Student {slot.StudentId} has reached their maximum bookings for this day ({slot.StartTime?.Date:dd-MM-yyyy})");
            Assert.False(Controller.ModelState.IsValid);
            
            Assert.IsType<ViewResult>(result);

            Assert.False(Context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime && s.StudentId == slot.StudentId));
        }
        
        [Fact]
        public async Task BookSlot_WithNonExistentRoom_ReturnFail()
        {
            // Arrange
            UserLoggedIn(StudentUsername);
            
            var createdSlot = new Slot
            {
                RoomId = "A",
                StaffId = StaffId,
                StudentId = null,
                StartTime = new DateTime(2019, 1, 1, 13, 0, 0)
            };

            Context.Slot.Add(createdSlot);
            
            await Context.SaveChangesAsync();
            
            var slot = new BookSlot
            {
                RoomId = "YEET",
                StaffId = createdSlot.StaffId,
                StartTime = createdSlot.StartTime
            };

            // Act
            IActionResult result = await Controller.Book(slot);

            // Assert
            IEnumerable<string> errorMessages = Controller.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage);
            
            Assert.Contains(errorMessages, e => e == $"Room {slot.RoomId} does not exist.");
            Assert.False(Controller.ModelState.IsValid);
            
            Assert.IsType<ViewResult>(result);

            Assert.False(Context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime && s.StudentId == slot.StudentId));
        }
        
        [Fact]
        public async Task BookSlot_WithNonExistentSlot_ReturnFail()
        {
            // Arrange
            UserLoggedIn(StudentUsername);
            
            var createdSlot = new Slot
            {
                RoomId = "A",
                StaffId = StaffId,
                StudentId = null,
                StartTime = new DateTime(2019, 1, 1, 13, 0, 0)
            };

            Context.Slot.Add(createdSlot);
            
            await Context.SaveChangesAsync();
            
            var slot = new BookSlot
            {
                RoomId = "B",
                StaffId = createdSlot.StaffId,
                StartTime = new DateTime(2019, 1, 1, 9, 0, 0)
            };

            // Act
            IActionResult result = await Controller.Book(slot);

            // Assert
            IEnumerable<string> errorMessages = Controller.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage);
            
            Assert.Contains(errorMessages, e => e == $"Slot does not exist in room {slot.RoomId} at {slot.StartTime:dd-MM-yyyy HH:mm}");
            Assert.False(Controller.ModelState.IsValid);
            
            Assert.IsType<ViewResult>(result);

            Assert.False(Context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime && s.StudentId == slot.StudentId));
        }
        
        [Fact]
        public async Task BookSlot_WithOtherStudentAlreadyBookedSlot_ReturnFail()
        {
            // Arrange
            UserLoggedIn(StudentUsername);
            
            var createdSlot = new Slot
            {
                RoomId = "A",
                StaffId = StaffId,
                StudentId = "s3604367",
                StartTime = new DateTime(2019, 1, 1, 13, 0, 0),
                Student = new Student
                {
                    Id = "s3604367",
                    StudentId = "s3604367",
                    FirstName = "Johnny",
                    LastName = "Doe",
                    Email = "s3604367@student.rmit.edu.au"
                }
            };

            Context.Slot.Add(createdSlot);
            
            await Context.SaveChangesAsync();
            
            var slot = new BookSlot
            {
                RoomId = createdSlot.RoomId,
                StaffId = createdSlot.StaffId,
                StartTime = createdSlot.StartTime
            };

            // Act
            IActionResult result = await Controller.Book(slot);

            // Assert
            IEnumerable<string> errorMessages = Controller.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage);
            
            Assert.Contains(errorMessages, e => e == $"Student {createdSlot.StudentId} has already booked slot in room {slot.RoomId} at {slot.StartTime.GetValueOrDefault():dd-MM-yyyy HH:mm}");
            Assert.False(Controller.ModelState.IsValid);
            
            Assert.IsType<ViewResult>(result);

            Assert.False(Context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime && s.StudentId == slot.StudentId));
        }
    }
}