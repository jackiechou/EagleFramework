using System;
using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Business.Personnel
{
    public class SkillDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "SkillId")]
        public int SkillId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SkillName")]
        public string SkillName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public bool IsActive { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ModifiedDate")]
        public DateTime? ModifiedDate { get; set; }
    }
    public class SkillEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "SkillName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string SkillName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        [StringLength(500, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public bool IsActive { get; set; }
    }
    public class SkillEditEntry : SkillEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "SkillId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int SkillId { get; set; }
    }
}
