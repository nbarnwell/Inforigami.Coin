using System;

namespace Coin.Web.Areas.Accounting.TransactionImport
{
    public class TransactionImportRow
    {
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public string SortCode { get; set; }
        public string AccountNumber { get; set; }
        public string TransactionDescription { get; set; }
        public decimal? DebitAmount { get; set; }
        public decimal? CreditAmount { get; set; }
        public decimal? Balance { get; set; }

        public override string ToString()
        {
            return $"{nameof(TransactionDate)}: {TransactionDate}, {nameof(TransactionType)}: {TransactionType}, {nameof(SortCode)}: {SortCode}, {nameof(AccountNumber)}: {AccountNumber}, {nameof(TransactionDescription)}: {TransactionDescription}, {nameof(DebitAmount)}: {DebitAmount}, {nameof(CreditAmount)}: {CreditAmount}, {nameof(Balance)}: {Balance}";
        }
    }
}