using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Business.Roster
{
    [Table("ShiftType", Schema = "Roster")]
    public class ShiftType : EntityBase
    {
        public ShiftType()
        {
            CreatedDate = DateTime.UtcNow;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ShiftTypeId { get; set; }
        public string ShiftTypeName { get; set; }
        public int? TimeOffTypeId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TimeOffType TimeOffType { get; set; }
    }
}
