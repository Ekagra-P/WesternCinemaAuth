using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WesternCinemaAuth.Models;

namespace WesternCinemaAuth.Pages.MovieGoers
{
    [Authorize(Roles = "Moviegoers")]
    public class MyDetailsModel : PageModel
    {
        private readonly WesternCinemaAuth.Data.ApplicationDbContext _context;

        public MyDetailsModel(WesternCinemaAuth.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public MovieGoer? Myself { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // retrieve the logged-in user's email
            // need to add "using System.Security.Claims;"
            string _email = User.FindFirst(ClaimTypes.Name).Value;

            // The line of code below is replaced by Raw SQL in the subsequent three lines 
            //MovieGoer movieGoer = await _context.MovieGoer.FirstOrDefaultAsync(m => m.Email == _email);

            string query = "SELECT * From MovieGoer Where Email = @EmailAddr";
            var parameter1 = new SqliteParameter("@EmailAddr", _email);
            MovieGoer movieGoer = await _context.MovieGoer.FromSqlRaw(query, parameter1).FirstOrDefaultAsync();

            /* The two lines below are not recommended, as they cannot resist SQLI attacks.
               string query = $"SELECT * From MovieGoer Where Email = {_email}";
               MovieGoer movieGoer = await _context.MovieGoer.FromSqlRaw(query).FirstOrDefaultAsync();
            */

            /* dealing with both cases of new user and existing user */
            if (movieGoer != null)
            {
                // existing user
                ViewData["ExistInDB"] = "true";
                Myself = movieGoer;
            }
            else
            {
                // new user
                ViewData["ExistInDB"] = "false";
            }

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            // retrieve the logged-in user's email
            // need to add "using System.Security.Claims;"
            string _email = User.FindFirst(ClaimTypes.Name).Value;

            MovieGoer movieGoer = await _context.MovieGoer.FirstOrDefaultAsync(m => m.Email == _email);

            /* dealing with both cases of new user and existing user */
            if (movieGoer != null)
            {
                // This ViewData entry is needed in the content file
                // The user has been created in the database
                ViewData["ExistInDB"] = "true";
            }
            else
            {
                // new user
                ViewData["ExistInDB"] = "false";
                movieGoer = new MovieGoer();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            /* for preventing overposting attacks */
            movieGoer.Email = _email;

            /* movieGoer: the object to update using the values submitted.
             * "MovieGoer": the Prefix used in the input names in web form.
             * Lambda expression: list the properties to be updated. If not listed, 
             *                    no updates, thus preventing overposting.
             */
            var success = await TryUpdateModelAsync<MovieGoer>(movieGoer, "Myself",
                                s => s.FirstName, s => s.LastName, s => s.Mobile);
            if (!success)
            {
                return Page();
            }

            if ((string)ViewData["ExistInDB"] == "true")
            {
                // Since the context doesn't allow tracking two objects with the same key,
                // we do the copying first, and then update.
                _context.MovieGoer.Update(movieGoer);
            }
            else
            {
                // add this newly-created record into db
                _context.MovieGoer.Add(movieGoer);
            }

            try  // catching the conflict of editing this record concurrently
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            ViewData["SuccessDB"] = "success";
            return Page();
        }
    }
}
