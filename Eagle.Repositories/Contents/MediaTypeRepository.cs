using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Common;
using Eagle.Core.Settings;
using Eagle.Entities.Contents.Media;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.Contents
{
    public class MediaTypeRepository : RepositoryBase<MediaType>, IMediaTypeRepository
    {
        public MediaTypeRepository(IDataContext dataContext) : base(dataContext) { }

        public IEnumerable<MediaType> GetList(MediaTypeStatus? status)
        {
            return DataContext.Get<MediaType>().Where(x => status==null || x.Status == status).AsEnumerable();
        }

        public IEnumerable<MediaType> GetMediaTypes(MediaTypeStatus? status, ref int? recordCount, int? page = null, int? pageSize = null)
        {
            var queryable = DataContext.Get<MediaType>().Where(x => status == null || x.Status == status);
            if (recordCount != null)
            {
                recordCount = queryable.Count();
            }

            queryable = queryable.OrderBy(m => m.TypeName);

            if (page != null && pageSize != null)
            {
                queryable = queryable.ApplyPaging(page.Value, pageSize.Value);
            }

            return queryable.AsEnumerable();
        }
        public int GetNewListOrder()
        {
            int listOrder = 1;
            var query = from u in DataContext.Get<MediaType>() select (int)u.ListOrder;
            if (query.Any())
            {
                listOrder = query.Max() + 1;
            }
            return listOrder;
        }
        public bool HasDataExisted(string typeName)
        {
            var query = DataContext.Get<MediaType>().FirstOrDefault(p => p.TypeName.ToLower().Contains(typeName.ToLower()));
            return (query != null);
        }

        public SelectList PopulateMediaTypeSelectList(int? selectedValue=null, bool? isShowSelectText = null, MediaTypeStatus? status = null)
        {
            var listItems = new List<SelectListItem>();
            var lst = GetList(status).ToList();
            if (lst.Any())
            {
                listItems = lst.Select(p => new SelectListItem { Text = p.TypeName, Value = p.TypeId.ToString() }).ToList();
                if (isShowSelectText != null && isShowSelectText == true)
                    listItems.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectMediaType} ---", Value = "" });
            }
            else
            {
                listItems.Insert(0, new SelectListItem { Text = $"-- {LanguageResource.None} --", Value = "-1" });
            }
            return new SelectList(listItems, "Value", "Text", selectedValue);
        }
    }
}
