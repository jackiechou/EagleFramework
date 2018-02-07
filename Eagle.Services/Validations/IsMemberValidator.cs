using System;
using System.Collections.Generic;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;

namespace Eagle.Services.Validations
{
    public class IsMemberValidator<TData> : SpecificationBase<TData>
    {
        private static IUnitOfWork UnitOfWork { get; set; }
        private string UserId { get; set; }
        private PermissionLevel PermissionLevel { get; set; }

       public IsMemberValidator(IUnitOfWork unitOfWork, string userId, PermissionLevel permissionLevel)
        {
            UnitOfWork = unitOfWork;
            UserId = userId;
            PermissionLevel = permissionLevel;
        }

        protected override bool IsSatisfyBy(TData data, IList<RuleViolation> violations = null)
        {
            if (string.IsNullOrEmpty(UserId))
            {
                if (violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NotFoundForUserId));
                }
            }
            else
            {
                var userId = Guid.Parse(UserId);
                var account = UnitOfWork.UserRepository.FindById(userId);
                if (account==null)
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.NoAccessData,"UserId", ErrorMessage.Messages[ErrorCode.NoAccessData]));
                    }
                    return false;
                }
            }
            return true;
        }
    }
}
