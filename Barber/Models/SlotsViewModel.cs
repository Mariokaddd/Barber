namespace Barber.Models
{
    public class SlotsViewModel
    {
        public DateTime Date { get; set; }
        public List<TimeSpan> AvailableSlots { get; set; } = null!;
        public int HairstyleId { get; set; }
    }
}
