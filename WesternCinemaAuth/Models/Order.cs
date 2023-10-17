using System.ComponentModel.DataAnnotations;

namespace WesternCinemaAuth.Models
{
    public class Order
    {
        // primary key
        public int ID { get; set; }

        // foreign key
        public int MovieID { get; set; }

        // foreign key
        [Required]
        [DataType(DataType.EmailAddress)]
        //[EmailAddress], implied above
        public string? MovieGoerEmail { get; set; }

        [Range(1, 10)]
        [Display(Name = "Ticket Count")]
        public int TicketCount { get; set; }

        [Range(0, 499.0)]
        [Display(Name = "Total Price")]
        public decimal TotalPrice { get; set; }

        // Navigation properties
        [Display(Name = "The Movie")]
        public Movie? TheMovie { get; set; }
        [Display(Name = "The Moviegoer")]
        public MovieGoer? TheMovieGoer { get; set; }
    }
}
