using System;
using System.Collections.Generic;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Validations;

namespace Eagle.Services.Business.Validation
{
    public class CurrencyRateEntryValidator : SpecificationBase<CurrencyRateEntry>
    {
        private static IUnitOfWork UnitOfWork { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        public CurrencyRateEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
        }

        protected override bool IsSatisfyBy(CurrencyRateEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundCurrencyRateEntry, "Currency rate entry",null, ErrorMessage.Messages[ErrorCode.NotFoundCurrencyRateEntry]));
                return false;
            }

            ISpecification<CurrencyRateEntry> hasValidFromCurrencyCode = new HasValidFromCurrencyCode();
            ISpecification<CurrencyRateEntry> hasValidToCurrencyCode = new HasValidToCurrencyCode();
            ISpecification<CurrencyRateEntry> hasValidCurrencyRateDate = new HasValidCurrencyRateDate();
            ISpecification<CurrencyRateEntry> hasValidAverageRate = new HasValidAverageRate();
            ISpecification<CurrencyRateEntry> hasValidEndOfDayRate = new HasValidEndOfDayRate();
            
            return hasValidFromCurrencyCode.And(hasValidToCurrencyCode).And(hasValidCurrencyRateDate)
                .And(hasValidAverageRate).And(hasValidEndOfDayRate).IsSatisfyBy(data, violations);
        }

        internal class HasValidFromCurrencyCode : SpecificationBase<CurrencyRateEntry>
        {
            protected override bool IsSatisfyBy(CurrencyRateEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.FromCurrencyCode) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidFromCurrencyCode, null,
                        ErrorMessage.Messages[ErrorCode.NotFoundCurrencyRateEntry]));

                    return false;
                }
                else
                {
                    if (data.FromCurrencyCode.Length > 3 && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidFromCurrencyCode, data.FromCurrencyCode, ErrorMessage.Messages[ErrorCode.InvalidFromCurrencyCode]));
                        return false;
                    }
                }
                return true;
            }
        }
        internal class HasValidToCurrencyCode : SpecificationBase<CurrencyRateEntry>
        {
            protected override bool IsSatisfyBy(CurrencyRateEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.ToCurrencyCode) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidToCurrencyCode, null,
                        ErrorMessage.Messages[ErrorCode.InvalidToCurrencyCode]));

                    return false;
                }
                else
                {
                    if (data.ToCurrencyCode.Length > 3 && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidToCurrencyCode, data.ToCurrencyCode, ErrorMessage.Messages[ErrorCode.InvalidToCurrencyCode]));
                        return false;
                    }
                }
                return true;
            }
        }
        internal class HasValidCurrencyRateDate : SpecificationBase<CurrencyRateEntry>
        {
            protected override bool IsSatisfyBy(CurrencyRateEntry data, IList<RuleViolation> violations = null)
            {
                if (data.CurrencyRateDate < DateTime.UtcNow.Date)
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidCurrencyRateDate, "CurrencyRateDate", data.CurrencyRateDate, ErrorMessage.Messages[ErrorCode.InvalidCurrencyRateDate]));
                        return false;
                    }
                }
                else
                {
                    bool isDuplicate = UnitOfWork.CurrencyRateRepository.HasDataExisted(data.CurrencyRateDate, data.AverageRate);
                    if (isDuplicate)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicateCurrencyRate, "CurrencyRate",
                                $"{data.CurrencyRateDate}- {data.AverageRate}", ErrorMessage.Messages[ErrorCode.DuplicateCurrencyRate]));
                            return false;
                        }
                    }
                }
                return true;
            }
        }
        internal class HasValidAverageRate : SpecificationBase<CurrencyRateEntry>
        {
            protected override bool IsSatisfyBy(CurrencyRateEntry data, IList<RuleViolation> violations = null)
            {
                if (data.AverageRate > 0 && data.AverageRate < 1 && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidAverageRate, "AverageRate"));
                    return false;
                }
                return true;
            }
        }
        internal class HasValidEndOfDayRate : SpecificationBase<CurrencyRateEntry>
        {
            protected override bool IsSatisfyBy(CurrencyRateEntry data, IList<RuleViolation> violations = null)
            {
                if (data.EndOfDayRate > 0 && data.EndOfDayRate < 1 && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidEndOfDayRate, "EndOfDayRate", data.EndOfDayRate, ErrorMessage.Messages[ErrorCode.InvalidEndOfDayRate]));
                    return false;
                }
                return true;
            }
        }
    }
}
