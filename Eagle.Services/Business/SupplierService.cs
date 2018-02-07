using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Core.Permission;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Manufacturers;
using Eagle.Repositories;
using Eagle.Resources;
using Eagle.Services.Business.Validation;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.EntityMapping.Common;
using Eagle.Services.SystemManagement;
using Eagle.Services.Validations;

namespace Eagle.Services.Business
{
    public class SupplierService: BaseService, ISupplierService
    {
        private IDocumentService DocumentService { get; set; }

        public SupplierService(IUnitOfWork unitOfWork, IDocumentService documentService) : base(unitOfWork)
        {
            DocumentService = documentService;
        }

        #region Manufacturer
        public IEnumerable<ManufacturerDetail> GetManufacturers(int vendorId, ManufacturerSearchEntry filter, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = UnitOfWork.ManufacturerRepository.GetManufacturers(vendorId, filter.ManufacturerName, filter.IsActive, ref recordCount, orderBy, page, pageSize);
            return lst.ToDtos<Manufacturer, ManufacturerDetail>();
        }
        public IEnumerable<ManufacturerInfoDetail> GetManufacturerList(int vendorId, ManufacturerStatus? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = new List<ManufacturerInfoDetail>();
            var manufacturers = UnitOfWork.ManufacturerRepository.GetManufacturers(vendorId,null, status, ref recordCount, orderBy, page, pageSize);
            if (manufacturers != null)
            {
                lst.AddRange(manufacturers.Select(item => new ManufacturerInfoDetail
                {
                    CategoryId = item.CategoryId,
                    ManufacturerId = item.ManufacturerId,
                    ManufacturerName = item.ManufacturerName,
                    Email = item.Email,
                    Phone = item.Phone,
                    Fax = item.Fax,
                    Address = item.Address,
                    IsActive = item.IsActive,
                    Photo = item.Photo,
                    FileUrl = (item.Photo != null && item.Photo > 0) ? DocumentService.GetFileInfoDetail(Convert.ToInt32(item.Photo)).FileUrl : GlobalSettings.NotFoundFileUrl,
                }));
            }
            return lst;
        }
        public ManufacturerDetail GetManufacturerDetail(int id)
        {
            var entity = UnitOfWork.ManufacturerRepository.GetDetails(id);
            return entity.ToDto<Manufacturer, ManufacturerDetail>();
        }
        public void InsertManufacturer(Guid applicationId, Guid userId, int vendorId, ManufacturerEntry entry)
        {
            ISpecification<ManufacturerEntry> validator = new ManufacturerEntryValidator(UnitOfWork, PermissionLevel.Create, CurrentClaimsIdentity);
            var violations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, violations);
            if (!isDataValid) throw new ValidationError(violations);

            var entity = entry.ToEntity<ManufacturerEntry, Manufacturer>();
            entity.VendorId = vendorId;

            if (entry.File != null && entry.File.ContentLength > 0)
            {
                int maxContentLength = UnitOfWork.ApplicationSettingRepository.GetAllowedMaxImageContentLength(applicationId);
                string[] allowedFileExtensions = UnitOfWork.ApplicationSettingRepository.GetAllowedFileExtensions(applicationId);

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
                    var fileInfo = DocumentService.UploadAndSaveDbByFolderId(applicationId, userId, entry.File, (int)FileLocation.Manufacturer, StorageType.Local);
                    entity.Photo = fileInfo.FileId;
                }
            }

            UnitOfWork.ManufacturerRepository.Insert(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateManufacturer(Guid applicationId, Guid userId, int vendorId, ManufacturerEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundManufacturerEntry, "ManufacturerEntry"));
                throw new ValidationError(violations);
            }

            var entity = UnitOfWork.ManufacturerRepository.FindById(entry.ManufacturerId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundManufacturer, "Manufacturer", entry.ManufacturerId));
                throw new ValidationError(violations);
            }

            if (!string.IsNullOrEmpty(entry.ManufacturerName) && entry.ManufacturerName.Length > 300)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidManufacturerName, "ManufacturerName"));
                throw new ValidationError(violations);
            }

            var category = UnitOfWork.ManufacturerCategoryRepository.FindById(entry.CategoryId);
            if (category == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidCategoryId, "CategoryId"));
                throw new ValidationError(violations);
            }

            if (entry.File != null && entry.File.ContentLength > 0)
            {
                if (entity.Photo != null)
                {
                    DocumentService.DeleteFile(Convert.ToInt32(entity.Photo));
                }

                var fileInfo = DocumentService.UploadAndSaveDbByFolderId(applicationId, userId, entry.File, (int)FileLocation.Manufacturer, StorageType.Local);
                if (fileInfo != null)
                {
                    entity.Photo = fileInfo.FileId;
                }
            }

            entity.VendorId = vendorId;
            entity.CategoryId = entry.CategoryId;
            entity.ManufacturerName = entry.ManufacturerName;
            entity.Address = entry.Address;
            entity.Email = entry.Email;
            entity.Phone = entry.Phone;
            entity.Fax = entry.Fax;
            entity.IsActive = entry.IsActive;

            UnitOfWork.ManufacturerRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateManufacturerStatus(int id, ManufacturerStatus status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ManufacturerRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundManufacturer, "Manufacturer", id));
                throw new ValidationError(violations);
            }

            var isValid = Enum.IsDefined(typeof(ManufacturerStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status", null,
                    ErrorMessage.Messages[ErrorCode.InvalidStatus]));
                throw new ValidationError(violations);
            }
            if (entity.IsActive == status) return;

            entity.IsActive = status;
            UnitOfWork.ManufacturerRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region Manufacturer Category
        public IEnumerable<ManufacturerCategoryDetail> GeManufacturerCategories(int vendorId, ManufacturerCategorySearchEntry filter, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = UnitOfWork.ManufacturerCategoryRepository.GetManufacturerCategories(vendorId, filter.CategoryName, filter.IsActive, ref recordCount, orderBy, page, pageSize);
            return lst.ToDtos<ManufacturerCategory, ManufacturerCategoryDetail>();
        }
        public ManufacturerCategoryDetail GetManufacturerCategoryDetail(int id)
        {
            var entity = UnitOfWork.ManufacturerCategoryRepository.GetDetails(id);
            return entity.ToDto<ManufacturerCategory, ManufacturerCategoryDetail>();
        }
        public SelectList PoplulateManufacturerCategorySelectList(ManufacturerCategoryStatus? status = null, int? selectedValue = null, bool? isShowSelectText = false)
        {
            return UnitOfWork.ManufacturerCategoryRepository.PopulateManufacturerCategorySelectList(status, selectedValue, isShowSelectText);
        }
        public void InsertManufacturerCategory(int vendorId, ManufacturerCategoryEntry entry)
        {
            ISpecification<ManufacturerCategoryEntry> validator = new ManufacturerCategoryEntryValidator(UnitOfWork,PermissionLevel.Create, CurrentClaimsIdentity);
            var dataViolations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isDataValid) throw new ValidationError(dataViolations);

            var entity = entry.ToEntity<ManufacturerCategoryEntry, ManufacturerCategory>();
            entity.VendorId = vendorId;

            UnitOfWork.ManufacturerCategoryRepository.Insert(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateManufacturerCategory(ManufacturerCategoryEditEntry entry)
        {
            ISpecification<ManufacturerCategoryEntry> validator = new ManufacturerCategoryEntryValidator(UnitOfWork, PermissionLevel.Create, CurrentClaimsIdentity);
            var dataViolations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isDataValid) throw new ValidationError(dataViolations);

            var entity = UnitOfWork.ManufacturerCategoryRepository.FindById(entry.CategoryId);
            if (entity == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NotFoundManufacturerCategory, "ManufacturerCategory", entry.CategoryId));
                throw new ValidationError(dataViolations);
            }

            entity.CategoryName = entry.CategoryName;
            entity.Description = entry.Description;
            entity.IsActive = entry.IsActive;
            UnitOfWork.ManufacturerCategoryRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateManufacturerCategoryStatus(int id, ManufacturerCategoryStatus status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ManufacturerCategoryRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundManufacturerCategory, "ManufacturerCategory", id));
                throw new ValidationError(violations);
            }

            var isValid = Enum.IsDefined(typeof(ManufacturerCategoryStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status", null,
                    ErrorMessage.Messages[ErrorCode.InvalidStatus]));
                throw new ValidationError(violations);
            }
            if (entity.IsActive == status) return;

            entity.IsActive = status;
            UnitOfWork.ManufacturerCategoryRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region Dispose

        private bool _disposed;
        protected override void Dispose(bool isDisposing)
        {
            if (!_disposed)
            {
                if (isDisposing)
                {

                    DocumentService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }
        #endregion
    }
}
