using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Eagle.Core.Permission;
using Eagle.Resources;

namespace Eagle.Services.Dtos.SystemManagement.Identity
{
    #region @Html.DropDownListFor(model=>model.ActionId, Model.ActionsList) ======================
    public class ActionModel
    {
        public ActionModel()
        {
            ActionsList = new List<SelectListItem>();
        }
        [Display(ResourceType = typeof (LanguageResource), Name = "Actions")]
        public int ActionId { get; set; }
        public IEnumerable<SelectListItem> ActionsList { get; set; }
    }
    #endregion ===================================================================================


    #region @Html.EnumDropDownListFor(model => model.ActionId,Model.ActionTypeList) ==============
    public class ActionTypeModel
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Actions")]
        public int ActionId { get; set; }
        public PermissionLevel PermissionDataLevel { get; set; }
    }
    #endregion ==================================================================================
}
