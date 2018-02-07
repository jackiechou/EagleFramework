using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Services.Booking
{
    [Table("ServicePeriod", Schema = "Booking")]
    public class ServicePeriod : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PeriodId { get; set; }
        public string PeriodName { get; set; }
        public int PeriodValue { get; set; }
        public bool Status { get; set; }
    }
}
