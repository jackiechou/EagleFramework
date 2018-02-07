using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Eagle.Common.Services.Parse;
using Eagle.Core.Common;
using Eagle.Entities.Services.Messaging;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.Services.Messaging
{ 
    public class MessageTemplateRepository : RepositoryBase<MessageTemplate>, IMessageTemplateRepository
    {
        public MessageTemplateRepository(IDataContext dataContext) : base(dataContext) { }

        public IEnumerable<MessageTemplateInfo> GetMessageTemplates(string searchText, int? messageTypeId, bool? status, out int recordCount, string orderBy=null, int? page = null, int? pageSize = null)
        {
            var queryable = from x in DataContext.Get<MessageTemplate>()
                            join y in DataContext.Get<MessageType>() on x.MessageTypeId equals y.MessageTypeId into xycJoin
                            from type in xycJoin.DefaultIfEmpty()
                            where (status==null || x.Status == status)
                            select new MessageTemplateInfo
                            {
                                TemplateId = x.TemplateId,
                                MessageTypeId = x.MessageTypeId,
                                TemplateName = x.TemplateName,
                                TemplateSubject = x.TemplateSubject,
                                TemplateBody = x.TemplateBody,
                                Status = x.Status,
                                MessageType = type
                            };

            if (messageTypeId != null && messageTypeId > 0)
            {
                queryable = queryable.Where(x => x.MessageTypeId == messageTypeId);
            }

            if (!string.IsNullOrEmpty(searchText))
            {
                queryable = queryable.Where(x => x.TemplateName.ToString().Contains(searchText));
            }
            return queryable.WithRecordCount(out recordCount).WithSortingAndPaging(orderBy, page, pageSize); ;
        }

        public IEnumerable<MessageTemplate> GetMessageTemplatesByNotificationTypeId(int notificationTypeId)
        {
            return (from x in DataContext.Get<MessageTemplate>()
                         where x.NotificationTypeId == notificationTypeId
                         select x).AsEnumerable();
        }

        public MessageTemplate GetMessageTemplateDetail(int notificationTypeId, int messageTypeId)
        {
            return (from x in DataContext.Get<MessageTemplate>()
                    where x.MessageTypeId == messageTypeId && x.NotificationTypeId == notificationTypeId
                    select x).FirstOrDefault();
        }
       
        public MessageTemplate FindByName(string typeName)
        {
            return DataContext.Get<MessageTemplate>().FirstOrDefault(x => x.TemplateName.Contains(typeName.ToLower()));
        }
        public MessageTemplate GetDetails(int templateId)
        {
            var entity = (from x in DataContext.Get<MessageTemplate>()
                          join y in DataContext.Get<MessageType>() on x.MessageTypeId equals y.MessageTypeId
                          where x.TemplateId == templateId
                          select x).FirstOrDefault();
            if (entity != null)
            {
                entity.TemplateBody = HttpUtility.HtmlDecode(entity.TemplateBody);
            }
            return entity;
        }
        public string GetTemplateContent(int id)
        {
            string result = string.Empty;
            var entity = FindById(id);
            if (entity != null)
            {

                result = HttpUtility.HtmlDecode(entity.TemplateBody);
            }
            return result;
        }
        
        public bool HasDataExists(string templateName)
        {
            var query = DataContext.Get<MessageTemplate>().Where(c => c.TemplateName.ToLower().Equals(templateName.ToLower()));
            return (query.Any());
        }

        public SelectList PopulateMessageTemplateStatus(bool? selectedValue = null, bool? isShowSelectText = true)
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

        #region PARSE PARAMETER AND OUTPUT CONTENT =====================================================

        /// <summary>
        /// Parse Template By TemplateId
        /// Hashtable templateVariables - Hashtable templateVars = new Hashtable(); - templateVars.Add("DAY", DAY);  
        /// </summary>
        /// <param name="templateVariables"></param>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public string ParseTemplateByTemplateId(Hashtable templateVariables, int templateId)
        {
            string htmlContents = GetTemplateContent(templateId);
            string bodycontent = ParseTemplateHandler.ParseTemplate(templateVariables, htmlContents);
            return bodycontent;
        }
        #endregion ==================================================================================================================================================

    }
}
