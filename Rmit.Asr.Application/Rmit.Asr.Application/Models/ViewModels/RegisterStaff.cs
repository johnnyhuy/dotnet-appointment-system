using Rmit.Asr.Application.Models.ValidationAttributes;

namespace Rmit.Asr.Application.Models.ViewModels
{
    public class RegisterStaff : Staff
    {
        /// <inheritdoc />
        [StaffId]
        public override string Id { get; set; }
    }
}