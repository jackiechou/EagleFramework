using System.Web;
using Eagle.Entities.SystemManagement.FileStorage;

namespace Eagle.Repositories.SystemManagement
{
    public interface IDocumentPermissionRepository
    {
        bool CanAccess(string path);
        bool AuthorizeRead(string path);
        bool AuthorizeThumbnail(string path);
        bool AuthorizeDeleteFile(string path);
        bool AuthorizeCreateDirectory(string path, string name);
        bool AuthorizeDeleteDirectory(string path);
        bool AuthorizeUpload(string path, HttpPostedFileBase file);
        bool AuthorizeImage(string path);
        bool AuthorizeFile(string path);
    }
}
