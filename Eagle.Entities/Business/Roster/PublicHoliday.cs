using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Business.Roster
{
    [Table("PublicHoliday", Schema = "Roster")]
    public class PublicHoliday : EntityBase
    {
        public PublicHoliday()
        {
            CreatedDate = DateTime.UtcNow;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PublicHolidayId { get; set; }
        public int PublicHolidaySetId { get; set; }
        public DateTime Holiday { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public PublicHolidaySet PublicHolidaySet { get; set; }
    }
}
