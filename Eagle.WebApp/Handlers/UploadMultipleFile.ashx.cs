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
    /// Summary description for UploadMultipleFile
    /// </summary>
    public class UploadMultipleFile : IHttpHandler, IRequiresSessionState 
    {
        string fileIds = string.Empty, folderKey = string.Empty, fileName = string.Empty, fileExt = string.Empty, fileTitle = string.Empty, fileDescription = string.Empty, fileType = string.Empty, virtual_dir_path = string.Empty, filePath = string.Empty, jsonResult = string.Empty;
        int? height = null, width = null, oFileId = null;
        int fileId = 0;
        bool flag = false;
        
        HttpPostedFile file = null;

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.Params["FileId"] != null && context.Request.QueryString["FileId"] != "")
                fileIds = context.Request.Params["FileId"].ToString();

            if (context.Request.Params["FolderKey"] != null && context.Request.QueryString["FolderKey"] != "")
                folderKey = context.Request.Params["FolderKey"].ToString();

            if (context.Request.Params["fileName"] != null && context.Request.QueryString["fileName"] != "")
                fileTitle = Convert.ToString(context.Request.Params["fileName"]);

            if (context.Request.Params["fileDescription"] != null && context.Request.QueryString["fileDescription"] != "")
                fileDescription = Convert.ToString(context.Request.Params["fileDescription"]);
            
            //Upload multiple files ----------------------------------------------------------------------------------------           
            if (context.Request.Files.Count > 0)
            {
                virtual_dir_path = "~" + FolderRepository.GetFolderPathByFolderKey(folderKey);
                int creater = Convert.ToInt32(HttpContext.Current.Session[SettingKeys.UserId].ToString());

                List<int?> lstResults = new List<int?>();
                HttpFileCollection files = context.Request.Files;             
                foreach (string key in files)
                {
                    file = files[key];
                    fileType = file.ContentType;
                    fileExt = System.IO.Path.GetExtension(file.FileName).ToLower().Trim();
                    fileName = FileUtils.GenerateEncodedFileNameWithDate(file.FileName, creater.ToString());
                    filePath = System.IO.Path.Combine(context.Server.MapPath(virtual_dir_path), fileName);
                    file.SaveAs(filePath);

                    if (string.IsNullOrEmpty(fileTitle))
                        fileTitle = System.IO.Path.GetFileNameWithoutExtension(fileName);                 
                   
                    System.IO.Stream fs = file.InputStream;
                    System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
                    Byte[] fileContent = br.ReadBytes((Int32)fs.Length);                   

                    if (string.IsNullOrEmpty(fileIds) || fileIds == "0")
                        flag = FileRepository.InsertData(folderKey, fileTitle, fileName, fileDescription, fileContent, width, height, creater, out oFileId);
                    else
                        flag = FileRepository.UpdateData(fileId, folderKey, fileTitle, fileName, fileDescription, fileContent, width, height, creater, out oFileId);

                    lstResults.Add(oFileId);
                }
                jsonResult = String.Join(",", lstResults);
            }
            context.Response.ContentType = "text/plain";
            context.Response.Write(jsonResult);
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