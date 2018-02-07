using System.Collections.Generic;
using System.Security.Claims;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.SystemManagement;
using Eagle.Services.Validations;

namespace Eagle.Services.SystemManagement.Validation
{
    public class SiteMapEntryValidator : SpecificationBase<SiteMapEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }

        public SiteMapEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel,
            ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(SiteMapEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundSiteMapEntry, "SiteMapEntry"));
                return false;
            }

            ISpecification<SiteMapEntry> hasValidName = new HasValidName();
            ISpecification<SiteMapEntry> hasValidParentId = new HasValidParentId();
            ISpecification<SiteMapEntry> hasValidControllerAction = new HasValidControllerAction();
            
            return hasValidName.And(hasValidParentId).And(hasValidControllerAction).IsSatisfyBy(data, violations);
        }

        private class HasValidName : SpecificationBase<SiteMapEntry>
        {
            protected override bool IsSatisfyBy(SiteMapEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.Title) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullTitle, "Title"));
                    return false;
                }
                else
                {
                    if (data.Title.Length > 250 && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidTitle, "Title"));
                        return false;
                    }
                    else
                    {
                        bool isDuplicate = UnitOfWork.SiteMapRepository.HasDataExisted(data.Title, data.ParentId);
                        if (isDuplicate)
                        {
                            if (violations != null)
                            {
                                violations.Add(new RuleViolation(ErrorCode.DuplicateTitle, "Title", data.Title));
                                return false;
                            }
                        }
                    }
                    return true;
                }
            }
        }

        private class HasValidControllerAction : SpecificationBase<SiteMapEntry>
        {
            protected override bool IsSatisfyBy(SiteMapEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.Action) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullAction, "Action"));
                    return false;
                }
                else
                {
                    if (data.Action.Length > 250 && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidAction, "Action", data.Action));
                        return false;
                    }
                }

                if (string.IsNullOrEmpty(data.Controller) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullController, "Controller"));
                    return false;
                }
                else
                {
                    if (data.Controller.Length > 250 && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidController, "Controller", data.Controller));
                        return false;
                    }
                }

                if (!string.IsNullOrEmpty(data.Action) && !string.IsNullOrEmpty(data.Controller))
                {
                    bool isDuplicate = UnitOfWork.SiteMapRepository.HasDataExisted(data.Controller, data.Action, data.ParentId);
                    if (isDuplicate)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicateControllerAction, "ControllerAction", $"{data.Controller} - {data.Action}"));
                            return false;
                        }
                    }
                }

                return true;
            }
        }

        public class HasValidParentId : SpecificationBase<SiteMapEntry>
        {
            protected override bool IsSatisfyBy(SiteMapEntry data, IList<RuleViolation> violations = null)
            {
                if (data.ParentId != null && data.ParentId > 0 && data.ParentId <= int.MaxValue)
                {
                    var entityByParentId = UnitOfWork.SiteMapRepository.FindById(data.ParentId);
                    if (entityByParentId == null && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.NullReferenceForParentId, "ParentId", null,
                            ErrorMessage.Messages[ErrorCode.NullReferenceForParentId]));
                        return false;
                    }
                }
                return true;
            }
        }
    }
}