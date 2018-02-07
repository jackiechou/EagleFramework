using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Services.Dtos.Business.Personnel;

namespace Eagle.Services.Business
{
    public interface IContractService : IBaseService
    {
        IEnumerable<ContractInfoDetail> GetContracts(ContractSearchEntry filter, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);

        string GenerateCode(int maxLetters);
        ContractDetail GetContractDetail(int id);
        ContractInfoDetail GetContractDetails(int id);
        SelectList PopulateContractStatus(bool? selectedValue = true, bool isShowSelectText = false);
        void InsertContract(ContractEntry entry);
        void UpdateContract(ContractEditEntry entry);
        void UpdateContractStatus(int id, bool status);
    }
}
