using System.Collections.Generic;
using Eagle.Core.Extension;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Companies;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business
{
    public interface ICompanyRepository: IRepositoryBase<Company>
    {
        IEnumerable<TreeGrid> GetCompanyTreeGrid(CompanyStatus? status, int? selectedId, bool? isRootShowed = false);
        IEnumerable<Company> GetCompanies(CompanyStatus? status);
        bool HasDataExisted(string name, int? parentId);
        int GetDepth(int? companyId);
        string GetSlogan(int companyId);
        string GetSupportOnline(int companyId);
        int GetNewListOrder();
        bool HasChildren(int companyId);
    }
}
