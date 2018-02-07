using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Services.Booking
{
    public class BookingCalendarSearchEntry : DtoBase, IValidatableObject
    {
        public BookingCalendarSearchEntry()
        {
            FromDate = DateTime.UtcNow.AddMonths(-6);
            ToDate = DateTime.UtcNow.AddMonths(6);
        }

        [Display(ResourceType = typeof(LanguageResource), Name = "From")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FromDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "To")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ToDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            if (FromDate <= DateTime.MinValue.ToUniversalTime())
            {
                results.Add(new ValidationResult(LanguageResource.InvalidStartDate, new[] { "StartDate" }));
            }

            if (ToDate < FromDate)
            {
                results.Add(new ValidationResult(LanguageResource.StartDateMustBeBeforeEndDate, new[] { "EndDate" }));
            }

            return results;
        }
    }
}
