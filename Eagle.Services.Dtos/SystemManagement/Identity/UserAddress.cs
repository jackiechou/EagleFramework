using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Eagle.Resources;

namespace Eagle.Services.Dtos.SystemManagement.Identity
{
    public class UserAddressDetail : DtoBase
    {
        public int UserAddressId { get; set; }
        public int AddressId { get; set; }
        public Guid UserId { get; set; }
        public bool? IsDefault { get; set; }

        public AddressDetail Address { get; set; }
    }

    public class UserAddressEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "AddressId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int AddressId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "UserId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public Guid UserId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsDefault")]
        public bool? IsDefault { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public bool? IsActive { get; set; }
    }

    public class UserAddressInfoDetail : UserAddressDetail
    {
        public string Location
        {
            get
            {
                var sb = new StringBuilder();
                if (!string.IsNullOrEmpty(Address.Street))
                {
                    sb.Append(Address.Street);
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

                if (!string.IsNullOrEmpty(Address.PostalCode))
                {
                    sb.Append($", {Address.PostalCode}");
                }

                if (!string.IsNullOrEmpty(Address.Description))
                {
                    sb.Append($", {Address.Description}");
                }
                return sb.ToString();
            }
        }

        public CountryDetail Country { get; set; }
        public ProvinceDetail Province { get; set; }
        public RegionDetail Region { get; set; }
    }
}
