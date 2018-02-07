using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Services.Events
{
    [NotMapped]
    public class EventInfo: Event
    {
        public virtual EventType EventType { get; set; }
    }
}
