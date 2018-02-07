using System;

namespace Eagle.Entities.SystemManagement
{
    /// <summary>
    /// represent class for user claim
    /// </summary>
    public class UserClaim : EntityBase
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
