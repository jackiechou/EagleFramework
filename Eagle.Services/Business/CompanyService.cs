using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Eagle.Common.Utilities;
using Eagle.Core.Configuration;
using Eagle.Core.Extension;
using Eagle.Core.Permission;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Companies;
using Eagle.Repositories;
using Eagle.Resources;
using Eagle.Services.Business.Validation;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.SystemManagement;
using Eagle.Services.EntityMapping.Common;
using Eagle.Services.SystemManagement;
using Eagle.Services.Validations;

namespace Eagle.Services.Business
{
    public class CompanyService : BaseService, ICompanyService
    {
        #region Contruct
        private IContactService ContactService { get; set; }
        private IDocumentService DocumentService { get; set; }

        public CompanyService(IUnitOfWork unitOfWork, IContactService contactService, IDocumentService documentService) : base(unitOfWork)
        {
            ContactService = contactService;
            DocumentService = documentService;
        }

        #endregion

        #region Company
        public IEnumerable<CompanyDetail> GetCompanies(CompanyStatus? status)
        {
            var entityList = UnitOfWork.CompanyRepository.GetCompanies(status);
            return entityList.ToDtos<Company, CompanyDetail>().AsEnumerable();
        }
        public IEnumerable<TreeGrid> GetCompanyTreeGrid(CompanyStatus? status, int? selectedId, bool? isRootShowed)
        {
            return UnitOfWork.CompanyRepository.GetCompanyTreeGrid(status, selectedId, isRootShowed);
        }

        public CompanyDetail GetCompanyDetail(int id)
        {
            var entity = UnitOfWork.CompanyRepository.Find(id);
            if (entity != null && !string.IsNullOrEmpty(entity.Description))
            {
                entity.Description = HttpUtility.HtmlDecode(entity.Description);
            }
            return entity.ToDto<Company, CompanyDetail>();
        }

        public string GetLogo(int id)
        {
            string logo = string.Empty;
            var company= UnitOfWork.CompanyRepository.FindById(id);
            if (company.Logo != null && company.Logo > 0)
            {
                var document = DocumentService.GetFileInfoDetail((int)company.Logo);
                logo = document.FileUrl;
            }
            return logo;
        }

        public string GetSlogan(int id)
        {
            return UnitOfWork.CompanyRepository.GetSlogan(id);
        }

        public string GetSupportOnline(int id)
        {
            return UnitOfWork.CompanyRepository.GetSupportOnline(id);
        }

        public void InsertCompany(Guid applicationId, Guid userId, CompanyEntry entry)
        {
            string ip = NetworkUtils.GetIP4Address();

            ISpecification<CompanyEntry> validator = new CompanyEntryValidator(UnitOfWork, PermissionLevel.Create, CurrentClaimsIdentity);
            var violations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, violations);
            if (!isDataValid) throw new ValidationError(violations);

            var entity = entry.ToEntity<CompanyEntry, Company>();

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
                    var fileInfo = DocumentService.UploadAndSaveDbByFolderId(applicationId, userId, entry.File, (int)FileLocation.Company, StorageType.Local);
                    entity.Logo = fileInfo.FileId;
                }
            }

            int depth = UnitOfWork.CompanyRepository.GetDepth(entry.ParentId);
            int listOrder = UnitOfWork.CompanyRepository.GetNewListOrder();

            entity.Description = StringUtils.UTF8_Encode(entry.Description);
            entity.Depth = depth;
            entity.HasChild = false;
            entity.ListOrder = listOrder;
            entity.ParentId = entry.ParentId ?? 0;
            entity.Ip = ip;
            entity.CreatedByUserId = userId;
            entity.CreatedDate = DateTime.UtcNow;

            UnitOfWork.CompanyRepository.Insert(entity);
            UnitOfWork.SaveChanges();

            //Update lineage and depth for menu
            if (entry.ParentId != null && entry.ParentId > 0)
            {
                var parentEntity = UnitOfWork.CompanyRepository.FindById(Convert.ToInt32(entry.ParentId));
                if (parentEntity == null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NotFoundParentId, "ParentId", entry.ParentId, ErrorMessage.Messages[ErrorCode.NotFoundParentId]));
                    throw new ValidationError(violations);
                }
                if (parentEntity.HasChild == null || parentEntity.HasChild == false)
                {
                    parentEntity.HasChild = true;
                    UnitOfWork.CompanyRepository.Update(parentEntity);
                    UnitOfWork.SaveChanges();
                }

                var lineage = $"{parentEntity.Lineage},{entity.CompanyId}";
                entity.Lineage = lineage;
                entity.Depth = lineage.Split(',').Count();
                entity.ParentId = entry.ParentId;
            }
            else
            {
                entity.ParentId = 0;
                entity.Lineage = $"{entity.CompanyId}";
                entity.Depth = 1;
            }

            if (entry.Address != null && entry.Address.CountryId != null)
            {
                var newAddress = ContactService.InsertAddress(entry.Address);
                if (newAddress != null)
                {
                    entity.AddressId = newAddress.AddressId;
                }
            }

            UnitOfWork.CompanyRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateCompany(Guid applicationId, Guid userId, CompanyEditEntry entry)
        {
            string ip = NetworkUtils.GetIP4Address();

            ISpecification<CompanyEditEntry> validator = new CompanyEditEntryValidator(UnitOfWork, PermissionLevel.Edit, CurrentClaimsIdentity);
            var violations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, violations);
            if (!isDataValid) throw new ValidationError(violations);

            var entity = UnitOfWork.CompanyRepository.FindById(entry.CompanyId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullCompany, "CompanyEditEntry",null, ErrorMessage.Messages[ErrorCode.NullCompany]));
                throw new ValidationError(violations);
            }

            if (entry.ParentId != entity.ParentId && entry.ParentId != entity.CompanyId)
            {
                if (entry.ParentId != null && entry.ParentId > 0)
                {
                    //Update parent entry
                    var parentEntity = UnitOfWork.CompanyRepository.FindById(Convert.ToInt32(entry.ParentId));
                    if (parentEntity == null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.NotFoundParentId, "ParentId", entry.ParentId, ErrorMessage.Messages[ErrorCode.NotFoundParentId]));
                        throw new ValidationError(violations);
                    }
                    if (parentEntity.HasChild == null || parentEntity.HasChild == false)
                    {
                        parentEntity.HasChild = true;
                        UnitOfWork.CompanyRepository.Update(parentEntity);
                        UnitOfWork.SaveChanges();
                    }

                    var lineage = $"{parentEntity.Lineage},{entry.CompanyId}";
                    entity.Lineage = lineage;
                    entity.Depth = lineage.Split(',').Count();
                    entity.ParentId = entry.ParentId;
                }
                else
                {
                    entity.Lineage = $"{entry.CompanyId}";
                    entity.Depth = 1;
                    entity.ParentId = 0;
                }
            }

            if (entry.File != null && entry.File.ContentLength > 0)
            {
                if (entity.Logo != null)
                {
                    DocumentService.DeleteFile(Convert.ToInt32(entity.Logo));
                }

                var fileInfo = DocumentService.UploadAndSaveDbByFolderId(applicationId, userId, entry.File, (int)FileLocation.Company, StorageType.Local);
                if (entity.Logo != null)
                {
                    entity.Logo = fileInfo.FileId;
                }
            }

            //Update address
            if (entry.Address == null)
            {
                if (entry.AddressId != null && entry.AddressId > 0)
                {
                    entity.AddressId = entry.AddressId;
                }
            }
            else
            {
                if (entry.AddressId != null && entry.AddressId > 0)
                {
                    var item = UnitOfWork.CompanyRepository.FindById(entity.AddressId);
                    if (item != null)
                    {
                        var address = new AddressEditEntry
                        {
                            AddressId = Convert.ToInt32(entry.AddressId),
                            AddressTypeId = AddressType.Company,
                            CountryId = entry.Address.CountryId,
                            ProvinceId = entry.Address.ProvinceId,
                            RegionId = entry.Address.RegionId,
                            Street = entry.Address.Street,
                            PostalCode = entry.Address.PostalCode,
                            Description = entry.Address.Description
                        };
                        ContactService.UpdateAddress(address);
                    }
                }
                else
                {
                    var addressEntry = new AddressEntry
                    {
                        AddressTypeId = AddressType.Company,
                        CountryId = entry.Address.CountryId,
                        ProvinceId = entry.Address.ProvinceId,
                        RegionId = entry.Address.RegionId,
                        Street = entry.Address.Street,
                        PostalCode = entry.Address.PostalCode,
                        Description = entry.Address.Description
                    };
                    var newAddress = ContactService.InsertAddress(addressEntry);
                    if (newAddress != null)
                    {
                        entity.AddressId = newAddress.AddressId;
                    }
                }
            }

            var hasChild = UnitOfWork.CompanyRepository.HasChildren(entity.CompanyId);
            entity.HasChild = hasChild;
            entity.CompanyName = entry.CompanyName;
            entity.Slogan = entry.Slogan;
            entity.Fax = entry.Fax;
            entity.Hotline = entry.Hotline;
            entity.Mobile = entry.Mobile;
            entity.Telephone = entry.Telephone;
            entity.Email = entry.Email;
            entity.Website = entry.Website;
            entity.SupportOnline = entry.SupportOnline;
            entity.CopyRight = entry.CopyRight;
            entity.TaxCode = entry.TaxCode;
            entity.Description = StringUtils.UTF8_Encode(entry.Description);
            entity.Status = entry.Status;
            entity.LastUpdatedIp = ip;
            entity.LastModifiedDate = DateTime.UtcNow;
            entity.LastModifiedByUserId = userId;

            UnitOfWork.CompanyRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateCompanyStatus(Guid userId, int id, CompanyStatus status)
        {
            string ip = NetworkUtils.GetIP4Address();

            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.CompanyRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidCompany, "CompanyId", id, ErrorMessage.Messages[ErrorCode.InvalidCompany]));
                throw new ValidationError(violations);
            }
           
            var isValid = Enum.IsDefined(typeof(CompanyStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status", status, ErrorMessage.Messages[ErrorCode.InvalidStatus]));
                throw new ValidationError(violations);
            }
            if (entity.Status == status) return;

            entity.Status = status;
            entity.LastUpdatedIp = ip;
            entity.LastModifiedByUserId = userId;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.CompanyRepository.Update(entity);
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
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}
