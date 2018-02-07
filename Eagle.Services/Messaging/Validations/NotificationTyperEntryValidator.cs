using System;
using System.Collections.Generic;
using System.Security.Claims;
using Eagle.Core.Permission;
using Eagle.Core.Settings;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Services.Message;
using Eagle.Services.Validations;

namespace Eagle.Services.Messaging.Validations
{
    public class NotificationTypeEntryValidator : SpecificationBase<NotificationTypeEntry>
    {
        private static IUnitOfWork UnitOfWork { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }

        public NotificationTypeEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel, ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(NotificationTypeEntry data, IList<RuleViolation> violations = null)
        {
            //ISpecification<MessageCategoryEntry> hasValidPermission = new PermissionValidator<MessageCategoryEntry>(CurrentClaimsIdentity, PermissionCapability.Mail, PermissionLevel.Create);
            ISpecification<NotificationTypeEntry> hasValidParentId = new HasValidParentId();
            ISpecification<NotificationTypeEntry> hasValidName = new HasValidName();

            return hasValidParentId.And(hasValidName).IsSatisfyBy(data, violations);
        }
        
        internal class HasValidParentId : SpecificationBase<NotificationTypeEntry>
        {
            protected override bool IsSatisfyBy(NotificationTypeEntry data, IList<RuleViolation> violations = null)
            {
                if (data.ParentId != null && data.ParentId > 0 && data.ParentId <= int.MaxValue)
                {
                    var entityByParentId = UnitOfWork.NotificationTypeRepository.FindById(data.ParentId);
                    if (entityByParentId == null && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.NullReferenceForParentId, "ParentId", null,
                        ErrorMessage.Messages[ErrorCode.NullReferenceForParentId]));
                        return false;
                    }
                }
                return true;
            }
        }

        internal class HasValidName : SpecificationBase<NotificationTypeEntry>
        {
            protected override bool IsSatisfyBy(NotificationTypeEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.NotificationTypeName) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullNotificationTypeName, "NotificationTypeName", null,
                        ErrorMessage.Messages[ErrorCode.NullNotificationTypeName]));
                    return false;
                }
                else
                {
                    if (data.NotificationTypeName.Length > 250 && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.NullCategoryName, "NotificationTypeName", data.NotificationTypeName,
                            ErrorMessage.Messages[ErrorCode.NullCategoryName]));
                        return false;
                    }
                    else
                    {
                        var isExisted = UnitOfWork.NotificationTypeRepository.HasDataExisted(data.NotificationTypeName, data.ParentId);
                        if (isExisted && violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicateName, "NotificationTypeName", data.NotificationTypeName,
                        ErrorMessage.Messages[ErrorCode.DuplicateName]));
                            return false;
                        }

                    }
                }
                return true;
            }
        }
    }
}
