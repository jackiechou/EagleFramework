using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web.Mvc;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.SystemManagement
{
    public class MenuPositionRepository : RepositoryBase<MenuPosition>, IMenuPositionRepository
    {
        public MenuPositionRepository(IDataContext dataContext) : base(dataContext) { }
        public MultiSelectList PopulateMenuPositionMultiSelectList(int typeId, bool? isActive=null, string positionId = null, string[] selectedValues = null)
        {
            var query = from p in DataContext.Get<MenuPosition>()
                        where p.TypeId == typeId
                        select p;

            if (isActive != null)
            {
                query = query.Where(p => p.IsActive == isActive);
            }

            var lst = query.Select(p => new SelectListItem{
                           Text = p.PositionName,
                           Value = p.PositionId.ToString()
                       }).ToList();

            if (!lst.Any())
                lst.Insert(0, new SelectListItem() { Value = "-1", Text = $"-- {LanguageResource.None} --" });
            else
            {
                if (!string.IsNullOrEmpty(positionId))
                {
                    var selectedPositions = positionId.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).OrderBy(x=>x).Select(p => p.Trim()).ToArray();
                    if (selectedPositions.Any())
                    {
                        var fullPositions = (from p in lst orderby p.Value ascending select p.Value).ToArray();
                        var output = fullPositions.Except(selectedPositions).ToList();
                        lst = lst.Where(i => output.Contains(i.Value)).ToList();
                    }
                }
            }

            return new MultiSelectList(lst, "Value", "Text", selectedValues);
        }

        public MultiSelectList PopulateMenuPositionMultiSelectedList(string positionId, bool? isActive = null)
        {
            var lst = new List<SelectListItem>();
            if (string.IsNullOrEmpty(positionId)) return new MultiSelectList(lst, "Value", "Text");
           
            List<int> selectedLst = positionId.Split(',').Select(int.Parse).ToList();
            if (selectedLst.Any())
            {
                foreach (var id in selectedLst)
                {
                    var query = from p in DataContext.Get<MenuPosition>() where p.PositionId == id select p;
                    if (isActive != null)
                    {
                        query = query.Where(p => p.IsActive == isActive);
                    }

                    var item = query.Select(p => new SelectListItem
                    {
                        Text = p.PositionName,
                        Value = p.PositionId.ToString(),
                        Selected = true
                    }).FirstOrDefault();

                    lst.Add(item);
                }
            }
            return new MultiSelectList(lst, "Value", "Text");
        }

        public MultiSelectList PopulateMenuPositionMultiSelectedListByMenuId(int? menuId, bool? isActive = null)
        {
            var lst = new List<SelectListItem>();
            if (menuId == null) return new MultiSelectList(lst, "Value", "Text");
            
            string positionId = (from m in DataContext.Get<Menu>() where m.MenuId == menuId select m.PositionId).FirstOrDefault();
            if (string.IsNullOrEmpty(positionId)) return new MultiSelectList(lst, "Value", "Text");

            List<int> selectedLst = positionId.Split(',').Select(int.Parse).ToList();
            if (selectedLst.Any())
            {
                foreach (var id in selectedLst)
                {
                    var query = from p in DataContext.Get<MenuPosition>() where p.PositionId == id select p;
                    if (isActive != null)
                    {
                        query = query.Where(p => p.IsActive == isActive);
                    }

                    var item = query.Select(p => new SelectListItem
                    {
                        Text = p.PositionName,
                        Value = p.PositionId.ToString(),
                        Selected = true
                    }).FirstOrDefault();

                    lst.Add(item);
                }
            }
            return new MultiSelectList(lst, "Value", "Text");
        }
    }
}
