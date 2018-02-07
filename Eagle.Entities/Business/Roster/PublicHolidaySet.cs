using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Entities.SystemManagement;

namespace Eagle.Entities.Business.Roster
{
    [Table("PublicHolidaySet", Schema = "Roster")]
    public class PublicHolidaySet : EntityBase
    {
        public PublicHolidaySet()
        {
            CreatedDate = DateTime.UtcNow;
            PublicHolidays = new HashSet<PublicHoliday>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PublicHolidaySetId { get; set; }
        public string Description { get; set; }
        public int CountryId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }


        public virtual ICollection<PublicHoliday> PublicHolidays { get; set; }
        public virtual Country Country { get; set; }
    }
}
