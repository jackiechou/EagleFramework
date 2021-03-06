﻿using System.Collections.Generic;
using System.Security.Claims;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Services.Event;
using Eagle.Services.Validations;

namespace Eagle.Services.Service.Validation
{
    public class EventTypeEntryValidator : SpecificationBase<EventTypeEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }
        public EventTypeEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel, ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(EventTypeEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundEventTypeEntry, "EventTypeEntry"));
                return false;
            }

            ISpecification<EventTypeEntry> isValidName = new HasValidName();
            ISpecification<EventTypeEntry> isValidParentId = new HasValidParentId();
            return isValidName.And(isValidParentId).IsSatisfyBy(data, violations);
        }

        private class HasValidName : SpecificationBase<EventTypeEntry>
        {
            protected override bool IsSatisfyBy(EventTypeEntry data, IList<RuleViolation> violations = null)
            {
                var isValid = (!string.IsNullOrEmpty(data.TypeName) || data.TypeName.Length > 300);
                if (!isValid && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidTypeName, "TypeName", null, ErrorMessage.Messages[ErrorCode.InvalidTypeName]));
                    return false;
                }
                else
                {
                    bool isDuplicate = UnitOfWork.NewsCategoryRepository.HasDataExisted(data.TypeName, data.ParentId);
                    if (isDuplicate)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicateTypeName, "TypeName", data.TypeName, ErrorMessage.Messages[ErrorCode.DuplicateTypeName]));
                            return false;
                        }
                    }
                    return true;
                }
            }
        }

        public class HasValidParentId : SpecificationBase<EventTypeEntry>
        {
            protected override bool IsSatisfyBy(EventTypeEntry data, IList<RuleViolation> violations = null)
            {
                if (data.ParentId != null && data.ParentId >= int.MaxValue && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidParentId, "ParentId", data.ParentId, ErrorMessage.Messages[ErrorCode.InvalidParentId]));
                    return false;
                }
                return true;
            }
        }

    }
}
