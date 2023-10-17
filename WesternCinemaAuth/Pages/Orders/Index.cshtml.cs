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
    public class IndexModel : PageModel
    {
        private readonly WesternCinemaAuth.Data.ApplicationDbContext _context;

        public IndexModel(WesternCinemaAuth.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Order> Order { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Order != null)
            {
                Order = await _context.Order
                .Include(o => o.TheMovie)
                .Include(o => o.TheMovieGoer).ToListAsync();
            }
        }
    }
}
