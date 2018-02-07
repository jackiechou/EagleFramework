using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Eagle.Core.Configuration;
using Eagle.Services.SystemManagement;

namespace Eagle.WebApp.Handlers
{
    /// <summary>
    /// Summary description for PreviewFile
    /// </summary>
    public class PreviewFile : IHttpHandler
    {
        public IApplicationService ApplicationService { get; set; }
        public IDocumentService DocumentService { get; set; }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string notFoundFileUrl = $"~{GlobalSettings.NotFoundFileUrl}";
                string sFileId = context.Request.QueryString["FileId"];
                if (string.IsNullOrEmpty(sFileId) || sFileId == "0")
                {
                    context.Response.WriteFile(notFoundFileUrl);
                }

                int fileId = Convert.ToInt32(sFileId);
                var fileEntity = DocumentService.GetFileInfoDetail(fileId);
                if (fileEntity == null)
                {
                    context.Response.WriteFile(notFoundFileUrl);
                }
                else
                {
                    var filePath = "~" + fileEntity.FileUrl;
                    var fileContent = fileEntity.FileContent;
                    var physicalFilePath = HttpContext.Current.Server.MapPath(filePath);
                    var folderPath = "~" + fileEntity.FolderPath;
                    var physicalFolderPath = HttpContext.Current.Server.MapPath(folderPath);
                    var contentType = fileEntity.FileType;
                    context.Response.ContentType = contentType;

                    if (!string.IsNullOrEmpty(fileEntity.FolderPath))
                    {
                        DirectoryInfo dirInfo = new DirectoryInfo(physicalFolderPath);
                        if (!dirInfo.Exists)
                            dirInfo.Create();

                        FileInfo fileInfo = new FileInfo(physicalFilePath);
                        if (!fileInfo.Exists)
                        {
                            context.Response.ContentType = contentType;
                            context.Response.BinaryWrite(fileContent);
                        }
                        else
                        {
                            string fileName = Path.GetFileName(physicalFilePath);
                            HttpContext.Current.Response.ClearHeaders();
                            HttpContext.Current.Response.ClearContent();
                            HttpContext.Current.Response.AppendHeader("Content-Length", fileInfo.Length.ToString());
                            HttpContext.Current.Response.ContentType = "application/octet-stream";
                            HttpContext.Current.Response.AppendHeader("Content-Disposition",
                                "attachment; filename=" + fileName);
                            context.Response.WriteFile(filePath);
                        }
                    }
                    else
                        context.Response.WriteFile(notFoundFileUrl);
                }
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write(ex.Message);
            }
            finally
            {
                context.Response.End();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}