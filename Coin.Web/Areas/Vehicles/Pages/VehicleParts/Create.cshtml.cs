using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Coin.Data;

namespace Coin.Web.Areas.Vehicles.Pages.VehicleParts
{
    public class CreateModel : PageModel
    {
        private readonly Coin.Data.CoinContext _context;

        public CreateModel(Coin.Data.CoinContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["VehicleTypeId"] = new SelectList(_context.VehicleType, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public VehiclePart VehiclePart { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.VehiclePart.Add(VehiclePart);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}