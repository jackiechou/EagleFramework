using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Services.Message;
using Eagle.Services.Validations;

namespace Eagle.Services.Messaging.Validations
{
    public class MessageQueueEditEntryValidator : SpecificationBase<MessageQueueEditEntry>
    {
        private static IUnitOfWork UnitOfWork { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        public MessageQueueEditEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
        }

        protected override bool IsSatisfyBy(MessageQueueEditEntry data, IList<RuleViolation> violations = null)
        {
            //ISpecification<MessageQueueEditEntry> isValidPermission = new IsManagerValidator<MessageQueueEditEntry>(UnitOfWork, data.UserId, PermissionLevel);
            ISpecification<MessageQueueEditEntry> isValidDataEntry = new IsValidDataEntry();
            ISpecification<MessageQueueEditEntry> isValidFrom = new IsValidFrom();
            ISpecification<MessageQueueEditEntry> isValidTo = new IsValidTo();
            ISpecification<MessageQueueEditEntry> isValidSubject = new IsValidSubject();
            ISpecification<MessageQueueEditEntry> isValidBody = new IsValidBody();

            return isValidDataEntry.And(isValidFrom).And(isValidTo).And(isValidSubject).And(isValidBody).IsSatisfyBy(data, violations);
        }
        internal class IsValidDataEntry : SpecificationBase<MessageQueueEditEntry>
        {
            protected override bool IsSatisfyBy(MessageQueueEditEntry data, IList<RuleViolation> violations = null)
            {
                if (data == null && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullMessageQueueEditEntry, "MessageQueueEditEntry"));
                    return false;
                }
                return true;
            }
        }
        internal class IsValidFrom : SpecificationBase<MessageQueueEditEntry>
        {
            protected override bool IsSatisfyBy(MessageQueueEditEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.From) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidFrom,"From", data.From));
                    return false;
                }
                return true;
            }
        }

        internal class IsValidTo : SpecificationBase<MessageQueueEditEntry>
        {
            protected override bool IsSatisfyBy(MessageQueueEditEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.To) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidTo, "To", data.To));
                    return false;
                }
                return true;
            }
        }

        internal class IsValidSubject : SpecificationBase<MessageQueueEditEntry>
        {
            protected override bool IsSatisfyBy(MessageQueueEditEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.Subject) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidSubject, "Subject", data.Subject));
                    return false;
                }
                return true;
            }
        }

        internal class IsValidBody : SpecificationBase<MessageQueueEditEntry>
        {
            protected override bool IsSatisfyBy(MessageQueueEditEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.Body) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidBody, "Body", data.Body));
                    return false;
                }
                return true;
            }
        }
    }
}
