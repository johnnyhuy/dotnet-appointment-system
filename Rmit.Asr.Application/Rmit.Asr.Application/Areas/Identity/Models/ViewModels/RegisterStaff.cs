using Rmit.Asr.Application.ValidationAttributes;

namespace Rmit.Asr.Application.Areas.Identity.Models.ViewModels
{
    public class RegisterStaff : Staff
    {
        [StaffId]
        public override string Id { get; set; }
    }
}