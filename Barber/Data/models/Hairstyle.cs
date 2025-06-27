namespace Barber.Data.models
{
    using Barber.Data.enums;
    using Barber.Data.Models;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Hairstyle
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string Title { get; set; } = null!;

        [Required]
        [MinLength(10)]
        [MaxLength(300)]
        public string Description { get; set; } = null!;

        [Required]
        [Url]
        public string ImageUrl { get; set; } = null!;

        [Required]
        public Gender Gender { get; set; }

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    }

}
