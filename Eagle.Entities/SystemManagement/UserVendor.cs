using System;
using System.ComponentModel.DataAnnotations;

namespace Eagle.Entities.SystemManagement
{
    public class UserVendor: EntityBase
    {
        [Key]
        public Guid UserId { get; set; }

        [Key]
        public int VendorId { get; set; }
    }
}
