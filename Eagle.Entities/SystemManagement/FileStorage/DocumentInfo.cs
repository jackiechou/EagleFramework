using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Configuration;

namespace Eagle.Entities.SystemManagement.FileStorage
{
    [NotMapped]
    public class DocumentInfo: DocumentFile
    {
        public DocumentInfo()
        {
            FileUrl = GlobalSettings.DefaultFileUrl;
        }
        public string FolderPath { get; set; }
        public string FileUrl { get; set; }

        //[NotMapped]
        //public virtual DocumentFolder Folder { get; set; }

        //[NotMapped]
        //public virtual ICollection<DownloadTracking> DownloadTrackings { get; set; }
    }
}
