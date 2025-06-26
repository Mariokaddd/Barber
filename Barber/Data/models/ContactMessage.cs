using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Barber.Data.models
{
    public class ContactMessage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

        [Required]
        [MinLength(10)]
        [MaxLength(1000)]
        public string Message { get; set; } = null!;

        public DateTime SentAt { get; set; } = DateTime.Now;
    }

}
