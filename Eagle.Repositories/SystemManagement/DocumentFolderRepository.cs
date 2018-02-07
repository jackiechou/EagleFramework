using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Eagle.Core.Settings;
using Eagle.Entities.Common;
using Eagle.Entities.SystemManagement.FileStorage;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.SystemManagement
{
    public class DocumentFolderRepository : RepositoryBase<DocumentFolder>, IDocumentFolderRepository
    {
        public DocumentFolderRepository(IDataContext databaseContext) : base(databaseContext) { }

        #region Directory Browser
        public IEnumerable<FileBrowser> GetContent(string virtualPath, string filter)
        {
            return GetFiles(virtualPath, filter).Concat(GetDirectories(virtualPath));
        }
        private IEnumerable<FileBrowser> GetFiles(string virtualPath, string filter)
        {
            var physicalPath = HttpContext.Current.Server.MapPath(virtualPath);
            if (string.IsNullOrEmpty(physicalPath)) return null;

            var directory = new DirectoryInfo(physicalPath);
            var extensions = (filter ?? "*").Split(",|;".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            return extensions.SelectMany(directory.GetFiles)
                .Select(file => new FileBrowser
                {
                    Name = file.Name,
                    Size = file.Length,
                    Type = FileBrowserType.File
                });
        }
        private IEnumerable<FileBrowser> GetDirectories(string virtualPath)
        {
            var physicalPath = HttpContext.Current.Server.MapPath(virtualPath);
            var directory = new DirectoryInfo(physicalPath);

            return directory.GetDirectories()
                .Select(subDirectory => new FileBrowser
                {
                    Name = subDirectory.Name,
                    Type = FileBrowserType.Directory
                });
        }
        public IEnumerable<DocumentFolder> GetDocumentFolders(DocumentFolderStatus? status)
        {
            return (from x in DataContext.Get<DocumentFolder>()
                    where (status == null || x.IsActive == status)
                    select x).AsEnumerable();
        }
        public void CreateFolder(string basePath, string folderName)
        {
            if (!string.IsNullOrEmpty(basePath) && !string.IsNullOrEmpty(folderName))
            {
                var physicalPath = Path.Combine(HttpContext.Current.Server.MapPath(basePath), folderName);

                if (!Directory.Exists(physicalPath))
                {
                    Directory.CreateDirectory(physicalPath);
                }
            }
        }
        public void DeleteDirectory(string physicalPath)
        {
            if (string.IsNullOrEmpty(physicalPath)) return;

            if (Directory.Exists(physicalPath))
            {
                Directory.Delete(physicalPath, true);
            }
        }

        #endregion
        
        #region Document Folder

        //HIERACHICAL TREE
        public IEnumerable<TreeEntity> GetDocumentFolderTree(DocumentFolderStatus? status, int? selectedId, bool? isRootShowed = false)
        {
            var list = (from p in DataContext.Get<DocumentFolder>()
                        where status == null || p.IsActive == status
                        select new TreeEntity
                        {
                            id = p.FolderId,
                            key = p.FolderId,
                            parentid = p.ParentId,
                            depth = p.Depth,
                            name = p.FolderName,
                            title = p.FolderName,
                            text = p.FolderName,
                            tooltip = p.Description,
                            hasChild = p.HasChild ?? false,
                            folder = p.HasChild ?? false,
                            lazy = p.HasChild ?? false,
                            expanded = p.HasChild ?? false,
                            selected = (selectedId != null && p.FolderId == selectedId),
                            state = (p.IsActive == DocumentFolderStatus.Active),
                        }).ToList();

            var recursiveObjects = RecursiveFillTree(list, 0);

            if (isRootShowed != null && isRootShowed == true)
            {
                recursiveObjects.Insert(0, new TreeEntity
                {
                    id = 0,
                    key = 0,
                    parentid = 0,
                    depth = 1,
                    name = LanguageResource.Root,
                    title = LanguageResource.Root,
                    text = LanguageResource.Root,
                    tooltip = LanguageResource.Root,
                    hasChild = recursiveObjects.Any(),
                    folder = true,
                    lazy = true,
                    expanded = true,
                    selected = (selectedId != null && selectedId == 0),
                    state = true
                });
            }
            return recursiveObjects;
        }
        private List<TreeEntity> RecursiveFillTree(List<TreeEntity> elements, int? parentid)
        {
            if (elements == null) return null;
            List<TreeEntity> items = new List<TreeEntity>();
            List<TreeEntity> children = elements.Where(p => p.parentid == parentid).Select(
               p => new TreeEntity
               {
                   id = p.id,
                   key = p.key,
                   parentid = p.parentid,
                   depth = p.depth,
                   name = p.name,
                   title = p.title,
                   text = p.text,
                   tooltip = p.tooltip,
                   hasChild = p.hasChild,
                   folder = p.folder,
                   lazy = p.lazy,
                   expanded = p.expanded,
                   selected = p.selected,
                   state = p.state
               }).ToList();

            if (children.Count > 0)
            {
                items.AddRange(children.Select(child => new TreeEntity
                {
                    id = child.id,
                    key = child.key,
                    parentid = child.parentid,
                    depth = child.depth,
                    name = child.name,
                    title = child.title,
                    text = child.text,
                    tooltip = child.tooltip,
                    hasChild = child.hasChild,
                    folder = child.folder,
                    lazy = child.lazy,
                    expanded = child.expanded,
                    selected = child.selected,
                    state = child.state,
                    children = RecursiveFillTree(elements, child.id)
                }));
            }
            return items;
        }

        public IEnumerable<DocumentFolder> GetAllParentNodesOfSelectedNode(int id, DocumentFolderStatus? status = null)
        {
            const string strCommand = @"EXEC dbo.DocumentFolder_GetParentNodesOfSelectedNode @id = {0}, @status = {1}";
            return DataContext.Get<DocumentFolder>(strCommand, id, status);
        }

        public IEnumerable<DocumentFolder> GetAllChildrenNodesOfSelectedNode(int id, DocumentFolderStatus? status = null)
        {
            const string strCommand = @"EXEC dbo.DocumentFolder_GetAllChildrenNodesOfSelectedNode @id = {0}, @status = {1}";
            return DataContext.Get<DocumentFolder>(strCommand, id, status);
        }

        public string GetFolderPathByFolderId(int folderId)
        {
            string folderPath = (from f in DataContext.Get<DocumentFolder>()
                                 where f.FolderId == folderId
                                 select f.FolderPath).FirstOrDefault();

            if (!string.IsNullOrEmpty(folderPath))
            {
                if (!Directory.Exists(HttpContext.Current.Server.MapPath(folderPath)))
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(folderPath));
            }
            return folderPath;
        }
        public string GetFolderPathByFolderCode(Guid folderCode)
        {
            string folderPath = (from f in DataContext.Get<DocumentFolder>()
                                 where f.FolderCode == folderCode
                                 select f.FolderPath).FirstOrDefault();
            if (!string.IsNullOrEmpty(folderPath))
            {
                if (!Directory.Exists(HttpContext.Current.Server.MapPath(folderPath)))
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(folderPath));
            }
            return folderPath;
        }
        public DocumentFolderInfo GetRootFolder()
        {
            return (from f in DataContext.Get<DocumentFolder>()
                    where f.ParentId == null
                    select new DocumentFolderInfo
                    {
                        ApplicationId = f.ApplicationId,
                        FolderCode = f.FolderCode,
                        FolderId = f.FolderId,
                        ParentId = f.ParentId,
                        FolderName = f.FolderName,
                        FolderPath = f.FolderPath,
                        FolderIcon = f.FolderIcon,
                        Description = f.Description,
                        IsActive = f.IsActive
                    }).FirstOrDefault();
        }
        public DocumentFolder GetDetailsByFolderId(int folderId)
        {
            return (from f in DataContext.Get<DocumentFolder>()
                    where f.FolderId == folderId
                    select f).FirstOrDefault();
        }
        public DocumentFolder GeDetailsByFolderCode(Guid folderCode)
        {
            return (from f in DataContext.Get<DocumentFolder>()
                    where f.FolderCode == folderCode
                    select f).FirstOrDefault();
        }
        public DocumentFolderInfo GetDetailsByFolderPath(string path)
        {
            if (string.IsNullOrEmpty(path) || path == "/")
            {
                return GetRootFolder();
            }

            var name = GetFolderNames(path).Last().ToLower();

            if (!path.StartsWith("/"))
            {
                path = "/" + path;
            }

            path = NormalizePath(path, name);

            return (from f in DataContext.Get<DocumentFolder>()
             where f.FolderPath.ToLower() == path
             select new DocumentFolderInfo
             {
                 ApplicationId = f.ApplicationId,
                 FolderCode = f.FolderCode,
                 FolderId = f.FolderId,
                 ParentId = f.ParentId,
                 FolderName = f.FolderName,
                 FolderPath = f.FolderPath,
                 FolderIcon = f.FolderIcon,
                 Description = f.Description,
                 IsActive = f.IsActive
             }).FirstOrDefault();
        }
        private string NormalizePath(string path, string name)
        {
            path = VirtualPathUtility.AppendTrailingSlash(path).Replace("\\", "/").ToLower();
            path = path.Remove(path.LastIndexOf(name, StringComparison.Ordinal));
            return path;
        }
        private IEnumerable<FileBrowser> GetFolders(DocumentFolderInfo parent)
        {
            //if (parent == null)
            //   return new FileBrowser[] {};
            //return DataContext.Get<DocumentFolder>().Where(f => f.ParentId == parent.ParentId).Select(f => new FileBrowser { Name = f.FolderName, Type = FileBrowserType.Directory });
            return (parent == null) ? new FileBrowser[] { } :
                parent.ChildFolders.Select(f => new FileBrowser { Name = f.FolderName, Type = FileBrowserType.Directory });
        }

        private IEnumerable<FileBrowser> GetFolders(string path)
        {
            var folderInfo = GetDetailsByFolderPath(path);
            return GetFolders(folderInfo);
        }
        private IEnumerable<string> GetFolderNames(string path)
        {
            return path.Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar },
                              StringSplitOptions.RemoveEmptyEntries);
        }
        public int GenerateNewFolderId()
        {
            int newId = 1;
            var query = from u in DataContext.Get<DocumentFolder>() select u.FolderId;
            if (query.Any())
            {
                newId = query.Max() + 1;
            }
            return newId;
        }

        public bool HasFolderExisted(string folderName, string folderPath)
        {
            if (string.IsNullOrEmpty(folderName) || string.IsNullOrEmpty(folderPath)) return false;

            var query = DataContext.Get<DocumentFolder>().FirstOrDefault(
                p => p.FolderName.ToLower() == folderName.ToLower()
                && p.FolderPath.ToLower() == folderPath.ToLower());
            return (query != null);
        }
        public bool HasDataExisted(string folderName, int? parentId)
        {
            if (string.IsNullOrEmpty(folderName)) return false;
            var query = DataContext.Get<DocumentFolder>().FirstOrDefault(c => (parentId == null || c.ParentId == parentId) && (c.FolderName.ToUpper().Equals(folderName.ToUpper())));
            return (query != null);
        }
        public bool HasChild(int folderId)
        {
            var query = DataContext.Get<DocumentFolder>().FirstOrDefault(c => c.ParentId == folderId);
            return (query != null);
        }
        public int GetNewListOrder()
        {
            int listOrder = 1;
            var query = from u in DataContext.Get<DocumentFolder>() select (int)u.ListOrder;
            if (query.Any())
            {
                listOrder = query.Max() + 1;
            }
            return listOrder;
        }
        #endregion
    }
}
