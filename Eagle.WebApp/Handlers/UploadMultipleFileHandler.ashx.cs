using Eagle.Common.Settings;
using Eagle.Common.Utilities;
using Eagle.Repositories.SYS.FileManager;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;

namespace Eagle.WebApp.Handlers
{
    /// <summary>
    /// Summary description for UploadMultipleFileHandler
    /// </summary>
    public class UploadMultipleFileHandler : IHttpHandler, IRequiresSessionState 
    {
        string fileIds = string.Empty, jsonResult = string.Empty, folderKey = string.Empty,
            fileTitle = string.Empty, fileName = string.Empty, fileDescription = string.Empty, fileExt = string.Empty, fileType = string.Empty, virtual_dir_path = string.Empty, filePath = string.Empty;
        int? height = null, width = null, oFileId = null;
        int fileId= 0;
        bool flag = false;
        HttpPostedFile file = null;

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.Params["fileIds"] != null && context.Request.QueryString["fileIds"] != "")
                fileIds = context.Request.Params["fileIds"].ToString();

            if (context.Request.Params["folderKey"] != null && context.Request.QueryString["folderKey"] != "")
                folderKey = context.Request.Params["folderKey"].ToString();

            if (context.Request.Params["fileName"] != null && context.Request.QueryString["fileName"] != "")
                fileTitle = Convert.ToString(context.Request.Params["fileName"]);

            if (context.Request.Params["fileDescription"] != null && context.Request.QueryString["fileDescription"] != "")
                fileDescription = Convert.ToString(context.Request.Params["fileDescription"]);

          
            //Upload multiple files ----------------------------------------------------------------------------------------           
            if (context.Request.Files.Count > 0)
            {
                virtual_dir_path = "~" + FolderRepository.GetFolderPathByFolderKey(folderKey);

                HttpFileCollection files = context.Request.Files;
                List<int?> lstResults = new List<int?>();
                foreach (string key in files)
                {
                    file = files[key];
                    fileExt = System.IO.Path.GetExtension(file.FileName).ToLower().Trim();
                    fileName = FileUtils.GenerateEncodedFileNameWithDate(file.FileName);
                    fileType = file.ContentType;
                    filePath = System.IO.Path.Combine(context.Server.MapPath(virtual_dir_path), fileName);
                    file.SaveAs(filePath);

                    if (string.IsNullOrEmpty(fileTitle))
                        fileTitle = System.IO.Path.GetFileNameWithoutExtension(fileName);                  

                    System.IO.Stream fs = file.InputStream;
                    System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
                    Byte[] fileContent = br.ReadBytes((Int32)fs.Length);

                    int creater = Convert.ToInt32(HttpContext.Current.Session[SettingKeys.UserId].ToString());                
                       
                    if (string.IsNullOrEmpty(fileIds) || fileIds == "0")
                        flag = FileRepository.InsertData(folderKey, fileTitle, fileName, fileDescription, fileContent, width, height, creater, out oFileId);
                    else
                        flag = FileRepository.UpdateData(fileId, folderKey, fileTitle, fileName, fileDescription, fileContent, width, height, creater, out oFileId);

                    lstResults.Add(oFileId);
                }
                jsonResult = jsonResult = String.Join(",", lstResults);
            }
            context.Response.ContentType = "text/plain";
            context.Response.Write(jsonResult);
            //------------------------------------------------------------------------------------------------------------           
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