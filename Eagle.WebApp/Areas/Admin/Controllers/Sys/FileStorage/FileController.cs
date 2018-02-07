using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using Eagle.Common.Utilities;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.SystemManagement;
using Eagle.WebApp.Areas.Admin.Controllers.Common;
using ElFinder;

//using ElFinder;
namespace Eagle.WebApp.Areas.Admin.Controllers.Sys.FileStorage
{
    public class FileController : BaseController
    {
        private static string rootPath = "Uploads";
        // private string thumbnailPath = "Thumbnails";
        //private string thumbnailPath = "Thumbnails";
        public string StartUrl => "http://" + Request.Url.Authority + "/" + rootPath + "/";
        public string StartPhysicalPath => Server.MapPath("~/" + rootPath);
        
        private IDocumentService DocumentService { get; }

        public FileController(IDocumentService documentService) : base(new IBaseService[] { documentService })
        {
            DocumentService = documentService;
        }

        //GET: /Admin/File/
        public ActionResult Index(string folder, string subFolder)
        {
            FileSystemDriver driver = new FileSystemDriver();

            var root = new Root(
                    new DirectoryInfo(Server.MapPath("~/Uploads/" + folder)),
                    "http://" + Request.Url.Authority + "/Uploads/" + folder)
            {
                // Sample using ASP.NET built in Membership functionality...
                // Only the super user can READ (download files) & WRITE (create folders/files/upload files).
                // Other users can only READ (download files)
                // IsReadOnly = !User.IsInRole(AccountController.SuperUser)

                IsReadOnly = false, // Can be readonly according to user's membership permission
                Alias = LanguageResource.Root, // Beautiful name given to the root/home folder
                MaxUploadSize = 30000000, // Limit imposed to user uploaded file <= 500 KB
                LockedFolders = new List<string>(new string[] { "Folder1" })
            };

            // Was a subfolder selected in Home Index page?
            if (!string.IsNullOrEmpty(subFolder))
            {
                root.StartPath = new DirectoryInfo(Server.MapPath("~/Uploads/" + folder + "/" + subFolder));
            }

            driver.AddRoot(root);
            var connector = new Connector(driver);
            return connector.Process(this.HttpContext.Request);
        }

        public ActionResult SelectFile(string target)
        {
            string physicalFolderPath = StartPhysicalPath;
            FileSystemDriver driver = new FileSystemDriver();

            if (Request.Url != null)
            {
                driver.AddRoot(new Root(new DirectoryInfo(physicalFolderPath), StartUrl) { IsReadOnly = false });
            }

            var connector = new Connector(driver);

            return Json(connector.GetFileByHash(target).FullName);
        }

        public ActionResult Thumbs(string tmb)
        {
            string physicalFolderPath = StartPhysicalPath;
            FileSystemDriver driver = new FileSystemDriver();
            if (Request.Url != null)
            {
                driver.AddRoot(new Root(new DirectoryInfo(physicalFolderPath), StartUrl) { IsReadOnly = false });
            }

            var connector = new Connector(driver);
            return connector.GetThumbnail(Request, Response, tmb);
        }


        //#region FILE MANAGER
        //<img src='@Url.Action("GetImage", "FileManager", new {FileId:1})'/>
        public ActionResult GetImage(int? fileId)
        {
            string imagePath = "~/Files/images/no_image.jpg", contentType = "images/jpg";
            byte[] imageByteData = null;
            if (fileId > 0)
            {
                var entity = DocumentService.GetFileInfoDetail(Convert.ToInt32(fileId));
                if (entity != null)
                {
                    imageByteData = entity.FileContent;
                    contentType = entity.FileType;
                }
                else
                    imageByteData = System.IO.File.ReadAllBytes(imagePath);
            }
            else
                imageByteData = System.IO.File.ReadAllBytes(imagePath);
            return File(imageByteData, contentType);
        }

        //[HttpGet]
        //public ActionResult GetUploadedFileList(string fileId)
        //{
        //    var model = DocumentService.GetFileInfoDetail(Convert.ToInt32(fileId));
        //    return PartialView("../Sys/FileStorage/_UploadedFileList", lst);
        //}

        [HttpGet]
        public ActionResult CreateDownloadLink(int? fileId)
        {
            if (fileId == null) return null;

            string downloadFileLink = DocumentService.GenerateDownloadLink(Convert.ToInt32(fileId));
            return PartialView("../Sys/FileStorage/_DownloadLink", downloadFileLink);
        }

        ////[HttpGet]
        ////public ActionResult GetDownloadFileList(int ItemId, string ItemTag, string FileIds)
        ////{
        ////    var downloadList = _documentService.GetDownloadFileList(ItemId, ItemTag, FileIds);           
        ////    return PartialView("../FileManager/_DownloadList", downloadList);
        ////}

        //////[HttpGet]
        //////public ActionResult PopulateDownloadFileList(int ItemId, string ItemTag)
        //////{
        //////    List<FileUploadModel> downloadList = new List<FileUploadModel>();
        //////    downloadList = FileRepository.GetDownloadFileList(ItemId, ItemTag);
        //////    return PartialView("../FileManager/_DownloadList", downloadList);
        //////}

        ////[HttpGet]
        ////public ActionResult PopulateDownloadPage(string ItemId, string ItemTag, string FileIds)
        ////{
        ////    var download_page_model = new FileModel
        ////    {
        ////        ItemId = ItemId,
        ////        ItemTag = ItemTag,
        ////        FileIds = FileIds
        ////    };
        ////    return PartialView("../FileManager/_DownloadPage", download_page_model);
        ////}

        //////
        ////// GET: /Admin/Contract/Details/5      
        ////[HttpGet]
        ////[SessionExpiration]
        ////public ActionResult PopulateAddFileForm(int ItemId, string ItemTag, string FolderKey)
        ////{
        ////    FileUploadModel entity = new FileUploadModel();
        ////    entity.ItemId = ItemId;
        ////    entity.ItemTag = ItemTag;
        ////    entity.FolderKey = FolderKey;
        ////    ViewBag.AddButtonStatus = "";
        ////    ViewBag.EditButtonStatus = "hide";
        ////    return PartialView("../FileManager/EditPhoto", entity);
        ////}    

        ////[HttpGet]
        ////[SessionExpiration]
        ////public ActionResult Details(int id)
        ////{
        ////    var entity = new FileUploadModel();
        ////    if (id > 0)
        ////    {
        ////        entity = _documentService.GetDetailByFileId(id);
        ////        if (entity == null)
        ////            entity = new FileUploadModel();
        ////    }
        ////    return PartialView("../FileManager/Edit", entity);
        ////}

        ////[HttpGet]
        ////[SessionExpiration]
        ////public ActionResult EditPhoto(int fileId, string folderKey, int itemId, string itemTag)
        ////{
        ////    var entity = new FileUploadModel();
        ////    if (fileId > 0)
        ////    {
        ////        entity = _documentService.GetDetailByFileId(fileId, folderKey, itemId, itemTag);
        ////        if (entity != null)
        ////        {
        ////            ViewBag.AddButtonStatus = "hide";
        ////            ViewBag.EditButtonStatus = "";
        ////        }
        ////        else
        ////        {                  
        ////            entity = new FileUploadModel();
        ////            entity.ItemId = itemId;
        ////            entity.ItemTag = itemTag;
        ////            entity.FolderKey = folderKey;
        ////            ViewBag.AddButtonStatus = "";
        ////            ViewBag.EditButtonStatus = "hide";
        ////        }
        ////    }
        ////    else
        ////    {
        ////        entity.ItemId = itemId;
        ////        entity.ItemTag = itemTag;
        ////        entity.FolderKey = folderKey;
        ////        ViewBag.AddButtonStatus = "";
        ////        ViewBag.EditButtonStatus = "hide";
        ////    }
        ////    return PartialView("../FileManager/EditPhoto", entity);
        ////}       


        ////[HttpGet]
        ////[SessionExpiration]
        ////public ActionResult Edit(int fileId, int ItemId, string itemTag)
        ////{
        ////    var entity = new FileUploadModel();
        ////    if (fileId > 0)
        ////    {
        ////        entity = _documentService.GetDetailByFileId(fileId, ItemId, itemTag);
        ////        if (entity == null)
        ////        {
        ////            entity = new FileUploadModel();
        ////            entity.ItemId = ItemId;
        ////            entity.ItemTag = itemTag;
        ////        }
        ////    }
        ////    ViewBag.ItemId = ItemId;
        ////    ViewBag.ItemTag = itemTag;
        ////    return PartialView("../FileManager/Edit", entity);
        ////}

        ////[HttpPost]
        ////[SessionExpiration]
        ////public ActionResult Insert(FileUploadModel model)
        ////{
        ////    string Message = string.Empty;
        ////    int? oFileId = null, Creater =null;
        ////    bool flag = false;

        ////    HttpPostedFileBase file = model.FileUploadName;
        ////    if (file != null && file.ContentLength > 0)
        ////    {
        ////        try
        ////        {
        ////            string folderKey = model.FolderKey;
        ////            string virtual_dir_path = "~" + _documentService.GetFolderPathByFolderKey(model.FolderKey);
        ////            string physical_dir_path = Server.MapPath(virtual_dir_path);
        ////            if (!Directory.Exists(physical_dir_path))
        ////                Directory.CreateDirectory(physical_dir_path);

        ////            int? width = model.Width;
        ////            int? height = model.Height;
        ////            Stream fs = file.InputStream;
        ////            BinaryReader br = new BinaryReader(fs);
        ////            Byte[] fileContent = br.ReadBytes((Int32)fs.Length);
        ////            var extension = System.IO.Path.GetExtension(file.FileName);
        ////            if (extension != null)
        ////            {
        ////                string fileExt = extension.ToLower().Trim();
        ////            }
        ////            string fileType = file.ContentType;
        ////            string fileName = FileUtils.GenerateEncodedFileNameWithDate(file.FileName, Creater.ToString());
        ////            string fileDescription = model.FileDescription;
        ////            string fileTitle = model.FileTitle;
        ////            if (string.IsNullOrEmpty(fileTitle))
        ////                fileTitle = System.IO.Path.GetFileNameWithoutExtension(fileName);
        ////            string filePath = System.IO.Path.Combine(physical_dir_path, fileName);
        ////            file.SaveAs(filePath);


        ////            flag = _documentService.InsertData(folderKey, fileTitle, fileName, fileDescription, fileContent, width, height, GlobalSettings.UserId, out oFileId);
        ////            if (flag == true)
        ////            {
        ////                Message = LanguageResource.CreateSuccess;
        ////                model.FileId = oFileId;
        ////                //cap nhat vao database table tuong ung voi table va id thong qua ItemId => id va ItemTag => controllername
        ////                int ItemId = model.ItemId;
        ////                string ItemTag = model.ItemTag;
        ////                if (ItemId > 0 && !string.IsNullOrEmpty(ItemTag))
        ////                    _documentService.UpdateFileId(ItemId, ItemTag, (int)oFileId);                       
        ////            }
        ////        }
        ////        catch (Exception ex)
        ////        {
        ////            Message = LanguageResource.Error + ": " + ex.Message.ToString();
        ////        }
        ////    }
        ////    else
        ////    {
        ////        Message = LanguageResource.PleaseChooseFile;
        ////    }
        ////    Dictionary<string, string> dict = new Dictionary<string, string>();
        ////    dict.Add("flag", flag.ToString());
        ////    dict.Add("message", Message);
        ////    dict.Add("ItemId", model.ItemId.ToString());
        ////    dict.Add("ItemTag", model.ItemTag.ToString());
        ////    dict.Add("FileId", model.FileId.ToString());
        ////    return Json(dict, JsonRequestBehavior.AllowGet);
        ////}

        ////[HttpPost]
        ////[SessionExpiration]
        ////public ActionResult Update(FileUploadModel model)
        ////{
        ////    bool flag = false;
        ////    string message = string.Empty, ItemTag = string.Empty;
        ////    int? oFileId = 0; int ItemId = 0;

        ////    HttpPostedFileBase file = Request.Files[0];
        ////    if (file != null && file.ContentLength > 0)
        ////    {
        ////        try
        ////        {        
        ////            string virtual_dir_path = "~" + _documentService.GetFolderPathByFolderKey(model.FolderKey);
        ////            string physical_dir_path = Server.MapPath(virtual_dir_path);
        ////            if (!Directory.Exists(physical_dir_path))
        ////                Directory.CreateDirectory(physical_dir_path);

        ////            Stream fs = file.InputStream;
        ////            BinaryReader br = new BinaryReader(fs);
        ////            model.FileContent = br.ReadBytes((Int32)fs.Length);
        ////            string fileExt = System.IO.Path.GetExtension(file.FileName).ToLower().Trim();
        ////            string fileType = file.ContentType;
        ////            model.FileName = FileUtils.GenerateEncodedFileNameWithDate(file.FileName, GlobalSettings.UserId.ToString());
        ////            if (string.IsNullOrEmpty(model.FileTitle))
        ////                model.FileTitle = System.IO.Path.GetFileNameWithoutExtension(model.FileName);
        ////            string filePath = System.IO.Path.Combine(physical_dir_path, model.FileName);
        ////            file.SaveAs(filePath);
        ////        }
        ////        catch (Exception ex)
        ////        {
        ////            message = LanguageResource.Error + ": " + ex.Message.ToString();
        ////        }
        ////    }

        ////    flag = _documentService.UpdateData((int)model.FileId, model.FolderKey, model.FileTitle, model.FileName, model.FileDescription, model.FileContent, model.Width, model.Height, GlobalSettings.UserId, out oFileId);
        ////    if (flag == true)
        ////    {
        ////        //cap nhat vao database table tuong ung voi table va id thong qua ItemId => id va ItemTag => controllername
        ////        ItemId = model.ItemId;
        ////        ItemTag = model.ItemTag;
        ////        if (ItemId > 0 && !string.IsNullOrEmpty(ItemTag))
        ////            _documentService.UpdateFileId(ItemId, ItemTag, (int)oFileId);
        ////        message = LanguageResource.UpdateSuccess;
        ////    }
        ////    Dictionary<string, string> dict = new Dictionary<string, string>();
        ////    dict.Add("flag", flag.ToString());
        ////    dict.Add("message", message);
        ////    dict.Add("ItemId", model.ItemId.ToString());
        ////    dict.Add("ItemTag", model.ItemTag.ToString());
        ////    dict.Add("FileId", model.FileId.ToString());
        ////    return Json(dict, JsonRequestBehavior.AllowGet);
        ////}

        ////// POST: /Admin/Qualification/Delete/5
        ////[HttpPost]
        ////public ActionResult Delete(int id)
        ////{
        ////    bool flag = false;
        ////    string message = id.ToString();
        ////    flag = _documentService.Delete(id, out message);
        ////    return Json(JsonUtils.SerializeResult(flag, message), JsonRequestBehavior.AllowGet);
        ////}

        //////[HttpPost]
        //////public JsonResult DeleteFileInFileList(string ItemId, string ItemTag, int FileId)
        //////{
        //////    bool flag = false;
        //////    string FileIds = string.Empty, Message = string.Empty;
        //////    flag = FileRepository.DeleteFileInFileList(Convert.ToInt32(ItemId), ItemTag, FileId, out FileIds, out Message);
        //////    Dictionary<string, string> dict = new Dictionary<string, string>();
        //////    dict.Add("flag",flag.ToString());
        //////    dict.Add("message",Message);
        //////    dict.Add("ItemId", ItemId.ToString());
        //////    dict.Add("ItemTag",ItemTag);
        //////    dict.Add("FileIds",FileIds);
        //////    return Json(dict, JsonRequestBehavior.AllowGet);
        //////}


        //////public FileResult GetFile(string filePath)
        //////{
        //////    string PhysicalPath = Server.MapPath(filePath);
        //////    FileInfo entity = new FileInfo(PhysicalPath);
        //////    string FileName = entity.FullName;
        //////    string FileExtension = entity.Extension.ToLower();
        //////    string[] documentTypes = { ".doc", ".docx", ".pdf"};
        //////    string MimeType = string.Empty;
        //////    if (FileExtension == ".doc")
        //////        MimeType = System.Net.Mime.MediaTypeNames.Application.Octet;

        //////    //string path = AppDomain.CurrentDomain.BaseDirectory + "App_Data/";
        //////    // Force the pdf document to be displayed in the browser
        //////    Response.AppendHeader("Content-Disposition", "inline; filename=" + FileName + ";");
        //////    return File(filePath, System.Net.Mime.MediaTypeNames.Application.Pdf, FileName);
        //////}

        //////public ActionResult GetFile(string filePath)
        //////{
        //////    Microsoft.Office.Interop.Word.ApplicationClass AC = new Microsoft.Office.Interop.Word.ApplicationClass();

        //////    Microsoft.Office.Interop.Word.Document doc = new Microsoft.Office.Interop.Word.Document();

        //////    object readOnly = false;
        //////    object isVisible = true;
        //////    object missing = System.Reflection.Missing.Value;

        //////    string result = string.Empty;
        //////    string physicalFilePath = Server.MapPath(filePath);
        //////    FileInfo fileInfo = new FileInfo(physicalFilePath);
        //////    object fileName = fileInfo.FullName;


        //////    try
        //////    {
        //////        doc = AC.Documents.Open(ref fileName, ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref isVisible);
        //////        result = doc.Content.Text;
        //////    }

        //////    catch (Exception ex)
        //////    {
        //////        ex.ToString();
        //////    }

        //////    finally
        //////    {         
        //////        ((Microsoft.Office.Interop.Word._Document)doc).Close(ref missing, ref missing, ref missing);               
        //////    }
        //////    return Json(result, JsonRequestBehavior.AllowGet); 
        //////}

        //////     



        //#endregion FILE MANAGER

        //#region UPLOAD FILE ==================================================================================================================
        
        ////public ActionResult UploadDragNDrop()
        ////{
        ////    return PartialView("../FileManager/_UploadDragNDrop");
        ////}

        ////public ActionResult DndPLUpload(string folderKey, string fileId)
        ////{
        ////    ViewBag.folderKey = folderKey;
        ////    ViewBag.fileId = fileId;
        ////    return PartialView("../FileManager/_DndPLUpload");
        ////}

        ////public ActionResult PLUpload(string FolderKey, string FileIds, string UpdateFileIdsServiceUrl, int? KeyId)
        ////{
        ////    FileControlModel entity = new FileControlModel();
        ////    entity.FolderKey = FolderKey;
        ////    entity.FileIds = FileIds;
        ////    entity.UpdateFileIdsServiceUrl = UpdateFileIdsServiceUrl;
        ////    entity.KeyId = KeyId;
        ////    return PartialView("../FileManager/_PLUpload",entity);
        ////}

        ////public ActionResult UILPUpload(string folderKey, string fileId)
        ////{
        ////    FileViewModel entity = new FileViewModel();
        ////    entity.FolderKey = folderKey;
        ////    if (!string.IsNullOrEmpty(fileId))
        ////        entity.FileId = Convert.ToInt32(fileId);
        ////    return PartialView("../FileManager/_UIPLUpload", entity);
        ////}

        ////public ActionResult PopulateUploadForm(string folderKey, string fileId)
        ////{
        ////    FileViewModel entity = new FileViewModel();
        ////    entity.FolderKey = folderKey;
        ////    if (!string.IsNullOrEmpty(fileId))
        ////        entity.FileId = Convert.ToInt32(fileId);
        ////    return PartialView("../FileManager/_UploadFile", entity);
        ////}

        ////public ActionResult PopulateUploadMultipleFileForm(string ItemId, string ItemTag, string FolderKey, string FileIds)
        ////{
        ////    FileModel entity = new FileModel();
        ////    entity.FolderKey = FolderKey;
        ////    entity.FileIds = FileIds;
        ////    entity.ItemId = ItemId;
        ////    entity.ItemTag = ItemTag;
        ////    return PartialView("../FileManager/_UploadMulitpleFileExt", entity);
        ////}

        ////[HttpGet]
        ////public ActionResult PopulateUploadFileListForm(string folderKey, string fileId)
        ////{
        ////    FileViewModel entity = new FileViewModel();
        ////    entity.FolderKey = folderKey;
        ////    if (!string.IsNullOrEmpty(fileId))
        ////        entity.FileId = Convert.ToInt32(fileId);
        ////    return PartialView("../FileManager/_UploadFiles", entity);
        ////}

        ////public ActionResult Upload()
        ////{
        ////    for (int i = 0; i < Request.Files.Count; i++)
        ////    {
        ////        var file = Request.Files[i];
        ////        file.SaveAs(AppDomain.CurrentDomain.BaseDirectory + "Uploads/" + file.FileName);
        ////    }
        ////    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        ////}


        //////[HttpPost]
        //////public ActionResult UploadMulitpleFiles(FileModel model)
        //////{           
        //////    string fileName = string.Empty, fileExt = string.Empty, fileTitle = string.Empty, fileDescription = string.Empty,
        //////        fileType = string.Empty, filePath = string.Empty, Added_FileIds = string.Empty, FileIds = string.Empty;

        //////    int? Creater = null, oFileId = null;
        //////    bool flag = false;

        //////    string folderKey = model.FolderKey;    
        //////    string virtual_dir_path = "~" + FolderRepository.GetFolderPathByFolderKey(folderKey);
        //////    List<int?> lstResults = new List<int?>();
        //////    foreach (var item in model.FileUploadList)
        //////    {  
        //////        /*Loop for multiple files*/                  
        //////        HttpPostedFileBase myFile = item.FileUploadName;                
        //////        Stream fs = myFile.InputStream;
        //////        BinaryReader br = new BinaryReader(fs);
        //////        Byte[] fileContent = br.ReadBytes((Int32)fs.Length);
        //////        fileType = myFile.ContentType;
        //////        fileExt = System.IO.Path.GetExtension(myFile.FileName).ToLower().Trim();
        //////        fileName = FileUtils.GenerateEncodedFileNameWithDate(myFile.FileName, Creater.ToString());
        //////        if (string.IsNullOrEmpty(item.FileTitle))
        //////            fileTitle = System.IO.Path.GetFileNameWithoutExtension(fileName);
        //////        else
        //////            fileTitle = item.FileTitle;
        //////        fileDescription = item.FileDescription;
        //////        filePath = System.IO.Path.Combine(Server.MapPath(virtual_dir_path), fileName);
        //////        myFile.SaveAs(filePath);

        //////        int creater = Convert.ToInt32(UserId);

        //////        flag = FileRepository.InsertData(folderKey, fileTitle, fileName, fileDescription, fileContent, item.Width, item.Height, creater, out oFileId);
        //////        lstResults.Add(oFileId);
        //////    }
        //////    //ket wa tra ve danh sach file id 
        //////    Added_FileIds = String.Join(",", lstResults);
        //////    //cap nhat vao database table tuong ung voi table va id thong qua ItemId => id va ItemTag => controllername
        //////    string ItemId = model.ItemId;
        //////    string ItemTag = model.ItemTag;
        //////    if (!string.IsNullOrEmpty(ItemId) && !string.IsNullOrEmpty(ItemTag))
        //////    {
        //////        FileRepository.UpdateFileList(Convert.ToInt32(ItemId), ItemTag, Added_FileIds, out FileIds);
        //////        model.FileIds = FileIds;
        //////    }
        //////    else
        //////        model.FileIds = Added_FileIds;
        //////    return Json(new { ItemId = ItemId, ItemTag = ItemTag, FileIds = model.FileIds }, JsonRequestBehavior.AllowGet);
        //////}


        ////[HttpPost]
        ////public ActionResult UploadSingleFile(FileUploadModel model)
        ////{
        ////    string Message = string.Empty;
        ////    int? oFileId = null;
        ////    bool flag = false;

        ////    HttpPostedFileBase file = model.FileUploadName;
        ////    if (file != null && file.ContentLength > 0)
        ////    {
        ////        try
        ////        {
        ////            string virtual_dir_path = "~" + _documentService.GetFolderPathByFolderKey(model.FolderKey);
        ////            string physical_dir_path = Server.MapPath(virtual_dir_path);
        ////            if (!Directory.Exists(physical_dir_path))
        ////                Directory.CreateDirectory(physical_dir_path);

        ////            Stream fs = file.InputStream;
        ////            BinaryReader br = new BinaryReader(fs);
        ////            Byte[] fileContent = br.ReadBytes((Int32)fs.Length);
        ////            string fileExt = System.IO.Path.GetExtension(file.FileName).ToLower().Trim();
        ////            string fileType = file.ContentType;
        ////            string fileName = FileUtils.GenerateEncodedFileNameWithDate(file.FileName, GlobalSettings.UserId.ToString());
        ////            model.FileName = fileName;
        ////            if (string.IsNullOrEmpty(model.FileTitle))
        ////                model.FileTitle = System.IO.Path.GetFileNameWithoutExtension(fileName);
        ////            string filePath = System.IO.Path.Combine(Server.MapPath(virtual_dir_path), fileName);
        ////            file.SaveAs(filePath);


        ////            flag = _documentService.InsertData(model.FolderKey, model.FileTitle, model.FileName, model.FileDescription, model.FileContent, model.Width, model.Height, GlobalSettings.UserId, out oFileId);
        ////            if (flag == true)
        ////            {
        ////                model.FileId = oFileId;
        ////                //cap nhat vao database table tuong ung voi table va id thong qua ItemId => id va ItemTag => controllername
        ////                int ItemId = model.ItemId;
        ////                string ItemTag = model.ItemTag;
        ////                if (ItemId > 0 && !string.IsNullOrEmpty(ItemTag))
        ////                {
        ////                    _documentService.UpdateFileId(ItemId, ItemTag, (int)oFileId);
        ////                    model.FileId = oFileId;
        ////                }
        ////                Message = LanguageResource.CreateSuccess;
        ////            }
        ////        }
        ////        catch (Exception ex)
        ////        {
        ////            Message = LanguageResource.Error + ": " + ex.Message.ToString();
        ////        }
        ////    }
        ////    else
        ////    {
        ////        Message = LanguageResource.PleaseChooseFile;
        ////    }
        ////    Dictionary<string, string> dict = new Dictionary<string, string>();
        ////    dict.Add("flag", flag.ToString());
        ////    dict.Add("message", Message);
        ////    dict.Add("ItemId", model.ItemId.ToString());
        ////    dict.Add("ItemTag", model.ItemTag.ToString());
        ////    dict.Add("FileId", model.FileId.ToString());
        ////    return Json(dict, JsonRequestBehavior.AllowGet);
        ////}


        ////[HttpPost]
        ////public ActionResult UploadOneFile(string folderKey, string fileTitle, string fileDescription, int? width, int? height, int? fileId)
        ////{
        ////    string fileName = string.Empty, virtual_dir_path = string.Empty, physical_dir_path = string.Empty, fileExt = string.Empty,
        ////        fileType = string.Empty, filePath = string.Empty, result = string.Empty;
        ////    bool flag = false;
        ////    int? oFileId = null;
        ////    HttpPostedFileBase file = Request.Files[0];
        ////    if (file != null && file.ContentLength > 0)
        ////    {
        ////        try
        ////        {

        ////            if (folderKey != string.Empty)
        ////            {
        ////                int creater = Convert.ToInt32(GlobalSettings.UserId);

        ////                virtual_dir_path = "~" + _documentService.GetFolderPathByFolderKey(folderKey);
        ////                physical_dir_path = Server.MapPath(virtual_dir_path);
        ////                if (!Directory.Exists(physical_dir_path))
        ////                    Directory.CreateDirectory(physical_dir_path);

        ////                Stream fs = file.InputStream;
        ////                BinaryReader br = new BinaryReader(fs);
        ////                Byte[] fileContent = br.ReadBytes((Int32)fs.Length);
        ////                fileExt = System.IO.Path.GetExtension(file.FileName).ToLower().Trim();
        ////                fileType = file.ContentType;
        ////                fileName = FileUtils.GenerateEncodedFileNameWithDate(file.FileName, creater.ToString());
        ////                if (string.IsNullOrEmpty(fileTitle))
        ////                    fileTitle = System.IO.Path.GetFileNameWithoutExtension(fileName);
        ////                filePath = System.IO.Path.Combine(Server.MapPath(virtual_dir_path), fileName);
        ////                file.SaveAs(filePath);


        ////                if (fileId == null || fileId == 0)
        ////                    flag = _documentService.InsertData(folderKey, fileTitle, fileName, fileDescription, fileContent, width, height, creater, out oFileId);
        ////                else
        ////                    flag = _documentService.UpdateData((int)fileId, folderKey, fileTitle, fileName, fileDescription, fileContent, width, height, creater, out oFileId);

        ////                result = oFileId.ToString();
        ////            }
        ////            else
        ////            {
        ////                result = LanguageResource.FolderKeyNotFound;
        ////            }                   
        ////        }
        ////        catch (Exception ex)
        ////        {
        ////            result = LanguageResource.Error + ": " + ex.Message.ToString();
        ////        }
        ////    }
        ////    else
        ////    {
        ////        result = LanguageResource.PleaseChooseFile;
        ////    }
        ////    return Json(result, JsonRequestBehavior.AllowGet);
        ////}

        /////// <summary>
        /////// Uploads the file.
        /////// </summary>
        /////// <returns></returns>
        //[HttpPost]
        //public ActionResult UploadFile(string fileTitle, string fileDescription, string fileKey, string folderKey, int? width, int? height, int? fileId)
        //{
        //    bool isUploaded = false;
        //    string fileName = string.Empty, virtual_dir_path = string.Empty, physical_dir_path = string.Empty, fileExt = string.Empty,
        //        fileType = string.Empty, filePath = string.Empty, message = string.Empty;
        //    bool flag = false;
        //    int? oFileId = null;

        //    HttpPostedFileBase myFile = Request.Files[fileKey];
        //    if (myFile == null || myFile.ContentLength == 0)
        //    {
        //        message = string.Format("{0}: {1}", LanguageResource.UploadingError, LanguageResource.PleaseChooseFile);
        //        return Json(new { isUploaded = isUploaded, message = message }, "text/html");
        //    }
            
        //    if (string.IsNullOrEmpty(folderKey))
        //    {
        //        message = string.Format("{0}: {1}", LanguageResource.UploadingError, LanguageResource.PleaseChooseFile);
        //        return Json(new { isUploaded = isUploaded, message = message }, "text/html");
        //    }

        //    virtual_dir_path = "~" + DocumentService.GetFolderPathByFolderKey(folderKey);
        //    if (FileUtils.CreateFolderIfNeeded(virtual_dir_path))
        //    {
        //        try
        //        {
        //            Stream fs = myFile.InputStream;
        //            BinaryReader br = new BinaryReader(fs);
        //            Byte[] fileContent = br.ReadBytes((Int32)fs.Length);
        //            fileExt = System.IO.Path.GetExtension(myFile.FileName).ToLower().Trim();
        //            fileType = myFile.ContentType;
        //            fileName = FileUtils.GenerateEncodedFileNameWithDate(myFile.FileName);
        //            if (string.IsNullOrEmpty(fileTitle))
        //                fileTitle = System.IO.Path.GetFileNameWithoutExtension(fileName);
        //            filePath = System.IO.Path.Combine(Server.MapPath(virtual_dir_path), fileName);
        //            myFile.SaveAs(filePath);
        //            isUploaded = true;


        //            int creater = Convert.ToInt32(GlobalSettings.UserId);

        //            if (fileId == null || fileId == 0)
        //                flag = DocumentService.InsertData(folderKey, fileTitle, fileName, fileDescription, fileContent, width, height, creater, out oFileId);
        //            else
        //                flag = DocumentService.UpdateData((int)fileId, folderKey, fileTitle, fileName, fileDescription, fileContent, width, height, creater, out oFileId);

        //            message = oFileId.ToString();
        //        }
        //        catch (Exception ex)
        //        {
        //            message = string.Format("{0}: {1}", LanguageResource.UploadingError, ex.Message);
        //        }
        //    }
        //    return Json(new { isUploaded = isUploaded, message = message }, "text/html");
        //}
        //#endregion UPLOAD FILE =============================================================================================================

        #region DOWNLOAD FILE ==================================================================================================================
        ////////<a href="/Admin/FileManager/DownloadFile/1>Click to get file</a>
        ////////string filePath = AppDomain.CurrentDomain.BaseDirectory + virtual_dir_path + "/" + fileNameWithExtension;
        //////@Html.ActionLink("DownloadFile", "FileManager", new { FileId = item.FileId })
        [HttpGet]
        public FilePathResult DownloadFile(int fileId)
        {
            string virtualFilePath = "~" + DocumentService.GetFilePath(fileId);
            string physicalFilePath = Path.Combine(virtualFilePath);
            FilePathResult fpr = null;
            FileInfo fileInfo = new FileInfo(physicalFilePath);
            if (fileInfo.Exists)
            {
                string contentType = FileUtils.GetMimeType(fileInfo);
                string downloadedFileName = Path.GetFileNameWithoutExtension(physicalFilePath);
                fpr = File(physicalFilePath, contentType, downloadedFileName);
            }
            return fpr;
        }

        //<a href="/Admin/FileManager/DownloadFileFromStream/1">Click to get file</a>
        [HttpGet]
        public FileStreamResult DownloadFileFromStream(int fileId)
        {
            string virtualFilePath = "~" + DocumentService.GetFilePath(fileId);
            string physicalFilePath = Path.Combine(virtualFilePath);

            FileStreamResult fsr = null;
            FileInfo fileInfo = new FileInfo(physicalFilePath);
            if (fileInfo.Exists)
            {
                FileStream fs = new FileStream(physicalFilePath, FileMode.Open);
                string contentType = FileUtils.GetMimeType(fileInfo);
                string downloadedFileName = Path.GetFileNameWithoutExtension(physicalFilePath);
                fsr = File(fs, contentType, downloadedFileName);
            }
            return fsr;
        }

        //////<a href="/Admin/FileManager/DownloadFileFromByte/1">Click to get file</a>
        [HttpGet]
        public FileContentResult DownloadFileFromByte(int fileId)
        {
            var entity = DocumentService.GetFileDetail(fileId);
            byte[] fileContent = entity.FileContent;
            string contentType = entity.FileType;
            string downloadedFileName = Path.GetFileNameWithoutExtension(entity.FileName);
            return File(fileContent, contentType, downloadedFileName);
        }

        #endregion =============================================================================================================================


    }
}
