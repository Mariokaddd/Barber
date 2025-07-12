using System.ComponentModel.DataAnnotations;

namespace Barber.Models
{
    public class ScheduleFormViewModel
    {
        [Required(ErrorMessage = "Моля, изберете дата.")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Моля, изберете час.")]
        [Range(9, 17, ErrorMessage = "Часът трябва да е между 9 и 17.")]
        public int? Hour { get; set; }

        [Display(Name = "Бележка (по избор)")]
        [StringLength(200)]
        public string? Description { get; set; }

        public bool IsAvailable { get; set; } = true;
    }
}
