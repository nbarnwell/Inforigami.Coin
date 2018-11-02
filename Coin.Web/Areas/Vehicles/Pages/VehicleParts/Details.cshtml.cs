using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Coin.Data;

namespace Coin.Web.Areas.Vehicles.Pages.VehicleParts
{
    public class DetailsModel : PageModel
    {
        private readonly Coin.Data.CoinContext _context;

        public DetailsModel(Coin.Data.CoinContext context)
        {
            _context = context;
        }

        public VehiclePart VehiclePart { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            VehiclePart = await _context.VehiclePart
                .Include(v => v.VehicleType).FirstOrDefaultAsync(m => m.Id == id);

            if (VehiclePart == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
