namespace Barber.Data.models
{
    using System.ComponentModel.DataAnnotations;

    public class WorkingSchedule
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan Time { get; set; }

        public bool IsAvailable { get; set; } = true;
    }

}
