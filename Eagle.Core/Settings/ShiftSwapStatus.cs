using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eagle.Resources;

namespace Eagle.Core.Settings
{
    public enum ShiftSwapStatus
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Pending", Description = "Pending", Order = 0)]
        Pending = 0,
        [Display(ResourceType = typeof(LanguageResource), Name = "Confirmed", Description = "Confirmed", Order = 1)]
        Confirmed = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "Declined", Description = "Declined", Order = 2)]
        Declined = 2,
        [Display(ResourceType = typeof(LanguageResource), Name = "Approved", Description = "Approved", Order = 3)]
        Approved = 3,
        [Display(ResourceType = typeof(LanguageResource), Name = "Rejected", Description = "Rejected", Order = 4)]
        Rejected = 4,
    }
}
