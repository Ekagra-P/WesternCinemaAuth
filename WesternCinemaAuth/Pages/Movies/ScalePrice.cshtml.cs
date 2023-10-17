using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WesternCinemaAuth.Models;

namespace WesternCinemaAuth.Pages.Movies
{
    public class ScalePriceModel : PageModel
    {
        private readonly WesternCinemaAuth.Data.ApplicationDbContext _context;

        public ScalePriceModel(WesternCinemaAuth.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        // need 'using <ProjectName>.Models'
        public PriceFactor FactorInput { get; set; } = default!;

        public IActionResult OnGet()
        {
            // No DB access is needed here.
            // Simply display an empty web form
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                ViewData["RowsAffected"] = await _context.Database.ExecuteSqlInterpolatedAsync
                                      ($"UPDATE Movie SET Price = Price * {FactorInput.Factor}");
            }

            return Page();
        }
    }
}
