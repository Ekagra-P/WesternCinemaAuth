using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WesternCinemaAuth.Data;
using WesternCinemaAuth.Models;

namespace WesternCinemaAuth.Pages.Orders
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly WesternCinemaAuth.Data.ApplicationDbContext _context;

        public CreateModel(WesternCinemaAuth.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            //ViewData["MovieID"] = new SelectList(_context.Movie, "ID", "Genre");
            //ViewData["MovieGoerEmail"] = new SelectList(_context.MovieGoer, "Email", "Email");
            ViewData["MovieID"] = new SelectList(_context.Movie, "ID", "Title");
            ViewData["MovieGoerEmail"] = new SelectList(_context.MovieGoer, "Email", "FullName");
            return Page();
        }

        [BindProperty]
        public Order Order { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.Order == null || Order == null)
            {
                return Page();
            }

            var emptyOrder = new Order();

            /* emptyOrder: the object to update using the values submitted.
             * "Order": the Prefix used in the input names in web form.
             * Lambda expression: list the properties to be updated. If not listed, 
             *                    no updates, thus preventing overposting.
             */
            var success = await TryUpdateModelAsync<Order>(emptyOrder, "Order",
                                s => s.MovieID, s => s.MovieGoerEmail, s => s.TicketCount);
            if (success)
            {
                var theMovie = await _context.Movie.FindAsync(emptyOrder.MovieID);
                // calculate the total price of this order
                emptyOrder.TotalPrice = emptyOrder.TicketCount * theMovie.Price;
                // add this newly-created order into db
                _context.Order.Add(emptyOrder);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            else
            {
                return Page();
            }
        }
    }
}
