using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Services.Booking
{
    [Table("ServicePackOption", Schema = "Booking")]
    public class ServicePackOption : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OptionId { get; set; }
        public string OptionName { get; set; }
        public decimal? OptionValue { get; set; }
        public int ListOrder { get; set; }
        public ServicePackOptionStatus IsActive { get; set; }

        public int PackageId { get; set; }
    }
}
