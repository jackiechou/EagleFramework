using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.SystemManagement
{
    [Table("PageModuleSetting")]
    public class PageModuleSetting : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PageModuleSettingId { get; set; }
        public int PageModuleId { get; set; }
        public string SettingName { get; set; }
        public string SettingValue { get; set; }
    }
}
