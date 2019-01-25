using Rmit.Asr.Application.ValidationAttributes;

namespace Rmit.Asr.Application.Models.ViewModels
{
    public class RegisterStudent : Student
    {
        /// <summary>
        /// Student ID applied with student ID validation.
        /// </summary>
        [StudentId]
        public override string Id { get; set; }
    }
}