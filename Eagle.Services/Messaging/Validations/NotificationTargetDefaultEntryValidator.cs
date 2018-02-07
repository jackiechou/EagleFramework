using System.Collections.Generic;
using System.Security.Claims;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Services.Message;
using Eagle.Services.Validations;

namespace Eagle.Services.Messaging.Validations
{
    public class NotificationTargetDefaultEntryValidator : SpecificationBase<NotificationTargetDefaultEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }

        public NotificationTargetDefaultEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel, ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(NotificationTargetDefaultEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullMailAccountEntry, "MailAccountEntry", null, ErrorMessage.Messages[ErrorCode.NullMailAccountEntry]));
                return false;
            }

            //ISpecification<MailAccountEntry> validPermission = new PermissionValidator<MailAccountEntry>(CurrentClaimsIdentity, MailServerProvider, PermissionLevel.View);
            ISpecification<NotificationTargetDefaultEntry> hasValidMailServerProviderId = new HasValidMailServerProviderId();
            ISpecification<NotificationTargetDefaultEntry> hasValidTargetName = new HasValidTargetName();
            ISpecification<NotificationTargetDefaultEntry> hasValidMailAddress = new HasValidMailAddress();
            ISpecification<NotificationTargetDefaultEntry> hasValidPassword = new HasValidPassword();

            return hasValidMailServerProviderId.And(hasValidTargetName).And(hasValidMailAddress)
                .And(hasValidPassword).And(hasValidPassword).IsSatisfyBy(data, violations);
        }

        private class HasValidMailServerProviderId : SpecificationBase<NotificationTargetDefaultEntry>
        {
            protected override bool IsSatisfyBy(NotificationTargetDefaultEntry data, IList<RuleViolation> violations = null)
            {
                var provider = UnitOfWork.MailServerProviderRepository.FindById(data.MailServerProviderId);
                if (provider == null && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidMailServerProviderId, "MailServerProviderId", data.MailServerProviderId, ErrorMessage.Messages[ErrorCode.InvalidMailServerProviderId]));
                    return false;
                }
                return true;
            }
        }

        private class HasValidTargetName : SpecificationBase<NotificationTargetDefaultEntry>
        {
            protected override bool IsSatisfyBy(NotificationTargetDefaultEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.TargetName))
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.NullTargetName, "TargetName", null, ErrorMessage.Messages[ErrorCode.NullTargetName]));
                        return false;
                    }
                }
                else
                {
                    if (data.TargetName.Length > 50)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.InvalidTargetName, "TargetName", data.TargetName, ErrorMessage.Messages[ErrorCode.InvalidTargetName]));
                            return false;
                        }
                    }
                    else
                    {
                        var isExisted = UnitOfWork.NotificationTargetDefaultRepository.HasTargetNameExisted(data.TargetName);
                        if (isExisted && violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicateTargetName, "TargetName", data.TargetName,
                       ErrorMessage.Messages[ErrorCode.DuplicateTargetName]));
                            return false;
                        }
                    }
                }
                return true;
            }
        }
        private class HasValidMailAddress : SpecificationBase<NotificationTargetDefaultEntry>
        {
            protected override bool IsSatisfyBy(NotificationTargetDefaultEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.MailAddress) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullMailAddress, "MailAddress", data.MailAddress, ErrorMessage.Messages[ErrorCode.NullMailAddress]));
                    return false;
                }
                else
                {
                    if (data.MailAddress.Length > 200 && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidMailAddress,
                                 "MailAddress", data.MailAddress,
                                 ErrorMessage.Messages[ErrorCode.InvalidMailAddress]));
                        return false;
                    }
                    else
                    {
                        var isExisted = UnitOfWork.NotificationTargetDefaultRepository.HasTargetNameExisted(data.MailAddress);
                        if (isExisted && violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicateMailAddress, "MailAddress", data.MailAddress,
                       ErrorMessage.Messages[ErrorCode.DuplicateMailAddress]));
                            return false;
                        }
                    }
                }
                return true;
            }
        }
        private class HasValidPassword : SpecificationBase<NotificationTargetDefaultEntry>
        {
            protected override bool IsSatisfyBy(NotificationTargetDefaultEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.Password) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullPassword, "Password",
                        data.Password, ErrorMessage.Messages[ErrorCode.NullPassword]));
                    return false;
                }
                else
                {
                    if (data.Password.Length > 20)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.InvalidPassword,
                                "Password", data.Password,
                                ErrorMessage.Messages[ErrorCode.InvalidPassword]));
                            return false;
                        }
                    }
                }
                return true;
            }
        }
    }
}
