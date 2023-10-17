using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WesternCinemaAuth.Data;
using WesternCinemaAuth.Models;

namespace WesternCinemaAuth.Pages.Orders
{
    public class DetailsModel : PageModel
    {
        private readonly WesternCinemaAuth.Data.ApplicationDbContext _context;

        public DetailsModel(WesternCinemaAuth.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Order Order { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            // Include related entities
            var order = await _context.Order
                                .Include(o => o.TheMovie)
                                .Include(o => o.TheMovieGoer)
                                .FirstOrDefaultAsync(m => m.ID == id);
            if (order == null)
            {
                return NotFound();
            }
            else
            {
                Order = order;
            }
            return Page();
        }
    }
}
