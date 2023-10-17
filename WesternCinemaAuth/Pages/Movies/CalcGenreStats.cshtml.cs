using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WesternCinemaAuth.Models;

namespace WesternCinemaAuth.Pages.Movies
{
    [Authorize(Roles = "Admin")]
    public class CalcGenreStatsModel : PageModel
    {
        private readonly WesternCinemaAuth.Data.ApplicationDbContext _context;

        public CalcGenreStatsModel(WesternCinemaAuth.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        // For passing the results to the Content file
        public IList<GenreStatistic> GenreStats { get; set; } = default!;

        // GET: /Movies/CalcGenreStats
        public async Task<IActionResult> OnGetAsync()
        {
            // divide the movies into groups by genre
            var movieGroups = _context.Movie.GroupBy(m => m.Genre);

            // for each group, get its genre value and the number of movies in this group
            GenreStats = await movieGroups
                             .Select(g => new GenreStatistic { Genre = g.Key, MovieCount = g.Count() })
                             .ToListAsync();

            return Page();
        }

        /* 
        public void OnGet()
        {
        }
        */
    }
}
