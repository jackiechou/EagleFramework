using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Common.Security.Cryptography;
using Eagle.Core.Common;
using Eagle.Core.Settings;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.SystemManagement
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<UserContact> GetList(Guid applicationId, out int recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var query = from u in DataContext.Get<User>()
                        join p in DataContext.Get<UserProfile>() on u.UserId equals p.UserId into profileInfo
                        from profile in profileInfo.DefaultIfEmpty()
                        join c in DataContext.Get<Contact>() on profile.ContactId equals c.ContactId into contactInfo
                        from contact in contactInfo.DefaultIfEmpty()
                        join a in DataContext.Get<ApplicationEntity>() on u.ApplicationId equals a.ApplicationId into appInfo
                        from app in appInfo.DefaultIfEmpty()
                        where u.ApplicationId == applicationId
                        select new UserContact
                        {
                            ApplicationId = u.ApplicationId,
                            SeqNo = u.SeqNo,
                            UserId = u.UserId,
                            UserName = u.UserName,
                            LoweredUserName = u.LoweredUserName,
                            Password = u.Password,
                            PasswordSalt = u.PasswordSalt,
                            PasswordQuestion = u.PasswordQuestion,
                            PasswordAnswer = u.PasswordAnswer,
                            IsSuperUser = u.IsSuperUser,
                            IsApproved = u.IsApproved,
                            IsLockedOut = u.IsLockedOut,
                            UpdatePassword = u.UpdatePassword,
                            EmailConfirmed = u.EmailConfirmed,
                            LastPasswordChangedDate = u.LastPasswordChangedDate,
                            FailedPasswordAttemptCount = u.FailedPasswordAttemptCount,
                            FailedPasswordAttemptTime = u.FailedPasswordAttemptTime,
                            FailedPasswordAnswerAttemptCount = u.FailedPasswordAnswerAttemptCount,
                            FailedPasswordAnswerAttemptTime = u.FailedPasswordAnswerAttemptTime,
                            StartDate = u.StartDate,
                            ExpiredDate = u.ExpiredDate,
                            LastLoginDate = u.LastLoginDate,
                            LastLockoutDate = u.LastLockoutDate,
                            LastActivityDate = u.LastActivityDate,
                            Ip = u.Ip,
                            LastUpdatedIp = u.LastUpdatedIp,
                            CreatedDate = u.CreatedDate,
                            LastModifiedDate = u.LastModifiedDate,
                            CreatedByUserId = u.CreatedByUserId,
                            LastModifiedByUserId = u.LastModifiedByUserId,
                            Profile = profile
                        };
            return query.WithRecordCount(out recordCount)
                            .WithSortingAndPaging(orderBy, page, pageSize);
        }
        public IEnumerable<User> GetUserOnlines(Guid applicationId, int minutesSinceLastInActive)
        {
            var prevDateTime = DateTime.UtcNow.AddMinutes(minutesSinceLastInActive);
            // var duration = DateExtension.DateDiffFromNow(DateInterval.Minute, prevDateTime);
            var query = from u in DataContext.Get<User>()
                        where u.ApplicationId == applicationId && u.LastActivityDate >= prevDateTime
                        select u;
            return query.AsEnumerable();
        }
        public IEnumerable<UserContact> SearchUsers(Guid applicationId, out int recordCount, Guid? roleId = null, string searchText = null, string email = null, string mobile = null, string phone = null, bool? isApproved = null, bool? isLockedOut = null, bool? isSuperUser = null, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var query = (from u in DataContext.Get<User>()
                         join p in DataContext.Get<UserProfile>() on u.UserId equals p.UserId into profileInfo
                         from profile in profileInfo.DefaultIfEmpty()
                         //join r in DataContext.Get<UserRole>() on u.UserId equals r.UserId into roleInfo
                         //from role in roleInfo.DefaultIfEmpty()
                         //join c in DataContext.Get<Contact>() on profile.ContactId equals c.ContactId into contactInfo
                         //from contact in contactInfo.DefaultIfEmpty()
                         where u.ApplicationId == applicationId
                         && (isApproved == null || u.IsApproved == isApproved)
                         && (isLockedOut == null || u.IsLockedOut == isLockedOut)
                         && (isSuperUser == null || u.IsSuperUser == isSuperUser)
                         select new UserContact
                         {
                             ApplicationId = u.ApplicationId,
                             SeqNo = u.SeqNo,
                             UserId = u.UserId,
                             UserName = u.UserName,
                             LoweredUserName = u.LoweredUserName,
                             Password = u.Password,
                             PasswordSalt = u.PasswordSalt,
                             PasswordQuestion = u.PasswordQuestion,
                             PasswordAnswer = u.PasswordAnswer,
                             IsSuperUser = u.IsSuperUser,
                             IsApproved = u.IsApproved,
                             IsLockedOut = u.IsLockedOut,
                             UpdatePassword = u.UpdatePassword,
                             EmailConfirmed = u.EmailConfirmed,
                             LastPasswordChangedDate = u.LastPasswordChangedDate,
                             FailedPasswordAttemptCount = u.FailedPasswordAttemptCount,
                             FailedPasswordAttemptTime = u.FailedPasswordAttemptTime,
                             FailedPasswordAnswerAttemptCount = u.FailedPasswordAnswerAttemptCount,
                             FailedPasswordAnswerAttemptTime = u.FailedPasswordAnswerAttemptTime,
                             StartDate = u.StartDate,
                             ExpiredDate = u.ExpiredDate,
                             LastLoginDate = u.LastLoginDate,
                             LastLockoutDate = u.LastLockoutDate,
                             LastActivityDate = u.LastActivityDate,
                             Ip = u.Ip,
                             LastUpdatedIp = u.LastUpdatedIp,
                             CreatedDate = u.CreatedDate,
                             LastModifiedDate = u.LastModifiedDate,
                             CreatedByUserId = u.CreatedByUserId,
                             LastModifiedByUserId = u.LastModifiedByUserId,
                             Profile = profile
                         }).Distinct();
            return query.WithRecordCount(out recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }
        public IEnumerable<UserInfo> SearchUsers(Guid applicationId, string name, out int recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var query = (from u in DataContext.Get<User>()
                         join p in DataContext.Get<UserProfile>() on u.UserId equals p.UserId into profileInfo
                         from profile in profileInfo.DefaultIfEmpty()
                         join c in DataContext.Get<Contact>() on profile.ContactId equals c.ContactId into contactInfo
                         from contact in contactInfo.DefaultIfEmpty()
                         join a in DataContext.Get<ApplicationEntity>() on u.ApplicationId equals a.ApplicationId into appInfo
                         from app in appInfo.DefaultIfEmpty()
                         where u.ApplicationId == applicationId
                         && u.IsLockedOut.Value ==false
                         && contact.FirstName.ToLower().Contains(name) || contact.LastName.ToLower().Contains(name) || contact.DisplayName.ToLower().Contains(name)
                         select new UserInfo
                         {
                             Application = app,
                             User = u,
                             Profile = profile,
                             Contact = contact
                         }).Distinct();
            return query.OrderBy(t => t.Contact.FirstName).ThenBy(t => t.Contact.LastName).WithRecordCount(out recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }
        public UserInfo FindByEmail(string email)
        {
            return (from u in DataContext.Get<User>()
                    join p in DataContext.Get<UserProfile>() on u.UserId equals p.UserId into profileInfo
                    from profile in profileInfo.DefaultIfEmpty()
                    join c in DataContext.Get<Contact>() on profile.ContactId equals c.ContactId into contactInfo
                    from contact in contactInfo.DefaultIfEmpty()
                    where contact.Email == email
                    select new UserInfo
                    {
                        User = u,
                        Profile = profile,
                        Contact = contact
                    }).FirstOrDefault();
        }
        public User FindByUserName(string userName)
        {
            return (from u in DataContext.Get<User>()
                    join p in DataContext.Get<UserProfile>() on u.UserId equals p.UserId
                    where u.UserName == userName
                    select u).FirstOrDefault();
        }
        public User FindByUserAndPassword(string userName, string password)
        {
            string hashedPassword = Md5Crypto.GetMd5Hash(password);
            return (from u in DataContext.Get<User>()
                    where u.UserName == userName && u.Password == hashedPassword
                    select u).FirstOrDefault();
        }
        public UserInfo GetDetails(Guid userId)
        {
            var entity = (from u in DataContext.Get<User>()
                          join p in DataContext.Get<UserProfile>() on u.UserId equals p.UserId into profileInfo
                          from profile in profileInfo.DefaultIfEmpty()
                          join c in DataContext.Get<Contact>() on profile.ContactId equals c.ContactId into contactInfo
                          from contact in contactInfo.DefaultIfEmpty()
                          join a in DataContext.Get<ApplicationEntity>() on u.ApplicationId equals a.ApplicationId into appInfo
                          from app in appInfo.DefaultIfEmpty()
                          where u.UserId == userId
                          select new UserInfo
                          {
                             Application = app,
                             User= u,
                             Profile = profile,
                             Contact = contact
                          }).FirstOrDefault();
            return entity;
        }
        public UserInfo GetUserDetails(string userName)
        {
            return (from u in DataContext.Get<User>()
                    join p in DataContext.Get<UserProfile>() on u.UserId equals p.UserId into profileInfo
                    from profile in profileInfo.DefaultIfEmpty()
                    join c in DataContext.Get<Contact>() on profile.ContactId equals c.ContactId into contactInfo
                    from contact in contactInfo.DefaultIfEmpty()
                    join a in DataContext.Get<ApplicationEntity>() on u.ApplicationId equals a.ApplicationId into appInfo
                    from app in appInfo.DefaultIfEmpty()
                    where u.UserName == userName
                    select new UserInfo
                    {
                        Application = app,
                        User = u,
                        Profile = profile,
                        Contact = contact
                    }).FirstOrDefault();
        }
        public UserInfo GetDetails(string userName, string passwordSalt)
        {
            string hashedPassword = Md5Crypto.GetMd5Hash(passwordSalt);
            return (from u in DataContext.Get<User>()
                    join p in DataContext.Get<UserProfile>() on u.UserId equals p.UserId into profileInfo
                    from profile in profileInfo.DefaultIfEmpty()
                    join c in DataContext.Get<Contact>() on profile.ContactId equals c.ContactId into contactInfo
                    from contact in contactInfo.DefaultIfEmpty()
                    join a in DataContext.Get<ApplicationEntity>() on u.ApplicationId equals a.ApplicationId into appInfo
                    from app in appInfo.DefaultIfEmpty()
                    where u.UserName == userName && u.Password == hashedPassword
                    select new UserInfo
                    {
                        Application = app,
                        User = u,
                        Profile = profile,
                        Contact = contact
                    }).FirstOrDefault();
        }
   
        public User GetProfile(Guid userId)
        {
            var entity = (from u in DataContext.Get<User>()
                          join p in DataContext.Get<UserProfile>() on u.UserId equals p.UserId into profileInfo
                          from profile in profileInfo.DefaultIfEmpty()
                          join c in DataContext.Get<Contact>() on profile.ContactId equals c.ContactId into contactInfo
                          from contact in contactInfo.DefaultIfEmpty()
                          join a in DataContext.Get<ApplicationEntity>() on u.ApplicationId equals a.ApplicationId into appInfo
                          from app in appInfo.DefaultIfEmpty()
                          where u.UserId == userId
                          select new User
                          {
                              ApplicationId = u.ApplicationId,
                              SeqNo = u.SeqNo,
                              UserId = u.UserId,
                              UserName = u.UserName,
                              LoweredUserName = u.LoweredUserName,
                              Password = u.Password,
                              PasswordSalt = u.PasswordSalt,
                              PasswordQuestion = u.PasswordQuestion,
                              PasswordAnswer = u.PasswordAnswer,
                              IsSuperUser = u.IsSuperUser,
                              IsApproved = u.IsApproved,
                              IsLockedOut = u.IsLockedOut,
                              UpdatePassword = u.UpdatePassword,
                              EmailConfirmed = u.EmailConfirmed,
                              LastPasswordChangedDate = u.LastPasswordChangedDate,
                              FailedPasswordAttemptCount = u.FailedPasswordAttemptCount,
                              FailedPasswordAttemptTime = u.FailedPasswordAttemptTime,
                              FailedPasswordAnswerAttemptCount = u.FailedPasswordAnswerAttemptCount,
                              FailedPasswordAnswerAttemptTime = u.FailedPasswordAnswerAttemptTime,
                              StartDate = u.StartDate,
                              ExpiredDate = u.ExpiredDate,
                              LastLoginDate = u.LastLoginDate,
                              LastLockoutDate = u.LastLockoutDate,
                              LastActivityDate = u.LastActivityDate,
                              Ip = u.Ip,
                              LastUpdatedIp = u.LastUpdatedIp,
                              CreatedDate = u.CreatedDate,
                              LastModifiedDate = u.LastModifiedDate,
                              CreatedByUserId = u.CreatedByUserId,
                              LastModifiedByUserId = u.LastModifiedByUserId
                          }).FirstOrDefault();
            return entity;
        }
    
        public UserProfile GetUserProfile(Guid userId)
        {
            var entity = (from p in DataContext.Get<UserProfile>()
                          where p.UserId == userId
                          select p).FirstOrDefault();
            return entity;
        }
        public IEnumerable<UserProfile> GetProfiles(List<Guid> userIds)
        {
            if (!userIds.Any()) return null;
            return userIds.Select(GetUserProfile).AsEnumerable();
        }
        public SelectList PopulateQuestionsSelectList(string selectedValue = null, bool? isShowSelectText = false)
        {
            var lst = (from x in QuestionSettings.Questions
                        select new SelectListItem
                        {
                            Text = x,
                            Value = x,
                            Selected = (!string.IsNullOrEmpty(selectedValue)) && x.ToLower().Equals(selectedValue.ToLower())
                        }).ToList();

            if (lst.Any())
            {
                if (isShowSelectText!=null && isShowSelectText==true)
                    lst.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectQuestion} ---", Value = "" });
            }
            else
            {
                lst.Insert(0, new SelectListItem { Text = $"-- {LanguageResource.None} --", Value = "-1" });
            }
            return new SelectList(lst, "Value", "Text", selectedValue);
        }

        public SelectList PopulateSexSelectList(int? selectedValue = null, bool? isShowSelectText = false)
        {
            var lst = (from SexType x in Enum.GetValues(typeof(SexType)).Cast<SexType>()
                        select new SelectListItem
                        {
                            Text = x.ToString(),
                            Value = ((int)x).ToString(),
                            Selected = selectedValue != null && x.Equals(selectedValue)
                        }).ToList();

            if (lst.Any())
            {
                if (isShowSelectText != null && isShowSelectText == true)
                    lst.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectSex} ---", Value = "" });
            }
            else
            {
                lst.Insert(0, new SelectListItem { Text = $"-- {LanguageResource.None} --", Value = "-1" });
            }
            return new SelectList(lst, "Value", "Text", selectedValue);
        }

        public SelectList PopulateTitleSelectList(int? selectedValue = null, bool? isShowSelectText = false)
        {
            var list = (from TitleSetting x in Enum.GetValues(typeof(TitleSetting)).Cast<TitleSetting>()
                        select new SelectListItem
                        {
                            Text = x.ToString(),
                            Value = ((int)x).ToString(),
                            Selected = selectedValue != null && x.Equals(selectedValue)
                        }).ToList();

            if (isShowSelectText != null && isShowSelectText == true)
                list.Insert(0, new SelectListItem() { Value = "", Text = $"-- {LanguageResource.Select} --" });

            return new SelectList(list, "Value", "Text", selectedValue);
        }

        public SelectList PopulateStatusSelectList(int? selectedValue = null, bool? isShowSelectText = false)
        {
            var lst = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = LanguageResource.On,
                    Value = "True"
                },
                new SelectListItem
                {
                    Text = LanguageResource.Off,
                    Value = "False"
                }
            };

            if (isShowSelectText != null && isShowSelectText == true)
                lst.Insert(0, new SelectListItem() { Value = "", Text = $"-- {LanguageResource.SelectStatus} --" });

            return new SelectList(lst, "Value", "Text", selectedValue);
        }

        public string GetPasswordSalt(string email)
        {
            if (string.IsNullOrEmpty(email)) return null;
            return (from u in DataContext.Get<User>()
                    join p in DataContext.Get<UserProfile>() on u.UserId equals p.UserId into profileInfo
                    from profile in profileInfo.DefaultIfEmpty()
                    join c in DataContext.Get<Contact>() on profile.ContactId equals c.ContactId into contactInfo
                    from contact in contactInfo.DefaultIfEmpty()
                    join a in DataContext.Get<ApplicationEntity>() on u.ApplicationId equals a.ApplicationId into appInfo
                    from app in appInfo.DefaultIfEmpty()
                    where contact.Email == email
                    select u.PasswordSalt).FirstOrDefault();
        }
        public string GetPasswordSalt(string email, string passwordQuestion, string passwordAnswer)
        {
            string passwordSalt = string.Empty;
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(passwordQuestion) || string.IsNullOrEmpty(passwordAnswer)) return null;
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(passwordQuestion) && !string.IsNullOrEmpty(passwordAnswer))
                passwordSalt = (from u in DataContext.Get<User>()
                                join p in DataContext.Get<UserProfile>() on u.UserId equals p.UserId into profileInfo
                                from profile in profileInfo.DefaultIfEmpty()
                                join c in DataContext.Get<Contact>() on profile.ContactId equals c.ContactId into contactInfo
                                from contact in contactInfo.DefaultIfEmpty()
                                join a in DataContext.Get<ApplicationEntity>() on u.ApplicationId equals a.ApplicationId into appInfo
                                from app in appInfo.DefaultIfEmpty()
                                where contact.Email == email && u.PasswordQuestion == passwordQuestion && u.PasswordAnswer == passwordAnswer
                                select u.PasswordSalt).FirstOrDefault();
            return passwordSalt;
        }
        public bool IsUsernameExisted(string userName)
        {
            var code = DataContext.Get<User>().FirstOrDefault(p => p.LoweredUserName.Equals(userName.ToLower()));
            return (code != null);
        }
        public bool IsEmailExisted(string email)
        {
            var user = (from u in DataContext.Get<User>()
                        join p in DataContext.Get<UserProfile>() on u.UserId equals p.UserId into profileInfo
                        from profile in profileInfo.DefaultIfEmpty()
                        join c in DataContext.Get<Contact>() on profile.ContactId equals c.ContactId into contactInfo
                        from contact in contactInfo.DefaultIfEmpty()
                        join a in DataContext.Get<ApplicationEntity>() on u.ApplicationId equals a.ApplicationId into appInfo
                        from app in appInfo.DefaultIfEmpty()
                        where contact.Email == email
                        select u).FirstOrDefault();
            return (user != null);
        }
        public bool IsMobileExisted(string mobile)
        {
            var entity = from u in DataContext.Get<User>()
                         join p in DataContext.Get<UserProfile>() on u.UserId equals p.UserId into profileInfo
                         from profile in profileInfo.DefaultIfEmpty()
                         join c in DataContext.Get<Contact>() on profile.ContactId equals c.ContactId into contactInfo
                         from contact in contactInfo.DefaultIfEmpty()
                         join a in DataContext.Get<ApplicationEntity>() on u.ApplicationId equals a.ApplicationId into appInfo
                         from app in appInfo.DefaultIfEmpty()
                         where contact.Mobile == mobile
                         select u;
            return entity.Any();
        }
        public bool IsPhoneExisted(string phone)
        {
            var entity = from u in DataContext.Get<User>()
                         join p in DataContext.Get<UserProfile>() on u.UserId equals p.UserId into profileInfo
                         from profile in profileInfo.DefaultIfEmpty()
                         join c in DataContext.Get<Contact>() on profile.ContactId equals c.ContactId into contactInfo
                         from contact in contactInfo.DefaultIfEmpty()
                         join a in DataContext.Get<ApplicationEntity>() on u.ApplicationId equals a.ApplicationId into appInfo
                         from app in appInfo.DefaultIfEmpty()
                         where contact.Mobile == phone || contact.LinePhone1 == phone || contact.LinePhone2 == phone
                         select u;
            return entity.Any();
        }
        public IEnumerable<User> GetUniqueUser(string firstName, string lastName, DateTime? dateOfBirth, string email)
        {
            return (from u in DataContext.Get<User>()
                                 join p in DataContext.Get<UserProfile>() on u.UserId equals p.UserId into profileInfo
                                 from profile in profileInfo.DefaultIfEmpty()
                                 join c in DataContext.Get<Contact>() on profile.ContactId equals c.ContactId into contactInfo
                                 from contact in contactInfo.DefaultIfEmpty()
                                 join a in DataContext.Get<ApplicationEntity>() on u.ApplicationId equals a.ApplicationId into appInfo
                                 from app in appInfo.DefaultIfEmpty()
                                 where contact.Dob == dateOfBirth
                             && contact.FirstName == firstName && contact.LastName == lastName && contact.Email == email
                                 select u).AsEnumerable();
        }
    }
}
