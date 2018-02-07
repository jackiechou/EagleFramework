using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Xml.Serialization;
using Eagle.Core.Configuration;
using Eagle.Core.Extension;
using Eagle.Core.Permission;
using Eagle.Core.Settings;
using Eagle.Entities.Common;
using Eagle.Entities.Services.Messaging;
using Eagle.Repositories;
using Eagle.Resources;
using Eagle.Services.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Services.Message;
using Eagle.Services.EntityMapping.Common;
using Eagle.Services.Messaging.Validations;
using Eagle.Services.SystemManagement;
using Eagle.Services.Validations;

namespace Eagle.Services.Messaging
{
    public class NotificationService : BaseService, INotificationService
    {
        private ICustomerService CustomerService { get; set; }
        private IEmployeeService EmployeeService { get; set; }
        private IMailService MailService { get; set; }
        private IMessageService MessageService { get; set; }
        private IUserService UserService { get; set; }

        public NotificationService(IUnitOfWork unitOfWork, IMailService mailService, IMessageService messageService, IUserService userService,
            ICustomerService customerService, IEmployeeService employeeService) : base(unitOfWork)
        {
            MailService = mailService;
            MessageService = messageService;
            UserService = userService;
            CustomerService = customerService;
            EmployeeService = employeeService;
        }

        #region Notification
        public List<NotificationTargetInfoDetail> GetNotificationTargets(int notificationTypeId, string targetId)
        {
            var violations = new List<RuleViolation>();
            var receivers = new List<NotificationTargetInfoDetail>();
            var notificationType = UnitOfWork.NotificationTypeRepository.FindById(notificationTypeId);
            if (notificationType == null) return null;
           
            var notificationTargetTypes = GetNotificationTargets(notificationTypeId).Select(x => x.NotificationTargetTypeId).ToList();
            if (!notificationTargetTypes.Any()) return null;
            foreach (var notificationTargetTypeId in notificationTargetTypes)
            {
                switch (notificationTargetTypeId)
                {
                    case NotificationTargetType.System:
                        var item = UnitOfWork.NotificationTargetRepository.GetDetails(notificationTypeId,
                            NotificationTargetType.System);
                        if (item == null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.NotFoundNotificationTarget,
                                "NotificationTarget", null,
                                ErrorMessage.Messages[ErrorCode.NotFoundNotificationTarget]));
                            throw new ValidationError(violations);
                        }
                        else
                        {
                            var targetDefault =
                                UnitOfWork.NotificationTargetDefaultRepository.GetNotificationTargetDefault();
                            receivers.Add(new NotificationTargetInfoDetail
                            {
                                MailServerProviderId = targetDefault.MailServerProviderId,
                                TargetNo = targetDefault.TargetNo,
                                TargetName = targetDefault.TargetName,
                                ContactName = targetDefault.ContactName,
                                Mobile = targetDefault.Mobile,
                                Address = targetDefault.Address,
                                MailAddress = targetDefault.MailAddress,
                                Password = targetDefault.Password,
                                IsActive = targetDefault.IsActive
                            });
                        }
                        break;
                    case NotificationTargetType.User:
                        if (!string.IsNullOrEmpty(targetId))
                        {
                            var user = UserService.GetProfileDetails(Guid.Parse(targetId));
                            if (user == null)
                            {
                                violations.Add(new RuleViolation(ErrorCode.NotFoundUser, "UserId", targetId,
                                    ErrorMessage.Messages[ErrorCode.NotFoundUser]));
                                throw new ValidationError(violations);
                            }
                            else
                            {
                                receivers.Add(new NotificationTargetInfoDetail
                                {
                                    TargetNo = user.User.UserId.ToString(),
                                    TargetName = user.Contact.FullName,
                                    ContactName = user.Contact.DisplayName,
                                    Mobile = user.Contact.Mobile,
                                    MailAddress = user.Contact.Email
                                });
                            }
                        }
                        else
                        {
                            violations.Add(new RuleViolation(ErrorCode.NotFoundUserId, "UserId", targetId,
                                  ErrorMessage.Messages[ErrorCode.NotFoundUserId]));
                            throw new ValidationError(violations);
                        }
                        break;
                    case NotificationTargetType.AllCustomers:
                        var customers = CustomerService.GetCustomers(CustomerStatus.Published);
                        if (customers != null)
                        {
                            foreach (var cust in customers)
                            {
                                var target = new NotificationTargetInfoDetail
                                {
                                    TargetNo = cust.CustomerNo,
                                    TargetName = $"{cust.FirstName} {cust.LastName}",
                                    ContactName = cust.ContactName,
                                    Mobile = cust.Mobile,
                                    MailAddress = cust.Email,
                                };
                               
                                if (cust.AddressId != null && cust.AddressId > 0)
                                {
                                    var sbAddress = new StringBuilder();
                                    if (cust.Address != null)
                                    {
                                        if (!string.IsNullOrEmpty(cust.Address.Street))
                                        {
                                            sbAddress.AppendFormat("{0}", cust.Address.Street);
                                        }

                                        if (cust.Address.Region != null && !string.IsNullOrEmpty(cust.Address.Region.RegionName))
                                        {
                                            sbAddress.AppendFormat(", {0} {1}", LanguageResource.District, cust.Address.Region.RegionName);
                                        }

                                        if (cust.Address.Province != null && !string.IsNullOrEmpty(cust.Address.Province.ProvinceName))
                                        {
                                            sbAddress.AppendFormat(", {0} {1}", LanguageResource.Province, cust.Address.Province.ProvinceName);
                                        }

                                        if (cust.Address.Country != null && !string.IsNullOrEmpty(cust.Address.Country.CountryName))
                                        {
                                            sbAddress.AppendFormat(", {0}", cust.Address.Country.CountryName);
                                        }
                                    }
                                    target.Address = sbAddress.ToString();
                                }
                                receivers.Add(target);
                            }
                        }
                        break;
                    case NotificationTargetType.AllEmployees:
                        var employees = EmployeeService.GetEmployees(GlobalSettings.DefaultVendorId, EmployeeStatus.Published);
                        if (employees != null)
                        {
                            receivers.AddRange(employees.Select(emp => new NotificationTargetInfoDetail
                            {
                                TargetNo = emp.EmployeeNo,
                                TargetName = emp.Contact.FirstName + " " + emp.Contact.LastName,
                                ContactName = emp.Contact.DisplayName,
                                Mobile = emp.Contact.Mobile,
                                MailAddress = emp.Contact.Email,
                            }));
                        }
                        break;
                    case NotificationTargetType.Customer:
                        if (string.IsNullOrEmpty(targetId))
                        {
                            violations.Add(new RuleViolation(ErrorCode.NotFoundCustomer, "CustomerId", targetId,
                                   ErrorMessage.Messages[ErrorCode.NotFoundCustomer]));
                            throw new ValidationError(violations);
                        }

                        var customer = CustomerService.GetCustomerInfoDetail(Convert.ToInt32(targetId));
                        if (customer != null)
                        {
                            var target = new NotificationTargetInfoDetail
                            {
                                TargetNo = customer.CustomerNo,
                                TargetName = $"{customer.FirstName} {customer.LastName}",
                                ContactName = customer.ContactName,
                                Mobile = customer.Mobile,
                                MailAddress = customer.Email
                            };

                            if (customer.AddressId != null && customer.AddressId > 0)
                            {
                                var sbAddress = new StringBuilder();
                                if (customer.Address != null)
                                {
                                    if (!string.IsNullOrEmpty(customer.Address.Street))
                                    {
                                        sbAddress.AppendFormat("{0}", customer.Address.Street);
                                    }

                                    if (customer.Address.Region != null && !string.IsNullOrEmpty(customer.Address.Region.RegionName))
                                    {
                                        sbAddress.AppendFormat(", {0} {1}", LanguageResource.District, customer.Address.Region.RegionName);
                                    }

                                    if (customer.Address.Province != null && !string.IsNullOrEmpty(customer.Address.Province.ProvinceName))
                                    {
                                        sbAddress.AppendFormat(", {0} {1}", LanguageResource.Province, customer.Address.Province.ProvinceName);
                                    }

                                    if (customer.Address.Country != null && !string.IsNullOrEmpty(customer.Address.Country.CountryName))
                                    {
                                        sbAddress.AppendFormat(", {0}", customer.Address.Country.CountryName);
                                    }
                                }
                                target.Address = sbAddress.ToString();
                            }
                            receivers.Add(target);
                        }
                        break;
                    case NotificationTargetType.Employee:
                        if (string.IsNullOrEmpty(targetId))
                        {
                            violations.Add(new RuleViolation(ErrorCode.NotFoundEmployee, "EmployeeId", targetId,
                                   ErrorMessage.Messages[ErrorCode.NotFoundEmployee]));
                            throw new ValidationError(violations);
                        }

                        var employee = EmployeeService.GetEmployeeDetail(Convert.ToInt32(targetId));
                        if (employee == null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.NotFoundEmployee, "EmployeeId", targetId,
                                ErrorMessage.Messages[ErrorCode.NotFoundEmployee]));
                            throw new ValidationError(violations);
                        }
                        else
                        {
                            receivers.Add(new NotificationTargetInfoDetail
                            {
                                TargetNo = employee.EmployeeNo,
                                TargetName = employee.Contact.FirstName + " " + employee.Contact.LastName,
                                ContactName = employee.Contact.DisplayName,
                                Mobile = employee.Contact.Mobile,
                                MailAddress = employee.Contact.Email
                            });
                        }
                        break;
                }
            }
            return receivers;
        }
        #endregion

        #region Notification Message

        public NotificationMessageDetail InsertNotificationMessage(NotificationMessageEntry entry)
        {
            entry.Message.MessageId = entry.Message.MessageId ?? 0;

            var stringwriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(entry.Message.GetType());
            serializer.Serialize(stringwriter, entry.Message);
            var messageXml = stringwriter.ToString();

            var notificationMessage = new NotificationMessage
            {
                SentStatus = entry.SentStatus ?? NotificationSentStatus.Pending,
                MessageInfo = messageXml,
                PublishDate = entry.PublishDate ?? DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow,
                ExceptionMessage = entry.Message.ExceptionMessage
            };

            UnitOfWork.NotificationMessageRepository.Insert(notificationMessage);
            UnitOfWork.SaveChanges();

            return notificationMessage.ToDto<NotificationMessage, NotificationMessageDetail>();
        }

        public void UpdateNotificationMessageStatus(int id, MessageDetail messageInfo, NotificationSentStatus status)
        {
            var notificationMessage = UnitOfWork.NotificationMessageRepository.FindById(id);
            if (notificationMessage == null) return;

            var stringwriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(messageInfo.GetType());
            serializer.Serialize(stringwriter, messageInfo);
            var messageXml = stringwriter.ToString();

            notificationMessage.SentStatus = status;
            notificationMessage.MessageInfo = messageXml;
            notificationMessage.LastModifiedDate = DateTime.UtcNow;
            notificationMessage.ExceptionMessage = messageInfo.ExceptionMessage;

            UnitOfWork.NotificationMessageRepository.Update(notificationMessage);
            UnitOfWork.SaveChanges();
        }
        #endregion  

        #region Notification Type

        public IEnumerable<TreeNode> GetNotificationTypeTreeNode(NotificationTypeStatus? status, int? selectedId,
            bool? isRootShowed = false)
        {
            return UnitOfWork.NotificationTypeRepository.GetNotificationTypeTreeNode(status, selectedId, isRootShowed);
        }

        public IEnumerable<TreeGrid> GetNotificationTypeTreeGrid(NotificationTypeStatus? status, int? selectedId,
            bool? isRootShowed)
        {
            return UnitOfWork.NotificationTypeRepository.GetNotificationTypeTreeGrid(status, selectedId, isRootShowed);
        }

        public IEnumerable<TreeDetail> GetNotificationTypeSelectTree(NotificationTypeStatus? status,
            int? selectedId = null, bool? isRootShowed = true)
        {
            var lst = UnitOfWork.NotificationTypeRepository.GetNotificationTypeSelectTree(status, selectedId,
                isRootShowed);
            return lst.ToDtos<TreeEntity, TreeDetail>();
        }

        public NotificationTypeDetail GetNotificationTypeDetail(int id)
        {
            var lst = UnitOfWork.NotificationTypeRepository.FindById(id);
            return lst.ToDto<NotificationType, NotificationTypeDetail>();
        }

        public void InsertNotificationType(NotificationTypeEntry entry)
        {
            ISpecification<NotificationTypeEntry> validator = new NotificationTypeEntryValidator(UnitOfWork,
                PermissionLevel.Create, CurrentClaimsIdentity);
            var violations = new List<RuleViolation>();
            var isValid = validator.IsSatisfyBy(entry, violations);
            if (!isValid) throw new ValidationError(violations);

            var entity = entry.ToEntity<NotificationTypeEntry, NotificationType>();
            entity.HasChild = false;
            entity.ListOrder = UnitOfWork.NotificationTypeRepository.GetNewListOrder();
            entity.CreatedDate = DateTime.UtcNow;

            UnitOfWork.NotificationTypeRepository.Insert(entity);
            UnitOfWork.SaveChanges();

            if (entry.ParentId != null && entry.ParentId > 0)
            {
                var parentEntity = UnitOfWork.NotificationTypeRepository.FindById(Convert.ToInt32(entry.ParentId));
                if (parentEntity == null) return;

                parentEntity.HasChild = true;
                UnitOfWork.NotificationTypeRepository.Update(parentEntity);

                var lineage = $"{parentEntity.Lineage},{entity.NotificationTypeId}";
                entity.Lineage = lineage;
                entity.Depth = lineage.Split(',').Count();
                entity.ParentId = entry.ParentId;
            }
            else
            {
                entity.ParentId = 0;
                entity.Lineage = $"{entity.NotificationTypeId}";
                entity.Depth = 1;
            }
            UnitOfWork.NotificationTypeRepository.Update(entity);
            UnitOfWork.SaveChanges();

            if (entry.SelectedNotificationTargetTypes != null && entry.SelectedNotificationTargetTypes.Any())
            {
                InsertNotificationTargetTypes(entity.NotificationTypeId, entry.SelectedNotificationTargetTypes);
            }

            if (entry.MessageTypeIds != null && entry.MessageTypeIds.Any())
            {
                InsertNotificationMessageTypes(entity.NotificationTypeId, entry.MessageTypeIds);
            }
        }

        public void UpdateNotificationType(NotificationTypeEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.NotificationTypeRepository.FindById(entry.NotificationTypeId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundNotificationType, "NotificationTypeId", entry.NotificationTypeId,
                    ErrorMessage.Messages[ErrorCode.NotFoundNotificationType]));
                throw new ValidationError(violations);
            }

            if (string.IsNullOrEmpty(entry.NotificationTypeName))
            {
                violations.Add(new RuleViolation(ErrorCode.NullNotificationTypeName, "NotificationTypeName", null,
                    ErrorMessage.Messages[ErrorCode.NullNotificationTypeName]));
                throw new ValidationError(violations);
            }
            else
            {

                if (entity.NotificationTypeName != entry.NotificationTypeName)
                {
                    bool isDuplicate = UnitOfWork.NotificationTypeRepository.HasDataExisted(entry.NotificationTypeName,
                        entry.ParentId);
                    if (isDuplicate)
                    {
                        violations.Add(new RuleViolation(ErrorCode.DuplicateNotificationTypeName, "NotificationTypeName",
                            entry.NotificationTypeName));
                        throw new ValidationError(violations);
                    }
                }
            }

            if (entry.ParentId != entity.ParentId && entry.ParentId != entity.NotificationTypeId)
            {
                if (entry.ParentId != null && entry.ParentId > 0)
                {
                    var children =
                        UnitOfWork.NotificationTypeRepository.GetAllChildrenNodesOfSelectedNode(
                            Convert.ToInt32(entry.NotificationTypeId)).ToList();
                    if (children.Any())
                    {
                        var parentIds = children.Select(x => x.NotificationTypeId).ToList();
                        if (parentIds.Contains(Convert.ToInt32(entry.ParentId)))
                        {
                            violations.Add(new RuleViolation(ErrorCode.InvalidParentId, "ParentId", entry.ParentId,
                                ErrorMessage.Messages[ErrorCode.InvalidParentId]));
                            throw new ValidationError(violations);
                        }
                    }

                    //Update parent entry
                    var parentEntryEntity =
                        UnitOfWork.NotificationTypeRepository.FindById(Convert.ToInt32(entry.ParentId));
                    if (parentEntryEntity == null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.NotFoundParentId, "ParentId"));
                        throw new ValidationError(violations);
                    }
                    else
                    {
                        if (parentEntryEntity.HasChild == null || parentEntryEntity.HasChild == false)
                        {
                            parentEntryEntity.HasChild = true;
                            UnitOfWork.NotificationTypeRepository.Update(parentEntryEntity);
                        }
                    }

                    //Update parent entity
                    var parentEntity = UnitOfWork.NotificationTypeRepository.FindById(entity.ParentId);
                    if (parentEntity != null)
                    {
                        var childList =
                            UnitOfWork.NotificationTypeRepository.GetAllChildrenNodesOfSelectedNode(entity.ParentId)
                                .ToList();
                        if (childList.Any())
                        {
                            childList =
                                childList.Where(
                                    x =>
                                        (x.NotificationTypeId != entity.ParentId) &&
                                        (x.NotificationTypeId != entity.NotificationTypeId)).ToList();
                            parentEntity.HasChild = childList.Any();
                            UnitOfWork.NotificationTypeRepository.Update(parentEntity);
                        }
                    }

                    var lineage = $"{parentEntryEntity.Lineage},{entry.NotificationTypeId}";
                    entity.Lineage = lineage;
                    entity.Depth = lineage.Split(',').Count();
                    entity.ParentId = entry.ParentId;
                }
                else
                {
                    entity.ParentId = 0;
                    entity.Lineage = $"{entry.NotificationTypeId}";
                    entity.Depth = 1;
                }
            }

            //Update entity
            var hasChild = UnitOfWork.GalleryTopicRepository.HasChild(entity.NotificationTypeId);
            entity.HasChild = hasChild;
            entity.NotificationSenderTypeId = entry.NotificationSenderTypeId;
            entity.NotificationTypeName = entry.NotificationTypeName;
            entity.Description = entry.Description;
            entity.Status = entry.Status;
            entity.LastModifiedDate = DateTime.UtcNow;
            UnitOfWork.NotificationTypeRepository.Update(entity);
            UnitOfWork.SaveChanges();

            if (entry.SelectedNotificationTargetTypes != null && entry.SelectedNotificationTargetTypes.Any())
            {
                UpdateNotificationTargets(entity.NotificationTypeId, entry.SelectedNotificationTargetTypes);
            }
            else
            {
                DeleteNotificationTargetByNotificationTypeId(entity.NotificationTypeId);
            }

            if (entry.MessageTypeIds != null && entry.MessageTypeIds.Any())
            {
                UpdateNotificationMessageTypes(entity.NotificationTypeId, entry.MessageTypeIds);
            }
            else
            {
                DeleteNotificationMessageTypesByNotificationTypeId(entity.NotificationTypeId);
            }

        }

        public void UpdateNotificationTypeListOrder(int id, int listOrder)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.NotificationTypeRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullCategoryId, "CategoryId", id,
                    ErrorMessage.Messages[ErrorCode.NullCategoryId]));
                throw new ValidationError(violations);
            }

            if (entity.ListOrder == listOrder) return;

            entity.ListOrder = listOrder;
            entity.LastModifiedDate = DateTime.UtcNow;


            UnitOfWork.NotificationTypeRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateNotificationTypeStatus(int id, NotificationTypeStatus status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.NotificationTypeRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidNotificationTypeId, "NotificationTypeId", id,
                    ErrorMessage.Messages[ErrorCode.InvalidNotificationTypeId]));
                throw new ValidationError(violations);
            }

            var isValid = Enum.IsDefined(typeof(NotificationTypeStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status", null,
                    ErrorMessage.Messages[ErrorCode.InvalidStatus]));
                throw new ValidationError(violations);
            }
            if (entity.Status == status) return;

            entity.Status = status;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.NotificationTypeRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void Delete(int id)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.NotificationTypeRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundNotificationType, "CategoryId", id,
                    ErrorMessage.Messages[ErrorCode.NotFoundNotificationType]));
                throw new ValidationError(violations);
            }

            entity.Status = NotificationTypeStatus.InActive;
            UnitOfWork.NotificationTypeRepository.Update(entity);
        }

        #endregion

        #region Notification Target
        public IEnumerable<NotificationTargetDetail> GetNotificationTargets(int notificationTypeId)
        {
            var lst = UnitOfWork.NotificationTargetRepository.GetNotificationTargets(notificationTypeId);
            return lst.ToDtos<NotificationTarget, NotificationTargetDetail>();
        }
        public MultiSelectList PopulateAvailableNotificationTargetTypes(int? notificationTypeId = null)
        {
            return UnitOfWork.NotificationTargetRepository.PopulateAvailableNotificationTargetTypes(notificationTypeId);
        }
        public MultiSelectList PopulateSelectedNotificationTargetTypes(int? notificationTypeId = null)
        {
            return UnitOfWork.NotificationTargetRepository.PopulateSelectedNotificationTargetTypes(notificationTypeId);
        }

        private void InsertNotificationTargetTypes(int notificationTypeId, NotificationTargetType[] selectedNotificationTargetTypeIds)
        {
            if (notificationTypeId > 0 && selectedNotificationTargetTypeIds != null && selectedNotificationTargetTypeIds.Any())
            {
                foreach (var notificationTargetId in selectedNotificationTargetTypeIds)
                {
                    var entity = UnitOfWork.NotificationTargetRepository.GetDetails(notificationTypeId, notificationTargetId);
                    if (entity == null)
                    {
                        var notificationTarget = new NotificationTarget
                        {
                            NotificationTypeId = notificationTypeId,
                            NotificationTargetTypeId = notificationTargetId
                        };

                        UnitOfWork.NotificationTargetRepository.Insert(notificationTarget);
                        UnitOfWork.SaveChanges();
                    }
                }
            }
        }

        private void UpdateNotificationTargets(int notificationTypeId, NotificationTargetType[] latestNotificationTargetIds)
        {
            var notificationTargets = UnitOfWork.NotificationTargetRepository.GetNotificationTargets(notificationTypeId).ToList();

            if (notificationTargets.Any())
            {
                var previousNotificationTargetIds = notificationTargets.Select(x => x.NotificationTargetTypeId).ToList();

                //Get the elements in previous list b but not in latest list a - Except
                var exceptPreviousList = previousNotificationTargetIds.Except(latestNotificationTargetIds).ToList();
                if (exceptPreviousList.Count > 0)
                {
                    foreach (var notificationTargetId in exceptPreviousList)
                    {
                        var notificationTarget = UnitOfWork.NotificationTargetRepository.GetDetails(notificationTypeId, notificationTargetId);
                        if (notificationTarget == null) return;

                        UnitOfWork.NotificationTargetRepository.Delete(notificationTarget);
                        UnitOfWork.SaveChanges();
                    }
                }

                //Get the elements in latest list a but not in previous list b - Except
                var exceptLatestList = latestNotificationTargetIds.Except(previousNotificationTargetIds).ToArray();
                if (exceptLatestList.Any())
                {
                    InsertNotificationTargetTypes(notificationTypeId, exceptLatestList);
                }
            }
            else
            {
                InsertNotificationTargetTypes(notificationTypeId, latestNotificationTargetIds);
            }
        }

        private void DeleteNotificationTargetByNotificationTypeId(int notificationTypeId)
        {
            var targets = UnitOfWork.NotificationTargetRepository.GetNotificationTargets(notificationTypeId);
            if (targets == null) return;

            foreach (var item in targets)
            {
                var notificationTarget = new NotificationTarget
                {
                    NotificationTargetId = item.NotificationTargetId,
                    NotificationTypeId = item.NotificationTypeId,
                    NotificationTargetTypeId = item.NotificationTargetTypeId
                };
                UnitOfWork.NotificationTargetRepository.Delete(notificationTarget);
            }
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region Notification Target Default
        public IEnumerable<NotificationTargetDefaultInfoDetail> GetNotificationTargetDefaults(int? mailServerProviderId)
        {
            var lst = new List<NotificationTargetDefaultInfoDetail>();
            var senders = UnitOfWork.NotificationTargetDefaultRepository.GetNotificationTargetDefaults(mailServerProviderId).ToList();
            if (senders.Any())
            {
                lst.AddRange(senders.Select(item => new NotificationTargetDefaultInfoDetail
                {
                    NotificationTargetDefaultId = item.NotificationTargetDefaultId,
                    MailServerProviderId = item.MailServerProviderId,
                    TargetNo = item.TargetNo,
                    TargetName = item.TargetName,
                    ContactName = item.ContactName,
                    MailAddress = item.MailAddress,
                    Password = item.Password,
                    Address = item.Address,
                    IsActive = item.IsActive,
                    IsSelected = item.IsSelected,
                    MailServerProvider = item.MailServerProvider.ToDto<MailServerProvider, MailServerProviderDetail>()
                }));
            }
            return lst;
        }
        public NotificationTargetDefaultDetail GetNotificationTargetDefault()
        {
            var item = UnitOfWork.NotificationTargetDefaultRepository.GetNotificationTargetDefault();
            return item.ToDto<NotificationTargetDefault, NotificationTargetDefaultDetail>();
        }
        public NotificationTargetDefaultDetail GetNotificationTargetDefaultDetail(int id)
        {
            var item = UnitOfWork.NotificationTargetDefaultRepository.FindById(id);
            return item.ToDto<NotificationTargetDefault, NotificationTargetDefaultDetail>();
        }
        public NotificationTargetDefaultDetail InsertNotificationTargetDefault(NotificationTargetDefaultEntry entry)
        {
            ISpecification<NotificationTargetDefaultEntry> validator = new NotificationTargetDefaultEntryValidator(UnitOfWork, PermissionLevel.Edit, CurrentClaimsIdentity);
            var violations = new List<RuleViolation>();
            if (!validator.IsSatisfyBy(entry, violations)) throw new ValidationError(violations);

            var entity = entry.ToEntity<NotificationTargetDefaultEntry, NotificationTargetDefault>();
            entity.TargetNo = Convert.ToString(Guid.NewGuid());
            UnitOfWork.NotificationTargetDefaultRepository.Insert(entity);
            UnitOfWork.SaveChanges();
            return entity.ToDto<NotificationTargetDefault, NotificationTargetDefaultDetail>();
        }
        public void UpdateNotificationTargetDefault(NotificationTargetDefaultEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.NotificationTargetDefaultRepository.Find(entry.NotificationTargetDefaultId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundNotificationTargetDefault, "NotificationTargetDefault", null,
                    ErrorMessage.Messages[ErrorCode.NotFoundNotificationTargetDefault]));
                throw new ValidationError(violations);
            }

            if (entity.TargetName != entry.TargetName)
            {
                var isExisted = UnitOfWork.NotificationTargetDefaultRepository.HasTargetNameExisted(entry.TargetName);
                if (isExisted)
                {
                    violations.Add(new RuleViolation(ErrorCode.DuplicateTargetName, "TargetName",
                        entry.TargetName,
                        ErrorMessage.Messages[ErrorCode.DuplicateTargetName]));
                    throw new ValidationError(violations);
                }
            }

            if (entity.MailAddress != entry.MailAddress)
            {
                var isExisted = UnitOfWork.NotificationTargetDefaultRepository.HasMailAddressExisted(entry.MailAddress);
                if (isExisted)
                {
                    violations.Add(new RuleViolation(ErrorCode.DuplicateMailAddress, "MailAddress", entry.MailAddress,
               ErrorMessage.Messages[ErrorCode.DuplicateMailAddress]));
                    throw new ValidationError(violations);
                }
            }

            //Assign data
            entity.MailServerProviderId = entry.MailServerProviderId;
            entity.TargetName = entry.TargetName;
            entity.ContactName = entry.ContactName;
            entity.Mobile = entry.Mobile;
            entity.MailAddress = entry.MailAddress;
            entity.Password = entry.Password;
            entity.Address = entry.Address;
            entity.IsActive = entry.IsActive;

            UnitOfWork.NotificationTargetDefaultRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateNotificationTargetDefaultStatus(int id, bool status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.NotificationTargetDefaultRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundNotificationTargetDefaultId, "NotificationTargetDefaultId", id, ErrorMessage.Messages[ErrorCode.NotFoundNotificationTargetDefaultId]));
                throw new ValidationError(violations);
            }
            if (entity.IsActive == status) return;

            entity.IsActive = status;

            UnitOfWork.NotificationTargetDefaultRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateSelectedNotificationTargetDefault(int id)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.NotificationTargetDefaultRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundNotificationTargetDefault, "NotificationTargetDefaultId", id,
                    ErrorMessage.Messages[ErrorCode.NotFoundNotificationTargetDefault]));
                throw new ValidationError(violations);
            }

            if (entity.IsSelected) return;

            var lst = UnitOfWork.NotificationTargetDefaultRepository.GetNotificationTargetDefaults().ToList();
            if (!lst.Any()) return;

            foreach (var item in lst)
            {
                item.IsSelected = (item.NotificationTargetDefaultId == id);
                UnitOfWork.NotificationTargetDefaultRepository.Update(item);
                UnitOfWork.SaveChanges();
            }
        }
        #endregion

        #region Notification Message Type
        public IEnumerable<NotificationMessageTypeDetail> GetNotificationMessageTypes(int notificationTypeId)
        {
            var lst = UnitOfWork.NotificationMessageTypeRepository.GetNotificationMessageTypes(notificationTypeId);
            return lst.ToDtos<NotificationMessageType, NotificationMessageTypeDetail>();
        }
        public IEnumerable<NotificationMessageTypeInfoDetail> GetNotificationMessageTypeList(int notificationTypeId)
        {
            var lst = new List<NotificationMessageTypeInfoDetail>();
            var items = UnitOfWork.NotificationMessageTypeRepository.GetNotificationMessageTypeList(notificationTypeId).ToList();
            if (items.Any())
            {
                lst.AddRange(items.Select(x => new NotificationMessageTypeInfoDetail
                {
                    NotificationMessageTypeId = x.NotificationMessageTypeId,
                    NotificationTypeId = x.NotificationTypeId,
                    MessageTypeId = x.MessageTypeId,
                    MessageType = x.MessageType.ToDto<MessageType, MessageTypeDetail>(),
                    NotificationType = x.NotificationType.ToDto<NotificationType, NotificationTypeDetail>()
                }));
            }
            return lst;
        }
        private void InsertNotificationMessageTypes(int notificationTypeId, int[] messageTypeIds)
        {
            if (notificationTypeId > 0 && messageTypeIds != null && messageTypeIds.Any())
            {
                foreach (var messageTypeId in messageTypeIds)
                {
                    var entity = UnitOfWork.NotificationMessageTypeRepository.GetDetails(notificationTypeId, messageTypeId);
                    if (entity == null)
                    {
                        var notificationMessageType = new NotificationMessageType
                        {
                            NotificationTypeId = notificationTypeId,
                            MessageTypeId = messageTypeId
                        };

                        UnitOfWork.NotificationMessageTypeRepository.Insert(notificationMessageType);
                        UnitOfWork.SaveChanges();
                    }
                }
            }
        }

        private void UpdateNotificationMessageTypes(int notificationTypeId, int[] latestMessageTypeIds)
        {
            var notificationMessageTypes = UnitOfWork.NotificationMessageTypeRepository.GetNotificationMessageTypes(notificationTypeId).ToList();

            if (notificationMessageTypes.Any())
            {
                var previousMessageTypeIds = notificationMessageTypes.Select(x => x.MessageTypeId).ToList();

                //Get the elements in previous list b but not in latest list a - Except
                var exceptPreviousList = previousMessageTypeIds.Except(latestMessageTypeIds).ToList();
                if (exceptPreviousList.Count > 0)
                {
                    foreach (var messageTypeId in exceptPreviousList)
                    {
                        var messageType = UnitOfWork.NotificationMessageTypeRepository.GetDetails(notificationTypeId,
                            messageTypeId);
                        if (messageType != null)
                        {
                            UnitOfWork.NotificationMessageTypeRepository.Delete(messageType);
                            UnitOfWork.SaveChanges();
                        }
                    }
                }

                //Get the elements in latest list a but not in previous list b - Except
                var exceptLatestList = latestMessageTypeIds.Except(previousMessageTypeIds).ToArray();
                if (exceptLatestList.Any())
                {
                    InsertNotificationMessageTypes(notificationTypeId, exceptLatestList);
                }
            }
            else
            {
                InsertNotificationMessageTypes(notificationTypeId, latestMessageTypeIds);
            }
        }

        private void DeleteNotificationMessageTypesByNotificationTypeId(int notificationTypeId)
        {
            var lst = UnitOfWork.NotificationMessageTypeRepository.GetNotificationMessageTypes(notificationTypeId);
            if (lst == null) return;

            foreach (var item in lst)
            {
                var notificationMessageType = new NotificationMessageType
                {
                    NotificationMessageTypeId = item.NotificationMessageTypeId,
                    NotificationTypeId = item.NotificationTypeId,
                    MessageTypeId = item.MessageTypeId
                };
                UnitOfWork.NotificationMessageTypeRepository.Delete(notificationMessageType);
            }
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region Notification Sender
        public NotificationSenderDetail GetNotificationSenderDetail(NotificationSenderType senderTypeId, string senderId = null)
        {
            var violations = new List<RuleViolation>();
            var sender = new NotificationSenderDetail();
            switch (senderTypeId)
            {
                case NotificationSenderType.System:
                    var item = UnitOfWork.NotificationSenderRepository.GetDefaultNotificationSender();
                    if (item == null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.NotFoundNotificationSender, "NotificationSender", null,
                                ErrorMessage.Messages[ErrorCode.NotFoundNotificationSender]));
                        throw new ValidationError(violations);
                    }
                    else
                    {
                        sender.NotificationSenderId = item.NotificationSenderId;
                        sender.MailServerProviderId = item.MailServerProviderId;
                        sender.SenderNo = item.SenderNo;
                        sender.SenderName = item.SenderName;
                        sender.ContactName = item.ContactName;
                        sender.Mobile = item.Mobile;
                        sender.MailAddress = item.MailAddress;
                        sender.Password = item.Password;
                        sender.Signature = item.Signature;
                        sender.IsActive = item.IsActive;
                    }
                    break;
                case NotificationSenderType.User:
                    if (!string.IsNullOrEmpty(senderId))
                    {
                        var user = UserService.GetProfileDetails(Guid.Parse(senderId));
                        if (user == null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.NotFoundUser, "UserId", senderId,
                                ErrorMessage.Messages[ErrorCode.NotFoundUser]));
                            throw new ValidationError(violations);
                        }
                        else
                        {
                            sender.SenderNo = user.User.UserId.ToString();
                            sender.SenderName = user.Contact.FullName;
                            sender.ContactName = user.Contact.DisplayName;
                            sender.MailAddress = user.Contact.Email;
                            sender.Mobile = user.Contact.Mobile;
                        }
                    }
                    else
                    {
                        violations.Add(new RuleViolation(ErrorCode.NotFoundUserId, "UserId", senderId,
                              ErrorMessage.Messages[ErrorCode.NotFoundUserId]));
                        throw new ValidationError(violations);
                    }
                    break;
                case NotificationSenderType.Customer:
                    if (!string.IsNullOrEmpty(senderId))
                    {
                        var customer = CustomerService.GetCustomerDetail(Convert.ToInt32(senderId));
                        if (customer == null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.NotFoundCustomer, "CustomerId", senderId,
                                ErrorMessage.Messages[ErrorCode.NotFoundCustomer]));
                            throw new ValidationError(violations);
                        }
                        else
                        {
                            sender.SenderNo = customer.CustomerNo;
                            sender.SenderName = $"{customer.FirstName} {customer.LastName}";
                            sender.ContactName = customer.ContactName;
                            sender.MailAddress = customer.Email;
                            sender.Mobile = customer.Mobile;
                        }
                    }
                    else
                    {
                        violations.Add(new RuleViolation(ErrorCode.NotFoundCustomer, "CustomerId", senderId,
                                 ErrorMessage.Messages[ErrorCode.NotFoundCustomer]));
                        throw new ValidationError(violations);
                    }
                    break;
                case NotificationSenderType.Employee:
                    if (!string.IsNullOrEmpty(senderId))
                    {
                        var employee = EmployeeService.GetEmployeeDetail(Convert.ToInt32(senderId));
                        if (employee == null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.NotFoundEmployee, "EmployeeId", senderId,
                                ErrorMessage.Messages[ErrorCode.NotFoundEmployee]));
                            throw new ValidationError(violations);
                        }
                        else
                        {
                            sender.SenderNo = employee.EmployeeNo;
                            sender.SenderName = employee.Contact.FirstName + " " + employee.Contact.LastName;
                            sender.ContactName = employee.Contact.DisplayName;
                            sender.MailAddress = employee.Contact.Email;
                            sender.Mobile = employee.Contact.Mobile;
                        }
                    }
                    break;
                default:
                    var senderInfo = UnitOfWork.NotificationSenderRepository.GetDefaultNotificationSender();
                    if (senderInfo == null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.NotFoundNotificationSender, "NotificationSender", null,
                                ErrorMessage.Messages[ErrorCode.NotFoundNotificationSender]));
                        throw new ValidationError(violations);
                    }
                    else
                    {
                        sender.NotificationSenderId = senderInfo.NotificationSenderId;
                        sender.MailServerProviderId = senderInfo.MailServerProviderId;
                        sender.SenderNo = senderInfo.SenderNo;
                        sender.SenderName = senderInfo.SenderName;
                        sender.ContactName = senderInfo.ContactName;
                        sender.Mobile = senderInfo.Mobile;
                        sender.MailAddress = senderInfo.MailAddress;
                        sender.Password = senderInfo.Password;
                        sender.Signature = senderInfo.Signature;
                        sender.IsActive = senderInfo.IsActive;
                    }
                    break;
            }
            return sender;
        }
        public IEnumerable<NotificationSenderInfoDetail> GetNotificationSenders(int? mailServerProviderId)
        {
            var lst = new List<NotificationSenderInfoDetail>();
            var senders = UnitOfWork.NotificationSenderRepository.GetNotificationSenders(mailServerProviderId).ToList();
            if (senders.Any())
            {
                lst.AddRange(senders.Select(item => new NotificationSenderInfoDetail
                {
                    NotificationSenderId = item.NotificationSenderId,
                    MailServerProviderId = item.MailServerProviderId,
                    SenderNo = item.SenderNo,
                    SenderName = item.SenderName,
                    ContactName = item.ContactName,
                    MailAddress = item.MailAddress,
                    Password = item.Password,
                    Signature = item.Signature,
                    IsActive = item.IsActive,
                    IsSelected = item.IsSelected,
                    MailServerProvider = item.MailServerProvider.ToDto<MailServerProvider, MailServerProviderDetail>()
                }));
            }
            return lst;
        }

        public SelectList PopulateNotificationSenderSelectList(int? mailServerProviderId, int? selectedValue = null, bool? isShowSelectText = true)
        {
            return UnitOfWork.NotificationSenderRepository.PopulateNotificationSenderSelectList(mailServerProviderId, selectedValue, isShowSelectText);
        }
        
        public NotificationSenderDetail GetNotificationSenderDetail(int id)
        {
            var item = UnitOfWork.NotificationSenderRepository.FindById(id);
            return item.ToDto<NotificationSender, NotificationSenderDetail>();
        }
        public NotificationSenderDetail InsertNotificationSender(NotificationSenderEntry entry)
        {
            ISpecification<NotificationSenderEntry> validator = new NotificationSenderEntryValidator(UnitOfWork, PermissionLevel.Edit, CurrentClaimsIdentity);
            var violations = new List<RuleViolation>();
            if (!validator.IsSatisfyBy(entry, violations)) throw new ValidationError(violations);

            var entity = entry.ToEntity<NotificationSenderEntry, NotificationSender>();
            entity.SenderNo = Convert.ToString(Guid.NewGuid());
            UnitOfWork.NotificationSenderRepository.Insert(entity);
            UnitOfWork.SaveChanges();
            return entity.ToDto<NotificationSender, NotificationSenderDetail>();
        }
        public void UpdateNotificationSender(NotificationSenderEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.NotificationSenderRepository.Find(entry.NotificationSenderId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundNotificationSender, "NotificationSender", null,
                    ErrorMessage.Messages[ErrorCode.NotFoundNotificationSender]));
                throw new ValidationError(violations);
            }

            if (entity.SenderName != entry.SenderName)
            {
                var isExisted = UnitOfWork.NotificationSenderRepository.HasSenderNameExisted(entry.SenderName);
                if (isExisted)
                {
                    violations.Add(new RuleViolation(ErrorCode.DuplicateSenderName, "SenderName",
                        entry.SenderName,
                        ErrorMessage.Messages[ErrorCode.DuplicateSenderName]));
                    throw new ValidationError(violations);
                }
            }

            if (entity.MailAddress != entry.MailAddress)
            {
                var isExisted = UnitOfWork.NotificationSenderRepository.HasMailAddressExisted(entry.MailAddress);
                if (isExisted)
                {
                    violations.Add(new RuleViolation(ErrorCode.DuplicateMailAddress, "MailAddress", entry.MailAddress,
               ErrorMessage.Messages[ErrorCode.DuplicateMailAddress]));
                    throw new ValidationError(violations);
                }
            }

            //Assign data
            entity.MailServerProviderId = entry.MailServerProviderId;
            entity.SenderName = entry.SenderName;
            entity.ContactName = entry.ContactName;
            entity.Mobile = entry.Mobile;
            entity.MailAddress = entry.MailAddress;
            entity.Password = entry.Password;
            entity.Signature = entry.Signature;
            entity.IsActive = entry.IsActive;

            UnitOfWork.NotificationSenderRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateNotificationSenderStatus(int id, bool status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.NotificationSenderRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundNotificationSenderId, "NotificationSenderId", id, ErrorMessage.Messages[ErrorCode.NotFoundNotificationSenderId]));
                throw new ValidationError(violations);
            }
            if (entity.IsActive == status) return;

            entity.IsActive = status;

            UnitOfWork.NotificationSenderRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateSelectedNotificationSender(int id)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.NotificationSenderRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundNotificationSender, "NotificationSenderId", id,
                    ErrorMessage.Messages[ErrorCode.NotFoundNotificationSender]));
                throw new ValidationError(violations);
            }

            if (entity.IsSelected) return;

            var lst = UnitOfWork.NotificationSenderRepository.GetNotificationSenders().ToList();
            if (!lst.Any()) return;

            foreach (var item in lst)
            {
                item.IsSelected = (item.NotificationSenderId == id);
                UnitOfWork.NotificationSenderRepository.Update(item);
                UnitOfWork.SaveChanges();
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
                    CustomerService = null;
                    EmployeeService = null;
                    MailService = null;
                    MessageService = null;
                    UserService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}
