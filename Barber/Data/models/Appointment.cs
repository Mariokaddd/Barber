namespace Barber.Data.models
{
    using System.ComponentModel.DataAnnotations;

    public class Appointment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Required]
        public int HairstyleId { get; set; }
        public Hairstyle Hairstyle { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan Time { get; set; }

        public bool IsBooked { get; set; } = true;
    }

}
