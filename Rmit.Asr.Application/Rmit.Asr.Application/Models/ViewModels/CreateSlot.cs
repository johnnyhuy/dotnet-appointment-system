using System.ComponentModel.DataAnnotations;

namespace Rmit.Asr.Application.Models.ViewModels
{
    public class CreateSlot : Slot
    {
        [Required]
        public string StaffId { get; set; }
    }
}
