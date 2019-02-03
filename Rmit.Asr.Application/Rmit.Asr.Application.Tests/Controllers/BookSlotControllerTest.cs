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
    public class BookSlotControllerTest : ControllerBaseTest
    {
        [Fact]
        public async Task BookSlot_WithValidParameters_ReturnSuccess()
        {
            // Arrange
            UserLoggedIn(StudentUsername);
            
            var createdSlot = new Slot
            {
                RoomId = RoomA.Id,
                Room = RoomA,
                StaffId = Staff.Id,
                StudentId = null,
                StartTime = new DateTime(2019, 1, 1, 13, 0, 0)
            };

            Context.Slot.Add(createdSlot);
            
            await Context.SaveChangesAsync();    
            
            var slot = new BookSlot
            {
                RoomId = createdSlot.RoomId,
                StartTime = createdSlot.StartTime
            };

            // Act
            IActionResult result = await SlotController.Book(slot);

            // Assert
            Assert.Empty(SlotController.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage));
            Assert.True(SlotController.ModelState.IsValid);
            
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", viewResult.ActionName);

            Assert.True(Context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime && s.StudentId == slot.StudentId));
        }
        
        [Fact]
        public async Task BookSlot_WithStudentAlreadyBookedOnDay_ReturnFail()
        {
            // Arrange
            UserLoggedIn(StudentUsername);
            
            var createdSlot = new Slot
            {
                RoomId = RoomA.Id,
                StaffId = Staff.Id,
                StudentId = Student.Id,
                StartTime = new DateTime(2019, 1, 1, 13, 0, 0)
            };
            
            var bookSlot = new Slot
            {
                RoomId = "B",
                StaffId = Staff.Id,
                StudentId = null,
                StartTime = new DateTime(2019, 1, 1, 9, 0, 0)
            };
            
            var slot = new BookSlot
            {
                RoomId = bookSlot.RoomId,
                StartTime = bookSlot.StartTime
            };

            Context.Slot.AddRange(createdSlot, bookSlot);

            await Context.SaveChangesAsync();

            // Act
            IActionResult result = await SlotController.Book(slot);

            // Assert
            IEnumerable<string> errorMessages = SlotController.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage);
            
            Assert.Contains(errorMessages, e => e =="Student has reached their maximum bookings for this day.");
            Assert.False(SlotController.ModelState.IsValid);
            
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
                RoomId = RoomA.Id,
                StaffId = Staff.Id,
                StudentId = null,
                StartTime = new DateTime(2019, 1, 1, 13, 0, 0)
            };

            Context.Slot.Add(createdSlot);
            
            await Context.SaveChangesAsync();
            
            var slot = new BookSlot
            {
                RoomId = "YEET",
                StartTime = createdSlot.StartTime
            };

            // Act
            IActionResult result = await SlotController.Book(slot);

            // Assert
            IEnumerable<string> errorMessages = SlotController.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage);
            
            Assert.Contains(errorMessages, e => e == "Room does not exist.");
            Assert.False(SlotController.ModelState.IsValid);
            
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
                RoomId = RoomA.Id,
                StaffId = Staff.Id,
                StudentId = null,
                StartTime = new DateTime(2019, 1, 1, 13, 0, 0)
            };

            Context.Slot.Add(createdSlot);
            
            await Context.SaveChangesAsync();
            
            var slot = new BookSlot
            {
                RoomId = "B",
                StartTime = new DateTime(2019, 1, 1, 9, 0, 0)
            };

            // Act
            IActionResult result = await SlotController.Book(slot);

            // Assert
            IEnumerable<string> errorMessages = SlotController.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage);
            
            Assert.Contains(errorMessages, e => e == "Slot does not exist.");
            Assert.False(SlotController.ModelState.IsValid);
            
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
                RoomId = RoomA.Id,
                StaffId = Staff.Id,
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
                StartTime = createdSlot.StartTime
            };

            // Act
            IActionResult result = await SlotController.Book(slot);

            // Assert
            IEnumerable<string> errorMessages = SlotController.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage);
            
            Assert.Contains(errorMessages, e => e == "Student has already booked slot in room.");
            Assert.False(SlotController.ModelState.IsValid);
            
            Assert.IsType<ViewResult>(result);

            Assert.False(Context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime && s.StudentId == slot.StudentId));
        }
        
        [Fact]
        public async Task BookSlot_WithStartTimeInThePast_ReturnFail()
        {
            // Arrange
            UserLoggedIn(StudentUsername);
            
            var createdSlot = new Slot
            {
                RoomId = RoomA.Id,
                StaffId = Staff.Id,
                StudentId = "s3604367",
                StartTime = DateTimeNow.Subtract(TimeSpan.FromDays(1)),
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
                StartTime = createdSlot.StartTime
            };

            // Act
            IActionResult result = await SlotController.Book(slot);

            // Assert
            IEnumerable<string> errorMessages = SlotController.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage);
            
            Assert.Contains(errorMessages, e => e == "Slot cannot be booked in the past.");
            Assert.False(SlotController.ModelState.IsValid);
            
            Assert.IsType<ViewResult>(result);

            Assert.False(Context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime && s.StudentId == slot.StudentId));
        }
    }
}