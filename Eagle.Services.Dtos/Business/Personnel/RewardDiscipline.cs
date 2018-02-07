using System;
using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Business.Personnel
{
    public class RewardDisciplineDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "RewardDisciplineId")]
        public int RewardDisciplineId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EmployeeId")]
        public int EmployeeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SignedDate")]
        public DateTime? SignedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EffectiveDate")]
        public DateTime EffectiveDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsReward")]
        public bool IsReward { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Reason")]
        public string Reason { get; set; }
    }
    public class RewardDisciplineEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "EmployeeId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int EmployeeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SignedDate")]
        public DateTime? SignedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EffectiveDate")]
        public DateTime EffectiveDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsReward")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public bool IsReward { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Reason")]
        [StringLength(500, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string Reason { get; set; }
    }
    public class RewardDisciplineEditEntry : RewardDisciplineEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "RewardDisciplineId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int RewardDisciplineId { get; set; }
    }

}
