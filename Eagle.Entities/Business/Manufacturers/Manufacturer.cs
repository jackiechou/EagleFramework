using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Business.Manufacturers
{
    [Table("Production.Manufacturer")]
    public class Manufacturer : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ManufacturerId { get; set; }
        public int CategoryId { get; set; }
        public string ManufacturerName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public int? Photo { get; set; }
        public ManufacturerStatus IsActive { get; set; }


        public int VendorId { get; set; }
    }
}
