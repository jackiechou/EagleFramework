using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using Eagle.Common.Security.Cryptography;
using Eagle.Common.Session;
using Eagle.Common.Utilities;
using Eagle.Core.Configuration;
using Eagle.Core.Permission;
using Eagle.Core.Session;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Customers;
using Eagle.Entities.SystemManagement;
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
    public class CustomerService : BaseService, ICustomerService
    {
        public ICacheService CacheService { get; set; }
        public IContactService ContactService { get; set; }
        private IDocumentService DocumentService { get; set; }

        public CustomerService(IUnitOfWork unitOfWork, ICacheService cacheService, IContactService contactService, IDocumentService documentService) : base(unitOfWork)
        {
            CacheService = cacheService;
            ContactService = contactService;
            DocumentService = documentService;
        }

        #region Customer

        public string GenerateCode(int maxLetters)
        {
            return UnitOfWork.CustomerRepository.GenerateCode(maxLetters);
        }
        public IEnumerable<CustomerInfoDetail> GetCustomers(CustomerStatus? status)
        {
            var lst = new List<CustomerInfoDetail>();
            var customers = UnitOfWork.CustomerRepository.GetCustomers(status);
            if (customers != null)
            {
                foreach (var customer in customers)
                {
                    var customerInfo = new CustomerInfoDetail
                    {
                        CustomerId = customer.CustomerId,
                        CustomerTypeId = customer.CustomerTypeId,
                        CustomerNo = customer.CustomerNo,
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        ContactName = customer.ContactName,
                        Email = customer.Email,
                        Mobile = customer.Mobile,
                        HomePhone = customer.HomePhone,
                        WorkPhone = customer.WorkPhone,
                        Fax = customer.Fax,
                        CardNo = customer.CardNo,
                        IdCardNo = customer.IdCardNo,
                        PassPortNo = customer.PassPortNo,
                        TaxCode = customer.TaxCode,
                        BirthDay = customer.BirthDay,
                        AddressId = customer.AddressId,
                        IsActive = customer.IsActive
                    };

                    if (customer.AddressId != null && customer.AddressId > 0)
                    {
                        customerInfo.Address = ContactService.GetAddressInfoDetail(customer.AddressId.Value);
                    }
                    lst.Add(customerInfo);
                }
            }
            return lst;
        }
        public IEnumerable<CustomerDetail> GetCustomers(CustomerSearchEntry filter, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = UnitOfWork.CustomerRepository.GetCustomers(filter.Keywords, filter.CustomerTypeId, filter.IsActive, ref recordCount, orderBy, page,
                pageSize);
            return lst.ToDtos<Customer, CustomerDetail>();
        }
        /// <summary>
        /// Auto Complete with select2
        /// </summary>
        /// <param name="status"></param>
        /// <param name="recordCount"></param>
        /// <param name="searchTerm"></param>
        /// <param name="orderBy"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public Select2PagedResult GetCustomerAutoCompleteList(string searchTerm, CustomerStatus? status, out int recordCount, string orderBy = null, int? page = null)
        {
            var lst = UnitOfWork.CustomerRepository.GetCustomerAutoCompleteList(searchTerm, status, out recordCount, orderBy, page, GlobalSettings.DefaultPageSize);
            var results = lst.Select(item => new Select2Result { id = item.Id.ToString(), name = item.Name, text = item.Text, level = item.Level }).ToList();
            if (!results.Any()) return new Select2PagedResult { Results = null, Total = 0, MorePage = false };

            return new Select2PagedResult
            {
                Results = results,
                Total = recordCount,
                PageSize = GlobalSettings.DefaultPageSize,
                MorePage = page * GlobalSettings.DefaultPageSize < recordCount
            };
        }
        public SelectList PopulateCustomerSelectList(CustomerStatus? status = null, int? selectedValue = null, bool? isShowSelectText = true)
        {
            return UnitOfWork.CustomerRepository.PopulateCustomerSelectList(status, selectedValue, isShowSelectText);
        }
        public CustomerInfoDetail GetCustomerInfoDetail(int id)
        {
            var model = new CustomerInfoDetail();
            var item = UnitOfWork.CustomerRepository.FindById(id);
            if (item != null)
            {
                model.CustomerId = item.CustomerId;
                model.CustomerTypeId = item.CustomerTypeId;
                model.CustomerNo = item.CustomerNo;
                model.CardNo = item.CardNo;
                model.FirstName = item.FirstName;
                model.LastName = item.LastName;
                model.ContactName = item.ContactName;
                model.IdCardNo = item.IdCardNo;
                model.PassPortNo = item.PassPortNo;
                model.TaxCode = item.TaxCode;
                model.Mobile = item.Mobile;
                model.Photo = item.Photo;
                model.Gender = item.Gender;
                model.BirthDay = item.BirthDay;
                model.HomePhone = item.HomePhone;
                model.WorkPhone = item.WorkPhone;
                model.Mobile = item.Mobile;
                model.Fax = item.Fax;
                model.Email = item.Email;
                model.IsActive = item.IsActive;
                model.AddressId = item.AddressId;

                if (item.Photo != null)
                {
                    var fileInfo = DocumentService.GetFileInfoDetail((int)item.Photo);
                    if (fileInfo != null)
                    {
                        model.FileUrl = fileInfo.FileUrl;
                    }
                }

                if (item.AddressId != null)
                {
                    var address = ContactService.GetAddressDetails(Convert.ToInt32(item.AddressId));
                    if (address != null)
                    {
                        model.Address = new AddressInfoDetail
                        {
                            AddressTypeId = address.AddressTypeId,
                            Street = address.Street,
                            PostalCode = address.PostalCode,
                            Description = address.Description,
                            CountryId = address.CountryId,
                            ProvinceId = address.ProvinceId,
                            RegionId = address.RegionId
                        };

                        if (address.CountryId != null)
                        {
                            model.Address.Country = ContactService.GetCountryDetails(Convert.ToInt32(address.CountryId));
                        }

                        if (address.ProvinceId != null)
                        {
                            model.Address.Province =
                                ContactService.GetProvinceDetails(Convert.ToInt32(address.ProvinceId));
                        }

                        if (address.RegionId != null)
                        {
                            model.Address.Region = ContactService.GetRegionDetails(Convert.ToInt32(address.RegionId));
                        }
                    }
                }
            }
            return model;
        }
        public CustomerDetail GetCustomerDetail(int id)
        {
            var entity = UnitOfWork.CustomerRepository.FindById(id);
            return entity.ToDto<Customer, CustomerDetail>();
        }
        public CustomerDetail GetCustomerDetailByCustomerNo(string customerNo)
        {
            var entity = UnitOfWork.CustomerRepository.FindByCustomerNo(customerNo);
            return entity.ToDto<Customer, CustomerDetail>();
        }
        public CustomerDetail GetCustomerDetailByEmail(string email)
        {
            var entity = UnitOfWork.CustomerRepository.FindByEmail(email);
            return entity.ToDto<Customer, CustomerDetail>();
        }
        public CustomerDetail Register(Guid userId, int vendorId, CustomerRegisterEntry entry)
        {
            using (var transcope = new TransactionScope())
            {
                ISpecification<CustomerRegisterEntry> validator = new CustomerRegisterEntryValidator(UnitOfWork, PermissionLevel.Create);
                var violations = new List<RuleViolation>();
                var isValid = validator.IsSatisfyBy(entry, violations);
                if (!isValid) throw new ValidationError(violations);

                var entity = entry.ToEntity<CustomerRegisterEntry, Customer>();
                entity.Email = entry.EmailAddress;

                if (!string.IsNullOrWhiteSpace(entry.Phone))
                {
                    switch (entry.PhoneType)
                    {
                        case PhoneType.Mobile:
                            entity.Mobile = entry.Phone;
                            break;
                        case PhoneType.Work:
                            entity.WorkPhone = entry.Phone;
                            break;
                        case PhoneType.Home:
                            entity.HomePhone = entry.Phone;
                            break;
                        default:
                            entity.Mobile = entry.Phone;
                            break;
                    }
                }
                
                var address = new Address
                {
                    AddressTypeId = AddressType.Customer,
                    CountryId = entry.Address.CountryId,
                    ProvinceId = entry.Address.ProvinceId,
                    RegionId = entry.Address.RegionId,
                    Street = entry.Address.Street,
                    PostalCode = entry.Address.PostalCode,
                    Description = entry.Address.Description,
                    CreatedDate = DateTime.UtcNow
                };
                UnitOfWork.AddressRepository.Insert(address);
                UnitOfWork.SaveChanges();

                entity.AddressId = address.AddressId;
                entity.FirstName = entry.FirstName;
                entity.LastName = entry.LastName;
                entity.ContactName = $"{entry.FirstName} {entry.LastName}";
                entity.PasswordHash = Md5Crypto.GetMd5Hash(entry.PasswordSalt);
                entity.CustomerTypeId = (int)CustomerTypeSetting.Normal;
                entity.VendorId = vendorId;
                entity.CustomerNo = GenerateCode(15);
                entity.Gender = (int)Sex.NoneSpecified;
                entity.Verified = true;
                entity.IsActive = CustomerStatus.Published;
                entity.Ip = NetworkUtils.GetIP4Address();
                entity.CreatedByUserId = userId;
                entity.CreatedDate = DateTime.UtcNow;

                UnitOfWork.CustomerRepository.Insert(entity);
                UnitOfWork.SaveChanges();

                transcope.Complete();

                return entity.ToDto<Customer, CustomerDetail>();
            }
        }
        public CustomerInfoDetail SignIn(CustomerLogin login)
        {
            var violations = new List<RuleViolation>();
            if (string.IsNullOrEmpty(login.Email))
            {
                violations.Add(new RuleViolation(ErrorCode.NullEmail, "Email", null, ErrorMessage.Messages[ErrorCode.NullEmail]));
                throw new ValidationError(violations);
            }
            if (string.IsNullOrEmpty(login.Password))
            {
                violations.Add(new RuleViolation(ErrorCode.NullPassword, "Password", null, ErrorMessage.Messages[ErrorCode.NullPassword]));
                throw new ValidationError(violations);
            }

            //Get customer info
            string hashedPassword = Md5Crypto.GetMd5Hash(login.Password);
            var entity = UnitOfWork.CustomerRepository.FindByUserAndPassword(login.Email, hashedPassword);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidAccount, "Email", login.Email, LanguageResource.InvalidAccount));
                throw new ValidationError(violations);
            }
            if (!entity.Verified)
            {
                violations.Add(new RuleViolation(ErrorCode.InVerified, "InVerified", entity.Verified, ErrorMessage.Messages[ErrorCode.InVerified]));
                throw new ValidationError(violations);
            }
            if (entity.IsActive != CustomerStatus.Published)
            {
                violations.Add(new RuleViolation(ErrorCode.IsLockedOut, "IsLockedOut", entity.IsActive, LanguageResource.IsLockedOut));
                throw new ValidationError(violations);
            }

            var item = new CustomerInfoDetail
            {
                CustomerId = entity.CustomerId,
                CustomerTypeId = entity.CustomerTypeId,
                AddressId = entity.AddressId,
                VendorId = entity.VendorId,
                CustomerNo = entity.CustomerNo,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                PasswordSalt = entity.PasswordSalt,
                PasswordHash = entity.PasswordHash,
                CardNo = entity.CardNo,
                ContactName = entity.ContactName,
                IdCardNo = entity.IdCardNo,
                PassPortNo = entity.PassPortNo,
                TaxCode = entity.TaxCode,
                Photo = entity.Photo,
                Gender = entity.Gender,
                BirthDay = entity.BirthDay,
                HomePhone = entity.HomePhone,
                WorkPhone = entity.WorkPhone,
                Mobile = entity.Mobile,
                Fax = entity.Fax,
                Email = entity.Email,
                Verified = entity.Verified,
                IsActive = entity.IsActive,
                Ip = entity.Ip,
                LastUpdatedIp = entity.LastUpdatedIp,
                CreatedDate = entity.CreatedDate,
                LastModifiedDate = entity.LastModifiedDate
            };

            if (entity.Photo != null)
            {
                var fileInfo = DocumentService.GetFileInfoDetail((int)entity.Photo);
                if (fileInfo != null)
                {
                    item.FileUrl = fileInfo.FileUrl;
                }
            }

            if (entity.AddressId != null)
            {
                var address = ContactService.GetAddressDetails(Convert.ToInt32(entity.AddressId));
                if (address != null)
                {
                    item.Address = new AddressInfoDetail
                    {
                        AddressTypeId = address.AddressTypeId,
                        Street = address.Street,
                        PostalCode = address.PostalCode,
                        Description = address.Description,
                        CountryId = address.CountryId,
                        ProvinceId = address.ProvinceId,
                        RegionId = address.RegionId
                    };

                    if (address.CountryId != null)
                    {
                        item.Address.Country = ContactService.GetCountryDetails(Convert.ToInt32(address.CountryId));
                    }

                    if (address.ProvinceId != null)
                    {
                        item.Address.Province =
                            ContactService.GetProvinceDetails(Convert.ToInt32(address.ProvinceId));
                    }

                    if (address.RegionId != null)
                    {
                        item.Address.Region = ContactService.GetRegionDetails(Convert.ToInt32(address.RegionId));
                    }
                }
            }
            //Save customer info to session
            var sessionManager = new SessionManager();
            sessionManager.SetSession(SessionKey.CustomerInfo, item);

            return item;
        }
        public bool IsDataExisted(string firstName, string lastName)
        {
            return UnitOfWork.CustomerRepository.HasCustomerNameExisted(firstName, lastName);
        }
        public bool IsExistedEmail(string email)
        {
            return UnitOfWork.CustomerRepository.HasEmailExisted(email);
        }
        public void InsertCustomer(Guid applicationId, Guid userId, int vendorId, CustomerEntry entry)
        {
            ISpecification<CustomerEntry> validator = new CustomerEntryValidator(UnitOfWork, PermissionLevel.Create, CurrentClaimsIdentity);
            var violations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, violations);
            if (!isDataValid) throw new ValidationError(violations);

            var entity = entry.ToEntity<CustomerEntry, Customer>();
            if (entry.Address != null && entry.Address.CountryId != null)
            {
                var newAddress = ContactService.InsertAddress(entry.Address);
                if (newAddress != null)
                {
                    entity.AddressId = newAddress.AddressId;
                }
            }

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
                var fileInfo = DocumentService.UploadAndSaveDbByFolderId(applicationId, userId, entry.File, (int)FileLocation.Company, StorageType.Local);
                if (fileInfo != null)
                {
                    entity.Photo = fileInfo.FileId;
                }
            }

            entity.VendorId = vendorId;
            entity.Ip = NetworkUtils.GetIP4Address();
            entity.CreatedByUserId = userId;
            entity.CreatedDate = DateTime.UtcNow;

            UnitOfWork.CustomerRepository.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateCustomer(Guid applicationId, Guid userId, int vendorId, CustomerEditEntry entry)
        {
            string ip = NetworkUtils.GetIP4Address();

            ISpecification<CustomerEditEntry> validator = new CustomerEditEntryValidator(UnitOfWork, PermissionLevel.Edit, CurrentClaimsIdentity);
            var violations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, violations);
            if (!isDataValid) throw new ValidationError(violations);

            var entity = UnitOfWork.CustomerRepository.FindById(entry.CustomerId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundCustomer, "Customer", entry.CustomerId, ErrorMessage.Messages[ErrorCode.NotFoundCustomer]));
                throw new ValidationError(violations);
            }

            if (!string.IsNullOrEmpty(entry.CustomerNo) && entity.CustomerNo != entry.CustomerNo)
            {
                bool isCustomerNoExisted = UnitOfWork.CustomerRepository.HasCustomerNumberExisted(entry.CustomerNo);
                if (isCustomerNoExisted)
                {
                    violations.Add(new RuleViolation(ErrorCode.DuplicateCustomerNo, "CustomerNo", entry.CustomerNo, ErrorMessage.Messages[ErrorCode.DuplicateCustomerNo]));
                    throw new ValidationError(violations);
                }
            }

            if (!string.IsNullOrEmpty(entry.Email) && entity.Email != entry.Email)
            {
                var isEmailExisted = UnitOfWork.CustomerRepository.HasEmailExisted(entry.Email);
                if (isEmailExisted)
                {
                    violations.Add(new RuleViolation(ErrorCode.ExistedEmail, "Email", entry.Email, ErrorMessage.Messages[ErrorCode.ExistedEmail]));
                    throw new ValidationError(violations);
                }
            }

            if (!string.IsNullOrEmpty(entry.HomePhone) && entity.HomePhone != entry.HomePhone)
            {
                var isPhoneExisted = UnitOfWork.CustomerRepository.HasPhoneExisted(entry.HomePhone);
                if (isPhoneExisted)
                {
                    violations.Add(new RuleViolation(ErrorCode.ExistedHomePhone, "HomePhone", entry.HomePhone, ErrorMessage.Messages[ErrorCode.ExistedHomePhone]));
                    throw new ValidationError(violations);
                }
            }

            if (!string.IsNullOrEmpty(entry.WorkPhone) && entity.WorkPhone != entry.WorkPhone)
            {
                var isPhoneExisted = UnitOfWork.CustomerRepository.HasPhoneExisted(entry.WorkPhone);
                if (isPhoneExisted)
                {
                    violations.Add(new RuleViolation(ErrorCode.ExistedWorkPhone, "WorkPhone", entry.WorkPhone, ErrorMessage.Messages[ErrorCode.ExistedWorkPhone]));
                    throw new ValidationError(violations);
                }
            }

            if (entry.Address != null)
            {
                if (entry.Address.AddressId != null && entry.Address.AddressId > 0)
                {
                    var address = new AddressEditEntry
                    {
                        AddressId = entry.Address.AddressId.Value,
                        AddressTypeId = AddressType.Customer,
                        CountryId = entry.Address.CountryId,
                        ProvinceId = entry.Address.ProvinceId,
                        RegionId = entry.Address.RegionId,
                        Street = entry.Address.Street,
                        PostalCode = entry.Address.PostalCode,
                        Description = entry.Address.Description
                    };
                    ContactService.UpdateAddress(address);
                }
                else
                {
                    var addressEntry = new AddressEntry
                    {
                        AddressTypeId = AddressType.Customer,
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

            if (entry.File != null && entry.File.ContentLength > 0)
            {
                int maxContentLength = UnitOfWork.ApplicationSettingRepository.GetAllowedMaxImageContentLength(applicationId);
                string[] allowedFileExtensions = UnitOfWork.ApplicationSettingRepository.GetAllowedImageExtensions(applicationId);

                if (!allowedFileExtensions.Contains(entry.File.FileName.Substring(entry.File.FileName.LastIndexOf('.'))))
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidFileType, "FileUpload", entry.File.FileName, LanguageResource.InvalidFileExtension + " : " + string.Join(", ", allowedFileExtensions)));
                    throw new ValidationError(violations);
                }
                if (entry.File.ContentLength > maxContentLength)
                {
                    violations.Add(new RuleViolation(ErrorCode.MaximumAllowedSize, "FileUpload", entry.File.ContentLength, LanguageResource.InvalidFileSize + " , " + LanguageResource.MaximumAllowedSize + maxContentLength + " MB"));
                    throw new ValidationError(violations);
                }
                var fileInfo = DocumentService.UploadAndSaveDbByFolderId(applicationId, userId, entry.File, (int)FileLocation.Company, StorageType.Local);
                if (fileInfo != null)
                {
                    if (entity.Photo != null)
                    {
                        DocumentService.DeleteFile(Convert.ToInt32(entity.Photo));
                    }
                    entity.Photo = fileInfo.FileId;
                }
            }

            entity.CustomerTypeId = entry.CustomerTypeId;
            entity.CustomerNo = entry.CustomerNo;
            entity.FirstName = entry.FirstName;
            entity.LastName = entry.LastName;
            entity.ContactName = entry.ContactName;
            entity.CardNo = entry.CardNo;
            entity.IdCardNo = entry.IdCardNo;
            entity.PassPortNo = entry.PassPortNo;
            entity.TaxCode = entry.TaxCode;
            entity.BirthDay = entry.BirthDay;
            entity.HomePhone = entry.HomePhone;
            entity.WorkPhone = entry.WorkPhone;
            entity.Mobile = entry.Mobile;
            entity.Fax = entry.Fax;
            entity.Email = entry.Email;
            entity.Gender = entry.Gender;
            entity.IsActive = entry.IsActive;
            entity.LastUpdatedIp = ip;
            entity.LastModifiedDate = DateTime.UtcNow;
            entity.LastModifiedByUserId = userId;

            UnitOfWork.CustomerRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateCustomerStatus(Guid userId, int id, CustomerStatus status)
        {
            string ip = NetworkUtils.GetIP4Address();

            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.CustomerRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundCustomer, "Customer", id, ErrorMessage.Messages[ErrorCode.NotFoundCustomer]));
                throw new ValidationError(violations);
            }

            var isValid = Enum.IsDefined(typeof(CustomerStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status", null,
                    ErrorMessage.Messages[ErrorCode.InvalidStatus]));
                throw new ValidationError(violations);
            }

            if (entity.IsActive == status) return;

            entity.IsActive = status;
            entity.LastUpdatedIp = ip;
            entity.LastModifiedDate = DateTime.UtcNow;
            entity.LastModifiedByUserId = userId;

            UnitOfWork.CustomerRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void EditProfile(Guid applicationId, Guid userId, int vendorId, CustomerEditEntry entry)
        {
            ISpecification<CustomerEditEntry> validator = new CustomerEditEntryValidator(UnitOfWork, PermissionLevel.Edit, CurrentClaimsIdentity);
            var violations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, violations);
            if (!isDataValid) throw new ValidationError(violations);

            var entity = UnitOfWork.CustomerRepository.FindById(entry.CustomerId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundCustomer, "Customer", entry.CustomerId, ErrorMessage.Messages[ErrorCode.NotFoundCustomer]));
                throw new ValidationError(violations);
            }

            if (!string.IsNullOrEmpty(entry.CustomerNo) && entity.CustomerNo != entry.CustomerNo)
            {
                bool isCustomerNoExisted = UnitOfWork.CustomerRepository.HasCustomerNumberExisted(entry.CustomerNo);
                if (isCustomerNoExisted)
                {
                    violations.Add(new RuleViolation(ErrorCode.DuplicateCustomerNo, "CustomerNo", entry.CustomerNo, ErrorMessage.Messages[ErrorCode.DuplicateCustomerNo]));
                    throw new ValidationError(violations);
                }
            }

            if (!string.IsNullOrEmpty(entry.Email) && entity.Email != entry.Email)
            {
                var isEmailExisted = UnitOfWork.CustomerRepository.HasEmailExisted(entry.Email);
                if (isEmailExisted)
                {
                    violations.Add(new RuleViolation(ErrorCode.ExistedEmail, "Email", entry.Email, ErrorMessage.Messages[ErrorCode.ExistedEmail]));
                    throw new ValidationError(violations);
                }
            }

            if (!string.IsNullOrEmpty(entry.HomePhone) && entity.HomePhone != entry.HomePhone)
            {
                var isPhoneExisted = UnitOfWork.CustomerRepository.HasPhoneExisted(entry.HomePhone);
                if (isPhoneExisted)
                {
                    violations.Add(new RuleViolation(ErrorCode.ExistedHomePhone, "HomePhone", entry.HomePhone, ErrorMessage.Messages[ErrorCode.ExistedHomePhone]));
                    throw new ValidationError(violations);
                }
            }

            if (!string.IsNullOrEmpty(entry.WorkPhone) && entity.WorkPhone != entry.WorkPhone)
            {
                var isPhoneExisted = UnitOfWork.CustomerRepository.HasPhoneExisted(entry.WorkPhone);
                if (isPhoneExisted)
                {
                    violations.Add(new RuleViolation(ErrorCode.ExistedWorkPhone, "WorkPhone", entry.WorkPhone, ErrorMessage.Messages[ErrorCode.ExistedWorkPhone]));
                    throw new ValidationError(violations);
                }
            }

            if (entry.Address != null)
            {
                if (entry.Address.AddressId != null && entry.Address.AddressId > 0)
                {
                    var address = new AddressEditEntry
                    {
                        AddressId = entry.Address.AddressId.Value,
                        AddressTypeId = AddressType.Customer,
                        CountryId = entry.Address.CountryId,
                        ProvinceId = entry.Address.ProvinceId,
                        RegionId = entry.Address.RegionId,
                        Street = entry.Address.Street,
                        PostalCode = entry.Address.PostalCode,
                        Description = entry.Address.Description
                    };
                    ContactService.UpdateAddress(address);
                }
                else
                {
                    var addressEntry = new AddressEntry
                    {
                        AddressTypeId = AddressType.Customer,
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

            if (entry.File != null && entry.File.ContentLength > 0)
            {
                int maxContentLength = UnitOfWork.ApplicationSettingRepository.GetAllowedMaxImageContentLength(applicationId);
                string[] allowedFileExtensions = UnitOfWork.ApplicationSettingRepository.GetAllowedImageExtensions(applicationId);

                if (!allowedFileExtensions.Contains(entry.File.FileName.Substring(entry.File.FileName.LastIndexOf('.'))))
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidFileType, "FileUpload", entry.File.FileName, LanguageResource.InvalidFileExtension + " : " + string.Join(", ", allowedFileExtensions)));
                    throw new ValidationError(violations);
                }
                if (entry.File.ContentLength > maxContentLength)
                {
                    violations.Add(new RuleViolation(ErrorCode.MaximumAllowedSize, "FileUpload", entry.File.ContentLength, LanguageResource.InvalidFileSize + " , " + LanguageResource.MaximumAllowedSize + maxContentLength + " MB"));
                    throw new ValidationError(violations);
                }
                var fileInfo = DocumentService.UploadAndSaveDbByFolderId(applicationId, userId, entry.File, (int)FileLocation.Company, StorageType.Local);
                if (fileInfo != null)
                {
                    if (entity.Photo != null)
                    {
                        DocumentService.DeleteFile(Convert.ToInt32(entity.Photo));
                    }
                    entity.Photo = fileInfo.FileId;
                }
            }

            entity.FirstName = entry.FirstName;
            entity.LastName = entry.LastName;
            entity.ContactName = entry.ContactName;
            entity.CardNo = entry.CardNo;
            entity.IdCardNo = entry.IdCardNo;
            entity.PassPortNo = entry.PassPortNo;
            entity.TaxCode = entry.TaxCode;
            entity.BirthDay = entry.BirthDay;
            entity.HomePhone = entry.HomePhone;
            entity.WorkPhone = entry.WorkPhone;
            entity.Mobile = entry.Mobile;
            entity.Fax = entry.Fax;
            entity.Email = entry.Email;
            entity.Gender = entry.Gender;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.CustomerRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void ChangePassword(CustomerChangePassword entry)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.CustomerRepository.FindByUserAndPassword(entry.OldPassword, entry.Email);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundCustomer, "Customer", entry.Email, ErrorMessage.Messages[ErrorCode.NotFoundCustomer]));
                throw new ValidationError(violations);
            }

            if (entity.PasswordSalt != entry.NewPassword)
            {
                entity.PasswordHash = Md5Crypto.GetMd5Hash(entry.NewPassword);
                entity.PasswordSalt = entry.NewPassword;
                entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
                entity.LastModifiedDate = DateTime.UtcNow;

                UnitOfWork.CustomerRepository.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void ActivateCustomer(Guid userId, string customerNo)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.CustomerRepository.FindByCustomerNo(customerNo);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundCustomer, "Customer", customerNo, ErrorMessage.Messages[ErrorCode.NotFoundCustomer]));
                throw new ValidationError(violations);
            }

            entity.Verified = true;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedDate = DateTime.UtcNow;
            entity.LastModifiedByUserId = userId;

            UnitOfWork.CustomerRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void LockCustomerAccount(Guid userId, int customerId)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.CustomerRepository.FindById(customerId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundCustomer, "Customer", customerId, ErrorMessage.Messages[ErrorCode.NotFoundCustomer]));
                throw new ValidationError(violations);
            }

            entity.Verified = false;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedDate = DateTime.UtcNow;
            entity.LastModifiedByUserId = userId;

            UnitOfWork.CustomerRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        #endregion

        #region Customer Type
        public IEnumerable<CustomerTypeDetail> GetCustomerTypes(int vendorId, CustomerTypeSearchEntry entry, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = UnitOfWork.CustomerTypeRepository.GetCustomerTypes(vendorId, entry.CustomerTypeName, entry.IsActive, ref recordCount, orderBy, page, pageSize);
            return lst.ToDtos<CustomerType, CustomerTypeDetail>();
        }
        public IEnumerable<CustomerTypeDetail> GetCustomerTypes(int vendorId, CustomerTypeStatus? status)
        {
            var lst = UnitOfWork.CustomerTypeRepository.GetCustomerTypes(vendorId, status);
            return lst.ToDtos<CustomerType, CustomerTypeDetail>();
        }
        public SelectList PopulateCustomerTypeSelectList(CustomerTypeStatus? status = null, int? selectedValue = null, bool? isShowSelectText = false)
        {
            return UnitOfWork.CustomerTypeRepository.PopulateCustomerTypeSelectList(status, selectedValue, isShowSelectText);
        }
        public CustomerTypeDetail GetCustomerTypeDetail(int id)
        {
            var entity = UnitOfWork.CustomerTypeRepository.FindById(id);
            return entity.ToDto<CustomerType, CustomerTypeDetail>();
        }
        public void InsertCustomerType(Guid userId, int vendorId, CustomerTypeEntry entry)
        {
            string ip = NetworkUtils.GetIP4Address();

            ISpecification<CustomerTypeEntry> validator = new CustomerTypeEntryValidator(UnitOfWork, PermissionLevel.Edit, CurrentClaimsIdentity);
            var dataViolations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isDataValid) throw new ValidationError(dataViolations);

            var entity = entry.ToEntity<CustomerTypeEntry, CustomerType>();
            entity.VendorId = vendorId;
            entity.Ip = ip;
            entity.CreatedByUserId = userId;
            entity.CreatedDate = DateTime.UtcNow;

            UnitOfWork.CustomerTypeRepository.Insert(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateCustomerType(Guid userId, CustomerTypeEditEntry entry)
        {
            string ip = NetworkUtils.GetIP4Address();
            var dataViolations = new List<RuleViolation>();
            var entity = UnitOfWork.CustomerTypeRepository.FindById(entry.CustomerTypeId);
            if (entity == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NotFoundCustomerType, "CustomerType",
                    entry.CustomerTypeId, ErrorMessage.Messages[ErrorCode.NotFoundCustomerType]));
                throw new ValidationError(dataViolations);
            }
            else
            {
                if (entry.CustomerTypeName.Length > 250)
                {
                    dataViolations.Add(new RuleViolation(ErrorCode.InvalidCustomerTypeName, "CustomerTypeName", entry.CustomerTypeName.Length, ErrorMessage.Messages[ErrorCode.InvalidCustomerTypeName]));
                    throw new ValidationError(dataViolations);
                }
                else
                {
                    if (entity.CustomerTypeName != entry.CustomerTypeName)
                    {
                        var isDuplicated = UnitOfWork.CustomerTypeRepository.HasDataExisted(entry.CustomerTypeName);
                        if (isDuplicated)
                        {
                            dataViolations.Add(new RuleViolation(ErrorCode.DuplicateCustomerTypeName, "CustomerTypeName",
                                    entry.CustomerTypeName, ErrorMessage.Messages[ErrorCode.DuplicateCustomerTypeName]));
                            throw new ValidationError(dataViolations);
                        }
                    }
                }
            }

            entity.CustomerTypeName = entry.CustomerTypeName;
            entity.PromotionalRate = entry.PromotionalRate;
            entity.Note = entry.Note;
            entity.IsActive = entry.IsActive;
            entity.LastUpdatedIp = ip;
            entity.LastModifiedDate = DateTime.UtcNow;
            entity.LastModifiedByUserId = userId;

            UnitOfWork.CustomerTypeRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateCustomerTypeStatus(Guid userId, int id, CustomerTypeStatus status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.CustomerTypeRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundCustomerType, "CustomerType", id, ErrorMessage.Messages[ErrorCode.NotFoundCustomerType]));
                throw new ValidationError(violations);
            }

            var isValid = Enum.IsDefined(typeof(CustomerTypeStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status", null,
                    ErrorMessage.Messages[ErrorCode.InvalidStatus]));
                throw new ValidationError(violations);
            }
            if (entity.IsActive == status) return;

            entity.IsActive = status;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedDate = DateTime.UtcNow;
            entity.LastModifiedByUserId = userId;

            UnitOfWork.CustomerTypeRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region Dipose

        private bool _disposed;
        protected override void Dispose(bool isDisposing)
        {
            if (!_disposed)
            {
                if (isDisposing)
                {
                    CacheService = null;
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
