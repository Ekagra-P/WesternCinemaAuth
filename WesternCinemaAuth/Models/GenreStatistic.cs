using System.ComponentModel.DataAnnotations;

namespace WesternCinemaAuth.Models
{
    public class GenreStatistic
    {
        [Display(Name = "Movie Genre")]
        public string? Genre { get; set; }

        [Display(Name = "Movie Count")]
        public int MovieCount { get; set; }
    }
}
