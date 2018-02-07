using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Entities.Services.Report
{
  
        public enum MonthLyEntity
        {
            [Display(ResourceType = typeof(LanguageResource), Name = "January")]
            January = 1,
            [Display(ResourceType = typeof(LanguageResource), Name = "February")]
            February = 2,
            [Display(ResourceType = typeof(LanguageResource), Name = "March")]
            March = 3,
            [Display(ResourceType = typeof(LanguageResource), Name = "April")]
            April = 4,
            [Display(ResourceType = typeof(LanguageResource), Name = "May")]
            May = 5,
            [Display(ResourceType = typeof(LanguageResource), Name = "June")]
            June = 6,
            [Display(ResourceType = typeof(LanguageResource), Name = "July")]
            July = 7,
            [Display(ResourceType = typeof(LanguageResource), Name = "August")]
            August = 8,
            [Display(ResourceType = typeof(LanguageResource), Name = "September")]
            September = 9,
            [Display(ResourceType = typeof(LanguageResource), Name = "October")]
            October = 10,
            [Display(ResourceType = typeof(LanguageResource), Name = "November")]
            November = 11,
            [Display(ResourceType = typeof(LanguageResource), Name = "December")]
            December = 12
        }
    //}
}
