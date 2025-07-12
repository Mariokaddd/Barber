namespace Barber.Data.models
{
    using Barber.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;

    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = null!;

        [Required]
        [Phone]
        public override string PhoneNumber { get; set; } = null!;

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<ContactMessage> ContactMessages { get; set; } = new List<ContactMessage>();
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    }
}
