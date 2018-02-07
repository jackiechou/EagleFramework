using System.Collections.Generic;
using System.Security.Claims;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Contents.Banners;
using Eagle.Services.Validations;

namespace Eagle.Services.Contents.Validation
{
    public class BannerEditEntryValidator : SpecificationBase<BannerEditEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }
        public BannerEditEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel, ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(BannerEditEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundBannerEditEntry, "BannerEditEntry"));
                return false;
            }

            //ISpecification<BannerEditEntry> validPermission = new PermissionValidator<BannerEditEntry>(CurrentClaimsIdentity, ModuleDefinition.Banner, PermissionLevel.View);

            ISpecification<BannerEditEntry> isValidName = new IsValidName();
            ISpecification<BannerEditEntry> isValidBannerTypeId = new IsValidBannerTypeId();
            ISpecification<BannerEditEntry> isValidBannerScopeId = new IsValidBannerScopeId();

            var result = isValidBannerTypeId.And(isValidBannerTypeId).And(isValidName).IsSatisfyBy(data, violations);
            return result;
        }

        private class IsValidName : SpecificationBase<BannerEditEntry>
        {
            protected override bool IsSatisfyBy(BannerEditEntry data, IList<RuleViolation> violations = null)
            {
                var result = !string.IsNullOrEmpty(data.BannerTitle) && data.BannerTitle.Length > 300;
                if (result && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidName, "BannerTitle"));
                    return false;
                }
                return true;
            }
        }
        private class IsValidBannerTypeId : SpecificationBase<BannerEditEntry>
        {
            protected override bool IsSatisfyBy(BannerEditEntry data, IList<RuleViolation> violations = null)
            {
                var result = data.TypeId > 0 && data.TypeId <= int.MaxValue;
                if (!result && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidBannerTypeId, "BannerTypeId"));
                    return false;
                }
                return true;
            }
        }

        private class IsValidBannerScopeId : SpecificationBase<BannerEditEntry>
        {
            protected override bool IsSatisfyBy(BannerEditEntry data, IList<RuleViolation> violations = null)
            {
                var result = data.ScopeId > 0 && data.ScopeId <= int.MaxValue;
                if (!result && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidBannerScopeId, "BannerScopeId"));
                    return false;
                }
                return true;
            }
        }

        //private class ValidUser : SpecificationBase<BannerEditEntry>
        //{
        //    private IUserService UserService { get; set; }

        //    private ClaimsPrincipal CurrentClaimsIdentity { get; set; }

        //    public ValidMember(IUserService userService, ClaimsPrincipal currentClaimsIdentity)
        //    {
        //        UserService = userService;
        //        CurrentClaimsIdentity = currentClaimsIdentity;
        //    }
        //    protected override bool IsSatisfyBy(BannerEditEntry data, IList<ErrorExtraInfo> violations = null)
        //    {
        //        int memberId = CurrentClaimsIdentity.GetMemberId();
        //        int networkId = CurrentClaimsIdentity.GetNetworkId();

        //        var member = UserService.FindById(memberId);
        //        if (member == null)
        //        {
        //            if (violations != null)
        //                violations.Add(new ErrorExtraInfo { Code = ErrorCodeType.InvalidMemberId });
        //            return false;
        //        }

        //        if (member.NetworkId != networkId)
        //        {
        //            if (violations != null)
        //                violations.Add(new ErrorExtraInfo { Code = ErrorCodeType.NoAccessData });
        //            return false;
        //        }

        //        return true;
        //    }
        //}
    }
}
