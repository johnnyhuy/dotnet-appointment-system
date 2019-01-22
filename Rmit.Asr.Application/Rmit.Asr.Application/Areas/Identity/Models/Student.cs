namespace Rmit.Asr.Application.Areas.Identity.Models
{
    public class Student : ApplicationUser
    {
        private const string EmailSuffix = "student.rmit.edu.au";

        public override string Email => $"{Id}@{EmailSuffix}";
    }
}