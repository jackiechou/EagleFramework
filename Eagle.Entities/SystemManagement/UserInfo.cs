using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.SystemManagement
{
    [NotMapped]
    public class UserInfo : EntityBase
    {
        public User User { get; set; }
        public ApplicationEntity Application { get; set; }
        public UserProfile Profile { get; set; }
        public Contact Contact { get; set; }
    }
}
