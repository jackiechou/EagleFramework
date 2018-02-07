using System.ComponentModel.DataAnnotations;
using Eagle.Core.Configuration;
using Eagle.Resources;

namespace Eagle.Services.Dtos.SystemManagement.FileStorage
{
    public class DocumentInfoDetail: DocumentFileDetail
    {
        public DocumentInfoDetail()
        {
            FileUrl = GlobalSettings.DefaultFileUrl;
        }
        [Display(ResourceType = typeof(LanguageResource), Name = "FolderPath")]
        public string FolderPath { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileUrl")]
        public string FileUrl { get; set; }
    }
}
