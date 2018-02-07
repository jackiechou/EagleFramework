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
    public class MediaAlbumEntryValidator : SpecificationBase<MediaAlbumEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }

        public MediaAlbumEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel,
            ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(MediaAlbumEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundMediaAlbumEntry, "MediaAlbumEntry"));
                return false;
            }

            ISpecification<MediaAlbumEntry> hasValidTypeId = new HasValidTypeId();
            ISpecification<MediaAlbumEntry> hasValidTopicId = new HasValidTopicId();
            ISpecification<MediaAlbumEntry> hasValidName = new HasValidName();
            ISpecification<MediaAlbumEntry> hasValidDescription = new HasValidDescription();
            ISpecification<MediaAlbumEntry> hasValidStatus = new HasValidStatus();

            return hasValidTypeId.And(hasValidTopicId).And(hasValidName).And(hasValidDescription).And(hasValidStatus).IsSatisfyBy(data, violations);
        }

        private class HasValidTypeId : SpecificationBase<MediaAlbumEntry>
        {
            protected override bool IsSatisfyBy(MediaAlbumEntry data, IList<RuleViolation> violations = null)
            {
                if (data.TypeId <= 0 && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullTypeId, "TypeId", null, ErrorMessage.Messages[ErrorCode.NullTypeId]));
                    return false;
                }
                else
                {
                    var entity = UnitOfWork.MediaTypeRepository.FindById(data.TypeId);
                    if (entity == null && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidTypeId, "TypeId", data.TypeId, ErrorMessage.Messages[ErrorCode.InvalidTypeId]));
                        return false;
                    }
                }
                return true;
            }
        }

        private class HasValidTopicId : SpecificationBase<MediaAlbumEntry>
        {
            protected override bool IsSatisfyBy(MediaAlbumEntry data, IList<RuleViolation> violations = null)
            {
                if (data.TopicId <= 0 && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullTopicId, "TopicId", null, ErrorMessage.Messages[ErrorCode.NullTopicId]));
                    return false;
                }
                else
                {
                    var entity = UnitOfWork.MediaTopicRepository.FindById(data.TopicId);
                    if (entity == null && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidTopicId, "TopicId", data.TopicId, ErrorMessage.Messages[ErrorCode.InvalidTopicId]));
                        return false;
                    }
                }
                return true;
            }
        }

        private class HasValidName : SpecificationBase<MediaAlbumEntry>
        {
            protected override bool IsSatisfyBy(MediaAlbumEntry data, IList<RuleViolation> violations = null)
            {
                var result = !string.IsNullOrEmpty(data.AlbumName) && data.AlbumName.Length > 250;
                if (result && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidName, "AlbumName", data.AlbumName.Length));
                    return false;
                }
                return true;
            }
        }


        private class HasValidDescription : SpecificationBase<MediaAlbumEntry>
        {
            protected override bool IsSatisfyBy(MediaAlbumEntry data, IList<RuleViolation> violations = null)
            {
                var result = !string.IsNullOrEmpty(data.Description) && data.Description.Length > 500;
                if (result && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidDescription, "Description", data.Description.Length));
                    return false;
                }
                return true;
            }
        }

        private class HasValidStatus : SpecificationBase<MediaAlbumEntry>
        {
            protected override bool IsSatisfyBy(MediaAlbumEntry data, IList<RuleViolation> violations = null)
            {
                if (!Enum.IsDefined(typeof(MediaAlbumStatus), data.Status))
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "MediaAlbumStatus", data.Status));
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
