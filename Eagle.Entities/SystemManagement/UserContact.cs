using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Entities.SystemManagement.FileStorage;

namespace Eagle.Entities.SystemManagement
{
    [NotMapped]
    public class UserContact : User
    {
        public UserProfile Profile { get; set; }

        [NotMapped]
        public virtual DocumentInfo DocumentInfo { get; set; }
    }
}
