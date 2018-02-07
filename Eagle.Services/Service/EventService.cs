using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Eagle.Common.Services.Mail;
using Eagle.Common.Services.Parse;
using Eagle.Common.Utilities;
using Eagle.Core.Common;
using Eagle.Core.Permission;
using Eagle.Core.Settings;
using Eagle.Entities.Common;
using Eagle.Entities.Services.Events;
using Eagle.Entities.Services.Messaging;
using Eagle.Repositories;
using Eagle.Resources;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Services.Event;
using Eagle.Services.Dtos.Services.Message;
using Eagle.Services.Dtos.SystemManagement.FileStorage;
using Eagle.Services.EntityMapping.Common;
using Eagle.Services.Exceptions;
using Eagle.Services.Messaging;
using Eagle.Services.Service.Validation;
using Eagle.Services.SystemManagement;
using Eagle.Services.Validations;

namespace Eagle.Services.Service
{
    public class EventService: BaseService, IEventService
    {
        public IDocumentService DocumentService { get; set; }
        public IMailService MailService { get; set; }
        public IMessageService MessageService { get; set; }
        public INotificationService NotificationService { get; set; }
        public EventService(IUnitOfWork unitOfWork, IDocumentService documentService, IMessageService messageService, 
            INotificationService notificationService, IMailService mailService) : base(unitOfWork)
        {
            DocumentService = documentService;
            MessageService = messageService;
            NotificationService = notificationService;
            MailService = mailService;
        }

        #region Event Type

        public IEnumerable<TreeDetail> GetEventTypeSelectTree(EventTypeStatus? status, int? selectedId, bool? isRootShowed = false)
        {
            var lst = UnitOfWork.EventTypeRepository.GetEventTypeSelectTree(status, selectedId, isRootShowed);
            return lst.ToDtos<TreeEntity, TreeDetail>();
        }
 
        public EventTypeDetail GetEventTypeDetail(int id)
        {
            var entity = UnitOfWork.EventTypeRepository.FindById(id);
            return entity.ToDto<EventType, EventTypeDetail>();
        }
  
        public void InsertEventType(Guid userId, EventTypeEntry entry)
        {
            ISpecification<EventTypeEntry> validator = new EventTypeEntryValidator(UnitOfWork, PermissionLevel.Create, CurrentClaimsIdentity);
            var dataViolations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isDataValid) throw new ValidationError(dataViolations);

            var entity = entry.ToEntity<EventTypeEntry, EventType>();
            entity.HasChild = false;
            entity.ListOrder = UnitOfWork.NewsRepository.GetNewListOrder();
            entity.CreatedDate = DateTime.UtcNow;
            entity.Ip = NetworkUtils.GetIP4Address();
            entity.CreatedByUserId = userId;

            UnitOfWork.EventTypeRepository.Insert(entity);
            UnitOfWork.SaveChanges();

            if (entry.ParentId != null && entry.ParentId > 0)
            {
                var parentEntity = UnitOfWork.EventTypeRepository.FindById(Convert.ToInt32(entry.ParentId));
                if (parentEntity == null) return;

                parentEntity.HasChild = true;
                UnitOfWork.EventTypeRepository.Update(parentEntity);

                var lineage = $"{parentEntity.Lineage},{entity.TypeId}";
                entity.Lineage = lineage;
                entity.Depth = lineage.Split(',').Count();
                entity.ParentId = entry.ParentId;
            }
            else
            {
                entity.ParentId = 0;
                entity.Lineage = $"{entity.TypeId}";
                entity.Depth = 1;
            }

            UnitOfWork.EventTypeRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateEventType(Guid userId, EventTypeEditEntry entry)
        {
            ISpecification<EventTypeEditEntry> validator = new EventTypeEditEntryValidator(UnitOfWork, PermissionLevel.Edit, CurrentClaimsIdentity);
            var dataViolations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isDataValid) throw new ValidationError(dataViolations);

            var entity = UnitOfWork.EventTypeRepository.FindById(entry.TypeId);
            if (entity == null) return;

            if (entity.TypeName != entry.TypeName)
            {
                bool isDuplicate = UnitOfWork.NewsCategoryRepository.HasDataExisted(entry.TypeName, entry.ParentId);
                if (isDuplicate)
                {
                    dataViolations.Add(new RuleViolation(ErrorCode.DuplicateTypeName, "TypeName", entry.TypeName, ErrorMessage.Messages[ErrorCode.DuplicateTypeName]));
                    throw new ValidationError(dataViolations);
                }
            }

            if (entry.ParentId != entity.ParentId && entry.ParentId != entity.TypeId)
            {
                if (entry.ParentId != null && entry.ParentId > 0)
                {
                    var children = UnitOfWork.EventTypeRepository.GetAllChildrenNodesOfSelectedNode(Convert.ToInt32(entry.TypeId)).ToList();
                    if (children.Any())
                    {
                        var parentIds = children.Select(x => x.TypeId).ToList();
                        if (parentIds.Contains(Convert.ToInt32(entry.ParentId)))
                        {
                            dataViolations.Add(new RuleViolation(ErrorCode.InvalidParentId, "ParentId", entry.ParentId, ErrorMessage.Messages[ErrorCode.InvalidParentId]));
                            throw new ValidationError(dataViolations);
                        }
                    }

                    //Update parent entry
                    var parentEntryEntity = UnitOfWork.EventTypeRepository.FindById(Convert.ToInt32(entry.ParentId));
                    if (parentEntryEntity == null)
                    {
                        dataViolations.Add(new RuleViolation(ErrorCode.NotFoundParentId, "ParentId", entry.ParentId, ErrorMessage.Messages[ErrorCode.NotFoundParentId]));
                        throw new ValidationError(dataViolations);
                    }
                    else
                    {
                        if (parentEntryEntity.HasChild == null || parentEntryEntity.HasChild == false)
                        {
                            parentEntryEntity.HasChild = true;
                            UnitOfWork.EventTypeRepository.Update(parentEntryEntity);
                        }
                    }

                    //Update parent entity
                    var parentEntity = UnitOfWork.EventTypeRepository.FindById(entity.ParentId);
                    if (parentEntity != null)
                    {
                        var childList = UnitOfWork.EventTypeRepository.GetAllChildrenNodesOfSelectedNode(Convert.ToInt32(entity.ParentId)).ToList();
                        if (childList.Any())
                        {
                            childList = childList.Where(x => (x.TypeId != entity.ParentId) && (x.TypeId != entity.TypeId)).ToList();
                            parentEntity.HasChild = childList.Any();
                            UnitOfWork.EventTypeRepository.Update(parentEntity);
                        }
                    }

                    var lineage = $"{parentEntryEntity.Lineage},{entry.TypeId}";
                    entity.Lineage = lineage;
                    entity.Depth = lineage.Split(',').Count();
                    entity.ParentId = entry.ParentId;
                }
                else
                {
                    entity.ParentId = 0;
                    entity.Lineage = $"{entry.TypeId}";
                    entity.Depth = 1;
                }
            }

            //Update entity

            var hasChild = UnitOfWork.EventTypeRepository.HasChild(entity.TypeId);
            entity.HasChild = hasChild;
            entity.TypeId = entry.TypeId;
            entity.TypeName = entry.TypeName;
            entity.Status = entry.Status;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedByUserId = userId;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.EventTypeRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateEventTypeStatus(Guid userId, int id, EventTypeStatus status)
        {
            var entity = UnitOfWork.EventTypeRepository.FindById(id);
            if (entity == null) throw new NotFoundDataException();

            var violations = new List<RuleViolation>();
            var isValid = Enum.IsDefined(typeof(EventTypeStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status", status, ErrorMessage.Messages[ErrorCode.InvalidStatus]));
                throw new ValidationError(violations);
            }
            if (entity.Status == status) return;

            entity.Status = status;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedByUserId = userId;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.EventTypeRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateEventTypeListOrder(Guid userId, int id, int listOrder)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.EventTypeRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundProductCategory, "EventType", id, ErrorMessage.Messages[ErrorCode.NotFoundProductCategory]));
                throw new ValidationError(violations);
            }

            if (entity.ListOrder == listOrder) return;

            entity.ListOrder = listOrder;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedByUserId = userId;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.EventTypeRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region Event
        public IEnumerable<EventInfoDetail> Search(EventSearchEntry searchEntry, out int recordCount, string orderBy = null, int? page = 1, int? pageSize = null)
        {
            var lst = new List<EventInfoDetail>();
            var events = UnitOfWork.EventRepository.Search(out recordCount, searchEntry.Keywords, searchEntry.TypeId, searchEntry.FromDate, searchEntry.ToDate, searchEntry.SearchStatus, orderBy, page, pageSize);
            if (events != null)
            {
                lst.AddRange(events.Select(item => new EventInfoDetail
                {
                    TypeId = item.TypeId,
                    EventId = item.EventId,
                    EventCode = item.EventCode,
                    EventTitle = item.EventTitle,
                    EventMessage = item.EventMessage,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    TimeZone = item.TimeZone,
                    Location = item.Location,
                    Latitude = item.Latitude,
                    Longitude = item.Longitude,
                    SmallPhoto = item.SmallPhoto,
                    LargePhoto = item.LargePhoto,
                    IsNotificationUsed = item.IsNotificationUsed,
                    Status = item.Status,
                    Ip = item.Ip,
                    LastUpdatedIp = item.LastUpdatedIp,
                    CreatedDate = item.CreatedDate,
                    LastModifiedDate = item.LastModifiedDate,
                    CreatedByUserId = item.CreatedByUserId,
                    LastModifiedByUserId = item.LastModifiedByUserId,
                    EventType = item.EventType.ToDto<EventType, EventTypeDetail>(),
                    //Topic = item.Topic.ToDto<MediaTopic, MediaTopicDetail>(),
                }));
            }
            return lst;
        }

        public IEnumerable<EventInfoDetail> GetEvents(int? typeId, EventStatus? status, out int recordCount, string orderBy = null, int? page = 1, int? pageSize = null)
        {
            var lst = new List<EventInfoDetail>();
            var events = UnitOfWork.EventRepository.GetEvents(out recordCount, typeId, status, orderBy, page, pageSize).ToList();
            if (events != null)
            {
                lst.AddRange(events.Select(item => new EventInfoDetail
                {
                    TypeId = item.TypeId,
                    EventId = item.EventId,
                    EventCode = item.EventCode,
                    EventTitle = item.EventTitle,
                    EventMessage = item.EventMessage,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    TimeZone = item.TimeZone,
                    Location = item.Location,
                    Latitude = item.Latitude,
                    Longitude = item.Longitude,
                    SmallPhoto = item.SmallPhoto,
                    LargePhoto = item.LargePhoto,
                    IsNotificationUsed = item.IsNotificationUsed,
                    Status = item.Status,
                    Ip = item.Ip,
                    LastUpdatedIp = item.LastUpdatedIp,
                    CreatedDate = item.CreatedDate,
                    LastModifiedDate = item.LastModifiedDate,
                    CreatedByUserId = item.CreatedByUserId,
                    LastModifiedByUserId = item.LastModifiedByUserId,
                    EventType = item.EventType.ToDto<EventType, EventTypeDetail>(),
                    //Topic = item.Topic.ToDto<MediaTopic, MediaTopicDetail>(),
                }));
            }
            return lst;
        }
        public string GenerateCode(int maxLetters)
        {
            return UnitOfWork.EventRepository.GenerateCode(maxLetters);
        }
        public SelectList PoplulateTimeZoneSelectList(string selectedValue = null, bool? isShowSelectText = true)
        {
            var listItems = new List<SelectListItem>();
            var lst = TimeZoneInfo.GetSystemTimeZones();
            if (lst.Any())
            {
                listItems = lst.Select(p => new SelectListItem { Text = p.DisplayName, Value = p.Id }).ToList();
                if (isShowSelectText != null && isShowSelectText == true)
                    listItems.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectTimeZone} ---", Value = "" });
            }
            else
            {
                listItems.Insert(0, new SelectListItem { Text = $"-- {LanguageResource.None} --", Value = "-1" });
            }
            return new SelectList(listItems, "Value", "Text", selectedValue);
        }
        public EventInfoDetail GeEventDetail(int id)
        {
            var documentInfos = new List<DocumentInfoDetail>();
            var entity = UnitOfWork.EventRepository.GetDetails(id);
            if (entity == null) return new EventInfoDetail();

            if (entity.SmallPhoto != null)
            {
                var frontImageInfo = DocumentService.GetFileInfoDetail(Convert.ToInt32(entity.SmallPhoto));
                documentInfos.Add(frontImageInfo);
            }
            if (entity.LargePhoto != null)
            {
                var mainImageInfo = DocumentService.GetFileInfoDetail(Convert.ToInt32(entity.LargePhoto));
                documentInfos.Add(mainImageInfo);
            }

            var item = new EventInfoDetail
            {
                TypeId = entity.TypeId,
                EventId = entity.EventId,
                EventCode = entity.EventCode,
                EventTitle = entity.EventTitle,
                EventMessage = HttpUtility.HtmlDecode(entity.EventMessage),
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                TimeZone = entity.TimeZone,
                Location = entity.Location,
                Latitude = entity.Latitude,
                Longitude = entity.Longitude,
                SmallPhoto = entity.SmallPhoto,
                LargePhoto = entity.LargePhoto,
                IsNotificationUsed = entity.IsNotificationUsed,
                Status = entity.Status,
                CreatedDate = entity.CreatedDate,
                LastModifiedDate = entity.LastModifiedDate,
                CreatedByUserId = entity.CreatedByUserId,
                LastModifiedByUserId = entity.LastModifiedByUserId,
                Ip = entity.Ip,
                LastUpdatedIp = entity.LastUpdatedIp,
                EventType = entity.EventType.ToDto<EventType, EventTypeDetail>(),
                DocumentInfos = documentInfos,
            };
           
            return item;
        }
        public EventDetail InsertEvent(Guid applicationId, Guid userId, int vendorId, EventEntry entry)
        {
            ISpecification<EventEntry> validator = new EventEntryValidator(UnitOfWork, PermissionLevel.Create, CurrentClaimsIdentity);
            var violations = new List<RuleViolation>();
            if (!validator.IsSatisfyBy(entry, violations)) throw new ValidationError(violations);

            var entity = entry.ToEntity<EventEntry, Event>();
            entity.EventMessage = StringUtils.UTF8_Encode(entry.EventMessage);
            entity.CreatedDate = DateTime.UtcNow;
            entity.Ip = NetworkUtils.GetIP4Address();
            entity.CreatedByUserId = userId;

            if (entry.File != null && entry.File.ContentLength > 0)
            {
                var fileIds = DocumentService.UploadAndSaveWithThumbnail(applicationId, userId, entry.File, (int)FileLocation.Event, StorageType.Local);
                entity.LargePhoto = fileIds[0].FileId;
                entity.SmallPhoto = fileIds[1].FileId;
            }

            if (!string.IsNullOrEmpty(entry.Location))
            {
                var address = entry.Location;
                entity.Location = address;

                var requestUri = string.Format("http://maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=false", Uri.EscapeDataString(address));
                var request = WebRequest.Create(requestUri);
                var response = request.GetResponse();
                var xdoc = XDocument.Load(response.GetResponseStream());
                if (xdoc != null)
                {
                    var geocodeResponse = xdoc.Element("GeocodeResponse");
                    if (geocodeResponse != null)
                    {
                        var result = geocodeResponse.Element("result");
                        if (result != null)
                        {
                            var geometry = result.Element("geometry");
                            if (geometry != null)
                            {
                                var locationElement = geometry.Element("location");
                                if (locationElement != null)
                                {
                                    var latitude = locationElement.Element("lat");
                                    if (latitude != null)
                                    {
                                        entity.Latitude = Double.Parse(latitude.Value);
                                    }

                                    var longitude = locationElement.Element("lng");
                                    if (longitude != null)
                                    {
                                        entity.Longitude = Double.Parse(longitude.Value);
                                    }

                                }
                            }
                        }
                    }
                }
            }

            UnitOfWork.EventRepository.Insert(entity);
            UnitOfWork.SaveChanges();

            if (entry.IsNotificationUsed && entity.EventId > 0)
            {
                SendMailNotification(applicationId, vendorId, NotificationTypeSetting.CreateEvent, entity.EventId);
            }
            return entity.ToDto<Event, EventDetail>();
        }
        public void UpdateEvent(Guid applicationId, Guid userId, int vendorId, EventEditEntry entry)
        {
            //Check validation
            ISpecification<EventEditEntry> validator = new EventEditEntryValidator(UnitOfWork, PermissionLevel.Create, CurrentClaimsIdentity);
            var violations = new List<RuleViolation>();
            if (!validator.IsSatisfyBy(entry, violations)) throw new ValidationError(violations);

            var entity = UnitOfWork.EventRepository.Find(entry.EventId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidEventId, "EventId", entry.EventId, ErrorMessage.Messages[ErrorCode.InvalidEventId]));
                throw new ValidationError(violations);
            }

            if (!string.IsNullOrEmpty(entry.EventCode) && entry.EventCode !=entity.EventCode)
            {
                var isDuplicated = UnitOfWork.EventRepository.HasCodeExisted(entry.EventCode);
                if (isDuplicated)
                {
                    violations.Add(new RuleViolation(ErrorCode.DuplicateEventCode, "EventCode", entry.EventTitle, ErrorMessage.Messages[ErrorCode.DuplicateEventCode]));
                    throw new ValidationError(violations);
                }
            }

            if (!string.IsNullOrEmpty(entry.EventTitle) && entry.EventTitle != entity.EventTitle)
            {
                var isDuplicated = UnitOfWork.EventRepository.HasDataExisted(entry.TypeId, entry.EventTitle);
                if (isDuplicated)
                {
                    violations.Add(new RuleViolation(ErrorCode.DuplicateTitle, "EventTitle", entry.EventTitle, ErrorMessage.Messages[ErrorCode.DuplicateTitle]));
                    throw new ValidationError(violations);
                }
            }
            var existedStatus = entity.Status;

            //Assign data
            entity.TypeId = entry.TypeId;
            entity.EventTitle = entry.EventTitle;
            entity.EventCode = entry.EventCode;
            entity.EventMessage = StringUtils.UTF8_Encode(entry.EventMessage);
            entity.StartDate = entry.StartDate;
            entity.EndDate = entry.EndDate;
            entity.TimeZone = entry.TimeZone;
            entity.IsNotificationUsed = entry.IsNotificationUsed;
            entity.Status = entry.Status;
            entity.LastModifiedDate = DateTime.UtcNow;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedByUserId = userId;

            if (entry.File != null && entry.File.ContentLength > 0)
            {
                if (entity.SmallPhoto != null)
                {
                    DocumentService.DeleteFile(Convert.ToInt32(entity.SmallPhoto));
                }

                if (entity.LargePhoto != null)
                {
                    DocumentService.DeleteFile(Convert.ToInt32(entity.LargePhoto));
                }

                var fileIds = DocumentService.UploadAndSaveWithThumbnail(applicationId, userId, entry.File, (int)FileLocation.Event, StorageType.Local);
                entity.LargePhoto = fileIds[0].FileId;
                entity.SmallPhoto = fileIds[1].FileId;
            }


            if (!string.IsNullOrEmpty(entry.Location) && entry.Location != entity.Location)
            {
                var address = entry.Location;
                entity.Location = address;

                var requestUri = string.Format("http://maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=false", Uri.EscapeDataString(address));
                var request = WebRequest.Create(requestUri);
                var response = request.GetResponse();
                var xdoc = XDocument.Load(response.GetResponseStream());
                if (xdoc != null)
                {
                    var geocodeResponse = xdoc.Element("GeocodeResponse");
                    if (geocodeResponse != null)
                    {
                        var result = geocodeResponse.Element("result");
                        if (result != null)
                        {
                            var geometry = result.Element("geometry");
                            if (geometry != null)
                            {
                                var locationElement = geometry.Element("location");
                                if (locationElement != null)
                                {
                                    var latitude = locationElement.Element("lat");
                                    if (latitude != null)
                                    {
                                        entity.Latitude = Double.Parse(latitude.Value);
                                    }

                                    var longitude = locationElement.Element("lng");
                                    if (longitude != null)
                                    {
                                        entity.Longitude = Double.Parse(longitude.Value);
                                    }

                                }
                            }
                        }
                    }
                }
            }

            UnitOfWork.EventRepository.Update(entity);
            UnitOfWork.SaveChanges();

            if (entry.IsNotificationUsed && existedStatus != entry.Status)
            {
                SendMailNotification(applicationId, vendorId, NotificationTypeSetting.ChangeEventStatus, entity.EventId);
            }
        }
        public void UpdateEventStatus(Guid applicationId, Guid userId, int vendorId, int id, EventStatus status)
        {
            var violations = new List<RuleViolation>();
            var isValid = Enum.IsDefined(typeof(EventStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status", status));
                throw new ValidationError(violations);
            }

            var entity = UnitOfWork.EventRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundEventId, "EventId", id, ErrorMessage.Messages[ErrorCode.NotFoundEventId]));
                throw new ValidationError(violations);
            }

            var existedStatus = entity.Status;
            if (entity.Status == status) return;

            entity.Status = status;
            entity.LastModifiedDate = DateTime.UtcNow;
            entity.LastModifiedByUserId = userId;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            UnitOfWork.EventRepository.Update(entity);
            UnitOfWork.SaveChanges();

            if (entity.IsNotificationUsed && existedStatus != status)
            {
                SendMailNotification(applicationId, vendorId, NotificationTypeSetting.ChangeEventStatus, entity.EventId);
            }
        }
        #endregion

        #region Event Notification

        public void SendMailNotification(Guid applicationId, int vendorId, NotificationTypeSetting notificationTypeSetting, int eventId, DateTime? predefinedDate = null, string targetId = null, string bcc = null, string cc = null)
        {
            var violations = new List<RuleViolation>();
            //int messageTypeId = Convert.ToInt32(MessateTypeSetting.Email);
            int notificationTypeId = Convert.ToInt32(notificationTypeSetting);

            var notificationType = UnitOfWork.NotificationTypeRepository.FindById(notificationTypeId);
            if (notificationType == null) return;

            //Get Template 
            var template = MessageService.GetMessageTemplateDetail(notificationTypeId, Convert.ToInt32(MessageTypeSetting.Email));
            if (template == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundForMessageTemplate, "MessageTemplate", null, ErrorMessage.Messages[ErrorCode.NotFoundForMessageTemplate]));
                throw new ValidationError(violations);
            }

            //Get Mail Settings
            var mailSettings = MailService.GetDefaultSmtpInfo(applicationId);
            if (mailSettings == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundNotificationSender, "NotificationSender", null, ErrorMessage.Messages[ErrorCode.NotFoundNotificationSender]));
                throw new ValidationError(violations);
            }

            //Get Sender Information 
            var sender = NotificationService.GetNotificationSenderDetail(notificationType.NotificationSenderTypeId);

            //Get Receivers
            var receivers = NotificationService.GetNotificationTargets(notificationTypeId, targetId);
            if (receivers.Any())
            {
                var eventInfo = GeEventDetail(eventId);

                //string receiverMails = string.Join(",", receivers.Select(x => x.MailAddress).ToArray());
                string subject = template.TemplateSubject;

                foreach (var receiver in receivers)
                {
                    var templateVariables = new Hashtable
                    {
                        //Bind Event
                        { "EventCode", eventInfo.EventCode },
                        { "EventTitle", eventInfo.EventTitle },
                        { "EventMessage",eventInfo.EventMessage },
                        { "StartDate", eventInfo.StartDate },
                        { "EndDate", eventInfo.EndDate },
                        { "TimeZone", eventInfo.TimeZone },
                        { "Location", eventInfo.Location },
                        { "Status", eventInfo.Status },
                        { "CreatedDate", eventInfo.CreatedDate },
                        { "LastModifiedDate", eventInfo.LastModifiedDate },

                         //Bind Customer
                        {"CustomerNo", receiver.TargetNo},
                        {"ContactName", receiver.ContactName},
                        {"CustomerName", receiver.TargetName},
                        {"Email", receiver.MailAddress},
                        {"Mobile", receiver.Mobile},
                        {"Address", receiver.Address}
                    };

                    string body = ParseTemplateHandler.ParseTemplate(templateVariables, template.TemplateBody);

                    //Create message queue
                    var messageQueueEntry = new MessageQueueEntry
                    {
                        From = sender.MailAddress,
                        To = receiver.MailAddress,
                        Subject = subject,
                        Body = body,
                        Bcc = bcc,
                        Cc = cc,
                        PredefinedDate = predefinedDate
                    };
                    var messageQueue = MessageService.CreateMessageQueue(messageQueueEntry);

                    //Create notification message
                    var extraInfo = (from string key in templateVariables.Keys
                        select new SerializableKeyValuePair<string, string>
                        {
                            Key = key, Value = Convert.ToString(templateVariables[key])
                        }).ToList();

                    var messsageInfo = new MessageDetail
                    {
                        MessageTypeId = MessageTypeSetting.Email,
                        NotificationTypeId = notificationTypeSetting,
                        TemplateId = template.TemplateId,
                        WebsiteUrl = string.Empty,
                        WebsiteUrlBase = string.Empty,
                        ExtraInfo = extraInfo,
                        Version = "1.0"
                    };

                    var notificationMessageEntry = new NotificationMessageEntry
                    {
                        Message = messsageInfo,
                        PublishDate = predefinedDate ?? DateTime.UtcNow,
                        SentStatus = NotificationSentStatus.Ready
                    };
                    var notificationMessage = NotificationService.InsertNotificationMessage(notificationMessageEntry);

                    //Send Mail Manually
                    string result;
                    var isEmailSent = MailHandler.SendMail(mailSettings.SmtpmEmail, receiver.MailAddress, sender.SenderName, receiver.TargetName, cc, bcc, null, MailPriority.Normal, subject, true, Encoding.UTF8, body, null,
                        mailSettings.SmtpServer, SmtpAuthentication.Basic, mailSettings.SmtpUsername, mailSettings.SmtpPassword, mailSettings.EnableSsl, out result);
                   
                    //Update status in message queue
                    var messageQueueEditEntry = new MessageQueueEditEntry
                    {
                        QueueId = messageQueue.QueueId,
                        From = messageQueue.From,
                        To = messageQueue.To,
                        Subject = messageQueue.Subject,
                        Body = messageQueue.Body,
                        Bcc = messageQueue.Bcc,
                        Cc = messageQueue.Cc,
                        Status = isEmailSent,
                        ResponseStatus = isEmailSent ? 1 : 0,
                        ResponseMessage = result,
                        SentDate = DateTime.UtcNow
                    };
                    MessageService.UpdateMessageQueue(messageQueueEditEntry);


                    //Update status in notification message
                    var sentStatus = isEmailSent ? NotificationSentStatus.Sent : NotificationSentStatus.Failed;
                    NotificationService.UpdateNotificationMessageStatus(notificationMessage.NotificationMessageId,
                        messsageInfo, sentStatus);
                }
            }
        }
        #endregion

        #region Dipose

        private bool _disposed = false;
        protected override void Dispose(bool isDisposing)
        {
            if (!this._disposed)
            {
                if (isDisposing)
                {
                    DocumentService = null;
                    MailService = null;
                    NotificationService = null;
                    MessageService = null;
                    NotificationService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}
