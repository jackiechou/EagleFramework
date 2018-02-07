using System;
using System.Collections.Generic;
using Eagle.Core.Settings;
using Eagle.Entities.Common;
using Eagle.Entities.SystemManagement.FileStorage;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.SystemManagement
{
    public interface IDocumentFolderRepository : IRepositoryBase<DocumentFolder>
    {
        #region Directory Browser

        IEnumerable<FileBrowser> GetContent(string path, string filter);
        void CreateFolder(string basePath, string folderName);
        void DeleteDirectory(string physicalPath);
        #endregion

        #region Document Folder

        IEnumerable<TreeEntity> GetDocumentFolderTree(DocumentFolderStatus? status, int? selectedId,
            bool? isRootShowed = false);

        IEnumerable<DocumentFolder> GetAllParentNodesOfSelectedNode(int id, DocumentFolderStatus? status = null);
        IEnumerable<DocumentFolder> GetAllChildrenNodesOfSelectedNode(int id, DocumentFolderStatus? status = null);

        IEnumerable<DocumentFolder> GetDocumentFolders(DocumentFolderStatus? status);
        string GetFolderPathByFolderId(int folderId);
        string GetFolderPathByFolderCode(Guid folderCode);
        DocumentFolderInfo GetRootFolder();
        DocumentFolder GetDetailsByFolderId(int folderId);
        DocumentFolder GeDetailsByFolderCode(Guid folderCode);
        DocumentFolderInfo GetDetailsByFolderPath(string path);
        int GenerateNewFolderId();
        bool HasDataExisted(string folderName, int? parentId);
        bool HasFolderExisted(string folderName, string folderPath);
        bool HasChild(int folderId);
        int GetNewListOrder();

        #endregion
    }
}
