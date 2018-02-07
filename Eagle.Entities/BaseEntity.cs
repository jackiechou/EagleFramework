using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities
{
    public class BaseEntity: EntityBase
    {
        public BaseEntity()
        {
            //Id = Guid.NewGuid().ToString();
            CreatedDate = DateTime.UtcNow;
        }

        ///// <summary>
        ///// Id of entity with format as GUID
        ///// </summary>
        //[NotMapped]
        //public string Id { get; set; }
        public string Ip { get; set; }
        public string LastUpdatedIp { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Guid? CreatedByUserId { get; set; }
        public Guid? LastModifiedByUserId { get; set; }
    }
}
