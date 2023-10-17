using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WesternCinemaAuth.Data;
using WesternCinemaAuth.Models;

namespace WesternCinemaAuth.Pages.Orders
{
    public class EditModel : PageModel
    {
        private readonly WesternCinemaAuth.Data.ApplicationDbContext _context;

        public EditModel(WesternCinemaAuth.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Order Order { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FirstOrDefaultAsync(m => m.ID == id);
            if (order == null)
            {
                return NotFound();
            }
            Order = order;
            //ViewData["MovieID"] = new SelectList(_context.Movie, "ID", "Genre");
            //ViewData["MovieGoerEmail"] = new SelectList(_context.MovieGoer, "Email", "Email");
            ViewData["MovieID"] = new SelectList(_context.Movie, "ID", "Title");
            ViewData["MovieGoerEmail"] = new SelectList(_context.MovieGoer, "Email", "FullName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(Order.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool OrderExists(int id)
        {
          return (_context.Order?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
