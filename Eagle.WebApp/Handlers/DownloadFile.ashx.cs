using System;
using System.IO;
using System.Web;
using System.Web.SessionState;

namespace Eagle.WebApp.Handlers
{
    /// <summary>
    /// Summary description for DownloadFile
    /// </summary>
    public class DownloadFile : IHttpHandler,IRequiresSessionState 
    {

        public void ProcessRequest(HttpContext context)
        {
            string downloadFileUrl = Convert.ToString(context.Request.QueryString["file"]);
            try
            {           
                if (!string.IsNullOrEmpty(downloadFileUrl))
                {
                    string physicalFilePath = context.Server.MapPath(downloadFileUrl);
                    string fileName = Path.GetFileName(physicalFilePath);

                    // Check to see if file exist
                    FileInfo fileInfo = new FileInfo(physicalFilePath);
                    if (!fileInfo.Exists)
                    {
                        context.Response.ContentType = "text/plain";
                        context.Response.Write("<span class=\"unlinked\">" + downloadFileUrl + " link doesn't exist!</script>");                        
                    }
                    else
                    {
                        HttpContext.Current.Response.ClearHeaders();
                        HttpContext.Current.Response.ClearContent();
                        HttpContext.Current.Response.AppendHeader("Content-Length", fileInfo.Length.ToString());
                        HttpContext.Current.Response.ContentType = "application/octet-stream";
                        HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
                        // HttpContext.Current.Response.BinaryWrite(ReadByteArryFromFile(fileInfo.FullName));
                        //context.Response.TransmitFile(fileInfo.FullName);
                        context.Response.WriteFile(fileInfo.FullName);
                        //HttpContext.Current.Response.End();
                        context.Response.Flush();
                    }
                }
                else
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("<span class=\"unlinked\">File cannot be found!</span>");
                }
            } catch (Exception ex) { 
                context.Response.ContentType = "text/plain"; 
                context.Response.Write(ex.Message); 
            } 
            finally { context.Response.End(); }
        }

        public bool IsReusable => false;
    }
}