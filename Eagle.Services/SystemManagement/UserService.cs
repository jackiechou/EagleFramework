using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using Eagle.Common.Security.Cryptography;
using Eagle.Common.Utilities;
using Eagle.Core.Configuration;
using Eagle.Core.Permission;
using Eagle.Core.Settings;
using Eagle.Entities.SystemManagement;
using Eagle.Repositories;
using Eagle.Resources;
using Eagle.Services.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.SystemManagement;
using Eagle.Services.Dtos.SystemManagement.Identity;
using Eagle.Services.EntityMapping.Common;
using Eagle.Services.SystemManagement.Validation;
using Eagle.Services.Validations;

namespace Eagle.Services.SystemManagement
{
    public class UserService : BaseService, IUserService
    {
        private IApplicationService ApplicationService { get; set; }
        private IContactService ContactService { get; set; }
        private ICurrencyService CurrencyService { get; set; }
        private IDocumentService DocumentService { get; set; }
        private IRoleService RoleService { get; set; }

        public UserService(IUnitOfWork unitOfWork, IApplicationService applicationService,
            ICurrencyService currencyService, IContactService contactService, IDocumentService documentService,
            IRoleService roleService) : base(unitOfWork)
        {
            ApplicationService = applicationService;
            ContactService = contactService;
            CurrencyService = currencyService;
            DocumentService = documentService;
            RoleService = roleService;
        }

        #region User
        public IEnumerable<UserContactDetail> SearchUsers(Guid applicationId, UserSearchEntry filter, out int recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = UnitOfWork.UserRepository.SearchUsers(applicationId, out recordCount, filter.RoleId, filter.KeyName, filter.Email, filter.Mobile, filter.Phone, filter.IsApproved, filter.IsLockedOut, filter.IsSuperUser, orderBy, page,
               pageSize).ToList();
            var users = new List<UserContactDetail>();
            if (lst.Any())
            {
                users.AddRange(from item in lst
                               let contact = ContactService.GetContactInfoDetails(item.Profile.ContactId)
                               let addresses = ContactService.GetUserAddresses(item.UserId)
                               let profile = new UserProfileInfoDetail
                               {
                                   ProfileId = item.Profile.ProfileId,
                                   UserId = item.UserId,
                                   ContactId = item.Profile.ContactId,
                                   Contact = contact,
                                   Addresses = addresses
                               }
                               select new UserContactDetail
                               {
                                   ApplicationId = item.ApplicationId,
                                   SeqNo = item.SeqNo,
                                   UserId = item.UserId,
                                   UserName = item.UserName,
                                   LoweredUserName = item.LoweredUserName,
                                   Password = item.Password,
                                   PasswordSalt = item.PasswordSalt,
                                   PasswordQuestion = item.PasswordQuestion,
                                   PasswordAnswer = item.PasswordAnswer,
                                   IsSuperUser = item.IsSuperUser,
                                   IsApproved = item.IsApproved,
                                   IsLockedOut = item.IsLockedOut,
                                   UpdatePassword = item.UpdatePassword,
                                   EmailConfirmed = item.EmailConfirmed,
                                   LastPasswordChangedDate = item.LastPasswordChangedDate,
                                   FailedPasswordAttemptCount = item.FailedPasswordAttemptCount,
                                   FailedPasswordAttemptTime = item.FailedPasswordAttemptTime,
                                   FailedPasswordAnswerAttemptCount = item.FailedPasswordAnswerAttemptCount,
                                   FailedPasswordAnswerAttemptTime = item.FailedPasswordAnswerAttemptTime,
                                   StartDate = item.StartDate,
                                   ExpiredDate = item.ExpiredDate,
                                   LastLoginDate = item.LastLoginDate,
                                   LastLockoutDate = item.LastLockoutDate,
                                   LastActivityDate = item.LastActivityDate,
                                   Ip = item.Ip,
                                   LastUpdatedIp = item.LastUpdatedIp,
                                   CreatedDate = item.CreatedDate,
                                   LastModifiedDate = item.LastModifiedDate,
                                   CreatedByUserId = item.CreatedByUserId,
                                   LastModifiedByUserId = item.LastModifiedByUserId,
                                   Profile = profile
                               });
            }
            return users;
        }
        public IEnumerable<UserContactDetail> GetUsers(Guid applicationId, out int recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = UnitOfWork.UserRepository.GetList(applicationId, out recordCount, orderBy, page,
                pageSize).ToList();

            var users = new List<UserContactDetail>();
            if (lst.Any())
            {
                users.AddRange(from item in lst
                               let contact = ContactService.GetContactInfoDetails(item.Profile.ContactId)
                               let addresses = ContactService.GetUserAddresses(item.UserId)
                               let profile = new UserProfileInfoDetail
                               {
                                   ProfileId = item.Profile.ProfileId,
                                   UserId = item.UserId,
                                   ContactId = item.Profile.ContactId,
                                   Contact = contact,
                                   Addresses = addresses
                               }
                               select new UserContactDetail
                               {
                                   ApplicationId = item.ApplicationId,
                                   SeqNo = item.SeqNo,
                                   UserId = item.UserId,
                                   UserName = item.UserName,
                                   LoweredUserName = item.LoweredUserName,
                                   Password = item.Password,
                                   PasswordSalt = item.PasswordSalt,
                                   PasswordQuestion = item.PasswordQuestion,
                                   PasswordAnswer = item.PasswordAnswer,
                                   IsSuperUser = item.IsSuperUser,
                                   IsApproved = item.IsApproved,
                                   IsLockedOut = item.IsLockedOut,
                                   UpdatePassword = item.UpdatePassword,
                                   EmailConfirmed = item.EmailConfirmed,
                                   LastPasswordChangedDate = item.LastPasswordChangedDate,
                                   FailedPasswordAttemptCount = item.FailedPasswordAttemptCount,
                                   FailedPasswordAttemptTime = item.FailedPasswordAttemptTime,
                                   FailedPasswordAnswerAttemptCount = item.FailedPasswordAnswerAttemptCount,
                                   FailedPasswordAnswerAttemptTime = item.FailedPasswordAnswerAttemptTime,
                                   StartDate = item.StartDate,
                                   ExpiredDate = item.ExpiredDate,
                                   LastLoginDate = item.LastLoginDate,
                                   LastLockoutDate = item.LastLockoutDate,
                                   LastActivityDate = item.LastActivityDate,
                                   Ip = item.Ip,
                                   LastUpdatedIp = item.LastUpdatedIp,
                                   CreatedDate = item.CreatedDate,
                                   LastModifiedDate = item.LastModifiedDate,
                                   CreatedByUserId = item.CreatedByUserId,
                                   LastModifiedByUserId = item.LastModifiedByUserId,
                                   Profile = profile
                               });
            }
            return users;
        }
        public IEnumerable<UserDetail> GetUserOnlines(Guid applicationId, int minutes)
        {
            var lst = UnitOfWork.UserRepository.GetUserOnlines(applicationId, minutes);
            return lst.ToDtos<User, UserDetail>();
        }
        public UserDetail GetDetailsByUserName(string userName)
        {
            var entity = UnitOfWork.UserRepository.FindByUserName(userName);
            return entity.ToDto<User, UserDetail>();
        }
        public SelectList PopulateQuestionsSelectList(string selectedValue = null, bool? isShowSelectText = false)
        {
            return UnitOfWork.UserRepository.PopulateQuestionsSelectList(selectedValue, isShowSelectText);
        }
        public SelectList PopulateSexSelectList(int? selectedValue = null, bool? isShowSelectText = false)
        {
            return UnitOfWork.UserRepository.PopulateSexSelectList(selectedValue, isShowSelectText);
        }
        public SelectList PopulateTitleSelectList(int? selectedValue = null, bool? isShowSelectText = false)
        {
            return UnitOfWork.UserRepository.PopulateTitleSelectList(selectedValue, isShowSelectText);
        }
        public SelectList PopulateStatusSelectList(int? selectedValue = null, bool? isShowSelectText = false)
        {
            return UnitOfWork.UserRepository.PopulateStatusSelectList(selectedValue, isShowSelectText);
        }

        public UserDetail CreateUser(Guid applicationId, Guid userId, int vendorId, UserEntry entry)
        {
            using (var tranScope = new TransactionScope())
            {
                ISpecification<UserEntry> validator = new UserEntryValidator(UnitOfWork, PermissionLevel.Create);
                var dataViolations = new List<RuleViolation>();
                var isDataValid = validator.IsSatisfyBy(entry, dataViolations);
                if (!isDataValid) throw new ValidationError(dataViolations);

                var entity = entry.ToEntity<UserEntry, User>();
                entity.LoweredUserName = entry.UserName.ToLower();
                entity.Password = Md5Crypto.GetMd5Hash(entry.PasswordSalt); 
                entity.PasswordSalt = entry.PasswordSalt;
                entity.IsSuperUser = entry.IsSuperUser;
                entity.ApplicationId = applicationId;
                entity.Ip = NetworkUtils.GetIP4Address();
                entity.CreatedByUserId = userId;
                entity.CreatedDate = DateTime.UtcNow;

                UnitOfWork.UserRepository.Insert(entity);
                UnitOfWork.SaveChanges();

                if (entry.Contact != null)
                {
                    var contact = ContactService.InsertContact(entry.Contact);
                    if (contact != null)
                    {
                        var profileEntry = new UserProfileEntry
                        {
                            UserId = entity.UserId,
                            ContactId = contact.ContactId
                        };
                        CreateProfile(profileEntry);

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
                                var photoInfo = DocumentService.UploadAndSaveDbByFolderId(applicationId, userId, entry.Contact.FileUpload, (int)FileLocation.User, StorageType.Local);
                                if (photoInfo != null)
                                {
                                    ContactService.UpdateContactPhoto(contact.ContactId, Convert.ToInt32(photoInfo.FileId));
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
                        var userAddressEntry = new UserAddressEntry
                        {
                            UserId = entity.UserId,
                            AddressId = newEmergencyAddress.AddressId,
                            IsDefault = false
                        };
                        CreateUserAddress(userAddressEntry);
                    }
                }

                if (entry.PermanentAddress != null && entry.PermanentAddress.CountryId != null)
                {
                    var newPermanentAddress = ContactService.InsertAddress(entry.PermanentAddress);
                    if (newPermanentAddress != null)
                    {
                        var userAddressEntry = new UserAddressEntry
                        {
                            UserId = entity.UserId,
                            AddressId = newPermanentAddress.AddressId,
                            IsDefault = false
                        };
                        CreateUserAddress(userAddressEntry);
                    }
                }

                if (entry.UserRoles.Any())
                {
                    var userRoleEntries = entry.UserRoles.Where(x => x.IsAllowed == true).ToList();
                    CreateRolesForUser(entity.UserId, userRoleEntries);
                }

                if (entry.UserRoleGroups.Any())
                {
                    var userGroupEntries = entry.UserRoleGroups.Where(x => x.IsAllowed == true).ToList();
                    CreateRoleGroupsForUser(entity.UserId, userGroupEntries);
                }

                InsertUserVendor(userId, vendorId);

                tranScope.Complete();

                return entity.ToDto<User, UserDetail>();
            }
        }
        public void EditUser(Guid applicationId, Guid userId, UserEditEntry entry)
        {
            using (var tranScope = new TransactionScope())
            {
                ISpecification<UserEditEntry> validator = new UserEditEntryValidator(UnitOfWork, PermissionLevel.Edit);
                var violations = new List<RuleViolation>();
                var isDataValid = validator.IsSatisfyBy(entry, violations);
                if (!isDataValid) throw new ValidationError(violations);

                var entity = UnitOfWork.UserRepository.FindById(entry.UserId);
                if (entity == null) return;

                entity.UserName = entry.UserName;
                entity.Password = Md5Crypto.GetMd5Hash(entry.PasswordSalt);
                entity.PasswordSalt = entry.PasswordSalt;
                entity.PasswordQuestion = entry.PasswordQuestion;
                entity.PasswordAnswer = entry.PasswordAnswer;
                entity.IsSuperUser = entry.IsSuperUser;
                entity.StartDate = entity.StartDate;
                entity.ExpiredDate = entity.ExpiredDate;
                entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
                entity.LastModifiedByUserId = userId;
                entity.LastModifiedDate = DateTime.UtcNow;

                UnitOfWork.UserRepository.Update(entity);

                //Edit user profile and contact
                EditUserProfile(entity.ApplicationId, entity.UserId, entry.Contact);

                //Check user roles
                if (entry.UserRoles.Any())
                {
                    var selectedUserRoles = entry.UserRoles.Where(x => x.IsAllowed == true).ToList();
                    if (selectedUserRoles.Any())
                    {
                        //Update user roles
                        EditUserRoles(entity.UserId, selectedUserRoles);

                        //Update user role groups what the roles are selected
                        var selectedRoleIds = selectedUserRoles.Select(x => x.RoleId).ToList();
                        if (selectedRoleIds.Any())
                        {
                            //var userRoleGroups = entry.UserRoles.UserRoleGroups.Where(x => selectedRoleIds.Contains(x.Role.RoleId)).ToList();
                            var selectedUserRoleGroups = new List<UserRoleGroupEdit>();
                            foreach (var userRole in selectedUserRoles)
                            {
                                var userRoleGroups = userRole.UserRoleGroups;
                                selectedUserRoleGroups.AddRange(userRoleGroups.Where(x => x.IsAllowed == true));
                            }
                            EditUserRoleGroups(entity.UserId, selectedUserRoleGroups);
                        }
                    }
                }
                
                //Edit Emergency Address
                if (entry.EmergencyAddress != null)
                {
                    EditUserEmergencyAddress(userId, entry.EmergencyAddress);
                }

                //Edit Permanent Address
                if (entry.PermanentAddress != null)
                {
                    EditUserPermanentAddress(userId, entry.PermanentAddress);
                }

                UnitOfWork.UserRepository.Update(entity);
                UnitOfWork.SaveChanges();

                tranScope.Complete();
            }
        }
        public void Approve(Guid userId)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.UserRepository.FindById(userId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundUser, "User"));
                throw new ValidationError(violations);
            }

            entity.IsApproved = true;
            entity.LastModifiedDate = DateTime.UtcNow;
            UnitOfWork.UserRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UnApprove(Guid userId)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.UserRepository.FindById(userId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundUser, "User"));
                throw new ValidationError(violations);
            }

            entity.IsApproved = false;
            entity.LastModifiedDate = DateTime.UtcNow;
            UnitOfWork.UserRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void LockUser(Guid userId)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.UserRepository.FindById(userId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundUser, "User", userId));
                throw new ValidationError(violations);
            }

            entity.IsLockedOut = true;
            entity.LastLockoutDate = DateTime.UtcNow;
            entity.LastModifiedDate = DateTime.UtcNow;
            UnitOfWork.UserRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UnLockUser(Guid userId)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.UserRepository.FindById(userId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundUser, "User", userId));
                throw new ValidationError(violations);
            }

            entity.IsLockedOut = false;
            entity.FailedPasswordAttemptCount = 0;
            entity.FailedPasswordAnswerAttemptCount = 0;
            entity.LastLockoutDate = DateTime.UtcNow;
            entity.LastModifiedDate = DateTime.UtcNow;
            UnitOfWork.UserRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UnLockAccount(string userName)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.UserRepository.FindByUserName(userName);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundUser, "User", userName));
                throw new ValidationError(violations);
            }

            entity.IsLockedOut = false;
            entity.FailedPasswordAttemptCount = 0;
            entity.FailedPasswordAnswerAttemptCount = 0;
            entity.LastLockoutDate = DateTime.UtcNow;
            entity.LastModifiedDate = DateTime.UtcNow;
            UnitOfWork.UserRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        #endregion
        
        #region User Address

        public void CreateUserAddress(UserAddressEntry entry)
        {
            var dataViolations = new List<RuleViolation>();
            if (entry == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NullUserAddressEntry, "UserAddressEntry"));
                throw new ValidationError(dataViolations);
            }

            bool flag = UnitOfWork.UserAddressRepository.HasDataExisted(entry.UserId, entry.AddressId);
            if (flag) return;

            var entity = new UserAddress
            {
                UserId = entry.UserId,
                AddressId = entry.AddressId,
                IsDefault = entry.IsDefault ?? false
            };
            UnitOfWork.UserAddressRepository.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void EditUserEmergencyAddress(Guid userId, EmergencyAddressEditEntry entry)
        {
            if (userId == Guid.Empty || userId == Guid.Parse("00000000-0000-0000-0000-000000000000") || entry == null) return;

            //Insert or Update emergency address
            var emergencyAddressId = ContactService.UpdateEmergencyAddress(entry);
            if (emergencyAddressId != null && emergencyAddressId > 0)
            {
                var userAddressEntry = new UserAddressEntry
                {
                    UserId = userId,
                    AddressId = Convert.ToInt32(emergencyAddressId),
                    IsDefault = false
                };
                CreateUserAddress(userAddressEntry); //No update if it has been existed
            }
        }
        public void EditUserPermanentAddress(Guid userId, PermanentAddressEditEntry entry)
        {
            if (userId == Guid.Empty || userId == Guid.Parse("00000000-0000-0000-0000-000000000000") || entry == null) return;
            
            //Insert or Update Permanent Address
            var permanentAddressId = ContactService.UpdatePermanentAddress(entry);
            if (permanentAddressId != null && permanentAddressId > 0)
            {
                var userAddressEntry = new UserAddressEntry
                {
                    UserId = userId,
                    AddressId = Convert.ToInt32(permanentAddressId),
                    IsDefault = false
                };
                CreateUserAddress(userAddressEntry); //No update if it has been existed
            }
        }

        #endregion

        #region User Profile

        public IEnumerable<UserProfileDetail> GetProfiles(List<Guid> userIds)
        {
            var lst = UnitOfWork.UserRepository.GetProfiles(userIds);
            return lst.ToDtos<UserProfile, UserProfileDetail>();
        }
        public UserInfoDetail GetUserProfile(Guid userId)
        {
            var dataViolations = new List<RuleViolation>();
            var userItem = UnitOfWork.UserRepository.GetDetails(userId);
            if (userItem == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NotFoundUser, "UserId", LanguageResource.NotFoundUser + " : " + string.Join(", ", userId)));
                throw new ValidationError(dataViolations);
            }

            if (userItem.Profile == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NotFoundProfile, "Profile", LanguageResource.NotFoundProfile + " : " + string.Join(", ", userId)));
                throw new ValidationError(dataViolations);
            }

            var contactDetail = ContactService.GetContactInfoDetails(userItem.Profile.ContactId);
            var addressres = ContactService.GetUserAddresses(userItem.User.UserId);

            var application = userItem.Application.ToDto<ApplicationEntity, ApplicationDetail>();
            var user = userItem.User.ToDto<User, UserDetail>();
            var profile = new UserProfileInfoDetail
            {
                ProfileId = userItem.Profile.ProfileId,
                UserId = userItem.Profile.UserId,
                ContactId = userItem.Profile.ContactId,
                Contact = contactDetail,
                Addresses = addressres
            };

            return new UserInfoDetail
            {
                Application = application,
                User = user,
                Profile = profile,
            };
        }
        public UserInfoDetail GetDetailsByEmail(string email)
        {
            var dataViolations = new List<RuleViolation>();
            var user = UnitOfWork.UserRepository.FindByEmail(email);
            if (user == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NotFoundUser, "Email", LanguageResource.NotFoundUser + " : " + string.Join(", ", email)));
                throw new ValidationError(dataViolations);
            }

            //user.Profile =  GetProfileDetails(user.UserId);

            // item.Roles = RoleService.GetRoles(userId, true);
            return user.ToDto<UserInfo, UserInfoDetail>();
        }
        public UserProfileInfoDetail GetProfileDetails(Guid userId)
        {
            var dataViolations = new List<RuleViolation>();
            var userItem = UnitOfWork.UserProfileRepository.GetProfile(userId);
            if (userItem == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NotFoundProfile, "UserId", LanguageResource.NotFoundUser + " : " + string.Join(", ", userId)));
                throw new ValidationError(dataViolations);
            }

            var contact = ContactService.GetContactInfoDetails(userItem.ContactId);
            var addressres = ContactService.GetUserAddresses(userItem.UserId);

            var user = userItem.User.ToDto<User, UserDetail>();
            return new UserProfileInfoDetail
            {
                ProfileId = userItem.ProfileId,
                UserId = userItem.UserId,
                ContactId = userItem.ContactId,
                User = user,
                Contact = contact,
                Addresses = addressres
            };
        }
        public UserProfileDetail CreateProfile(UserProfileEntry entry)
        {
            var dataViolations = new List<RuleViolation>();
            if (entry == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NullProfileEntry, "UserProfileEntry"));
                throw new ValidationError(dataViolations);
            }
            else
            {
                bool flag = UnitOfWork.UserProfileRepository.HasDataExisted(entry.UserId, entry.ContactId);
                if (flag)
                {
                    return GetProfileDetails(entry.UserId);
                }
                else
                {
                    var entity = new UserProfile
                    {
                        UserId = entry.UserId,
                        ContactId = entry.ContactId
                    };
                    UnitOfWork.UserProfileRepository.Insert(entity);
                    UnitOfWork.SaveChanges();
                    return entity.ToDto<UserProfile, UserProfileDetail>();
                }
            }
        }
        private void EditUserProfile(Guid applicationId, Guid userId, ContactEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            var userProfile = UnitOfWork.UserProfileRepository.GetDetails(userId);
            if (userProfile == null) return;

            if (entry == null) return;
            int contactId = entry.ContactId;

            var contact = UnitOfWork.ContactRepository.FindById(contactId);
            if (contact == null)
            {
                var profile = UnitOfWork.UserProfileRepository.GetDetails(userId, contactId);
                if (profile != null)
                {
                    UnitOfWork.UserProfileRepository.Delete(profile);
                    UnitOfWork.SaveChanges();
                }

                var newContactEntry = new ContactEntry
                {
                    FirstName = entry.FirstName,
                    LastName = entry.LastName,
                    DisplayName = entry.DisplayName,
                    Sex = entry.Sex,
                    JobTitle = entry.JobTitle,
                    Dob = entry.Dob,
                    LinePhone1 = entry.LinePhone1,
                    LinePhone2 = entry.LinePhone2,
                    Mobile = entry.Mobile,
                    Fax = entry.Fax,
                    Email = entry.Email,
                    Website = entry.Website,
                    IdNo = entry.IdNo,
                    IdIssuedDate = entry.IdIssuedDate,
                    TaxNo = entry.TaxNo
                };
                var newContact = ContactService.InsertContact(newContactEntry);
                if (newContact != null)
                {
                    if (entry.FileUpload != null && entry.FileUpload.ContentLength > 0)
                    {
                        int maxContentLength = UnitOfWork.ApplicationSettingRepository.GetAllowedMaxImageContentLength(applicationId);
                        string[] allowedFileExtensions = UnitOfWork.ApplicationSettingRepository.GetAllowedImageExtensions(applicationId);

                        if (!allowedFileExtensions.Contains(
                            entry.FileUpload.FileName.Substring(entry.FileUpload.FileName
                                .LastIndexOf('.'))))
                        {
                            violations.Add(new RuleViolation(ErrorCode.InvalidFileType, "FileUpload",
                                LanguageResource.InvalidFileExtension + " : " +
                                string.Join(", ", allowedFileExtensions)));
                            throw new ValidationError(violations);
                        }
                        else if (entry.FileUpload.ContentLength > maxContentLength)
                        {
                            violations.Add(new RuleViolation(ErrorCode.MaximumAllowedSize, "FileUpload",
                                LanguageResource.InvalidFileSize + " , " + LanguageResource.MaximumAllowedSize +
                                maxContentLength + " MB"));
                            throw new ValidationError(violations);
                        }
                        else
                        {
                            var photoInfo = DocumentService.UploadAndSaveDbByFolderId(applicationId, userId,
                                entry.FileUpload, (int) FileLocation.User, StorageType.Local);
                            if (photoInfo != null)
                            {
                                entry.Photo = photoInfo.FileId;
                                ContactService.UpdateContactPhoto(newContact.ContactId, photoInfo.FileId);
                            }
                        }
                    }

                    var profileEntry = new UserProfileEntry
                    {
                        UserId = userId,
                        ContactId = Convert.ToInt32(contactId)
                    };
                    CreateProfile(profileEntry);
                }
            }
            else
            {
                if (entry.FileUpload != null && entry.FileUpload.ContentLength > 0)
                {
                    int maxContentLength = UnitOfWork.ApplicationSettingRepository.GetAllowedMaxImageContentLength(applicationId);
                    string[] allowedFileExtensions = UnitOfWork.ApplicationSettingRepository.GetAllowedImageExtensions(applicationId);

                    if (!allowedFileExtensions.Contains(
                        entry.FileUpload.FileName.Substring(
                            entry.FileUpload.FileName.LastIndexOf('.'))))
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidFileType, "FileUpload",
                            LanguageResource.InvalidFileExtension + " : " + string.Join(", ", allowedFileExtensions)));
                        throw new ValidationError(violations);
                    }
                    else if (entry.FileUpload.ContentLength > maxContentLength)
                    {
                        violations.Add(new RuleViolation(ErrorCode.MaximumAllowedSize, "FileUpload",
                            LanguageResource.InvalidFileSize + " , " + LanguageResource.MaximumAllowedSize +
                            maxContentLength + " MB"));
                        throw new ValidationError(violations);
                    }
                    else
                    {
                        var photoInfo = DocumentService.UploadAndSaveDbByFolderId(applicationId, userId,
                            entry.FileUpload, (int)FileLocation.User, StorageType.Local);
                        if (photoInfo != null)
                        {
                            entry.Photo = photoInfo.FileId;
                        }
                    }
                }
              
                ContactService.UpdateContact(entry);
            }
        }


        #endregion

        #region User Role

        public IEnumerable<UserRoleDetail> GetUserRoles(Guid userId)
        {
            var lst = UnitOfWork.UserRoleRepository.GetUserRolesByUserId(userId);
            return lst.ToDtos<UserRole, UserRoleDetail>();
        }
        public void AssignRolesForUser(Guid userId, List<Guid> roleIds)
        {
            if (roleIds == null) return;
            using (var tranScope = new TransactionScope())
            {
                foreach (var roleId in roleIds)
                {
                    var userRoleEntry = new UserRoleEntry
                    {
                        UserId = userId,
                        RoleId = roleId,
                        IsDefaultRole = false
                    };
                    CreateUserRole(userRoleEntry);
                }
                tranScope.Complete();
            }
        }
        public void CreateUserRole(UserRoleEntry entry)
        {
            var result = UnitOfWork.UserRoleRepository.HasDataExisted(entry.UserId, entry.RoleId);
            if (result) return;

            var entity = entry.ToEntity<UserRoleEntry, UserRole>();
            UnitOfWork.UserRoleRepository.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void CreateRolesForUser(Guid userId, List<UserRoleCreate> roles)
        {
            var user = UnitOfWork.UserRepository.FindById(userId);
            if (user == null) return;

            if (roles.Any())
            {
                foreach (var role in roles)
                {
                    if (role.IsAllowed != null && role.IsAllowed == true)
                    {
                        var result = UnitOfWork.UserRoleRepository.HasDataExisted(userId, role.RoleId);
                        if (!result)
                        {
                            var entry = new UserRoleEntry
                            {
                                UserId = userId,
                                RoleId = role.RoleId,
                                IsDefaultRole = role.IsDefaultRole,
                                IsTrialUsed = role.IsTrialUsed,
                                EffectiveDate = role.EffectiveDate,
                                ExpiryDate = role.ExpiryDate
                            };
                            CreateUserRole(entry);
                        }
                    }
                }
            }
        }
        public void UpdateUserRole(UserRoleEdit entry)
        {
            var dataViolations = new List<RuleViolation>();
            var item = UnitOfWork.UserRoleRepository.GetDetails(entry.UserId, entry.RoleId);
            if (item == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NotFoundUserRole, "RoleId",
                    $"UserId : {entry.UserId} and RoleId: {entry.RoleId} is invalid"));
                throw new ValidationError(dataViolations);
            }
            else
            {
                item.UserId = entry.UserId;
                item.RoleId = entry.RoleId;
                item.EffectiveDate = entry.EffectiveDate;
                item.ExpiryDate = entry.ExpiryDate;
                item.IsTrialUsed = entry.IsTrialUsed;
                item.IsDefaultRole = entry.IsDefaultRole;

                UnitOfWork.UserRoleRepository.Update(item);
                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteRoleForUser(Guid userId, List<Guid> roleIds)
        {
            if (roleIds.Any())
            {
                foreach (var roleId in roleIds)
                {
                    var userRole = UnitOfWork.UserRoleRepository.GetDetails(userId, roleId);
                    if (userRole != null)
                    {
                        UnitOfWork.UserRoleRepository.Delete(userRole);
                    }
                }
                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteUserRoleByUserId(Guid userId)
        {
            var lst = UnitOfWork.UserRoleRepository.GetUserRolesByUserId(userId);
            if (lst != null)
            {
                foreach (var item in lst)
                {
                    UnitOfWork.UserRoleRepository.Delete(item);
                }
                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteUserRoleByRoleId(Guid roleId)
        {
            var lst = UnitOfWork.UserRoleRepository.GetUsersByRoleId(roleId);
            if (lst != null)
            {
                foreach (var item in lst)
                {
                    UnitOfWork.UserRoleRepository.Delete(item);
                }
                UnitOfWork.SaveChanges();
            }
        }

        private void EditUserRoles(Guid userId, List<UserRoleEdit> selectedRoles)
        {
            if (!selectedRoles.Any()) return;
            var selectedRoleIds = selectedRoles.Where(x => x.IsAllowed == true).Select(x => x.RoleId).ToList();
            var roleIds = UnitOfWork.UserRoleRepository.GetUserRolesByUserId(userId).Select(x => x.RoleId).ToList();
            if (!roleIds.Any())
            {
                //Just insert new roles
                var newRoleIds = selectedRoleIds.Except(roleIds).ToList();
                if (newRoleIds.Any())
                {
                    foreach (var roleId in newRoleIds)
                    {
                        bool isDuplicate = UnitOfWork.UserRoleRepository.HasDataExisted(userId, roleId);
                        if (!isDuplicate)
                        {
                            var userRole = selectedRoles.FirstOrDefault(x => x.RoleId == roleId);
                            if (userRole != null)
                            {
                                CreateUserRole(new UserRoleEntry
                                {
                                    UserId = userRole.UserId,
                                    RoleId = userRole.RoleId,
                                    IsDefaultRole = userRole.IsDefaultRole,
                                    IsTrialUsed = userRole.IsTrialUsed,
                                    EffectiveDate = userRole.EffectiveDate,
                                    ExpiryDate = userRole.ExpiryDate,
                                });
                            }
                        }
                    }
                }
            }
            else
            {
                //Update intersection roles
                var intersectionRoleIds = roleIds.Intersect(selectedRoleIds).ToList();
                if (intersectionRoleIds.Any())
                {
                    foreach (var roleId in intersectionRoleIds)
                    {
                        var userRoleEntry = selectedRoles.FirstOrDefault(x => x.RoleId == roleId);
                        if (userRoleEntry != null)
                        {
                            userRoleEntry.UserId = userId;
                            UpdateUserRole(userRoleEntry);
                        }
                    }
                }

                //Remove unused roles
                var differentRoleIds = roleIds.Except(selectedRoleIds).ToList();
                if (differentRoleIds.Any())
                {
                    DeleteRoleForUser(userId, differentRoleIds);
                }

                //Just insert new roles
                var newRoleIds = selectedRoleIds.Except(roleIds).ToList();
                if (newRoleIds.Any())
                {
                    foreach (var roleId in newRoleIds)
                    {
                        bool isDuplicate = UnitOfWork.UserRoleRepository.HasDataExisted(userId, roleId);
                        if (!isDuplicate)
                        {
                            var userRole = selectedRoles.FirstOrDefault(x => x.RoleId == roleId);
                            if (userRole != null)
                            {
                                CreateUserRole(new UserRoleEntry
                                {
                                    UserId = userRole.UserId,
                                    RoleId = userRole.RoleId,
                                    IsDefaultRole = userRole.IsDefaultRole,
                                    IsTrialUsed = userRole.IsTrialUsed,
                                    EffectiveDate = userRole.EffectiveDate,
                                    ExpiryDate = userRole.ExpiryDate,
                                });
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region User Role Group

        public IEnumerable<UserRoleGroupEdit> GetUserRoleGroups(Guid userId)
        {
            var lst = new List<UserRoleGroupEdit>();
            var userRoleGroups = UnitOfWork.UserRoleGroupRepository.GetUserRoleGroups(userId).ToList();
            if (userRoleGroups.Any())
            {
                lst.AddRange(userRoleGroups.Select(item => new UserRoleGroupEdit
                {
                    UserId = item.UserId,
                    RoleGroupId = item.RoleGroupId,
                    IsDefault = item.IsDefault,
                    EffectiveDate = item.EffectiveDate,
                    ExpiryDate = item.ExpiryDate,
                    IsAllowed = true,
                    RoleGroup = item.RoleGroup.ToDto<RoleGroup, RoleGroupDetail>(),
                    Role = item.Role.ToDto<Role, RoleDetail>(),
                    Group = item.Group.ToDto<Group, GroupDetail>()
                }));
            }
            return lst;
        }


        /// <summary>
        /// Get merged user group by user Id
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="roleId"></param>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public IEnumerable<UserRoleGroupEdit> GeUserRoleGroups(Guid applicationId, Guid roleId, Guid userId, bool? status = null)
        {
            var userRoleGroups = new List<UserRoleGroupEdit>();
            var roleGroups = UnitOfWork.RoleGroupRepository.GetRoleGroupsByRoleId(applicationId, roleId, status).ToList();
            var roleGroupIds = roleGroups.Select(x => x.RoleGroupId).ToList();

            var selectedUserRoleGroups = UnitOfWork.UserRoleGroupRepository.GetUserRoleGroupsByUserId(userId).ToList();
            var selectedRoleGroupIds = selectedUserRoleGroups.Select(x => x.RoleGroupId).ToList();
            var intersectionRoleGroupIds = roleGroupIds.Intersect(selectedRoleGroupIds).ToList();
            var differentRoleGroupIds = roleGroupIds.Except(selectedRoleGroupIds).ToList();
            if (differentRoleGroupIds.Any())
            {
                userRoleGroups.AddRange((from roleGroupId in differentRoleGroupIds
                                         let roleGroupDetail = RoleService.GetRoleGroupDetail(roleGroupId)
                                         select new UserRoleGroupEdit
                                         {
                                             UserId = userId,
                                             RoleGroupId = roleGroupId,
                                             EffectiveDate = DateTime.UtcNow,
                                             ExpiryDate = null,
                                             IsDefault = false,
                                             IsAllowed = false,
                                             RoleGroup = roleGroupDetail,
                                             Role = roleGroupDetail.Role,
                                             Group = roleGroupDetail.Group
                                         }));
            }

            if (intersectionRoleGroupIds.Any())
            {
                userRoleGroups.AddRange(from roleGroupId in intersectionRoleGroupIds
                                        let @group = selectedUserRoleGroups.FirstOrDefault(x => x.RoleGroupId == roleGroupId)
                                        where @group != null
                                        let roleGroupDetail = RoleService.GetRoleGroupDetail(roleGroupId)
                                        select new UserRoleGroupEdit
                                        {
                                            UserId = userId,
                                            RoleGroupId = roleGroupId,
                                            EffectiveDate = @group.EffectiveDate,
                                            ExpiryDate = @group.ExpiryDate,
                                            IsDefault = @group.IsDefault,
                                            IsAllowed = true,
                                            RoleGroup = roleGroupDetail,
                                            Role = roleGroupDetail.Role,
                                            Group = roleGroupDetail.Group
                                        });
            }

            return userRoleGroups;
        }
        public void CreateUserRoleGroup(UserRoleGroupEntry entry)
        {
            var result = UnitOfWork.UserRoleGroupRepository.HasDataExisted(entry.UserId, entry.RoleGroupId);
            if (result) return;

            var entity = new UserRoleGroup
            {
                UserId = entry.UserId,
                RoleGroupId = entry.RoleGroupId,
                IsDefault = entry.IsDefault,
                EffectiveDate = entry.EffectiveDate,
                ExpiryDate = entry.ExpiryDate
            };
            UnitOfWork.UserRoleGroupRepository.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void CreateRoleGroupsForUser(Guid userId, List<UserRoleGroupCreate> groups)
        {
            var user = UnitOfWork.GroupRepository.FindById(userId);
            if (user == null) return;

            if (groups.Any())
            {
                foreach (var group in groups)
                {
                    if (group.IsAllowed != null && group.IsAllowed == true)
                    {
                        var result = UnitOfWork.UserRoleGroupRepository.HasDataExisted(userId, group.RoleGroupId);
                        if (!result)
                        {
                            var entry = new UserRoleGroupEntry
                            {
                                UserId = userId,
                                RoleGroupId = group.RoleGroupId,
                                IsDefault = group.IsDefault,
                                EffectiveDate = group.EffectiveDate,
                                ExpiryDate = group.ExpiryDate
                            };
                            CreateUserRoleGroup(entry);
                        }
                    }
                }
            }
        }
        public void DeleteRoleGroupsForUser(Guid userId, List<int> roleGroupIds)
        {
            if (roleGroupIds.Any())
            {
                foreach (var roleGroupId in roleGroupIds)
                {
                    var userRoleGroup = UnitOfWork.UserRoleGroupRepository.FindById(roleGroupId);
                    if (userRoleGroup != null)
                    {
                        UnitOfWork.UserRoleGroupRepository.Delete(userRoleGroup);
                    }
                }
                UnitOfWork.SaveChanges();
            }
        }
        public void UpdateUserRoleGroup(UserRoleGroupEdit entry)
        {
            var dataViolations = new List<RuleViolation>();
            var item = UnitOfWork.UserRoleGroupRepository.GetDetail(entry.UserId, entry.RoleGroupId);
            if (item == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NotFoundUserRole, "RoleId",
                    $"UserId : {entry.UserId} and RoleGroupId: {entry.RoleGroupId} is invalid"));
                throw new ValidationError(dataViolations);
            }
            else
            {
                item.UserId = entry.UserId;
                item.RoleGroupId = entry.RoleGroupId;
                item.EffectiveDate = entry.EffectiveDate;
                item.ExpiryDate = entry.ExpiryDate;
                item.IsDefault = entry.IsDefault;

                UnitOfWork.UserRoleGroupRepository.Update(item);
                UnitOfWork.SaveChanges();
            }
        }
        private void EditUserRoleGroups(Guid userId, List<UserRoleGroupEdit> lst)
        {
            if (!lst.Any()) return;

            var selectedRoleGroupIds = lst.Where(x => x.IsAllowed).Select(x => x.RoleGroupId).ToList();
            if (selectedRoleGroupIds.Any())
            {
                var groupIds = UnitOfWork.UserRoleGroupRepository.GetUserRoleGroupsByUserId(userId).Select(x => x.RoleGroupId).ToList();
                if (!groupIds.Any())
                {
                    //Just insert new groups
                    var newGroupIds = selectedRoleGroupIds.Except(groupIds).ToList();
                    if (newGroupIds.Any())
                    {
                        foreach (var roleGroupId in newGroupIds)
                        {
                            bool isDuplicate = UnitOfWork.UserRoleGroupRepository.HasDataExisted(userId, roleGroupId);
                            if (!isDuplicate)
                            {
                                var userGroup = lst.FirstOrDefault(x => x.RoleGroupId == roleGroupId);
                                CreateUserRoleGroup(userGroup);
                            }
                        }
                    }
                }
                else
                {
                    //Update intersection groups
                    var intersectionGroupIds = groupIds.Intersect(selectedRoleGroupIds).ToList();
                    if (intersectionGroupIds.Any())
                    {
                        foreach (var roleGroupId in intersectionGroupIds)
                        {
                            var userGroupEntry = lst.FirstOrDefault(x => x.RoleGroupId == roleGroupId);
                            UpdateUserRoleGroup(userGroupEntry);
                        }
                    }

                    //Remove unused groups
                    var differentGroupIds = groupIds.Except(selectedRoleGroupIds).ToList();
                    if (differentGroupIds.Any())
                    {
                        DeleteRoleGroupsForUser(userId, differentGroupIds);
                    }

                    //Just insert new groups
                    var newRoleGroupIds = selectedRoleGroupIds.Except(groupIds).ToList();
                    if (newRoleGroupIds.Any())
                    {
                        foreach (var roleGroupId in newRoleGroupIds)
                        {
                            bool isDuplicate = UnitOfWork.UserRoleGroupRepository.HasDataExisted(userId, roleGroupId);
                            if (!isDuplicate)
                            {
                                var userGroup = lst.FirstOrDefault(x => x.RoleGroupId == roleGroupId);
                                CreateUserRoleGroup(userGroup);
                            }
                        }
                    }
                }
            }
            UnitOfWork.SaveChanges();
        }

        #endregion

        #region User Vendor

        public void InsertUserVendor(Guid userId, int vendorId)
        {
            var entity = new UserVendor
            {
                VendorId = vendorId,
                UserId = userId
            };
            
            UnitOfWork.UserVendorRepository.Insert(entity);
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
                    ApplicationService = null;
                    ContactService = null;
                    CurrencyService = null;
                    DocumentService = null;
                    RoleService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}
