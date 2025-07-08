namespace Barber.Models
{
    public class ReservationDayViewModel
    {
        public DateTime Date { get; set; }
        public bool HasSchedule { get; set; }
        public int FreeSlotCount { get; set; }
    }
}
