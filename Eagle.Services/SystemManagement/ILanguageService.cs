using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Services.Dtos.SystemManagement;

namespace Eagle.Services.SystemManagement
{
    public interface ILanguageService : IBaseService
    {
        ApplicationLanguageDetail GetSelectedLanguage(Guid applicationId);
        IEnumerable<ApplicationLanguageDetail> GetApplicationLanguages(Guid applicationId, LanguageSearchEntry filter, ref int? recordCount,
           string orderBy = null, int? page = null, int? pageSize = null);
        ApplicationLanguageDetail GetApplicationLanguageDetail(Guid applicationId, string languageCode);
        SelectList PopulateApplicationLanguages(Guid applicationId, ApplicationLanguageStatus? status = null, string selectedValue = null, bool isShowSelectText = false);
        void EditApplicationLanguages(Guid applicationId, ApplicationLanguageListEntry entry);
        void InsertApplicationLanguage(Guid applicationId, ApplicationLanguageEntry entry);
        void UpdateSelectedApplicationLanguage(Guid applicationId, string languageCode);
        void UpdateApplicationLanguageStatus(Guid applicationId, string languageCode, ApplicationLanguageStatus status);
        MultiSelectList PopulateLanguageMultiSelectList(LanguageStatus? status = null, string[] selectedValues = null);

        MultiSelectList PopulateAvailableLanguageMultiSelectList(LanguageStatus? status = null,
            string[] selectedValues = null);
        MultiSelectList PopulateSelectedLanguageMultiSelectList(Guid applicationId, ApplicationLanguageStatus? status = null,
            string[] selectedValues = null);
    }
}
