using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Coin.Data;

namespace Coin.Web.Areas.Vehicles.Pages.VehicleMileageLogs
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
        ViewData["VehicleId"] = new SelectList(_context.Vehicle, "Id", "Make");
        ViewData["VehicleTravelPurposeTypeId"] = new SelectList(_context.VehicleTravelPurposeType, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public VehicleMileageLog VehicleMileageLog { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.VehicleMileageLog.Add(VehicleMileageLog);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}