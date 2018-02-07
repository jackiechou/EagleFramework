using System.Collections.Generic;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Services.Message;
using Eagle.Services.Validations;

namespace Eagle.Services.Messaging.Validations
{
   public class MessageQueueEntryValidator : SpecificationBase<MessageQueueEntry>
    {
        private static IUnitOfWork UnitOfWork { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        public MessageQueueEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
        }

        protected override bool IsSatisfyBy(MessageQueueEntry data, IList<RuleViolation> violations = null)
        {
            //ISpecification<MessageQueueEntry> isValidPermission = new IsManagerValidator<MessageQueueEntry>(UnitOfWork, data.UserId, PermissionLevel);
            ISpecification<MessageQueueEntry> isValidDataEntry = new IsValidDataEntry();
            ISpecification<MessageQueueEntry> isValidFrom = new IsValidFrom();
            ISpecification<MessageQueueEntry> isValidTo = new IsValidTo();
            ISpecification<MessageQueueEntry> isValidSubject = new IsValidSubject();
            ISpecification<MessageQueueEntry> isValidBody = new IsValidBody();

            return isValidDataEntry.And(isValidFrom).And(isValidTo).And(isValidSubject).And(isValidBody).IsSatisfyBy(data, violations);
        }
        internal class IsValidDataEntry : SpecificationBase<MessageQueueEntry>
        {
            protected override bool IsSatisfyBy(MessageQueueEntry data, IList<RuleViolation> violations = null)
            {
                if (data == null && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullReferenceForMessageQueueEntry, "MessageQueueEntry"));
                    return false;
                }
                return true;
            }
        }
        internal class IsValidFrom : SpecificationBase<MessageQueueEntry>
        {
            protected override bool IsSatisfyBy(MessageQueueEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.From) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidFrom, "From", data.From));
                    return false;
                }
                return true;
            }
        }

        internal class IsValidTo : SpecificationBase<MessageQueueEntry>
        {
            protected override bool IsSatisfyBy(MessageQueueEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.To) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidTo, "To", data.To));
                    return false;
                }
                return true;
            }
        }

        internal class IsValidSubject : SpecificationBase<MessageQueueEntry>
        {
            protected override bool IsSatisfyBy(MessageQueueEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.Subject) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidSubject, "Subject", data.Subject));
                    return false;
                }
                return true;
            }
        }

        internal class IsValidBody : SpecificationBase<MessageQueueEntry>
        {
            protected override bool IsSatisfyBy(MessageQueueEntry data, IList<RuleViolation> violations = null)
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
