using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Eagle.Common.Utilities;
using Eagle.Core.Configuration;
using Eagle.Core.Permission;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Vendors;
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
    public class VendorService : BaseService, IVendorService
    {
        private IContactService ContactService { get; set; }
        private IDocumentService DocumentService { get; set; }

        public VendorService(IUnitOfWork unitOfWork, IContactService contactService, IDocumentService documentService)
            : base(unitOfWork)
        {
            DocumentService = documentService;
            ContactService = contactService;
        }

        #region Vendor
        public IEnumerable<VendorDetail> GetVendors(VendorSearchEntry filter, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = UnitOfWork.VendorRepository.GetVendors(filter.VendorName, filter.IsAuthorized, ref recordCount, orderBy, page, pageSize);
            return lst.ToDtos<Vendor, VendorDetail>();
        }
        public VendorDetail GetVendorDetail(int id)
        {
            var entity = UnitOfWork.VendorRepository.FindById(id);
            return entity.ToDto<Vendor, VendorDetail>();
        }
        public VendorInfoDetail GetDefaultVendor()
        {
            var model = new VendorInfoDetail();
            var item = UnitOfWork.VendorRepository.FindById(GlobalSettings.DefaultVendorId);
            if (item != null)
            {
                model.VendorId = item.VendorId;
                model.VendorName = item.VendorName;
                model.StoreName = item.StoreName;
                model.AccountNumber = item.AccountNumber;
                model.Fax = item.Fax;
                model.CopyRight = item.CopyRight;
                model.TaxCode = item.TaxCode;
                model.Logo = item.Logo;
                model.Slogan = item.Slogan;
                model.Telephone = item.Telephone;
                model.Mobile = item.Mobile;
                model.Fax = item.Fax;
                model.Email = item.Email;
                model.Hotline = item.Hotline;
                model.SupportOnline = item.SupportOnline;
                model.Website = item.Website;
                model.ClickThroughs = item.ClickThroughs;
                model.TermsOfService = item.TermsOfService;
                model.Keywords = item.Keywords;
                model.CreditRating = item.CreditRating;
                model.Description = HttpUtility.HtmlDecode(item.Description);
                model.RefundPolicy = HttpUtility.HtmlDecode(item.RefundPolicy);
                model.ShoppingHelp = HttpUtility.HtmlDecode(item.ShoppingHelp);
                model.OrganizationalStructure = HttpUtility.HtmlDecode(item.OrganizationalStructure);
                model.FunctionalAreas = HttpUtility.HtmlDecode(item.FunctionalAreas);
                model.Summary = item.Summary;
                model.IsAuthorized = item.IsAuthorized;
                model.Addresses = GetVendorAddresses(item.VendorId);

                if (item.Logo != null)
                {
                    var fileInfo = DocumentService.GetFileInfoDetail((int)item.Logo);
                    if (fileInfo != null)
                    {
                        model.FileUrl = fileInfo.FileUrl;
                    }
                }
            }
            return model;
        }
        public void InsertVendor(Guid applicationId, Guid userId, VendorEntry entry)
        {
            ISpecification<VendorEntry> validator = new VendorEntryValidator(UnitOfWork, PermissionLevel.Create, CurrentClaimsIdentity);
            var dataViolations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isDataValid) throw new ValidationError(dataViolations);

            var entity = entry.ToEntity<VendorEntry, Vendor>();
            entity.Ip = NetworkUtils.GetIP4Address();
            entity.CreatedByUserId = userId;
            entity.CreatedDate = DateTime.UtcNow;

            if (entry.File != null && entry.File.ContentLength > 0)
            {
                int maxContentLength = UnitOfWork.ApplicationSettingRepository.GetAllowedMaxImageContentLength(applicationId);
                string[] allowedFileExtensions = UnitOfWork.ApplicationSettingRepository.GetAllowedImageExtensions(applicationId);

                if (!allowedFileExtensions.Contains(entry.File.FileName.Substring(entry.File.FileName.LastIndexOf('.'))))
                {
                    dataViolations.Add(new RuleViolation(ErrorCode.InvalidFileType, "FileUpload", LanguageResource.InvalidFileExtension + " : " + string.Join(", ", allowedFileExtensions)));
                    throw new ValidationError(dataViolations);
                }
                if (entry.File.ContentLength > maxContentLength)
                {
                    dataViolations.Add(new RuleViolation(ErrorCode.MaximumAllowedSize, "FileUpload", LanguageResource.InvalidFileSize + " , " + LanguageResource.MaximumAllowedSize + maxContentLength + " MB"));
                    throw new ValidationError(dataViolations);
                }
                var fileInfo = DocumentService.UploadAndSaveDbByFolderId(applicationId, userId, entry.File, (int)FileLocation.Vendor, StorageType.Local);
                if (fileInfo != null)
                {
                    entity.Logo = fileInfo.FileId;
                }
            }

            UnitOfWork.VendorRepository.Insert(entity);
            UnitOfWork.SaveChanges();

            //Insert new address
            if (entry.Address != null && entry.Address.CountryId != null)
            {
                var addressEntry = new AddressEntry
                {
                    Street = entry.Address.Street,
                    PostalCode = entry.Address.PostalCode,
                    Description = entry.Address.Description,
                    CountryId = entry.Address.CountryId,
                    ProvinceId = entry.Address.ProvinceId,
                    RegionId = entry.Address.RegionId
                };
                var newAddress = ContactService.InsertAddress(addressEntry);
                if (newAddress != null)
                {
                    var vendorAddress = UnitOfWork.VendorAddressRepository.GetDetails(entity.VendorId, newAddress.AddressId);
                    if (vendorAddress != null)
                    {
                        var newVendorAddress = new VendorAddress
                        {
                            VendorId = entity.VendorId,
                            AddressId = newAddress.AddressId,
                            ModifiedDate = DateTime.UtcNow
                        };
                        UnitOfWork.VendorAddressRepository.Insert(newVendorAddress);
                        UnitOfWork.SaveChanges();
                    }
                }
            }

            //Insert new currency
            if (entry.Currency != null)
            {
                var vendorCurrency = entry.Currency.ToEntity<VendorCurrencyEntry, VendorCurrency>();
                vendorCurrency.VendorId = entity.VendorId;
                UnitOfWork.VendorCurrencyRepository.Insert(vendorCurrency);
                UnitOfWork.SaveChanges();
            }
        }
        public void UpdateVendor(Guid applicationId, Guid userId, VendorEditEntry entry)
        {
            string ip = NetworkUtils.GetIP4Address();

            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundVendorEditEntry, "VendorEditEntry"));
                throw new ValidationError(violations);
            }

            var entity = UnitOfWork.VendorRepository.FindById(entry.VendorId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundVendor, "VendorId", entry.VendorId));
                throw new ValidationError(violations);
            }

            if (!string.IsNullOrEmpty(entry.VendorName) && entity.VendorName != entry.VendorName)
            {
                var item = UnitOfWork.VendorRepository.FindByVendorName(entry.VendorName);
                if (item != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.DuplicateVendorName, "VendorName", entry.VendorName));
                    throw new ValidationError(violations);
                }
            }

            entity.VendorName = entry.VendorName;
            entity.StoreName = entry.StoreName;
            entity.AccountNumber = entry.AccountNumber;
            entity.Slogan = entry.Slogan;
            entity.Telephone = entry.Telephone;
            entity.Mobile = entry.Mobile;
            entity.Fax = entry.Fax;
            entity.Email = entry.Email;
            entity.Hotline = entry.Hotline;
            entity.SupportOnline = entry.SupportOnline;
            entity.CopyRight = entry.CopyRight;
            entity.Website = entry.Website;
            entity.ClickThroughs = entry.ClickThroughs;
            entity.TermsOfService = entry.TermsOfService;
            entity.Keywords = entry.Keywords;
            entity.Summary = entry.Summary;
            entity.Description = entry.Description;
            entity.ShoppingHelp = HttpUtility.HtmlDecode(entry.ShoppingHelp);
            entity.RefundPolicy = HttpUtility.HtmlDecode(entry.RefundPolicy);
            entity.OrganizationalStructure = HttpUtility.HtmlDecode(entry.OrganizationalStructure);
            entity.FunctionalAreas = HttpUtility.HtmlDecode(entry.FunctionalAreas);
            entity.CreditRating = entry.CreditRating;
            entity.IsAuthorized = entry.IsAuthorized;
            entity.LastUpdatedIp = ip;
            entity.LastModifiedDate = DateTime.UtcNow;
            entity.LastModifiedByUserId = userId;

            if (entry.File != null && entry.File.ContentLength > 0)
            {
                int maxContentLength = UnitOfWork.ApplicationSettingRepository.GetAllowedMaxImageContentLength(applicationId);
                string[] allowedFileExtensions = UnitOfWork.ApplicationSettingRepository.GetAllowedImageExtensions(applicationId);

                if (!allowedFileExtensions.Contains(entry.File.FileName.Substring(entry.File.FileName.LastIndexOf('.'))))
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidFileType, "FileUpload", LanguageResource.InvalidFileExtension + " : " + string.Join(", ", allowedFileExtensions)));
                    throw new ValidationError(violations);
                }
                if (entry.File.ContentLength > maxContentLength)
                {
                    violations.Add(new RuleViolation(ErrorCode.MaximumAllowedSize, "FileUpload", LanguageResource.InvalidFileSize + " , " + LanguageResource.MaximumAllowedSize + maxContentLength + " MB"));
                    throw new ValidationError(violations);
                }
                var fileInfo = DocumentService.UploadAndSaveDbByFolderId(applicationId, userId, entry.File, (int)FileLocation.Vendor, StorageType.Local);
                if (fileInfo != null)
                {
                    if (entity.Logo != null)
                    {
                        DocumentService.DeleteFile(Convert.ToInt32(entity.Logo));
                    }
                    entity.Logo = fileInfo.FileId;
                }
            }

            UnitOfWork.VendorRepository.Update(entity);
            UnitOfWork.SaveChanges();

            //Update address
            if (entry.Address != null && entry.Address.CountryId != null)
            {
                if (entry.Address.AddressId > 0)
                {
                    var address = GetVendorAddresses(entity.VendorId).FirstOrDefault();
                    if (address != null)
                    {
                        var addressEditEntry = new AddressEditEntry
                        {
                            AddressId = Convert.ToInt32(entry.Address.AddressId),
                            Street = entry.Address.Street,
                            PostalCode = entry.Address.PostalCode,
                            Description = entry.Address.Description,
                            CountryId = entry.Address.CountryId,
                            ProvinceId = entry.Address.ProvinceId,
                            RegionId = entry.Address.RegionId
                        };
                        ContactService.UpdateAddress(addressEditEntry);
                    }
                }
                else
                {
                    var addressEntry = new AddressEntry
                    {
                        Street = entry.Address.Street,
                        PostalCode = entry.Address.PostalCode,
                        Description = entry.Address.Description,
                        CountryId = entry.Address.CountryId,
                        ProvinceId = entry.Address.ProvinceId,
                        RegionId = entry.Address.RegionId
                    };

                    var newAddress = ContactService.InsertAddress(addressEntry);
                    if (newAddress != null)
                    {
                        var vendorAddress = UnitOfWork.VendorAddressRepository.GetDetails(entity.VendorId, newAddress.AddressId);
                        if (vendorAddress == null)
                        {
                            var newVendorAddress = new VendorAddress
                            {
                                VendorId = entity.VendorId,
                                AddressId = newAddress.AddressId,
                                ModifiedDate = DateTime.UtcNow
                            };
                            UnitOfWork.VendorAddressRepository.Insert(newVendorAddress);
                            UnitOfWork.SaveChanges();
                        }
                    }
                }
            }

            //Update currency
            if (entry.Currency != null)
            {
                var vendorCurrency = UnitOfWork.VendorCurrencyRepository.GetDetails(entity.VendorId,
                    entry.Currency.CurrencyCode);
                if (vendorCurrency == null)
                {
                    vendorCurrency = new VendorCurrency
                    {
                        VendorId = entity.VendorId,
                        CurrencyCode = entry.Currency.CurrencyCode,
                        CurrencySymbol = entry.Currency.CurrencySymbol,
                        Decimals = entry.Currency.Decimals,
                        DecimalSymbol = entry.Currency.DecimalSymbol,
                        ThousandSeparator = entry.Currency.ThousandSeparator,
                        PositiveFormat = entry.Currency.PositiveFormat,
                        NegativeFormat = entry.Currency.NegativeFormat,
                    };
                   
                    UnitOfWork.VendorCurrencyRepository.Insert(vendorCurrency);
                    UnitOfWork.SaveChanges();
                }
                else
                {
                    vendorCurrency.VendorId = entity.VendorId;
                    vendorCurrency.CurrencyCode = entry.Currency.CurrencyCode;
                    vendorCurrency.CurrencySymbol = entry.Currency.CurrencySymbol;
                    vendorCurrency.Decimals = entry.Currency.Decimals;
                    vendorCurrency.DecimalSymbol = entry.Currency.DecimalSymbol;
                    vendorCurrency.ThousandSeparator = entry.Currency.ThousandSeparator;
                    vendorCurrency.PositiveFormat = entry.Currency.PositiveFormat;
                    vendorCurrency.NegativeFormat = entry.Currency.NegativeFormat;

                    UnitOfWork.VendorCurrencyRepository.Update(vendorCurrency);
                    UnitOfWork.SaveChanges();
                }
            }
        }
        public void UpdateClickThroughs(Guid userId, int vendorId)
        {
            string ip = NetworkUtils.GetIP4Address();

            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.VendorRepository.FindById(vendorId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundVendor, "VendorId", vendorId));
                throw new ValidationError(violations);
            }

            entity.ClickThroughs = UnitOfWork.VendorRepository.GetNewClickThrough();
            entity.LastUpdatedIp = ip;
            entity.LastModifiedDate = DateTime.UtcNow;
            entity.LastModifiedByUserId = userId;

            UnitOfWork.VendorRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateVendorStatus(Guid userId, int id, VendorStatus status)
        {
            string ip = NetworkUtils.GetIP4Address();

            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.VendorRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundVendor, "VendorId", id));
                throw new ValidationError(violations);
            }

            var isValid = Enum.IsDefined(typeof(VendorStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status", null,
                    ErrorMessage.Messages[ErrorCode.InvalidStatus]));
                throw new ValidationError(violations);
            }
            if (entity.IsAuthorized == status) return;

            entity.IsAuthorized = status;
            entity.LastUpdatedIp = ip;
            entity.LastModifiedDate = DateTime.UtcNow;
            entity.LastModifiedByUserId = userId;

            UnitOfWork.VendorRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region Vendor Address
        public List<VendorAddressDetail> GetVendorAddresses(int vendorId)
        {
            List<VendorAddressDetail> lst= new List<VendorAddressDetail>();
            var vendorAddresses = UnitOfWork.VendorAddressRepository.GetVendorAddresses(vendorId).ToList();
            if (vendorAddresses.Any())
            {
                lst.AddRange(from vendorAddress in vendorAddresses
                    let address = ContactService.GetAddressInfoDetail(vendorAddress.AddressId) ?? new AddressInfoDetail()
                    select new VendorAddressDetail
                    {
                        VendorAddressId = vendorAddress.VendorAddressId,
                        VendorId = vendorAddress.VendorId,
                        AddressId = vendorAddress.AddressId,
                        ModifiedDate = vendorAddress.ModifiedDate,
                        Address = address
                    });
            }
            return lst;
        }
        #endregion

        #region Vendor Partner
        public IEnumerable<VendorPartnerInfoDetail> GetPartners(VendorPartnerSearchEntry filter, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var partners = UnitOfWork.VendorPartnerRepository.GetPartners(filter.PartnerName, filter.Status, ref recordCount, orderBy, page, pageSize).ToList();

            if (!partners.Any()) return null;
            var lst = partners.Select(x => new VendorPartnerInfoDetail
            {
                PartnerId = x.PartnerId,
                VendorId = x.VendorId,
                PartnerName = x.PartnerName,
                Logo = x.Logo,
                FileUrl = (x.Logo != null && x.Logo > 0) ? DocumentService.GetFileInfoDetail(Convert.ToInt32(x.Logo)).FileUrl : GlobalSettings.NotFoundFileUrl,
                Telephone = x.Telephone,
                Mobile = x.Mobile,
                Fax = x.Fax,
                Email = x.Email,
                Description = !string.IsNullOrEmpty(x.Description) ? HttpUtility.HtmlDecode(x.Description) : string.Empty,
                Status = x.Status,
                CreatedDate = x.CreatedDate,
                LastModifiedDate = x.LastModifiedDate
            }).ToList();
            return lst;
        }

        public VendorPartnerDetail GetPartnerDetail(int id)
        {
            var entity = UnitOfWork.VendorPartnerRepository.FindById(id);
            return entity.ToDto<VendorPartner, VendorPartnerDetail>();
        }
        public SelectList PopulatePartnerStatus(bool? selectedValue = true, bool isShowSelectText = false)
        {
            return UnitOfWork.VendorPartnerRepository.PopulatePartnerStatus(selectedValue, isShowSelectText);
        }
        public void InsertPartner(Guid applicationId, Guid userId, int vendorId, VendorPartnerEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (string.IsNullOrEmpty(entry.PartnerName))
            {
                violations.Add(new RuleViolation(ErrorCode.NullPartnerName, "PartnerName", null,
                    ErrorMessage.Messages[ErrorCode.NullPartnerName]));
                throw new ValidationError(violations);
            }
            else
            {
                var item = UnitOfWork.VendorPartnerRepository.FindByPartnerName(entry.PartnerName);
                if (item != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.DuplicatePartnerName, "PartnerName", entry.PartnerName));
                    throw new ValidationError(violations);
                }
            }

            var entity = entry.ToEntity<VendorPartnerEntry, VendorPartner>();
            entity.VendorId = vendorId;
            entity.CreatedDate = DateTime.UtcNow;

            if (entry.File != null && entry.File.ContentLength > 0)
            {
                int maxContentLength = UnitOfWork.ApplicationSettingRepository.GetAllowedMaxImageContentLength(applicationId);
                string[] allowedFileExtensions = UnitOfWork.ApplicationSettingRepository.GetAllowedImageExtensions(applicationId);

                if (!allowedFileExtensions.Contains(entry.File.FileName.Substring(entry.File.FileName.LastIndexOf('.'))))
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidFileType, "FileUpload", LanguageResource.InvalidFileExtension + " : " + string.Join(", ", allowedFileExtensions)));
                    throw new ValidationError(violations);
                }
                if (entry.File.ContentLength > maxContentLength)
                {
                    violations.Add(new RuleViolation(ErrorCode.MaximumAllowedSize, "FileUpload", LanguageResource.InvalidFileSize + " , " + LanguageResource.MaximumAllowedSize + maxContentLength + " MB"));
                    throw new ValidationError(violations);
                }
                var fileInfo = DocumentService.UploadAndSaveDbByFolderId(applicationId, userId, entry.File, (int)FileLocation.Partner, StorageType.Local);
                if (fileInfo != null)
                {
                    entity.Logo = fileInfo.FileId;
                }
            }

            UnitOfWork.VendorPartnerRepository.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdatePartner(Guid applicationId, Guid userId, VendorPartnerEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundVendorPartnerEditEntry, "VendorPartnerEditEntry"));
                throw new ValidationError(violations);
            }

            var entity = UnitOfWork.VendorPartnerRepository.FindById(entry.PartnerId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundPartner, "PartnerId", entry.PartnerId));
                throw new ValidationError(violations);
            }

            if (!string.IsNullOrEmpty(entry.PartnerName) && entity.PartnerName != entry.PartnerName)
            {
                var item = UnitOfWork.VendorPartnerRepository.FindByPartnerName(entry.PartnerName);
                if (item != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.DuplicatePartnerName, "PartnerName", entry.PartnerName));
                    throw new ValidationError(violations);
                }
            }

            entity.PartnerName = entry.PartnerName;
            entity.Telephone = entry.Telephone;
            entity.Mobile = entry.Mobile;
            entity.Description = entry.Description;
            entity.Fax = entry.Fax;
            entity.Email = entry.Email;
            entity.Description = entry.Description;
            entity.Status = entry.Status;
            entity.LastModifiedDate = DateTime.UtcNow;

            if (entry.File != null && entry.File.ContentLength > 0)
            {
                int maxContentLength = UnitOfWork.ApplicationSettingRepository.GetAllowedMaxImageContentLength(applicationId);
                string[] allowedFileExtensions = UnitOfWork.ApplicationSettingRepository.GetAllowedImageExtensions(applicationId);

                if (!allowedFileExtensions.Contains(entry.File.FileName.Substring(entry.File.FileName.LastIndexOf('.'))))
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidFileType, "FileUpload", LanguageResource.InvalidFileExtension + " : " + string.Join(", ", allowedFileExtensions)));
                    throw new ValidationError(violations);
                }
                if (entry.File.ContentLength > maxContentLength)
                {
                    violations.Add(new RuleViolation(ErrorCode.MaximumAllowedSize, "FileUpload", LanguageResource.InvalidFileSize + " , " + LanguageResource.MaximumAllowedSize + maxContentLength + " MB"));
                    throw new ValidationError(violations);
                }
                var fileInfo = DocumentService.UploadAndSaveDbByFolderId(applicationId, userId, entry.File, (int)FileLocation.Partner, StorageType.Local);
                if (fileInfo != null)
                {
                    if (entity.Logo != null)
                    {
                        DocumentService.DeleteFile(Convert.ToInt32(entity.Logo));
                    }
                    entity.Logo = fileInfo.FileId;
                }
            }

            UnitOfWork.VendorPartnerRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdatePartnerStatus(int id, bool status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.VendorPartnerRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundVendorPartner, "PartnerId", id));
                throw new ValidationError(violations);
            }

            if (entity.Status == status) return;

            entity.Status = status;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.VendorPartnerRepository.Update(entity);
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
