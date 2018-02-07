using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Eagle.Common.Utilities;
using Eagle.Core.Configuration;
using Eagle.Core.Settings;
using Eagle.Entities.SystemManagement.FileStorage;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.SystemManagement
{
    public class DocumentFileRepository : RepositoryBase<DocumentFile>, IDocumentFileRepository
    {
        public HttpServerUtilityBase Server { get; set; }
        public DocumentFileRepository(IDataContext databaseContext) : base(databaseContext) { }
        
        public byte[] CreateThumbnail(string relativePath)
        {
            string physicalPath = HttpContext.Current.Server.MapPath(relativePath);
            using (var fileStream = File.OpenRead(physicalPath))
            {
                var desiredSize = new ImageSize
                {
                    Width = 80,
                    Height = 80
                };

                const string contentType = "image/png";
                var thumbnailCreator = new ImageThumbnailCreator();
                return thumbnailCreator.Create(fileStream, desiredSize, contentType);
            }
        }
        public string NormalizePath(string relativePath)
        {
            return string.IsNullOrEmpty(relativePath) ? FileUtils.ToAbsolutePath(GlobalSettings.UploadFolderRoot) : FileUtils.CombinePaths(FileUtils.ToAbsolutePath(GlobalSettings.UploadFolderRoot), relativePath);
        }
        public void DeleteFile(string physicalPath)
        {
            if (string.IsNullOrEmpty(physicalPath)) return;
            if (File.Exists(physicalPath))
            {
                File.Delete(physicalPath);
            }
        }
        public string UploadFile(string virtualPath, HttpPostedFileBase file, int? width = null, int? height = null)
        {
            string newFileName = string.Empty;
            if (string.IsNullOrEmpty(virtualPath)) return newFileName;
            string physicalPath = HttpContext.Current.Server.MapPath(virtualPath);
            if (!Directory.Exists(physicalPath))
            {
                throw new DirectoryNotFoundException();
            }

            if (file.ContentLength > 0)
            {
                string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                string fileExt = Path.GetExtension(file.FileName).ToLower().Trim();
                string fileNameEncode = FileUtils.GenerateEncodedFileNameWithDateSignature(fileName);
                newFileName = HttpContext.Current.Server.HtmlEncode($"{fileNameEncode}{fileExt}");
                string filePath = Path.Combine(physicalPath, newFileName);
                file.SaveAs(filePath);

                if (width != null && height != null)
                {
                    ImageUtils.ResizeImage(filePath, Convert.ToInt32(width), Convert.ToInt32(height));
                }
            }
            return newFileName;
        }

        public FileStream OpenReadFile(string virtualPath)
        {
            if (string.IsNullOrEmpty(virtualPath)) return null;
            string physicalPath = HttpContext.Current.Server.MapPath(virtualPath);
            if (!File.Exists(physicalPath)) return null;
            return File.OpenRead(physicalPath);
        }
      
        public DocumentInfo GetDetails(int fileId)
        {
            return (from file in DataContext.Get<DocumentFile>()
                    join folder in DataContext.Get<DocumentFolder>() on file.FolderId equals folder.FolderId
                    where file.FileId == fileId
                    select new DocumentInfo
                    {
                        ApplicationId = file.ApplicationId,
                        FileId = file.FileId,
                        FileTitle = file.FileTitle,
                        FileName = file.FileName,
                        FileDescription = file.FileDescription,
                        FileContent = file.FileContent,
                        FileExtension = file.FileExtension,
                        FileType = file.FileType,
                        StorageType = file.StorageType,
                        FolderId = file.FolderId,
                        FileUrl = folder.FolderPath + "/" + file.FileName,
                        Size = file.Size,
                        Width = file.Width,
                        Height = file.Height,
                        FolderPath = folder.FolderPath
                    }).FirstOrDefault();
        }
        public DocumentFile GetDetailsByFolderIdAndFileName(int folderId, string fileName)
        {
            return (from file in DataContext.Get<DocumentFile>()
                    join folder in DataContext.Get<DocumentFolder>() on file.FolderId equals folder.FolderId
                    where file.FolderId == folderId && file.FileName.ToLower() == fileName.ToLower()
                    select file).FirstOrDefault();
        }

        public string GetFileNameByFileId(int fileId)
        {
            return (from file in DataContext.Get<DocumentFile>()
                    where file.FileId == fileId
                    select file.FileName).FirstOrDefault();
        }
        public byte[] GetFileContentByFileId(int fileId)
        {
            return (from f in DataContext.Get<DocumentFile>() where f.FileId == fileId select f.FileContent).FirstOrDefault();
        }

        public string GetFileUrlByFileId(int fileId)
        {
            return (from file in DataContext.Get<DocumentFile>()
                    join folder in DataContext.Get<DocumentFolder>() on file.FolderId equals folder.FolderId
                    where file.FileId == fileId
                    select folder.FolderPath + "/" + file.FileName).FirstOrDefault();
        }
        public bool HasDataExisted(int folderId, string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return false;

            var query = (from file in DataContext.Get<DocumentFile>()
                         where file.FileName.ToLower().Contains(fileName.ToLower())
                             && file.FolderId == folderId
                         select file).FirstOrDefault();
            return (query != null);
        }
        public IEnumerable<DocumentInfo> GetList()
        {
            return (from file in DataContext.Get<DocumentFile>()
                    join folder in DataContext.Get<DocumentFolder>() on file.FolderId equals folder.FolderId into filelist
                    from filelst in filelist.DefaultIfEmpty()
                    select new DocumentInfo
                    {
                        ApplicationId = file.ApplicationId,
                        FileId = file.FileId,
                        FileTitle = file.FileTitle,
                        FileName = file.FileName,
                        FileDescription = file.FileDescription,
                        FileExtension = file.FileExtension,
                        FileType = file.FileType,
                        FolderId = file.FolderId,
                        Size = file.Size,
                        Width = file.Width,
                        Height = file.Height,
                        CreatedByUserId = file.CreatedByUserId,
                        CreatedDate = file.CreatedDate,
                        LastModifiedByUserId = file.LastModifiedByUserId,
                        LastModifiedDate = file.LastModifiedDate,
                        FolderPath = filelst.FolderPath
                    }).AsEnumerable();
        }   
        
        public SelectList PopulateStorageTypes(StorageType? selectedValue, bool? isShowSelectText = false)
        {
            var lst = (from StorageType x in Enum.GetValues(typeof(StorageType)).Cast<StorageType>()
                       select new SelectListItem
                       {
                           Text = x.ToString(),
                           Value = ((int)x).ToString(),
                           Selected = x.Equals(selectedValue)
                       }).ToList();

            if (lst.Any())
            {
                if (isShowSelectText!=null && isShowSelectText==true)
                    lst.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.Select} ---", Value = "" });
            }
            else
            {
                lst.Insert(0, new SelectListItem { Text = $"-- {LanguageResource.None} --", Value = "-1" });
            }
            return new SelectList(lst, "Value", "Text", selectedValue);
        }
        private IEnumerable<FileBrowser> GetFiles(DocumentFolderInfo parent)
        {
            return parent == null ? new FileBrowser[0] : parent.Files.Select(f => new FileBrowser { Name = f.FileName, Type = FileBrowserType.File });
        }
        private string NormalizePath(string path, string name)
        {
            path = VirtualPathUtility.AppendTrailingSlash(path).Replace("\\", "/").ToLower();
            path = path.Remove(path.LastIndexOf(name, StringComparison.Ordinal));
            return path;
        }

    }
}
