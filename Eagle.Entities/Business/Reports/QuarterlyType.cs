using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Entities.Business.Reports
{
    public enum QuarterlyType
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "FirstQuarter")]
        FirstQuarter = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "SecondQuarter")]
        SecondQuarter = 2,
        [Display(ResourceType = typeof(LanguageResource), Name = "ThirdQuarter")]
        ThirdQuarter = 3,
        [Display(ResourceType = typeof(LanguageResource), Name = "FourthQuarter")]
        FourthQuarter = 4
    }
}
