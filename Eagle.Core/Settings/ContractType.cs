using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Core.Settings
{
    public enum ContractType
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "LaborContract", Description = "LaborContract", Order = 0)]
        LaborContract = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "TradeContract", Description = "TradeContract", Order = 1)]
        TradeContract = 2
    }
}
