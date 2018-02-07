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
    /// Summary description for UploadMultipleFiles
    /// </summary>
    //public class UploadMultipleFiles : IHttpHandler, IRequiresSessionState 
    public class UploadMultipleFiles : IHttpHandler, IRequiresSessionState 
    {      
        string fileIds = string.Empty, jsonResult = string.Empty, folderKey = string.Empty,fileTitle = string.Empty, fileName = string.Empty, fileDescription = string.Empty, fileExt = string.Empty, fileType = string.Empty, filePath = string.Empty;
        int? height = null, width = null, oFileId = null;
        int fileId = 0;
        bool flag = false;
        
        public void ProcessRequest(HttpContext context)
        {
          
            if (context.Request.Params["fileIds"] != null && context.Request.QueryString["fileIds"] != "")
                fileIds = context.Request.Params["fileIds"].ToString();

            if (context.Request.Params["folderKey"] != null && context.Request.QueryString["folderKey"] != "")
                folderKey = context.Request.Params["folderKey"].ToString();

            if (context.Request.Params["fileTitle"] != null && context.Request.QueryString["fileTitle"] != "")
                fileTitle = Convert.ToString(context.Request.Params["fileTitle"]);

            if (context.Request.Params["fileDescription"] != null && context.Request.QueryString["fileDescription"] != "")
                fileDescription = Convert.ToString(context.Request.Params["fileDescription"]);

            string virtual_dir_path = "~" + FolderRepository.GetFolderPathByFolderKey(folderKey);
            //System.Collections.Specialized.NameValueCollection nvc = context.Request.Form;
            //Dictionary<string, string> fieldsDict = new Dictionary<string, string>();
            //foreach (string key in nvc.AllKeys)
            //{
            //    fieldsDict.Add(key, nvc[key]);
            //    HttpPostedFile hpFile = fileCollection[key];
            //}

            List<int?> lstResults = new List<int?>();
            HttpFileCollection files = context.Request.Files;
            foreach (string file in files)
            {
                HttpPostedFile hpf = files[file];
                if (hpf.ContentLength == 0)  continue;
                fileExt = System.IO.Path.GetExtension(hpf.FileName).ToLower().Trim();
                fileName = FileUtils.GenerateEncodedFileNameWithDate(hpf.FileName);
                fileType = hpf.ContentType;
                filePath = System.IO.Path.Combine(context.Server.MapPath(virtual_dir_path), fileName);
                hpf.SaveAs(filePath);

                if (string.IsNullOrEmpty(fileTitle))
                    fileTitle = System.IO.Path.GetFileNameWithoutExtension(fileName);

                System.IO.Stream fs = hpf.InputStream;
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

            //---------------------------------------------------------------------
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