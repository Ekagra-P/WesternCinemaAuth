using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WesternCinemaAuth.Models
{
    public class MovieGoer
    {
        [Key, Required]
        [DataType(DataType.EmailAddress)]
        // [EmailAddress], the one above implies this one
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string? Email { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [RegularExpression(@"[A-Z][a-z'-]{1,19}")]
        public string? FirstName { get; set; }

        [Required, Display(Name = "Last Name")]
        [RegularExpression(@"[A-Z][a-z'-]{1,19}")]
        public string? LastName { get; set; }

        [NotMapped] // not mapping this property to database, but exist in memory
        public string FullName => $"{FirstName} {LastName}";

        [Required]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^04[0-9]{8}$")]
        public string? Mobile { get; set; }

        // Navigation properties
        public ICollection<Order>? TheOrders { get; set; }

    }
}
