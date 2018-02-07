using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Permission;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Transactions;
using Eagle.Repositories;
using Eagle.Services.Business.Validation;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.EntityMapping.Common;
using Eagle.Services.Validations;

namespace Eagle.Services.Business
{
    public class CurrencyService : BaseService, ICurrencyService
    {
        public CurrencyService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        #region Currency

        public IEnumerable<CurrencyDetail> GetCurrencies(CurrencySearchEntry filter, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = UnitOfWork.CurrencyRepository.GetCurrencies(filter.SearchText, filter.IsActive, ref recordCount, orderBy, page,
                pageSize);
            return lst.ToDtos<CurrencyGroup, CurrencyDetail>();
        }

        public CurrencyDetail GetSelectedCurrency()
        {
            var entity = UnitOfWork.CurrencyRepository.GetSelectedCurrency();
            return entity.ToDto<CurrencyGroup, CurrencyDetail>();
        }
        
        public SelectList PopulateCurrencySelectList(CurrencyStatus? status = null, string selectedValue = null,
            bool? isShowSelectText = false)
        {
            return UnitOfWork.CurrencyRepository.PopulateCurrencySelectList(status, selectedValue, isShowSelectText);
        }
        
        public CurrencyDetail GetCurrencyDetail(int id)
        {
            var entity = UnitOfWork.CurrencyRepository.FindById(id);
            return entity.ToDto<CurrencyGroup, CurrencyDetail>();
        }

        public void InsertCurrency(CurrencyEntry entry)
        {
            ISpecification<CurrencyEntry> validator = new CurrencyEntryValidator(UnitOfWork, PermissionLevel.Create);
            var dataViolations = new List<RuleViolation>();
            var isValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isValid) throw new ValidationError(dataViolations);

            var entity = entry.ToEntity<CurrencyEntry, CurrencyGroup>();
            UnitOfWork.CurrencyRepository.Insert(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateCurrency(CurrencyEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullCurrencyEntry, "CurrencyEntry", null, ErrorMessage.Messages[ErrorCode.NullCurrencyEntry]));
                throw new ValidationError(violations);
            }

            var entity = UnitOfWork.CurrencyRepository.FindById(entry.CurrencyId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullCurrency, "Currency", entry.CurrencyId,ErrorMessage.Messages[ErrorCode.NullCurrency]));
                throw new ValidationError(violations);
            }

            if (string.IsNullOrEmpty(entry.CurrencyName))
            {
                violations.Add(new RuleViolation(ErrorCode.NullCurrencyName, "CurrencyName", entry.CurrencyName, ErrorMessage.Messages[ErrorCode.NullCurrencyName]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.CurrencyName.Length > 250)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidCurrencyName, "CurrencyName", entry.CurrencyName, ErrorMessage.Messages[ErrorCode.InvalidCurrencyName]));
                    throw new ValidationError(violations);
                }
                else
                {
                    if (entity.CurrencyName != entry.CurrencyName)
                    {
                        bool isDuplicate = UnitOfWork.CurrencyRepository.HasDataExisted(entry.CurrencyName);
                        if (isDuplicate)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicateCurrencyName, "Currency",
                                entry.CurrencyName, ErrorMessage.Messages[ErrorCode.DuplicateCurrencyName]));
                            throw new ValidationError(violations);
                        }
                    }
                }
            }

            entity.CurrencyName = entry.CurrencyName;
            entity.IsSelected = false;
            entity.IsActive = entry.IsActive;

            UnitOfWork.CurrencyRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateSelectedCurrency(int id)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.CurrencyRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundCurrency, "Currency", id, ErrorMessage.Messages[ErrorCode.NotFoundCurrency]));
                throw new ValidationError(violations);
            }

            if (entity.IsSelected) return;

            var lst = UnitOfWork.CurrencyRepository.GetCurrencyList(null).ToList();
            if (!lst.Any()) return;

            foreach (var item in lst)
            {
                item.IsSelected = item.CurrencyId == id;
                UnitOfWork.CurrencyRepository.Update(item);
            }
            UnitOfWork.SaveChanges();
        }

        public void SetSelectedCurrency(string currencyCode)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.CurrencyRepository.GetDetail(currencyCode);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundCurrency, "CurrencyCode", currencyCode, ErrorMessage.Messages[ErrorCode.NotFoundCurrency]));
                throw new ValidationError(violations);
            }

            if (entity.IsSelected) return;

            var lst = UnitOfWork.CurrencyRepository.GetCurrencyList(null).ToList();
            if (!lst.Any()) return;

            foreach (var item in lst)
            {
                item.IsSelected = item.CurrencyCode == currencyCode;
                UnitOfWork.CurrencyRepository.Update(item);
            }
            UnitOfWork.SaveChanges();
        }

        public void UpdateCurrencyStatus(int id, CurrencyStatus status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.CurrencyRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundCurrency, "Currency", id, ErrorMessage.Messages[ErrorCode.NotFoundCurrency]));
                throw new ValidationError(violations);
            }

            var isValid = Enum.IsDefined(typeof(CurrencyStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status", null,
                    ErrorMessage.Messages[ErrorCode.InvalidStatus]));
                throw new ValidationError(violations);
            }

            if (entity.IsActive == status) return;

            entity.IsActive = status;
            UnitOfWork.CurrencyRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region Currency Rate
        public IEnumerable<CurrencyRateDetail> GetCurrencyRates(ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = UnitOfWork.CurrencyRateRepository.GetList(ref recordCount, orderBy, page,
                pageSize);
            return lst.ToDtos<CurrencyRate, CurrencyRateDetail>();
        }
        public CurrencyRateDetail GetCurrencyRateDetail(int id)
        {
            var entity = UnitOfWork.CurrencyRateRepository.FindById(id);
            return entity.ToDto<CurrencyRate, CurrencyRateDetail>();
        }

        public void InsertCurrencyRate(CurrencyRateEntry entry)
        {
            ISpecification<CurrencyRateEntry> validator = new CurrencyRateEntryValidator(UnitOfWork, PermissionLevel.Create);
            var dataViolations = new List<RuleViolation>();
            var isValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isValid) throw new ValidationError(dataViolations);

            var entity = entry.ToEntity<CurrencyRateEntry, CurrencyRate>();
            UnitOfWork.CurrencyRateRepository.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateCurrencyRate(int id, CurrencyRateEntry entry)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.CurrencyRateRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullCurrencyRate, "CurrencyRate", id, ErrorMessage.Messages[ErrorCode.NullCurrencyRate]));
                throw new ValidationError(violations);
            }

            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullCurrencyRateEntry, "CurrencyRateEntry", null, ErrorMessage.Messages[ErrorCode.NullCurrencyRateEntry]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entity.AverageRate != entry.AverageRate)
                {
                    bool isDuplicate = UnitOfWork.CurrencyRateRepository.HasDataExisted(entry.CurrencyRateDate, entry.AverageRate);
                    if (isDuplicate)
                    {
                        violations.Add(new RuleViolation(ErrorCode.DuplicateCurrencyRate, "CurrencyRate",
                            $"{entry.CurrencyRateDate}- {entry.AverageRate}"));
                        throw new ValidationError(violations);
                    }
                }
            }

            entity.CurrencyRateDate = entry.CurrencyRateDate;
            entity.FromCurrencyCode = entry.FromCurrencyCode;
            entity.ToCurrencyCode = entry.ToCurrencyCode;
            entity.AverageRate = entry.AverageRate;
            entity.EndOfDayRate = entry.EndOfDayRate;

            UnitOfWork.CurrencyRateRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        #endregion

    }
}
