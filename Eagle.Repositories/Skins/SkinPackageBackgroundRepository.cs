using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Common;
using Eagle.Entities.Skins;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.Skins
{
    public class SkinPackageBackgroundRepository : RepositoryBase<SkinPackageBackground>, ISkinPackageBackgroundRepository
    {
        public SkinPackageBackgroundRepository(IDataContext dataContext) : base(dataContext) { }

        public IEnumerable<SkinPackageBackground> GetSkinPackageBackgrounds(int? packageId, bool? status, out int recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var result = from s in DataContext.Get<SkinPackageBackground>()
                        where (packageId == null || s.PackageId == packageId) && (status==null || s.IsActive == status)
                        select s;

            return result.OrderBy(s => s.BackgroundId)
                .WithRecordCount(out recordCount)
                .WithSortingAndPaging(orderBy, page, pageSize);
        }
        public IEnumerable<SkinPackageBackground> GetActiveListByQty(int skinPackageId, int? recordCount)
        {
            var query = (from s in DataContext.Get<SkinPackageBackground>()
                          where s.PackageId == skinPackageId && s.IsActive == true
                         select s);
            int countItem = query.Count();

            return (recordCount != null && recordCount < countItem) ? query.Take(countItem).AsEnumerable() : query.AsEnumerable();
        }

        //Expression<Func<SkinPackageBackground, bool>> FilterByQuantity(int num)
        //{
        //    return x => x.SkinPackageId == skinPackageId;
        //}

        public IEnumerable<SkinPackageBackground> GetListBySkinPackageIdWithQty(int qty, int skinPackageId)
        {
            return DataContext.Get<SkinPackageBackground>().Where(x=>x.IsActive==true).OrderByDescending(x=>x.ListOrder).Take(qty);
        }

        public SkinPackageBackgroundInfo GetDetail(int backgroundId)
        {
            return (from b in DataContext.Get<SkinPackageBackground>()
                    join p in DataContext.Get<SkinPackage>() on b.PackageId equals p.PackageId into bpJoin
                    from package in bpJoin.DefaultIfEmpty()
                    where b.BackgroundId == backgroundId
                    select new SkinPackageBackgroundInfo
                    {
                        TypeId = b.TypeId,
                        PackageId = b.PackageId,
                        BackgroundId = b.BackgroundId,
                        BackgroundFile = b.BackgroundFile,
                        BackgroundLink = b.BackgroundLink,
                        IsExternalLink = b.IsExternalLink,
                        IsActive = b.IsActive,
                        Package = package
                    }).FirstOrDefault();
        }

        public int GetNewListOrder()
        {
            int listOrder = 1;
            var query = from u in DataContext.Get<SkinPackageBackground>() select (int)u.ListOrder;
            if (query.Any())
            {
                listOrder = query.Max() + 1;
            }
            return listOrder;
        }

        public SkinPackageBackground GetCurrentBackground(int currentIdx)
        {
            return DataContext.Get<SkinPackageBackground>().OrderByDescending(x => x.BackgroundId).FirstOrDefault(x => x.BackgroundId == currentIdx);
        }
        public SkinPackageBackground GetPreviousBackground(int currentIdx)
        {
            return DataContext.Get<SkinPackageBackground>().OrderByDescending(x => x.BackgroundId).FirstOrDefault(x => x.BackgroundId < currentIdx);
        }
        public SkinPackageBackground GetNextBackground(int currentIdx)
        {
            return DataContext.Get<SkinPackageBackground>().OrderByDescending(x => x.BackgroundId).FirstOrDefault(x => x.BackgroundId > currentIdx);
        }

        public bool HasDataExists(int packageId, string backgroundName)
        {
            var query = DataContext.Get<SkinPackageBackground>().Where(c => c.PackageId == packageId && c.BackgroundName.ToLower().Equals(backgroundName.ToLower()));
            return (query.Any());
        }

        public SelectList PopulateSkinPackageBackgroundStatus(bool? selectedValue = null, bool? isShowSelectText = true)
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
