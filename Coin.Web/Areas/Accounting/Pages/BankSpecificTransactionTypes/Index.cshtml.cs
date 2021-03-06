﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Coin.Data;

namespace Coin.Web.Areas.Accounting.Pages.BankSpecificTransactionTypes
{
    public class IndexModel : PageModel
    {
        private readonly Coin.Data.CoinContext _context;

        public IndexModel(Coin.Data.CoinContext context)
        {
            _context = context;
        }

        public IList<BankSpecificTransactionType> BankSpecificTransactionType { get;set; }

        public async Task OnGetAsync()
        {
            BankSpecificTransactionType = await _context.BankSpecificTransactionType
                .Include(b => b.AccountTransactionType)
                .Include(b => b.Bank).ToListAsync();
        }
    }
}
