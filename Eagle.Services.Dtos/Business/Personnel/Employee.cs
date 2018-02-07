using System;
using System.ComponentModel.DataAnnotations;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services.Dtos.SystemManagement;
using Eagle.Services.Dtos.SystemManagement.Identity;

namespace Eagle.Services.Dtos.Business.Personnel
{
    public class EmployeeEntry : DtoBase
    {
        public EmployeeEntry()
        {
            Contact = new ContactEntry();
            EmergencyAddress = new EmergencyAddressEntry();
            PermanentAddress = new PermanentAddressEntry();
            JoinedDate = DateTime.UtcNow;
            Status = EmployeeStatus.Published;
        }

        [Display(ResourceType = typeof(LanguageResource), Name = "EmployeeNo")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string EmployeeNo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CompanyId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "PleaseSelectCompany")]
        public int CompanyId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PositionId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "PleaseSelectPosition")]
        public int PositionId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "JoinedDate")]
        public DateTime? JoinedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PasswordHash")]
        public string PasswordHash { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PasswordSalt")]
        public string PasswordSalt { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        [EnumDataType(typeof(EmployeeStatus))]
        public EmployeeStatus Status { get; set; }

        public ContactEntry Contact { get; set; }
        public EmergencyAddressEntry EmergencyAddress { get; set; }
        public PermanentAddressEntry PermanentAddress { get; set; }
    }
    public class EmployeeEditEntry : DtoBase
    {
        public EmployeeEditEntry()
        {
            Contact = new ContactEditEntry();
            EmergencyAddress = new EmergencyAddressEditEntry();
            PermanentAddress = new PermanentAddressEditEntry();
        }
        [Display(ResourceType = typeof(LanguageResource), Name = "EmployeeId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int EmployeeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EmployeeNo")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string EmployeeNo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ContactId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int ContactId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CompanyId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "PleaseSelectCompany")]
        public int CompanyId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PositionId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "PleaseSelectPosition")]
        public int PositionId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "VendorId")]
        public int? VendorId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "JoinedDate")]
        public DateTime? JoinedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PasswordHash")]
        public string PasswordHash { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PasswordSalt")]
        public string PasswordSalt { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        [EnumDataType(typeof(EmployeeStatus))]
        public EmployeeStatus Status { get; set; }

        public ContactEditEntry Contact { get; set; }
        public EmergencyAddressEditEntry EmergencyAddress { get; set; }
        public PermanentAddressEditEntry PermanentAddress { get; set; }
    }
    public class EmployeeSearchEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "SearchText")]
        public string SearchText { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SearchStatus")]
        public EmployeeStatus? SearchStatus { get; set; }
    }
    public class EmployeeDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "EmployeeId")]
        public int EmployeeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EmployeeNo")]
        public string EmployeeNo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ContactId")]
        public int ContactId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EmergencyAddressId")]
        public int? EmergencyAddressId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PermanentAddressId")]
        public int? PermanentAddressId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "VendorId")]
        public int? VendorId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CompanyId")]
        public int CompanyId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PositionId")]
        public int PositionId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "JoinedDate")]
        public DateTime? JoinedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PasswordHash")]
        public string PasswordHash { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PasswordSalt")]
        public string PasswordSalt { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public EmployeeStatus Status { get; set; }
    }
    public class EmployeeInfoDetail : EmployeeDetail
    {
        public virtual ContactInfoDetail Contact { get; set; }
        public virtual AddressInfoDetail EmergencyAddress { get; set; }
        public virtual AddressInfoDetail PermanentAddress { get; set; }
        public virtual CompanyDetail Company { get; set; }
        public virtual JobPositionDetail JobPosition { get; set; }
    }



    public class EmployeeAvailabilityEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "EmployeeId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int EmployeeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "StartDate")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public DateTime StartDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EndDate")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public DateTime EndDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TimeZoneId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string TimeZoneId { get; set; }
    }
    public class EmployeeAvailabilityEditEntry : EmployeeAvailabilityEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "EmployeeAvailabilityId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int EmployeeAvailabilityId { get; set; }
    }


    public class EmployeeTimeOffDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "EmployeeTimeOffId")]

        public int EmployeeTimeOffId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EmployeeId")]
        public int EmployeeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TimeZoneId")]
        public string TimeZoneId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EmployeeId")]
        public int TimeOffTypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "StartDate")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime StartDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EndDate")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime EndDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Reason")]
        public string Reason { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public TimeOffStatus Status { get; set; }
    }
    public class EmployeeTimeOffEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "EmployeeId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int EmployeeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TimeZoneId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string TimeZoneId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EmployeeId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int TimeOffTypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "StartDate")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public DateTime StartDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EndDate")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public DateTime EndDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Reason")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string Reason { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public TimeOffStatus Status { get; set; }

    }
    public class EmployeeTimeOffEditEntry : EmployeeTimeOffEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "EmployeeTimeOffId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int EmployeeTimeOffId { get; set; }
    }
}
