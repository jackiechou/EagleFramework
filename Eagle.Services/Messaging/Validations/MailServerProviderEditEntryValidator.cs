using System.Collections.Generic;
using System.Security.Claims;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Services.Message;
using Eagle.Services.Validations;

namespace Eagle.Services.Messaging.Validations
{
     public class MailServerProviderEditEntryValidator : SpecificationBase<MailServerProviderEditEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }

        public MailServerProviderEditEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel, ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(MailServerProviderEditEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullMailServerProviderEditEntry, "MailServerProviderEditEntry", null, ErrorMessage.Messages[ErrorCode.NullMailServerProviderEditEntry]));
                return false;
            }

            //ISpecification<MailServerProviderEditEntry> validPermission = new PermissionValidator<MailServerProviderEditEntry>(CurrentClaimsIdentity, MailServerProvider, PermissionLevel.View);
            ISpecification<MailServerProviderEditEntry> hasValidMailServerProviderName = new HasValidMailServerProviderName();
            ISpecification<MailServerProviderEditEntry> hasValidMailServerProtocol = new HasValidMailServerProtocol();
            ISpecification<MailServerProviderEditEntry> hasValidIncomingMailServerHost = new HasValidIncomingMailServerHost();
            ISpecification<MailServerProviderEditEntry> hasValidIncomingMailServerPortt = new HasValidIncomingMailServerPort();
            return hasValidMailServerProviderName.And(hasValidMailServerProtocol).And(hasValidIncomingMailServerHost)
                .And(hasValidIncomingMailServerPortt).IsSatisfyBy(data, violations);
        }

        private class HasValidMailServerProviderName : SpecificationBase<MailServerProviderEditEntry>
        {
            protected override bool IsSatisfyBy(MailServerProviderEditEntry data, IList<RuleViolation> violations = null)
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
                }
                return true;
            }
        }

        private class HasValidMailServerProtocol : SpecificationBase<MailServerProviderEditEntry>
        {
            protected override bool IsSatisfyBy(MailServerProviderEditEntry data, IList<RuleViolation> violations = null)
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

        private class HasValidIncomingMailServerHost : SpecificationBase<MailServerProviderEditEntry>
        {
            protected override bool IsSatisfyBy(MailServerProviderEditEntry data, IList<RuleViolation> violations = null)
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

        private class HasValidIncomingMailServerPort : SpecificationBase<MailServerProviderEditEntry>
        {
            protected override bool IsSatisfyBy(MailServerProviderEditEntry data, IList<RuleViolation> violations = null)
            {
                if (data.IncomingMailServerHost.Length <= 0 && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidIncomingMailServerPort, "IncomingMailServerPort", data.IncomingMailServerPort, ErrorMessage.Messages[ErrorCode.InvalidIncomingMailServerPort]));
                    return false;
                }
                return true;
            }
        }
    }
}
