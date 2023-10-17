using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WesternCinemaAuth.Data;
using WesternCinemaAuth.Models;

namespace WesternCinemaAuth.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly WesternCinemaAuth.Data.ApplicationDbContext _context;

        public IndexModel(WesternCinemaAuth.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Movie> Movie { get;set; } = default!;

        // GET: /Movies?sortOrder=xxx or /Movies/Index?sortOrder=xxx
        public async Task OnGetAsync(string? sortOrder)
        {
            if (string.IsNullOrEmpty(sortOrder))
            {
                // When the Index page is loaded for the first time, the sortOrder is empty.
                // By default, the movies should be displayed in the order of title_asc.
                sortOrder = "title_asc";
            }

            // Prepare the query for getting the entire list of movies.
            // Convert the data type from DbSet<Movie> to IQueryable<Movie>
            var movies = (IQueryable<Movie>)_context.Movie;

            // Sort the movies by specified order
            switch (sortOrder)
            {
                case "title_asc":
                    movies = movies.OrderBy(m => m.Title);
                    break;
                case "title_desc":
                    movies = movies.OrderByDescending(m => m.Title);
                    break;
                case "price_asc":
                    movies = movies.OrderBy(m => (double)m.Price);
                    break;
                case "price_desc":
                    movies = movies.OrderByDescending(m => (double)m.Price);
                    break;
            }

            // Deciding the query string (sortOrder=xxx) to include in the heading links
            // for Title and Price respectively.
            // They specify the next display order if a heading link is clicked. 
            // Store them in ViewData dictionary to pass them to View.
            ViewData["NextTitleOrder"] = sortOrder != "title_asc" ? "title_asc" : "title_desc";
            ViewData["NextPriceOrder"] = sortOrder != "price_asc" ? "price_asc" : "price_desc";

            // Access database to execute the query prepared above
            // Assign the returned movie list to the Movie property of this PageModel class
            Movie = await movies.AsNoTracking().ToListAsync();

            /* ***************************************************
             * the original code from scaffolding
            if (_context.Movie != null)
            {
                Movie = await _context.Movie.ToListAsync();
            }
            *****************************************************/
        }
    }
}
