using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Services.Booking
{
    [Table("ServicePackType", Schema = "Booking")]
    public class ServicePackType : EntityBase
    {
        public ServicePackType()
        {
            IsOnline = true;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public bool IsOnline { get; set; }
        public ServicePackTypeStatus IsActive { get; set; }
    }
}
