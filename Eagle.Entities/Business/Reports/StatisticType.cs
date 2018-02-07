using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Entities.Business.Reports
{
    public enum StatisticType
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "OrganizationalStructureTypes")]
        OrganizationalStructureTypes = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "AgePeriod")]
        AgePeriod = 2,
        [Display(ResourceType = typeof(LanguageResource), Name = "SalaryLevelRatio")]
        SalaryLevelRatio = 3
    }
}
