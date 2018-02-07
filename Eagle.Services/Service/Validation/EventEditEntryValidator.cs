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
    public class EventEditEntryValidator : SpecificationBase<EventEditEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }
        private PermissionLevel PermissionLevel { get; set; }

        public EventEditEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel,
            ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(EventEditEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullEventEditEntry, "EventEditEntry", null,
                    ErrorMessage.Messages[ErrorCode.NullEventEditEntry]));
                return false;
            }

            ISpecification<EventEditEntry> hasValidTypeId = new HasValidTypeId();
            ISpecification<EventEditEntry> hasValidCode = new HasValidCode();
            ISpecification<EventEditEntry> hasValidTitle = new HasValidTitle();
            ISpecification<EventEditEntry> hasValidEventMessage = new HasValidEventMessage();
            ISpecification<EventEditEntry> hasValidStatus = new HasValidStatus();

            return hasValidTypeId.And(hasValidCode).And(hasValidTitle).And(hasValidEventMessage).And(hasValidStatus).IsSatisfyBy(data, violations);
        }

        private class HasValidTypeId : SpecificationBase<EventEditEntry>
        {
            protected override bool IsSatisfyBy(EventEditEntry data, IList<RuleViolation> violations = null)
            {
                var result = data.TypeId <= 0;
                if (result && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidTypeId, "TypeId", data.TypeId,
                        ErrorMessage.Messages[ErrorCode.InvalidTypeId]));
                    return false;
                }
                return true;
            }
        }

        private class HasValidCode : SpecificationBase<EventEditEntry>
        {
            protected override bool IsSatisfyBy(EventEditEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.EventCode) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullEventCode, "EventCode", null,
                        ErrorMessage.Messages[ErrorCode.NullEventCode]));
                    return false;
                }
                else
                {
                    if (data.EventCode.Length > 50 && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidEventCode, "EventCode", data.EventCode,
                            ErrorMessage.Messages[ErrorCode.InvalidEventCode]));
                        return false;
                    }
                    return true;
                }
            }
        }

        private class HasValidTitle : SpecificationBase<EventEditEntry>
        {
            protected override bool IsSatisfyBy(EventEditEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.EventTitle) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullTitle, "Title", null,
                        ErrorMessage.Messages[ErrorCode.NullTitle]));
                    return false;
                }
                else
                {
                    if (data.EventTitle.Length > 300 && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidTitle, "Title", data.EventTitle,
                            ErrorMessage.Messages[ErrorCode.InvalidTitle]));
                        return false;
                    }
                    return true;
                }
            }
        }

        private class HasValidEventMessage : SpecificationBase<EventEditEntry>
        {
            protected override bool IsSatisfyBy(EventEditEntry data, IList<RuleViolation> violations = null)
            {
                var result = string.IsNullOrEmpty(data.EventMessage);
                if (result && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvaliEventMessage, "EventMessage", null,
                        ErrorMessage.Messages[ErrorCode.InvaliEventMessage]));
                    return false;
                }
                return true;
            }
        }
        private class HasValidStatus : SpecificationBase<EventEditEntry>
        {
            protected override bool IsSatisfyBy(EventEditEntry data, IList<RuleViolation> violations = null)
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
