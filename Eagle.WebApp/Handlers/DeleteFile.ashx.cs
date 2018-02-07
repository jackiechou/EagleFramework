using System;
using System.Web;
using System.Web.SessionState;
using Eagle.Services.SystemManagement;

namespace Eagle.WebApp.Handlers
{
    /// <summary>
    /// Summary description for DeleteFile
    /// </summary>
    public class DeleteFile :IHttpHandler, IRequiresSessionState
    {
        public IDocumentService DocumentService { get; set; }
        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.Params["fileId"] != null && !string.IsNullOrEmpty(context.Request.QueryString["fileId"]))
            {
                int fileId = Convert.ToInt32(context.Request.Params["fileId"]);
                if (fileId > 0)
                    DocumentService.DeleteFile(fileId);
            }

            context.Response.ContentType = "text/plain";
            context.Response.Write(true);
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