using System.Collections.Generic;
using System.Security.Claims;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Business.Personnel;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Validations;

namespace Eagle.Services.Business.Validation
{
    public class SkillEditEntryValidator : SpecificationBase<SkillEditEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        public SkillEditEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel, ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }
        protected override bool IsSatisfyBy(SkillEditEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullSkillEditEntry, "SkillEditEntry", null, ErrorMessage.Messages[ErrorCode.NullSkillEditEntry]));
                return false;
            }
            //ISpecification<SkillEditEntry> hasValidPermission = new PermissionValidator<SkillEntry>(CurrentClaimsIdentity,
            //                                                SherpaPermissionCapabilities.PROFILE, Permission);

            ISpecification<SkillEditEntry> hasValidSkillId = new HasValidSkillId();
            ISpecification<SkillEditEntry> hasValidSkillName = new HasValidSkillName();

            var result = hasValidSkillId.And(hasValidSkillName)
                .IsSatisfyBy(data, violations);

            return result;
        }
        public class HasValidSkillId : SpecificationBase<SkillEditEntry>
        {
            protected override bool IsSatisfyBy(SkillEditEntry data, IList<RuleViolation> violations = null)
            {
                if (data.SkillId <= 0 && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NotFoundSkill, "SkillId", data, ErrorMessage.Messages[ErrorCode.NotFoundSkill]));
                    return false;
                }

                return true;
            }
        }

        public class HasValidSkillName : SpecificationBase<SkillEditEntry>
        {
            protected override bool IsSatisfyBy(SkillEditEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.SkillName) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullSkillName, "SkillName", null, ErrorMessage.Messages[ErrorCode.NullSkillName]));
                    return false;
                }
                else
                {
                    if (data.SkillName.Length > 250 && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidSkillName, "SkillName", null, ErrorMessage.Messages[ErrorCode.InvalidSkillName]));
                        return false;
                    }
                }
                return true;
            }
        }
        //public class HasValidMemberId : SpecificationBase<MemberSkill>
        //{
        //    private IMemberService MemberService { get; set; }

        //    private ClaimsPrincipal CurrentClaimsIdentity { get; set; }

        //    public HasValidMemberId(IMemberService memberService, ClaimsPrincipal currentClaimsIdentity)
        //    {
        //        MemberService = memberService;
        //        CurrentClaimsIdentity = currentClaimsIdentity;
        //    }
        //    protected override bool IsSatisfyBy(MemberSkill memberSkilldata, IList<RuleViolation> violations = null)
        //    {
        //        if (memberSkilldata.MemberId <= 0)
        //        {
        //            if (violations != null)
        //                violations.Add(new ErrorExtraInfo { Code = ErrorCodeType.InvalidMemberId });
        //            return false;
        //        }

        //        var member = MemberService.GetById(memberSkilldata.MemberId);
        //        if (member == null)
        //        {
        //            if (violations != null)
        //                violations.Add(new ErrorExtraInfo { Code = ErrorCodeType.InvalidMemberId });
        //            return false;
        //        }
        //        if (member.NetworkId != CurrentClaimsIdentity.GetNetworkId())
        //        {
        //            if (violations != null)
        //                violations.Add(new ErrorExtraInfo { Code = ErrorCodeType.NoAccessData });
        //            return false;
        //        }

        //        return true;
        //    }
        //}
    }

    public class SkillEntryValidator : SpecificationBase<SkillEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }
        private PermissionLevel PermissionLevel { get; set; }

        public SkillEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel, ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(SkillEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullSkillEntry, "SkillEntry", null, ErrorMessage.Messages[ErrorCode.NullSkillEntry]));
                return false;
            }
            //ISpecification<SkillEntry> hasValidPermission = new PermissionValidator<SkillEntry>(CurrentClaimsIdentity,
            //                                                SherpaPermissionCapabilities.PROFILE, Permission);

            ISpecification<SkillEntry> hasValidSkillName = new HasValidSkillName();

            var result = hasValidSkillName
                .IsSatisfyBy(data, violations);

            return result;
        }

        public class HasValidSkillName : SpecificationBase<SkillEntry>
        {
            protected override bool IsSatisfyBy(SkillEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.SkillName) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullSkillName, "SkillName", null, ErrorMessage.Messages[ErrorCode.NullSkillName]));
                    return false;
                }
                else
                {
                    if (data.SkillName.Length > 250 && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidSkillName, "SkillName", null, ErrorMessage.Messages[ErrorCode.InvalidSkillName]));
                        return false;
                    }
                    else
                    {
                        bool isDuplicate = UnitOfWork.SkillRepository.HasNameExisted(data.SkillName);
                        if (isDuplicate && violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicateSkillName, "SkillName",
                                    data, ErrorMessage.Messages[ErrorCode.DuplicateSkillName]));
                            return false;
                        }
                    }
                }
                return true;
            }
        }
    }
}
