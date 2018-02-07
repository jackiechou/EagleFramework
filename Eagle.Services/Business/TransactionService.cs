using System;
using System.Collections.Generic;
using System.Linq;
using Eagle.Common.Utilities;
using Eagle.Core.Permission;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Shipping;
using Eagle.Entities.Business.Transactions;
using Eagle.Repositories;
using Eagle.Services.Business.Validation;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.EntityMapping.Common;
using Eagle.Services.Validations;

namespace Eagle.Services.Business
{
    public class TransactionService: BaseService, ITransactionService
    {
        public ICommonService CommonService { get; set; }
        public TransactionService(IUnitOfWork unitOfWork, ICommonService commonService) : base(unitOfWork)
        {
            CommonService = commonService;
        }

        #region Transaction Method

        public IEnumerable<TransactionMethodDetail> GetTransactionMethods(TransactionMethodSearchEntry filter, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = UnitOfWork.TransactionMethodRepository.GetTransactionMethods(filter.TransactionMethodName, filter.IsActive, ref recordCount, orderBy, page,
                pageSize);
            return lst.ToDtos<TransactionMethod, TransactionMethodDetail>();
        }

        public IEnumerable<TransactionMethodDetail> GetTransactionMethods(TransactionMethodStatus? status)
        {
            var lst = UnitOfWork.TransactionMethodRepository.GetTransactionMethods(status).ToList();
            return lst.ToDtos<TransactionMethod, TransactionMethodDetail>();
        }

        public TransactionMethodDetail GetTransactionMethodDetail(int id)
        {
            var entity = UnitOfWork.TransactionMethodRepository.FindById(id);
            return entity.ToDto<TransactionMethod, TransactionMethodDetail>();
        }

        public void InsertTransactionMethod(TransactionMethodEntry entry)
        {
            bool isDuplicate = UnitOfWork.TransactionMethodRepository.HasDataExisted(entry.TransactionMethodName);
            if (isDuplicate) return;

            var entity = new TransactionMethod
            {
                TransactionMethodName = entry.TransactionMethodName,
                TransactionMethodFee = !string.IsNullOrEmpty(entry.TransactionMethodFee)?Convert.ToDecimal(entry.TransactionMethodFee):0,
                IsActive = entry.IsActive
            };

            UnitOfWork.TransactionMethodRepository.Insert(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateTransactionMethod(TransactionMethodEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.TransactionMethodRepository.FindById(entry.TransactionMethodId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundTransactionMethod, "TransactionMethod", entry.TransactionMethodId));
                throw new ValidationError(violations);
            }

            entity.TransactionMethodName = entry.TransactionMethodName;
            entity.TransactionMethodFee = !string.IsNullOrEmpty(entry.TransactionMethodFee)
                ? Convert.ToDecimal(entry.TransactionMethodFee)
                : 0;
            entity.IsActive = entry.IsActive;

            UnitOfWork.TransactionMethodRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateTransactionMethodStatus(int id, TransactionMethodStatus status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.TransactionMethodRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundTransactionMethod, "TransactionMethod", id));
                throw new ValidationError(violations);
            }

            var isValid = Enum.IsDefined(typeof(TransactionMethodStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status", null,
                    ErrorMessage.Messages[ErrorCode.InvalidStatus]));
                throw new ValidationError(violations);
            }
            if (entity.IsActive == status) return;

            entity.IsActive = status;
            UnitOfWork.TransactionMethodRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region Payment Method
        public IEnumerable<PaymentMethodDetail> GetPaymentMethods(PaymentMethodSearchEntry filter, ref int? recordCount,
           string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = UnitOfWork.PaymentMethodRepository.GetPaymentMethods(filter.PaymentMethodName, filter.IsActive, ref recordCount, orderBy, page,
                pageSize);
            return lst.ToDtos<PaymentMethod, PaymentMethodDetail>();
        }
        public IEnumerable<PaymentMethodDetail> GetPaymentMethods(PaymentMethodStatus? status = null)
        {
            var lst = UnitOfWork.PaymentMethodRepository.GetPaymentMethods(status);
            return lst.ToDtos<PaymentMethod, PaymentMethodDetail>();
        }
        public PaymentMethodDetail GetPaymentMethodDetail(int id)
        {
            var entity = UnitOfWork.PaymentMethodRepository.FindById(id);
            return entity.ToDto<PaymentMethod, PaymentMethodDetail>();
        }
        public void InsertPaymentMethod(PaymentMethodEntry entry)
        {
            var violations = new List<RuleViolation>();
            bool isDuplicate = UnitOfWork.PaymentMethodRepository.HasDataExisted(entry.PaymentMethodName);
            if (isDuplicate)
            {
                violations.Add(new RuleViolation(ErrorCode.DuplicatePaymentMethodName, "PaymentMethodName", entry.PaymentMethodName));
                throw new ValidationError(violations);
            }

            var entity = entry.ToEntity<PaymentMethodEntry, PaymentMethod>();
            UnitOfWork.PaymentMethodRepository.Insert(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdatePaymentMethod(PaymentMethodEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.PaymentMethodRepository.FindById(entry.PaymentMethodId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundPaymentMethod, "PaymentMethod", entry.PaymentMethodId));
                throw new ValidationError(violations);
            }

            entity.PaymentMethodName = entry.PaymentMethodName;
            entity.IsCreditCard = entry.IsCreditCard;
            entity.IsSelected = entry.IsSelected;
            entity.IsActive = entry.IsActive;

            UnitOfWork.PaymentMethodRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateSelectedPaymentMethod(int paymentMethodId)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.PaymentMethodRepository.FindById(paymentMethodId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundPaymentMethod, "PaymentMethodId",null, ErrorMessage.Messages[ErrorCode.NotFoundPaymentMethod]));
                throw new ValidationError(violations);
            }
            
            if (entity.IsSelected) return;

            var lst = UnitOfWork.PaymentMethodRepository.GetPaymentMethods();
            if (lst==null) return;

            foreach (var item in lst)
            {
                if (item.PaymentMethodId == paymentMethodId)
                {
                    item.IsSelected = true;
                    entity.IsActive = PaymentMethodStatus.Active;
                }
                else
                {
                    item.IsSelected = false;
                }
                UnitOfWork.PaymentMethodRepository.Update(item);
            }
            UnitOfWork.SaveChanges();
        }

        public void UpdatePaymentMethodStatus(int id, PaymentMethodStatus status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.PaymentMethodRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundPaymentMethod, "PaymentMethod"));
                throw new ValidationError(violations);
            }

            var isValid = Enum.IsDefined(typeof(PaymentMethodStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status", null,
                    ErrorMessage.Messages[ErrorCode.InvalidStatus]));
                throw new ValidationError(violations);
            }
            if (entity.IsActive == status) return;

            entity.IsActive = status;
            UnitOfWork.PaymentMethodRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region Promotion
       
        public IEnumerable<PromotionDetail> GetPromotions(int vendorId, PromotionSearchEntry filter, ref int? recordCount,
           string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = UnitOfWork.PromotionRepository.GetPromotions(vendorId, filter.StartDate, filter.EndDate, filter.IsActive, ref recordCount, orderBy, page,
                pageSize);
            return lst.ToDtos<Promotion, PromotionDetail>();
        }

        public bool HasPromotionCodeExisted(int vendorId, string promotionCode)
        {
            return UnitOfWork.PromotionRepository.HasPromotionCodeExisted(vendorId, promotionCode);
        }
        public string GeneratePromotionCode(int maxLength)
        {
            return UnitOfWork.PromotionRepository.GenerateCode(maxLength).ToUpper();
        }
        public PromotionDetail GetPromotionDetailByCode(int vendorId, string promotionCode)
        {
            var entity = UnitOfWork.PromotionRepository.GetDetails(vendorId, promotionCode);
            return entity.ToDto<Promotion, PromotionDetail>();
        }
        public PromotionDetail GetPromotionDetail(int id)
        {
            var entity = UnitOfWork.PromotionRepository.FindById(id);
            return entity.ToDto<Promotion, PromotionDetail>();
        }
        public void InsertPromotion(Guid userId, int vendorId, PromotionEntry entry)
        {
            ISpecification<PromotionEntry> validator = new PromotionEntryValidator(UnitOfWork, PermissionLevel.Create, CurrentClaimsIdentity);
            var violations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, violations);
            if (!isDataValid) throw new ValidationError(violations);

            if (string.IsNullOrEmpty(entry.PromotionCode))
            {
                violations.Add(new RuleViolation(ErrorCode.NullPromotionCode, "PromotionCode", null,
                    ErrorMessage.Messages[ErrorCode.NullPromotionCode]));
                throw new ValidationError(violations);
            }
            else
            {

                bool isDuplicate = UnitOfWork.PromotionRepository.HasPromotionCodeExisted(vendorId, entry.PromotionCode);
                if (isDuplicate)
                {
                    violations.Add(new RuleViolation(ErrorCode.DuplicatePromotionCode, "PromotionCode", entry.PromotionCode,
                    ErrorMessage.Messages[ErrorCode.DuplicatePromotionCode]));
                    throw new ValidationError(violations);
                }
            }

            if (string.IsNullOrEmpty(entry.PromotionTitle))
            {
                violations.Add(new RuleViolation(ErrorCode.NullTitle, "PromotionTitle", entry.PromotionTitle,
                    ErrorMessage.Messages[ErrorCode.NullTitle]));
                throw new ValidationError(violations);
            }

            var entity = entry.ToEntity<PromotionEntry, Promotion>();
            entity.VendorId = vendorId;
            entity.Ip = NetworkUtils.GetIP4Address();
            entity.CreatedDate = DateTime.UtcNow;
            entity.CreatedByUserId = userId;

            UnitOfWork.PromotionRepository.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdatePromotion(Guid userId, int vendorId, PromotionEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.PromotionRepository.FindById(entry.PromotionId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundPromotion, "Promotion", entry.PromotionId));
                throw new ValidationError(violations);
            }

            if (entity.PromotionCode != entry.PromotionCode)
            {
                bool isDuplicate = UnitOfWork.PromotionRepository.HasPromotionCodeExisted(vendorId, entry.PromotionCode);
                if (isDuplicate)
                {
                    violations.Add(new RuleViolation(ErrorCode.DuplicatePromotionCode, "PromotionCode", entry.PromotionCode, ErrorMessage.Messages[ErrorCode.DuplicatePromotionCode]));
                    throw new ValidationError(violations);
                }
            }

            if (string.IsNullOrEmpty(entry.PromotionTitle))
            {
                violations.Add(new RuleViolation(ErrorCode.NullTitle, "PromotionTitle", entry.PromotionTitle,
                    ErrorMessage.Messages[ErrorCode.NullTitle]));
                throw new ValidationError(violations);
            }
            else
            {
                if(entity.PromotionTitle != entry.PromotionTitle)
                {
                    bool isDuplicate = UnitOfWork.PromotionRepository.HasPromotionTitleExisted(vendorId, entry.PromotionTitle);
                    if (isDuplicate)
                    {
                        violations.Add(new RuleViolation(ErrorCode.DuplicateTitle, "PromotionTitle", entry.PromotionCode, ErrorMessage.Messages[ErrorCode.DuplicateTitle]));
                        throw new ValidationError(violations);
                    }
                }
            }

            entity.VendorId = vendorId;
            entity.PromotionType = entry.PromotionType;
            entity.PromotionCode = entry.PromotionCode;
            entity.PromotionTitle = entry.PromotionTitle;
            entity.PromotionValue = entry.PromotionValue;
            entity.IsPercent = entry.IsPercent;
            entity.StartDate = entry.StartDate;
            entity.EndDate = entry.EndDate;
            entity.Description = entry.Description;
            entity.IsActive = entry.IsActive;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedDate = DateTime.UtcNow;
            entity.LastModifiedByUserId = userId;

            UnitOfWork.PromotionRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdatePromotionStatus(Guid userId, int id, PromotionStatus status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.PromotionRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundPromotion, "Promotion"));
                throw new ValidationError(violations);
            }

            var isValid = Enum.IsDefined(typeof(PromotionStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status", null,
                    ErrorMessage.Messages[ErrorCode.InvalidStatus]));
                throw new ValidationError(violations);
            }
            if (entity.IsActive == status) return;

            entity.IsActive = status;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedDate = DateTime.UtcNow;
            entity.LastModifiedByUserId = userId;

            UnitOfWork.PromotionRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region Dipose

        private bool _disposed;
        protected override void Dispose(bool isDisposing)
        {
            if (!_disposed)
            {
                if (isDisposing)
                {
                    CommonService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}
