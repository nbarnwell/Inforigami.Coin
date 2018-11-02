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
    public class IndexModel : PageModel
    {
        private readonly Coin.Data.CoinContext _context;

        public IndexModel(Coin.Data.CoinContext context)
        {
            _context = context;
        }

        public IList<VehicleMileageLog> VehicleMileageLog { get;set; }

        public async Task OnGetAsync()
        {
            VehicleMileageLog = await _context.VehicleMileageLog
                .Include(v => v.Vehicle)
                .Include(v => v.VehicleTravelPurposeType).ToListAsync();
        }
    }
}
