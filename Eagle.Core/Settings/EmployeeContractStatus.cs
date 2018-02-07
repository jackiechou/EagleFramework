using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Core.Settings
{
    public enum EmployeeContractStatus
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Probationary", Description = "Probationary", Order = 0)]
        Probationary = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "Official", Description = "Official", Order = 1)]
        Official = 2
    }
}
