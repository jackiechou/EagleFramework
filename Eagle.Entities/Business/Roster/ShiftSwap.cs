using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Business.Roster
{
    [Table("ShiftSwap", Schema = "Roster")]
    public class ShiftSwap : EntityBase
    {
        public ShiftSwap()
        {
            CreatedDate = DateTime.UtcNow;
            Status = ShiftSwapStatus.Pending;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ShiftSwapId { get; set; }
        public int RequesterId { get; set; }
        public int ReceiverId { get; set; }
        public int ShiftId { get; set; }
        public string Description { get; set; }
        public ShiftSwapStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

    }
}
