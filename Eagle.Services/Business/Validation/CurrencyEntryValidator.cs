using System.Collections.Generic;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Validations;

namespace Eagle.Services.Business.Validation
{
    public class CurrencyEntryValidator : SpecificationBase<CurrencyEntry>
    {
        private static IUnitOfWork UnitOfWork { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        public CurrencyEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
        }

        protected override bool IsSatisfyBy(CurrencyEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullCurrencyEntry, "CurrencyEntry",null, ErrorMessage.Messages[ErrorCode.NullCurrencyEntry]));
                return false;
            }

            ISpecification<CurrencyEntry> hasValidCurrencyName = new HasValidCurrencyName();
            return hasValidCurrencyName.IsSatisfyBy(data, violations);
        }

        internal class HasValidCurrencyName : SpecificationBase<CurrencyEntry>
        {
            protected override bool IsSatisfyBy(CurrencyEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.CurrencyName))
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.NullCurrencyName, "CurrencyName", null, ErrorMessage.Messages[ErrorCode.NullCurrencyName]));
                        return false;
                    }
                }
                else
                {
                    if (data.CurrencyName.Length > 250)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.InvalidCurrencyName, "CurrencyName", data.CurrencyName, ErrorMessage.Messages[ErrorCode.InvalidCurrencyName]));
                        }
                        return false;
                    }
                    else
                    {
                        bool isDuplicate = UnitOfWork.CurrencyRepository.HasDataExisted(data.CurrencyName);
                        if (isDuplicate)
                        {
                            if (violations != null)
                            {
                                violations.Add(new RuleViolation(ErrorCode.DuplicateCurrencyName, "CurrencyName", data.CurrencyName, ErrorMessage.Messages[ErrorCode.DuplicateCurrencyName]));
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
        }

    }
}
