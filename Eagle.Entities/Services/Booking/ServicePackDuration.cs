using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Services.Booking
{
    [Table("ServicePackDuration", Schema = "Booking")]
    public class ServicePackDuration : EntityBase
    {
        public ServicePackDuration()
        {
            CreatedDate = DateTime.UtcNow;
            IsActive = true;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DurationId { get; set; }
        public string DurationName { get; set; }
        public int AllotedTime { get; set; }
        public string Unit { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
