using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Coin.Web.Areas.Accounting.Pages.AccountTransactions
{
    public class FileUpload
    {
        [Required]
        [Display(Name = "Transactions CSV File")]
        public IFormFile TransactionsFile { get; set; }
    }
}