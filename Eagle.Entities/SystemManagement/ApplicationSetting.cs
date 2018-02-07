using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.SystemManagement
{
    [Table("ApplicationSetting")]
    public class ApplicationSetting: EntityBase
    {
        public ApplicationSetting()
        {
            IsActive = ApplicationSettingStatus.Active;
            IsSecured = true;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SettingId { get; set; }
        public string SettingName { get; set; }
        public string SettingValue { get; set; }
        public bool IsSecured { get; set; }
        public ApplicationSettingStatus IsActive { get; set; }

        public Guid ApplicationId { get; set; }
    }
}
