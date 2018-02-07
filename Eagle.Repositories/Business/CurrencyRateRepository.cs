using System;
using System.Collections.Generic;
using System.Linq;
using Eagle.Core.Common;
using Eagle.Entities.Business.Transactions;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Business
{
    public class CurrencyRateRepository: RepositoryBase<CurrencyRate>, ICurrencyRateRepository
    {
        public CurrencyRateRepository(IDataContext dataContext) : base(dataContext) { }

        public IEnumerable<CurrencyRate> GetList(ref int? recordCount,string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = DataContext.Get<CurrencyRate>();
            return lst.WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }
        public CurrencyRate FindByCurrencyRateDate(DateTime currencyRateDate)
        {
            return DataContext.Get<CurrencyRate>().FirstOrDefault(x =>x.CurrencyRateDate == currencyRateDate);
        }

        public bool HasDataExisted(DateTime currencyRateDate, decimal averageRate)
        {
            var entity =
                DataContext.Get<CurrencyRate>().FirstOrDefault(x => 
                x.CurrencyRateDate == currencyRateDate && x.AverageRate == averageRate);
            return entity != null;
        }
    }
}
