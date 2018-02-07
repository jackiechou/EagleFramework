using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Common.Utilities;
using Eagle.Core.Configuration;
using Eagle.Core.Permission;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Companies;
using Eagle.Entities.Business.Employees;
using Eagle.Repositories;
using Eagle.Resources;
using Eagle.Services.Business.Validation;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Business.Personnel;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.SystemManagement.Identity;
using Eagle.Services.EntityMapping.Common;
using Eagle.Services.SystemManagement;
using Eagle.Services.Validations;

namespace Eagle.Services.Business
{
    public class EmployeeService : BaseService, IEmployeeService
    {
        private IContactService ContactService { get; set; }
        public IDocumentService DocumentService { get; set; }
        public IVendorService VendorService { get; set; }
        public EmployeeService(IUnitOfWork unitOfWork, IVendorService vendorService, IContactService contactService, IDocumentService documentService) : base(unitOfWork)
        {
            VendorService = vendorService;
            ContactService = contactService;
            DocumentService = documentService;
        }

        #region Employee
        public IEnumerable<EmployeeInfoDetail> GetEmployees(int vendorId, EmployeeSearchEntry entry, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst  = new List<EmployeeInfoDetail>();
            var employees = UnitOfWork.EmployeeRepository.GetEmployees(vendorId, entry.SearchText, entry.SearchStatus, ref recordCount, orderBy, page, pageSize);
            if (employees!=null)
            {
                foreach (var employee in employees)
                {
                    var contact = ContactService.GetContactInfoDetails(employee.ContactId);
                    var emergencyAddress = employee.EmergencyAddressId != null && employee.EmergencyAddressId > 0 ? ContactService.GetAddressInfoDetail(Convert.ToInt32(employee.EmergencyAddressId)) : null;
                    var permanentAddress = employee.PermanentAddressId != null && employee.PermanentAddressId > 0 ? ContactService.GetAddressInfoDetail(Convert.ToInt32(employee.PermanentAddressId)) : null;

                    var item = new EmployeeInfoDetail
                    {
                        EmployeeId = employee.EmployeeId,
                        EmployeeNo = employee.EmployeeNo,
                        ContactId = employee.ContactId,
                        EmergencyAddressId = employee.EmergencyAddressId,
                        PermanentAddressId = employee.PermanentAddressId,
                        VendorId = employee.VendorId,
                        CompanyId = employee.CompanyId,
                        PositionId = employee.PositionId,
                        JoinedDate = employee.JoinedDate,
                        PasswordHash = employee.PasswordHash,
                        PasswordSalt = employee.PasswordSalt,
                        Status = employee.Status,
                        Contact = contact,
                        EmergencyAddress = emergencyAddress,
                        PermanentAddress = permanentAddress,
                        Company = employee.Company.ToDto<Company, CompanyDetail>(),
                        JobPosition = employee.JobPosition.ToDto<JobPosition, JobPositionDetail>()
                    };

                    lst.Add(item);
                }
            }
            return lst;
        }
        public IEnumerable<EmployeeInfoDetail> GetEmployees(int vendorId, EmployeeStatus? status)
        {
            var lst = new List<EmployeeInfoDetail>();
            var employees = UnitOfWork.EmployeeRepository.GetEmployees(vendorId, status);
            if (employees != null)
            {
                foreach (var employee in employees)
                {
                    var contact = ContactService.GetContactInfoDetails(employee.ContactId);
                    var emergencyAddress = employee.EmergencyAddressId != null && employee.EmergencyAddressId > 0 ? ContactService.GetAddressInfoDetail(Convert.ToInt32(employee.EmergencyAddressId)) : null;
                    var permanentAddress = employee.PermanentAddressId != null && employee.PermanentAddressId > 0 ? ContactService.GetAddressInfoDetail(Convert.ToInt32(employee.PermanentAddressId)) : null;

                    var item = new EmployeeInfoDetail
                    {
                        EmployeeId = employee.EmployeeId,
                        EmployeeNo = employee.EmployeeNo,
                        ContactId = employee.ContactId,
                        EmergencyAddressId = employee.EmergencyAddressId,
                        PermanentAddressId = employee.PermanentAddressId,
                        VendorId = employee.VendorId,
                        CompanyId = employee.CompanyId,
                        PositionId = employee.PositionId,
                        JoinedDate = employee.JoinedDate,
                        PasswordHash = employee.PasswordHash,
                        PasswordSalt = employee.PasswordSalt,
                        Status = employee.Status,
                        Contact = contact,
                        EmergencyAddress = emergencyAddress,
                        PermanentAddress = permanentAddress,
                        Company = employee.Company.ToDto<Company, CompanyDetail>(),
                        JobPosition = employee.JobPosition.ToDto<JobPosition, JobPositionDetail>()
                    };
                    lst.Add(item);
                }
            }
            return lst;
        }
        public SelectList PopulateEmployeeSelectList(int vendorId, EmployeeStatus? status = null, int? selectedValue = null, bool? isShowSelectText = false)
        {
            return UnitOfWork.EmployeeRepository.PopulateEmployeeSelectList(vendorId, status, selectedValue, isShowSelectText);
        }

        public MultiSelectList PopulateEmployeeMultiSelectList(int vendorId, EmployeeStatus? status = null, int[] selectedValues = null)
        {
            return UnitOfWork.EmployeeRepository.PopulateEmployeeMultiSelectList(vendorId, status, selectedValues);
        }
        public EmployeeInfoDetail GetEmployeeDetail(int id)
        {
            var employee = UnitOfWork.EmployeeRepository.GetDetails(id);
            if (employee == null) return new EmployeeInfoDetail();

            var contact = ContactService.GetContactInfoDetails(employee.ContactId);
            var emergencyAddress = employee.EmergencyAddressId != null && employee.EmergencyAddressId > 0 ? ContactService.GetAddressInfoDetail(Convert.ToInt32(employee.EmergencyAddressId)) : null;
            var permanentAddress = employee.PermanentAddressId != null && employee.PermanentAddressId > 0 ? ContactService.GetAddressInfoDetail(Convert.ToInt32(employee.PermanentAddressId)) : null;

            var item = new EmployeeInfoDetail
            {
                EmployeeId = employee.EmployeeId,
                EmployeeNo = employee.EmployeeNo,
                ContactId = employee.ContactId,
                EmergencyAddressId = employee.EmergencyAddressId,
                PermanentAddressId = employee.PermanentAddressId,
                VendorId = employee.VendorId,
                CompanyId = employee.CompanyId,
                PositionId = employee.PositionId,
                JoinedDate = employee.JoinedDate,
                PasswordHash = employee.PasswordHash,
                PasswordSalt = employee.PasswordSalt,
                Status = employee.Status,
                Contact = contact,
                EmergencyAddress = emergencyAddress,
                PermanentAddress = permanentAddress,
                Company = employee.Company.ToDto<Company, CompanyDetail>(),
                JobPosition = employee.JobPosition.ToDto<JobPosition, JobPositionDetail>()
            };
            return item;
        }
        public EmployeeInfoDetail GetEmployeeDetails(int id)
        {
            var employee = UnitOfWork.EmployeeRepository.GetDetails(id);
            if (employee == null) return new EmployeeInfoDetail();

            var contact = ContactService.GetContactInfoDetails(employee.ContactId);
            var emergencyAddress = employee.EmergencyAddressId != null && employee.EmergencyAddressId > 0 ? ContactService.GetAddressInfoDetail(Convert.ToInt32(employee.EmergencyAddressId)) : null;
            var permanentAddress = employee.PermanentAddressId != null && employee.PermanentAddressId > 0 ? ContactService.GetAddressInfoDetail(Convert.ToInt32(employee.PermanentAddressId)) : null;

            return new EmployeeInfoDetail
            {
                EmployeeId = employee.EmployeeId,
                EmployeeNo = employee.EmployeeNo,
                ContactId = employee.ContactId,
                EmergencyAddressId = employee.EmergencyAddressId,
                PermanentAddressId = employee.PermanentAddressId,
                VendorId = employee.VendorId,
                CompanyId = employee.CompanyId,
                PositionId = employee.PositionId,
                JoinedDate = employee.JoinedDate,
                PasswordHash = employee.PasswordHash,
                PasswordSalt = employee.PasswordSalt,
                Status = employee.Status,
                Contact = contact,
                EmergencyAddress = emergencyAddress,
                PermanentAddress = permanentAddress,
                Company = employee.Company.ToDto<Company, CompanyDetail>(),
                JobPosition = employee.JobPosition.ToDto<JobPosition, JobPositionDetail>()
            };
        }

        public string GenerateCode(int maxLetters)
        {
            return UnitOfWork.EmployeeRepository.GenerateCode(maxLetters);
        }
        public void InsertEmployee(Guid applicationId, Guid userId, int vendorId, EmployeeEntry entry)
        {
            string ip = NetworkUtils.GetIP4Address();

            ISpecification<EmployeeEntry> validator = new EmployeeEntryValidator(UnitOfWork, PermissionLevel.Edit, CurrentClaimsIdentity);
            var dataViolations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isDataValid) throw new ValidationError(dataViolations);

            var entity = entry.ToEntity<EmployeeEntry, Employee>();
            entity.VendorId = vendorId;
            entity.Ip = ip;
            entity.CreatedByUserId = userId;
            entity.CreatedDate = DateTime.UtcNow;

            UnitOfWork.EmployeeRepository.Insert(entity);
            UnitOfWork.SaveChanges();

            if (entry.Contact != null)
            {
                var newContact = ContactService.InsertContact(entry.Contact);
                if (newContact != null)
                {
                    entity.ContactId = newContact.ContactId;

                    if (entry.Contact.FileUpload != null && entry.Contact.FileUpload.ContentLength > 0)
                    {
                        int maxContentLength = UnitOfWork.ApplicationSettingRepository.GetAllowedMaxImageContentLength(applicationId);
                        string[] allowedFileExtensions = UnitOfWork.ApplicationSettingRepository.GetAllowedImageExtensions(applicationId);

                        if (!allowedFileExtensions.Contains(entry.Contact.FileUpload.FileName.Substring(entry.Contact.FileUpload.FileName.LastIndexOf('.'))))
                        {
                            dataViolations.Add(new RuleViolation(ErrorCode.InvalidFileType, "FileUpload", LanguageResource.InvalidFileExtension + " : " + string.Join(", ", allowedFileExtensions)));
                            throw new ValidationError(dataViolations);
                        }
                        else if (entry.Contact.FileUpload.ContentLength > maxContentLength)
                        {
                            dataViolations.Add(new RuleViolation(ErrorCode.MaximumAllowedSize, "FileUpload", LanguageResource.InvalidFileSize + " , " + LanguageResource.MaximumAllowedSize + maxContentLength + " MB"));
                            throw new ValidationError(dataViolations);
                        }
                        else
                        {
                            var photoInfo = DocumentService.UploadAndSaveDbByFolderId(applicationId, userId, entry.Contact.FileUpload, (int)FileLocation.Employee, StorageType.Local);
                            if (photoInfo != null)
                            {
                                ContactService.UpdateContactPhoto(newContact.ContactId, Convert.ToInt32(photoInfo.FileId));
                            }
                        }
                    }
                }
            }
            
            if (entry.EmergencyAddress != null && entry.EmergencyAddress.CountryId != null)
            {
                var newEmergencyAddress = ContactService.InsertAddress(entry.EmergencyAddress);
                if (newEmergencyAddress != null)
                {
                    entity.EmergencyAddressId = newEmergencyAddress.AddressId;
                }
            }

            if (entry.PermanentAddress != null && entry.PermanentAddress.CountryId != null)
            {
                var newPermanentAddress = ContactService.InsertAddress(entry.PermanentAddress);
                if (newPermanentAddress != null)
                {
                    entity.PermanentAddressId = newPermanentAddress.AddressId;
                }
            }
            UnitOfWork.EmployeeRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateEmployee(Guid applicationId, Guid userId, int vendorId, EmployeeEditEntry entry)
        {
            ISpecification<EmployeeEditEntry> validator = new EmployeeEditEntryValidator(UnitOfWork, PermissionLevel.Edit, CurrentClaimsIdentity);
            var violations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, violations);
            if (!isDataValid) throw new ValidationError(violations);

            int? emergencyAddressId = null, permanentAddressId = null;
            var entity = UnitOfWork.EmployeeRepository.FindById(entry.EmployeeId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullEmployee, "Employee", null, ErrorMessage.Messages[ErrorCode.NullEmployee]));
                throw new ValidationError(violations);
            }

            if (entity.EmployeeNo != entry.EmployeeNo)
            {
                bool isDuplicate = UnitOfWork.EmployeeRepository.HasEmployeeNumberExisted(entry.EmployeeNo);
                if (isDuplicate)
                {
                    violations.Add(new RuleViolation(ErrorCode.DuplicateEmployeeNo, "EmployeeNo", entry.EmployeeNo, ErrorMessage.Messages[ErrorCode.InvalidEmployeeNo]));
                    throw new ValidationError(violations);
                }
            }

            if (entity.ContactId > 0)
            {
                //Update Contact
                var contactEditEntry = new ContactEditEntry
                {
                    ContactId = entry.Contact.ContactId,
                    FirstName = entry.Contact.FirstName,
                    LastName = entry.Contact.LastName,
                    DisplayName = entry.Contact.DisplayName,
                    Sex = entry.Contact.Sex,
                    JobTitle = entry.Contact.JobTitle,
                    Dob = entry.Contact.Dob,
                    LinePhone1 = entry.Contact.LinePhone1,
                    LinePhone2 = entry.Contact.LinePhone2,
                    Mobile = entry.Contact.Mobile,
                    Fax = entry.Contact.Fax,
                    Email = entry.Contact.Email,
                    Website = entry.Contact.Website,
                    IdNo = entry.Contact.IdNo,
                    IdIssuedDate = entry.Contact.IdIssuedDate,
                    TaxNo = entry.Contact.TaxNo,
                    Photo = entry.Contact.Photo
                };

                if (entry.Contact.FileUpload != null && entry.Contact.FileUpload.ContentLength > 0)
                {
                    int maxContentLength = UnitOfWork.ApplicationSettingRepository.GetAllowedMaxImageContentLength(applicationId);
                    string[] allowedFileExtensions = UnitOfWork.ApplicationSettingRepository.GetAllowedImageExtensions(applicationId);

                    if (!allowedFileExtensions.Contains(entry.Contact.FileUpload.FileName.Substring(entry.Contact.FileUpload.FileName.LastIndexOf('.'))))
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidFileType, "FileUpload", LanguageResource.InvalidFileExtension + " : " + string.Join(", ", allowedFileExtensions)));
                        throw new ValidationError(violations);
                    }
                    else if (entry.Contact.FileUpload.ContentLength > maxContentLength)
                    {
                        violations.Add(new RuleViolation(ErrorCode.MaximumAllowedSize, "FileUpload", LanguageResource.InvalidFileSize + " , " + LanguageResource.MaximumAllowedSize + maxContentLength + " MB"));
                        throw new ValidationError(violations);
                    }
                    else
                    {
                        var photoInfo = DocumentService.UploadAndSaveDbByFolderId(applicationId, userId, entry.Contact.FileUpload, (int)FileLocation.Employee, StorageType.Local);
                        if (photoInfo != null)
                        {
                            contactEditEntry.Photo = photoInfo.FileId;
                        }
                    }
                }

                ContactService.UpdateContact(contactEditEntry);
            }
            else
            {
                var newContactEntry = new ContactEntry
                {
                    FirstName = entry.Contact.FirstName,
                    LastName = entry.Contact.LastName,
                    DisplayName = entry.Contact.DisplayName,
                    Sex = entry.Contact.Sex,
                    JobTitle = entry.Contact.JobTitle,
                    Dob = entry.Contact.Dob,
                    LinePhone1 = entry.Contact.LinePhone1,
                    LinePhone2 = entry.Contact.LinePhone2,
                    Mobile = entry.Contact.Mobile,
                    Fax = entry.Contact.Fax,
                    Email = entry.Contact.Email,
                    Website = entry.Contact.Website,
                    IdNo = entry.Contact.IdNo,
                    IdIssuedDate = entry.Contact.IdIssuedDate,
                    TaxNo = entry.Contact.TaxNo
                };
                var newContact = ContactService.InsertContact(newContactEntry);
                if (newContact != null)
                {
                    entity.ContactId = newContact.ContactId;

                    if (entry.Contact.FileUpload != null && entry.Contact.FileUpload.ContentLength > 0)
                    {
                        int maxContentLength = UnitOfWork.ApplicationSettingRepository.GetAllowedMaxImageContentLength(applicationId);
                        string[] allowedFileExtensions = UnitOfWork.ApplicationSettingRepository.GetAllowedImageExtensions(applicationId);

                        if (!allowedFileExtensions.Contains(
                            entry.Contact.FileUpload.FileName.Substring(entry.Contact.FileUpload.FileName
                                .LastIndexOf('.'))))
                        {
                            violations.Add(new RuleViolation(ErrorCode.InvalidFileType, "FileUpload",
                                LanguageResource.InvalidFileExtension + " : " +
                                string.Join(", ", allowedFileExtensions)));
                            throw new ValidationError(violations);
                        }
                        else if (entry.Contact.FileUpload.ContentLength > maxContentLength)
                        {
                            violations.Add(new RuleViolation(ErrorCode.MaximumAllowedSize, "FileUpload",
                                LanguageResource.InvalidFileSize + " , " + LanguageResource.MaximumAllowedSize +
                                maxContentLength + " MB"));
                            throw new ValidationError(violations);
                        }
                        else
                        {
                            var photoInfo = DocumentService.UploadAndSaveDbByFolderId(applicationId, userId,
                                entry.Contact.FileUpload, (int)FileLocation.Employee, StorageType.Local);
                            if (photoInfo != null)
                            {
                                int photo = photoInfo.FileId;
                                ContactService.UpdateContactPhoto(newContact.ContactId, photo);
                            }
                        }
                    }
                }
            }

            //Update Emergency Address
            if (entry.EmergencyAddress != null)
            {
                emergencyAddressId = ContactService.UpdateEmergencyAddress(entry.EmergencyAddress);
            }

            //Update Permanent Address
            if (entry.PermanentAddress != null)
            {
                permanentAddressId = ContactService.UpdatePermanentAddress(entry.PermanentAddress);
            }

            //Update employee
            entity.EmployeeNo = entry.EmployeeNo;
            entity.CompanyId = entry.CompanyId;
            entity.PositionId = entry.PositionId;
            entity.JoinedDate = entry.JoinedDate;
            entity.PositionId = entry.PositionId;
            entity.JoinedDate = entry.JoinedDate;
            entity.EmergencyAddressId = emergencyAddressId;
            entity.PermanentAddressId = permanentAddressId;
            entity.Status = entry.Status;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedDate = DateTime.UtcNow;
            entity.LastModifiedByUserId = userId;

            UnitOfWork.EmployeeRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateEmployeeStatus(Guid userId, int id, EmployeeStatus status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.EmployeeRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundEmployee, "Employee", id, ErrorMessage.Messages[ErrorCode.NotFoundEmployee]));
                throw new ValidationError(violations);
            }

            var isValid = Enum.IsDefined(typeof(EmployeeStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status", null,
                    ErrorMessage.Messages[ErrorCode.InvalidStatus]));
                throw new ValidationError(violations);
            }
            if (entity.Status == status) return;

            entity.Status = status;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedDate = DateTime.UtcNow;
            entity.LastModifiedByUserId = userId;

            UnitOfWork.EmployeeRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion
        
        #region Job Position
        public IEnumerable<JobPositionDetail> GetJobPositions(JobPositionSearchEntry filter, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = UnitOfWork.JobPositionRepository.GetJobPositions(filter.PositionName, filter.IsActive, ref recordCount, orderBy, page, pageSize);
            return lst.ToDtos<JobPosition, JobPositionDetail>();
        }

        public JobPositionDetail GetJobPositionDetail(int id)
        {
            var entity = UnitOfWork.JobPositionRepository.FindById(id);
            return entity.ToDto<JobPosition, JobPositionDetail>();
        }

        public SelectList PoplulateJobPositionSelectList(bool? status = null,  int ? selectedValue=null, bool? isShowSelectText = true)
        {
            return UnitOfWork.JobPositionRepository.PopulateJobPositionSelectList(status,selectedValue, isShowSelectText);
        }

        public SelectList PopulateJobPositionStatus(bool? selectedValue = true, bool isShowSelectText = false)
        {
            return UnitOfWork.JobPositionRepository.PopulateJobPositionStatus(selectedValue, isShowSelectText);
        }

        public JobPositionDetail InsertJobPosition(JobPositionEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullJobPositionEntry, "JobPositionEntry", null, ErrorMessage.Messages[ErrorCode.NullJobPositionEntry]));
                throw new ValidationError(violations);
            }

            if (string.IsNullOrEmpty(entry.PositionName))
            {
                violations.Add(new RuleViolation(ErrorCode.NullPositionName, "PositionName", null, ErrorMessage.Messages[ErrorCode.NullPositionName]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.PositionName.Length > 300)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidPositionName, "PositionName", null, ErrorMessage.Messages[ErrorCode.InvalidPositionName]));
                    throw new ValidationError(violations);
                }
                else
                {
                    bool isDuplicate = UnitOfWork.JobPositionRepository.HasDataExisted(entry.PositionName);
                    if (isDuplicate)
                    {
                        violations.Add(new RuleViolation(ErrorCode.DuplicatePositionName, "PositionName",
                                entry.PositionName));
                        throw new ValidationError(violations);
                    }
                }
            }

            var entity = entry.ToEntity<JobPositionEntry, JobPosition>();

            UnitOfWork.JobPositionRepository.Insert(entity);
            UnitOfWork.SaveChanges();
            return entity.ToDto<JobPosition, JobPositionDetail>();
        }
        public void UpdateJobPosition(JobPositionEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullJobPositionEditEntry, "JobPositionEditEntry", null, ErrorMessage.Messages[ErrorCode.NullJobPositionEditEntry]));
                throw new ValidationError(violations);
            }

            var entity = UnitOfWork.JobPositionRepository.FindById(entry.PositionId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidPositionId, "PositionId", entry.PositionId, ErrorMessage.Messages[ErrorCode.InvalidPositionId]));
                throw new ValidationError(violations);
            }

            if (string.IsNullOrEmpty(entry.PositionName))
            {
                violations.Add(new RuleViolation(ErrorCode.NullPositionName, "JobPositionName", null, ErrorMessage.Messages[ErrorCode.NullPositionName]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.PositionName.Length > 300)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidPositionName, "JobPositionName", null, ErrorMessage.Messages[ErrorCode.InvalidPositionName]));
                    throw new ValidationError(violations);
                }
                else
                {
                    if (entity.PositionName != entry.PositionName)
                    {
                        bool isDuplicate = UnitOfWork.JobPositionRepository.HasDataExisted(entry.PositionName);
                        if (isDuplicate)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicatePositionName, "JobPositionName",
                                    entry.PositionName));
                            throw new ValidationError(violations);
                        }
                    }
                }
            }

            entity.PositionName = entry.PositionName;
            entity.Description = entry.Description;
            entity.IsActive = entry.IsActive;

            UnitOfWork.JobPositionRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateJobPositionStatus(int id, bool status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.JobPositionRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundJobPosition, "JobPosition", id, ErrorMessage.Messages[ErrorCode.NotFoundJobPosition]));
                throw new ValidationError(violations);
            }
            if (entity.IsActive == status) return;

            entity.IsActive = status;

            UnitOfWork.JobPositionRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        #endregion

        #region Job Position Skill
        public void InsertJobPositionSkill(int positionId, int skillId)
        {
            var violations = new List<RuleViolation>();
            if (skillId <= 0)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundSkill, "SkillId", skillId, ErrorMessage.Messages[ErrorCode.NotFoundSkill]));
                throw new ValidationError(violations);
            }
            else
            {
                var skill = UnitOfWork.SkillRepository.FindById(skillId);
                if (skill == null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidSkillId, "SkillId", skillId,
                    ErrorMessage.Messages[ErrorCode.InvalidSkillId]));
                    throw new ValidationError(violations);
                }
            }

            var position = UnitOfWork.JobPositionRepository.FindById(positionId);
            if (position == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidPositionId, "PositionId", positionId, ErrorMessage.Messages[ErrorCode.InvalidPositionId]));
                throw new ValidationError(violations);
            }

            var jobPositionSkill = new JobPositionSkill
            {
                PositionId = positionId,
                SkillId = skillId
            };
            UnitOfWork.JobPositionSkillRepository.Insert(jobPositionSkill);
            UnitOfWork.SaveChanges();
        }

        public void DeleteJobPositionSkill(int positionId, int skillId)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.JobPositionSkillRepository.GetDetails(positionId, skillId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundJobPositionSkill, "PositionId",
                    $"{positionId} {skillId}", ErrorMessage.Messages[ErrorCode.NotFoundJobPositionSkill]));
                throw new ValidationError(violations);
            }

            UnitOfWork.EmployeePositionRepository.Delete(entity);
            UnitOfWork.SaveChanges();
        }

        #endregion

        #region Employee Position
        public void InsertEmployeePosition(int employeeId, JobPositionEntry entry)
        {
            var item = InsertJobPosition(entry);
            if (item.PositionId > 0)
            {
                var employeePositionEntity = new EmployeePosition
                {
                    EmployeeId = employeeId,
                    PositionId = item.PositionId
                };
                UnitOfWork.EmployeePositionRepository.Insert(employeePositionEntity);
                UnitOfWork.SaveChanges();
            }
        }

        public void DeleteEmployeePosition(int employeeId, int positionId)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.EmployeePositionRepository.GetEmployeePositionDetail(employeeId, positionId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundPosition, "PositionId", positionId, ErrorMessage.Messages[ErrorCode.NotFoundPosition]));
                throw new ValidationError(violations);
            }

            UnitOfWork.EmployeePositionRepository.Delete(entity);
            UnitOfWork.SaveChanges();
        }

        #endregion

        #region Skill
        public IEnumerable<SkillDetail> GetSkillsByMember(int employeeId)
        {
            var entities = UnitOfWork.SkillRepository.GetSkillsByEmployee(employeeId);
            return entities.ToDtos<Skill, SkillDetail>();
        }
        public SkillDetail GetSkillDetail(int skillId)
        {
            var skill = UnitOfWork.SkillRepository.FindById(skillId);
            return skill.ToDto<Skill, SkillDetail>();
        }
        public SkillDetail InsertSkill(SkillEntry entry)
        {
            ISpecification<SkillEntry> validator = new SkillEntryValidator(UnitOfWork, PermissionLevel.Create, CurrentClaimsIdentity);
            var violations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, violations);
            if (!isDataValid) throw new ValidationError(violations);

            var entity = entry.ToEntity<SkillEntry, Skill>();
            UnitOfWork.SkillRepository.Insert(entity);
            UnitOfWork.SaveChanges();
            
            return entity.ToDto<Skill, SkillDetail>();
        }
        public void UpdateSkill(SkillEditEntry entry)
        {
            ISpecification<SkillEditEntry> validator = new SkillEditEntryValidator(UnitOfWork, PermissionLevel.Edit, CurrentClaimsIdentity);
            var violations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, violations);
            if (!isDataValid) throw new ValidationError(violations);

            var entity = UnitOfWork.SkillRepository.FindById(entry.SkillId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullSkill, "SkillId", entry.SkillId, ErrorMessage.Messages[ErrorCode.NullSkill]));
                throw new ValidationError(violations);
            }

            if (entity.SkillName != entry.SkillName)
            {
                bool isDuplicate = UnitOfWork.SkillRepository.HasNameExisted(entry.SkillName);
                if (isDuplicate)
                {
                    violations.Add(new RuleViolation(ErrorCode.DuplicateSkillName, "SkillName",
                            entry.SkillName, ErrorMessage.Messages[ErrorCode.DuplicateSkillName]));
                    throw new ValidationError(violations);
                }
            }

            entity.SkillName = entry.SkillName;
            entity.ModifiedDate = DateTime.UtcNow;

            UnitOfWork.SkillRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void DeleteSkill(int id)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.SkillRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundSkill, "SkillId", id, ErrorMessage.Messages[ErrorCode.NotFoundSkill]));
                throw new ValidationError(violations);
            }
           
            UnitOfWork.SkillRepository.Delete(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region Employee Skill

        public void AssignSkill(int employeeId, int skillId)
        {
            var violations = new List<RuleViolation>();
            if (skillId <= 0)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidSkillId, "SkillId", skillId, ErrorMessage.Messages[ErrorCode.InvalidSkillId]));
                throw new ValidationError(violations);
            }

            var employee = UnitOfWork.EmployeeRepository.FindById(employeeId);
            if (employee == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullEmployee, "EmployeeId",
                        employeeId, ErrorMessage.Messages[ErrorCode.NullEmployee]));
                throw new ValidationError(violations);
            }

            var skill = UnitOfWork.SkillRepository.FindById(skillId);
            if (skill == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundSkill, "SkillId", skillId, ErrorMessage.Messages[ErrorCode.NotFoundSkill]));
                throw new ValidationError(violations);
            }

            var employeeSkillEntity = new EmployeeSkill
            {
                EmployeeId = employeeId,
                SkillId = skillId
            };
            UnitOfWork.EmployeeSkillRepository.Insert(employeeSkillEntity);
            UnitOfWork.SaveChanges();
        }
        public void UnAssignSkill(int employeeId, int skillId)
        {
            var violations = new List<RuleViolation>();
            if (skillId <= 0)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidSkillId, "SkillId", skillId, ErrorMessage.Messages[ErrorCode.InvalidSkillId]));
                throw new ValidationError(violations);
            }

            var employee = UnitOfWork.EmployeeRepository.FindById(employeeId);
            if (employee == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullEmployee, "EmployeeId",
                        employeeId, ErrorMessage.Messages[ErrorCode.NullEmployee]));
                throw new ValidationError(violations);
            }

            var skill = UnitOfWork.SkillRepository.FindById(skillId);
            if (skill == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundSkill, "SkillId", skillId, ErrorMessage.Messages[ErrorCode.NotFoundSkill]));
                throw new ValidationError(violations);
            }

            var entity = UnitOfWork.EmployeeSkillRepository.GetEmployeeSkillDetail(employeeId, skillId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundSkill, "SkillId", skillId, ErrorMessage.Messages[ErrorCode.NotFoundSkill]));
                throw new ValidationError(violations);
            }

            UnitOfWork.EmployeeSkillRepository.Delete(entity);
            UnitOfWork.SaveChanges();
        }

        #endregion

        #region Employee Availability

        public void InsertEmployeeAvailability(EmployeeAvailabilityEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullEmployeeAvailabilityEntry, "EmployeeAvailabilityEntry", null, ErrorMessage.Messages[ErrorCode.NullEmployeeAvailabilityEntry]));
                throw new ValidationError(violations);
            }

            var employee = UnitOfWork.EmployeeRepository.FindById(entry.EmployeeId);
            if (employee == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullEmployee, "EmployeeId",
                        entry.EmployeeId, ErrorMessage.Messages[ErrorCode.NullEmployee]));
                throw new ValidationError(violations);
            }

            if (string.IsNullOrEmpty(entry.TimeZoneId))
            {
                violations.Add(new RuleViolation(ErrorCode.NullTimeZoneId, "TimeZoneId", null, ErrorMessage.Messages[ErrorCode.NullTimeZoneId]));
                throw new ValidationError(violations);
            }

            var entity = entry.ToEntity<EmployeeAvailabilityEntry, EmployeeAvailability>();
            UnitOfWork.EmployeeAvailabilityRepository.Insert(entity);
            UnitOfWork.SaveChanges();
        }

        public void DeleteEmployeeAvailability(int employeeAvailabilityId)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.EmployeeAvailabilityRepository.FindById(employeeAvailabilityId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullEmployeeAvailability, "employeeAvailabilityId", employeeAvailabilityId, ErrorMessage.Messages[ErrorCode.NullEmployeeAvailability]));
                throw new ValidationError(violations);
            }

            UnitOfWork.EmployeeAvailabilityRepository.Delete(entity);
            UnitOfWork.SaveChanges();
        }

        #endregion

        #region Employee Time-Off

        public void InsertEmployeeTimeOff(EmployeeTimeOffEntry entry)
        {
            ISpecification<EmployeeTimeOffEntry> validator = new EmployeeTimeOffEntryValidator(UnitOfWork, PermissionLevel.Edit, CurrentClaimsIdentity);
            var violations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, violations);
            if (!isDataValid) throw new ValidationError(violations);

            var entity = entry.ToEntity<EmployeeTimeOffEntry, EmployeeTimeOff>();
            UnitOfWork.EmployeeTimeOffRepository.Insert(entity);
            UnitOfWork.SaveChanges();
        }

        public void DeleteEmployeeTimeOff(int employeeTimeOffId)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.EmployeeTimeOffRepository.FindById(employeeTimeOffId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullEmployeeTimeOff, "EmployeeTimeOffId", employeeTimeOffId, ErrorMessage.Messages[ErrorCode.NullEmployeeTimeOff]));
                throw new ValidationError(violations);
            }

            UnitOfWork.EmployeeTimeOffRepository.Delete(entity);
            UnitOfWork.SaveChanges();
        }

        #endregion

        #region Qualification
        public string GenerateQualificationNo(int maxLetters)
        {
            return UnitOfWork.QualificationRepository.GenerateQualificationNo(maxLetters);
        }
        public QualificationDetail GetQualificationDetail(int id)
        {
            var entity = UnitOfWork.QualificationRepository.FindById(id);
            return entity.ToDto<Qualification, QualificationDetail>();
        }
        public QualificationDetail InsertQualification(Guid applicationId, Guid userId, QualificationEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullQualificationEntry, "QualificationEntry", null, ErrorMessage.Messages[ErrorCode.NullQualificationEntry]));
                throw new ValidationError(violations);
            }

            if (string.IsNullOrEmpty(entry.QualificationNo))
            {
                violations.Add(new RuleViolation(ErrorCode.NullQualificationNo, "QualificationNo", null, ErrorMessage.Messages[ErrorCode.NullQualificationNo]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.QualificationNo.Length > 50)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidQualificationNo, "QualificationNo", null, ErrorMessage.Messages[ErrorCode.InvalidQualificationNo]));
                    throw new ValidationError(violations);
                }
                else
                {
                    bool isDuplicate = UnitOfWork.QualificationRepository.HasQualificationNoExisted(entry.QualificationNo);
                    if (isDuplicate)
                    {
                        violations.Add(new RuleViolation(ErrorCode.DuplicateQualificationNo, "QualificationNo",
                                entry.QualificationNo, ErrorMessage.Messages[ErrorCode.DuplicateQualificationNo]));
                        throw new ValidationError(violations);
                    }
                }
            }

            var employee = UnitOfWork.EmployeeRepository.FindById(entry.EmployeeId);
            if (employee == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullEmployee, "EmployeeId",
                        entry.EmployeeId, ErrorMessage.Messages[ErrorCode.NullEmployee]));
                throw new ValidationError(violations);
            }

            var entity = entry.ToEntity<QualificationEntry, Qualification>();
            if (entry.File != null && entry.File.ContentLength > 0)
            {
                int maxContentLength = UnitOfWork.ApplicationSettingRepository.GetAllowedMaxImageContentLength(applicationId);
                string[] allowedFileExtensions = UnitOfWork.ApplicationSettingRepository.GetAllowedImageExtensions(applicationId);

                if (!allowedFileExtensions.Contains(entry.File.FileName.Substring(entry.File.FileName.LastIndexOf('.'))))
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidFileType, "FileUpload", LanguageResource.InvalidFileExtension + " : " + string.Join(", ", allowedFileExtensions)));
                    throw new ValidationError(violations);
                }
                else if (entry.File.ContentLength > maxContentLength)
                {
                    violations.Add(new RuleViolation(ErrorCode.MaximumAllowedSize, "FileUpload", LanguageResource.InvalidFileSize + " , " + LanguageResource.MaximumAllowedSize + maxContentLength + " MB"));
                    throw new ValidationError(violations);
                }
                else
                {
                    var fileInfo = DocumentService.UploadAndSaveDbByFolderId(applicationId, userId, entry.File, (int)FileLocation.Qualification, StorageType.Local);
                    entity.FileId = fileInfo.FileId;
                }
            }

            UnitOfWork.QualificationRepository.Insert(entity);
            UnitOfWork.SaveChanges();
            return entity.ToDto<Qualification, QualificationDetail>();
        }
        public void UpdateQualification(Guid applicationId, Guid userId, QualificationEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullQualificationEditEntry, "QualificationEditEntry", null, ErrorMessage.Messages[ErrorCode.NullQualificationEditEntry]));
                throw new ValidationError(violations);
            }

            var entity = UnitOfWork.QualificationRepository.FindById(entry.QualificationId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundQualification, "QualificationId", entry.QualificationId, ErrorMessage.Messages[ErrorCode.NotFoundQualification]));
                throw new ValidationError(violations);
            }

            if (string.IsNullOrEmpty(entry.QualificationNo))
            {
                violations.Add(new RuleViolation(ErrorCode.NullQualificationNo, "QualificationNo", null, ErrorMessage.Messages[ErrorCode.NullQualificationNo]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.QualificationNo.Length > 50)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidQualificationNo, "QualificationNo", null, ErrorMessage.Messages[ErrorCode.InvalidQualificationNo]));
                    throw new ValidationError(violations);
                }
                else
                {
                    if (entity.QualificationNo != entry.QualificationNo)
                    {
                        bool isDuplicate = UnitOfWork.QualificationRepository.HasQualificationNoExisted(entry.QualificationNo);
                        if (isDuplicate)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicateQualificationNo, "QualificationNo",
                                    entry.QualificationNo, ErrorMessage.Messages[ErrorCode.DuplicateQualificationNo]));
                            throw new ValidationError(violations);
                        }
                    }
                }
            }

            var employee = UnitOfWork.EmployeeRepository.FindById(entry.EmployeeId);
            if (employee == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullEmployee, "EmployeeId",
                        entry.EmployeeId, ErrorMessage.Messages[ErrorCode.NullEmployee]));
                throw new ValidationError(violations);
            }

            if (entry.File != null && entry.File.ContentLength > 0)
            {
                if (entity.FileId != null)
                {
                    DocumentService.DeleteFile(Convert.ToInt32(entity.FileId));
                }

                var fileInfo = DocumentService.UploadAndSaveDbByFolderId(applicationId, userId, entry.File, (int)FileLocation.Qualification, StorageType.Local);
                if (entity.FileId != null)
                {
                    entity.FileId = fileInfo.FileId;
                }
            }

            entity.QualificationNo = entry.QualificationNo;
            entity.QualificationDate = entry.QualificationDate;
            entity.Note = entry.Note;
            entity.EmployeeId = entry.EmployeeId;

            UnitOfWork.QualificationRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void DeleteQualification(int qualificationId)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.QualificationRepository.FindById(qualificationId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidQualificationId, "QualificationId", qualificationId, ErrorMessage.Messages[ErrorCode.InvalidQualificationId]));
                throw new ValidationError(violations);
            }

            UnitOfWork.QualificationRepository.Delete(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region Reward Discipline
        public RewardDisciplineDetail InsertRewardDiscipline(RewardDisciplineEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullRewardDisciplineEntry, "RewardDisciplineEntry", null, ErrorMessage.Messages[ErrorCode.NullRewardDisciplineEntry]));
                throw new ValidationError(violations);
            }

            var employee = UnitOfWork.EmployeeRepository.FindById(entry.EmployeeId);
            if (employee == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullEmployee, "EmployeeId",
                        entry.EmployeeId, ErrorMessage.Messages[ErrorCode.NullEmployee]));
                throw new ValidationError(violations);
            }

            var entity = entry.ToEntity<RewardDisciplineEntry, RewardDiscipline>();

            UnitOfWork.RewardDisciplineRepository.Insert(entity);
            UnitOfWork.SaveChanges();
            return entity.ToDto<RewardDiscipline, RewardDisciplineDetail>();
        }

        public void UpdateRewardDiscipline(RewardDisciplineEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullJobPositionEditEntry, "JobPositionEditEntry", null, ErrorMessage.Messages[ErrorCode.NullJobPositionEditEntry]));
                throw new ValidationError(violations);
            }

            var entity = UnitOfWork.RewardDisciplineRepository.FindById(entry.RewardDisciplineId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundRewardDiscipline, "RewardDisciplineId", entry.RewardDisciplineId, ErrorMessage.Messages[ErrorCode.NotFoundRewardDiscipline]));
                throw new ValidationError(violations);
            }

            var employee = UnitOfWork.EmployeeRepository.FindById(entry.EmployeeId);
            if (employee == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullEmployee, "EmployeeId",
                        entry.EmployeeId, ErrorMessage.Messages[ErrorCode.NullEmployee]));
                throw new ValidationError(violations);
            }
            
            entity.EmployeeId = entry.EmployeeId;
            entity.SignedDate = entry.SignedDate;
            entity.EffectiveDate = entry.EffectiveDate;
            entity.IsReward = entry.IsReward;
            entity.Reason = entry.Reason;

            UnitOfWork.RewardDisciplineRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region Salary
        public SalaryDetail InsertSalary(SalaryEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullJobPositionEntry, "JobPositionEntry", null, ErrorMessage.Messages[ErrorCode.NullJobPositionEntry]));
                throw new ValidationError(violations);
            }

            if (string.IsNullOrEmpty(entry.CurrencyCode))
            {
                violations.Add(new RuleViolation(ErrorCode.NullCurrencyCode, "CurrencyCode", null, ErrorMessage.Messages[ErrorCode.NullCurrencyCode]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.CurrencyCode.Length > 10)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidCurrencyCode, "CurrencyCode", null, ErrorMessage.Messages[ErrorCode.InvalidCurrencyCode]));
                    throw new ValidationError(violations);
                }
                else
                {
                    var item = UnitOfWork.CurrencyRepository.GetDetail(entry.CurrencyCode);
                    if (item ==null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidCurrencyCode, "CurrencyCode",
                                entry.CurrencyCode, ErrorMessage.Messages[ErrorCode.InvalidCurrencyCode]));
                        throw new ValidationError(violations);
                    }
                }
            }

            var entity = entry.ToEntity<SalaryEntry, Salary>();

            UnitOfWork.SalaryRepository.Insert(entity);
            UnitOfWork.SaveChanges();
            return entity.ToDto<Salary, SalaryDetail>();
        }

        public void UpdateSalary(SalaryEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullJobPositionEditEntry, "JobPositionEditEntry", null, ErrorMessage.Messages[ErrorCode.NullJobPositionEditEntry]));
                throw new ValidationError(violations);
            }

            var entity = UnitOfWork.SalaryRepository.FindById(entry.SalaryId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullSalary, "SalaryId", entry.SalaryId, ErrorMessage.Messages[ErrorCode.NullSalary]));
                throw new ValidationError(violations);
            }

            if (string.IsNullOrEmpty(entry.CurrencyCode))
            {
                violations.Add(new RuleViolation(ErrorCode.NullCurrencyCode, "CurrencyCode", null, ErrorMessage.Messages[ErrorCode.NullCurrencyCode]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.CurrencyCode.Length > 10)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidCurrencyCode, "CurrencyCode", null, ErrorMessage.Messages[ErrorCode.InvalidCurrencyCode]));
                    throw new ValidationError(violations);
                }
                else
                {
                    var item = UnitOfWork.CurrencyRepository.GetDetail(entry.CurrencyCode);
                    if (item == null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidCurrencyCode, "CurrencyCode",
                                entry.CurrencyCode, ErrorMessage.Messages[ErrorCode.InvalidCurrencyCode]));
                        throw new ValidationError(violations);
                    }
                }
            }

            entity.EmployeeId = entry.EmployeeId;
            entity.SignedDate = entry.SignedDate;
            entity.BasicSalary = entry.BasicSalary;
            entity.ActualSalary = entry.ActualSalary;
            entity.GrossSalary = entry.GrossSalary;
            entity.InsuranceFee = entry.InsuranceFee;
            entity.CurrencyCode = entry.CurrencyCode;

            UnitOfWork.SalaryRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }


        #endregion

        #region Termination
      
        public TerminationDetail GetTerminationDetailByEmployeeId(int employeeId)
        {
            var termination = UnitOfWork.TerminationRepository.GetTerminationDetailByEmployeeId(employeeId);
            return termination.ToDto<Termination, TerminationDetail>();
        }
        public TerminationDetail GetTerminationDetail(int terminationId)
        {
            var termination = UnitOfWork.TerminationRepository.FindById(terminationId);
            return termination.ToDto<Termination, TerminationDetail>();
        }
        public TerminationDetail InsertTermination(TerminationEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullTerminationEntry, "TerminationEntry", null, ErrorMessage.Messages[ErrorCode.NullTerminationEntry]));
                throw new ValidationError(violations);
            }

            var employee = UnitOfWork.EmployeeRepository.FindById(entry.EmployeeId);
            if (employee == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullEmployee, "EmployeeId",
                        entry.EmployeeId, ErrorMessage.Messages[ErrorCode.NullEmployee]));
                throw new ValidationError(violations);
            }

            if (string.IsNullOrEmpty(entry.Reason))
            {
                violations.Add(new RuleViolation(ErrorCode.NullReason, "Reason", null, ErrorMessage.Messages[ErrorCode.NullReason]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.Reason.Length > 500)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidReason, "Reason", entry.Reason, ErrorMessage.Messages[ErrorCode.InvalidReason]));
                    throw new ValidationError(violations);
                }
            }

            var entity = entry.ToEntity<TerminationEntry, Termination>();

            UnitOfWork.TerminationRepository.Insert(entity);
            UnitOfWork.SaveChanges();
            return entity.ToDto<Termination, TerminationDetail>();
        }
        public void UpdateTermination(TerminationEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullTerminationEditEntry, "TerminationEditEntry", null, ErrorMessage.Messages[ErrorCode.NullTerminationEditEntry]));
                throw new ValidationError(violations);
            }

            var entity = UnitOfWork.TerminationRepository.FindById(entry.TerminationId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidTerminationId, "TerminationId", entry.TerminationId, ErrorMessage.Messages[ErrorCode.InvalidTerminationId]));
                throw new ValidationError(violations);
            }

            if (string.IsNullOrEmpty(entry.Reason))
            {
                violations.Add(new RuleViolation(ErrorCode.NullReason, "Reason", null, ErrorMessage.Messages[ErrorCode.NullReason]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.Reason.Length > 500)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidReason, "Reason", entry.Reason, ErrorMessage.Messages[ErrorCode.InvalidReason]));
                    throw new ValidationError(violations);
                }
            }

            entity.EmployeeId = entry.EmployeeId;
            entity.Reason = entry.Reason;
            entity.InformedDate = entry.InformedDate;
            entity.LastWorkingDate = entry.LastWorkingDate;
            entity.IsTerminationPaid = entry.IsTerminationPaid;
            entity.SignDate = entry.SignDate;
            entity.Signer = entry.Signer;

            UnitOfWork.TerminationRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        #endregion

        #region Working History
        public IEnumerable<WorkingHistoryDetail> GetWorkingHistories(int employeeId)
        {
            var lst = UnitOfWork.WorkingHistoryRepository.GetWorkingHistories(employeeId);
            return lst.ToDtos<WorkingHistory, WorkingHistoryDetail>();
        }
        public WorkingHistoryDetail GetWorkingHistoryDetail(int workingHistoryId)
        {
            var workingHistory = UnitOfWork.WorkingHistoryRepository.FindById(workingHistoryId);
            return workingHistory.ToDto<WorkingHistory, WorkingHistoryDetail>();
        }
        public WorkingHistoryDetail InsertWorkingHistory(WorkingHistoryEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullWorkingHistory, "WorkingHistory", null, ErrorMessage.Messages[ErrorCode.NullWorkingHistory]));
                throw new ValidationError(violations);
            }

            var employee = UnitOfWork.WorkingHistoryRepository.FindById(entry.EmployeeId);
            if (employee == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullEmployee, "EmployeeId",
                        entry.EmployeeId, ErrorMessage.Messages[ErrorCode.NullEmployee]));
                throw new ValidationError(violations);
            }

            var position = UnitOfWork.JobPositionRepository.FindById(entry.PositionId);
            if (position == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidPositionId, "PositionId", entry.PositionId, ErrorMessage.Messages[ErrorCode.InvalidPositionId]));
                throw new ValidationError(violations);
            }

            var entity = entry.ToEntity<WorkingHistoryEntry, WorkingHistory>();

            UnitOfWork.WorkingHistoryRepository.Insert(entity);
            UnitOfWork.SaveChanges();
            return entity.ToDto<WorkingHistory, WorkingHistoryDetail>();
        }
        public void UpdateWorkingHistory(WorkingHistoryEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullWorkingHistoryEditEntry, "WorkingHistoryEditEntry", null, ErrorMessage.Messages[ErrorCode.NullWorkingHistoryEditEntry]));
                throw new ValidationError(violations);
            }

            var entity = UnitOfWork.WorkingHistoryRepository.FindById(entry.WorkingHistoryId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidWorkingHistoryId, "WorkingHistoryId", entry.WorkingHistoryId, ErrorMessage.Messages[ErrorCode.InvalidWorkingHistoryId]));
                throw new ValidationError(violations);
            }

            entity.EmployeeId = entry.EmployeeId;
            entity.JoinedDate = entry.JoinedDate;
            entity.FromDate = entry.FromDate;
            entity.ToDate = entry.ToDate;
            entity.ManagerId = entry.ManagerId;
            entity.PositionId = entry.PositionId;
            entity.Duty = entry.Duty;
            entity.Note = entry.Note;

            UnitOfWork.WorkingHistoryRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region Dipose

        private bool _disposed = false;
        protected override void Dispose(bool isDisposing)
        {
            if (!this._disposed)
            {
                if (isDisposing)
                {
                    ContactService = null;
                    DocumentService = null;
                    VendorService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
        
    }
}
