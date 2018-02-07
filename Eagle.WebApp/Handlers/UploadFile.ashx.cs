using Eagle.Common.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Web;
using System.Web.SessionState;
using Eagle.Core.Configuration;
using Eagle.Core.Settings;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.SystemManagement.FileStorage;
using Eagle.Services.SystemManagement;
using Eagle.Services.Validations;
using Eagle.WebApp.Common;

namespace Eagle.WebApp.Handlers
{
    /// <summary>
    /// Summary description for UploadFile
    /// </summary>
    public class UploadFile : IHttpHandler, IRequiresSessionState
    {
        public IDocumentService DocumentService { get; set; }

        string _fileKey = string.Empty, _folderKey = string.Empty,
            _fileTitle = string.Empty, _fileName = string.Empty, _fileDescription = string.Empty, _virtualDirPath = string.Empty, _physicalDirPath = string.Empty, _filePath = string.Empty;

        int? _fileId, _folderId, _height, _width;

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.Params["fileKey"] != null && context.Request.QueryString["fileKey"] != "")
                _fileKey = Convert.ToString(context.Request.Params["fileKey"]);

            if (context.Request.Params["folderId"] != null && context.Request.QueryString["folderId"] != "")
                _folderId = Convert.ToInt32(context.Request.Params["folderId"]);
            
            if (context.Request.Params["fileId"] != null && context.Request.QueryString["fileId"] != "")
                _fileId = Convert.ToInt32(context.Request.Params["fileId"]);

            if (context.Request.Params["fileTitle"] != null && context.Request.QueryString["fileTitle"] != "")
                _fileTitle = Convert.ToString(context.Request.Params["fileTitle"]);

            if (context.Request.Params["fileName"] != null && context.Request.QueryString["fileName"] != "")
                _fileName = Convert.ToString(context.Request.Params["fileName"]);

            if (context.Request.Params["fileDescription"] != null && context.Request.QueryString["fileDescription"] != "")
                _fileDescription = Convert.ToString(context.Request.Params["fileDescription"]);

            if (context.Request.Params["width"] != null && context.Request.QueryString["width"] != "")
                _width = Convert.ToInt32(context.Request.Params["width"]);

            if (context.Request.Params["height"] != null && context.Request.QueryString["height"] != "")
                _height = Convert.ToInt32(context.Request.Params["height"]);


            //Upload one file -----------------------------------------------------------------------------------------------
            var violations = new List<RuleViolation>();
            var file = !string.IsNullOrEmpty(_fileKey) ? context.Request.Files[_fileKey] : context.Request.Files[0];
            var fileSize = file.ContentLength;
            if (fileSize <= 0)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundHttpPostedFile, "HttpPostedFile"));
                throw new ValidationError(violations);
            }
           
            var folderItem = DocumentService.GetFolderDetailByFolderId((int)_folderId);
            if (folderItem == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundForDocumentFolder, "DocumentFolder", _folderId));
                throw new ValidationError(violations);
            }

            _virtualDirPath = "~" + folderItem.FolderPath;
            _physicalDirPath = HttpContext.Current.Server.MapPath(_virtualDirPath);
            if (!Directory.Exists(_physicalDirPath))
                Directory.CreateDirectory(_physicalDirPath);

           
            _fileName = FileUtils.GenerateEncodedFileNameWithDateSignature(file.FileName);
            string fileExtention = Path.GetExtension(file.FileName);
            string fileName = HttpContext.Current.Server.HtmlEncode(_fileName + fileExtention);
            string filePath = Path.Combine(context.Server.MapPath(_virtualDirPath), fileName);
            var width = _width ?? ImageSettings.ImageWidthVga;
            var height = _height ?? ImageSettings.ImageHeightVga;

            //Save with resize
            System.Drawing.Image image = System.Drawing.Image.FromStream(file.InputStream);
            System.Drawing.Image thumb = image.GetThumbnailImage(width, height, null, IntPtr.Zero);
            thumb.Save(filePath);
            image.Dispose();
            thumb.Dispose();

            //Save without resize
            //file.SaveAs(filePath);

            var applicationId = ClaimsPrincipal.Current.GetApplicationId();
            var userId = ClaimsPrincipal.Current.GetUserId();

            var fileEntry = new DocumentFileEntry
            {
                FileName = _fileName,
                FileTitle = _fileTitle,
                FileDescription = _fileDescription,
                FolderId = (int)_folderId,
                FileUrl = _filePath,
                Width = _width,
                Height = _height,
                StorageType = StorageType.Local
            };

            if (_fileId == null || _fileId == 0)
            {
                var fileInfo = DocumentService.InsertFile(applicationId, userId, fileEntry);
                _fileId = fileInfo.FileId;
            }
            else
            {
                var fileEditEntry = new DocumentFileEditEntry
                {
                    FileId = (int)_fileId,
                    FileName = _fileName,
                    FileTitle = _fileTitle,
                    FileDescription = _fileDescription,
                    FolderId = (int)_folderId,
                    FileUrl = _filePath,
                    Width = _width,
                    Height = _height,
                    StorageType = StorageType.Local
                };

                DocumentService.UpdateFile(applicationId, userId, fileEditEntry);
            }

            context.Response.ContentType = "text/plain";
            context.Response.Write(_fileId);
            context.Response.End();
        }

        public bool IsReusable => false;
    }
}