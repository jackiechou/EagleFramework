using System.Collections.Generic;
using System.Security.Claims;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Services.Message;
using Eagle.Services.Validations;

namespace Eagle.Services.Messaging.Validations
{
    public class NotificationSenderEntryValidator : SpecificationBase<NotificationSenderEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }

        public NotificationSenderEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel, ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(NotificationSenderEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullMailAccountEntry, "MailAccountEntry", null, ErrorMessage.Messages[ErrorCode.NullMailAccountEntry]));
                return false;
            }

            //ISpecification<MailAccountEntry> validPermission = new PermissionValidator<MailAccountEntry>(CurrentClaimsIdentity, MailServerProvider, PermissionLevel.View);
            ISpecification<NotificationSenderEntry> hasValidMailServerProviderId = new HasValidMailServerProviderId();
            ISpecification<NotificationSenderEntry> hasValidSenderName = new HasValidSenderName();
            ISpecification<NotificationSenderEntry> hasValidMailAddress = new HasValidMailAddress();
            ISpecification<NotificationSenderEntry> hasValidPassword = new HasValidPassword();

            return hasValidMailServerProviderId.And(hasValidSenderName).And(hasValidMailAddress)
                .And(hasValidPassword).And(hasValidPassword).IsSatisfyBy(data, violations);
        }

        private class HasValidMailServerProviderId : SpecificationBase<NotificationSenderEntry>
        {
            protected override bool IsSatisfyBy(NotificationSenderEntry data, IList<RuleViolation> violations = null)
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

        private class HasValidSenderName : SpecificationBase<NotificationSenderEntry>
        {
            protected override bool IsSatisfyBy(NotificationSenderEntry data, IList<RuleViolation> violations = null)
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
                    else
                    {
                        var isExisted = UnitOfWork.NotificationSenderRepository.HasSenderNameExisted(data.SenderName);
                        if (isExisted && violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicateSenderName, "SenderName", data.SenderName,
                       ErrorMessage.Messages[ErrorCode.DuplicateSenderName]));
                            return false;
                        }
                    }
                }
                return true;
            }
        }
        private class HasValidMailAddress : SpecificationBase<NotificationSenderEntry>
        {
            protected override bool IsSatisfyBy(NotificationSenderEntry data, IList<RuleViolation> violations = null)
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
                        var isExisted = UnitOfWork.NotificationSenderRepository.HasSenderNameExisted(data.MailAddress);
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
        private class HasValidPassword : SpecificationBase<NotificationSenderEntry>
        {
            protected override bool IsSatisfyBy(NotificationSenderEntry data, IList<RuleViolation> violations = null)
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
