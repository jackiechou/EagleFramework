using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Entities.Services.Messaging;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.Services.Messaging
{
    public class MessageTypeRepository : RepositoryBase<MessageType>, IMessageTypeRepository
    {
        public MessageTypeRepository(IDataContext dataContext) : base(dataContext){}

        public IEnumerable<MessageType> GetMessageTypes(bool? status)
        {
            return (from x in DataContext.Get<MessageType>()
                    where status == null || x.Status == status
                    orderby x.MessageTypeId ascending
                    select x).AsEnumerable();
        }
       
        public bool HasDataExists(string messageTypeName)
        {
            var query = DataContext.Get<MessageType>().Where(c => c.MessageTypeName.Contains(messageTypeName));
            return (query.Any());
        }

        public MultiSelectList PopulateMessageTypeMultiSelectList(bool? status = null, int[] selectedValues=null, bool? isShowSelectText = false)
        {
            var query = (from t in DataContext.Get<MessageType>()
                where (status == null || t.Status == status)
                select t).ToList();

             var lst = query.Select(t=> new SelectListItem
             {
                 Text = t.MessageTypeName,
                 Value = t.MessageTypeId.ToString(),
                 Selected = selectedValues != null && selectedValues.Contains(t.MessageTypeId)
             }).OrderBy(m => m.Text).ToList();

            if (lst.Any())
            {
                if (isShowSelectText != null && isShowSelectText == true)
                    lst.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectMessageType} ---", Value = "" });
            }
            else
            {
                lst.Insert(0, new SelectListItem { Text = $"-- {LanguageResource.None} --", Value = "-1" });
            }
            return new MultiSelectList(lst, "Value", "Text", selectedValues);
        }

        public SelectList PopulateMessageTypeSelectList(int? selectedValue = null, bool? isShowSelectText = true)
        {
            var lst = (from p in DataContext.Get<MessageType>()
                       select new SelectListItem { Text = p.MessageTypeName, Value = p.MessageTypeId.ToString(), Selected = (selectedValue != null && p.MessageTypeId == selectedValue) }).ToList();

            if (lst.Any())
            {
                if (isShowSelectText != null && isShowSelectText == true)
                    lst.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectMessageType} ---", Value = "" });
            }
            else
            {
                lst.Insert(0, new SelectListItem { Text = $"-- {LanguageResource.None} --", Value = "-1" });
            }
            return new SelectList(lst, "Value", "Text", selectedValue);
        }

        public SelectList PopulateMessageTypeStatus(bool? selectedValue = null, bool? isShowSelectText = true)
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
