using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Services.Dtos.SystemManagement.Identity
{
    public class UserProfileDetail : DtoBase
    {
        public int ProfileId { get; set; }
        public Guid UserId { get; set; }
        public int ContactId { get; set; }
    }
    public class UserProfileInfoDetail : UserProfileDetail
    {
        public UserDetail User { get; set; }
        public ContactInfoDetail Contact { get; set; }
        public IEnumerable<UserAddressInfoDetail> Addresses { get; set; }
    }
    public class UserProfileEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ContactId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int ContactId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "UserId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public Guid UserId { get; set; }
    }


}
