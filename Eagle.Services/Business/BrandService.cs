using System;
using System.Collections.Generic;
using System.Linq;
using Eagle.Core.Permission;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Brand;
using Eagle.Repositories;
using Eagle.Services.Business.Validation;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.EntityMapping.Common;
using Eagle.Services.Validations;

namespace Eagle.Services.Business
{
    public class BrandService: BaseService, IBrandService
    {

        public BrandService(IUnitOfWork unitOfWork): base(unitOfWork)
        {
        }

        public void CreateBrand(BrandEntry entry)
        {
            var entity = entry.ToEntity<BrandEntry, Brand>();
            var violations = new List<RuleViolation>();
            ISpecification<BrandEntry> validator = new BrandEntryValidator(UnitOfWork, PermissionLevel.Create,
                CurrentClaimsIdentity);
            if (!validator.IsSatisfyBy(entry, violations))
            {
                throw new ValidationError(violations);
            }

            UnitOfWork.BrandRepository.Insert(entity);
            UnitOfWork.SaveChanges();
        }

        public void DeleteBrand(int brandId)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.BrandRepository.FindById(brandId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundBrand, "Brand", brandId, ErrorMessage.Messages[ErrorCode.NotFoundBrand]));
                throw new ValidationError(violations);
            }
            if (UnitOfWork.ProductRepository.HasBrandExisted(brandId))
            {
                violations.Add(new RuleViolation(ErrorCode.ExistedProductCanNotDeleteBrand, "Brand", brandId, ErrorMessage.Messages[ErrorCode.ExistedProductCanNotDeleteBrand]));
            }
            UnitOfWork.BrandRepository.Delete(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateBrand(BrandEditEntry brand)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.BrandRepository.FindById(brand.BrandId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundBrand, "Brand", brand.BrandId, ErrorMessage.Messages[ErrorCode.NotFoundBrand]));
                throw new ValidationError(violations);
            }

            ISpecification<BrandEditEntry> validator = new BrandEditEntryValidator(UnitOfWork, PermissionLevel.Edit,
                CurrentClaimsIdentity);
            if (!validator.IsSatisfyBy(brand, violations))
            {
                throw new ValidationError(violations);
            }

            entity.BrandAlias = brand.BrandAlias;
            entity.BrandName = brand.BrandName;
            entity.IsOnline = brand.IsOnline == BrandStatus.Active;

            UnitOfWork.BrandRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateStatus(int brandId, BrandStatus status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.BrandRepository.FindById(brandId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundBrand, "Brand", brandId, ErrorMessage.Messages[ErrorCode.NotFoundBrand]));
                throw new ValidationError(violations);
            }
            
            entity.IsOnline = status == BrandStatus.Active;

            UnitOfWork.BrandRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public IEnumerable<BrandInfo> GetBrandList(BrandSearchEntry brandSearchEntry, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null)
        {
            var list = UnitOfWork.BrandRepository.GetBrandList(brandSearchEntry.SearchText, brandSearchEntry.IsOnline.HasValue ? brandSearchEntry.IsOnline.Value == BrandStatus.Active: (bool?)null ,
                ref recordCount, orderBy, page, pageSize);
            return list.ToDtos<Brand, BrandInfo>();
        }

        public BrandDetail GetBrandDetail(int brandId)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.BrandRepository.FindById(brandId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundBrand, "Brand", brandId, ErrorMessage.Messages[ErrorCode.NotFoundBrand]));
                throw new ValidationError(violations);
            }
            return entity.ToDto<Brand, BrandDetail>();
        }

        private bool _disposed;
        protected override void Dispose(bool isDisposing)
        {
            if (!this._disposed)
            {
                if (isDisposing)
                {
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }
        
    }
}
