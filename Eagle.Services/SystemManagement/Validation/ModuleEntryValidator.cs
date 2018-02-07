using System;
using System.Collections.Generic;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Resources;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.SystemManagement;
using Eagle.Services.Validations;

namespace Eagle.Services.SystemManagement.Validation
{
    public class ModuleEntryValidator : SpecificationBase<ModuleEntry>
    {
        private static IUnitOfWork UnitOfWork { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        public ModuleEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
        }

        protected override bool IsSatisfyBy(ModuleEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null)
            {
                if (violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullModuleEntry, "ModuleEntry", null, ErrorMessage.Messages[ErrorCode.NullModuleEntry]));
                    return false;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(data.ModuleName) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullModuleName, "ModuleName", data.ModuleName,
                        ErrorMessage.Messages[ErrorCode.NullModuleName]));
                    return false;
                }
                else
                {
                    if (data.ModuleName.Length > 256 && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidModuleName, "ModuleName", data.ModuleName,
                            ErrorMessage.Messages[ErrorCode.InvalidModuleName]));
                        return false;
                    }
                    else
                    {
                        bool flag = UnitOfWork.ModuleRepository.HasModuleNameExisted(data.ModuleName);
                        if (flag && violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicateModuleName, "ModuleName",
                                data.ModuleName, ErrorMessage.Messages[ErrorCode.NullModuleName]));
                            return false;
                        }
                    }
                }

                if (string.IsNullOrEmpty(data.ModuleTitle) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullModuleTitle, "ModuleTitle", data.ModuleTitle, ErrorMessage.Messages[ErrorCode.NullModuleTitle]));
                    return false;
                }
                else
                {
                    if (data.ModuleTitle.Length > 256 && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidModuleTitle, "ModuleTitle", data.ModuleTitle,
                            ErrorMessage.Messages[ErrorCode.InvalidModuleTitle]));
                        return false;
                    }
                    else
                    {
                        bool flag = UnitOfWork.ModuleRepository.HasModuleTitleExisted(data.ModuleTitle);
                        if (flag && violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicateModuleTitle, "ModuleTitle",
                                data.ModuleTitle, ErrorMessage.Messages[ErrorCode.DuplicateModuleTitle]));
                            return false;
                        }
                    }
                }

                if (string.IsNullOrEmpty(data.ModuleCode) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullModuleCode, "ModuleCode", data.ModuleCode, ErrorMessage.Messages[ErrorCode.NullModuleCode]));
                    return false;
                }
                else
                {
                    if (data.ModuleCode.Length > 250 && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidModuleCode, "ModuleCode", data.ModuleCode,
                            ErrorMessage.Messages[ErrorCode.InvalidModuleCode]));
                        return false;
                    }
                    else
                    {
                        bool flag = UnitOfWork.ModuleRepository.HasModuleCodeExisted(data.ModuleCode);
                        if (flag && violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicateModuleCode, "ModuleCode",
                                data.ModuleCode, ErrorMessage.Messages[ErrorCode.DuplicateModuleCode]));
                            return false;
                        }
                    }
                }

                DateTime? startDate = data.StartDate;
                DateTime? endDate = data.EndDate;
                if (startDate.HasValue && endDate.HasValue)
                {
                    if (DateTime.Compare(startDate.Value, endDate.Value) > 0 && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidEndDate, "EndDate", data.EndDate, LanguageResource.ValidateStartDateEndDate));
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
