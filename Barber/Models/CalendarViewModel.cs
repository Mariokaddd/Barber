namespace Barber.Models
{
    public class CalendarViewModel
    {
        public List<CalendarDayViewModel> Days { get; set; } = new List<CalendarDayViewModel>();
        public DateTime MonthStart { get; set; }
    }
}
