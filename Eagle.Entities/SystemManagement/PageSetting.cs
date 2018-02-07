using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.SystemManagement
{
    [Table("PageSetting")]
    public class PageSetting : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SettingId { get; set; }
        public string SettingName { get; set; }
        public string SettingValue { get; set; }

        public Guid? PageCode { get; set; }
    }
}
