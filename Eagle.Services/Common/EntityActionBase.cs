using System.Configuration;
using Eagle.Services.Dtos.Common;

namespace Eagle.Services.Common
{
    public class EntityActionBase<TDto> : EntityAction where TDto : class
    {
        internal static readonly string BaseUrl = ConfigurationManager.AppSettings["WebApiBaseUrl"];

        protected EntityActionBase()
        {
            
        }

        public static EntityAction Add(string id = null, string caption = null, string description = null, string url = null)
        {
            const string defaultValue = "Add";

            var result = new EntityActionBase<TDto>
            {
                Code = EntityActionType.Add,
                Caption = defaultValue,
                Description = "POST",
                ActionUrl = $"{BaseUrl}{url}"
            };

            return result;
        }

        public static EntityAction Edit(string id = null, string caption = null, string description = null, string url = null)
        {
            const string defaultValue = "Edit";

            var result = new EntityActionBase<TDto>
            {
                Code = EntityActionType.Edit,
                Caption = defaultValue,
                Description = "PUT",
                ActionUrl = $"{BaseUrl}{url}/{id}"
            };

            return result;
        }

        public static EntityAction Delete(string id = null, string caption = null, string description = null, string url = null)
        {
            const string defaultValue = "DeleteUserContainer";

            var result = new EntityActionBase<TDto>
            {
                Code = EntityActionType.Delete,
                Caption = defaultValue,
                Description = "DELETE",
                ActionUrl = $"{BaseUrl}{url}/{id}"
            };

            return result;
        }

        public static EntityAction UpdateStatus(int id, int status, string caption = null, string description = null, string url = null)
        {
            const string defaultValue = "UpdateStatus";

            var result = new EntityActionBase<TDto>
            {
                Code = EntityActionType.UpdateStatus,
                Caption = caption ?? defaultValue,
                Description = "PUT",
                ActionUrl = $"{BaseUrl}{url}/{id}/updatestatus/{status}"
            };

            return result;
        }
    }
}
