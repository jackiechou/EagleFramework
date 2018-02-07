using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.SystemManagement
{
    [Table("ApplicationLanguage")]
    public class ApplicationLanguage : EntityBase
    {
        public ApplicationLanguage()
        {
            Status = ApplicationLanguageStatus.Active;
       } 

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ApplicationLanguageId { get; set; }
        public Guid ApplicationId { get; set; }
        public string LanguageCode { get; set; }
        public bool IsSelected { get; set; }
        public ApplicationLanguageStatus Status { get; set; }

        [NotMapped]
        public virtual Language Language { get; set; }
    }
}
