namespace Barber.Data.models
{
    using System.ComponentModel.DataAnnotations;

    public class Appointment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;

        public int? HairstyleId { get; set; }
        public Hairstyle? Hairstyle { get; set; } = null!;

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan Time { get; set; }

        public bool IsBooked { get; set; } = true;
    }

}
