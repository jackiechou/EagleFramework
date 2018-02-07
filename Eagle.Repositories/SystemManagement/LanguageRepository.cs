using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.SystemManagement
{
    public class LanguageRepository : RepositoryBase<Language>, ILanguageRepository
    {
        public LanguageRepository(IDataContext dataContext) : base(dataContext) { }
        
        public Language GetDetailsByCode(string languageCode)
        {
            return DataContext.Get<Language>().SingleOrDefault(x => x.LanguageCode == languageCode);
        }

        public SelectList PopulateLanguages(string selectedValue, bool isShowSelectText = false)
        {
            List<SelectListItem> lst = new List<SelectListItem>
            {
                new SelectListItem {Text = LanguageResource.Vietnamese, Value = "vi-VN"},
                new SelectListItem {Text = LanguageResource.English, Value = "en-Us"}
            };
            if (isShowSelectText)
                lst.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectLanguage} ---", Value = "" });
            return new SelectList(lst, "Value", "Text", selectedValue);
        }
        public MultiSelectList PopulateLanguageMultiSelectList(LanguageStatus? status=null, string[] selectedValues = null)
        {
            var query = from x in DataContext.Get<Language>()
                        where (status == null || x.Status == status)
                        select x;

            var lst = query.Select(p => new SelectListItem
            {
                Text = p.LanguageName,
                Value = p.LanguageCode
            }).ToList();

            if (lst.Count == 0)
                lst.Insert(0, new SelectListItem() { Value = "-1", Text = $"-- {LanguageResource.None} --" });
          
            return new MultiSelectList(lst, "Value", "Text", selectedValues);
        }

        public MultiSelectList PopulateAvailableLanguageMultiSelectList(LanguageStatus? status = null, string[] selectedValues = null)
        {
            var selectedLanguageCodes = (from x in DataContext.Get<ApplicationLanguage>()
                                     select x.LanguageCode).ToList();
            var availableLanguages = (from x in DataContext.Get<Language>()
                             where (status == null || x.Status == status)
                             where !selectedLanguageCodes.Contains(x.LanguageCode)
                            select x).ToList();

            var lst = availableLanguages.Select(p => new SelectListItem
            {
                Text = p.LanguageName,
                Value = p.LanguageCode
            }).ToList();

            if (lst.Count == 0)
                lst.Insert(0, new SelectListItem() { Value = "-1", Text = $"-- {LanguageResource.None} --" });

            return new MultiSelectList(lst, "Value", "Text", selectedValues);
        }
    }
}
