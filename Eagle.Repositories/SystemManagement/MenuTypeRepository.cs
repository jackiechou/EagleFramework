using System.Linq;
using System.Web.Mvc;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.SystemManagement
{
    public class MenuTypeRepository : RepositoryBase<MenuType>, IMenuTypeRepository
    {
        public MenuTypeRepository(IDataContext dataContext) : base(dataContext) { }

        public SelectList PopulateMenuTypeSelectList(bool? isActive=null, int? selectedValue=null, bool? isShowSelectText = false)
        {
            var query = from p in DataContext.Get<MenuType>() select p;
            if (isActive != null)
            {
                query = query.Where(p => p.IsActive == isActive);
            }

            int selectedVal = selectedValue ?? query.Take(1).Select(p => p.TypeId).FirstOrDefault();

            var lst = query.Select(p => new SelectListItem
                       {
                           Text = p.TypeName,
                           Value = p.TypeId.ToString(),
                           Selected = (p.TypeId == selectedVal)
            }).ToList();

            if (lst.Any())
            {
                if (isShowSelectText != null && isShowSelectText == true)
                {
                    lst.Insert(0, new SelectListItem() { Value = "-1", Text = $"-- {LanguageResource.SelectMenuType} --" });
                }
            }
            else
            {
                lst.Insert(0, new SelectListItem() { Value = "-1", Text = $"-- {LanguageResource.None} --" });
            }

            return new SelectList(lst, "Value", "Text", selectedVal);
        }
    }
}
