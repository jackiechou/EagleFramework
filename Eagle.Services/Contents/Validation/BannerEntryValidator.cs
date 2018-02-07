using System.Collections.Generic;
using System.Security.Claims;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Contents.Banners;
using Eagle.Services.Validations;

namespace Eagle.Services.Contents.Validation
{
    public class BannerEntryValidator : SpecificationBase<BannerEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }
        public BannerEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel, ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(BannerEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundBannerEntry, "BannerEntry"));
                return false;
            }

            //ISpecification<BannerEntry> validPermission = new PermissionValidator<BannerEntry>(CurrentClaimsIdentity, ModuleDefinition.Banner, PermissionLevel.View);
            ISpecification<BannerEntry> isValidBannerTypeId = new IsValidBannerTypeId();
            ISpecification<BannerEntry> isValidBannerScopeId = new IsValidBannerScopeId();
            ISpecification<BannerEntry> isValidName = new IsValidName();

            return isValidBannerTypeId.And(isValidBannerScopeId).And(isValidName).IsSatisfyBy(data, violations);
        }

        private class IsValidBannerTypeId : SpecificationBase<BannerEntry>
        {
            protected override bool IsSatisfyBy(BannerEntry data, IList<RuleViolation> violations = null)
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
        private class IsValidBannerScopeId : SpecificationBase<BannerEntry>
        {
            protected override bool IsSatisfyBy(BannerEntry data, IList<RuleViolation> violations = null)
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


        private class IsValidName : SpecificationBase<BannerEntry>
        {
            protected override bool IsSatisfyBy(BannerEntry data, IList<RuleViolation> violations = null)
            {
                var result = !string.IsNullOrEmpty(data.BannerTitle) && data.BannerTitle.Length > 300;
                if (result && violations != null)
                {
                    violations.Add(new RuleViolation (ErrorCode.InvalidName, "BannerTitle"));
                    return false;
                }
                else
                {
                    if (data.TypeId > 0 && data.TypeId <= int.MaxValue)
                    {
                        bool isDuplicated = UnitOfWork.BannerRepository.HasDataExisted(data.TypeId, data.BannerTitle);
                        if (isDuplicated)
                        {
                            if (violations != null)
                            {
                                violations.Add(new RuleViolation(ErrorCode.DuplicateBannerTitle, "BannerTitle"));
                                return false;
                            }
                        }
                    }
                    return true;
                }
            }
        }
   
       
        //private class ValidUser : SpecificationBase<BannerEntry>
        //{
        //    private IUserService UserService { get; set; }

        //    private ClaimsPrincipal CurrentClaimsIdentity { get; set; }

        //    public ValidMember(IUserService userService, ClaimsPrincipal currentClaimsIdentity)
        //    {
        //        UserService = userService;
        //        CurrentClaimsIdentity = currentClaimsIdentity;
        //    }
        //    protected override bool IsSatisfyBy(BannerEntry data, IList<ErrorExtraInfo> violations = null)
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
