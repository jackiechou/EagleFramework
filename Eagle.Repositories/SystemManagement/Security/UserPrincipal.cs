using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace Eagle.Repositories.SystemManagement.Security
{
    [Serializable]
    public class UserPrincipal : IUserPrincipal
    {
        private static readonly UserPrincipal unidentified = new UserPrincipal(Security.UserIdentity.Unidentified, null);

        public static UserPrincipal Unidentified { get { return unidentified; } }

        private readonly IUserIdentity identity;
        private readonly List<string> roles = new List<string>();

        public UserPrincipal(IUserIdentity identity, IEnumerable<string> roles)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }

            this.identity = identity;

            if (roles != null)
            {
                this.roles.AddRange(roles);
            }
        }

        #region IUserPrincipal Members

        public IUserIdentity UserIdentity
        {
            get { return this.identity; }
        }

        public IEnumerable<string> Roles
        {
            get
            {
                foreach (string role in this.roles)
                {
                    yield return role;
                }
            }
        }

        #endregion

        #region IPrincipal Members

        public IIdentity Identity
        {
            get { return this.identity; }
        }

        public bool IsInRole(string role)
        {
            return this.roles.Contains(role);
        }

        #endregion
    }
}
