using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Entities.SystemManagement.FileStorage;

namespace Eagle.Entities.Services.Booking
{
    [NotMapped]
    public class ServicePackInfo : ServicePack
    {
        public virtual ServiceCategory Category { get; set; }
        public virtual ServicePackType Type { get; set; }
        public virtual ServicePeriod Period { get; set; }
        public virtual ServiceDiscount Discount { get; set; }
        public virtual ServicePackDuration Duration { get; set; }
        public virtual ServiceTaxRate Tax { get; set; }
        public virtual ICollection<ServicePackOption> Options { get; set; }
        public virtual ICollection<ServicePackProvider> Providers { get; set; }
        public virtual ICollection<ServicePackRating> Ratings { get; set; }
    }
}
