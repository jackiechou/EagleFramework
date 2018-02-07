using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Eagle.Core.Permission;
using Eagle.Core.Settings;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Contents.Media;
using Eagle.Services.Validations;

namespace Eagle.Services.Contents.Validation
{
    public class MediaPlayListEntryValidator : SpecificationBase<MediaPlayListEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }

        public MediaPlayListEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel,
            ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(MediaPlayListEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundMediaPlayListEntry, "MediaPlayListEntry",null, ErrorMessage.Messages[ErrorCode.NotFoundMediaPlayListEntry]));
                return false;
            }

            ISpecification<MediaPlayListEntry> isValidName = new IsValidName();
            ISpecification<MediaPlayListEntry> isValidDescription = new IsValidDescription();
            ISpecification<MediaPlayListEntry> isValidStatus = new IsValidStatus();

            return
                isValidName.And(isValidDescription)
                    .And(isValidStatus)
                    .IsSatisfyBy(data, violations);
        }

        private class IsValidName : SpecificationBase<MediaPlayListEntry>
        {
            protected override bool IsSatisfyBy(MediaPlayListEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.PlayListName))
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.NullPlayListName, "PlayListName",data.PlayListName, ErrorMessage.Messages[ErrorCode.NullPlayListName]));
                        return false;
                    }
                }
                else
                {
                    if (data.PlayListName.Length > 250)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.InvalidName, "TypeName"));
                            return false;
                        }
                    }
                    else
                    {
                        var isDataDuplicate = UnitOfWork.MediaPlayListRepository.HasDataExisted(data.PlayListName);
                        if (isDataDuplicate)
                        {
                            if (violations != null)
                            {
                                violations.Add(new RuleViolation(ErrorCode.DuplicatePlayListName, "PlayListName", data.PlayListName, ErrorMessage.Messages[ErrorCode.DuplicatePlayListName]));
                                return false;
                            }
                        }
                    }
                }
               
                return true;
            }
        }

        private class IsValidDescription : SpecificationBase<MediaPlayListEntry>
        {
            protected override bool IsSatisfyBy(MediaPlayListEntry data, IList<RuleViolation> violations = null)
            {
                var result = !string.IsNullOrEmpty(data.Description) && data.Description.Length > 500;
                if (result && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidDescription, "Description", data.Description.Length > 500, ErrorMessage.Messages[ErrorCode.InvalidDescription]));
                    return false;
                }
                return true;
            }
        }

        private class IsValidStatus : SpecificationBase<MediaPlayListEntry>
        {
            protected override bool IsSatisfyBy(MediaPlayListEntry data, IList<RuleViolation> violations = null)
            {
                if (!Enum.IsDefined(typeof (MediaPlayListStatus), data.Status))
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "MediaPlayListStatus"));
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
