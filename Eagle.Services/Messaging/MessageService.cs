using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Permission;
using Eagle.Entities.Services.Messaging;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Services.Message;
using Eagle.Services.EntityMapping.Common;
using Eagle.Services.Messaging.Validations;
using Eagle.Services.Validations;

namespace Eagle.Services.Messaging
{
    public class MessageService : BaseService, IMessageService
    {
        public MessageService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
          
        }

        #region Message Queue

        public MessageQueueDetail CreateMessageQueue(MessageQueueEntry entry)
        {
            ISpecification<MessageQueueEntry> validator = new MessageQueueEntryValidator(UnitOfWork, PermissionLevel.Create);
            var dataViolations = new List<RuleViolation>();
            var isValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isValid) throw new ValidationError(dataViolations);

            var messageQueue = entry.ToEntity<MessageQueueEntry, MessageQueue>();
            UnitOfWork.MessageQueueRepository.Insert(messageQueue);
            UnitOfWork.SaveChanges();

            return messageQueue.ToDto<MessageQueue, MessageQueueDetail>();
        }


        public void UpdateMessageQueue(MessageQueueEditEntry entry)
        {
            ISpecification<MessageQueueEditEntry> validator = new MessageQueueEditEntryValidator(UnitOfWork, PermissionLevel.Create);
            var violations = new List<RuleViolation>();
            var isValid = validator.IsSatisfyBy(entry, violations);
            if (!isValid) throw new ValidationError(violations);

            var entity = UnitOfWork.MessageQueueRepository.FindById(entry.QueueId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundMessageTemplate, "MessageTemplate", null,
                    ErrorMessage.Messages[ErrorCode.NotFoundMessageTemplate]));
                throw new ValidationError(violations);
            }

            entity.From = entry.From;
            entity.To = entry.To;
            entity.Subject = entry.Subject;
            entity.Body = entry.Body;
            entity.Bcc = entry.Bcc;
            entity.Cc = entry.Cc;
            entity.Status = entry.Status;
            entity.ResponseStatus = entry.ResponseStatus;
            entity.ResponseMessage = entry.ResponseMessage;
            entity.PredefinedDate = entry.PredefinedDate;
            entity.SentDate = entry.SentDate;

            UnitOfWork.MessageQueueRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region Message Type
        public IEnumerable<MessageTypeDetail> GetMessageTypes(bool? status)
        {
            var entityList = UnitOfWork.MessageTypeRepository.GetMessageTypes(status);
            return entityList.ToDtos<MessageType, MessageTypeDetail>();
        }
        public MessageTypeDetail GetMessageTypeDetail(int id)
        {
            var item = UnitOfWork.MessageTypeRepository.FindById(id);
            return item.ToDto<MessageType, MessageTypeDetail>();
        }
        public MultiSelectList PopulateMessageTypeMultiSelectList(bool? status = null, int[] selectedValues = null, bool? isShowSelectText = false)
        {
            return UnitOfWork.MessageTypeRepository.PopulateMessageTypeMultiSelectList(status, selectedValues, isShowSelectText);
        }
        public SelectList PopulateMessageTypeSelectList(int? selectedValue = null, bool? isShowSelectText = true)
        {
            return UnitOfWork.MessageTypeRepository.PopulateMessageTypeSelectList(selectedValue, isShowSelectText);
        }
        public SelectList PopulateMessageTypeStatus(bool? selectedValue = null, bool? isShowSelectText = true)
        {
            return UnitOfWork.MessageTypeRepository.PopulateMessageTypeStatus(selectedValue, isShowSelectText);
        }

        public MessageTypeDetail InsertMessageType(MessageTypeEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (string.IsNullOrEmpty(entry.MessageTypeName))
            {
                violations.Add(new RuleViolation(ErrorCode.NullMessageTypeName, "MessageTypeName", entry.MessageTypeName,
                    ErrorMessage.Messages[ErrorCode.NullMessageTypeName]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.MessageTypeName.Length > 250)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidMessageTypeName, "MessageTypeName", entry.MessageTypeName,
                   ErrorMessage.Messages[ErrorCode.InvalidMessageTypeName]));
                    throw new ValidationError(violations);
                }
                else
                {
                    var isExisted = UnitOfWork.MessageTypeRepository.HasDataExists(entry.MessageTypeName);
                    if (isExisted)
                    {
                        violations.Add(new RuleViolation(ErrorCode.DuplicateMessageTypeName, "MessageTypeName", entry.MessageTypeName,
                   ErrorMessage.Messages[ErrorCode.DuplicateMessageTypeName]));
                        throw new ValidationError(violations);
                    }
                }
            }
            
            var entity = entry.ToEntity<MessageTypeEntry, MessageType>();
            UnitOfWork.MessageTypeRepository.Insert(entity);
            UnitOfWork.SaveChanges();
            return entity.ToDto<MessageType, MessageTypeDetail>();
        }

        public void UpdateMessageType(MessageTypeEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.MessageTypeRepository.Find(entry.MessageTypeId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundMessageType, "MessageType", null,
                    ErrorMessage.Messages[ErrorCode.NotFoundMessageType]));
                throw new ValidationError(violations);
            }

           
            if (string.IsNullOrEmpty(entry.MessageTypeName))
            {
                violations.Add(new RuleViolation(ErrorCode.NullMessageTypeName, "MessageTypeName", entry.MessageTypeName,
                    ErrorMessage.Messages[ErrorCode.NullMessageTypeName]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.MessageTypeName.Length > 250)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidMessageTypeName, "MessageTypeName", entry.MessageTypeName,
                   ErrorMessage.Messages[ErrorCode.InvalidMessageTypeName]));
                    throw new ValidationError(violations);
                }
                else
                {
                    if (entity.MessageTypeName != entry.MessageTypeName)
                    {
                        var isExisted = UnitOfWork.MessageTypeRepository.HasDataExists(entry.MessageTypeName);
                        if (isExisted)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicateMessageTypeName, "MessageTypeName",
                                entry.MessageTypeName,
                                ErrorMessage.Messages[ErrorCode.DuplicateMessageTypeName]));
                            throw new ValidationError(violations);
                        }
                    }
                }
            }

            //Assign data
            entity.MessageTypeName = entry.MessageTypeName;
            entity.Description = entry.Description;
            entity.Status = entry.Status;

            UnitOfWork.MessageTypeRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateMessageTypeStatus(int id, bool status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.MessageTypeRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundMessageTypeId, "MessageTypeId", id, ErrorMessage.Messages[ErrorCode.NotFoundMessageTypeId]));
                throw new ValidationError(violations);
            }
            if (entity.Status == status) return;

            entity.Status = status;

            UnitOfWork.MessageTypeRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        #endregion
        
        #region Message Template

        public IEnumerable<MessageTemplateInfoDetail> GetMessageTemplates(MessageTemplateSearchEntry entry, out int recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = new List<MessageTemplateInfoDetail>();
            var templates = UnitOfWork.MessageTemplateRepository.GetMessageTemplates(entry.SearchText, entry.TypeId, entry.IsActive, out recordCount, orderBy, page, pageSize).ToList();
            if (templates.Any())
            {
                lst.AddRange(templates.Select(item => new MessageTemplateInfoDetail
                {
                    MessageTypeId = item.MessageTypeId,
                    NotificationTypeId =  item.NotificationTypeId,
                    TemplateId = item.TemplateId,
                    TemplateName = item.TemplateName,
                    TemplateSubject = item.TemplateSubject,
                    TemplateBody = item.TemplateBody,
                    Status = item.Status,
                    MessageType = item.MessageType.ToDto<MessageType, MessageTypeDetail>()
                }));
            }
            return lst;
        }

        public IEnumerable<MessageTemplateDetail> GetMessageTemplatesByNotificationTypeId(int notificationTypeId)
        {
            var items = UnitOfWork.MessageTemplateRepository.GetMessageTemplatesByNotificationTypeId(notificationTypeId);
            return items.ToDtos<MessageTemplate, MessageTemplateDetail>();
        }
      
        public MessageTemplateDetail GetMessageTemplateDetail(int id)
        {
            var item = UnitOfWork.MessageTemplateRepository.FindById(id);
            return item.ToDto<MessageTemplate, MessageTemplateDetail>();
        }

        public MessageTemplateDetail GetMessageTemplateDetail(int notificationTypeId, int messageTypeId)
        {
            var items = UnitOfWork.MessageTemplateRepository.GetMessageTemplateDetail(notificationTypeId, messageTypeId);
            return items.ToDto<MessageTemplate, MessageTemplateDetail>();
        }

        public SelectList PopulateMessageTemplateStatus(bool? selectedValue = null, bool? isShowSelectText = true)
        {
            return UnitOfWork.MessageTemplateRepository.PopulateMessageTemplateStatus(selectedValue, isShowSelectText);
        }
        public MessageTemplateDetail InsertMessageTemplate(MessageTemplateEntry entry)
        {
            ISpecification<MessageTemplateEntry> validator = new MessageTemplateEntryValidator(UnitOfWork);
            var dataViolations = new List<RuleViolation>();
            var isValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isValid) throw new ValidationError(dataViolations);

            var entity = entry.ToEntity<MessageTemplateEntry, MessageTemplate>();
            entity.CreatedDate = DateTime.UtcNow;

            UnitOfWork.MessageTemplateRepository.Insert(entity);
            UnitOfWork.SaveChanges();
            return entity.ToDto<MessageTemplate, MessageTemplateDetail>();
        }

        public void UpdateMessageTemplate(MessageTemplateEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.MessageTemplateRepository.Find(entry.TemplateId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundMessageTemplate, "MessageTemplate", null,
                    ErrorMessage.Messages[ErrorCode.NotFoundMessageTemplate]));
                throw new ValidationError(violations);
            }

            ISpecification<MessageTemplateEditEntry> validator = new MessageTemplateEditEntryValidator(UnitOfWork);
            var dataViolations = new List<RuleViolation>();
            var isValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isValid) throw new ValidationError(dataViolations);

            if (entity.TemplateName != entry.TemplateName)
            {
                var isExisted = UnitOfWork.MessageTemplateRepository.HasDataExists(entry.TemplateName);
                if (isExisted)
                {
                    violations.Add(new RuleViolation(ErrorCode.DuplicateTemplateName, "TemplateName",
                        entry.TemplateName,
                        ErrorMessage.Messages[ErrorCode.DuplicateTemplateName]));
                    throw new ValidationError(violations);
                }
            }

            //Assign data
            entity.MessageTypeId = entry.MessageTypeId;
            entity.NotificationTypeId = entry.NotificationTypeId;
            entity.TemplateName = entry.TemplateName;
            entity.TemplateSubject = entry.TemplateSubject;
            entity.TemplateBody = entry.TemplateBody;
            entity.TemplateCode = entry.TemplateCode;
            entity.Status = entry.Status;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.MessageTemplateRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateMessageTemplateStatus(int id, bool status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.MessageTemplateRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundForMessageTemplate, "TemplateId", id, ErrorMessage.Messages[ErrorCode.NotFoundForMessageTemplate]));
                throw new ValidationError(violations);
            }

            if (entity.Status == status) return;

            entity.Status = status;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.MessageTemplateRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion
        
    }
}
