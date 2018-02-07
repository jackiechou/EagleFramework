using System;
using System.ComponentModel.DataAnnotations;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Services.Booking
{
    public class BookingSingleKindEntry : DtoBase
    {
        public BookingSingleKindEntry()
        {
            Capacity = 1;
            FromPeriod = 0;
            Deposit = 0;
            StartDate = DateTime.UtcNow;
            PeriodGroup = BookingPeriodGroup.Anytime;
        }

        //How many guests?
        [Display(ResourceType = typeof(LanguageResource), Name = "Capacity")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "PleaseEnterCapacity")]
        [Range(1, 50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "InvalidCapacity")]
        public int Capacity { get; set; }

        //When?
        [Display(ResourceType = typeof(LanguageResource), Name = "StartDate")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public DateTime StartDate { get; set; }
        
        [Display(ResourceType = typeof(LanguageResource), Name = "PeriodGroup")]
        [Range(0,int.MaxValue, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Select")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [EnumDataType(typeof(BookingPeriodGroup))]
        public BookingPeriodGroup PeriodGroup { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "From")]
        public int? FromPeriod { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Deposit")]
        public decimal? Deposit { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CurrencyCode")]
        public string CurrencyCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Comment")]
        public string Comment { get; set; }

        //What kind? List of services
        //MinimumElements(1, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [MinLength(1, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MinLength")]
        public BookingPackageEntry[] Packages { get; set; }
    }
}
