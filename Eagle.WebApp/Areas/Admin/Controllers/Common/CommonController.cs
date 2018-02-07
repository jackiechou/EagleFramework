using System.Web;
using System.Web.Mvc;

namespace Eagle.WebApp.Areas.Admin.Controllers.Common
{
    public class CommonController : BaseController
    {

        //public ActionResult SendEmail()
        //{ 
        //    Hashtable TemplateVariables = new Hashtable();
        //    TemplateVariables.Add("FullName", "Nam Vu Hoai");  
        //    TemplateVariables.Add("Link", "http://adncd.com"); 


        //    int Mail_Template_Id = 30;
        //    string MailTo = "minhnv@unit.com.vn";
        //    //string MailTo = "5eagles.live@gmail.com";
        //    string Cc = "";
        //    string Bcc = "";


        //    if (MailTemplateRespository.SendGMailByTemplate(TemplateVariables, Mail_Template_Id, MailTo, Cc, Bcc))
        //    {
        //        return Content("success");
        //    }else
        //    {
        //        return Content("error");
        //    }
        //}

        //public ActionResult SendEmailTLS()
        //{
        //    Hashtable TemplateVariables = new Hashtable();
        //    TemplateVariables.Add("FullName", "Nam Vu Hoai");
        //    TemplateVariables.Add("Link", "http://adncd.com");


        //    int Mail_Template_Id = 30;
        //    string MailTo = "minhnv@unit.com.vn";
        //    //string MailTo = "5eagles.live@gmail.com";
        //    string Cc = "";
        //    string Bcc = "";


        //    if (MailTemplateRespository.SendMailWithTLSByTemplate(TemplateVariables, Mail_Template_Id, MailTo, Cc, Bcc))
        //    {
        //        return Content("success");
        //    }
        //    else
        //    {
        //        return Content("error");
        //    }
        //}
        public ActionResult GetMd5(string id)
        {
            return Content(Eagle.Common.Utilities.StringUtils.GetMd5Sum(id));
        }

        #region Gender ----------------------------------------------------


        //public static IEnumerable<SelectListItem> PopulateGenderList(int? selectedId = 0, int LanguageId = 10001)
        //{
        //    GenderListModel lst = new GenderListModel(LanguageId);
        //    var selectList = new SelectList(lst.GenderList, "GenderId", "GenderName", selectedId);
        //    return selectList;
        //}

        //public static IEnumerable<SelectListItem> PopulateGenderList()
        //{
        //IEnumerable<GenderModel> genderTypes = Enum.GetValues(typeof(Gender)).Cast<Gender>();

        //IEnumerable<SelectListItem> genders = genderTypes.Select(x => new SelectListItem
        //    {
        //        Text = x.ToString(),
        //        Value = x.ToString()
        //    });

        //return genders; 
        //}
        #endregion ---------------------------------------------------------------


        #region Upload File -----------------------------------------------
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult UploadFile(HttpPostedFileBase uploadFile)
        {
            string fileName = string.Empty;
            string physicalDirPath = HttpContext.Server.MapPath("~/Content/Uploads");
            if (uploadFile.ContentLength > 0)
            {
                fileName = System.IO.Path.GetFileName(uploadFile.FileName);                
                string filePath = System.IO.Path.Combine(physicalDirPath, fileName);
                if(System.IO.File.Exists(filePath))
                    uploadFile.SaveAs(filePath);
            }
            return Json(fileName, JsonRequestBehavior.AllowGet);
            //return View("_Create");
            //return RedirectToAction("_Create");
        }

        //public class ViewDataUploadFilesResult
        //{
        //    public string Name { get; set; }
        //    public int Length { get; set; }
        //}
        //public ActionResult UploadFiles()
        //{
        //    var result = new List<ViewDataUploadFilesResult>();

        //    foreach (string file in Request.Files)
        //    {
        //        HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
        //        if (hpf.ContentLength == 0)
        //            continue;
        //        string savedFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,  Path.GetFileName(hpf.FileName));
        //        hpf.SaveAs(savedFileName);

        //        result.Add(new ViewDataUploadFilesResult()
        //        {
        //            Name = savedFileName,
        //            Length = hpf.ContentLength
        //        });
        //    }
        //    return View("UploadedFiles", result);
        //}
        #endregion -------------------------------------------------------

        #region Company - Department - Section ----------------------------
        //// Get Countries
        //[AcceptVerbs(HttpVerbs.Get)]
        //public JsonResult GetCountries()
        //{
        //    var list = db.LS_tblCountry
        //                    .Select(p => new LS_tblCountryViewModel()
        //                    {
        //                        LSCountryID = p.LSCountryID,
        //                        LSCountryCode = p.LSCountryCode,
        //                        Name = p.Name,
        //                        VNName = p.VNName,
        //                        Rank = p.Rank,
        //                        Used = p.Used,
        //                        Note = p.Note,
        //                        strUsed = p.Used == true ? "X" : ""
        //                    }).ToList();
        //    list.Insert(0, new LS_tblCountryViewModel() { LSCountryID = 0, Name = " --- " + @LanguageResource.Select + " --- " });
        //    var retData = list.Select(m => new SelectListItem()
        //    {
        //        Text = m.Name,
        //        Value = m.LSCountryID.ToString(),
        //    });
        //    return Json(retData, JsonRequestBehavior.AllowGet);
        //}

        //======================================================================================
        // Get Provinces By Country Id =========================================================
        //======================================================================================
        //[AcceptVerbs(HttpVerbs.Get)]
        //public JsonResult GetProvincesByCountryId(int id)
        //{
        //    try
        //    {
        //        var province = db.LS_tblProvince
        //            .Where(p => (int)p.LSCountryID == id)
        //            .Select(p => new { p.LSProvinceID, p.Name })
        //            .OrderBy(p => p.Name).ToList();

        //        var retData = province.Select(m => new SelectListItem()
        //        {
        //            Text = m.Name,
        //            Value = m.LSProvinceID.ToString(),
        //        });
        //        return Json(retData, JsonRequestBehavior.AllowGet);
        //    }
        //    catch
        //    {
        //        return Json("", JsonRequestBehavior.AllowGet);
        //    }
        //}
        //======================================================================================
        // Get Districts By Province Id ==================================================
        //======================================================================================  
        //[AcceptVerbs(HttpVerbs.Get)]
        //public JsonResult GetDistrictsByProvinceId(int id)
        //{
        //    try
        //    {
        //        //int proid = Convert.ToInt32(provinceid);
        //        var districtList = db.LS_tblDistrict
        //            .Where(p => (int)p.LSProvinceID == id)
        //            .Select(p => new { p.LSDistrictID, DistrictName = p.Name + "(" + p.LSDistrictCode + ")" })
        //            .OrderBy(p => p.DistrictName).ToList();

        //        var retData = districtList.Select(m => new SelectListItem()
        //        {
        //            Text = m.DistrictName,
        //            Value = m.LSDistrictID.ToString(),
        //        });
        //        return Json(retData, JsonRequestBehavior.AllowGet);
        //    }
        //    catch
        //    {
        //        return Json("", JsonRequestBehavior.AllowGet);
        //    }
        //}

         //public void PopulateCompaniesToDropDownList()
         //{
         //    //C1: Sd Hmtl.Dropdownlist
         //    LS_tblCompanyRepository _respository = new LS_tblCompanyRepository(db);
         //    List<CompanyDetail> _list = _respository.GetList();
         //    if (_list.Count == 0)
         //        _list.Insert(0, new CompanyDetail() { LSCompanyID = 0, Name = @LanguageResource.None });
         //    else
         //        ViewBag.LSCompanyID = new SelectList(_list, "LSCompanyID", "Name");

         //     // C2: Load du lieu tu jquery ajax
         //    //ViewBag.LSCompanyID = new SelectList(new List<CompanyDetail>(), "LSCompanyID", "Name");
         //}

         //public void PopulateDepartmentsToDropDownList(int? LSCompanyID)
         //{
         //    List<LS_tblLevel1ViewModel> _list = new List<LS_tblLevel1ViewModel>();
         //    if (LSCompanyID > 0)
         //    {
         //        LS_tblLevel1Repository _respository = new LS_tblLevel1Repository(db);
         //        _list = _respository.GetListByCompanyID(LSCompanyID);
         //    }
         //    if (_list.Count == 0)
         //        _list.Insert(0, new LS_tblLevel1ViewModel() { LSLevel1ID = 0, Name = @LanguageResource.None });

         //    ViewBag.LSLevel1ID = new SelectList(_list, "LSLevel1ID", "Name");
         //}

         //public void PopulateTeamGroupsToDropDownList(int? LSLevel1ID)
         //{
         //    List<LS_tblLevel2ViewModel> _list = new List<LS_tblLevel2ViewModel>();
         //    LS_tblLevel2ViewModel default_model = new LS_tblLevel2ViewModel() { LSLevel2ID = 0, Name = @LanguageResource.None };
         //    if (LSLevel1ID > 0)
         //    {
         //        LS_tblLevel2Repository _respository = new LS_tblLevel2Repository(db);
         //        _list = _respository.GetListByDepartmentID(LSLevel1ID);
         //    }
         //    if (_list.Count == 0)
         //        _list.Insert(0, default_model);
         //    ViewBag.LSLevel2ID = new SelectList(_list, "LSLevel2ID", "Name");
         //}
        #endregion -------------------------------------------------------------------------------
        
        #region Employee box -----------------------------------------------
        //public PartialViewResult Employee()
        //{

        //    EmployeeViewModel Emp = (EmployeeViewModel)Session[SettingKeys.EmpInfo];
        //    if (Emp == null)
        //    {
        //        Emp = new EmployeeViewModel();
        //    }
        //    return PartialView("./Common/Employee", Emp);
        //}
        #endregion

        #region leftbar => employee display -------------------------------
        //public ActionResult EmployeeDisplay(int? EmpID)
        //{
        //    EmployeeViewModel entity = new EmployeeViewModel();
        //    if (EmpID != null && EmpID != 0)
        //        entity.EmpID = (int)EmpID;
        //    return PartialView("../Common/Common/EmployeeDisplay", entity);
        //}
        //public ActionResult EmployeeDisplayDetails(int? EmpID)
        //{
        //    EmployeeViewModel Emp = new EmployeeViewModel();
        //    if (EmpID != null)
        //    {
        //        Emp = EmployeeRepository.GetDetails(EmpID, LanguageId);
        //        if (Emp != null)
        //        {
        //            Session[SettingKeys.EmpInfo] = Emp;
        //            CurrentEmpId = Emp.EmpID;
        //            CurrentEmpCode = Emp.EmpCode;
        //        }
        //        else
        //            Emp = (EmployeeViewModel)Session[SettingKeys.EmpInfo];
        //    }
        //    else
        //        Emp = (EmployeeViewModel)Session[SettingKeys.EmpInfo];
        //    return PartialView("../Common/Common/EmployeeDisplayDetails", Emp);
        //}
        #endregion
        
        #region Dropdownlist Employee
        //[HttpGet]
        //public ActionResult EmployeeDropdownList(string searchTerm, int pageSize, int pageNum)
        //{
        //    EmployeeRepository _repository = new EmployeeRepository(db);
        //    List<AutoCompleteModel> sources = _repository.ListDropdown2(searchTerm, pageSize, pageNum).ToList();
        //    int iTotal = sources.Count;

        //    //Translate the list into a format the select2 dropdown expects
        //    Select2PagedResultViewModel pagedList = ConvertAutoListToSelect2Format(sources, iTotal);

        //    //Return the data as a jsonp result
        //    return new JsonpResult
        //    {
        //        Data = pagedList,
        //        JsonRequestBehavior = JsonRequestBehavior.AllowGet
        //    };
        //}
        #endregion
    }
}
