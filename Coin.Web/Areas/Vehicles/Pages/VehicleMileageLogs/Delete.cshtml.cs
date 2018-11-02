using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Coin.Data;

namespace Coin.Web.Areas.Vehicles.Pages.VehicleMileageLogs
{
    public class DeleteModel : PageModel
    {
        private readonly Coin.Data.CoinContext _context;

        public DeleteModel(Coin.Data.CoinContext context)
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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            VehicleMileageLog = await _context.VehicleMileageLog.FindAsync(id);

            if (VehicleMileageLog != null)
            {
                _context.VehicleMileageLog.Remove(VehicleMileageLog);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
