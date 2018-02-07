using System.Collections.Generic;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Services.Message;
using Eagle.Services.Validations;

namespace Eagle.Services.Messaging.Validations
{
    public class MessageTemplateEntryValidator : SpecificationBase<MessageTemplateEntry>
    {
        private static IUnitOfWork UnitOfWork { get; set; }

        public MessageTemplateEntryValidator(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        protected override bool IsSatisfyBy(MessageTemplateEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullMessageTemplateEntry, "MessageTemplateEntry", null,
                    ErrorMessage.Messages[ErrorCode.NullMessageTemplateEntry]));
                return false;
            }

            ISpecification<MessageTemplateEntry> hasValidNotificationTypeId = new HasValidNotificationTypeId();
            ISpecification<MessageTemplateEntry> hasValidMessageTypeId = new HasValidMessageTypeId();
            ISpecification<MessageTemplateEntry> hasValidTemplateName = new HasValidTemplateName();
            ISpecification<MessageTemplateEntry> hasValiTemplateBody = new HasValiTemplateBody();
            
            return hasValidNotificationTypeId.And(hasValidMessageTypeId).And(hasValidTemplateName).And(hasValiTemplateBody).IsSatisfyBy(data, violations);
        }

        internal class HasValidNotificationTypeId : SpecificationBase<MessageTemplateEntry>
        {
            protected override bool IsSatisfyBy(MessageTemplateEntry data, IList<RuleViolation> violations = null)
            {
                if (data.NotificationTypeId <= 0 && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NotFoundNotificationTypeId, "NotificationTypeId", data.NotificationTypeId,
                         ErrorMessage.Messages[ErrorCode.NotFoundNotificationTypeId]));
                    return false;
                }
                else
                {
                    var item = UnitOfWork.NotificationTypeRepository.FindById(data.NotificationTypeId);
                    if (item == null && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidNotificationTypeId, "NotificationTypeId", data.NotificationTypeId,
                        ErrorMessage.Messages[ErrorCode.InvalidNotificationTypeId]));
                        return false;
                    }
                }
                return true;
            }
        }

        internal class HasValidMessageTypeId : SpecificationBase<MessageTemplateEntry>
        {
            protected override bool IsSatisfyBy(MessageTemplateEntry data, IList<RuleViolation> violations = null)
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

        internal class HasValidTemplateName : SpecificationBase<MessageTemplateEntry>
        {
            protected override bool IsSatisfyBy(MessageTemplateEntry data, IList<RuleViolation> violations = null)
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
                    else
                    {
                        var isExisted = UnitOfWork.MessageTemplateRepository.HasDataExists(data.TemplateName);
                        if (isExisted && violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicateTemplateName, "TemplateName",
                                data.TemplateName,
                                ErrorMessage.Messages[ErrorCode.DuplicateTemplateName]));
                            return false;
                        }
                    }
                }
                return true;
            }
        }
        internal class HasValiTemplateBody : SpecificationBase<MessageTemplateEntry>
        {
            protected override bool IsSatisfyBy(MessageTemplateEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.TemplateBody) && violations != null)
                {
                        violations.Add(new RuleViolation(ErrorCode.NullTemplateBody, "TemplateBody",
                                data.TemplateName,
                                ErrorMessage.Messages[ErrorCode.NullTemplateBody]));
                    return false;
                }
                return true;
            }
        }
    }
}
