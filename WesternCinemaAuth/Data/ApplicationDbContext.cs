using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WesternCinemaAuth.Models;

namespace WesternCinemaAuth.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<WesternCinemaAuth.Models.Movie> Movie { get; set; } = default!;
        public DbSet<WesternCinemaAuth.Models.MovieGoer> MovieGoer { get; set; } = default!;
        public DbSet<WesternCinemaAuth.Models.Order> Order { get; set; } = default!;
    }
}