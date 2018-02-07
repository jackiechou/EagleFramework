using Eagle.Resources;
using System.ComponentModel.DataAnnotations;

namespace Eagle.Core.Settings
{
    public enum CartItemStatus
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "CartItemStatusAvailable", Description = "CartItemStatusAvailable", Order = 0)]
        Available = 0,
        [Display(ResourceType = typeof(LanguageResource), Name = "CartItemStatusOutOfStock", Description = "CartItemStatusOutOfStock", Order = 1)]
        OutOfStock = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "CartItemStatusRemoved", Description = "CartItemStatusRemoved", Order = 2)]
        Removed = 2,
        [Display(ResourceType = typeof(LanguageResource), Name = "CartItemStatusNotBeenSalesYet", Description = "CartItemStatusNotBeenSalesYet", Order = 3)]
        NotBeenSalesYet = 3,
        [Display(ResourceType = typeof(LanguageResource), Name = "CartItemStatusExceedUnitOfStock", Description = "CartItemStatusExceedUnitOfStock", Order = 4)]
        ExceedUnitOfStock = 4,
    }
}
