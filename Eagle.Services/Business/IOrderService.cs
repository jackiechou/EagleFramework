using Eagle.Core.Settings;
using Eagle.Entities.Business.Orders;
using Eagle.Services.Dtos.Business;
using System;
using System.Collections.Generic;
using Eagle.Services.Dtos.Business.Transaction;
using Eagle.Services.Dtos.Common;

namespace Eagle.Services.Business
{
    public interface IOrderService: IBaseService
    {
        #region Order
        IEnumerable<OrderInfoDetail> GetOrders(int vendorId, OrderSearchEntry filter, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);

        IEnumerable<OrderInfoDetail> GetListByCustomerNo(string customerNo, OrderStatus? status, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);

        IEnumerable<OrderInfoDetail> GetOrdersByCustomer(OrderSearch filter, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<OrderInfoDetail> GetListByMarkAsRead(MarkAsRead markAsRead, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);
        OrderDetail GetOrderDetail(int id);
        OrderInfoDetail GetOrderDetailByOrderNo(Guid orderNo);
        OrderDetail InsertOrder(int vendorId, OrderEntry entry);
        void UpdateOrder(int id, OrderEntry entry);
        void UpdateOrder(Guid orderNo, OrderEntry entry);
        void UpdateMarkAsRead(Guid orderNo);
        void UpdateOrderStatus(Guid orderNo, OrderStatus status);
        void DeleteOrder(int id);
        #endregion

        #region OrderTemp

        OrderTempInfoDetail GetOrderTempByOrderNo(string orderNo);
        OrderTempDetail InsertOrderTemp(int vendorId, OrderEntry entry);
        void UpdateOrderTemp(int id, OrderEntry entry);
        void UpdateOrderTemp(Guid orderNo, OrderEntry entry);
        void ClearOrderTemp(Guid orderNo);

        #endregion

        #region Order Product
        IEnumerable<OrderProductDetail> GetOrderProducts(Guid orderNo);
        IEnumerable<OrderProductInfoDetail> GetOrderProducts(int vendorId, ItemType type, DateTime? startDate,
            DateTime? endDate, OrderProductStatus? status);
        IEnumerable<OrderProductInfoDetail> GetOrderProductsByOrderNo(Guid orderNo);
        IEnumerable<OrderProductDetail> GetOrderProductsByCustomer(int vendorId, int customerId);
        OrderProductInfoDetail GetOrderProductDetails(int orderProductId);
        OrderProductDetail GetOrderProductDetails(Guid orderNo, int productId);
        bool HasOrderProductExisted(int customerId, Guid orderNo, int productId);
        OrderProductDetail InsertOrderProduct(OrderProductEntry entry);
        OrderProductDetail UpdateOrderProduct(OrderProductEditEntry entry);
        void RemoveOrderProduct(Guid orderNo);
        void RemoveOrderProduct(Guid orderNo, int productId);
        ItemDetail GetOItemDetailsByProductId(int productId, ItemType typeId);
        #endregion

        #region Order Product Temp
        OrderProductDetail InsertOrderProductTemp(OrderProductTempEntry entry);
        OrderProductDetail UpdateOrderProductTemp(OrderProductTempEditEntry entry);
        void RemoveOrderProductTemp(Guid orderNo);
        void RemoveOrderProductTemp(Guid orderNo, int productId);
        #endregion

        #region Order Shipment

        OrderShipmentDetail InsertOrderShipment(OrderShipmentEntry entry);

        OrderShipmentDetail UpdateOrderShipment(OrderShipmentEditEntry entry);

        #endregion

        #region Order Payment

        OrderPaymentDetail InsertOrderPayment(OrderPaymentEntry entry);

        OrderPaymentDetail UpdateOrderPayment(OrderPaymentEditEntry entry);


        #endregion

        #region Order Promotion
        PromotionDetail GetPromotion(int vendorId, string promotionCode, DateTime date);
        void ApplyPromotion(int vendorId, string promotionCode, ShoppingCart cart);
        void RemovePromotion(ShoppingCart cart);
        #endregion

        #region Order Purchase
        bool ValidateUnitsInStock(List<OrderProductInfoDetail> products);
        bool ValidatePromotionCodeAvailable(string promotionCode);
        List<RuleViolation> ValidateTransaction(OrderTransactionEntry order);
        TransactionState PurchaseOrder(Guid applicationId, int vendorId, OrderTransactionEntry order);
        void UpdateOrderStatus(OrderTransactionEntry order);
        bool ProcessPayment(Guid applicationId, OrderPaymentEntry order, out string statePayment);
        #endregion
    }
}
