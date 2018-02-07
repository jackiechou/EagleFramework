using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Entities.SystemManagement.FileStorage;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.SystemManagement
{
    public interface IDocumentFileRepository : IRepositoryBase<DocumentFile>
    {
        byte[] CreateThumbnail(string relativePath);
        void DeleteFile(string physicalPath);
        string UploadFile(string virtualPath, HttpPostedFileBase file, int? width = null, int? height = null);
        FileStream OpenReadFile(string virtualPath);
        DocumentFile GetDetailsByFolderIdAndFileName(int folderId, string fileName);
        string GetFileNameByFileId(int fileId);
        byte[] GetFileContentByFileId(int ileId);
        string GetFileUrlByFileId(int fileId);
        DocumentInfo GetDetails(int fileId);

        bool HasDataExisted(int folderId, string fileName);
        IEnumerable<DocumentInfo> GetList();
        SelectList PopulateStorageTypes(StorageType? selectedValue, bool? isShowSelectText = false);
    }
}
