using Rmit.Asr.Application.ValidationAttributes;

namespace Rmit.Asr.Application.Models.ViewModels
{
    public class RegisterStudent : Student
    {
        /// <inheritdoc />
        [StudentId]
        public override string Id { get; set; }
    }
}