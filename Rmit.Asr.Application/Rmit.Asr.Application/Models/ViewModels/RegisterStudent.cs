using Rmit.Asr.Application.Models.ValidationAttributes;

namespace Rmit.Asr.Application.Models.ViewModels
{
    public class RegisterStudent : Student
    {
        /// <inheritdoc />
        [StudentId]
        public override string StudentId { get; set; }
    }
}