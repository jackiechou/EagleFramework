using Eagle.Common.Utilities;
using System;
using System.Web;
using System.Web.SessionState;
using Eagle.Services.SystemManagement;

namespace Eagle.WebApp.Handlers
{
    /// <summary>
    /// Summary description for UploadHandler
    /// </summary>
    public class UploadHandler : IHttpHandler, IRequiresSessionState 
    {
        public IDocumentService DocumentService { get; set; }

        string _folderKey = string.Empty, _fileTitle = string.Empty, fileName = string.Empty, _fileDescription = string.Empty, fileExt = string.Empty, fileType = string.Empty, filePath = string.Empty, physicalPath = string.Empty, message = string.Empty;
        int? fileId = null, height = null, width = null, oFileId = null;
        bool flag = false;  

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.Params["fileName"] != null && context.Request.QueryString["fileName"] != "")
                _fileTitle = Convert.ToString(context.Request.Params["fileName"]);

            if (context.Request.Params["fileDescription"] != null && context.Request.QueryString["fileDescription"] != "")
                _fileDescription = Convert.ToString(context.Request.Params["fileDescription"]);

            //Upload one file -----------------------------------------------------------------------------------------------
            if (context.Request.Files.Count > 0)
            {
                string virtual_dir_path = "~/Uploads/";
                //if (context.Request.Params["fileId"] != null && context.Request.QueryString["fileId"] != "")
                //    fileId = Convert.ToInt32(context.Request.Params["fileId"]);

                //if (context.Request.Params["folderKey"] != null && context.Request.QueryString["folderKey"] != "")
                //    folderKey = Convert.ToString(context.Request.Params["folderKey"]);

               
                HttpFileCollection files = context.Request.Files;
                HttpPostedFile file = null;         
                foreach (string key in files)
                {
                    file = files[key];
                    fileName = FileUtils.GenerateEncodedFileNameWithDate(file.FileName);
                    fileExt = System.IO.Path.GetExtension(file.FileName).ToLower().Trim();
                    fileType = file.ContentType;
                    filePath = System.IO.Path.Combine(context.Server.MapPath(virtual_dir_path), fileName);
                    file.SaveAs(filePath);

                    if (string.IsNullOrEmpty(_fileTitle))
                        _fileTitle = System.IO.Path.GetFileNameWithoutExtension(fileName);                   

                    System.IO.Stream fs = file.InputStream;
                    System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
                    Byte[] fileContent = br.ReadBytes((Int32)fs.Length);

                    //var fileEntry = new DocumentFileEntry
                    //{
                    //    FileName = fileName,
                    //    FolderId = _folderId,
                    //    FolderKey = _folderKey,
                    //    FileUrl = _filePath,
                    //    Width = _width,
                    //    Height = _height,
                    //};

                    //if (fileId == null || fileId == 0)
                    //    DocumentService.InsertData(folderKey, fileTitle, fileName, fileDescription, fileContent, width, height, creater, out oFileId);
                    //else
                    //    DocumentService.UpdateData((int)fileId, folderKey, fileTitle, fileName, fileDescription, fileContent, width, height, creater, out oFileId);
                }
            }
            //HttpPostedFile file = context.Request.Files["fileUpload"];
            //if (file != null)
            //{
            //    //string name = file.FileName;
            //    //byte[] binaryWriteArray = new
            //    //byte[file.InputStream.Length];
            //    //file.InputStream.Read(binaryWriteArray, 0,
            //    //(int)file.InputStream.Length);
            //    //FileStream objfilestream = new FileStream(Server.MapPath("uploads//" + name), FileMode.Create, FileAccess.ReadWrite);
            //    //objfilestream.Write(binaryWriteArray, 0,
            //    //binaryWriteArray.Length);
            //    //objfilestream.Close();
            //    //string[][] JaggedArray = new string[1][];
            //    //JaggedArray[0] = new string[] { "File was uploaded successfully" };
            //    //JavaScriptSerializer js = new JavaScriptSerializer();
            //    //strJSON = js.Serialize(JaggedArray);
            //    physicalPath = context.Server.MapPath(virtual_dir_path);
            //    if (System.IO.Directory.Exists(physicalPath))
            //    {
            //        fileName = file.FileName;
            //        filePath = System.IO.Path.Combine(physicalPath, fileName);
            //        file.SaveAs(filePath);
            //        result = "Uploaded Successfully";
            //    }               
            //}

            context.Response.ContentType = "text/plain";
            context.Response.Write(oFileId);
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