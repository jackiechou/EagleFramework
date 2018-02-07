using System.ComponentModel.DataAnnotations;

namespace Eagle.Core.Common
{
   public enum PriorityLevel
    {
        [Display(Name = "Normal")]
        Normal,
        [Display(Name = "Low")]
        Low,
        [Display(Name = "High")]
        High
    }
}
