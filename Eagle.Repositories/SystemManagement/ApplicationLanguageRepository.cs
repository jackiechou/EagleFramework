using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Common;
using Eagle.Core.Settings;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.SystemManagement
{
    public class ApplicationLanguageRepository : RepositoryBase<ApplicationLanguage>, IApplicationLanguageRepository
    {
        public ApplicationLanguageRepository(IDataContext dataContext) : base(dataContext)
        {
        }

        public ApplicationLanguageInfo GetSelectedLanguage(Guid applicationId)
        {
            return (from al in DataContext.Get<ApplicationLanguage>()
                    join l in DataContext.Get<Language>() on al.LanguageCode equals l.LanguageCode into appLang
                    from lang in appLang.DefaultIfEmpty()
                    where al.Status == ApplicationLanguageStatus.Active && al.ApplicationId == applicationId
                    select new ApplicationLanguageInfo
                    {
                        ApplicationLanguageId = al.ApplicationLanguageId,
                        ApplicationId = al.ApplicationId,
                        LanguageCode = al.LanguageCode,
                        IsSelected = al.IsSelected,
                        Status = al.Status,
                        Language = lang
                    }).FirstOrDefault();
        }

        public IEnumerable<ApplicationLanguageInfo> GetApplicationLanguages(Guid applicationId, ApplicationLanguageStatus? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var query = (from al in DataContext.Get<ApplicationLanguage>()
                         join l in DataContext.Get<Language>() on al.LanguageCode equals l.LanguageCode into appLang
                         from lang in appLang.DefaultIfEmpty()
                         where (status == null || al.Status == status) && al.ApplicationId == applicationId
                         select new ApplicationLanguageInfo
                         {
                             ApplicationLanguageId = al.ApplicationLanguageId,
                             ApplicationId = al.ApplicationId,
                             LanguageCode = al.LanguageCode,
                             IsSelected = al.IsSelected,
                             Status = al.Status,
                             Language = lang
                         });
            return query.WithRecordCount(ref recordCount)
                            .WithSortingAndPaging(orderBy, page, pageSize);
        }

        public IEnumerable<ApplicationLanguage> GetApplicationLanguageList(Guid applicationId, ApplicationLanguageStatus? status = null)
        {
            return (from al in DataContext.Get<ApplicationLanguage>()
                    where (status == null || al.Status == status) && al.ApplicationId == applicationId
                    select al).AsEnumerable();
        }

        public ApplicationLanguage GetDetails(Guid applicationId, string languageCode)
        {
            return (from al in DataContext.Get<ApplicationLanguage>().Include(x => x.Language).DefaultIfEmpty()
                    join l in DataContext.Get<Language>() on al.LanguageCode equals l.LanguageCode into appLang
                    from lang in appLang.DefaultIfEmpty()
                    where al.ApplicationId == applicationId && al.LanguageCode == languageCode
                    select al).FirstOrDefault();
        }
        public SelectList PopulateApplicationLanguages(Guid applicationId, ApplicationLanguageStatus? status = null, string selectedValue = null, bool isShowSelectText = false)
        {
            var lst = (from al in DataContext.Get<ApplicationLanguage>()
                       join l in DataContext.Get<Language>()
                       on al.LanguageCode equals l.LanguageCode
                       where al.ApplicationId == applicationId && al.Status == status
                       select new SelectListItem
                       {
                           Text = l.LanguageName,
                           Value = l.LanguageCode,
                           Selected = !string.IsNullOrEmpty(selectedValue) && l.LanguageCode.ToLower() == selectedValue.ToLower()
                       }).ToList();

            if (lst.Any())
            {
                if (isShowSelectText)
                    lst.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectApplicationLanguage} ---", Value = "" });
            }
            else
            {
                lst.Insert(0, new SelectListItem { Text = $"-- {LanguageResource.None} --", Value = "-1" });
            }

            return new SelectList(lst, "Value", "Text", selectedValue);
        }
        public MultiSelectList PopulateeApplicationLanguageMultiSelectList(Guid applicationId, ApplicationLanguageStatus? status = null, string[] selectedValues = null)
        {
            var lst = (from al in DataContext.Get<ApplicationLanguage>()
                       join l in DataContext.Get<Language>() on al.LanguageCode equals l.LanguageCode into languageJoin
                       from lang in languageJoin.DefaultIfEmpty()
                       where al.ApplicationId == applicationId && (status == null || al.Status == status)
                       select new SelectListItem
                       {
                           Text = lang.LanguageName,
                           Value = lang.LanguageCode,
                           Selected = (al.IsSelected == true)
                       }).ToList();

            return new MultiSelectList(lst, "Value", "Text", selectedValues);
        }
    }
}
