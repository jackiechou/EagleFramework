using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Entities.Business.Shipping;
using Eagle.Repositories;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.EntityMapping.Common;
using Eagle.Services.Validations;
using System.Linq;
using Eagle.Services.Dtos.Business.Transaction;

namespace Eagle.Services.Business
{
    public class ShippingService : BaseService, IShippingService
    {
        public ICurrencyService CurrencyService { get; set; }

        public ShippingService(IUnitOfWork unitOfWork, ICurrencyService currencyService) : base(unitOfWork)
        {
            CurrencyService = currencyService;
        }

        #region Shipping Method
        public IEnumerable<ShippingMethodDetail> GetShippingMethods(ShippingMethodSearchEntry filter, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = UnitOfWork.ShippingMethodRepository.GetShippingMethods(filter.ShippingMethodName, filter.IsActive, ref recordCount, orderBy, page, pageSize);
            return lst.ToDtos<ShippingMethod, ShippingMethodDetail>();
        }
        public List<ShippingMethodInfo> GetShippingMethods(bool? status)
        {
            var lst = new List<ShippingMethodInfo>();
            var shippingMethods = UnitOfWork.ShippingMethodRepository.GetShippingMethods(status).ToList();
            if (shippingMethods.Any())
            {
                foreach (var shippingMethod in shippingMethods)
                {
                    var shippingCarriers = GetShippingCarriersByShippingMethodId(shippingMethod.ShippingMethodId).ToList();
                    lst.Add(new ShippingMethodInfo
                    {
                        Carriers = shippingCarriers,
                        ShippingMethod = shippingMethod.ToDto<ShippingMethod, ShippingMethodDetail>()
                    });
                }
            }

            return lst;
        }
        public ShippingMethodDetail GetShippingMethodDetail(int id)
        {
            var entity = UnitOfWork.ShippingMethodRepository.FindById(id);
            return entity.ToDto<ShippingMethod, ShippingMethodDetail>();
        }
        public SelectList PopulateShippingMethodStatus(bool? selectedValue = true, bool isShowSelectText = false)
        {
            return UnitOfWork.ShippingMethodRepository.PopulateShippingMethodStatus(selectedValue, isShowSelectText);
        }
        public SelectList PopulateShippingMethodSelectList(bool? status = null, int? selectedValue = null,
           bool? isShowSelectText = true)
        {
            return UnitOfWork.ShippingMethodRepository.PopulateShippingMethodSelectList(status, selectedValue, isShowSelectText);
        }
        public ShippingMethodDetail InsertShippingMethod(ShippingMethodEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullShippingMethodEntry, "ShippingMethodEntry", null, ErrorMessage.Messages[ErrorCode.NullShippingMethodEntry]));
                throw new ValidationError(violations);
            }

            if (string.IsNullOrEmpty(entry.ShippingMethodName))
            {
                violations.Add(new RuleViolation(ErrorCode.NullShippingMethodName, "ShippingMethodName", null, ErrorMessage.Messages[ErrorCode.NullShippingMethodName]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.ShippingMethodName.Length > 300)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidShippingMethodName, "ShippingMethodName", null, ErrorMessage.Messages[ErrorCode.InvalidShippingMethodName]));
                    throw new ValidationError(violations);
                }
                else
                {
                    bool isDuplicate = UnitOfWork.ShippingMethodRepository.HasDataExisted(entry.ShippingMethodName);
                    if (isDuplicate)
                    {
                        violations.Add(new RuleViolation(ErrorCode.DuplicateShippingMethodName, "ShippingMethodName",
                                entry.ShippingMethodName));
                        throw new ValidationError(violations);
                    }
                }
            }

            var entity = entry.ToEntity<ShippingMethodEntry, ShippingMethod>();
            entity.ListOrder = UnitOfWork.MediaAlbumRepository.GetNewListOrder();

            UnitOfWork.ShippingMethodRepository.Insert(entity);
            UnitOfWork.SaveChanges();
            return entity.ToDto<ShippingMethod, ShippingMethodDetail>();
        }
        public void UpdateShippingMethod(ShippingMethodEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullShippingMethodEditEntry, "ShippingMethodEditEntry", null, ErrorMessage.Messages[ErrorCode.NullShippingMethodEditEntry]));
                throw new ValidationError(violations);
            }

            var entity = UnitOfWork.ShippingMethodRepository.FindById(entry.ShippingMethodId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundShippingMethod, "ShippingMethod", entry.ShippingMethodId, ErrorMessage.Messages[ErrorCode.NotFoundShippingMethod]));
                throw new ValidationError(violations);
            }

            if (string.IsNullOrEmpty(entry.ShippingMethodName))
            {
                violations.Add(new RuleViolation(ErrorCode.NullShippingMethodName, "ShippingMethodName", null, ErrorMessage.Messages[ErrorCode.NullShippingMethodName]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.ShippingMethodName.Length > 300)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidShippingMethodName, "ShippingMethodName", null, ErrorMessage.Messages[ErrorCode.InvalidShippingMethodName]));
                    throw new ValidationError(violations);
                }
                else
                {
                    if (entity.ShippingMethodName != entry.ShippingMethodName)
                    {
                        bool isDuplicate = UnitOfWork.ShippingMethodRepository.HasDataExisted(entry.ShippingMethodName);
                        if (isDuplicate)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicateShippingMethodName, "ShippingMethodName",
                                    entry.ShippingMethodName));
                            throw new ValidationError(violations);
                        }
                    }
                }
            }

            entity.ShippingMethodName = entry.ShippingMethodName;
            entity.IsActive = entry.IsActive;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.ShippingMethodRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateShippingMethodStatus(int id, bool status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ShippingMethodRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundShippingMethod, "ShippingMethod", id, ErrorMessage.Messages[ErrorCode.NotFoundShippingMethod]));
                throw new ValidationError(violations);
            }

            if (entity.IsActive == status) return;

            entity.IsActive = status;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.ShippingMethodRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateShippingMethodListOrder(int id, int listOrder)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ShippingMethodRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundShippingMethod, "ShippingMethod", id, ErrorMessage.Messages[ErrorCode.NotFoundShippingMethod]));
                throw new ValidationError(violations);
            }

            entity.ListOrder = listOrder;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.ShippingMethodRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region Shipping Carrier
        public ShippingCarrierDetail GetSelectedShippingCarriers(int vendorId)
        {
            var carrier = UnitOfWork.ShippingCarrierRepository.GetSelectedShippingCarrier(vendorId);
            return carrier.ToDto<ShippingCarrier, ShippingCarrierDetail>();
        }
        public IEnumerable<ShippingCarrierDetail> GetShippingCarriers(int vendorId, ShippingCarrierSearchEntry filter, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = UnitOfWork.ShippingCarrierRepository.GetShippingCarriers(vendorId, filter.ShippingCarrierName, filter.IsActive, ref recordCount, orderBy, page, pageSize);
            return lst.ToDtos<ShippingCarrier, ShippingCarrierDetail>();
        }

        public IEnumerable<ShippingCarrierDetail> GetShippingCarriersByShippingMethodId(int shippingMethodId)
        {
            var shippingCarrierIds = UnitOfWork.ShippingFeeRepository.GetShippingCarrierProvicedService(shippingMethodId, true).ToList();
            var result = UnitOfWork.ShippingCarrierRepository.GetShippingCarriersByIds(shippingCarrierIds, true);
            return result.ToDtos<ShippingCarrier, ShippingCarrierDetail>();
        }

        public ShippingCarrierDetail GetShippingCarrierDetail(int id)
        {
            var entity = UnitOfWork.ShippingCarrierRepository.FindById(id);
            return entity.ToDto<ShippingCarrier, ShippingCarrierDetail>();
        }

        public SelectList PopulateShippingCarrierStatus(bool? selectedValue = null, bool isShowSelectText = true)
        {
            return UnitOfWork.ShippingCarrierRepository.PopulateShippingCarrierStatus(selectedValue, isShowSelectText);
        }

        public SelectList PopulateShippingCarrierSelectList(bool? status = null, int? selectedValue = null,
            bool? isShowSelectText = true)
        {
            return UnitOfWork.ShippingCarrierRepository.PopulateShippingCarrierSelectList(status, selectedValue, isShowSelectText);
        }

        public ShippingCarrierDetail InsertShippingCarrier(int vendorId, ShippingCarrierEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullShippingCarrierEntry, "ShippingCarrierEntry", null, ErrorMessage.Messages[ErrorCode.NullShippingCarrierEntry]));
                throw new ValidationError(violations);
            }

            if (string.IsNullOrEmpty(entry.ShippingCarrierName))
            {
                violations.Add(new RuleViolation(ErrorCode.NullShippingCarrierName, "ShippingCarrierName", null, ErrorMessage.Messages[ErrorCode.NullShippingCarrierName]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.ShippingCarrierName.Length > 300)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidShippingCarrierName, "ShippingCarrierName", null, ErrorMessage.Messages[ErrorCode.InvalidShippingCarrierName]));
                    throw new ValidationError(violations);
                }
                else
                {
                    bool isDuplicate = UnitOfWork.ShippingCarrierRepository.HasDataExisted(vendorId, entry.ShippingCarrierName);
                    if (isDuplicate)
                    {
                        violations.Add(new RuleViolation(ErrorCode.DuplicateShippingCarrierName, "ShippingCarrierName",
                                entry.ShippingCarrierName));
                        throw new ValidationError(violations);
                    }
                }
            }

            var entity = entry.ToEntity<ShippingCarrierEntry, ShippingCarrier>();
            entity.ListOrder = UnitOfWork.MediaAlbumRepository.GetNewListOrder();
            entity.IsSelected = false;
            entity.VendorId = vendorId;

            UnitOfWork.ShippingCarrierRepository.Insert(entity);
            UnitOfWork.SaveChanges();
            return entity.ToDto<ShippingCarrier, ShippingCarrierDetail>();
        }
        public void UpdateShippingCarrier(int vendorId, ShippingCarrierEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullShippingCarrierEditEntry, "ShippingCarrierEditEntry", null, ErrorMessage.Messages[ErrorCode.NullShippingCarrierEditEntry]));
                throw new ValidationError(violations);
            }

            var entity = UnitOfWork.ShippingCarrierRepository.FindById(entry.ShippingCarrierId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundShippingCarrier, "ShippingCarrier", entry.ShippingCarrierId, ErrorMessage.Messages[ErrorCode.NotFoundShippingCarrier]));
                throw new ValidationError(violations);
            }

            if (string.IsNullOrEmpty(entry.ShippingCarrierName))
            {
                violations.Add(new RuleViolation(ErrorCode.NullShippingCarrierName, "ShippingCarrierName", null, ErrorMessage.Messages[ErrorCode.NullShippingCarrierName]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.ShippingCarrierName.Length > 300)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidShippingCarrierName, "ShippingCarrierName", null, ErrorMessage.Messages[ErrorCode.InvalidShippingCarrierName]));
                    throw new ValidationError(violations);
                }
                else
                {
                    if (entity.ShippingCarrierName != entry.ShippingCarrierName)
                    {
                        bool isDuplicate = UnitOfWork.ShippingCarrierRepository.HasDataExisted(vendorId, entry.ShippingCarrierName);
                        if (isDuplicate)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicateShippingCarrierName, "ShippingCarrierName",
                                    entry.ShippingCarrierName));
                            throw new ValidationError(violations);
                        }
                    }
                }
            }

            entity.ShippingCarrierName = entry.ShippingCarrierName;
            entity.Phone = entry.Phone;
            entity.Address = entry.Address;
            entity.IsActive = entry.IsActive;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.ShippingCarrierRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateSelectedShippingCarrier(int vendorId, int shippingCarrierId)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ShippingCarrierRepository.FindById(shippingCarrierId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundShippingCarrier, "ShippingCarrierId", shippingCarrierId, ErrorMessage.Messages[ErrorCode.NotFoundShippingCarrier]));
                throw new ValidationError(violations);
            }

            if (entity.IsSelected) return;

            var lst = UnitOfWork.ShippingCarrierRepository.GetShippingCarriers(vendorId, null).ToList();
            if (!lst.Any()) return;

            foreach (var item in lst)
            {
                item.IsSelected = (item.ShippingCarrierId == shippingCarrierId);
                UnitOfWork.ShippingCarrierRepository.Update(item);
            }
            UnitOfWork.SaveChanges();
        }

        public void UpdateShippingCarrierStatus(int id, bool status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ShippingCarrierRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundShippingCarrier, "ShippingCarrier", id, ErrorMessage.Messages[ErrorCode.NotFoundShippingCarrier]));
                throw new ValidationError(violations);
            }
            if (entity.IsActive == status) return;

            entity.IsActive = status;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.ShippingCarrierRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateShippingCarrierListOrder(int id, int listOrder)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ShippingCarrierRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundShippingCarrier, "ShippingCarrier", id, ErrorMessage.Messages[ErrorCode.NotFoundShippingCarrier]));
                throw new ValidationError(violations);
            }

            entity.ListOrder = listOrder;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.ShippingCarrierRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }


        #endregion

        #region Shipping Fee

        //CalculateShippingFee
        public ShippingFeeDetail GetShippingFee(ShippingFeeSearchZipCodeEntry filter)
        {
            var query = UnitOfWork.ShippingFeeRepository.GetShippingFee(filter.ShippingCarrierId,
                filter.ShippingMethodId, filter.ZipCode, filter.TotalWeight);
            return query.ToDto<ShippingFee, ShippingFeeDetail>();
        }

        public ShipmentInfo GetShipmentInfo(ShippingFeeSearchZipCodeEntry filter)
        {
            var shipmentInfo = new ShipmentInfo();
            var shippingFee = UnitOfWork.ShippingFeeRepository.Get()
                .FirstOrDefault(p => string.Compare(p.ZipStart, filter.ZipCode, StringComparison.OrdinalIgnoreCase) <= 0
                                     && string.Compare(p.ZipEnd, filter.ZipCode, StringComparison.OrdinalIgnoreCase) >= 0
                                     && p.ShippingCarrierId == filter.ShippingCarrierId && p.ShippingMethodId == filter.ShippingMethodId
                                     && p.WeightStart <= filter.TotalWeight && p.WeightEnd >= filter.TotalWeight
                                     && p.IsActive);
            if (shippingFee != null)
            {
                shipmentInfo.ShippingMethodId = filter.ShippingMethodId;
                shipmentInfo.Weight = filter.TotalWeight;
                shipmentInfo.PackageFee = shippingFee.PackageFee;
                shipmentInfo.RateFee = shippingFee.RateFee;
                shipmentInfo.Vat = shippingFee.Vat;
            }
            return shipmentInfo;
        }

        public IEnumerable<ShippingFeeDetail> GetShippingFees(ShippingFeeSearchEntry filter, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = UnitOfWork.ShippingFeeRepository.GetShippingFees(filter.ShippingFeeName, filter.IsActive, ref recordCount, orderBy, page, pageSize);
            return lst.ToDtos<ShippingFee, ShippingFeeDetail>();
        }
        public ShippingFeeDetail GetShippingFeeDetail(int id)
        {
            var entity = UnitOfWork.ShippingFeeRepository.FindById(id);
            return entity.ToDto<ShippingFee, ShippingFeeDetail>();
        }
        public SelectList PopulateShippingFeeStatus(bool? selectedValue = null, bool isShowSelectText = true)
        {
            return UnitOfWork.ShippingFeeRepository.PopulateShippingFeeStatus(selectedValue, isShowSelectText);
        }
        public ShippingFeeDetail InsertShippingFee(ShippingFeeEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullShippingRateEntry, "ShippingRateEntry", null, ErrorMessage.Messages[ErrorCode.NullShippingRateEntry]));
                throw new ValidationError(violations);
            }

            if (string.IsNullOrEmpty(entry.ShippingFeeName))
            {
                violations.Add(new RuleViolation(ErrorCode.NullShippingRateName, "ShippingRateName", null, ErrorMessage.Messages[ErrorCode.NullShippingRateName]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.ShippingFeeName.Length > 300)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidShippingRateName, "ShippingRateName", null, ErrorMessage.Messages[ErrorCode.InvalidShippingRateName]));
                    throw new ValidationError(violations);
                }
                else
                {
                    bool isDuplicate = UnitOfWork.ShippingFeeRepository.HasDataExisted(entry.ShippingFeeName);
                    if (isDuplicate)
                    {
                        violations.Add(new RuleViolation(ErrorCode.DuplicateShippingRateName, "ShippingRateName",
                                entry.ShippingFeeName, ErrorMessage.Messages[ErrorCode.DuplicateShippingRateName]));
                        throw new ValidationError(violations);
                    }
                }
            }

            var currency = CurrencyService.GetSelectedCurrency();
            var entity = entry.ToEntity<ShippingFeeEntry, ShippingFee>();
            entity.ListOrder = UnitOfWork.ShippingFeeRepository.GetNewListOrder();
            entity.CurrencyCode = currency.CurrencyCode;
            entity.CreatedDate = DateTime.UtcNow;

            UnitOfWork.ShippingFeeRepository.Insert(entity);
            UnitOfWork.SaveChanges();
            return entity.ToDto<ShippingFee, ShippingFeeDetail>();
        }
        public void UpdateShippingFee(ShippingFeeEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullShippingRateEditEntry, "ShippingRateEditEntry", null, ErrorMessage.Messages[ErrorCode.NullShippingRateEditEntry]));
                throw new ValidationError(violations);
            }

            var entity = UnitOfWork.ShippingFeeRepository.FindById(entry.ShippingFeeId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundShippingRate, "ShippingRate", entry.ShippingFeeId, ErrorMessage.Messages[ErrorCode.NotFoundShippingRate]));
                throw new ValidationError(violations);
            }

            if (string.IsNullOrEmpty(entry.ShippingFeeName))
            {
                violations.Add(new RuleViolation(ErrorCode.NullShippingRateName, "ShippingRateName", null, ErrorMessage.Messages[ErrorCode.NullShippingRateName]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.ShippingFeeName.Length > 300)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidShippingRateName, "ShippingFeeName", null, ErrorMessage.Messages[ErrorCode.InvalidShippingRateName]));
                    throw new ValidationError(violations);
                }
                else
                {
                    if (entity.ShippingFeeName != entry.ShippingFeeName)
                    {
                        bool isDuplicate = UnitOfWork.ShippingFeeRepository.HasDataExisted(entry.ShippingFeeName);
                        if (isDuplicate)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicateShippingRateName, "ShippingFeeName",
                                    entry.ShippingFeeName));
                            throw new ValidationError(violations);
                        }
                    }
                }
            }

            entity.ShippingCarrierId = entry.ShippingCarrierId;
            entity.ShippingMethodId = entry.ShippingMethodId;
            entity.ShippingFeeName = entry.ShippingFeeName;
            entity.ZipStart = entry.ZipStart;
            entity.ZipEnd = entry.ZipEnd;
            entity.WeightStart = entry.WeightStart;
            entity.WeightEnd = entry.WeightEnd;
            entity.RateFee = entry.RateFee;
            entity.PackageFee = entry.PackageFee;
            entity.Vat = entry.Vat;
            entity.CurrencyCode = entry.CurrencyCode;
            entity.IsActive = entry.IsActive;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.ShippingFeeRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateShippingFeeStatus(int id, bool status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ShippingFeeRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundShippingRate, "ShippingRate", id, ErrorMessage.Messages[ErrorCode.NotFoundShippingRate]));
                throw new ValidationError(violations);
            }
            if (entity.IsActive == status) return;

            entity.IsActive = status;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.ShippingFeeRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateShippingFeeListOrder(int id, int listOrder)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ShippingFeeRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundShippingRate, "ShippingRate", id, ErrorMessage.Messages[ErrorCode.NotFoundShippingRate]));
                throw new ValidationError(violations);
            }

            entity.ListOrder = listOrder;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.ShippingFeeRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        #endregion

        #region Dipose

        private bool _disposed = false;
        protected override void Dispose(bool isDisposing)
        {
            if (!this._disposed)
            {
                if (isDisposing)
                {
                    CurrencyService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}
