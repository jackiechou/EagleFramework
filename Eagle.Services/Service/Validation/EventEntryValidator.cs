using System;
using System.Collections.Generic;
using System.Security.Claims;
using Eagle.Core.Permission;
using Eagle.Core.Settings;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Services.Event;
using Eagle.Services.Validations;

namespace Eagle.Services.Service.Validation
{
    public class EventEntryValidator : SpecificationBase<EventEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        public EventEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel, ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(EventEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundEventEntry, "EventEntry",null, ErrorMessage.Messages[ErrorCode.InvalidTypeId]));
                return false;
            }

            ISpecification<EventEntry> hasValidTypeId = new HasValidTypeId();
            ISpecification<EventEntry> hasValidCode = new HasValidCode();
            ISpecification<EventEntry> hasValidTitle = new HasValidTitle();
            ISpecification<EventEntry> hasValidEventMessage = new HasValidEventMessage();
            ISpecification<EventEntry> hasValidStatus = new HasValidStatus();
            
            return hasValidTypeId.And(hasValidCode).And(hasValidTitle).And(hasValidEventMessage).And(hasValidStatus).IsSatisfyBy(data, violations);
        }

        private class HasValidTypeId : SpecificationBase<EventEntry>
        {
            protected override bool IsSatisfyBy(EventEntry data, IList<RuleViolation> violations = null)
            {
                if (data.TypeId <= 0 && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidTypeId, "TypeId", data.TypeId,
                        ErrorMessage.Messages[ErrorCode.InvalidTypeId]));
                    return false;
                }
                else
                {
                    var eventType = UnitOfWork.EventTypeRepository.Find(data.TypeId);
                    if (eventType == null && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidEventTypeId, "EventTypeEditEntry", data.TypeId, ErrorMessage.Messages[ErrorCode.InvalidEventTypeId]));
                        return false;
                    }
                }
                return true;
            }
        }
        private class HasValidCode : SpecificationBase<EventEntry>
        {
            protected override bool IsSatisfyBy(EventEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.EventCode) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullEventCode, "EventCode", null, ErrorMessage.Messages[ErrorCode.NullEventCode]));
                    return false;
                }
                else
                {
                    if (data.EventCode.Length > 50 && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidEventCode, "EventCode", data.EventCode, ErrorMessage.Messages[ErrorCode.InvalidEventCode]));
                        return false;
                    }

                    var isDuplicated = UnitOfWork.EventRepository.HasCodeExisted(data.EventCode);
                    if (isDuplicated && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.DuplicateEventCode, "EventCode", data.EventTitle, ErrorMessage.Messages[ErrorCode.DuplicateEventCode]));
                        return false;
                    }
                    return true;
                }
            }
        }
        private class HasValidTitle : SpecificationBase<EventEntry>
        {
            protected override bool IsSatisfyBy(EventEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.EventTitle) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullTitle, "EventTitle", null, ErrorMessage.Messages[ErrorCode.NullTitle]));
                    return false;
                }
                else
                {
                    if (data.EventTitle.Length > 300 && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidTitle, "EventTitle", data.EventTitle, ErrorMessage.Messages[ErrorCode.InvalidTitle]));
                        return false;
                    }

                    var isDuplicated = UnitOfWork.EventRepository.HasDataExisted(data.TypeId, data.EventTitle);
                    if (isDuplicated && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.DuplicateTitle, "EventTitle", data.EventTitle, ErrorMessage.Messages[ErrorCode.DuplicateTitle]));
                        return false;
                    }
                    return true;
                }
            }
        }
        private class HasValidEventMessage : SpecificationBase<EventEntry>
        {
            protected override bool IsSatisfyBy(EventEntry data, IList<RuleViolation> violations = null)
            {
                var result = string.IsNullOrEmpty(data.EventMessage);
                if (result && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvaliEventMessage, "EventMessage", null, ErrorMessage.Messages[ErrorCode.InvaliEventMessage]));
                    return false;
                }
                return true;
            }
        }
        private class HasValidStatus : SpecificationBase<EventEntry>
        {
            protected override bool IsSatisfyBy(EventEntry data, IList<RuleViolation> violations = null)
            {
                if (!Enum.IsDefined(typeof(EventStatus), data.Status) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status", data.Status, ErrorMessage.Messages[ErrorCode.InvalidStatus]));
                    return false;
                }
                return true;
            }
        }
    }
}
