using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.SystemManagement
{
    public class AddressInfoDetail : AddressDetail
    {
        private string _location;
        public string Location
        {
            get
            {
                var sb = new StringBuilder();
                if (!string.IsNullOrEmpty(Street))
                {
                    sb.Append(Street);
                }

                if (Region != null && !string.IsNullOrEmpty(Region.RegionName))
                {
                    sb.Append($", {Region.RegionName}");
                }

                if (Province != null && !string.IsNullOrEmpty(Province.ProvinceName))
                {
                    sb.Append($", {Province.ProvinceName}");
                }
                
                if (!string.IsNullOrEmpty(PostalCode))
                {
                    sb.Append($", {PostalCode}");
                }

                if (!string.IsNullOrEmpty(Description))
                {
                    sb.Append($", {Description}");
                }
                _location = sb.ToString();
                return _location;
            }
            set { _location = value; }
        }
        public string LocationInfo
        {
            get
            {
                var sb = new StringBuilder();
                if (!string.IsNullOrEmpty(Street))
                {
                    sb.Append(Street);
                }

                if (Region != null && !string.IsNullOrEmpty(Region.RegionName))
                {
                    sb.Append($", {Region.RegionName}");
                }

                if (Province != null && !string.IsNullOrEmpty(Province.ProvinceName))
                {
                    sb.Append($", {Province.ProvinceName}");
                }

                if (Country != null && !string.IsNullOrEmpty(Country.CountryName))
                {
                    sb.Append($", {Country.CountryName}");
                }

                if (!string.IsNullOrEmpty(PostalCode))
                {
                    sb.Append($", {PostalCode}");
                }

                if (!string.IsNullOrEmpty(Description))
                {
                    sb.Append($", {Description}");
                }
                _location = sb.ToString();
                return _location;
            }
            set { _location = value; }
        }
        public CountryDetail Country { get; set; }
        public RegionDetail Region { get; set; }
        public ProvinceDetail Province { get; set; }
    }
    public class AddressDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "AddressId")]
        public int AddressId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "AddressTypeId")]
        public AddressType AddressTypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Street")]
        public string Street { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PostalCode")]
        public string PostalCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CreatedDate")]
        public DateTime? CreatedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ModifiedDate")]
        public DateTime? ModifiedDate { get; set; }


        [Display(ResourceType = typeof(LanguageResource), Name = "CountryId")]
        public int? CountryId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ProvinceId")]
        public int? ProvinceId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "RegionId")]
        public int? RegionId { get; set; }
    }

    public class AddressEntry : DtoBase
    {
        public AddressEntry()
        {
            AddressTypeId = AddressType.Temporary;
            CountryId = GlobalSettings.DefaultCountryId;
            ProvinceId = GlobalSettings.DefaultProvinceId;
        }

        [Display(ResourceType = typeof(LanguageResource), Name = "AddressTypeId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public AddressType AddressTypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Street")]
        [StringLength(512, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        [RegularExpression(@"[a-zA-Z0-9_ÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ\\s \\\-~!@#$%^&*()_+={}:|""?`;:><',./[\]]+", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "InvalidTitle")]
        public string Street { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PostalCode")]
        [StringLength(10, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string PostalCode { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        //[StringLength(int.MaxValue, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Description { get; set; }


        [Display(ResourceType = typeof(LanguageResource), Name = "CountryId")]
        public int? CountryId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ProvinceId")]
        public int? ProvinceId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "RegionId")]
        public int? RegionId { get; set; }

    }
    public class AddressEditEntry : AddressEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "AddressId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int AddressId { get; set; }
    }
    public class EmergencyAddressEntry : AddressEntry
    {
        public EmergencyAddressEntry()
        {
            AddressTypeId = AddressType.Emergency;
        }
    }
    public class EmergencyAddressEditEntry : EmergencyAddressEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "AddressId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int AddressId { get; set; }
    }
    public class PermanentAddressEntry : AddressEntry
    {
        public PermanentAddressEntry()
        {
            AddressTypeId = AddressType.Permanent;
        }
    }
    public class PermanentAddressEditEntry : PermanentAddressEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "AddressId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int AddressId { get; set; }
    }
    public class TemporaryAddressEntry : AddressEntry
    {
        public TemporaryAddressEntry()
        {
            AddressTypeId = AddressType.Temporary;
        }
    }
    public class TemporaryAddressEditEntry : TemporaryAddressEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "AddressId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int AddressId { get; set; }
    }
}
