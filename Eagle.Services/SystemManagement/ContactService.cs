using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Core.Settings;
using Eagle.Entities.SystemManagement;
using Eagle.Repositories;
using Eagle.Resources;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.SystemManagement;
using Eagle.Services.Dtos.SystemManagement.Identity;
using Eagle.Services.EntityMapping.Common;
using Eagle.Services.Validations;

namespace Eagle.Services.SystemManagement
{
    public class ContactService : BaseService, IContactService
    {
        private IDocumentService DocumentService { get; set; }

        public ContactService(IUnitOfWork unitOfWork, IDocumentService documentService) : base(unitOfWork)
        {
            DocumentService = documentService;
        }

        #region Addresss----------------------------------------------------------------------------------------------
        public IEnumerable<UserAddressInfoDetail> GetUserAddresses(Guid userId)
        {
            var addresses = new List<UserAddressInfoDetail>();
            var userAddresses = UnitOfWork.UserAddressRepository.GetList(userId).ToList();
            if (userAddresses.Any())
            {
                addresses.AddRange(userAddresses.Select(userAddress => new UserAddressInfoDetail
                {
                    UserAddressId = userAddress.UserAddressId, AddressId = userAddress.AddressId, UserId = userAddress.UserId, IsDefault = userAddress.IsDefault,
                    Address = new AddressDetail
                    {
                        AddressId = userAddress.Address.AddressId, AddressTypeId = userAddress.Address.AddressTypeId, Street = userAddress.Address.Street,
                        PostalCode = userAddress.Address.PostalCode, Description = userAddress.Address.Description, CreatedDate = userAddress.Address.CreatedDate,
                        ModifiedDate = userAddress.Address.ModifiedDate, CountryId = userAddress.Address.CountryId, ProvinceId = userAddress.Address.ProvinceId, RegionId = userAddress.Address.RegionId
                    },
                    Country = userAddress.Country.ToDto<Country, CountryDetail>(),
                    Province = userAddress.Province.ToDto<Province, ProvinceDetail>(),
                    Region = userAddress.Region.ToDto<Region, RegionDetail>()
                }));
            }
            return addresses;
        }
        public IEnumerable<AddressDetail> GetAddresses(out int recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var result = UnitOfWork.AddressRepository.Get(out recordCount, null, orderBy, null, page, pageSize);
            return result.ToDtos<Address, AddressDetail>();
        }
        public AddressInfoDetail GetAddressInfoDetail(int addressId)
        {
            var address = new AddressInfoDetail();
            if (addressId <= 0) return null;
            var item = UnitOfWork.AddressRepository.GetDetails(addressId);
            if (item != null)
            {
                address.AddressId = item.AddressId;
                address.AddressTypeId = item.AddressTypeId;
                address.Street = item.Street;
                address.PostalCode = item.PostalCode;
                address.Description = item.Description;
                address.CountryId = item.CountryId;
                address.ProvinceId = item.ProvinceId;
                address.RegionId = item.RegionId;
                address.Location = item.Location;
                address.Country = item.Country.ToDto<Country, CountryDetail>();
                address.Province = item.Province.ToDto<Province, ProvinceDetail>();
                address.Region = item.Region.ToDto<Region, RegionDetail>();
            };
            return address;
        }
        public AddressDetail GetAddressDetails(int id)
        {
            var entity = UnitOfWork.AddressRepository.FindById(id);
            return entity.ToDto<Address, AddressDetail>();
        }
        public AddressDetail InsertAddress(AddressEntry entry)
        {
            var dataViolations = new List<RuleViolation>();
            if (entry == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NullAddressEntry, "AddressEntry"));
                throw new ValidationError(dataViolations);
            }
            if (entry.CountryId == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.InvalidCountryId, "CountryId", entry.CountryId));
                throw new ValidationError(dataViolations);
            }
            else
            {
                var countryEntity = UnitOfWork.CountryRepository.FindById(entry.CountryId);
                if (countryEntity == null)
                {
                    dataViolations.Add(new RuleViolation(ErrorCode.NotFoundCountry, "CountryId", entry.CountryId));
                    throw new ValidationError(dataViolations);
                }
            }

            if (entry.ProvinceId != null)
            { 
                var provinceEntity = UnitOfWork.ProvinceRepository.FindById(entry.ProvinceId);
                if (provinceEntity == null)
                {
                    dataViolations.Add(new RuleViolation(ErrorCode.NotFoundProvince, "ProvinceId", entry.ProvinceId));
                    throw new ValidationError(dataViolations);
                }
            }
            //else
            //{
            //    dataViolations.Add(new RuleViolation(ErrorCode.InvalidProvinceId, "ProvinceId", entry.ProvinceId));
            //    throw new ValidationError(dataViolations);
            //}

            if (entry.RegionId != null)
            {
                var regionEntity = UnitOfWork.RegionRepository.FindById(entry.RegionId);
                if (regionEntity == null)
                {
                    dataViolations.Add(new RuleViolation(ErrorCode.NotFoundRegion, "RegionId", entry.RegionId));
                    throw new ValidationError(dataViolations);
                }
            }
            //else
            //{
            //    dataViolations.Add(new RuleViolation(ErrorCode.InvalidRegionId, "RegionId", entry.RegionId));
            //    throw new ValidationError(dataViolations);
            //}

            var entity = entry.ToEntity<AddressEntry, Address>();
            entity.CreatedDate = DateTime.UtcNow;

            UnitOfWork.AddressRepository.Insert(entity);
            UnitOfWork.SaveChanges();

            return entity.ToDto<Address, AddressDetail>();
        }
        public void UpdateAddress(AddressEditEntry entry)
        {
            var dataViolations = new List<RuleViolation>();
            if (entry == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NullAddressEntry, "AddressEditEntry"));
                throw new ValidationError(dataViolations);
            }

            if (entry.CountryId == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.InvalidCountryId, "CountryId", entry.CountryId));
                throw new ValidationError(dataViolations);
            }
            else
            {
                var countryEntity = UnitOfWork.CountryRepository.FindById(entry.CountryId);
                if (countryEntity == null)
                {
                    dataViolations.Add(new RuleViolation(ErrorCode.NotFoundCountry, "CountryId", entry.CountryId));
                    throw new ValidationError(dataViolations);
                }
            }

            if (entry.ProvinceId != null)
            {
                var provinceEntity = UnitOfWork.ProvinceRepository.FindById(entry.ProvinceId);
                if (provinceEntity == null)
                {
                    dataViolations.Add(new RuleViolation(ErrorCode.NotFoundProvince, "ProvinceId", entry.ProvinceId));
                    throw new ValidationError(dataViolations);
                }
            }
            //else
            //{
            //    dataViolations.Add(new RuleViolation(ErrorCode.InvalidProvinceId, "ProvinceId", entry.ProvinceId));
            //    throw new ValidationError(dataViolations);
            //}

            if (entry.RegionId != null)
            {
                var regionEntity = UnitOfWork.RegionRepository.FindById(entry.RegionId);
                if (regionEntity == null)
                {
                    dataViolations.Add(new RuleViolation(ErrorCode.NotFoundRegion, "RegionId", entry.RegionId));
                    throw new ValidationError(dataViolations);
                }
               
            }
            //else
            //{
            //    dataViolations.Add(new RuleViolation(ErrorCode.InvalidRegionId, "RegionId", entry.RegionId));
            //    throw new ValidationError(dataViolations);
            //}

            var entity = UnitOfWork.AddressRepository.FindById(entry.AddressId);
            if (entity == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NotFoundAddress, "AddressId", entry.AddressId));
                throw new ValidationError(dataViolations);
            }

            entity.AddressTypeId = entry.AddressTypeId;
            entity.Street = entry.Street;
            entity.PostalCode = entry.PostalCode;
            entity.Description = entry.Description;
            entity.CountryId = entry.CountryId?? GlobalSettings.DefaultCountryId;
            entity.ProvinceId = entry.ProvinceId;
            entity.RegionId = entry.RegionId;
            entity.ModifiedDate = DateTime.UtcNow;

            UnitOfWork.AddressRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void DeleteAddress(int id)
        {
            var dataViolations = new List<RuleViolation>();
            var entity = UnitOfWork.AddressRepository.FindById(id);
            if (entity == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NotFoundAddress, "AddressId", id));
                throw new ValidationError(dataViolations);
            }

            UnitOfWork.AddressRepository.Delete(entity);

            var address = UnitOfWork.UserAddressRepository.FindById(id);
            if (address != null)
            {
                UnitOfWork.UserAddressRepository.Delete(address);
            }
            UnitOfWork.SaveChanges();
        }
        public int? UpdateEmergencyAddress(EmergencyAddressEditEntry entry)
        {
            if (entry.AddressId > 0)
            {
                var emergencyAddressEditEntry = new AddressEditEntry
                {
                    AddressId = entry.AddressId,
                    AddressTypeId = AddressType.Emergency,
                    CountryId = entry.CountryId,
                    ProvinceId = entry.ProvinceId,
                    RegionId = entry.RegionId,
                    Street = entry.Street,
                    PostalCode = entry.PostalCode,
                    Description = entry.Description
                };

                UpdateAddress(emergencyAddressEditEntry);
                return entry.AddressId;
            }
            else
            {
                if (entry.CountryId != null && entry.CountryId > 0
                    && entry.ProvinceId != null && entry.ProvinceId > 0
                    && entry.RegionId != null && entry.RegionId > 0)
                {
                    var emergencyAddressEntry = new AddressEntry
                    {
                        AddressTypeId = AddressType.Emergency,
                        CountryId = entry.CountryId,
                        ProvinceId = entry.ProvinceId,
                        RegionId = entry.RegionId,
                        Street = entry.Street,
                        PostalCode = entry.PostalCode,
                        Description = entry.Description
                    };

                    var newEmergencyAddress = InsertAddress(emergencyAddressEntry);
                    return newEmergencyAddress.AddressId;
                }
                return null;
            }
        }
        public int? UpdatePermanentAddress(PermanentAddressEditEntry entry)
        {
            if (entry.AddressId > 0)
            {
                var address = new AddressEditEntry
                {
                    AddressId = entry.AddressId,
                    AddressTypeId = AddressType.Permanent,
                    CountryId = entry.CountryId,
                    ProvinceId = entry.ProvinceId,
                    RegionId = entry.RegionId,
                    Street = entry.Street,
                    PostalCode = entry.PostalCode,
                    Description = entry.Description
                };

                UpdateAddress(address);
                return entry.AddressId;
            }
            else
            {
                if (entry.CountryId != null && entry.CountryId > 0
                    && entry.ProvinceId != null && entry.ProvinceId > 0
                    && entry.RegionId != null && entry.RegionId > 0)
                {
                    var address = new AddressEntry
                    {
                        AddressTypeId = AddressType.Permanent,
                        CountryId = entry.CountryId,
                        ProvinceId = entry.ProvinceId,
                        RegionId = entry.RegionId,
                        Street = entry.Street,
                        PostalCode = entry.PostalCode,
                        Description = entry.Description
                    };

                    var newPermanentAddress = InsertAddress(address);
                    return newPermanentAddress.AddressId;
                }
                return null;
            }
        }
        #endregion ----------------------------------------------------------------------------------------------


        #region Contact ----------------------------------------------------------------------------------------------
        public IEnumerable<ContactDetail> GetContacts(out int recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var result = UnitOfWork.ContactRepository.Get(out recordCount, null, orderBy, null, page, pageSize);
            return result.ToDtos<Contact, ContactDetail>();
        }
        public ContactDetail GetContactDetails(int id)
        {
            var contact = UnitOfWork.ContactRepository.FindById(id);
            return contact.ToDto<Contact, ContactDetail>();
        }

        public ContactInfoDetail GetContactInfoDetails(int id)
        {
            var dataViolations = new List<RuleViolation>();
            var contact = UnitOfWork.ContactRepository.FindById(id);
            if (contact == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NotFoundContact, "Contact", LanguageResource.NotFoundContact));
                throw new ValidationError(dataViolations);
            }

            var item = new ContactInfoDetail
            {
                ContactId = contact.ContactId,
                Title = contact.Title,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                FullName = contact.FirstName + " " + contact.LastName,
                DisplayName = contact.DisplayName,
                Sex = contact.Sex,
                Dob = contact.Dob,
                JobTitle = contact.JobTitle,
                Photo = contact.Photo,
                LinePhone1 = contact.LinePhone1,
                LinePhone2 = contact.LinePhone2,
                Mobile = contact.Mobile,
                Fax = contact.Fax,
                Email = contact.Email,
                Website = contact.Website,
                IdNo = contact.IdNo,
                IdIssuedDate = contact.IdIssuedDate,
                TaxNo = contact.TaxNo,
                IsActive = contact.IsActive,
                
            };

            if (contact.Photo != null)
            {
                var photoInfo = DocumentService.GetFileInfoDetail(Convert.ToInt32(contact.Photo));
                item.DocumentInfo = photoInfo;
                item.FileUrl = photoInfo.FileUrl;
            }
            return item;
        }

        public ContactDetail InsertContact(ContactEntry entry)
        {
            var dataViolations = new List<RuleViolation>();
            if (entry == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NullContactEntry, "ContactEntry"));
                throw new ValidationError(dataViolations);
            }

            bool flag = UnitOfWork.ContactRepository.HasDataExisted(entry.Email, entry.Mobile,
                entry.FirstName, entry.LastName, entry.Sex, entry.Dob, entry.LinePhone1, entry.LinePhone2, entry.TaxNo);
            if (flag)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.DuplicateContact, "ContactName"));
                throw new ValidationError(dataViolations);
            }

            var entity = entry.ToEntity<ContactEntry, Contact>();
            entity.Title = entry.Sex == SexType.Male ? LanguageResource.Mr : LanguageResource.Ms;
            entity.CreatedOn = DateTime.UtcNow;

            UnitOfWork.ContactRepository.Insert(entity);
            UnitOfWork.SaveChanges();
            return entity.ToDto<Contact, ContactDetail>();
        }
        public void UpdateContact(ContactEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullContactEditEntry, "ContactEditEntry"));
                throw new ValidationError(violations);
            }

            var entity = UnitOfWork.ContactRepository.FindById(entry.ContactId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundContact, "ContactId", entry.ContactId));
                throw new ValidationError(violations);
            }

            if (entity.Email != entry.Email)
            {
                var hasEmailExisted = UnitOfWork.EmployeeRepository.HasEmailExisted(entry.Email);
                if (hasEmailExisted)
                {
                    violations.Add(new RuleViolation(ErrorCode.ExistedEmail, "Email", entry.Email, ErrorMessage.Messages[ErrorCode.ExistedEmail]));
                    throw new ValidationError(violations);
                }
            }

            if (!string.IsNullOrEmpty(entry.LinePhone1) && entry.LinePhone1.Length < 50 &&
                entity.LinePhone1 != entry.LinePhone1)
            {
                var hasPhoneExisted = UnitOfWork.EmployeeRepository.HasPhoneExisted(entry.LinePhone1);
                if (hasPhoneExisted)
                {
                    violations.Add(new RuleViolation(ErrorCode.ExistedPhone, "LinePhone1", entry.LinePhone1,
                        ErrorMessage.Messages[ErrorCode.ExistedPhone]));
                    throw new ValidationError(violations);
                }
            }

            if (!string.IsNullOrEmpty(entry.LinePhone2) && entry.LinePhone2.Length < 50 &&
                entity.LinePhone2 != entry.LinePhone2)
            {
                var hasPhoneExisted = UnitOfWork.EmployeeRepository.HasPhoneExisted(entry.LinePhone2);
                if (hasPhoneExisted)
                {
                    violations.Add(new RuleViolation(ErrorCode.ExistedPhone, "LinePhone2", entry.LinePhone2,
                        ErrorMessage.Messages[ErrorCode.ExistedPhone]));
                    throw new ValidationError(violations);
                }
            }
            
            entity.Title = entry.Sex == SexType.Male ? LanguageResource.Mr : LanguageResource.Ms;
            entity.FirstName = entry.FirstName;
            entity.LastName = entry.LastName;
            entity.DisplayName = entry.DisplayName;
            entity.Sex = entry.Sex;
            entity.JobTitle = entry.JobTitle;
            entity.Dob = entry.Dob;
            entity.Photo = entry.Photo;
            entity.LinePhone1 = entry.LinePhone1;
            entity.LinePhone2 = entry.LinePhone2;
            entity.Mobile = entry.Mobile;
            entity.Fax = entry.Fax;
            entity.Email = entry.Email;
            entity.Website = entry.Website;
            entity.IdNo = entry.IdNo;
            entity.IdIssuedDate = entry.IdIssuedDate;
            entity.TaxNo = entry.TaxNo;
            entity.ModifiedOn = DateTime.UtcNow;

            UnitOfWork.ContactRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateContactPhoto(int id, int photo)
        {
            var dataViolations = new List<RuleViolation>();
            var entity = UnitOfWork.ContactRepository.FindById(id);
            if (entity == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NotFoundContact, "ContactId", id));
                throw new ValidationError(dataViolations);
            }

            entity.Photo = photo;
            entity.ModifiedOn = DateTime.UtcNow;

            UnitOfWork.ContactRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateContactStatus(int id, bool status)
        {
            var dataViolations = new List<RuleViolation>();
            var entity = UnitOfWork.ContactRepository.FindById(id);
            if (entity == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NotFoundContact, "ContactId", id));
                throw new ValidationError(dataViolations);
            }

            entity.IsActive = status;
            entity.ModifiedOn = DateTime.UtcNow;

            UnitOfWork.ContactRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion ----------------------------------------------------------------------------------------------

        #region Country ----------------------------------------------------------------------------------------------
        public IEnumerable<CountryDetail> GetCountries(out int recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var result = UnitOfWork.CountryRepository.Get(out recordCount, null, orderBy, null, page, pageSize);
            return result.ToDtos<Country, CountryDetail>();
        }
        public SelectList PopulateCountrySelectList(bool? status=true, int? selectedValue = null, bool isShowSelectText = true)
        {
            return UnitOfWork.CountryRepository.PopulateCountrySelectList(status, selectedValue, isShowSelectText);
        }
        public CountryDetail GetCountryDetails(int id)
        {
            var entity = UnitOfWork.CountryRepository.FindById(id);
            return entity.ToDto<Country, CountryDetail>();
        }
        public int? InsertCountry(CountryEntry entry)
        {
            var dataViolations = new List<RuleViolation>();
            if (entry == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NullCountryEntry, "CountryEntry"));
                throw new ValidationError(dataViolations);
            }
            else
            {
                bool flag = UnitOfWork.ModuleRepository.HasModuleNameExisted(entry.CountryName);
                if (flag)
                {
                    dataViolations.Add(new RuleViolation(ErrorCode.DuplicateCountryName, "CountryName"));
                    throw new ValidationError(dataViolations);
                }

                var entity = entry.ToEntity<CountryEntry, Country>();
                UnitOfWork.CountryRepository.Insert(entity);
                UnitOfWork.SaveChanges();
                return entity.CountryId;
            }
        }
        public void UpdateCountry(int id, CountryEntry entry)
        {
            var dataViolations = new List<RuleViolation>();
            var entity = UnitOfWork.CountryRepository.FindById(id);
            if (entity == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NotFoundCountry, "CountryId", id));
                throw new ValidationError(dataViolations);
            }

            entity.CountryName = entry.CountryName;
            entity.NiceName = entry.NiceName;
            entity.Iso = entry.Iso;
            entity.Iso3 = entry.Iso3;
            entity.NumCode = entry.NumCode;
            entity.PhoneCode = entry.PhoneCode;
            entity.IsActive = true;

            UnitOfWork.CountryRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateCountryStatus(int id, bool status)
        {
            var dataViolations = new List<RuleViolation>();
            var entity = UnitOfWork.CountryRepository.FindById(id);
            if (entity == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NotFoundCountry, "CountryId", id));
                throw new ValidationError(dataViolations);
            }

            entity.IsActive = status;
            UnitOfWork.CountryRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion ----------------------------------------------------------------------------------------------
        
        #region City - Province ----------------------------------------------------------------------------------------------
        public IEnumerable<ProvinceDetail> GetProvinces(out int recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var result = UnitOfWork.ProvinceRepository.Get(out recordCount, null, orderBy, null, page, pageSize);
            return result.ToDtos<Province, ProvinceDetail>();
        }
        public ProvinceDetail GetProvinceDetails(int id)
        {
            var entity = UnitOfWork.ProvinceRepository.FindById(id);
            return entity.ToDto<Province, ProvinceDetail>();
        }
        public SelectList PopulateProvinceSelectList(int countryId, bool? status = true, int? selectedValue=null, bool? isShowSelectText = true)
        {
            return UnitOfWork.ProvinceRepository.PopulateProvinceSelectList(countryId, status, selectedValue, isShowSelectText);
        }
        public int? InsertProvince(ProvinceEntry entry)
        {
            var dataViolations = new List<RuleViolation>();
            if (entry == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NullProvinceEntry, "ProvinceEntry"));
                throw new ValidationError(dataViolations);
            }
            else
            {
                bool flag = UnitOfWork.ProvinceRepository.HasDataExisted(entry.ProvinceName);
                if (flag)
                {
                    dataViolations.Add(new RuleViolation(ErrorCode.DuplicateProvinceName, "ProvinceName"));
                    throw new ValidationError(dataViolations);
                }

                var entity = entry.ToEntity<ProvinceEntry, Province>();
                UnitOfWork.ProvinceRepository.Insert(entity);
                UnitOfWork.SaveChanges();
                return entity.ProvinceId;
            }
        }
        public void UpdateProvince(int id, ProvinceEntry entry)
        {
            var dataViolations = new List<RuleViolation>();
            var entity = UnitOfWork.ProvinceRepository.FindById(id);
            if (entity == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NotFoundProvince, "ProvinceId", id));
                throw new ValidationError(dataViolations);
            }

            entity.ProvinceCode = entry.ProvinceCode;
            entity.ProvinceName = entry.ProvinceName;

            UnitOfWork.ProvinceRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateProvinceStatus(int id, bool status)
        {
            var dataViolations = new List<RuleViolation>();
            var entity = UnitOfWork.ProvinceRepository.FindById(id);
            if (entity == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NotFoundProvince, "ProvinceId", id));
                throw new ValidationError(dataViolations);
            }

            entity.IsActive = status;
            UnitOfWork.ProvinceRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion ----------------------------------------------------------------------------------------------

        #region District - Region ----------------------------------------------------------------------------------------------
        public IEnumerable<RegionDetail> GetRegions(out int recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var result = UnitOfWork.RegionRepository.Get(out recordCount, null, orderBy, null, page, pageSize);
            return result.ToDtos<Region, RegionDetail>();
        }
        public RegionDetail GetRegionDetails(int id)
        {
            var entity = UnitOfWork.RegionRepository.FindById(id);
            return entity.ToDto<Region, RegionDetail>();
        }
        public SelectList PopulateRegionSelectList(int? provinceId, bool? status = true, int? selectedValue=null, bool? isShowSelectText = true)
        {
            return UnitOfWork.RegionRepository.PopulateRegionSelectList(provinceId, status, selectedValue, isShowSelectText);
        }
        public int? InsertRegion(RegionEntry entry)
        {
            var dataViolations = new List<RuleViolation>();
            if (entry == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NullRegionEntry, "RegionEntry"));
                throw new ValidationError(dataViolations);
            }
            else
            {
                bool flag = UnitOfWork.RegionRepository.HasDataExisted(entry.RegionName);
                if (flag)
                {
                    dataViolations.Add(new RuleViolation(ErrorCode.DuplicateRegionName, "RegionName"));
                    throw new ValidationError(dataViolations);
                }

                var entity = entry.ToEntity<RegionEntry, Region>();
                UnitOfWork.RegionRepository.Insert(entity);
                UnitOfWork.SaveChanges();
                return entity.RegionId;
            }
        }
        public void UpdateRegion(int id, RegionEntry entry)
        {
            var dataViolations = new List<RuleViolation>();
            var entity = UnitOfWork.RegionRepository.FindById(id);
            if (entity == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NotFoundRegion, "RegionId", id));
                throw new ValidationError(dataViolations);
            }

            entity.RegionCode = entry.RegionCode;
            entity.RegionName = entry.RegionName;

            UnitOfWork.RegionRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateRegionStatus(int id, bool status)
        {
            var dataViolations = new List<RuleViolation>();
            var entity = UnitOfWork.RegionRepository.FindById(id);
            if (entity == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NotFoundRegion, "RegionId", id));
                throw new ValidationError(dataViolations);
            }

            entity.IsActive = status;
            UnitOfWork.RegionRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion ----------------------------------------------------------------------------------------------

        #region Dipose

        private bool _disposed = false;
        protected override void Dispose(bool isDisposing)
        {
            if (!this._disposed)
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
