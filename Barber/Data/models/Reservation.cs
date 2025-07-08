using System.ComponentModel.DataAnnotations;

namespace Barber.Data.models
{
    public class Reservation
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;

        [Required]
        public int HairstyleId { get; set; }
        public Hairstyle Hairstyle { get; set; } = null!;

        [Required]
        public DateTime Date { get; set; }
        [Required]
        public TimeSpan TimeSlot { get; set; }
    }

}
