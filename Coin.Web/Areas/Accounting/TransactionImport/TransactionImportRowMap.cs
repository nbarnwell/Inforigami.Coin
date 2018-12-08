using CsvHelper.Configuration;

namespace Coin.Web.Areas.Accounting.TransactionImport
{
    public class TransactionImportRowMap : ClassMap<TransactionImportRow>
    {
        public TransactionImportRowMap()
        {
        }
    }
}