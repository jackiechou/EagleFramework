using System;
using System.ComponentModel.DataAnnotations;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services.Dtos.Business.Personnel;

namespace Eagle.Services.Dtos.Business.Roster
{
    public class TimesheetDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TimesheetId")]
        public int TimesheetId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EmployeeId")]
        public int EmployeeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ShiftId")]
        public int? ShiftId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ShiftTypeId")]
        public int ShiftTypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EmployeeTimeOffId")]
        public int? EmployeeTimeOffId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "StartTime")]
        public DateTime StartTime { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EndTime")]
        public DateTime EndTime { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "BreakStartTime")]
        public DateTime? BreakStartTime { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "BreakLength")]
        public int? BreakLength { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Location")]
        public string Location { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SubLocation")]
        public string SubLocation { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Comments")]
        public string Comments { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public TimesheetStatus Status { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LastModifiedDate")]
        public DateTime? LastModifiedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Employee")]
        public EmployeeDetail Employee { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EmployeeTimeOff")]
        public EmployeeTimeOffDetail EmployeeTimeOff { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Shift")]
        public ShiftDetail Shift { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ShiftType")]
        public ShiftTypeDetail ShiftType { get; set; }
    }
    public class TimesheetEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "EmployeeId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int EmployeeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ShiftId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int? ShiftId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ShiftTypeId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int ShiftTypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EmployeeTimeOffId")]
        public int? EmployeeTimeOffId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "StartTime")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public DateTime StartTime { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EndTime")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public DateTime EndTime { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "BreakStartTime")]
        public DateTime? BreakStartTime { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "BreakLength")]
        public int? BreakLength { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Location")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string Location { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SubLocation")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string SubLocation { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Comments")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string Comments { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public TimesheetStatus Status { get; set; }
    }
    public class TimesheetEditEntry : TimesheetEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TimesheetId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int TimesheetId { get; set; }
    }
}
