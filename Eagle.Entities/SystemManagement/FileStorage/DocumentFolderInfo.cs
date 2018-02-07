using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.SystemManagement.FileStorage
{
    [NotMapped]
    public class DocumentFolderInfo: DocumentFolder
    {
        [NotMapped]
        public virtual ICollection<DocumentFolder> ChildFolders { get; set; }

        [NotMapped]
        public virtual ICollection<DocumentFile> Files { get; set; }
    }
}
