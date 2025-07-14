namespace Barber.Models
{
    public class BookingViewModel
    {
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Phone { get; set; } = "";
        public bool IsBooked { get; set; }
    }

}
