using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Entities.Skins;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.Skins
{
    public class SkinPackageTypeRepository : RepositoryBase<SkinPackageType>, ISkinPackageTypeRepository
    {
        public SkinPackageTypeRepository(IDataContext dataContext) : base(dataContext) { }

        public IEnumerable<SkinPackageType> GetSkinPackageTypes(bool? status)
        {
            return (from x in DataContext.Get<SkinPackageType>()
                    where status == null || x.IsActive == status
                    orderby x.TypeId ascending
                    select x).AsEnumerable();
        }

        public bool HasDataExists(string typeName)
        {
            var query = DataContext.Get<SkinPackageType>().Where(c => c.TypeName.Contains(typeName));
            return (query.Any());
        }

        public SelectList PopulateSkinPackageTypeSelectList(int? selectedValue = null, bool? isShowSelectText = true)
        {
            var lst = (from p in DataContext.Get<SkinPackageType>()
                       select new SelectListItem {
                           Text = p.TypeName,
                           Value = p.TypeId.ToString(),
                           Selected = (selectedValue != null && p.TypeId == selectedValue)
                       }).ToList();

            if (lst.Any())
            {
                if (isShowSelectText != null && isShowSelectText == true)
                    lst.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectSkinPackageType} ---", Value = "" });
            }
            else
            {
                lst.Insert(0, new SelectListItem { Text = $"-- {LanguageResource.None} --", Value = "-1" });
            }
            return new SelectList(lst, "Value", "Text", selectedValue);
        }

        public SelectList PopulateSkinPackageTypeStatus(bool? selectedValue = null, bool? isShowSelectText = true)
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
