using System;
using System.Collections.Generic;
using System.Web;
using Eagle.Core.Settings;
using Eagle.Entities.SystemManagement.FileStorage;

namespace Eagle.Services.Dtos.SystemManagement.FileStorage
{
    public class FileBrowserDetail : DtoBase
    {
        // public int Id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public long size { get; set; }
    }
    public class FileBrowserEntry : DtoBase
    {
        public string Name { get; set; }
        public FileBrowserType Type { get; set; }
        public long Size { get; set; }
    }
    public class DownloadTrackingDetail : DtoBase
    {
        public int DownloadId { get; set; }
        public DocumentType DocumentType { get; set; }
        public string Code { get; set; }
        public DownloadStatus Status { get; set; }
        public DateTime ExpiredDate { get; set; }
        public int PercentCompleted { get; set; }

        public int? FileId { get; set; }
    }
    public class FileUploadDetail : DtoBase
    {
        public DocumentInfo DocumentFileInfo { get; set; }
        public HttpPostedFileBase FileUploadName { get; set; }
        public int ItemId { get; set; }
        public string ItemTag { get; set; }
        public string FileIds { get; set; }
    }
    public class FileUploadModel : DtoBase
    {
        public string FolderKey { get; set; }
        public int FileId { get; set; }
        public string FileName { get; set; }
        public string FileTitle { get; set; }
        public string FileDescription { get; set; }
        public string FileUrl { get; set; }

        public string ItemId { get; set; }
        public string ItemTag { get; set; }

    }
    public class FileViewModel
    {
        public string Folder { get; set; }
        public string SubFolder { get; set; }
    }
    public class FileDetail : DtoBase
    {
        public string DisplayName { get; set; }
        public string FilePath { get; set; }
    }
    public class FileModel : DtoBase
    {
        public string ItemId { get; set; }
        public string ItemTag { get; set; }
        public string FileIds { get; set; }
        public string FolderKey { get; set; }
        public IList<FileUploadDetail> FileUploadList { get; set; }
    }
    public class FileControlDetail : DtoBase
    {
        public string FolderKey { get; set; }
        public string FileIds { get; set; }
        public int? KeyId { get; set; }
        public string UpdateFileIdsServiceUrl { get; set; }
    }
}
