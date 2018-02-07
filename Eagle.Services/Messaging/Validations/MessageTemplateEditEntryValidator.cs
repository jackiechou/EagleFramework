using System.Collections.Generic;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Services.Message;
using Eagle.Services.Validations;

namespace Eagle.Services.Messaging.Validations
{
    public class MessageTemplateEditEntryValidator : SpecificationBase<MessageTemplateEditEntry>
    {
        private static IUnitOfWork UnitOfWork { get; set; }

        public MessageTemplateEditEntryValidator(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        protected override bool IsSatisfyBy(MessageTemplateEditEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullMessageTemplateEditEntry, "MessageTemplateEditEntry", null,
                    ErrorMessage.Messages[ErrorCode.NullMessageTemplateEditEntry]));
                return false;
            }

            ISpecification<MessageTemplateEditEntry> hasValidCategoryId = new HasValidCategoryId();
            ISpecification<MessageTemplateEditEntry> hasValidMessageTypeId = new HasValidMessageTypeId();
            ISpecification<MessageTemplateEditEntry> hasValidTemplateName = new HasValidName();
            ISpecification<MessageTemplateEditEntry> hasValiTemplateBody = new HasValidTemplateBody();
            return hasValidCategoryId.And(hasValidMessageTypeId).And(hasValidTemplateName).And(hasValiTemplateBody).IsSatisfyBy(data, violations);
        }

        internal class HasValidCategoryId : SpecificationBase<MessageTemplateEditEntry>
        {
            protected override bool IsSatisfyBy(MessageTemplateEditEntry data, IList<RuleViolation> violations = null)
            {
                if (data.NotificationTypeId <= 0 && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullCategoryId, "CategoryId", data.NotificationTypeId,
                         ErrorMessage.Messages[ErrorCode.NullCategoryId]));
                    return false;
                }
                else
                {
                    var category = UnitOfWork.NotificationTypeRepository.FindById(data.NotificationTypeId);
                    if (category == null && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidCategoryId, "CategoryId", data.NotificationTypeId,
                        ErrorMessage.Messages[ErrorCode.InvalidCategoryId]));
                        return false;
                    }
                }
                return true;
            }
        }

        internal class HasValidMessageTypeId : SpecificationBase<MessageTemplateEditEntry>
        {
            protected override bool IsSatisfyBy(MessageTemplateEditEntry data, IList<RuleViolation> violations = null)
            {
                if (data.MessageTypeId <= 0 && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullMessageTypeId, "MessageTypeId", data.MessageTypeId,
                         ErrorMessage.Messages[ErrorCode.NullMessageTypeId]));
                    return false;
                }
                else
                {
                    var type = UnitOfWork.MessageTypeRepository.FindById(data.MessageTypeId);
                    if (type == null && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidMessageTypeId, "MessageTypeId", data.MessageTypeId,
                        ErrorMessage.Messages[ErrorCode.InvalidMessageTypeId]));
                        return false;
                    }
                }
                return true;
            }
        }

        internal class HasValidName : SpecificationBase<MessageTemplateEditEntry>
        {
            protected override bool IsSatisfyBy(MessageTemplateEditEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.TemplateName) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullTemplateName, "TemplateName", data.TemplateName,
                        ErrorMessage.Messages[ErrorCode.NullTemplateName]));
                    return false;
                }
                else
                {
                    if (data.TemplateName.Length > 250 && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidTemplateName, "TemplateName", data.TemplateName,
                       ErrorMessage.Messages[ErrorCode.InvalidTemplateName]));
                        return false;
                    }
                }

                return true;
            }
        }
        internal class HasValidTemplateBody : SpecificationBase<MessageTemplateEditEntry>
        {
            protected override bool IsSatisfyBy(MessageTemplateEditEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.TemplateBody) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidTemplateContent, "TemplateBody", data.TemplateBody,
                       ErrorMessage.Messages[ErrorCode.InvalidTemplateContent]));
                    return false;
                }
                return true;
            }
        }
    }
}
