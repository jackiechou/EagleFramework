using System;
using System.ComponentModel.DataAnnotations;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.SystemManagement
{
    public class LanguageDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "LanguageId")]
        public int LanguageId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LanguageCode")]
        public string LanguageCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LanguageName")]
        public string LanguageName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public LanguageStatus Status { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ModifiedDate")]
        public DateTime? ModifiedDate { get; set; }
    }

    public class LanguageSearchEntry : DtoBase
    {

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public ApplicationLanguageStatus? Status { get; set; }
    }
}
