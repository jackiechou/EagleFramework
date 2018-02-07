using System.Collections.Generic;
using System.Linq;
using Eagle.Core.Common;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Transactions;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Business
{
    public class TransactionMethodRepository : RepositoryBase<TransactionMethod>, ITransactionMethodRepository
    {
        public TransactionMethodRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<TransactionMethod> GetTransactionMethods(string transactionMethodName, TransactionMethodStatus? status, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null)
        {
            var queryable= DataContext.Get<TransactionMethod>().Where(x => (status == null || x.IsActive == status));
            if (!string.IsNullOrEmpty(transactionMethodName))
            {
                queryable =
                    queryable.Where(x => x.TransactionMethodName.ToLower().Contains(transactionMethodName.ToLower()));
            }
            return queryable.WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }
        public IEnumerable<TransactionMethod> GetTransactionMethods(TransactionMethodStatus? status)
        {
            var queryable = DataContext.Get<TransactionMethod>().Where(x => x.IsActive == status);
            return queryable.AsEnumerable();
        }
        public bool HasDataExisted(string transactionMethodName)
        {
            var entity =
                DataContext.Get<TransactionMethod>().FirstOrDefault(x => x.TransactionMethodName.Contains(transactionMethodName));
            return entity != null;
        }
    }
}
