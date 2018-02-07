using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Employees;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business.Employees
{
    public interface IContractRepository : IRepositoryBase<Contract>
    {
        IEnumerable<ContractInfo> GetContracts(string searchText, ContractType? contractTypeId, bool? status, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);

        ContractInfo GetDetails(int id);
        bool HasContractNoExisted(string contractNo);
        SelectList PopulateContractStatus(bool? selectedValue= true, bool isShowSelectText = false);
        string GenerateCode(int maxLetters);
    }
}
