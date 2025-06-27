using Barber.Data.models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Barber.Data.Models
{
    public class Favorite
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

        [Required]
        public int HairstyleId { get; set; }

        [ForeignKey(nameof(HairstyleId))]
        public Hairstyle Hairstyle { get; set; } = null!;
    }
}
