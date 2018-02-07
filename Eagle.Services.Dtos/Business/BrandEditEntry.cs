using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Business
{
    public class BrandEditEntry
    {
        
        public int BrandId { get; set; }
        [Display(ResourceType = typeof(LanguageResource), Name = "BrandName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string BrandName { get; set; }
        [Display(ResourceType = typeof(LanguageResource), Name = "BrandAlias")]
        public string BrandAlias { get; set; }
        [Display(ResourceType = typeof(LanguageResource), Name = "BrandStatus")]
        public BrandStatus IsOnline { get; set; }
        public int? FileId { get; set; }
    }
}
