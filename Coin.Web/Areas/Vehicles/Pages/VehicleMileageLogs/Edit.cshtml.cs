using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Coin.Data;

namespace Coin.Web.Areas.Vehicles.Pages.VehicleMileageLogs
{
    public class EditModel : PageModel
    {
        private readonly Coin.Data.CoinContext _context;

        public EditModel(Coin.Data.CoinContext context)
        {
            _context = context;
        }

        [BindProperty]
        public VehicleMileageLog VehicleMileageLog { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            VehicleMileageLog = await _context.VehicleMileageLog
                .Include(v => v.Vehicle)
                .Include(v => v.VehicleTravelPurposeType).FirstOrDefaultAsync(m => m.Id == id);

            if (VehicleMileageLog == null)
            {
                return NotFound();
            }
           ViewData["VehicleId"] = new SelectList(_context.Vehicle, "Id", "Make");
           ViewData["VehicleTravelPurposeTypeId"] = new SelectList(_context.VehicleTravelPurposeType, "Id", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(VehicleMileageLog).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleMileageLogExists(VehicleMileageLog.Id))
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

        private bool VehicleMileageLogExists(int id)
        {
            return _context.VehicleMileageLog.Any(e => e.Id == id);
        }
    }
}
