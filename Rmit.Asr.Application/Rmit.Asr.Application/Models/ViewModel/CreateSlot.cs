using System;
using System.ComponentModel.DataAnnotations;
using Rmit.Asr.Application.ValidationAttributes;

namespace Rmit.Asr.Application.Models.ViewModel
{
    public class CreateSlot : Slot
    {
        [Required]
        public string StaffID { get; set; }
    }
}
