using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.SystemManagement
{
    [NotMapped]
    public class UserProfileInfo : UserProfile
    {
        public virtual User User { get; set; }
        public virtual Contact Contact { get; set; }
        public virtual ICollection<UserAddress> Addresses { get; set; }
    }
}
