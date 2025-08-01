using Barber.Data.models;

namespace Barber.Models
{
    public class AppointmentFilterViewModel
    {
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? Date { get; set; }

        public List<Appointment> Results { get; set; } = new();
    }

}
