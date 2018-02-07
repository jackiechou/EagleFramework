using System;
using System.Globalization;
using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Resources;

namespace Eagle.WebApp.Areas.Admin.Controllers.Common
{
    [OutputCache(Location = System.Web.UI.OutputCacheLocation.None, NoStore = true)]
    public class ValidationController : Controller
    {

        #region Candidate

        //public JsonResult ValidationCandidateQualificationNo(string QualificationNo, int CandidateID, string InitialQualificationNo)
        //{
        //    CandidateQualificationRepository _repository = new CandidateQualificationRepository(db);
        //    if (InitialQualificationNo != null)
        //    {
        //        if ((InitialQualificationNo != QualificationNo) && _repository.isQualificationNoExists(CandidateID, QualificationNo))
        //        {
        //            return base.Json(LanguageResource.InValidQualificationNo, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    else
        //    {
        //        if (_repository.isQualificationNoExists(CandidateID, QualificationNo))
        //        {
        //            return base.Json(LanguageResource.InValidQualificationNo, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    return base.Json(true, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult ValidationCandidatePriority(int PriorityAlowNull, int CandidateID, int? InitialPriorityAlowNull)
        //{
        //    CandidateQualificationRepository _repository = new CandidateQualificationRepository(db);
        //    if (InitialPriorityAlowNull != null)
        //    {
        //        if ((InitialPriorityAlowNull != PriorityAlowNull) && _repository.isPriorityExists(CandidateID, PriorityAlowNull))
        //        {
        //            return base.Json(LanguageResource.InValidPriority, JsonRequestBehavior.AllowGet);
        //        }
        //        if (PriorityAlowNull <= 0)
        //        {
        //            return base.Json(string.Format(LanguageResource.MinErrorMessage,LanguageResource.Priority,"0"), JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    else
        //    {
        //        if (_repository.isPriorityExists(CandidateID, PriorityAlowNull))
        //        {
        //            return base.Json(LanguageResource.InValidPriority, JsonRequestBehavior.AllowGet);
        //        }
        //        if (PriorityAlowNull <= 0)
        //        {
        //            return base.Json(string.Format(LanguageResource.MinErrorMessage, LanguageResource.Priority, "0"), JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    return base.Json(true, JsonRequestBehavior.AllowGet);
        //}
        #endregion
        #region For Demand
        //public JsonResult ValidationDemandCode(string DemandCode, string InitialDemandCode)
        //{
        //    DemandRepository repository = new DemandRepository(db);
        //    if ((DemandCode != InitialDemandCode) && repository.CheckExist(DemandCode))
        //    {
        //        if (LanguageId == 124)
        //        {
        //            return base.Json("\"" + DemandCode + "\" is exists!", JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return base.Json("\"" + DemandCode + "\" đ\x00e3 tồn tại!", JsonRequestBehavior.AllowGet);
        //        }
        //    }

        //    return base.Json(true, JsonRequestBehavior.AllowGet);
        //}
        #endregion
        #region For Candidate
        //public JsonResult ValidationNoIDCandidate(string IDNo, string InitialIDNo)
        //{
        //    CandidateRepository repository = new CandidateRepository(db);
        //    if ((IDNo != InitialIDNo) && repository.IDNoExists(IDNo))
        //    {
        //        if (LanguageId == 124)
        //        {
        //            return base.Json("\"" + IDNo + "\" is exists!", JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return base.Json("\"" + IDNo + "\" đ\x00e3 tồn tại!", JsonRequestBehavior.AllowGet);
        //        }
        //    }

        //    return base.Json(true, JsonRequestBehavior.AllowGet);
        //}
        
        //public JsonResult ValidationCandidateCode(string CandidateCode, string InitialCandidateCode)
        //{
        //    CandidateRepository repository = new CandidateRepository(db);
        //    if ((CandidateCode != InitialCandidateCode) && repository.CandidateCodeExists(CandidateCode))
        //    {
        //        if (LanguageId == 124)
        //        {
        //            return base.Json("\"" + CandidateCode + "\" is exists!", JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return base.Json("\"" + CandidateCode + "\" đ\x00e3 tồn tại!", JsonRequestBehavior.AllowGet);
        //        }
        //    }

        //    return base.Json(true, JsonRequestBehavior.AllowGet);
        //}
        #endregion
        #region For account

        //public JsonResult ValidationUserName(string UserName, string InitialUserName)
        //{
        //    if ((UserName != InitialUserName) && UserRepository.CheckExist(UserName))
        //    {
        //        if (GlobalSettings.LanguageId == 124)
        //        {
        //            return base.Json("\"" + UserName + "\" is exists!", JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return base.Json("\"" + UserName + "\" đ\x00e3 tồn tại!", JsonRequestBehavior.AllowGet);
        //        }
        //    }

        //    return base.Json(true, JsonRequestBehavior.AllowGet);
        //}
        #endregion
        #region Recruitment Project
        //public JsonResult ValidationProjectCode(string ProjectCode, string InitialProjectCode)
        //{
        //    RecruitmentProjectRepository repository = new RecruitmentProjectRepository(db);
        //    if ((ProjectCode != InitialProjectCode) && repository.CheckExist(ProjectCode))
        //    {
        //        if (LanguageId == 124)
        //        {
        //            return base.Json("\"" + ProjectCode + "\" is exists!", JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return base.Json("\"" + ProjectCode + "\" đ\x00e3 tồn tại!", JsonRequestBehavior.AllowGet);
        //        }
        //    }

        //    return base.Json(true, JsonRequestBehavior.AllowGet);
        //}
        #endregion
        #region For BirthDate Valiate
        public JsonResult ValidateBirthDate(string selectedDate)
        {
            string result = "true", SpecifiedFormat = "dd/MM/yyyy";
            if (selectedDate != string.Empty)
            {
                //if (LanguageId == 124)
                //    SpecifiedFormat = "MM/dd/yyyy";
                //else
                //    SpecifiedFormat = "dd/MM/yyyy";

                DateTime SELECTED_DATE;
                if (DateTime.TryParseExact(selectedDate, SpecifiedFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out SELECTED_DATE))
                {
                    DateTime rangeDateTime = DateTime.UtcNow.AddYears(-Math.Abs(18));
                    DateTime minDate = new DateTime(1900, 1, 1);
                    if (SELECTED_DATE.CompareTo(minDate) > 0)
                    {
                        if (SELECTED_DATE.CompareTo(rangeDateTime) > 0)
                        {
                            if (GlobalSettings.DefaultLanguageId == 124)
                                result = selectedDate + " is invalid!";
                            else
                                result = selectedDate + " không hợp lệ!";
                        }
                    }
                    else
                    {
                        if (GlobalSettings.DefaultLanguageId == 124)
                            result = selectedDate + " is greater than 1900!";
                        else
                            result = selectedDate + " phải lớn hơn 1900!";
                    }
                }
                else
                {
                    if (GlobalSettings.DefaultLanguageId == 124)
                        result = selectedDate + " is not date type!";
                    else
                        result = selectedDate + " không phải kiểu ngày!";
                }
            }
            else
            {
                result = GlobalSettings.DefaultLanguageId == 124 ? "Please pick a date!" : "Vui lòng chọn ngày!";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ValidateBirthDate(string selectedDate, int allowedYear)
        {
            string result = "true", SpecifiedFormat = "dd/MM/yyyy";
            if (selectedDate != string.Empty)
            {
                //if (LanguageId == 124)
                //    SpecifiedFormat = "MM/dd/yyyy";
                //else
                //    SpecifiedFormat = "dd/MM/yyyy";

                DateTime _selectedDate;
                if (DateTime.TryParseExact(selectedDate, SpecifiedFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _selectedDate))
                {
                    int _allowedYear = (allowedYear > 0) ? allowedYear : 18;
                    DateTime rangeDateTime = DateTime.UtcNow.AddYears(-Math.Abs(_allowedYear));
                    DateTime minDate = new DateTime(1900, 1, 1);
                    if (_selectedDate.CompareTo(minDate) > 0)
                    {
                        if (_selectedDate.CompareTo(rangeDateTime) > 0)
                        {
                            if (GlobalSettings.DefaultLanguageId == 124)
                                result = selectedDate + " is invalid!";
                            else
                                result = selectedDate + " không hợp lệ!";
                        }
                    }else
                    {
                        if (GlobalSettings.DefaultLanguageId == 124)
                            result = selectedDate + " is greater than 1900!";
                        else
                            result = selectedDate + " phải lớn hơn 1900!";
                    }
                }                   
                else
                {
                    if (GlobalSettings.DefaultLanguageId == 124)
                        result = selectedDate + " is not date type!";
                    else
                        result = selectedDate + " không phải kiểu ngày!";
                }
            }
            else
            {
                if (GlobalSettings.DefaultLanguageId == 124)
                    result = "Please pick a date!";
                else
                    result = "Vui lòng chọn ngày!";
            }           
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region For Group
        //public JsonResult ValidationGroupCode(string GroupCode, string InitialGroupCode)
        //{
        //    RoleRepository repository = new RoleRepository();
        //    if (repository.CheckExist(GroupCode) && (GroupCode.ToUpper() != InitialGroupCode.ToUpper()))
        //    {
        //        if (GlobalSettings.LanguageId == 124)
        //        {
        //            return base.Json("\"" + GroupCode + "\" is exists!", JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return base.Json("\"" + GroupCode + "\" đ\x00e3 tồn tại!", JsonRequestBehavior.AllowGet);
        //        }
        //    }

        //    return base.Json(true, JsonRequestBehavior.AllowGet);
        //}
        #endregion

        #region For LeaveDayType Code

        //public JsonResult ValidationLSLeaveDayTypeCode(string LSLeaveDayTypeCode, string strInitCode)
        //{
        //    LeaveDayTypeRepository repository = new LeaveDayTypeRepository(db);
        //    if (repository.CheckExist(LSLeaveDayTypeCode) && (LSLeaveDayTypeCode != strInitCode))
        //    {
        //        if (LanguageId == 124)
        //        {
        //            return base.Json("\"" + LSLeaveDayTypeCode + "\" is exists!", JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return base.Json("\"" + LSLeaveDayTypeCode + "\" đ\x00e3 tồn tại!", JsonRequestBehavior.AllowGet);
        //        }
        //    }

        //    return base.Json(true, JsonRequestBehavior.AllowGet);
        //}
        #endregion

        
        public JsonResult ValidationOtLimitIsGreaterThan0(int otLimit)
        {
            if (otLimit < 0)
            {
                string result = String.Format(LanguageResource.IsGreaterOrEqualThan0, LanguageResource.OTLimit);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ValidationStandardAnnualLeaveIsGreaterThan0(int standardAnnualLeave)
        {
            if (standardAnnualLeave < 0)
            {
                string result = String.Format(LanguageResource.IsGreaterOrEqualThan0, LanguageResource.StandardAnnualLeave);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult ValidationCompare2Date(DateTime? startDate, DateTime? endDate)
        {
            string result = "true";
            if(startDate.HasValue && endDate.HasValue)
            {
                if (DateTime.Compare(startDate.Value, endDate.Value) > 0)
                {
                    if (GlobalSettings.DefaultLanguageId == 124)
                        result = String.Format(CultureInfo.InvariantCulture, "{0} is less than {1}.", startDate, endDate);
                    else
                        result = String.Format(CultureInfo.InvariantCulture, "Vui lòng chọn ngày trước {0} nhỏ hơn ngày sau {1}", startDate, endDate);
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region For Training Code
        //public JsonResult ValidationTrainingCode(string Code, string InitialTrainingCode)
        //{
        //    TrainingCodeRepository repository = new TrainingCodeRepository(db);
        //    string errorMessage;
        //    if (repository.CheckExist(Code, out errorMessage) && (Code != InitialTrainingCode))
        //    {
        //        if (LanguageId == 124)
        //        {
        //            return base.Json("\"" + Code + "\" is exists!", JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return base.Json("\"" + Code + "\" đ\x00e3 tồn tại!", JsonRequestBehavior.AllowGet);
        //        }
        //    }

        //    return base.Json(true, JsonRequestBehavior.AllowGet);
        //}
        #endregion

        #region For Training Category
        //public JsonResult ValidationTrainingCategory(string Code, string InitialTrainingCategory)
        //{
        //    TrainingCategoryRepository repository = new TrainingCategoryRepository(db);
        //    string errorMessage;
        //    if (repository.CheckExist(Code, out errorMessage) && (Code != InitialTrainingCategory))
        //    {
        //        if (LanguageId == 124)
        //        {
        //            return base.Json("\"" + Code + "\" is exists!", JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return base.Json("\"" + Code + "\" đ\x00e3 tồn tại!", JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    return base.Json(true, JsonRequestBehavior.AllowGet);
        //}
        #endregion

        #region For Training Provider
        //public JsonResult ValidationTrainingProvider(string Code, string InitialTrainingProvider)
        //{
        //    TrainingProviderRepository repository = new TrainingProviderRepository(db);
        //    string errorMessage;
        //    if (repository.CheckExist(Code, out errorMessage) && (Code != InitialTrainingProvider))
        //    {
        //        if (LanguageId == 124)
        //        {
        //            return base.Json("\"" + Code + "\" is exists!", JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return base.Json("\"" + Code + "\" đ\x00e3 tồn tại!", JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    return base.Json(true, JsonRequestBehavior.AllowGet);
        //}
        #endregion

        #region For Training Expense
        //public JsonResult ValidationTrainingExpense(string Code, string InitialTrainingExpense)
        //{
        //    TrainingExpenseRepository repository = new TrainingExpenseRepository(db);
        //    string errorMessage;
        //    if (repository.CheckExist(Code, out errorMessage) && (Code != InitialTrainingExpense))
        //    {
        //        if (LanguageId == 124)
        //        {
        //            return base.Json("\"" + Code + "\" is exists!", JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return base.Json("\"" + Code + "\" đ\x00e3 tồn tại!", JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    return base.Json(true, JsonRequestBehavior.AllowGet);
        //}
        #endregion

        #region For Training Form
        //public JsonResult ValidationTrainingForm(string Code, string InitialTrainingForm)
        //{
        //    TrainingFormRepository repository = new TrainingFormRepository(db);
        //    string errorMessage;
        //    if (repository.CheckExist(Code, out errorMessage) && (Code != InitialTrainingForm))
        //    {
        //        if (LanguageId == 124)
        //        {
        //            return base.Json("\"" + Code + "\" is exists!", JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return base.Json("\"" + Code + "\" đ\x00e3 tồn tại!", JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    return base.Json(true, JsonRequestBehavior.AllowGet);
        //}
        #endregion

        #region For Training Type
        //public JsonResult ValidationTrainingType(string Code, string InitialTrainingType)
        //{
        //    TrainingTypeRepository repository = new TrainingTypeRepository(db);
        //    string errorMessage;
        //    if (repository.CheckExist(Code, out errorMessage) && (Code != InitialTrainingType))
        //    {
        //        if (LanguageId == 124)
        //        {
        //            return base.Json("\"" + Code + "\" is exists!", JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return base.Json("\"" + Code + "\" đ\x00e3 tồn tại!", JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    return base.Json(true, JsonRequestBehavior.AllowGet);
        //}
        #endregion

        #region For Training Location
        //public JsonResult ValidationTrainingLocation(string Code, string InitialTrainingLocation)
        //{
        //    TrainingLocationRepository repository = new TrainingLocationRepository(db);
        //    string errorMessage;
        //    if (repository.CheckExist(Code, out errorMessage) && (Code != InitialTrainingLocation))
        //    {
        //        if (LanguageId == 124)
        //        {
        //            return base.Json("\"" + Code + "\" is exists!", JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return base.Json("\"" + Code + "\" đ\x00e3 tồn tại!", JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    return base.Json(true, JsonRequestBehavior.AllowGet);
        //}
        #endregion

        #region For Training Course
        //public JsonResult ValidationTrainingCourse(string LSTrainingCourseCode, string InitialTrainingCourse)
        //{
        //    TrainingCourseRepository repository = new TrainingCourseRepository(db);
        //    string errorMessage;
        //    if (repository.CheckExist(LSTrainingCourseCode, out errorMessage) && (LSTrainingCourseCode != InitialTrainingCourse))
        //    {
        //        if (LanguageId == 124)
        //        {
        //            return base.Json("\"" + LSTrainingCourseCode + "\" is exists!", JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return base.Json("\"" + LSTrainingCourseCode + "\" đ\x00e3 tồn tại!", JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    return base.Json(true, JsonRequestBehavior.AllowGet);
        //}
        #endregion

        #region For Payroll SalaryAdjust
        //public JsonResult ValidationSalaryAdjust(string LSSalaryAdjustCode, string InitialSalaryAdjust)
        //{
        //    SalaryAjustRepository repository = new SalaryAjustRepository(db);
        //    string errorMessage;
        //    if (repository.CheckExist(LSSalaryAdjustCode, out errorMessage) && (LSSalaryAdjustCode != InitialSalaryAdjust))
        //    {
        //        if (LanguageId == 124)
        //        {
        //            return base.Json("\"" + LSSalaryAdjustCode + "\" is exists!", JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return base.Json("\"" + LSSalaryAdjustCode + "\" đ\x00e3 tồn tại!", JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    return base.Json(true, JsonRequestBehavior.AllowGet);
        //}
        #endregion
        //public JsonResult ValidationPriority(int Priority, int EmpID, int? InitialPriority)
        //{
        //    QualificationRespository _repository = new QualificationRespository(db);
        //    if (InitialPriority != null)
        //    {
        //        if ((InitialPriority != Priority) && _repository.IsPriorityExists(EmpID, Priority))
        //            return base.Json(LanguageResource.InValidPriority, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        if (_repository.IsPriorityExists(EmpID, Priority))
        //            return base.Json(LanguageResource.InValidPriority, JsonRequestBehavior.AllowGet);
        //    }
        //    return base.Json(true, JsonRequestBehavior.AllowGet);
        //}

        #region For AnswerType
        //public JsonResult ValidationAnswerType(string AnswerTypeCode, string InitialAnswerType)
        //{
        //    var repository = new AnswerTypeRepository(db);
        //    string errorMessage;
        //    if (repository.IsExisted(AnswerTypeCode, out errorMessage) && (AnswerTypeCode != InitialAnswerType))
        //    {
        //        if (LanguageId == 124)
        //        {
        //            return base.Json("\"" + AnswerTypeCode + "\" is exists!", JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return base.Json("\"" + AnswerTypeCode + "\" đ\x00e3 tồn tại!", JsonRequestBehavior.AllowGet);
        //        }
        //    }

        //    return base.Json(true, JsonRequestBehavior.AllowGet);
        //}
        #endregion

        #region For TrainingAnswerType
        //public JsonResult ValidationTrainingAnswerType(string LSTrainingAnswerTypeCode, string InitialTrainingAnswerType)
        //{
        //    var repository = new TrainingAnswerTypeRepository(db); 
        //    string errorMessage;
        //    if (repository.IsExisted(LSTrainingAnswerTypeCode, out errorMessage) && (LSTrainingAnswerTypeCode != InitialTrainingAnswerType))
        //    {
        //        if (LanguageId == 124)
        //        {
        //            return base.Json("\"" + LSTrainingAnswerTypeCode + "\" is exists!", JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return base.Json("\"" + LSTrainingAnswerTypeCode + "\" đ\x00e3 tồn tại!", JsonRequestBehavior.AllowGet);
        //        }
        //    }

        //    return base.Json(true, JsonRequestBehavior.AllowGet);
        //}
        #endregion

        #region For TrainingApprisalPart
        //public JsonResult ValidationTrainingApprisalPart(string LSTrainingApprisalPartCode, string InitialTrainingApprisalPart)
        //{
        //    var repository = new TrainingApprisalPartRepository(db);
        //    string errorMessage;
        //    if (repository.IsExisted(LSTrainingApprisalPartCode, out errorMessage) && (LSTrainingApprisalPartCode != InitialTrainingApprisalPart))
        //    {
        //        if (LanguageId == 124)
        //        {
        //            return base.Json("\"" + LSTrainingApprisalPartCode + "\" is exists!", JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return base.Json("\"" + LSTrainingApprisalPartCode + "\" đ\x00e3 tồn tại!", JsonRequestBehavior.AllowGet);
        //        }
        //    }

        //    return base.Json(true, JsonRequestBehavior.AllowGet);
        //}
        #endregion

        #region For TrainingApprisalItem
        //public JsonResult ValidationTrainingApprisalItem(string LSTrainingApprisalItemCode, string InitialTrainingApprisalItem)
        //{
        //    var repository = new TrainingApprisalItemRepository(db);
        //    string errorMessage;
        //    if (repository.IsExisted(LSTrainingApprisalItemCode, out errorMessage) && (LSTrainingApprisalItemCode != InitialTrainingApprisalItem))
        //    {
        //        if (LanguageId == 124)
        //        {
        //            return base.Json("\"" + LSTrainingApprisalItemCode + "\" is exists!", JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return base.Json("\"" + LSTrainingApprisalItemCode + "\" đ\x00e3 tồn tại!", JsonRequestBehavior.AllowGet);
        //        }
        //    }

        //    return base.Json(true, JsonRequestBehavior.AllowGet);
        //}
        #endregion
    }
}
