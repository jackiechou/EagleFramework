using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Services.Booking
{
    [Table("ServiceCategory", Schema = "Booking")]
    public class ServiceCategory : BaseEntity
    {
        public ServiceCategory()
        {
            TypeId = ServiceType.Single;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? ParentId { get; set; }
        public int? Depth { get; set; }
        public string Lineage { get; set; }
        public bool? HasChild { get; set; }
        public int? ListOrder { get; set; }
        public ServiceCategoryStatus Status { get; set; }

        public ServiceType TypeId { get; set; }
    }
}
