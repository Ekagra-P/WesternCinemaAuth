using System.ComponentModel.DataAnnotations;

namespace WesternCinemaAuth.Models
{
    public class TwoMovieGoers
    {
        [Required, EmailAddress]
        public string? MovieGoerA { get; set; }

        [Required, EmailAddress]
        public string? MovieGoerB { get; set; }
    }
}
