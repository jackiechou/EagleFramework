using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.SystemManagement.FileStorage
{
    public class Library: EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DocumentId { get; set; }

        public Guid Documentkey { get; set; }

        public string DocumentFile { get; set; }

        public string DocumentTitle { get; set; }

        public Guid SubmitterUserid { get; set; }

        public DateTime Documentmodified { get; set; }
  
        public string Visibility { get; set; }

        public int CategoryId { get; set; }

        public string SharedWithDesc { get; set; }

        public string LibraryType { get; set; }
   
        public string LibraryStorage { get; set; }

        public string StorageFileName { get; set; }

        public Guid? UploadKey { get; set; }

        public int? ActivityId { get; set; }
    }
}
