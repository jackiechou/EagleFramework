using Sherpa.Services.Dtos.Common;

namespace Sherpa.Services.Library
{
    public class DocumentAction : EntityAction
    {
        public static EntityAction UploadDocument(EntityActionType code = EntityActionType.UploadDocument, string id = null, string caption = null, string description = null, string url = null)
        {
            const string defaultValue = "CanUpload";

            var result = new DocumentAction()
            {
                Id = id,
                Code = code,
                Caption = caption ?? id ?? defaultValue,
                Description = description ?? caption ?? id ?? defaultValue,
                ActionUrl = "/documents"
            };

            return result;
        }

        public static EntityAction ViewDocument(EntityActionType code = EntityActionType.ViewDocument, string id = null, string caption = null, string description = null, string url = null)
        {
            const string defaultValue = "CanView";

            var result = new DocumentAction()
            {
                Id = id,
                Code = code,
                Caption = caption ?? id ?? defaultValue,
                Description = description ?? caption ?? id ?? defaultValue,
                ActionUrl = url
            };

            return result;
        }

        public static EntityAction EditDocument(EntityActionType code = EntityActionType.EditDocument, string id = null, string caption = null, string description = null, string url = null)
        {
            const string defaultValue = "CanEdit";

            var result = new DocumentAction()
            {
                Id = id,
                Code = code,
                Caption = caption ?? id ?? defaultValue,
                Description = description ?? caption ?? id ?? defaultValue,
                ActionUrl = url
            };

            return result;
        }

        public static EntityAction DeleteDocument(EntityActionType code = EntityActionType.DeleteDocument, string id = null, string caption = null, string description = null, string url = null)
        {
            const string defaultValue = "CanDelete";

            var result = new DocumentAction()
            {
                Id = id,
                Code = code,
                Caption = caption ?? id ?? defaultValue,
                Description = description ?? caption ?? id ?? defaultValue,
                ActionUrl = url
            };

            return result;
        }
    }
}