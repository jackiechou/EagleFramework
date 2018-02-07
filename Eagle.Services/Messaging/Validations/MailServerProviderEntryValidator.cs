using System.Collections.Generic;
using System.Security.Claims;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Services.Message;
using Eagle.Services.Validations;

namespace Eagle.Services.Messaging.Validations
{
    public class MailServerProviderEntryValidator : SpecificationBase<MailServerProviderEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }

        public MailServerProviderEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel, ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(MailServerProviderEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullMailServerProviderEntry, "MailServerProviderEntry"));
                return false;
            }

            //ISpecification<MailServerProviderEntry> validPermission = new PermissionValidator<MailServerProviderEntry>(CurrentClaimsIdentity, MailServerProvider, PermissionLevel.View);
            ISpecification<MailServerProviderEntry> hasValidMailServerProviderName = new HasValidMailServerProviderName();
            ISpecification<MailServerProviderEntry> hasValidMailServerProtocol = new HasValidMailServerProtocol();
            ISpecification<MailServerProviderEntry> hasValidIncomingMailServerHost = new HasValidIncomingMailServerHost();
            return hasValidMailServerProviderName.And(hasValidMailServerProtocol).And(hasValidIncomingMailServerHost).IsSatisfyBy(data, violations);
        }

        private class HasValidMailServerProviderName : SpecificationBase<MailServerProviderEntry>
        {
            protected override bool IsSatisfyBy(MailServerProviderEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.MailServerProviderName))
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.NullMailServerProviderName, "MailServerProviderName", null, ErrorMessage.Messages[ErrorCode.NullMailServerProviderName]));
                        return false;
                    }
                }
                else
                {
                    if (data.MailServerProviderName.Length > 50)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.InvalidMailServerProviderName, "MailServerProviderName", data.MailServerProviderName, ErrorMessage.Messages[ErrorCode.InvalidMailServerProviderName]));
                            return false;
                        }
                    }
                    else
                    {
                        var isDataDuplicate = UnitOfWork.MailServerProviderRepository.HasDataExisted(data.MailServerProviderName);
                        if (isDataDuplicate)
                        {
                            if (violations != null)
                            {
                                violations.Add(new RuleViolation(ErrorCode.DuplicateMailServerProviderName,
                                    "MailServerProviderName",
                                    data.MailServerProviderName,
                                ErrorMessage.Messages[ErrorCode.DuplicateMailServerProviderName]));
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
        }

        private class HasValidMailServerProtocol : SpecificationBase<MailServerProviderEntry>
        {
            protected override bool IsSatisfyBy(MailServerProviderEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.MailServerProtocol) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullMailServerProtocol, "MailServerProtocol",
                        data.MailServerProtocol, ErrorMessage.Messages[ErrorCode.NullMailServerProtocol]));
                    return false;
                }
                else
                {
                    if (data.MailServerProtocol.Length > 10)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.InvalidMailServerProtocol,
                                "MailServerProtocol", data.MailServerProtocol,
                                ErrorMessage.Messages[ErrorCode.InvalidMailServerProtocol]));
                            return false;
                        }
                    }
                }
                return true;
            }
        }

        private class HasValidIncomingMailServerHost : SpecificationBase<MailServerProviderEntry>
        {
            protected override bool IsSatisfyBy(MailServerProviderEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.IncomingMailServerHost) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullIncomingMailServerHost, "IncomingMailServerHost", data.IncomingMailServerHost, ErrorMessage.Messages[ErrorCode.NullIncomingMailServerHost]));
                    return false;
                }
                else
                {
                    if (data.IncomingMailServerHost.Length > 20)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.InvalidIncomingMailServerHost,
                                "IncomingMailServerHost", data.IncomingMailServerHost,
                                ErrorMessage.Messages[ErrorCode.InvalidIncomingMailServerHost]));
                            return false;
                        }
                    }
                }
                return true;
            }
        }
    }
}
