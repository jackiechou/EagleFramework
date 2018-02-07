using System;
using System.Web;
using Eagle.Services.SystemManagement;

namespace Eagle.WebApp.Handlers
{
    /// <summary>
    /// Summary description for GetFileList
    /// </summary>
    public class GetFileList : IHttpHandler
    {
        public IDocumentService DocumentService { get; set; }
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string fileIdList = context.Request.QueryString["FileIds"];
                string result = DocumentService.GetList(fileIdList);
                context.Response.ContentType = "text/plain";
                context.Response.Write(result);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write(ex.Message);
            }
            finally { context.Response.End(); }
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