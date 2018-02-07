using System.Collections.Generic;
using System.Security.Claims;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.SystemManagement;
using Eagle.Services.Validations;

namespace Eagle.Services.SystemManagement.Validation
{
    public class SiteMapEditEntryValidator : SpecificationBase<SiteMapEditEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }
        public SiteMapEditEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel, ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(SiteMapEditEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundSiteMapEditEntry, "SiteMapEditEntry"));
                return false;
            }

            ISpecification<SiteMapEditEntry> hasValidName = new HasValidName();
            ISpecification<SiteMapEditEntry> hasValidParentId = new HasValidParentId();
            ISpecification<SiteMapEditEntry> hasValidControllerAction = new HasValidControllerAction();
            return hasValidName.And(hasValidParentId).And(hasValidControllerAction).IsSatisfyBy(data, violations);
        }

        private class HasValidName : SpecificationBase<SiteMapEditEntry>
        {
            protected override bool IsSatisfyBy(SiteMapEditEntry data, IList<RuleViolation> violations = null)
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
                    return true;
                }
            }
        }

        private class HasValidControllerAction : SpecificationBase<SiteMapEditEntry>
        {
            protected override bool IsSatisfyBy(SiteMapEditEntry data, IList<RuleViolation> violations = null)
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
                return true;
            }
        }

        public class HasValidParentId : SpecificationBase<SiteMapEditEntry>
        {
            protected override bool IsSatisfyBy(SiteMapEditEntry data, IList<RuleViolation> violations = null)
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
