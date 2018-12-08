using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coin.Web.Areas.Accounting.TransactionImport
{
    public interface ITransactionImporter
    {
        Task<Task<int>> Import(IEnumerable<TransactionImportRow> transactions, int personId);
    }
}