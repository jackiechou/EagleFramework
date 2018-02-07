using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.SystemManagement.FileStorage
{
    [NotMapped]
    public class FileBrowser : EntityBase
    {
        public string Name { get; set; }
        public FileBrowserType Type { get; set; }
        public long Size { get; set; }
    }
}
