using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Common;
using Eagle.Core.UI.Layout;
using Eagle.Entities.Skins;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.Skins
{
    public class SkinPackageTemplateRepository : RepositoryBase<SkinPackageTemplate>, ISkinPackageTemplateRepository
    {
        public SkinPackageTemplateRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<SkinPackageTemplateInfo> GetSkinPackageTemplates(int? packageId, bool? status, out int recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var result = from t in DataContext.Get<SkinPackageTemplate>()
                         join p in DataContext.Get<SkinPackage>() on t.PackageId equals p.PackageId into tpJoin
                         from package in tpJoin.DefaultIfEmpty()
                         join pt in DataContext.Get<SkinPackageType>() on package.TypeId equals pt.TypeId into ptJoin
                         from type in ptJoin.DefaultIfEmpty()
                         where (status==null || t.IsActive == status) && (packageId==null || t.PackageId == packageId)
                         select new SkinPackageTemplateInfo
                         {
                             TypeId = t.TypeId,
                             PackageId = t.PackageId,
                             TemplateId = t.TemplateId,
                             TemplateName = t.TemplateName,
                             TemplateKey = t.TemplateKey,
                             TemplateSrc = t.TemplateSrc,
                             IsActive = t.IsActive,
                             Package = package,
                             Type = type
                         };

            return result.WithRecordCount(out recordCount)
                .WithSortingAndPaging(orderBy, page, pageSize);
        }

        public IEnumerable<SkinPackageTemplate> GetListByPackageId(int skinPackageId)
        {
            return (from x in DataContext.Get<SkinPackageTemplate>() where x.PackageId == skinPackageId select x).AsEnumerable();
        }

        public IEnumerable<SkinPackageTemplate> GetListBySelectedSkin()
        {
            var lst = (from t in DataContext.Get<SkinPackageTemplate>()
                       join s in DataContext.Get<SkinPackage>() on t.PackageId equals s.PackageId
                       where s.IsSelected == true
                       select t).AsEnumerable();
            return lst;
        }

        public SkinPackageTemplateInfo GetDetail(int templateId)
        {
            return (from t in DataContext.Get<SkinPackageTemplate>()
                    join p in DataContext.Get<SkinPackage>() on t.PackageId equals p.PackageId into tpJoin
                    from package in tpJoin.DefaultIfEmpty()
                    join pt in DataContext.Get<SkinPackageType>() on package.TypeId equals pt.TypeId into ptJoin
                    from type in ptJoin.DefaultIfEmpty()
                    where t.TemplateId == templateId
                    select new SkinPackageTemplateInfo
                    {
                        TypeId = t.TypeId,
                        PackageId = t.PackageId,
                        TemplateId = t.TemplateId,
                        TemplateName = t.TemplateName,
                        TemplateKey = t.TemplateKey,
                        TemplateSrc = t.TemplateSrc,
                        IsActive = t.IsActive,
                        Package = package,
                        Type = type
                    }).FirstOrDefault();
        }

        public SkinPackageTemplate GetLayoutInfoByPageId(int? pageId)
        {
            var entity = (from p in DataContext.Get<Page>()
                          join t in DataContext.Get<SkinPackageTemplate>() on p.TemplateId equals t.TemplateId into ptJoint
                          from template in ptJoint.DefaultIfEmpty()
                          join s in DataContext.Get<SkinPackage>() on template.PackageId equals s.PackageId into psJoint
                          from package in psJoint.DefaultIfEmpty()
                          where package.IsSelected == true && p.PageId == pageId
                          select new SkinPackageTemplate()
                          {
                              TypeId = template.TypeId,
                              PackageId = template.PackageId,
                              TemplateId = template.TemplateId,
                              TemplateName = template.TemplateName,
                              TemplateKey = template.TemplateKey,
                              TemplateSrc = template.TemplateSrc,
                              IsActive = template.IsActive
                          }).FirstOrDefault();
            return entity;
        }
        public string GetTemplateSrcByPageId(int? pageId)
        {
            string templateSrc = (from p in DataContext.Get<Page>()
                                  join t in DataContext.Get<SkinPackageTemplate>() on p.TemplateId equals t.TemplateId
                                  join s in DataContext.Get<SkinPackage>() on t.PackageId equals s.PackageId
                                  where s.IsSelected == true && p.PageId == pageId
                                  select t.TemplateSrc).FirstOrDefault();
            if (string.IsNullOrEmpty(templateSrc))
                templateSrc = LayoutType.MainLayout;
            return templateSrc;
        }

        public SelectList PopulateTemplateSelectList(int packageId, bool? status = null, string selectedValue = null, bool? isShowSelectText = true)
        {
            var lst = (from t in DataContext.Get<SkinPackageTemplate>()
                       where t.PackageId == packageId
                       select new SelectListItem
                       {
                           Text = t.TemplateName,
                           Value = t.TemplateId.ToString(),
                           Selected = (!string.IsNullOrEmpty(selectedValue) && t.TemplateId.ToString() == selectedValue)
                       }).ToList();

            if (lst.Any())
            {
                if (isShowSelectText != null && isShowSelectText == true)
                    lst.Insert(0, new SelectListItem() { Value = "", Text = $"-- {LanguageResource.SelectTemplate} --" });
            }
            else
            {
                lst.Insert(0, new SelectListItem() { Value = "", Text = $"-- {LanguageResource.None} --" });
            }

            return new SelectList(lst, "Value", "Text", selectedValue);
        }

        public SelectList PopulateTemplateSelectListBySelectedSkin(string selectedValue = null, bool isShowSelectText = true)
        {
            var list = (from t in DataContext.Get<SkinPackageTemplate>()
                        join s in DataContext.Get<SkinPackage>() on t.PackageId equals s.PackageId
                        where s.IsSelected == true
                        select new SelectListItem
                        {
                            Text = t.TemplateName,
                            Value = t.TemplateId.ToString(),
                            Selected = (!string.IsNullOrEmpty(selectedValue) && t.TemplateId.ToString() == selectedValue)
                        }).ToList();

            if (list.Any())
            {
                if (isShowSelectText)
                {
                    list.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectTemplate} ---", Value = "" });
                }
            }
            else
            {
                list.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.NoneSpecified} ---", Value = "" });
            }

            return new SelectList(list, "Value", "Text", selectedValue);
        }

        public bool HasDataExists(int packageId, string templateName)
        {
            var query = DataContext.Get<SkinPackageTemplate>().Where(c => c.PackageId == packageId && c.TemplateName.ToLower().Equals(templateName.ToLower()));
            return (query.Any());
        }
    }
}
