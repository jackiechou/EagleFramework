using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.SystemManagement
{
    [Table("PageOption")]
    public class PageOption : EntityBase
    {     
        public Guid PageCode { get; set; }
        public int OptionId { get; set; }
        public Guid OptionCode { get; set; }
        public string OptionName { get; set; }
        public string OptionValue { get; set; }
    }
}
