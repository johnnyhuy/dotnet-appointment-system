namespace Rmit.Asr.Application.Areas.Identity.Models
{
    public class Staff : ApplicationUser
    {
        public const string EmailSuffix = "rmit.edu.au";

        public override string Email => $"{Id}@{EmailSuffix}";
    }
}