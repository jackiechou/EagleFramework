using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Entities.SystemManagement.FileStorage;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Contents.Documentation;
using Eagle.Services.Dtos.SystemManagement.FileStorage;

namespace Eagle.Services.SystemManagement
{
    public interface IDocumentService: IBaseService
    {
        #region Document FOLDER

        IEnumerable<TreeDetail> GetDocumentFolderTree(DocumentFolderStatus? status = null, int? selectedId = null,
            bool? isRootShowed = false);
        DocumentFolderDetail GetFolderDetailByFolderId(int folderId);
        DocumentFolderDetail GetFolderDetailByFolderCode(Guid folderCode);
        DocumentFolderDetail InsertDocumentFolder(Guid applicationId, Guid userId, DocumentFolderEntry entry);
        void UpdateDocumentFolder(Guid userId, DocumentFolderEditEntry entry);
        void UpdateDocumentFolderStatus(Guid userId, int folderId, DocumentFolderStatus status);
        void DeleteDocumentFolder(int id);
        #endregion

        #region Document File

        SelectList PopulateStorageTypes(StorageType? selectedValue = null, bool? isShowSelectText = false);
        IEnumerable<FileBrowserDetail> GetImageBrowser(Guid applicationId, string path, FileBrowserType? type = null);
        IEnumerable<FileBrowserDetail> GetFileBrowser(Guid applicationId, string virtualPath, FileBrowserType? fileType = null);
        bool CheckImageTypeValid(Guid applicationId, HttpPostedFileBase fileUpload, out string errorMessage);
        bool IsImage(Guid applicationId, string filePath);
        byte[] CreateThumbnail(string physicalPath);
        void CreateDirectory(string basePath, FileBrowserEntry entry);
        void Destroy(string basePath, string relativeName, string type);
        string Upload(string path, HttpPostedFileBase file);
        DocumentFileDetail UploadAndSaveDbByPath(Guid applicationId, Guid? userId, string virtualPath, HttpPostedFileBase file, StorageType storageType, int? width = null, int? height = null, string description = null, string source = null);
        DocumentFileDetail UploadAndSaveDbByFolderId(Guid applicationId, Guid? userId, HttpPostedFileBase file, int folderId, StorageType storageType, int? width = null, int? height = null, string description = null, string source = null);
        DocumentFileDetail[] UploadAndSaveWithThumbnail(Guid applicationId, Guid? userId, HttpPostedFileBase file, int folderId, StorageType storageType, int? width=null, int? height=null, string description = null, string source = null);
        FileStream OpenReadFile(string virtualPath);
        string GetList(string fileIds);
        List<DocumentInfo> GetDownloadFileList(string fileIdList);
        string GenerateDownloadLink(int fileId);
        string CreateDownloadLink(int fileId);
        List<FileUploadDetail> GetDownloadFileList(int itemId, string itemTag, string fileIds);
        DocumentationLink CreateLinks(int fileId, bool? isOnlyPath);
        DocumentFileDetail GetFileDetail(int fileId);
        DocumentInfoDetail GetFileInfoDetail(int fileId);
        DocumentFileDetail GetFileDetailByFilePath(string filePath);
        string GetFilePath(int? fileId);
        DocumentFileDetail InsertFile(Guid applicationId, Guid? userId, DocumentFileEntry entry);
        void UpdateFile(Guid applicationId, Guid? userId, DocumentFileEditEntry entry);
        void UpdateFileStatus(Guid? userId, int fileId, DocumentFileStatus status);
        void DeleteFile(int id);
        void DeleteFiles(List<int> ids);
        void SaveImage(Guid? userId, DocumentFolder parent, HttpPostedFileBase file);
        void UploadAndUpdateFile(Guid? userId, int fileId, HttpPostedFileBase file, int? width = null, int? height = null);

        #endregion
    }
}
