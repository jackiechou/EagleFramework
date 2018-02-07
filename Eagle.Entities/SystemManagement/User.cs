using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

//using Microsoft.AspNet.Identity;

namespace Eagle.Entities.SystemManagement
{
    [Serializable]
    [Table("User", Schema = "dbo")]
    public class User : BaseEntity //, IUser
    {
        public User()
        {
            UserId = Guid.NewGuid();
            StartDate = DateTime.UtcNow;
        }

        public Guid ApplicationId { get; set; }

        [Key]
        public Guid UserId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SeqNo { get; set; }
        public string UserName { get; set; }
        public string LoweredUserName { get; set; }

        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string PasswordQuestion { get; set; }
        public string PasswordAnswer { get; set; }
        public bool? UpdatePassword { get; set; }
        public bool? EmailConfirmed { get; set; }
        public DateTime? LastPasswordChangedDate { get; set; }
        public int? FailedPasswordAttemptCount { get; set; }
        public DateTime? FailedPasswordAttemptTime { get; set; }
        public  int? FailedPasswordAnswerAttemptCount { get; set; }
        public DateTime? FailedPasswordAnswerAttemptTime { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public bool? IsApproved { get; set; }
        public bool? IsLockedOut { get; set; }
        public bool? IsSuperUser { get; set; }
        public DateTime? LastLockoutDate { get; set; }
        public DateTime? LastActivityDate { get; set; }

        //public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager, string authenticationType)
        //{
        //    // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
        //    var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
        //    // Add custom user claims here
        //    return userIdentity;
        //}

        ///// <summary>
        ///// eager load user role
        ///// </summary>
        //public ICollection<UserRole> UserRoles { get; set; }
    }
}
