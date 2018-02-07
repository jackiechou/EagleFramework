using System.Linq;
using System.Web.Mvc;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.SystemManagement
{
    public class ContentTypeRepository : RepositoryBase<ContentType>, IContentTypeRepository
    {
        public ContentTypeRepository(IDataContext dataContext) : base(dataContext) { }
        public SelectList PopulateContentTypesToDropDownList(string selectedValue, bool isShowSelectText = false)
        {
            var lst = (from p in DataContext.Get<ContentType>().AsEnumerable()
                   select new SelectListItem
                   {
                       Text = p.ContentTypeName,
                       Value = p.ContentTypeId.ToString()
                   }).ToList();

            if (lst.Count == 0)
                lst.Insert(0, new SelectListItem() { Value = "-1", Text = $"-- {LanguageResource.None} --"});

            if (isShowSelectText)
                lst.Insert(0, new SelectListItem() { Value = "-1", Text = $"-- {LanguageResource.SelectContentType} --"});

            SelectList selectlist = new SelectList(lst, "Value", "Text", selectedValue);
            return selectlist;
        }
        public bool HasDataExisted(string contentTypeName)
        {
            var query = DataContext.Get<ContentType>().FirstOrDefault(c => c.ContentTypeName.ToLower().Contains(contentTypeName.ToLower()));
            return (query != null);
        }

    }
}
