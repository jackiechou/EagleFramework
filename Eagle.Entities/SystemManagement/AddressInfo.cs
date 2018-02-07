using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Eagle.Entities.SystemManagement
{
    [NotMapped]
    public class AddressInfo : Address
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
        public virtual Country Country { get; set; }
        public virtual Region Region { get; set; }
        public virtual Province Province { get; set; }
    }
}
