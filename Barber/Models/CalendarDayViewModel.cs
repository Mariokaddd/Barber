namespace Barber.Models
{
    public class CalendarDayViewModel
    {
        public DateTime Date { get; set; }

        // Това е за да оцветиш деня ако има свободни часове
        public bool HasSchedule { get; set; }

        // Колко свободни часа има — примерно за да покажеш баджче
        public int FreeSlotCount { get; set; }
    }

}
