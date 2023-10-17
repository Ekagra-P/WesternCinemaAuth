using System.ComponentModel.DataAnnotations;

namespace WesternCinemaAuth.Models
{
    public class PriceFactor
    {
        [Range(0.1, 3)]
        public decimal Factor { get; set; }
    }
}
