using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.SystemManagement
{
    public class ModuleTreeModel
    {
        public int id { get; set; }
        public int key { get; set; }
        public int? parentId { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public string text { get; set; }
        public string tooltip { get; set; }
        public bool? isParent { get; set; }
        public bool? open { get; set; }
        public List<ModuleTreeModel> children { get; set; }

        public ModuleTreeModel()
        {
            children = new List<ModuleTreeModel>();
        }
    }
    public class ModuleSearchEntry : DtoBase
    {
        public ModuleSearchEntry()
        {
            SearchModuleType = ModuleType.Admin;
        }

        [Display(ResourceType = typeof(LanguageResource), Name = "SearchModuleType")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public ModuleType SearchModuleType { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SearchText")]
        public string SearchText { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public ModuleStatus? Status { get; set; }
    }
    public class ModuleDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ApplicationId")]
        public Guid ApplicationId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ModuleId")]
        public int ModuleId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ModuleCode")]
        public string ModuleCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ModuleTypeId")]
        public ModuleType ModuleTypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ModuleTitle")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string ModuleTitle { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ModuleName")]
        public string ModuleName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "InheritViewPermissions")]
        public bool? InheritViewPermissions { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "AllPages")]
        public bool? AllPages { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsSecured")]
        public bool? IsSecured { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        [EnumDataType(typeof(ModuleStatus))]
        public ModuleStatus? IsActive { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Header")]
        public string Header { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Footer")]
        public string Footer { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "StartDate")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "DateValid")]
        public DateTime? StartDate { get; set; }

        // jQuery.validator.addMethod('isdateafter', function (value, element, params) { if (!/Invalid|NaN/.test(new Date(value))) { return new Date(value) > new Date(); } return isNaN(value) && isNaN($(params).val()) || (parseFloat(value) > parseFloat($(params).val())); }, ''); jQuery.validator.unobtrusive.adapters.add('isdateafter', {}, function (options) { options.rules['isdateafter'] = true; options.messages['isdateafter'] = options.message; });

        [Display(ResourceType = typeof(LanguageResource), Name = "EndDate")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "DateValid")]
        //[DateGreaterThan("StartDate", true, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "StartDateMustBeBeforeEndDate")]
        //[GreaterThan("StartDate", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "StartDateMustBeBeforeEndDate")]
        public DateTime? EndDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ListOrder")]
        public int? ViewOrder { get; set; }

        //[Display(ResourceType = typeof(LanguageResource), Name = "Visibility")]
        //public bool? Visibility { get; set; }

        //Modified --------------------------------------------------------------------------

        //public List<PageModule> PageModules { get; set; }
        //public List<RolePermission> RolePermissionList { get; set; }

        //Modified --------------------------------------------------------------------------

        //[Display(ResourceType = typeof(LanguageResource), Name = "PageId")]
        //public int PageId { get; set; }

        //[Display(ResourceType = typeof(LanguageResource), Name = "Pane")]
        //public string Pane { get; set; }

        //[Display(ResourceType = typeof(LanguageResource), Name = "Alignment")]
        //public string Alignment { get; set; }

        //[Display(ResourceType = typeof(LanguageResource), Name = "Color")]
        //public string Color { get; set; }

        //[Display(ResourceType = typeof(LanguageResource), Name = "Border")]
        //[Range(typeof(int), "0", "10", ErrorMessage = "{0} can only be between {1} and {2}")]
        //public string Border { get; set; }

        //[Display(ResourceType = typeof(LanguageResource), Name = "InsertedPosition")]
        //public string InsertedPosition { get; set; }

        //[Display(ResourceType = typeof(LanguageResource), Name = "ReferencedModuleId")]
        //public int? ReferencedModuleId { get; set; }

        //[Display(ResourceType = typeof(LanguageResource), Name = "IconFile")]
        //public int IconFile { get; set; }

        //[Display(ResourceType = typeof(LanguageResource), Name = "IconClass")]
        //public string IconClass { get; set; }



        //[Display(ResourceType = typeof(LanguageResource), Name = "IsVisible")]
        //public bool? IsVisible { get; set; }
    }
    public class ModuleEntry : DtoBase, IValidatableObject
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ModuleType")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int ModuleTypeId { get; set; }

        //[ConcurrencyCheck]
        [Display(ResourceType = typeof(LanguageResource), Name = "ModuleTitle")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(256, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        [RegularExpression(@"[a-zA-Z0-9_ÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ\\s \\\-~!@#$%^&*()_+={}:|""?`;:><',./[\]]+", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "InvalidTitle")]
        public string ModuleTitle { get; set; }

        //[RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]
        [Display(ResourceType = typeof(LanguageResource), Name = "ModuleName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(256, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string ModuleName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ModuleCode")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 2)]
        [RegularExpression(@"^[-_ a-zA-Z0-9]+$", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "InvalidCode")]
        public string ModuleCode { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "InheritViewPermissions")]
        public bool? InheritViewPermissions { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "AllPages")]
        public bool? AllPages { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsSecured")]
        public bool? IsSecured { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Header")]
        //[StringLength(int.MaxValue, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Header { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Footer")]
        //[StringLength(int.MaxValue, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Footer { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "StartDate")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "DateValid")]
        public DateTime? StartDate { get; set; }

        // jQuery.validator.addMethod('isdateafter', function (value, element, params) { if (!/Invalid|NaN/.test(new Date(value))) { return new Date(value) > new Date(); } return isNaN(value) && isNaN($(params).val()) || (parseFloat(value) > parseFloat($(params).val())); }, ''); jQuery.validator.unobtrusive.adapters.add('isdateafter', {}, function (options) { options.rules['isdateafter'] = true; options.messages['isdateafter'] = options.message; });

        [Display(ResourceType = typeof(LanguageResource), Name = "EndDate")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "DateValid")]
        //[DateGreaterThan("StartDate", true, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "StartDateMustBeBeforeEndDate")]
        // [GreaterThan("StartDate", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "StartDateMustBeBeforeEndDate")]
        public DateTime? EndDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> res = new List<ValidationResult>();
            if (EndDate < StartDate)
            {
                //yield return new ValidationResult(LanguageResource.ValidateCompareStartDateEndDate, new[] { "EndDate" });
                res.Add(new ValidationResult(LanguageResource.ValidateCompareStartDateEndDate, new[] { "EndDate" }));
            }
            return res;
        }

        //Modified --------------------------------------------------------------------------
        public List<int> SelectedPages { get; set; }
        public List<CapabilityEntry> ModuleCapabilities { get; set; }
    }
    public class ModuleEditEntry : DtoBase, IValidatableObject
    {
        public ModuleEditEntry()
        {
            AllPages = AllPages ?? false;
            InheritViewPermissions = InheritViewPermissions ?? false;
            IsSecured = IsSecured ?? false;
        }

        [Display(ResourceType = typeof(LanguageResource), Name = "ModuleId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int ModuleId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ModuleType")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int ModuleTypeId { get; set; }

        //[ConcurrencyCheck]
        [Display(ResourceType = typeof(LanguageResource), Name = "ModuleTitle")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(256, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        [RegularExpression(@"[a-zA-Z0-9_ÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ\\s \\\-~!@#$%^&*()_+={}:|""?`;:><',./[\]]+", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "InvalidTitle")]
        public string ModuleTitle { get; set; }

        //[RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]
        [Display(ResourceType = typeof(LanguageResource), Name = "ModuleName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(256, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string ModuleName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ModuleCode")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 2)]
        [RegularExpression(@"^[-_ a-zA-Z0-9]+$", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "InvalidCode")]
        public string ModuleCode { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "InheritViewPermissions")]
        public bool? InheritViewPermissions { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "AllPages")]
        public bool? AllPages { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsSecured")]
        public bool? IsSecured { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Header")]
        //[StringLength(int.MaxValue, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Header { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Footer")]
        //[StringLength(int.MaxValue, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Footer { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "StartDate")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "DateValid")]
        public DateTime? StartDate { get; set; }

        // jQuery.validator.addMethod('isdateafter', function (value, element, params) { if (!/Invalid|NaN/.test(new Date(value))) { return new Date(value) > new Date(); } return isNaN(value) && isNaN($(params).val()) || (parseFloat(value) > parseFloat($(params).val())); }, ''); jQuery.validator.unobtrusive.adapters.add('isdateafter', {}, function (options) { options.rules['isdateafter'] = true; options.messages['isdateafter'] = options.message; });

        [Display(ResourceType = typeof(LanguageResource), Name = "EndDate")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "DateValid")]
        //[DateGreaterThan("StartDate", true, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "StartDateMustBeBeforeEndDate")]
        // [GreaterThan("StartDate", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "StartDateMustBeBeforeEndDate")]
        public DateTime? EndDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> res = new List<ValidationResult>();
            if (EndDate < StartDate)
            {
                //yield return new ValidationResult(LanguageResource.ValidateCompareStartDateEndDate, new[] { "EndDate" });
                res.Add(new ValidationResult(LanguageResource.ValidateCompareStartDateEndDate, new[] { "EndDate" }));
            }
            return res;
        }

        //Modified --------------------------------------------------------------------------
        public List<int> SelectedPages { get; set; }
        public List<ModuleCapabilityEditEntry> ExistedModuleCapabilities { get; set; }
        public List<CapabilityEntry> ModuleCapabilities { get; set; }
    }
}
