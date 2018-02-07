using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Eagle.Entities.SystemManagement
{
    [NotMapped]
    public class UserAddressInfo : UserAddress
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

        public virtual Country Country { get; set; }
        public virtual Province Province { get; set; }
        public virtual Region Region { get; set; }
    }
}
