using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.SystemManagement
{
    [Table("Application")]
    public class ApplicationEntity: EntityBase
    {
        public ApplicationEntity()
        {
            ApplicationId = Guid.NewGuid();
        }

        [Key]
        public Guid ApplicationId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SeqNo { get; set; }
        public string ApplicationName { get; set; }
        public string DefaultLanguage { get; set; }
        public string HomeDirectory { get; set; }
        public string Currency { get; set; }
        public string TimeZoneOffset { get; set; }
        public string Url { get; set; }
        public string LogoFile { get; set; }
        public string BackgroundFile { get; set; }
        public string KeyWords { get; set; }
        public string CopyRight { get; set; }
        public string FooterText { get; set; }
        public string Description { get; set; }
        public int? HostSpace { get; set; }
        public double? HostFee { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public Guid? RegisteredUserId { get; set; }
    }
}
