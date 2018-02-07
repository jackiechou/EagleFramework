using System;
using System.IO;
using System.Linq;
using System.Web;
using Eagle.Core.Configuration;
using Eagle.Entities.SystemManagement.FileStorage;

namespace Eagle.Repositories.SystemManagement
{
    public class DocumentPermissionRepository : IDocumentPermissionRepository
    {
        #region Authorize
        public virtual bool CanAccess(string path)
        {
            //string uploadPath = GetDocumentPathByType(type);
            //string result;
            //switch (category)
            //{
            //    case DocumentCategory.Banner:
            //        result = Path.Combine(uploadPath, GlobalSettings.UploadBannerFolder);
            //        break;
            //    case DocumentCategory.News:
            //        result = Path.Combine(uploadPath, GlobalSettings.UploadNewsFolder);
            //        break;
            //    case DocumentCategory.Event:
            //        result = Path.Combine(uploadPath, GlobalSettings.UploadEventFolder);
            //        break;
            //    case DocumentCategory.Product:
            //        result = Path.Combine(uploadPath, GlobalSettings.UploadProductFolder);
            //        break;
            //    default:
            //        result = Path.Combine(uploadPath, GlobalSettings.UploadBannerFolder);
            //        break;
            //}
            //return path.StartsWith(FileUtils.ToAbsolutePath(result), StringComparison.OrdinalIgnoreCase);

            //string physicalPath = FileUtils.ToAbsolutePath(path);
            bool result = path.StartsWith(path, StringComparison.OrdinalIgnoreCase);
            return result;
        }

        public string GetDocumentPathByType(DocumentType type)
        {
            string uploadPath;
            switch (type)
            {
                case DocumentType.Image:
                    uploadPath = GlobalSettings.UploadImagePath;
                    break;
                case DocumentType.Document:
                    uploadPath = GlobalSettings.UploadDocumentPath;
                    break;
                case DocumentType.Video:
                    uploadPath = GlobalSettings.VideoUploadPath;
                    break;
                case DocumentType.Audio:
                    uploadPath = GlobalSettings.AudioUploadPath;
                    break;
                default:
                    uploadPath = GlobalSettings.UploadImagePath;
                    break;
            }
            return uploadPath;
        }

        public virtual bool AuthorizeRead(string path)
        {
            return CanAccess(path);
        }
        public virtual bool AuthorizeThumbnail(string path)
        {
            return CanAccess(path);
        }

        public virtual bool AuthorizeDeleteFile(string path)
        {
            return CanAccess(path);
        }
        public virtual bool AuthorizeCreateDirectory(string path, string name)
        {
            return CanAccess(path);
        }
   
        public virtual bool AuthorizeDeleteDirectory(string path)
        {
            return CanAccess(path);
        }
        public virtual bool AuthorizeUpload(string path, HttpPostedFileBase file)
        {
            //return CanAccess(path) && IsValidFile(file.FileName);
            return CanAccess(path);
        }
        public virtual bool AuthorizeImage(string path)
        {
            //return CanAccess(path) && IsValidImage(Path.GetExtension(path));
            return CanAccess(path);
        }

        public virtual bool AuthorizeFile(string path)
        {
            //return CanAccess(path) && IsValidFile(Path.GetExtension(path));
            return CanAccess(path);
        }

        //private bool IsValidImage(string fileName)
        //{
        //    if (string.IsNullOrEmpty(fileName)) return false;
        //    var extension = Path.GetExtension(fileName);
        //    var allowedExtensions = GlobalSettings.DefaultImageFilter.Split(',');
        //    return allowedExtensions.Any(e => e.EndsWith(extension, StringComparison.InvariantCultureIgnoreCase));
        //}

        //private bool IsValidFile(string fileName)
        //{
        //    if (string.IsNullOrEmpty(fileName)) return false;
        //    var extension = Path.GetExtension(fileName);
        //    var allowedExtensions = GlobalSettings.DefaultFileFilter.Split(',');
        //    return allowedExtensions.Any(e => e.EndsWith(extension, StringComparison.InvariantCultureIgnoreCase));
        //}

        #endregion
    }
}
