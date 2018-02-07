using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Core.Extension;
using Eagle.Core.Settings;
using Eagle.Entities.Services.Messaging;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Services.Message;

namespace Eagle.Services.Messaging
{
    public interface INotificationService : IBaseService
    {
        #region Notificaiotn

        List<NotificationTargetInfoDetail> GetNotificationTargets(int notificationTypeId, string targetId);
        
        #endregion

        #region Notification Message


        IEnumerable<NotificationMessageTypeDetail> GetNotificationMessageTypes(int notificationTypeId);
        IEnumerable<NotificationMessageTypeInfoDetail> GetNotificationMessageTypeList(int notificationTypeId);
        NotificationMessageDetail InsertNotificationMessage(NotificationMessageEntry entry);
        void UpdateNotificationMessageStatus(int id, MessageDetail messageInfo, NotificationSentStatus status);
        #endregion

        #region Notification Type

        IEnumerable<TreeNode> GetNotificationTypeTreeNode(NotificationTypeStatus? status, int? selectedId,
            bool? isRootShowed = false);
        IEnumerable<TreeGrid> GetNotificationTypeTreeGrid(NotificationTypeStatus? status, int? selectedId,
            bool? isRootShowed);
        IEnumerable<TreeDetail> GetNotificationTypeSelectTree(NotificationTypeStatus? status, int? selectedId = null, bool? isRootShowed = true);
        NotificationTypeDetail GetNotificationTypeDetail(int id);
        
        void InsertNotificationType(NotificationTypeEntry entry);
        void UpdateNotificationType(NotificationTypeEditEntry entry);
        void UpdateNotificationTypeListOrder(int id, int listOrder);
        void UpdateNotificationTypeStatus(int id, NotificationTypeStatus status);
        void Delete(int id);

        #endregion

        #region Notification Target

        IEnumerable<NotificationTargetDetail> GetNotificationTargets(int notificationTypeId);
        MultiSelectList PopulateAvailableNotificationTargetTypes(int? notificationTypeId = null);
        MultiSelectList PopulateSelectedNotificationTargetTypes(int? notificationTypeId = null);

        #endregion

        #region Notification Target Default

        IEnumerable<NotificationTargetDefaultInfoDetail> GetNotificationTargetDefaults(int? mailServerProviderId);
        NotificationTargetDefaultDetail GetNotificationTargetDefault();
        NotificationTargetDefaultDetail GetNotificationTargetDefaultDetail(int id);
        NotificationTargetDefaultDetail InsertNotificationTargetDefault(NotificationTargetDefaultEntry entry);
        void UpdateNotificationTargetDefault(NotificationTargetDefaultEditEntry entry);
        void UpdateNotificationTargetDefaultStatus(int id, bool status);
        void UpdateSelectedNotificationTargetDefault(int id);
        #endregion

        #region Notification Sender
        NotificationSenderDetail GetNotificationSenderDetail(NotificationSenderType senderTypeId, string senderId = null);
        IEnumerable<NotificationSenderInfoDetail> GetNotificationSenders(int? mailServerProviderId);
        SelectList PopulateNotificationSenderSelectList(int? mailServerProviderId, int? selectedValue = null,
            bool? isShowSelectText = true);
        NotificationSenderDetail GetNotificationSenderDetail(int id);
        NotificationSenderDetail InsertNotificationSender(NotificationSenderEntry entry);
        void UpdateNotificationSender(NotificationSenderEditEntry entry);
        void UpdateNotificationSenderStatus(int id, bool status);
        void UpdateSelectedNotificationSender(int id);

        #endregion
    }
}
