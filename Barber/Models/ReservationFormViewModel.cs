using System.ComponentModel.DataAnnotations;

namespace Barber.Models
{
    public class ReservationFormViewModel
    {
        public int? HairstyleId { get; set; }
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Hour { get; set; } = null!;

        [Required]
        [Display(Name = "Име")]
        public string FirstName { get; set; } = null!;

        [Required]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; } = null!;

        [Required]
        [Phone]
        [Display(Name = "Телефон")]
        public string Phone { get; set; } = null!;

        public string? Notes { get; set; }
    }

}
