using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using WesternCinemaAuth.Models;

namespace WesternCinemaAuth.Pages.MovieGoers
{
    public class PeopleDiffModel : PageModel
    {
        private readonly WesternCinemaAuth.Data.ApplicationDbContext _context;

        public PeopleDiffModel(WesternCinemaAuth.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        // need 'using <ProjectName>.Models'
        public TwoMovieGoers MovieGoersInput { get; set; }

        // List of different movies; for passing to Content file to display
        public IList<Movie> DiffMovies { get; set; }

        // GET: MovieGoers/PeopleDiff
        public IActionResult OnGet()
        {
            // Get the options for the MovieGoer select list from the database
            // and save them in ViewData for passing to Content file
            ViewData["MovieGoerList"] = new SelectList(_context.MovieGoer, "Email", "FullName");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // prepare the parameters to be inserted into the query
            var emailA = new SqliteParameter("personA", MovieGoersInput.MovieGoerA);
            var emailB = new SqliteParameter("personB", MovieGoersInput.MovieGoerB);

            // Construct the query to get the movies watched by Moviegoer A but not Moviegoer B
            // Use placeholders as the parameters
            var diffMovies = _context.Movie.FromSqlRaw("select [Movie].* from [Movie] inner join [Order] on "
                             + "[Movie].ID = [Order].MovieID where [Order].MovieGoerEmail = @personA and "
                             + "[Movie].ID not in (select [Movie].ID from [Movie] inner join [Order] on "
                             + "[Movie].ID = [Order].MovieID where [Order].MovieGoerEmail = @personB)", emailA, emailB);

            //.Select(mo => new Movie { ID = mo.ID, Genre = mo.Genre, Price = mo.Price, ReleaseDate = mo.ReleaseDate, Title = mo.Title });

            // Run the query and save the results in DiffMovies for passing to content file
            DiffMovies = await diffMovies.ToListAsync();
            // Save the options for both dropdown lists in ViewData for passing to content file
            ViewData["MovieGoerList"] = new SelectList(_context.MovieGoer, "Email", "FullName");
            // invoke the content file
            return Page();
        }

        /*
        public void OnGet()
        {
        }
        */
    }
}
