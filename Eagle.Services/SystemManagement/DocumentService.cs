using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Eagle.Common.Extensions;
using Eagle.Common.Settings;
using Eagle.Common.Utilities;
using Eagle.Core.Configuration;
using Eagle.Core.Permission;
using Eagle.Core.Settings;
using Eagle.Entities.Common;
using Eagle.Entities.SystemManagement.FileStorage;
using Eagle.Repositories;
using Eagle.Resources;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Contents.Documentation;
using Eagle.Services.Dtos.SystemManagement.FileStorage;
using Eagle.Services.EntityMapping.Common;
using Eagle.Services.SystemManagement.Validation;
using Eagle.Services.Validations;

namespace Eagle.Services.SystemManagement
{
    public class DocumentService : BaseService, IDocumentService
    {
        public DocumentService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        #region FOLDER

        public DocumentFolderDetail GetFolderDetailByFolderId(int folderId)
        {
            var entity = UnitOfWork.DocumentFolderRepository.GetDetailsByFolderId(folderId);
            return entity.ToDto<DocumentFolder, DocumentFolderDetail>();
        }
        public DocumentFolderDetail GetFolderDetailByFolderCode(Guid folderCode)
        {
            var entity = UnitOfWork.DocumentFolderRepository.GeDetailsByFolderCode(folderCode);
            return entity.ToDto<DocumentFolder, DocumentFolderDetail>();
        }
        public IEnumerable<TreeDetail> GetDocumentFolderTree(DocumentFolderStatus? status = null, int? selectedId = null, bool? isRootShowed = false)
        {
            var lst = UnitOfWork.DocumentFolderRepository.GetDocumentFolderTree(status, selectedId, isRootShowed);
            return lst.ToDtos<TreeEntity, TreeDetail>();
        }

        public DocumentFolderDetail InsertDocumentFolder(Guid applicationId, Guid userId, DocumentFolderEntry entry)
        {
            var violations = new List<RuleViolation>();
            bool isDuplicate = UnitOfWork.DocumentFolderRepository.HasFolderExisted(entry.FolderName, entry.FolderPath);
            if (isDuplicate)
            {
                violations.Add(new RuleViolation(ErrorCode.DuplicateDocumentFolder, "DocumentFolder", entry.FolderName));
                throw new ValidationError(violations);
            }

            var entity = new DocumentFolder
            {
                ApplicationId = applicationId,
                ParentId = entry.ParentId ?? 0,
                FolderId = UnitOfWork.DocumentFolderRepository.GenerateNewFolderId(),
                FolderCode = Guid.NewGuid(),
                FolderName = entry.FolderName ?? new DirectoryInfo(entry.FolderPath).Name,
                FolderIcon = entry.FolderIcon,
                FolderPath = entry.FolderPath,
                Description = entry.Description,
                IsActive = entry.IsActive,
                HasChild = false,
                ListOrder = UnitOfWork.DocumentFolderRepository.GetNewListOrder(),
                Ip = NetworkUtils.GetIP4Address(),
                CreatedDate = DateTime.UtcNow,
                CreatedByUserId = userId
            };

            UnitOfWork.DocumentFolderRepository.Insert(entity);
            UnitOfWork.SaveChanges();

            if (entry.ParentId != null && entry.ParentId > 0)
            {
                var parentEntity = UnitOfWork.DocumentFolderRepository.FindById(Convert.ToInt32(entry.ParentId));
                if (parentEntity == null) return null;

                parentEntity.HasChild = true;
                UnitOfWork.DocumentFolderRepository.Update(parentEntity);

                var lineage = $"{parentEntity.Lineage},{entity.FolderId}";
                entity.Lineage = lineage;
                entity.Depth = lineage.Split(',').Count();
                entity.ParentId = entry.ParentId;
            }
            else
            {
                entity.ParentId = 0;
                entity.Lineage = $"{entity.FolderId}";
                entity.Depth = 1;
            }

            return entity.ToDto<DocumentFolder, DocumentFolderDetail>();
        }
        public void UpdateDocumentFolder(Guid userId, DocumentFolderEditEntry entry)
        {
            string ip = NetworkUtils.GetIP4Address();

            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.DocumentFolderRepository.FindById(entry.FolderId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundForDocumentFolder, "DocumentFolder", entry.FolderId, ErrorMessage.Messages[ErrorCode.NotFoundForDocumentFolder]));
                throw new ValidationError(violations);
            }

            if (entity.FolderName != entry.FolderName)
            {
                bool isDuplicate = UnitOfWork.DocumentFolderRepository.HasDataExisted(entry.FolderName, entry.ParentId);
                if (isDuplicate)
                {
                    violations.Add(new RuleViolation(ErrorCode.DuplicateFolderName, "FolderName", entry.FolderName, ErrorMessage.Messages[ErrorCode.DuplicateFolderName]));
                    throw new ValidationError(violations);
                }
            }

            if (entry.ParentId != entity.ParentId && entry.ParentId != entity.FolderId)
            {
                if (entry.ParentId != null && entry.ParentId > 0)
                {
                    var children = UnitOfWork.DocumentFolderRepository.GetAllChildrenNodesOfSelectedNode(entry.FolderId).ToList();
                    if (children.Any())
                    {
                        var parentIds = children.Select(x => x.FolderId).ToList();
                        if (parentIds.Contains(Convert.ToInt32(entry.ParentId)))
                        {
                            violations.Add(new RuleViolation(ErrorCode.InvalidParentId, "ParentId", entry.ParentId, ErrorMessage.Messages[ErrorCode.InvalidParentId]));
                            throw new ValidationError(violations);
                        }
                    }

                    //Update parent entry
                    var parentEntryEntity = UnitOfWork.DocumentFolderRepository.FindById(Convert.ToInt32(entry.ParentId));
                    if (parentEntryEntity == null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.NotFoundParentId, "ParentId", entry.ParentId, ErrorMessage.Messages[ErrorCode.NotFoundParentId]));
                        throw new ValidationError(violations);
                    }
                    else
                    {
                        if (parentEntryEntity.HasChild == null || parentEntryEntity.HasChild == false)
                        {
                            parentEntryEntity.HasChild = true;
                            UnitOfWork.DocumentFolderRepository.Update(parentEntryEntity);
                        }
                    }

                    //Update parent entity
                    var parentEntity = UnitOfWork.DocumentFolderRepository.FindById(entity.ParentId);
                    if (parentEntity != null)
                    {
                        var childList = UnitOfWork.DocumentFolderRepository.GetAllChildrenNodesOfSelectedNode(Convert.ToInt32(entity.ParentId)).ToList();
                        if (childList.Any())
                        {
                            childList = childList.Where(x => (x.FolderId != entity.ParentId) && (x.FolderId != entity.FolderId)).ToList();
                            parentEntity.HasChild = childList.Any();
                            UnitOfWork.DocumentFolderRepository.Update(parentEntity);
                        }
                    }

                    var lineage = $"{parentEntryEntity.Lineage},{entry.FolderId}";
                    entity.Lineage = lineage;
                    entity.Depth = lineage.Split(',').Count();
                    entity.ParentId = entry.ParentId;
                }
                else
                {
                    entity.ParentId = 0;
                    entity.Lineage = $"{entry.FolderId}";
                    entity.Depth = 1;
                }
            }

            var hasChild = UnitOfWork.DocumentFolderRepository.HasChild(entity.FolderId);
            entity.HasChild = hasChild;
            entity.ParentId = entry.ParentId ?? 0;
            entity.FolderName = entry.FolderName ?? new DirectoryInfo(entry.FolderPath).Name;
            entity.FolderPath = entry.FolderPath;
            entity.FolderIcon = entry.FolderIcon;
            entity.Description = entry.Description;
            entity.IsActive = entry.IsActive;
            entity.LastUpdatedIp = ip;
            entity.LastModifiedByUserId = userId;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.DocumentFolderRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        private void CreateDirectory(DocumentFolder parent, string name)
        {
            var folderId = UnitOfWork.DocumentFolderRepository.GenerateNewFolderId();
            var listOrder = UnitOfWork.DocumentFolderRepository.GetNewListOrder();
            var path = VirtualPathUtility.AppendTrailingSlash(Path.Combine(parent.FolderPath, parent.FolderName));
            var item = new DocumentFolder { FolderId = folderId, FolderName = name, ParentId = parent.FolderId, FolderPath = path, ListOrder = listOrder };
            UnitOfWork.DocumentFolderRepository.Insert(item);
            UnitOfWork.SaveChanges();
        }
        public void UpdateDocumentFolderStatus(Guid userId, int folderId, DocumentFolderStatus status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.DocumentFolderRepository.FindById(folderId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundForDocumentFolder, "DocumentFolder", folderId, ErrorMessage.Messages[ErrorCode.NotFoundForDocumentFolder]));
                throw new ValidationError(violations);
            }
            if (entity.IsActive == status) return;

            entity.IsActive = status;
            entity.LastModifiedDate = DateTime.UtcNow;
            entity.LastModifiedByUserId = userId;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();

            UnitOfWork.DocumentFolderRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void DeleteDocumentFolder(int id)
        {
            var entity = UnitOfWork.DocumentFolderRepository.FindById(id);
            if (entity == null) return;

            UnitOfWork.DocumentFolderRepository.Delete(entity);
            UnitOfWork.SaveChanges();
        }
        private void DeleteDirectory(string physicalPath)
        {
            if (!UnitOfWork.DocumentPermissionRepository.AuthorizeDeleteDirectory(physicalPath))
            {
                throw new HttpException(403, "Forbidden");
            }
            UnitOfWork.DocumentFolderRepository.DeleteDirectory(physicalPath);
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region FILES

        public SelectList PopulateStorageTypes(StorageType? selectedValue = null, bool? isShowSelectText = false)
        {
            return UnitOfWork.DocumentFileRepository.PopulateStorageTypes(selectedValue, isShowSelectText);
        }

        private string ConvertImageFiltersToString(Guid applicationId)
        {
            int i = 0;
            var sb = new StringBuilder();
            string[] allowedFileExtensions = UnitOfWork.ApplicationSettingRepository.GetAllowedImageExtensions(applicationId);
            var count = allowedFileExtensions.Length;
            foreach (var item in allowedFileExtensions)
            {
                sb.AppendFormat(++i != count ? "*{0}," : "*{0}", item);
            }
            return sb.ToString();
        }
        private string ConvertFileFiltersToString(Guid applicationId)
        {
            int i = 0;
            var sb = new StringBuilder();
            string[] allowedFileExtensions = UnitOfWork.ApplicationSettingRepository.GetAllowedFileExtensions(applicationId);
            var count = allowedFileExtensions.Length;
            foreach (var item in allowedFileExtensions)
            {
                sb.AppendFormat(++i != count ? "*{0}," : "*{0}", item);
            }
            return sb.ToString();
        }

        public IEnumerable<FileBrowserDetail> GetImageBrowser(Guid applicationId, string virtualPath, FileBrowserType? fileType = null)
        {
            if (!UnitOfWork.DocumentPermissionRepository.AuthorizeRead(virtualPath))
            {
                throw new HttpException(403, "Forbidden");
            }

            //string extensions = GlobalSettings.DefaultImageFilter;
            string extensions = ConvertImageFiltersToString(applicationId);
            var lst = UnitOfWork.DocumentFolderRepository.GetContent(virtualPath, extensions).
                   Where(x => fileType == null || x.Type == fileType)
                   .Select(f => new FileBrowserDetail
                   {
                       name = f.Name,
                       type = f.Type == FileBrowserType.File ? "f" : "d",
                       size = f.Size
                   }).AsEnumerable();
            return lst;
        }

        public IEnumerable<FileBrowserDetail> GetFileBrowser(Guid applicationId, string virtualPath, FileBrowserType? fileType = null)
        {
            if (!UnitOfWork.DocumentPermissionRepository.AuthorizeRead(virtualPath))
            {
                throw new HttpException(403, "Forbidden");
            }
            //string extensions = GlobalSettings.DefaultFileFilter;
            string extensions = ConvertFileFiltersToString(applicationId);
            var lst = UnitOfWork.DocumentFolderRepository.GetContent(virtualPath, extensions).
                   Where(x => (fileType == null || x.Type == fileType))
                   .Select(f => new FileBrowserDetail
                   {
                       name = f.Name,
                       type = f.Type == FileBrowserType.File ? "f" : "d",
                       size = f.Size
                   }).AsEnumerable();
            return lst;
        }
        public bool CheckImageTypeValid(Guid applicationId, HttpPostedFileBase fileUpload, out string errorMessage)
        {
            if (fileUpload == null)
            {
                errorMessage = LanguageResource.InvalidFileUpload;
                return false;
            }
            else
            {
                string extension = fileUpload.ContentType;
                //  string[] validImageTypes = GlobalSettings.DefaultValidImageTypes;
                string[] validImageTypes = UnitOfWork.ApplicationSettingRepository.GetAllowedFileExtensions(applicationId);
                if (!validImageTypes.Contains(extension))
                {
                    errorMessage = LanguageResource.FileUploadInvalid;
                    return false;
                }
                errorMessage = "";
                return true;
            }
        }
        public bool IsImage(Guid applicationId, string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return false;
            string extension = Path.GetExtension(filePath);

            //string[] validImageTypes = GlobalSettings.DefaultValidImageTypes;
            string[] allowedImageExtensions = UnitOfWork.ApplicationSettingRepository.GetAllowedImageExtensions(applicationId);
            if (!string.IsNullOrEmpty(extension) && allowedImageExtensions.Any(e => e.EndsWith(extension, StringComparison.InvariantCultureIgnoreCase)))
            {
                return true;
            }
            return false;
        }
        public byte[] CreateThumbnail(string relativePath)
        {
            if (!UnitOfWork.DocumentPermissionRepository.AuthorizeThumbnail(relativePath))
            {
                throw new HttpException(403, "Forbidden");
            }
            return UnitOfWork.DocumentFileRepository.CreateThumbnail(relativePath);

        }
        public void Destroy(string basePath, string relativeName, string type)
        {
            var result = !string.IsNullOrEmpty(relativeName) && !string.IsNullOrEmpty(type);
            if (!result)
            {
                throw new HttpException(404, "File Not Found");
            }

            string path = FileUtils.CombinePaths(basePath, relativeName);
            if (type.ToLowerInvariant() == "f")
            {
                DeleteFile(path);
            }
            else
            {
                DeleteDirectory(path);
            }
        }
        public void CreateDirectory(string basePath, FileBrowserEntry entry)
        {
            var name = entry.Name;
            var result = !string.IsNullOrEmpty(name) &&
                         UnitOfWork.DocumentPermissionRepository.AuthorizeCreateDirectory(basePath, name);
            if (!result)
            {
                throw new HttpException(403, "Forbidden");
            }

            UnitOfWork.DocumentFolderRepository.CreateFolder(basePath, name);
        }
        private void DeleteFile(string physicalPath)
        {
            if (!UnitOfWork.DocumentPermissionRepository.AuthorizeDeleteFile(physicalPath))
            {
                throw new HttpException(403, "Forbidden");
            }
            UnitOfWork.DocumentFileRepository.DeleteFile(physicalPath);
        }
        public string Upload(string path, HttpPostedFileBase file)
        {
            var violations = new List<RuleViolation>();
            if (!UnitOfWork.DocumentPermissionRepository.AuthorizeUpload(path, file))
            {
                violations.Add(new RuleViolation(ErrorCode.UnauthorizedUpload, "Forbidden", path));
                throw new ValidationError(violations);
            }

            string physicalPath = HttpContext.Current.Server.MapPath(path);
            if (!Directory.Exists(physicalPath))
            {
                violations.Add(new RuleViolation(ErrorCode.DirectoryNotFoundException, "404", path));
                throw new ValidationError(violations);
            }
            return UnitOfWork.DocumentFileRepository.UploadFile(path, file);
        }
        public DocumentFileDetail UploadAndSaveDbByPath(Guid applicationId, Guid? userId, string virtualPath, HttpPostedFileBase file, StorageType storageType, int? width = null, int? height = null, string description = null, string source = null)
        {
            if (file == null || file.ContentLength <= 0) return null;

            var violations = new List<RuleViolation>();
            var folderEntity = UnitOfWork.DocumentFolderRepository.GetDetailsByFolderPath(virtualPath);
            if (folderEntity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundForDocumentFolder, "DocumentFolder"));
                throw new ValidationError(violations);
            }

            UnitOfWork.DocumentFileRepository.UploadFile(virtualPath, file, width ?? GlobalSettings.DefaultImageWidth,
                height ?? GlobalSettings.DefaultImagelHeight);

            string fileName = Path.GetFileNameWithoutExtension(file.FileName);
            string fileExt = (file.FileName != null) ? Path.GetExtension(file.FileName).ToLower().Trim() : ".png";
            string newFileName = FileUtils.GenerateEncodedFileNameWithDateSignature(fileName);
            string newFileNameEncoded = HttpContext.Current.Server.HtmlEncode(newFileName + fileExt);

            var fileEntry = new DocumentFileEntry
            {
                FileName = newFileNameEncoded,
                FileTitle = fileName,
                FolderId = folderEntity.FolderId,
                StorageType = storageType,
                FileDescription = description,
                FileSource = source
            };

            return InsertFile(applicationId, userId, fileEntry);
        }
        public DocumentFileDetail UploadAndSaveDbByFolderId(Guid applicationId, Guid? userId, HttpPostedFileBase file, int folderId, StorageType storageType, int? width = null, int? height = null, string description = null, string source = null)
        {
            var violations = new List<RuleViolation>();
            if (file == null || file.ContentLength <= 0)
            {
                violations.Add(new RuleViolation(ErrorCode.NullReferenceForFileUpload, "file", folderId, ErrorMessage.Messages[ErrorCode.NullReferenceForFileUpload]));
                throw new ValidationError(violations);
            }

            var folderEntity = UnitOfWork.DocumentFolderRepository.FindById(folderId);
            if (folderEntity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundForDocumentFolder, "DocumentFolder", folderId));
                throw new ValidationError(violations);
            }

            string virtualPath = "~" + folderEntity.FolderPath;
            string newFileName = UnitOfWork.DocumentFileRepository.UploadFile(virtualPath, file, width ?? GlobalSettings.DefaultImageWidth,
                height ?? GlobalSettings.DefaultImagelHeight);

            var fileEntry = new DocumentFileEntry
            {
                FileName = newFileName,
                FileTitle = Path.GetFileNameWithoutExtension(file.FileName),
                FolderId = folderEntity.FolderId,
                StorageType = storageType,
                FileDescription = description,
                FileSource = source
            };

            return InsertFile(applicationId, userId, fileEntry);
        }
        public DocumentFileDetail[] UploadAndSaveWithThumbnail(Guid applicationId, Guid? userId, HttpPostedFileBase file, int folderId, StorageType storageType, int? width = null, int? height = null, string description = null, string source = null)
        {
            var violations = new List<RuleViolation>();
            if (file == null || file.ContentLength <= 0)
            {
                violations.Add(new RuleViolation(ErrorCode.NullReferenceForFileUpload, "file", folderId, ErrorMessage.Messages[ErrorCode.NullReferenceForFileUpload]));
                throw new ValidationError(violations);
            }

            var folderEntity = UnitOfWork.DocumentFolderRepository.FindById(folderId);
            if (folderEntity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundForDocumentFolder, "DocumentFolder", folderId));
                throw new ValidationError(violations);
            }

            string virtualPath = "~" + folderEntity.FolderPath;
            string[] fileNames = FileUtils.UploadFileWithThumbnail(file, virtualPath, width ?? GlobalSettings.DefaultImageWidth,
                height ?? GlobalSettings.DefaultImagelHeight);

            var largePhotoFileEntry = new DocumentFileEntry
            {
                FileName = fileNames[0],
                FileTitle = Path.GetFileNameWithoutExtension(file.FileName),
                FolderId = folderId,
                StorageType = storageType,
                FileDescription = description,
                FileSource = source
            };

            var smallPhotoFileEntry = new DocumentFileEntry
            {
                FileName = fileNames[1],
                FileTitle = $"{Path.GetFileNameWithoutExtension(file.FileName)}_thumb",
                FolderId = folderId,
                StorageType = storageType,
                FileDescription = description,
                FileSource = source
            };

            DocumentFileDetail[] results = new DocumentFileDetail[2];
            results[0] = InsertFile(applicationId, userId, largePhotoFileEntry);
            results[1] = InsertFile(applicationId, userId, smallPhotoFileEntry);

            UnitOfWork.SaveChanges();
            return results;
        }
        public FileStream OpenReadFile(string virtualPath)
        {
            if (!UnitOfWork.DocumentPermissionRepository.AuthorizeFile(virtualPath))
            {
                throw new HttpException(403, "Forbidden");
            }
            return UnitOfWork.DocumentFileRepository.OpenReadFile(virtualPath);

        }
        public string GetList(string fileIds)
        {
            string json = string.Empty;
            if (!string.IsNullOrEmpty(fileIds))
            {
                List<DocumentInfo> lst = new List<DocumentInfo>();
                List<string> idList = fileIds.Split(',').Reverse().ToList();
                if (idList.Count > 0)
                {
                    lst.AddRange(idList.Select(id => UnitOfWork.DocumentFileRepository.GetDetails(Convert.ToInt32(id))));
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    json = serializer.Serialize(lst);
                }
            }
            return json;
        }
        public List<DocumentInfo> GetDownloadFileList(string fileIdList)
        {
            if (string.IsNullOrEmpty(fileIdList)) return null;
            List<DocumentInfo> lst = new List<DocumentInfo>();
            List<string> idList = fileIdList.Split(',').Reverse().ToList();
            if (idList.Count > 0)
            {
                lst.AddRange(idList.Select(id => UnitOfWork.DocumentFileRepository.GetDetails(Convert.ToInt32(id))));
            }
            return lst;
        }
        public string GenerateDownloadLink(int fileId)
        {
            string downloadFileLink = "";
            var entity = UnitOfWork.DocumentFileRepository.GetDetails(fileId);
            if (entity != null)
            {
                string physicalFolderPath = HttpContext.Current.Server.MapPath("~" + entity.FolderPath);
                string fileTitle = entity.FileTitle;
                //string fileName = entity.FileName;
                string fileUrl = entity.FileUrl;
                Uri requestUri = HttpContext.Current.Request.Url;
                string baseUrl = requestUri.Scheme + Uri.SchemeDelimiter + requestUri.Host + (requestUri.IsDefaultPort ? "" : ":" + requestUri.Port);

                DirectoryInfo dirInfo = new DirectoryInfo(physicalFolderPath);
                if (!dirInfo.Exists)
                    dirInfo.Create();

                if (string.IsNullOrEmpty(fileUrl)) return GlobalSettings.DefaultFileUrl;

                var physicalFilePath = HttpContext.Current.Server.MapPath(fileUrl);
                FileInfo fileInfo = new FileInfo(physicalFilePath);
                if (fileInfo.Exists)
                {
                    // string dowloadText = LanguageResource.DownloadFile;
                    string extension = fileInfo.Extension;
                    //string[] ValidImageTypes = HostSettings.ExensionImage.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    //  string[] validImageTypes = GlobalSettings.DefaultFileTypes;

                    string[] validImageTypes = UnitOfWork.ApplicationSettingRepository.GetAllowedFileExtensions(entity.ApplicationId);
                    bool isValidImageFile = validImageTypes.Any(t => extension == t);
                    if (!isValidImageFile)
                    {
                        downloadFileLink = "<span><a data-id='" + fileId + "' href=\"" + baseUrl + "/Handlers/DownloadFile.ashx?file=" + fileUrl + "\">" + fileTitle + "</a></span>";
                    }
                }
                else
                {
                    //DownloadFileLink = "<span>File Url doesn't exist</span>";
                    fileInfo.Create();
                }
            }
            else
            {
                downloadFileLink = "<span>No File exists</span>";
            }
            return downloadFileLink;
        }
        public string CreateDownloadLink(int fileId)
        {
            string downloadFileLink = "";
            var entity = UnitOfWork.DocumentFileRepository.GetDetails(fileId);
            if (entity != null)
            {
                string physicalFolderPath = HttpContext.Current.Server.MapPath("~" + entity.FolderPath);
                // string fileName = entity.FileName;
                string fileUrl = entity.FileUrl;
                Uri requestUri = HttpContext.Current.Request.Url;
                string baseUrl = requestUri.Scheme + Uri.SchemeDelimiter + requestUri.Host + (requestUri.IsDefaultPort ? "" : ":" + requestUri.Port);

                DirectoryInfo dirInfo = new DirectoryInfo(physicalFolderPath);
                if (!dirInfo.Exists)
                    dirInfo.Create();
                else
                {
                    if (string.IsNullOrEmpty(fileUrl)) return null;

                    var physicalPath = HttpContext.Current.Server.MapPath(fileUrl);
                    FileInfo fileInfo = new FileInfo(physicalPath);
                    if (fileInfo.Exists)
                    {
                        string dowloadText = LanguageResource.DownloadFile;
                        string extension = fileInfo.Extension;
                        string[] validImageTypes = SystemSettings.FileTypes;
                        bool isValidImageFile = false;
                        for (int i = 0; i < validImageTypes.Length; i++)
                        {
                            if (extension == validImageTypes[i])
                            {
                                isValidImageFile = true;
                                break;
                            }
                        }
                        if (!isValidImageFile)
                        {
                            downloadFileLink = "<span><a data-id='" + fileId + "' href=\"" + baseUrl + "/Handlers/DownloadFile.ashx?file=" + fileId + "\">" + dowloadText + "</a></span>";
                        }
                    }
                    else
                    {
                        //DownloadFileLink = "<span>File Url doesn't exist</span>";
                        fileInfo.Create();
                    }
                }
            }
            return downloadFileLink;
        }
        public List<FileUploadDetail> GetDownloadFileList(int itemId, string itemTag, string fileIds)
        {
            List<FileUploadDetail> lst = new List<FileUploadDetail>();
            if (!string.IsNullOrEmpty(fileIds))
            {
                List<string> idList = fileIds.Split(',').Reverse().ToList();
                if (idList.Count > 0)
                {
                    foreach (var id in idList)
                    {
                        var entity = UnitOfWork.DocumentFileRepository.GetDetails(Convert.ToInt32(id));
                        var item = new FileUploadDetail
                        {
                            DocumentFileInfo = entity,
                            ItemId = itemId,
                            ItemTag = itemTag
                        };
                        lst.Add(item);
                    }
                }
            }
            return lst;
        }
        public DocumentationLink CreateLinks(int fileId, bool? isOnlyPath)
        {
            Uri requestUri = HttpContext.Current.Request.Url;
            string baseUrl = requestUri.Scheme + Uri.SchemeDelimiter + requestUri.Host + (requestUri.IsDefaultPort ? "" : ":" + requestUri.Port);
            string downloadLink = "", viewLink = "";
            var violations = new List<RuleViolation>();
            var item = GetFileInfoDetail(fileId);
            if (item == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundForDocumentFile, "FileId", fileId, ErrorMessage.Messages[ErrorCode.NotFoundForDocumentFile]));
                throw new ValidationError(violations);
            }

            if (isOnlyPath == true)
            {
                downloadLink = baseUrl + "/Handlers/DownloadFile.ashx?file=" + fileId;
                viewLink = baseUrl + "/Handlers/PreviewFile.ashx?file=" + fileId;
            }
            else
            {
                string fileTitle = item.FileTitle;
                downloadLink = "<span><a data-id='" + fileId + "' href=\"" + baseUrl + "/Handlers/DownloadFile.ashx?file=" + fileId + "\">" + fileTitle + "</a></span>";
                viewLink = "<span><a data-id='" + fileId + "' href=\"" + baseUrl + "/Handlers/PreviewFile.ashx?file=" + fileId + "\">" + fileTitle + "</a></span>";
            }

            var documentationLink = new DocumentationLink
            {
                FileInfo = item,
                DownloadLink = downloadLink,
                ViewLink = viewLink
            };

            return documentationLink;
        }
        public DocumentFileDetail GetFileDetail(int fileId)
        {
            var entity = UnitOfWork.DocumentFileRepository.FindById(fileId);
            return entity.ToDto<DocumentFile, DocumentFileDetail>();
        }
        public DocumentInfoDetail GetFileInfoDetail(int fileId)
        {
            if (fileId <= 0) return new DocumentInfoDetail
            {
                FileUrl = GlobalSettings.DefaultFileUrl,
                FolderPath = GlobalSettings.DefaultImageFolderPath,
                StorageType = StorageType.Local
            };

            var entity = UnitOfWork.DocumentFileRepository.GetDetails(fileId);
            if (entity == null) return new DocumentInfoDetail();

            string fileUrl = entity.FileUrl;
            if (entity.StorageType == StorageType.Local)
            {
                string filePath = HttpContext.Current.Server.MapPath("~" + entity.FileUrl);
                FileInfo fileInfo = new FileInfo(filePath);
                if (!fileInfo.Exists)
                {
                    //var fileContent = fileEntity.FileContent;
                    //ImageUtils.CreateFileFromByteArray(fileName, folderPath, fileContent);

                    var isImage = ImageUtils.IsValidImage(filePath);
                    fileUrl = isImage ? GlobalSettings.NotFoundImageUrl : GlobalSettings.NotFoundVideoUrl;
                }
            }
            
            var documentInfo = new DocumentInfoDetail
            {
                ApplicationId = entity.ApplicationId,
                FileId = entity.FileId,
                FolderId = Convert.ToInt32(entity.FolderId),
                FileCode = entity.FileCode,
                FileTitle = entity.FileTitle,
                FileName = entity.FileName,
                FileExtension = entity.FileExtension,
                FileVersion = entity.FileVersion,
                FileContent = entity.FileContent,
                FileType = entity.FileType,
                FileDescription = entity.FileDescription,
                FileSource = entity.FileSource,
                StorageType = entity.StorageType,
                Size = entity.Size,
                Width = entity.Width,
                Height = entity.Height,
                ClickThroughs = entity.ClickThroughs,
                FolderPath = entity.FolderPath,
                FileUrl = fileUrl,
                IsActive = entity.IsActive
            };

            return documentInfo;
        }
        public DocumentFileDetail GetFileDetailByFilePath(string filePath)
        {
            var fileName = Path.GetFileName(filePath);
            var folder = UnitOfWork.DocumentFolderRepository.GetDetailsByFolderPath(Path.GetDirectoryName(filePath));
            var file = UnitOfWork.DocumentFileRepository.GetDetailsByFolderIdAndFileName(folder.FolderId, fileName);
            return file.ToDto<DocumentFile, DocumentFileDetail>();
        }
        public string GetFilePath(int? fileId)
        {
            if (fileId == null) return GlobalSettings.DefaultFileUrl;
            var fileEntity = UnitOfWork.DocumentFileRepository.FindById(Convert.ToInt32(fileId));
            if (fileEntity == null) return GlobalSettings.DefaultFileUrl;
            var fileName = fileEntity.FileName;

            var folderEntity = UnitOfWork.DocumentFolderRepository.FindById(fileEntity.FolderId);
            if (string.IsNullOrEmpty(folderEntity?.FolderPath)) return null;

            var folderPath = "~" + folderEntity.FolderPath;
            var physicalFolderPath = HttpContext.Current.Server.MapPath(folderPath);
            DirectoryInfo dirInfo = new DirectoryInfo(physicalFolderPath);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            string fileUrl = folderEntity.FolderPath + "/" + fileName;
            if (fileEntity.StorageType == StorageType.Local)
            {
                string filePath = HttpContext.Current.Server.MapPath("~" + fileUrl);
                FileInfo fileInfo = new FileInfo(filePath);
                if (!fileInfo.Exists)
                {
                    //var fileContent = fileEntity.FileContent;
                    //ImageUtils.CreateFileFromByteArray(fileName, folderPath, fileContent);

                    var isImage = ImageUtils.IsValidImage(filePath);
                    fileUrl = isImage ? GlobalSettings.NotFoundImageUrl : GlobalSettings.NotFoundVideoUrl;
                }
            }
            return fileUrl;
        }
        public DocumentFileDetail InsertFile(Guid applicationId, Guid? userId, DocumentFileEntry entry)
        {
            ISpecification<DocumentFileEntry> validator = new DocumentFileEntryValidator(UnitOfWork, PermissionLevel.Create);
            var dataViolations = new List<RuleViolation>();
            var isValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isValid) throw new ValidationError(dataViolations);

            var entity = new DocumentFile
            {
                ApplicationId = applicationId,
                FileCode = entry.FileCode ?? Guid.NewGuid().ToString(),
                FileName = entry.FileName,
                FileTitle = entry.FileTitle ?? Path.GetFileNameWithoutExtension(entry.FileName),
                FileDescription = entry.FileDescription,
                Width = entry.Width ?? ImageSettings.ImageWidthVga,
                Height = entry.Height ?? ImageSettings.ImageHeightVga,
                FileSource = entry.FileSource,
                StorageType = entry.StorageType,
                FolderId = entry.FolderId,
                IsActive = DocumentFileStatus.Published,
                CreatedByUserId = userId,
                CreatedDate = DateTime.UtcNow,
                Ip = NetworkUtils.GetIP4Address()
            };

            if (entry.StorageType == StorageType.Local)
            {
                var folderEntity = UnitOfWork.DocumentFolderRepository.FindById(entry.FolderId);
                string folderPath = "~" + folderEntity.FolderPath;
                string physicalDirPath = HttpContext.Current.Server.MapPath(folderPath);
                if (!Directory.Exists(physicalDirPath))
                    Directory.CreateDirectory(physicalDirPath);
                string filePath = Path.Combine(physicalDirPath, entry.FileName);

                var fileInfo = new FileInfo(filePath);
                if (!fileInfo.Exists) return null;

                int? fileSize = Convert.ToInt32(fileInfo.Length);
                string fileExtenstion = fileInfo.Extension;
                string fileType = MimeMapping.GetMimeMapping(entry.FileName);
                byte[] fileContent = FileUtils.ReadAndProcessLargeFile(filePath);

                entity.FileContent = fileContent;
                entity.FileType = fileType;
                entity.FileExtension = fileExtenstion;
                entity.Size = fileSize;
            }
            else
            {
                var fileSource = entry.FileSource;
                if (!string.IsNullOrEmpty(fileSource))
                {
                    string filetype = fileSource.Substring(fileSource.LastIndexOf(".", StringComparison.Ordinal) + 1, (fileSource.Length - fileSource.LastIndexOf(".", StringComparison.Ordinal) - 1));
                    string filename = fileSource.Substring(fileSource.LastIndexOf("/", StringComparison.Ordinal) + 1, (fileSource.Length - fileSource.LastIndexOf("/", StringComparison.Ordinal) - 1));
                    var request = (HttpWebRequest)WebRequest.Create(fileSource);
                    var response = (HttpWebResponse)request.GetResponse();
                    if (long.TryParse(response.Headers.Get("Content-Length"), out var contentLength))
                    {
                        long fileSize;
                        if (contentLength >= 1073741824)
                        {
                            fileSize = contentLength / 1073741824;
                        }
                        else if (contentLength >= 1048576)
                        {
                            fileSize = contentLength / 1048576;
                        }
                        else
                        {
                            fileSize = contentLength / 1024;
                        }

                        entity.FileSource = fileSource;
                        entity.FileName = filename;
                        entity.FileExtension = Path.GetExtension(filename);
                        entity.FileType = filetype;
                        entity.Size = Convert.ToInt32(fileSize);
                    }
                }
            }

            UnitOfWork.DocumentFileRepository.Insert(entity);
            UnitOfWork.SaveChanges();

            return entity.ToDto<DocumentFile, DocumentFileDetail>();
        }
        public void UpdateFile(Guid applicationId, Guid? userId, DocumentFileEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (string.IsNullOrEmpty(entry.FileName))
            {
                violations.Add(new RuleViolation(ErrorCode.NullFileName, "FileName", null, ErrorMessage.Messages[ErrorCode.NullFileName]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.FileName.Length > 250)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidFileName, "FileName", null, ErrorMessage.Messages[ErrorCode.InvalidFileName]));
                    throw new ValidationError(violations);
                }
            }

            var entity = UnitOfWork.DocumentFileRepository.FindById(entry.FileId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundForDocumentFile, "FileId", entry.FileId, ErrorMessage.Messages[ErrorCode.NotFoundForDocumentFile]));
                throw new ValidationError(violations);
            }

            entity.FileCode = entry.FileCode;
            entity.FileName = entry.FileName;
            entity.FileTitle = entry.FileTitle;
            entity.FileDescription = entry.FileDescription;
            entity.FileSource = entry.FileSource;
            entity.Width = entry.Width;
            entity.Height = entry.Height;
            entity.FolderId = entry.FolderId;
            entity.StorageType = entry.StorageType;
            entity.LastModifiedDate = DateTime.UtcNow;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedByUserId = userId ?? Guid.Parse(GlobalSettings.DefaultUserId);
           
            if (entry.StorageType == StorageType.Local)
            {
                var folderEntity = UnitOfWork.DocumentFolderRepository.FindById(entry.FolderId);
                if (folderEntity == null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NotFoundForDocumentFolder, "DocumentFolder",
                        entry.FolderId,
                        ErrorMessage.Messages[ErrorCode.NotFoundForDocumentFolder]));
                    throw new ValidationError(violations);
                }

                string newFilePath = "~" + folderEntity.FolderPath + "/" + entry.FileName;
                string physicalDirPath = HttpContext.Current.Server.MapPath(folderEntity.FolderPath);
                if (Directory.Exists(physicalDirPath))
                {
                    string physicalNewFilePath = HttpContext.Current.Server.MapPath(newFilePath);
                    var fileInfo = new FileInfo(physicalNewFilePath);
                    entity.Size = Convert.ToInt32(fileInfo.Length);

                    var fileContent = FileUtils.ReadAndProcessLargeFile(newFilePath);
                    if (fileContent != null && fileContent.Length > 0)
                    {
                        string originalFileName = entity.FileName;
                        int? originalFolderId = entity.FolderId;
                        if (originalFolderId != null)
                        {
                            string foldePath =
                                UnitOfWork.DocumentFolderRepository.GetFolderPathByFolderId(
                                    Convert.ToInt32(originalFolderId));
                            string originalFolderPath = "~" + foldePath;
                            string physicalOriginalFolderPath =
                                HttpContext.Current.Server.MapPath(originalFolderPath);
                            if (!Directory.Exists(physicalOriginalFolderPath))
                                Directory.CreateDirectory(physicalOriginalFolderPath);
                            string physicalOldFilePath = Path.Combine(physicalOriginalFolderPath, originalFileName);
                            if (physicalOldFilePath != string.Empty && File.Exists(physicalOldFilePath))
                                File.Delete(physicalOldFilePath);
                        }
                        entity.FileContent = fileContent;
                    }

                    if (entity.FileName == entry.FileName && entity.FolderId == entry.FolderId)
                    {
                        var fileVersionInfo = FileVersion.GetFileVersion(entity.FileName);
                        if (fileVersionInfo != null)
                        {
                            entity.FileVersion = fileVersionInfo.Version;
                        }
                    }

                    entity.FileType = MimeMapping.GetMimeMapping(entry.FileName);
                }
            }
            else
            {
                var fileSource = entry.FileSource;
                if (string.IsNullOrEmpty(fileSource))
                {
                    violations.Add(new RuleViolation(ErrorCode.NullFileSource, "FileSource", fileSource, ErrorMessage.Messages[ErrorCode.NullFileSource]));
                    throw new ValidationError(violations);
                }

                string filetype = fileSource.Substring(fileSource.LastIndexOf(".", StringComparison.Ordinal) + 1, (fileSource.Length - fileSource.LastIndexOf(".", StringComparison.Ordinal) - 1));
                string filename = fileSource.Substring(fileSource.LastIndexOf("/", StringComparison.Ordinal) + 1, (fileSource.Length - fileSource.LastIndexOf("/", StringComparison.Ordinal) - 1));
                var request = (HttpWebRequest)WebRequest.Create(fileSource);
                var response = (HttpWebResponse)request.GetResponse();
                if (long.TryParse(response.Headers.Get("Content-Length"), out var contentLength))
                {
                    long fileSize;
                    if (contentLength >= 1073741824)
                    {
                        fileSize = contentLength / 1073741824;
                    }
                    else if (contentLength >= 1048576)
                    {
                        fileSize = contentLength / 1048576;
                    }
                    else
                    {
                        fileSize = contentLength / 1024;
                    }
                    
                    entity.Size = Convert.ToInt32(fileSize);
                }

                entity.FileSource = fileSource;
                entity.FileName = filename;
                entity.FileExtension = Path.GetExtension(filename);
                entity.FileType = filetype;
            }

            UnitOfWork.DocumentFileRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateFileStatus(Guid? userId, int fileId, DocumentFileStatus status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.DocumentFileRepository.FindById(fileId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundForDocumentFile, "FileId", fileId, ErrorMessage.Messages[ErrorCode.NotFoundForDocumentFile]));
                throw new ValidationError(violations);
            }
            if (entity.IsActive == status) return;

            entity.IsActive = status;
            entity.LastModifiedDate = DateTime.UtcNow;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedByUserId = userId ?? Guid.Parse(GlobalSettings.DefaultUserId);

            UnitOfWork.DocumentFileRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void DeleteFile(int id)
        {
            var entity = UnitOfWork.DocumentFileRepository.FindById(id);
            if (entity != null)
            {
                if (entity.FolderId != null)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    string folderPath = UnitOfWork.DocumentFolderRepository.GetFolderPathByFolderId(Convert.ToInt32(entity.FolderId));
                    string physicalFilePath = HttpContext.Current.Server.MapPath("~/" + folderPath + "/" + entity.FileName);
                    if (File.Exists(physicalFilePath))
                    {
                        File.Delete(physicalFilePath);
                    }
                }
                
                UnitOfWork.DocumentFileRepository.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteFiles(List<int> ids)
        {
            if (ids == null) return;
            var dataViolations = new List<RuleViolation>();
            foreach (var id in ids)
            {
                var entity = UnitOfWork.DocumentFileRepository.FindById(id);
                if (entity == null)
                {
                    dataViolations.Add(new RuleViolation(ErrorCode.NotFoundForDocumentFile, "DocumentFile", id));
                    throw new ValidationError(dataViolations);
                }

                if (entity.FolderId != null)
                {
                    string folderPath = UnitOfWork.DocumentFolderRepository.GetFolderPathByFolderId(Convert.ToInt32(entity.FolderId));
                    string physicalFilePath = HttpContext.Current.Server.MapPath("~/" + folderPath + "/" + entity.FileName);
                    File.Delete(physicalFilePath);
                }
                
                UnitOfWork.DocumentFileRepository.Delete(entity);
            }
            UnitOfWork.SaveChanges();
        }
        public void SaveImage(Guid? userId, DocumentFolder parent, HttpPostedFileBase file)
        {
            var buffer = new byte[file.InputStream.Length];
            file.InputStream.Read(buffer, 0, (int)file.InputStream.Length);
            var entity = new DocumentFile
            {
                FileName = Path.GetFileName(file.FileName),
                FileTitle = Path.GetFileNameWithoutExtension(file.FileName),
                FileExtension = Path.GetExtension(file.FileName),
                FileVersion = FileVersion.GetFileVersion(file.FileName).Version,
                FileType = FileUtils.GetMimeType(file.FileName),
                FolderId = parent.FolderId,
                FileContent = buffer,
                StorageType = StorageType.Local,
                CreatedByUserId = userId,
                CreatedDate = DateTime.UtcNow
            };
            UnitOfWork.DocumentFileRepository.Insert(entity);
            UnitOfWork.SaveChanges();

            // TODO: should insert Document as well
            //var document = new Document()
            //{
            //    FileId = entity.FileId,
            //    VendorId = vendorId,
            //    Status = DocumentStatus.Active,
            //    CreatedByUserId = userId,
            //    CreatedDate = DateTime.UtcNow,
            //    Ip = ipAddress,
            //};
            //UnitOfWork.DocumentRepository.Insert(document);
            //UnitOfWork.SaveChanges();
        }
        public void UploadAndUpdateFile(Guid? userId, int fileId, HttpPostedFileBase file, int? width = null, int? height = null)
        {
            if (file == null || file.ContentLength <= 0) return;

            var dataViolations = new List<RuleViolation>();
            var entity = UnitOfWork.DocumentFileRepository.FindById(fileId);
            if (entity == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NotFoundForDocumentFile, "DocumentFile", fileId));
                throw new ValidationError(dataViolations);
            }

            var folderEntity = UnitOfWork.DocumentFolderRepository.FindById(entity.FolderId);
            if (folderEntity == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NotFoundForDocumentFolder, "DocumentFolder", entity.FolderId));
                throw new ValidationError(dataViolations);
            }

            //Upload file to folder path
            string originalFolderPath = "~" + folderEntity.FolderPath;
            string physicalOriginalFolderPath = HttpContext.Current.Server.MapPath(originalFolderPath);
            if (!Directory.Exists(physicalOriginalFolderPath))
                Directory.CreateDirectory(physicalOriginalFolderPath);
            string physicalFilePath = Path.Combine(physicalOriginalFolderPath, entity.FileName);
            File.Delete(physicalFilePath);

            UnitOfWork.DocumentFileRepository.UploadFile(originalFolderPath, file, width ?? GlobalSettings.DefaultImageWidth,
                height ?? GlobalSettings.DefaultImagelHeight);

            //Update file
            if (entity.FileName == file.FileName)
            {
                var fileVersionInfo = FileVersion.GetFileVersion(entity.FileName);
                entity.FileVersion = fileVersionInfo.Version;
            }

            string fileName = Path.GetFileNameWithoutExtension(file.FileName);
            string fileExt = Path.GetExtension(file.FileName).ToLower().Trim();
            string newFileName = FileUtils.GenerateEncodedFileNameWithDateSignature(fileName);
            string newFileNameEncoded = HttpContext.Current.Server.HtmlEncode(newFileName + fileExt);

            entity.FileName = newFileNameEncoded;
            entity.FileTitle = fileName;
            entity.FileType = FileUtils.GetMimeType(newFileNameEncoded);
            entity.Width = width ?? GlobalSettings.DefaultImageWidth;
            entity.Height = height ?? GlobalSettings.DefaultImagelHeight;
            entity.StorageType = StorageType.Local;
            entity.LastModifiedDate = DateTime.UtcNow;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedByUserId = userId ?? Guid.Parse(GlobalSettings.DefaultUserId);
          
            UnitOfWork.DocumentFileRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        #endregion
        
        //#region Dipose

        //private bool _disposed = false;
        //protected override void Dispose(bool isDisposing)
        //{
        //    if (!this._disposed)
        //    {
        //        if (isDisposing)
        //        {

        //        }
        //        _disposed = true;
        //    }
        //    base.Dispose(isDisposing);
        //}

        //#endregion
    }
}
