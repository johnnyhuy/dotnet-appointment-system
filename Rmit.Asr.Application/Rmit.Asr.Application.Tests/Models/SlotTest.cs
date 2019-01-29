using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Rmit.Asr.Application.Models;
using Rmit.Asr.Application.Models.ViewModels;
using Xunit;

namespace Rmit.Asr.Application.Tests.Models
{
    public class SlotTest
    {
        [Fact]
        public void Slot_WithValidInput_ValidationSuccess()
        {
            // Arrange
            var validationResults = new List<ValidationResult>();
            var slot = new Slot
            {
                RoomId = "A",
                Room = new Room(),
                Staff = new Staff(),
                StaffId = "e12345",
                StartTime = new DateTime(2019, 1, 1, 10, 0, 0),
                Student = new Student(),
                StudentId = "s1234567"
            };
            var validationContext = new ValidationContext(slot);

            // Act
            bool results = Validator.TryValidateObject(slot, validationContext, validationResults, true);

            // Assert
            Assert.Empty(validationResults);
            Assert.True(results);
        }
        
        public static object[][] InvalidDateTimeIntervals =
        {
            new object[] { new DateTime(2019, 1, 1, 9, 1, 1), new CreateSlot() },
            new object[] { new DateTime(2019, 1, 1, 9, 23, 0), new CreateSlot() },
            new object[] { new DateTime(2019, 1, 1, 9, 0, 1), new CreateSlot() },
            new object[] { new DateTime(2019, 1, 1, 9, 12, 12), new CreateSlot() },
            new object[] { new DateTime(2019, 1, 1, 9, 1, 1), new RemoveSlot() },
            new object[] { new DateTime(2019, 1, 1, 9, 23, 0), new RemoveSlot() },
            new object[] { new DateTime(2019, 1, 1, 9, 0, 1), new RemoveSlot() },
            new object[] { new DateTime(2019, 1, 1, 9, 12, 12), new RemoveSlot() },
            new object[] { new DateTime(2019, 1, 1, 9, 1, 1), new BookSlot() },
            new object[] { new DateTime(2019, 1, 1, 9, 23, 0), new BookSlot() },
            new object[] { new DateTime(2019, 1, 1, 9, 0, 1), new BookSlot() },
            new object[] { new DateTime(2019, 1, 1, 9, 12, 12), new BookSlot() },
            new object[] { new DateTime(2019, 1, 1, 9, 1, 1), new CancelSlot() },
            new object[] { new DateTime(2019, 1, 1, 9, 23, 0), new CancelSlot() },
            new object[] { new DateTime(2019, 1, 1, 9, 0, 1), new CancelSlot() },
            new object[] { new DateTime(2019, 1, 1, 9, 12, 12), new CancelSlot() }
        };
        
        [Theory, MemberData(nameof(InvalidDateTimeIntervals))]
        public void SlotStartTime_WithNonIntervalTime_ValidationFails(DateTime input, Slot slot)
        {
            // Arrange
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(slot) { MemberName = nameof(slot.StartTime) };

            // Act
            slot.StartTime = input;
            
            bool results = Validator.TryValidateProperty(slot.StartTime, validationContext, validationResults);

            // Assert
            string expectedMessage = $"Slot start time {slot.StartTime} must be in 1 hour intervals e.g. 9:00, 13:00, 22:00";
            
            Assert.Contains(validationResults, r => r.ErrorMessage == expectedMessage);
            Assert.False(results);
        }
        
        public static object[][] InvalidDateTimeTimeBetween =
        {
            new object[] { new DateTime(2019, 1, 1, 8, 0, 0), new CreateSlot() },
            new object[] { new DateTime(2019, 1, 1, 15, 0, 0), new CreateSlot() },
            new object[] { new DateTime(2019, 1, 1, 1, 0, 0), new CreateSlot() },
            new object[] { new DateTime(2019, 1, 1, 8, 0, 0), new RemoveSlot() },
            new object[] { new DateTime(2019, 1, 1, 15, 0, 0), new RemoveSlot() },
            new object[] { new DateTime(2019, 1, 1, 1, 0, 0), new RemoveSlot() },
            new object[] { new DateTime(2019, 1, 1, 8, 0, 0), new BookSlot() },
            new object[] { new DateTime(2019, 1, 1, 15, 0, 0), new BookSlot() },
            new object[] { new DateTime(2019, 1, 1, 1, 0, 0), new BookSlot() },
            new object[] { new DateTime(2019, 1, 1, 8, 0, 0), new CancelSlot() },
            new object[] { new DateTime(2019, 1, 1, 15, 0, 0), new CancelSlot() },
            new object[] { new DateTime(2019, 1, 1, 1, 0, 0), new CancelSlot() }
        };
        
        [Theory, MemberData(nameof(InvalidDateTimeTimeBetween))]
        public void SlotStartTime_WithInvalidTimeBetween_ValidationFails(DateTime input, Slot slot)
        {
            // Arrange
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(slot) { MemberName = nameof(slot.StartTime) };

            // Act
            slot.StartTime = input;
            
            bool results = Validator.TryValidateProperty(slot.StartTime, validationContext, validationResults);

            // Assert
            string expectedMessage = $"The date time field {slot.StartTime} must be between 9:00 - 14:00";
            
            Assert.Contains(validationResults, r => r.ErrorMessage == expectedMessage);
            Assert.False(results);
        }
        
        public static object[][] InheritedSlots =
        {
            new object[] { new CreateSlot() },
            new object[] { new RemoveSlot() },
            new object[] { new BookSlot() },
            new object[] { new CancelSlot() }
        };
        
        [Theory, MemberData(nameof(InheritedSlots))]
        public void SlotStartTime_WithNoInput_ValidationFails(Slot slot)
        {
            // Arrange
            var validationResults = new List<ValidationResult>();

            // Act
            Validator.TryValidateProperty(slot.StartTime, new ValidationContext(slot) { MemberName = nameof(slot.StartTime) }, validationResults);

            // Assert
            const string expectedMessages = "The Start Time field is required.";
            
            Assert.Contains(validationResults, r => r.ErrorMessage == expectedMessages);
        }
    }
}