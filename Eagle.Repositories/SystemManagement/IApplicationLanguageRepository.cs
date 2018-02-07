using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.SystemManagement
{
    public interface IApplicationLanguageRepository: IRepositoryBase<ApplicationLanguage>
    {
        ApplicationLanguageInfo GetSelectedLanguage(Guid applicationId);
        IEnumerable<ApplicationLanguageInfo> GetApplicationLanguages(Guid applicationId, ApplicationLanguageStatus? status,
            ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<ApplicationLanguage> GetApplicationLanguageList(Guid applicationId,
            ApplicationLanguageStatus? status=null);
        ApplicationLanguage GetDetails(Guid applicationId, string languageCode);
        SelectList PopulateApplicationLanguages(Guid applicationId, ApplicationLanguageStatus? status = null, string selectedValue = null, bool isShowSelectText = false);
        MultiSelectList PopulateeApplicationLanguageMultiSelectList(Guid applicationId, ApplicationLanguageStatus? status = null, string[] selectedValues = null);
    }
}
