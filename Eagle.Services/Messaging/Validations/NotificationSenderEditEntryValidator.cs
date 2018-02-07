using System.Collections.Generic;
using System.Security.Claims;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Services.Message;
using Eagle.Services.Validations;

namespace Eagle.Services.Messaging.Validations
{
    public class NotificationSenderEditEntryValidator : SpecificationBase<NotificationSenderEditEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }

        public NotificationSenderEditEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel, ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(NotificationSenderEditEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullMailAccountEditEntry, "MailAccountEditEntry", null, ErrorMessage.Messages[ErrorCode.NullMailAccountEditEntry]));
                return false;
            }

            //ISpecification<MailAccountEditEntry> validPermission = new PermissionValidator<MailAccountEditEntry>(CurrentClaimsIdentity, MailServerProvider, PermissionLevel.View);
            ISpecification<NotificationSenderEditEntry> hasValidMailServerProviderId = new HasValidMailServerProviderId();
            ISpecification<NotificationSenderEditEntry> hasValidSenderName = new HasValidSenderName();
            ISpecification<NotificationSenderEditEntry> hasValidMailAddress = new HasValidMailAddress();
            ISpecification<NotificationSenderEditEntry> hasValidPassword = new HasValidPassword();

            return hasValidMailServerProviderId.And(hasValidSenderName).And(hasValidMailAddress)
                .And(hasValidPassword).And(hasValidPassword).IsSatisfyBy(data, violations);
        }

        private class HasValidMailServerProviderId : SpecificationBase<NotificationSenderEditEntry>
        {
            protected override bool IsSatisfyBy(NotificationSenderEditEntry data, IList<RuleViolation> violations = null)
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

        private class HasValidSenderName : SpecificationBase<NotificationSenderEditEntry>
        {
            protected override bool IsSatisfyBy(NotificationSenderEditEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.SenderName))
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.NullSenderName, "SenderName", null, ErrorMessage.Messages[ErrorCode.NullSenderName]));
                        return false;
                    }
                }
                else
                {
                    if (data.SenderName.Length > 50)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.InvalidSenderName, "SenderName", data.SenderName, ErrorMessage.Messages[ErrorCode.InvalidSenderName]));
                            return false;
                        }
                    }
                }
                return true;
            }
        }
        private class HasValidMailAddress : SpecificationBase<NotificationSenderEditEntry>
        {
            protected override bool IsSatisfyBy(NotificationSenderEditEntry data, IList<RuleViolation> violations = null)
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
                }
                return true;
            }
        }
        private class HasValidPassword : SpecificationBase<NotificationSenderEditEntry>
        {
            protected override bool IsSatisfyBy(NotificationSenderEditEntry data, IList<RuleViolation> violations = null)
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
