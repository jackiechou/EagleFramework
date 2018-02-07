using System;
using System.Collections.Generic;
using Eagle.Core.Settings;
using Eagle.Services.Dtos.Business;

namespace Eagle.Services.Business
{
    public interface ITransactionService : IBaseService
    {
        #region Transaction Method
        IEnumerable<TransactionMethodDetail> GetTransactionMethods(TransactionMethodSearchEntry filter, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);

        IEnumerable<TransactionMethodDetail> GetTransactionMethods(TransactionMethodStatus? status);
        TransactionMethodDetail GetTransactionMethodDetail(int id);
        void InsertTransactionMethod(TransactionMethodEntry entry);
        void UpdateTransactionMethod(TransactionMethodEditEntry entry);
        void UpdateTransactionMethodStatus(int id, TransactionMethodStatus status);
        #endregion

        #region Payment Method

        IEnumerable<PaymentMethodDetail> GetPaymentMethods(PaymentMethodSearchEntry filter, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<PaymentMethodDetail> GetPaymentMethods(PaymentMethodStatus? status=null);
        PaymentMethodDetail GetPaymentMethodDetail(int id);
        void InsertPaymentMethod(PaymentMethodEntry entry);
        void UpdatePaymentMethod(PaymentMethodEditEntry entry);
        void UpdateSelectedPaymentMethod(int paymentMethodId);
        void UpdatePaymentMethodStatus(int id, PaymentMethodStatus status);

        #endregion

        #region Promotion

        IEnumerable<PromotionDetail> GetPromotions(int vendorId, PromotionSearchEntry filter,
            ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        bool HasPromotionCodeExisted(int vendorId, string promotionCode);
        string GeneratePromotionCode(int maxLength);
        PromotionDetail GetPromotionDetailByCode(int vendorId, string promotionCode);
        PromotionDetail GetPromotionDetail(int id);
        void InsertPromotion(Guid userId, int vendorId, PromotionEntry entry);
        void UpdatePromotion(Guid userId, int vendorId, PromotionEditEntry entry);
        void UpdatePromotionStatus(Guid userId, int id, PromotionStatus status);

        #endregion
    }
}
