using System.Collections.Generic;
using System.Security.Claims;
using Eagle.Core.Permission;
using Eagle.Services.Dtos.Common;

namespace Eagle.Services.Validations
{
    public class PermissionValidator<TData> : SpecificationBase<TData>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private string PermissionCapability { get; set; }
        private PermissionLevel PermissionLevel { get; set; }

        public PermissionValidator(ClaimsPrincipal currentClaimsIdentity, string permissionCapability, PermissionLevel permissionLevel)
        {
            CurrentClaimsIdentity = currentClaimsIdentity;
            PermissionCapability = permissionCapability;
            PermissionLevel = permissionLevel;
        }

        protected override bool IsSatisfyBy(TData data, IList<RuleViolation> violations = null)
        {
            //Ignore data and only check user's permission
            if (CurrentClaimsIdentity.FindFirst(PermissionCapability) != null &&
                CurrentClaimsIdentity.FindFirst(PermissionCapability).Value.ToPermission() >=
                PermissionLevel)
                return true;

            violations?.Add(new RuleViolation(ErrorCode.NoPermission));

            return false;
        }
    }
}
