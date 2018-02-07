using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Services.Booking
{
    public class ServicePackProvider : EntityBase
    { 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProviderId { get; set; }
        public int PackageId { get; set; }
        public int EmployeeId { get; set; }
    }
}
