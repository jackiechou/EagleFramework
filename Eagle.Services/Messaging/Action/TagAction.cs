using Eagle.Services.Common;
using Eagle.Services.Dtos.Contents.Tags;

namespace Eagle.Services.Messaging.Action
{
    public class TagAction : EntityActionBase<TagDetail>
    {
        //public static EntityAction EditTag(string id = null, string type = null, string caption = null, string description = null, string url = null)
        //{
        //    const string defaultValue = "EditTagEntityAction";

        //    var result = new TagAction
        //    {
        //        Code = EntityActionType.EditTag,
        //        Caption = defaultValue,
        //        Description = "Edit tag",
        //        ActionUrl = GetFullUrl($"tags/{id}/tagtypes/{type}")
        //    };

        //    return result;
        //}

        //public static EntityAction ActivateTag(string id = null, string caption = null, string description = null, string url = null)
        //{
        //    const string defaultValue = "ChangeTagStatusEntityAction";

        //    var result = new TagAction
        //    {
        //        Code = EntityActionType.UpdateState,
        //        Caption = defaultValue,
        //        Description = "Activate tag",
        //        ActionUrl = GetFullUrl(string.Format("tags/changestatus"))
        //    };

        //    return result;
        //}

        //public static EntityAction DeactivateTag(string id = null, string caption = null, string description = null, string url = null)
        //{
        //    const string defaultValue = "ChangeTagStatusEntityAction";

        //    var result = new TagAction
        //    {
        //        Code = EntityActionType.UpdateState,
        //        Caption = defaultValue,
        //        Description = "Deactivate tag",
        //        ActionUrl = GetFullUrl(string.Format("tags/{0}/changestatus", id))
        //    };

        //    return result;
        //}

        //public static EntityAction DeleteTag(string id = null, string caption = null, string description = null, string url = null)
        //{
        //    const string defaultValue = "DeleteTagEntityAction";

        //    var result = new TagAction
        //    {
        //        Code = EntityActionType.DeleteTag,
        //        Caption = defaultValue,
        //        Description = "Delete tag",
        //        ActionUrl = GetFullUrl(string.Format("tags/{0}", id))
        //    };

        //    return result;
        //}

        //public static EntityAction AttachTag(string id = null, string domainType = null, string domainKey = null, string caption = null, string description = null, string url = null)
        //{
        //    const string defaultValue = "AttachTagEntityAction";

        //    var result = new TagAction
        //    {
        //        Code = EntityActionType.AttachTag,
        //        Caption = defaultValue,
        //        Description = "Attach tag",
        //        ActionUrl = GetFullUrl(string.Format("tags/{0}/domaintypes/{1}/domainkeys/{2}/isattached/{3}", id, domainType, domainKey, true))
        //    };

        //    return result;
        //}

        //public static EntityAction DetachTag(string id = null, string domainType = null, string domainKey = null, string caption = null, string description = null, string url = null)
        //{
        //    const string defaultValue = "DetachTagEntityAction";

        //    var result = new TagAction
        //    {
        //        Code = EntityActionType.DetachTag,
        //        Caption = defaultValue,
        //        Description = "Detach tag",
        //        ActionUrl = GetFullUrl(string.Format("tags/{0}/domaintypes/{1}/domainkeys/{2}/isattached/{3}", id, domainType, domainKey, false))
        //    };

        //    return result;
        //}
    }
}
