using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Business
{
    public class BrandEntry: BaseDto
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "BrandName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string BrandName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "BrandAlias")]
        public string BrandAlias { get; set; }
        [Display(ResourceType = typeof(LanguageResource), Name = "BrandStatus")]

        public BrandStatus BrandStatus { get; set; }

        public bool IsOnline => BrandStatus == BrandStatus.Active;
        public int? FileId { get; set; }

    }
}
