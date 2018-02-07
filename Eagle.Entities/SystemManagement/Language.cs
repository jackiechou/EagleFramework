using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.SystemManagement
{
    [Table("Language")]
    public class Language : EntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LanguageId { get; set; }

        [Key]
        public string LanguageCode { get; set; }
        public string LanguageName { get; set; }
        public string Description { get; set; }
        public LanguageStatus Status { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
