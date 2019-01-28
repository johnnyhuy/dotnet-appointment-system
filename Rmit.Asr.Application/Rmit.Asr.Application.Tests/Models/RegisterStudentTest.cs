using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Rmit.Asr.Application.Models.ViewModels;
using Xunit;

namespace Rmit.Asr.Application.Tests.Models
{
    public class RegisterStudentTest
    {
        [Theory]
        [InlineData("s3603267")]
        [InlineData("s2000000")]
        [InlineData("s1234567")]
        public void tRegisterStudenId_WithValidInput_ValidationSuccess(string input)
        {
            // Arrange
            var student = new RegisterStudent();
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(student) { MemberName = nameof(student.Id) };

            // Act
            student.Id = input;
            
            bool results = Validator.TryValidateProperty(student.Id, validationContext, validationResults);

            // Assert
            Assert.Empty(validationResults);
            Assert.True(results);
        }
        
        [Theory]
        [InlineData("s12345678")]
        [InlineData("s36046723123")]
        [InlineData("s")]
        [InlineData("s2603267@rmit.edu.au")]
        [InlineData("@#&(!@*#&")]
        [InlineData("this is a wacky string!")]
        public void RegisterStudentId_WithInvalidInput_ValidationFails(string input)
        {
            // Arrange
            var student = new RegisterStudent();
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(student) { MemberName = nameof(student.Id) };

            // Act
            student.Id = input;
            
            bool results = Validator.TryValidateProperty(student.Id, validationContext, validationResults);

            // Assert
            string expectedMessage =
                $"The booked in student ID {student.Id} is invalid, it always starts with a letter ‘s’ followed by 7 numbers.";
            
            Assert.Contains(validationResults, r => r.ErrorMessage == expectedMessage);
            Assert.False(results);
        }
    }
}