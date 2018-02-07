using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Common;
using Eagle.Entities.Skins;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.Skins
{
    public class SkinPackageRepository: RepositoryBase<SkinPackage>, ISkinPackageRepository
    {
        public SkinPackageRepository(IDataContext dataContext) : base(dataContext) { }
        public Theme GetSelectedTheme(Guid applicationId)
        {
            var entity = (from p in DataContext.Get<SkinPackage>() 
                          where p.IsActive && p.ApplicationId == applicationId && p.IsSelected
                          select new Theme
                          {
                              PackageName = p.PackageName,
                              PackageSrc = p.PackageSrc
                          }).FirstOrDefault();
            return entity;
        }
        
        public IEnumerable<SkinPackageInfo> GetSkinPackages(Guid applicationId, int? typeId, bool? status, out int recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var result = from p in DataContext.Get<SkinPackage>()
                         join t in DataContext.Get<SkinPackageType>() on p.TypeId equals t.TypeId into ptJoin
                         from type in ptJoin.DefaultIfEmpty()
                         where p.ApplicationId == applicationId 
                         && (typeId == null || p.TypeId == typeId)
                         && (p.IsActive == status || status == null)
                         select new SkinPackageInfo
                         {
                             ApplicationId = p.ApplicationId,
                             TypeId = p.TypeId,
                             PackageId = p.PackageId,
                             PackageName = p.PackageName,
                             PackageAlias = p.PackageAlias,
                             PackageSrc = p.PackageSrc,
                             IsSelected = p.IsSelected,
                             IsActive = p.IsActive,
                             Type = type
                         };

            return result.OrderBy(t => t.PackageId)
                .WithRecordCount(out recordCount)
                .WithSortingAndPaging(orderBy, page, pageSize);
        }
        public IEnumerable<SkinPackage> GetSkinPackages(Guid applicationId, int? typeId, bool? status = null)
        {
            return (from p in DataContext.Get<SkinPackage>()
                   where p.ApplicationId == applicationId
                   && (typeId == null || p.TypeId == typeId)
                   && (p.IsActive == status || status == null)
                   select p).AsEnumerable();
        }

        public SkinPackageInfo GetDetail(int packageId)
        {
            return (from p in DataContext.Get<SkinPackage>() 
                    join t in DataContext.Get<SkinPackageType>() on p.TypeId equals t.TypeId into tsJoin
                    from type in tsJoin.DefaultIfEmpty()
                    where p.PackageId == packageId
                    select new SkinPackageInfo
                    {
                        ApplicationId = p.ApplicationId,
                        TypeId = p.TypeId,
                        PackageId = p.PackageId,
                        PackageName = p.PackageName,
                        PackageAlias = p.PackageAlias,
                        PackageSrc = p.PackageSrc,
                        IsSelected = p.IsSelected,
                        IsActive = p.IsActive,
                        Type = type
                    }).FirstOrDefault();
        }

        public string GetSkinSrcBySkinId(int packageId)
        {
            return (from p in DataContext.Get<SkinPackage>()
                    where p.PackageId == packageId
                    select p.PackageSrc).FirstOrDefault();
        }

        public string GetCssWithTheme(ViewContext viewContext)
        {
            var themeName = ConfigurationManager.AppSettings["themeName"];

            return $"~/Themes/{viewContext.HttpContext.Items[themeName]}/Content/style.css";
        }

        public bool HasDataExisted(Guid applicationId, string skinPackageName)
        {
            var enttity = from p in DataContext.Get<SkinPackage>()
                where p.ApplicationId == applicationId && p.PackageName == skinPackageName
                select p;
            return (enttity.Any());
        }

        public SelectList PopulateSkinPackageSelectList(Guid applicationId, int? typeId, bool? status = null, int? selectedValue = null, bool? isShowSelectText = true)
        {
            var lst = (from p in DataContext.Get<SkinPackage>()
                       where p.ApplicationId == applicationId && (typeId == null || p.TypeId == typeId) && (status == null || p.IsActive == status)
                       select new SelectListItem { Text = p.PackageName, Value = p.PackageId.ToString(), Selected = (selectedValue != null && p.PackageId == selectedValue) }).ToList();

            if (lst.Any())
            {
                if (isShowSelectText != null && isShowSelectText == true)
                    lst.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectSkinPackage} ---", Value = "" });
            }
            else
            {
                lst.Insert(0, new SelectListItem { Text = $"-- {LanguageResource.None} --", Value = "-1" });
            }
            return new SelectList(lst, "Value", "Text", selectedValue);
        }

        public SelectList PopulateSkinPackageStatus(bool? selectedValue = null, bool? isShowSelectText = true)
        {
            List<SelectListItem> lst = new List<SelectListItem>
            {
                 new SelectListItem {Text = LanguageResource.Active, Value = "True", Selected = (selectedValue != null && selectedValue == true) },
                new SelectListItem {Text = LanguageResource.InActive, Value = "False", Selected = (selectedValue == null || selectedValue == false) }
            };
            if (isShowSelectText != null && isShowSelectText == true)
                lst.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectStatus} ---", Value = "" });
            return new SelectList(lst, "Value", "Text", selectedValue);
        }

    }
}
