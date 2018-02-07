using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Services.Events
{
    [Table("EventType")]
    public class EventType : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public int? ParentId { get; set; }
        public int? Depth { get; set; }
        public string Lineage { get; set; }
        public bool? HasChild { get; set; }
        public int? ListOrder { get; set; }
        public EventTypeStatus Status { get; set; }
    }
}
