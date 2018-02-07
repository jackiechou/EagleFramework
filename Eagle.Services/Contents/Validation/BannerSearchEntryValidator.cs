using System.Collections.Generic;
using System.Security.Claims;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Contents.Banners;
using Eagle.Services.Validations;

namespace Eagle.Services.Contents.Validation
{
 
    public class BannerSearchEntryValidator : SpecificationBase<BannerSearchEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        public BannerSearchEntryValidator(ClaimsPrincipal currentClaimsIdentity)
        {
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(BannerSearchEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundBannerSearchEntry, "BannerSearchEntry"));
                return false;
            }
            //ISpecification<BannerSearchEntry> validPermission = new PermissionValidator<BannerSearchEntry>(CurrentClaimsIdentity, ModuleDefinition.Banners, PermissionAccessLevel.View);
            //var result = validPermission.And(validMember).IsSatisfyBy(data, violations);
            //return result;
            ISpecification<BannerSearchEntry> isValidBannerTypeId = new IsValidBannerTypeId();
            ISpecification<BannerSearchEntry> isValidBannerPositionId = new IsValidBannerPositionId();
            var result = isValidBannerTypeId.And(isValidBannerPositionId).IsSatisfyBy(data, violations);
            return result;
        }
        private class IsValidBannerTypeId : SpecificationBase<BannerSearchEntry>
        {
            protected override bool IsSatisfyBy(BannerSearchEntry data, IList<RuleViolation> violations = null)
            {
                var result = data.BannerTypeId > 0 && data.BannerTypeId <= int.MaxValue;
                if (!result && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidBannerTypeId, "BannerTypeId"));
                    return false;
                }
                return true;
            }
        }

        private class IsValidBannerPositionId : SpecificationBase<BannerSearchEntry>
        {
            protected override bool IsSatisfyBy(BannerSearchEntry data, IList<RuleViolation> violations = null)
            {
                var result = data.BannerPositionId > 0 && data.BannerPositionId <= int.MaxValue;
                if (!result && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidBannerPositionId,"BannerPositionId"));
                    return false;
                }
                return true;
            }
        }
        //private class ValidUser : SpecificationBase<BannerSearchEntry>
        //{
        //    private IUserService UserService { get; set; }

        //    private ClaimsPrincipal CurrentClaimsIdentity { get; set; }

        //    public ValidMember(IUserService userService, ClaimsPrincipal currentClaimsIdentity)
        //    {
        //        UserService = userService;
        //        CurrentClaimsIdentity = currentClaimsIdentity;
        //    }
        //    protected override bool IsSatisfyBy(BannerSearchEntry data, IList<ErrorExtraInfo> violations = null)
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
