using System.Collections.Generic;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Transactions;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business
{
    public interface ITransactionMethodRepository : IRepositoryBase<TransactionMethod>
    {
        IEnumerable<TransactionMethod> GetTransactionMethods(string transactionMethodName, TransactionMethodStatus? status, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);

        IEnumerable<TransactionMethod> GetTransactionMethods(TransactionMethodStatus? status);
        bool HasDataExisted(string transactionMethodName);
    }
}
