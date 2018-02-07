using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Services.Events
{
    [Table("Event")]
    public class Event : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EventId { get; set; }
        public string EventCode { get; set; }
        public string EventTitle { get; set; }
        public string EventMessage { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string TimeZone { get; set; }
        public bool IsNotificationUsed { get; set; }
        public EventStatus Status { get; set; }

        public string Location { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int? SmallPhoto { get; set; }
        public int? LargePhoto { get; set; }

        public int TypeId { get; set; }
    }
}
