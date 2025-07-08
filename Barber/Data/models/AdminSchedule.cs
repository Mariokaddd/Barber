using System.ComponentModel.DataAnnotations;

namespace Barber.Data.models
{
    public class AdminSchedule
    {
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [Required]
        public TimeSpan SlotDuration { get; set; } = TimeSpan.FromHours(1);
    }
}
