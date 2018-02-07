using System;

namespace Eagle.Repositories.SystemManagement.Security
{
    [Serializable]
    public class UserIdentity : IUserIdentity
    {
        private static UserIdentity unidentified = new UserIdentity(-1, "Unidentified", "Unidentified", String.Empty, false);

        public static UserIdentity Unidentified { get { return unidentified; } }

        public UserIdentity(int id, string name, string fullName, string authenticationType, bool isAuthenticated)
        {
            this.Id = id;
            this.Name = name;
            this.FullName = fullName;
            this.AuthenticationType = authenticationType;
            this.IsAuthenticated = isAuthenticated;
        }

        #region IIdentity Members

        public string AuthenticationType { get; private set; }

        public bool IsAuthenticated { get; private set; }

        public string Name { get; private set; }

        #endregion

        #region IUserIdentity Members

        public int Id { get; set; }

        public string FullName { get; set; }

        #endregion
    }
}
