using System;
using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Business.Roster
{
    public class PublicHolidayDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PublicHolidayId")]
        public int PublicHolidayId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PublicHolidaySetId")]
        public int PublicHolidaySetId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Holiday")]
        public DateTime Holiday { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LastModifiedDate")]
        public DateTime? LastModifiedDate { get; set; }

        //[Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        //public PublicHolidaySet PublicHolidaySet { get; set; }
    }
    public class PublicHolidayEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PublicHolidaySetId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int PublicHolidaySetId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Holiday")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public DateTime Holiday { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        [StringLength(500, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string Description { get; set; }
    }
    public class PublicHolidayEditEntry : PublicHolidayEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PublicHolidayId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int PublicHolidayId { get; set; }
    }

    public class PublicHolidaySetDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PublicHolidaySetId")]
        public int PublicHolidaySetId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CountryId")]
        public int CountryId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LastModifiedDate")]
        public DateTime? LastModifiedDate { get; set; }


        //public IEnumerable<PublicHolidayDetail> PublicHolidays { get; set; }
        //public CountryDetail Country { get; set; }
    }
    public class PublicHolidaySetEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ShiftTypeName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(500, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ShiftTypeName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int CountryId { get; set; }
    }
    public class PublicHolidaySetEditEntry : PublicHolidaySetEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PublicHolidaySetId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int PublicHolidaySetId { get; set; }
    }
}
