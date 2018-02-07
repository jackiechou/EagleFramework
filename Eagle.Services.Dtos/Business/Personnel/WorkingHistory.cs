using System;
using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Business.Personnel
{
    public class WorkingHistoryDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "WorkingHistoryId")]
        public int WorkingHistoryId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EmployeeId")]
        public int EmployeeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "JoinedDate")]
        public DateTime? JoinedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FromDate")]
        public DateTime? FromDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ToDate")]
        public DateTime? ToDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ManagerId")]
        public int? ManagerId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PositionId")]
        public int PositionId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Duty")]
        public string Duty { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Note")]
        public string Note { get; set; }

    }
    public class WorkingHistoryEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "EmployeeId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int EmployeeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "JoinedDate")]
        public DateTime? JoinedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FromDate")]
        public DateTime? FromDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ToDate")]
        public DateTime? ToDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ManagerId")]
        public int? ManagerId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PositionId")]
        public int PositionId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Duty")]
        public string Duty { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Note")]
        public string Note { get; set; }
    }
    public class WorkingHistoryEditEntry : WorkingHistoryEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "WorkingHistoryId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int WorkingHistoryId { get; set; }
    }
}
