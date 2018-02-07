using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.SystemManagement
{
    public interface ILanguageRepository : IRepositoryBase<Language>
    {
        Language GetDetailsByCode(string languageCode);
        SelectList PopulateLanguages(string selectedValue, bool isShowSelectText = false);
        MultiSelectList PopulateLanguageMultiSelectList(LanguageStatus? status = null, string[] selectedValues = null);

        MultiSelectList PopulateAvailableLanguageMultiSelectList(LanguageStatus? status = null,
            string[] selectedValues = null);
    }
}
