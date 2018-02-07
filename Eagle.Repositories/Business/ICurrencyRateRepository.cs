using System;
using System.Collections.Generic;
using Eagle.Entities.Business.Transactions;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business
{
    public interface ICurrencyRateRepository : IRepositoryBase<CurrencyRate>
    {
        IEnumerable<CurrencyRate> GetList(ref int? recordCount, string orderBy = null, int? page = null,
            int? pageSize = null);

        CurrencyRate FindByCurrencyRateDate(DateTime currencyRateDate);
        bool HasDataExisted(DateTime currencyRateDate, decimal averageRate);
    }
}
