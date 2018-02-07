using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.SystemManagement
{
    [Table("PageMeta")]
    public class PageMeta : EntityBase
    {
        public Guid? PageCode { get; set; }
        public int MetaId { get; set; }
        public string MetaKey { get; set; }
        public string MetaValue { get; set; }
        public string Title { get; set; }
    }
}
