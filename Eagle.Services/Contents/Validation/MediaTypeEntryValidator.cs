using System;
using System.Collections.Generic;
using System.Security.Claims;
using Eagle.Core.Permission;
using Eagle.Core.Settings;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Contents.Media;
using Eagle.Services.Validations;

namespace Eagle.Services.Contents.Validation
{
    public class MediaTypeEntryValidator : SpecificationBase<MediaTypeEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }
        public MediaTypeEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel, ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }
        protected override bool IsSatisfyBy(MediaTypeEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundMediaTypeEntry, "MediaTypeEntry"));
                return false;
            }

            ISpecification<MediaTypeEntry> isValidName = new IsValidName();
            ISpecification<MediaTypeEntry> isValidDescription = new IsValidDescription();
            ISpecification<MediaTypeEntry> isValidTypeExtension = new IsValidTypeExtension();
            ISpecification<MediaTypeEntry> isValidTypePath = new IsValidTypePath();
            ISpecification<MediaTypeEntry> isValidStatus = new IsValidStatus();

            return isValidName.And(isValidTypeExtension).And(isValidTypePath).And(isValidDescription).And(isValidStatus).IsSatisfyBy(data, violations);
        }

        private class IsValidName : SpecificationBase<MediaTypeEntry>
        {
            protected override bool IsSatisfyBy(MediaTypeEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.TypeName))
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.NullTypeName, "TypeName", data.TypeName, ErrorMessage.Messages[ErrorCode.InvalidName]));
                        return false;
                    }
                }
                else
                {
                    if (data.TypeName.Length > 250)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.InvalidName, "TypeName", data.TypeName,
                                ErrorMessage.Messages[ErrorCode.InvalidName]));
                            return false;
                        }
                    }
                    else
                    {
                        var isDataDuplicate = UnitOfWork.MediaTypeRepository.HasDataExisted(data.TypeName);
                        if (isDataDuplicate)
                        {
                            if (violations != null)
                            {
                                violations.Add(new RuleViolation(ErrorCode.DuplicateTypeName, "MediaTypeEntry",
                                    data.TypeName, ErrorMessage.Messages[ErrorCode.DuplicateTypeName]));
                                return false;
                            }
                        }
                    }
                
                }
                return true;
            }
        }

        private class IsValidTypeExtension : SpecificationBase<MediaTypeEntry>
        {
            protected override bool IsSatisfyBy(MediaTypeEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.TypeExtension))
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.NullTypeExtension, "TypeExtension", data.TypeExtension, ErrorMessage.Messages[ErrorCode.InvalidName]));
                        return false;
                    }
                }
                else
                {
                    if (data.TypeExtension.Length > 100)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.InvalidExtension, "TypeExtension", data.TypeExtension,
                                ErrorMessage.Messages[ErrorCode.InvalidExtension]));
                            return false;
                        }
                    }
                }
                return true;
            }
        }


        private class IsValidTypePath : SpecificationBase<MediaTypeEntry>
        {
            protected override bool IsSatisfyBy(MediaTypeEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.TypePath))
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.NullTypePath, "TypePath", data.TypePath, ErrorMessage.Messages[ErrorCode.InvalidName]));
                        return false;
                    }
                }
                else
                {
                    if (data.TypePath.Length > 100)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.InvalidTypePath, "TypePath", data.TypePath,
                                ErrorMessage.Messages[ErrorCode.InvalidTypePath]));
                            return false;
                        }
                    }
                }
                return true;
            }
        }

        private class IsValidDescription : SpecificationBase<MediaTypeEntry>
        {
            protected override bool IsSatisfyBy(MediaTypeEntry data, IList<RuleViolation> violations = null)
            {
                if (!string.IsNullOrEmpty(data.Description) && (data.Description.Length > 500))
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidDescription, "Description", data.Description,
                            ErrorMessage.Messages[ErrorCode.InvalidDescription]));
                        return false;
                    }
                }
                return true;
            }
        }

        private class IsValidStatus : SpecificationBase<MediaTypeEntry>
        {
            protected override bool IsSatisfyBy(MediaTypeEntry data, IList<RuleViolation> violations = null)
            {
                if (!Enum.IsDefined(typeof(MediaTypeStatus), data.Status))
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "MediaTypeStatus"));
                        return false;
                    }
                }
                return true;
            }
        }

    }
}
