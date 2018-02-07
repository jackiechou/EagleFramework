using System.Collections.Generic;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.SystemManagement;
using Eagle.Services.Validations;

namespace Eagle.Services.SystemManagement.Validation
{
    public class ApplicationSettingEntryValidator : SpecificationBase<ApplicationSettingEntry>
    {
        private static IUnitOfWork UnitOfWork { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        public ApplicationSettingEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
        }

        protected override bool IsSatisfyBy(ApplicationSettingEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null)
            {
                if (violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NotFoundApplicationSetting, "ApplicationSetting"));
                    return false;
                }
            }
            ISpecification<ApplicationSettingEntry> hasValidIsValidSettingName = new HasValidSettingName();
            ISpecification<ApplicationSettingEntry> hasValidIsValidSettingValue = new HasValidSettingValue();

            return hasValidIsValidSettingName.And(hasValidIsValidSettingValue).IsSatisfyBy(data, violations);
        }

        internal class HasValidSettingName : SpecificationBase<ApplicationSettingEntry>
        {
            protected override bool IsSatisfyBy(ApplicationSettingEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.SettingName))
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidSettingName));
                        return false;
                    }
                }
                else
                {
                    if (data.SettingName.Length > 250)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.InvalidSettingName, "SettingName",
                                data.SettingName));
                            return false;
                        }
                    }
                    else
                    {
                        bool isDuplicate = UnitOfWork.ApplicationSettingRepository.HasDataExisted(data.SettingName);
                        if (isDuplicate)
                        {
                            if (violations != null)
                            {
                                violations.Add(new RuleViolation(ErrorCode.DuplicateSettingName, "SettingName",
                                    data.SettingName));
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
        }

        internal class HasValidSettingValue : SpecificationBase<ApplicationSettingEntry>
        {
            protected override bool IsSatisfyBy(ApplicationSettingEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.SettingValue))
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidSettingValue));
                        return false;
                    }
                }
                else
                {
                    if (data.SettingName.Length > 2000)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.InvalidSettingValue, data.SettingValue));
                            return false;
                        }
                    }
                }
                return true;
            }
        }
    }
}
