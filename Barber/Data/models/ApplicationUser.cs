namespace Barber.Data.models
{
    using Barber.Data.Models;
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser
    {
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<ContactMessage> ContactMessages { get; set; } = new List<ContactMessage>();
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    }
}
