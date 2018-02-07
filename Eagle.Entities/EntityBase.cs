using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities
{
    [Serializable]
    public abstract class EntityBase : IObjectState
    {
        [NotMapped]
        public ObjectState State { get; set; }
    }
}