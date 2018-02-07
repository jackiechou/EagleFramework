using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Eagle.Entities.SystemManagement;
using Eagle.Resources;

namespace Eagle.Services.Dtos.SystemManagement
{
    public class AppClaimEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Key")]
        [StringLength(500, MinimumLength = 2, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MinMaxTitleLength")]
        public string Key { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Value")]
        [StringLength(500, MinimumLength = 2, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MinMaxTitleLength")]
        public string Value { get; set; }

        public Guid GroupId { get; set; }
    }
    public class AppClaimDetail : DtoBase
    {
        public Guid ClaimId { get; set; }

        [StringLength(500, ErrorMessage = "Key cannot be longer than 500 characters.")]
        public string Key { get; set; }

        [StringLength(500, ErrorMessage = "Name cannot be longer than 500 characters.")]
        public string Value { get; set; }

        public Guid GroupId { get; set; }
        public bool IsDeleted { get; set; }

        public IEnumerable<FunctionCommand> FunctionCommands { get; set; } = new HashSet<FunctionCommand>();

        public IEnumerable<Group> RoleGroups { get; set; } = new HashSet<Group>();

    }
    public class FunctionCommand : DtoBase
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(256, ErrorMessage = "Name cannot be longer than 256 characters.")]
        public string Name { get; set; }

        public virtual ICollection<AppClaimDetail> AppClaims { get; set; } = new HashSet<AppClaimDetail>();

        public bool IsDeleted { get; set; }
    }
}
