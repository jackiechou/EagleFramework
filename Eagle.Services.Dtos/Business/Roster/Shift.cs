using System;
using System.ComponentModel.DataAnnotations;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services.Dtos.Business.Personnel;

namespace Eagle.Services.Dtos.Business.Roster
{
    public class ShiftEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Location")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string Location { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SubLocation")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string SubLocation { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Comments")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string Comments { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "StartTime")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public DateTime StartTime { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EndTime")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public DateTime EndTime { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "BreakLength")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int? BreakLength { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "BreakStartTime")]
        public DateTime? BreakStartTime { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public ShiftStatus Status { get; set; }
    }
    public class ShiftEditEntry : ShiftEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ShiftId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int ShiftId { get; set; }
    }
    public class ShiftDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ShiftId")]
        public int ShiftId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Location")]
        public string Location { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SubLocation")]
        public string SubLocation { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Comments")]
        public string Comments { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "StartTime")]
        public DateTime StartTime { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EndTime")]
        public DateTime EndTime { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "BreakLength")]
        public int? BreakLength { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "BreakStartTime")]
        public DateTime? BreakStartTime { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public ShiftStatus Status { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CreatedDate")]
        public DateTime? CreatedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LastModifiedDate")]
        public DateTime? LastModifiedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ShiftPositionId")]
        public int? ShiftPositionId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EmployeeId")]
        public int? EmployeeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ShiftPosition")]
        public ShiftPositionDetail ShiftPosition { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Employee")]
        public EmployeeDetail Employee { get; set; }
    }


    public class ShiftPositionDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ShiftPositionId")]
        public int ShiftPositionId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ShiftId")]
        public int ShiftId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PositionId")]
        public int PositionId { get; set; }
    }
    public class ShiftPositionEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ShiftId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int ShiftId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PositionId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int PositionId { get; set; }
    }


    public class ShiftSwapEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "RequesterId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int RequesterId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ReceiverId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int ReceiverId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ShiftId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int ShiftId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public ShiftSwapStatus Status { get; set; }
    }
    public class ShiftSwapDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ShiftSwapId")]
        public int ShiftSwapId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "RequesterId")]
        public int RequesterId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ReceiverId")]
        public int ReceiverId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ShiftId")]
        public int ShiftId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public ShiftSwapStatus Status { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ModifiedDate")]
        public DateTime? ModifiedDate { get; set; }
    }


    public class ShiftTypeDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ShiftTypeId")]
        public int ShiftTypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ShiftTypeName")]
        public string ShiftTypeName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TimeOffTypeId")]
        public int? TimeOffTypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public bool IsActive { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ModifiedDate")]
        public DateTime? ModifiedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TimeOffType")]
        public TimeOffTypeDetail TimeOffType { get; set; }
    }
    public class ShiftTypeEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ShiftTypeName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string ShiftTypeName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TimeOffTypeId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int? TimeOffTypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public bool IsActive { get; set; }
    }
    public class ShiftTypeEditEntry : ShiftTypeEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ShiftTypeId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int ShiftTypeId { get; set; }
    }
}
