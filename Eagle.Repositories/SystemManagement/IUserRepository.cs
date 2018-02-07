using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.SystemManagement
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        IEnumerable<UserContact> GetList(Guid applicationId, out int recordCount, string orderBy = null, int? page = null,
            int? pageSize = null);

        IEnumerable<User> GetUserOnlines(Guid applicationId, int minutesSinceLastInActive);

        IEnumerable<UserContact> SearchUsers(Guid applicationId, out int recordCount, Guid? roleId = null, string searchText = null, string email = null, string mobile = null, string phone = null, bool? isApproved = null, bool? isLockedOut = null, bool? isSuperUser = null, string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<UserInfo> SearchUsers(Guid applicationId, string name, out int recordCount, string orderBy = null,
            int? page = null, int? pageSize = null);
        User FindByUserName(string userName);
        UserInfo FindByEmail(string email);
        User FindByUserAndPassword(string userName, string password);
        UserInfo GetDetails(Guid userId);
        UserInfo GetUserDetails(string userName);
        UserInfo GetDetails(string userName, string passwordSalt);
        User GetProfile(Guid id);
        IEnumerable<UserProfile> GetProfiles(List<Guid> userIds);
        SelectList PopulateQuestionsSelectList(string selectedValue = null, bool? isShowSelectText = false);
        SelectList PopulateSexSelectList(int? selectedValue = null, bool? isShowSelectText = false);
        SelectList PopulateTitleSelectList(int? selectedValue = null, bool? isShowSelectText = false);
        SelectList PopulateStatusSelectList(int? selectedValue = null, bool? isShowSelectText = false);
        string GetPasswordSalt(string email);
        string GetPasswordSalt(string email, string passwordQuestion, string passwordAnswer);
        bool IsUsernameExisted(string userName);
        bool IsEmailExisted(string email);
        bool IsMobileExisted(string mobile);
        bool IsPhoneExisted(string phone);
        IEnumerable<User> GetUniqueUser(string firstName, string lastName, DateTime? dateOfBirth, string email);
    }
}
