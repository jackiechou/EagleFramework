using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities
{
    public interface IObjectState
    {
        [NotMapped]
        ObjectState State { get; set; }
    }
}