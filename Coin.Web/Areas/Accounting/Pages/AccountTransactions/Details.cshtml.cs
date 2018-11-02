﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Coin.Data;

namespace Coin.Web.Areas.Accounting.Pages.AccountTransactions
{
    public class DetailsModel : PageModel
    {
        private readonly Coin.Data.CoinContext _context;

        public DetailsModel(Coin.Data.CoinContext context)
        {
            _context = context;
        }

        public AccountTransaction AccountTransaction { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AccountTransaction = await _context.AccountTransaction
                .Include(a => a.AccountStatement)
                .Include(a => a.AccountTransactionStatus)
                .Include(a => a.AccountTransactionType).FirstOrDefaultAsync(m => m.Id == id);

            if (AccountTransaction == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
