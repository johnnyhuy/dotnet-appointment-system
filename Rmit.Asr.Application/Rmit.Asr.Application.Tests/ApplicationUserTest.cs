using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Rmit.Asr.Application.Models;
using Xunit;

namespace Rmit.Asr.Application.Tests
{
    public class ApplicationUserTest
    {
        [Fact]
        public void ApplicationUser_WithValidInput_ValidationSuccess()
        {
            // Arrange
            var validationResults = new List<ValidationResult>();
            var staff = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "John",
                LastName = "Doe"
            };
            var validationContext = new ValidationContext(staff);

            // Act
            bool results = Validator.TryValidateObject(staff, validationContext, validationResults, true);

            // Assert
            Assert.Empty(validationResults);
            Assert.True(results);
        }
        
        [Theory]
        [InlineData("John", "Doe")]
        [InlineData("jane", "doe")]
        [InlineData(" john ", " doe ")]
        public void ApplicationUserFullName_WithValidInput_ValidationSuccess(string firstName, string lastName)
        {
            // Arrange
            var user = new ApplicationUser();
            var validationResults = new List<ValidationResult>();

            // Act
            user.FirstName = firstName;
            user.LastName = lastName;
            
            Validator.TryValidateProperty(user.FirstName, new ValidationContext(user) { MemberName = nameof(user.FirstName) }, validationResults);
            Validator.TryValidateProperty(user.LastName, new ValidationContext(user) { MemberName = nameof(user.LastName) }, validationResults);

            // Assert
            Assert.Empty(validationResults);
        }
        
        [Theory]
        [InlineData(null, null)]
        [InlineData("John", null)]
        [InlineData(null, "Doe")]
        public void ApplicationUserFullName_WithInvalidInput_ValidationFails(string firstName, string lastName)
        {
            // Arrange
            var user = new ApplicationUser();
            var validationResults = new List<ValidationResult>();

            // Act
            user.FirstName = firstName;
            user.LastName = lastName;
            
            Validator.TryValidateProperty(user.FirstName, new ValidationContext(user) { MemberName = nameof(user.FirstName) }, validationResults);
            Validator.TryValidateProperty(user.LastName, new ValidationContext(user) { MemberName = nameof(user.LastName) }, validationResults);

            // Assert
            string[] expectedMessages = {
                "The First Name field is required.",
                "The Last Name field is required."
            };
            
            Assert.Contains(validationResults, r => expectedMessages.Contains(r.ErrorMessage));
        }
    }
}