using System;
using System.Collections.Generic;
using Eagle.Core.Extension;
using Eagle.Core.Settings;
using Eagle.Services.Dtos.Business;

namespace Eagle.Services.Business
{
    public interface ICompanyService : IBaseService
    {
        #region Company
        IEnumerable<TreeGrid> GetCompanyTreeGrid(CompanyStatus? status, int? selectedId, bool? isRootShowed);
        IEnumerable<CompanyDetail> GetCompanies(CompanyStatus? status);
        CompanyDetail GetCompanyDetail(int id);
        string GetLogo(int id);
        string GetSlogan(int id);
        string GetSupportOnline(int id);
        void InsertCompany(Guid applicationId, Guid userId, CompanyEntry entry);
        void UpdateCompany(Guid applicationId, Guid userId, CompanyEditEntry entry);
        void UpdateCompanyStatus(Guid userId, int id, CompanyStatus status);

        #endregion

    }
}
