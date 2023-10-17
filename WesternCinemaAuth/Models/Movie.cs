using System.ComponentModel.DataAnnotations;

namespace WesternCinemaAuth.Models
{
    public class Movie
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Each movie must have a title!")]
        public string? Title { get; set; }

        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z][a-z]{1,15}$", ErrorMessage = "A Genre name must start with a captical letter and continue with 1-15 lower-case letters.")]
        public string? Genre { get; set; }

        [Range(1, 100)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        // Navigation properties
        public ICollection<Order>? TheOrders { get; set; }

    }
}
