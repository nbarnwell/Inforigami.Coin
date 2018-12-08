using System;
using System.IO;
using System.Threading.Tasks;
using Coin.Data;
using Coin.Web.Areas.Accounting.TransactionImport;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Coin.Web.Areas.Accounting.Pages.AccountTransactions
{
    public class ImportModel : PageModel
    {
        private readonly ITransactionImporter _importer;
        private readonly CoinContext _context;

        public ImportModel(ITransactionImporter importer, CoinContext context)
        {
            _importer = importer ?? throw new ArgumentNullException(nameof(importer));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [BindProperty]
        public FileUpload FileUpload { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            // Perform an initial check to catch FileUpload class
            // attribute violations.
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var transactionData =
                await FileHelpers.ProcessFormFile<TransactionImportRow>(
                    FileUpload.TransactionsFile,
                    ModelState);

            // Perform a second check to catch ProcessFormFile method
            // violations.
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userId = this.User.GetUserId();
            var person = await _context.Person.SingleOrDefaultAsync(x => x.Name == userId);

            await _importer.Import(transactionData, person.Id);

            return RedirectToPage("./Index");
        }
    }
}