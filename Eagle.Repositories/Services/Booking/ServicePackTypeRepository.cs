using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Entities.Services.Booking;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.Services.Booking
{
    public class ServicePackTypeRepository : RepositoryBase<ServicePackType>, IServicePackTypeRepository
    {
        public ServicePackTypeRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<ServicePackType> GetServicePackTypes(ServicePackTypeStatus? status)
        {
            return (from o in DataContext.Get<ServicePackType>()
                    where o.IsActive == status
                    select o).AsEnumerable();
        }

        public bool HasDataExisted(string typeName)
        {
            var query = (from o in DataContext.Get<ServicePackType>()
                         where o.TypeName.ToLower() == typeName.ToLower()
                         select o).FirstOrDefault();
            return query != null;
        }

        public SelectList PopulateServicePackTypeSelectList(ServicePackTypeStatus? status = null, int? selectedValue = null,
            bool? isShowSelectText = true)
        {
            var listItems = new List<SelectListItem>();
            var lst = (from c in DataContext.Get<ServicePackType>()
                where status == null || c.IsActive == status
                select c).ToList();

            if (lst.Any())
            {
                listItems = lst.Select(p => new SelectListItem
                {
                    Text = p.TypeName,
                    Value = p.TypeId.ToString(),
                    Selected = (selectedValue != null && p.TypeId == selectedValue)
                }).ToList();

                if (isShowSelectText != null && isShowSelectText == true)
                {
                    listItems.Insert(0, new SelectListItem { Text = $@"--- {LanguageResource.SelectServicePackType} ---", Value = "" });
                }
            }
            else
            {
                listItems.Insert(0, new SelectListItem { Text = $@"-- {LanguageResource.None} --", Value = "-1" });
            }
            return new SelectList(listItems, "Value", "Text", selectedValue);
        }
    }
}
