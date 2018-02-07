using System;
using System.Collections.Generic;
using System.Security.Claims;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Resources;
using Eagle.Services.Dtos.Business.Personnel;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Validations;

namespace Eagle.Services.Business.Validation
{
    public class EmployeeTimeOffEntryValidator : SpecificationBase<EmployeeTimeOffEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }
        private PermissionLevel PermissionLevel { get; set; }

        public EmployeeTimeOffEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel, ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(EmployeeTimeOffEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullEmployeeTimeOffEntry, "EmployeeTimeOffEntry", null, ErrorMessage.Messages[ErrorCode.NullEmployeeTimeOffEntry]));
                return false;
            }

            //ISpecification<EmployeeTimeOffEntry> hasValidPermission = new PermissionValidator<EmployeeTimeOffEntry>
            //    (CurrentClaimsIdentity, SherpaPermissionCapabilities.RECRUITMENT, Permission);
            ISpecification<EmployeeTimeOffEntry> hasValidEmployeeId = new HasValidEmployeeId();
            ISpecification<EmployeeTimeOffEntry> hasValidTimeOffTypeId = new HasValidTimeOffTypeId();
            ISpecification<EmployeeTimeOffEntry> hasValidTimeZoneId = new HasValidTimeZoneId();
            ISpecification<EmployeeTimeOffEntry> hasValidDate = new HasValidDate();
            ISpecification<EmployeeTimeOffEntry> hasValidReason = new HasValidReason();

            var result = hasValidEmployeeId.And(hasValidTimeOffTypeId).And(hasValidTimeZoneId).And(hasValidDate)
                .And(hasValidReason).IsSatisfyBy(data, violations);

            return result;
        }

        private class HasValidEmployeeId : SpecificationBase<EmployeeTimeOffEntry>
        {
            protected override bool IsSatisfyBy(EmployeeTimeOffEntry data, IList<RuleViolation> violations = null)
            {
                var employee = UnitOfWork.EmployeeTimeOffRepository.FindById(data.EmployeeId);
                if (employee == null && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullEmployee, "EmployeeId",
                            data.EmployeeId, ErrorMessage.Messages[ErrorCode.NullEmployee]));
                    return false;
                }
                return true;
            }
        }
        private class HasValidTimeOffTypeId : SpecificationBase<EmployeeTimeOffEntry>
        {
            protected override bool IsSatisfyBy(EmployeeTimeOffEntry data, IList<RuleViolation> violations = null)
            {
                var timeOffType = UnitOfWork.TimeOffTypeRepository.FindById(data.TimeOffTypeId);
                if (timeOffType == null && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullTimeZoneId, "TimeZoneId", null, ErrorMessage.Messages[ErrorCode.NullTimeZoneId]));
                    return false;
                }
                return true;
            }
        }
        private class HasValidTimeZoneId : SpecificationBase<EmployeeTimeOffEntry>
        {
            protected override bool IsSatisfyBy(EmployeeTimeOffEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.TimeZoneId) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullTimeZoneId, "TimeZoneId", null, ErrorMessage.Messages[ErrorCode.NullTimeZoneId]));
                    return false;
                }
                return true;
            }
        }
        private class HasValidDate : SpecificationBase<EmployeeTimeOffEntry>
        {
            protected override bool IsSatisfyBy(EmployeeTimeOffEntry data, IList<RuleViolation> violations = null)
            {
                if (data.StartDate.Date < DateTime.UtcNow.Date && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidStartDate, "StartDate", data.StartDate, ErrorMessage.Messages[ErrorCode.InvalidStartDate]));
                    return false;
                }

                if (data.EndDate < DateTime.UtcNow.Date && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidEndDate, "EndDate", data.EndDate,
                        ErrorMessage.Messages[ErrorCode.InvalidEndDate]));
                    return false;
                }
                else
                {
                    if (data.EndDate < data.StartDate && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.EndDateMustBeGreaterThanStartDate, "EndDate",
                            $"{LanguageResource.StartDate} : {data.StartDate} vs {LanguageResource.EndDate} : {data.EndDate}",
                       ErrorMessage.Messages[ErrorCode.EndDateMustBeGreaterThanStartDate]));
                        return false;
                    }
                }
                return true;
            }
        }
        private class HasValidReason : SpecificationBase<EmployeeTimeOffEntry>
        {
            protected override bool IsSatisfyBy(EmployeeTimeOffEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.Reason) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullReason, "Reason", null, ErrorMessage.Messages[ErrorCode.NullReason]));
                    return false;
                }
                return true;
            }
        }

    }
}
