using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Eagle.Common.Utilities;
using Eagle.Core.Configuration;
using Eagle.Core.Permission;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Employees;
using Eagle.Entities.Common;
using Eagle.Entities.Services.Booking;
using Eagle.Repositories;
using Eagle.Resources;
using Eagle.Services.Business;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Business.Personnel;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Services.Booking;
using Eagle.Services.Dtos.SystemManagement.Identity;
using Eagle.Services.EntityMapping.Common;
using Eagle.Services.Exceptions;
using Eagle.Services.Messaging;
using Eagle.Services.Service.Validation;
using Eagle.Services.SystemManagement;
using Eagle.Services.Validations;

namespace Eagle.Services.Service
{
    public class BookingService : BaseService, IBookingService
    {
        #region Contruct
        public IApplicationService ApplicationService { get; set; }
        private ICustomerService CustomerService { get; set; }
        private IEmployeeService EmployeeService { get; set; }
        public ICurrencyService CurrencyService { get; set; }
        private IContactService ContactService { get; set; }
        public IDocumentService DocumentService { get; set; }
        public IOrderService OrderService { get; set; }
        public IMailService MailService { get; set; }
        public IMessageService MessageService { get; set; }
        public INotificationService NotificationService { get; set; }

        public BookingService(IUnitOfWork unitOfWork, IApplicationService applicationService,
            IOrderService orderService, ICustomerService customerService, IEmployeeService employeeService, ICurrencyService currencyService,
            IContactService contactService, IDocumentService documentService,
            IMailService mailService, IMessageService messageService,
            INotificationService notificationService) : base(unitOfWork)
        {
            ApplicationService = applicationService;
            CustomerService = customerService;
            EmployeeService = employeeService;
            CurrencyService = currencyService;
            ContactService = contactService;
            DocumentService = documentService;
            OrderService = orderService;
            MailService = mailService;
            MessageService = messageService;
            NotificationService = notificationService;
        }

        #endregion

        #region Service Category

        public IEnumerable<TreeDetail> GetServiceCategorySelectTree(ServiceType typeId = ServiceType.Single, ServiceCategoryStatus? status = null, int? selectedId = null, bool? isRootShowed = false)
        {
            var lst = UnitOfWork.ServiceCategoryRepository.GetServiceCategorySelectTree(typeId, status, selectedId, isRootShowed);
            return lst.ToDtos<TreeEntity, TreeDetail>();
        }

        public IEnumerable<TreeDetail> GetServiceCategoryTree(ServiceCategoryStatus? status, int? selectedId = null, bool? isRootShowed = true)
        {
            var list = UnitOfWork.ServiceCategoryRepository.GetListByStatus(status).Select(p => new TreeDetail
            {
                id = p.CategoryId,
                key = p.CategoryId,
                name = p.CategoryName,
                title = p.CategoryName,
                text = p.CategoryName,
                parentid = p.ParentId,
                depth = p.Depth,
                hasChild = p.HasChild ?? false,
                folder = (p.HasChild != null && p.HasChild == true),
                lazy = (p.HasChild != null && p.HasChild == true),
                expanded = (p.HasChild != null && p.HasChild == true),
                selected = (selectedId != null && p.CategoryId == selectedId),
                tooltip = p.CategoryName
            }).ToList();

            var recursiveObjects = new List<TreeDetail>();
            if (list.Any())
            {
                recursiveObjects = RecursiveFillTree(list, null, selectedId);
            }

            if (isRootShowed != null && isRootShowed == true)
            {
                recursiveObjects.Insert(0, new TreeDetail { id = 0, key = 0, parentid = 0, name = " --- " + LanguageResource.Root + " --- ", title = " --- " + LanguageResource.Root + " --- ", text = " --- " + LanguageResource.Root + " --- ", depth = 1, hasChild = true, folder = true, lazy = true, expanded = true, selected = (selectedId == null || selectedId == 0) });
            }
            else
            {
                if (!list.Any())
                {
                    recursiveObjects.Insert(0,
                        new TreeDetail
                        {
                            id = 0,
                            key = 0,
                            parentid = 0,
                            name = " --- " + LanguageResource.NonSpecified + " --- ",
                            title = " --- " + LanguageResource.NonSpecified + " --- ",
                            text = " --- " + LanguageResource.NonSpecified + " --- ",
                            depth = 1,
                            hasChild = true,
                            folder = true,
                            lazy = true,
                            expanded = true,
                            selected = (selectedId == null || selectedId == 0)
                        });
                }
            }

            return recursiveObjects;
        }

        private List<TreeDetail> RecursiveFillTree(IEnumerable<TreeDetail> elements, int? parentid, int? selectedId)
        {
            if (elements == null) return null;
            List<TreeDetail> items = new List<TreeDetail>();
            List<TreeDetail> children = elements.Where(p => p.parentid == (parentid ?? 0)).Select(
               p => new TreeDetail
               {
                   id = p.id,
                   key = p.key,
                   name = p.name,
                   title = p.title,
                   text = p.text,
                   parentid = p.parentid,
                   depth = p.depth,
                   hasChild = p.hasChild ?? false,
                   folder = (p.hasChild != null && p.hasChild == true),
                   lazy = (p.hasChild != null && p.hasChild == true),
                   expanded = (p.hasChild != null && p.hasChild == true),
                   selected = (selectedId != null && p.id == selectedId),
                   tooltip = p.tooltip
               }).ToList();

            if (children.Count > 0)
            {
                items.AddRange(children.Select(child => new TreeDetail()
                {
                    id = child.id,
                    key = child.key,
                    parentid = child.parentid,
                    name = child.name,
                    title = child.title,
                    text = child.text,
                    depth = child.depth,
                    hasChild = child.hasChild ?? false,
                    folder = (child.hasChild != null && child.hasChild == true),
                    lazy = (child.hasChild != null && child.hasChild == true),
                    expanded = (child.hasChild != null && child.hasChild == true),
                    selected = (selectedId != null && child.id == selectedId),
                    tooltip = child.tooltip,
                    children = RecursiveFillTree(elements, child.key, selectedId)
                }));
            }
            return items;
        }

        public SelectList GetServiceCategoryChildList(ServiceCategoryStatus? status = null, int? selectedValue = null, bool? isShowSelectText = true)
        {
            var lst = UnitOfWork.ServiceCategoryRepository.GetServiceCategoryChildList(status, selectedValue, isShowSelectText);
            return lst;
        }

        public SelectList GetServiceCategoryChildListByCode(string discountCode, ServiceCategoryStatus? status = null, int? selectedValue = null, bool? isShowSelectText = true)
        {
            var lst = UnitOfWork.ServiceCategoryRepository.GetServiceCategoryChildListByCode(discountCode, status, selectedValue, isShowSelectText);
            return lst;
        }

        public ServiceCategoryDetail GetServiceCategoryDetail(int id)
        {
            var entity = UnitOfWork.ServiceCategoryRepository.FindById(id);
            return entity.ToDto<ServiceCategory, ServiceCategoryDetail>();
        }

        public void InsertServiceCategory(Guid userId, ServiceCategoryEntry entry)
        {
            ISpecification<ServiceCategoryEntry> validator = new ServiceCategoryEntryValidator(UnitOfWork, PermissionLevel.Create, CurrentClaimsIdentity);
            var dataViolations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isDataValid) throw new ValidationError(dataViolations);

            var entity = entry.ToEntity<ServiceCategoryEntry, ServiceCategory>();
            entity.HasChild = false;
            entity.ListOrder = UnitOfWork.NewsRepository.GetNewListOrder();
            entity.CreatedDate = DateTime.UtcNow;
            entity.Ip = NetworkUtils.GetIP4Address();
            entity.CreatedByUserId = userId;

            UnitOfWork.ServiceCategoryRepository.Insert(entity);
            UnitOfWork.SaveChanges();

            if (entry.ParentId != null && entry.ParentId > 0)
            {
                var parentEntity = UnitOfWork.ServiceCategoryRepository.FindById(Convert.ToInt32(entry.ParentId));
                if (parentEntity == null) return;

                parentEntity.HasChild = true;
                UnitOfWork.ServiceCategoryRepository.Update(parentEntity);

                var lineage = $"{parentEntity.Lineage},{entity.CategoryId}";
                entity.Lineage = lineage;
                entity.Depth = lineage.Split(',').Count();
                entity.ParentId = entry.ParentId;
            }
            else
            {
                entity.ParentId = 0;
                entity.Lineage = $"{entity.CategoryId}";
                entity.Depth = 1;
            }

            UnitOfWork.ServiceCategoryRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateServiceCategory(Guid userId, ServiceCategoryEditEntry entry)
        {
            ISpecification<ServiceCategoryEditEntry> validator = new ServiceCategoryEditEntryValidator(UnitOfWork, PermissionLevel.Edit, CurrentClaimsIdentity);
            var dataViolations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isDataValid) throw new ValidationError(dataViolations);

            var entity = UnitOfWork.ServiceCategoryRepository.FindById(entry.CategoryId);
            if (entity == null) return;

            if (entity.CategoryName != entry.CategoryName)
            {
                bool isDuplicate = UnitOfWork.NewsCategoryRepository.HasDataExisted(entry.CategoryName, entry.ParentId);
                if (isDuplicate)
                {
                    dataViolations.Add(new RuleViolation(ErrorCode.DuplicateCategoryName, "CategoryName", entry.CategoryName, ErrorMessage.Messages[ErrorCode.DuplicateCategoryName]));
                    throw new ValidationError(dataViolations);
                }
            }

            if (entry.ParentId != entity.ParentId && entry.ParentId != entity.CategoryId)
            {
                if (entry.ParentId != null && entry.ParentId > 0)
                {
                    var children = UnitOfWork.ServiceCategoryRepository.GetAllChildrenNodesOfSelectedNode(Convert.ToInt32(entry.CategoryId)).ToList();
                    if (children.Any())
                    {
                        var parentIds = children.Select(x => x.CategoryId).ToList();
                        if (parentIds.Contains(Convert.ToInt32(entry.ParentId)))
                        {
                            dataViolations.Add(new RuleViolation(ErrorCode.InvalidParentId, "ParentId", entry.ParentId, ErrorMessage.Messages[ErrorCode.InvalidParentId]));
                            throw new ValidationError(dataViolations);
                        }
                    }

                    //Update parent entry
                    var parentEntryEntity = UnitOfWork.ServiceCategoryRepository.FindById(Convert.ToInt32(entry.ParentId));
                    if (parentEntryEntity == null)
                    {
                        dataViolations.Add(new RuleViolation(ErrorCode.NotFoundParentId, "ParentId", entry.ParentId, ErrorMessage.Messages[ErrorCode.NotFoundParentId]));
                        throw new ValidationError(dataViolations);
                    }
                    else
                    {
                        if (parentEntryEntity.HasChild == null || parentEntryEntity.HasChild == false)
                        {
                            parentEntryEntity.HasChild = true;
                            UnitOfWork.ServiceCategoryRepository.Update(parentEntryEntity);
                        }
                    }

                    //Update parent entity
                    var parentEntity = UnitOfWork.ServiceCategoryRepository.FindById(entity.ParentId);
                    if (parentEntity != null)
                    {
                        var childList = UnitOfWork.ServiceCategoryRepository.GetAllChildrenNodesOfSelectedNode(Convert.ToInt32(entity.ParentId)).ToList();
                        if (childList.Any())
                        {
                            childList = childList.Where(x => (x.CategoryId != entity.ParentId) && (x.CategoryId != entity.CategoryId)).ToList();
                            parentEntity.HasChild = childList.Any();
                            UnitOfWork.ServiceCategoryRepository.Update(parentEntity);
                        }
                    }

                    var lineage = $"{parentEntryEntity.Lineage},{entry.CategoryId}";
                    entity.Lineage = lineage;
                    entity.Depth = lineage.Split(',').Count();
                    entity.ParentId = entry.ParentId;
                }
                else
                {
                    entity.ParentId = 0;
                    entity.Lineage = $"{entry.CategoryId}";
                    entity.Depth = 1;
                }
            }

            //Update entity

            var hasChild = UnitOfWork.ServiceCategoryRepository.HasChild(entity.CategoryId);
            entity.HasChild = hasChild;
            entity.TypeId = entry.TypeId;
            entity.CategoryName = entry.CategoryName;
            entity.Status = entry.Status;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedByUserId = userId;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.ServiceCategoryRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateServiceCategoryStatus(Guid userId, int id, ServiceCategoryStatus status)
        {
            var entity = UnitOfWork.ServiceCategoryRepository.FindById(id);
            if (entity == null) throw new NotFoundDataException();

            var violations = new List<RuleViolation>();
            var isValid = Enum.IsDefined(typeof(ServiceCategoryStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status", status, ErrorMessage.Messages[ErrorCode.InvalidStatus]));
                throw new ValidationError(violations);
            }
            if (entity.Status == status) return;

            entity.Status = status;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedByUserId = userId;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.ServiceCategoryRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateServiceCategoryListOrder(Guid userId, int id, int listOrder)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ServiceCategoryRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundProductCategory, "ServiceCategory", id, ErrorMessage.Messages[ErrorCode.NotFoundProductCategory]));
                throw new ValidationError(violations);
            }

            if (entity.ListOrder == listOrder) return;

            entity.ListOrder = listOrder;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedByUserId = userId;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.ServiceCategoryRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void DeleteServiceCategory(int categoryId)
        {
            var violations = new List<RuleViolation>();
            var packages = UnitOfWork.ServicePackRepository.GetServicePacks(categoryId, null).ToList();
            if (packages.Any())
            {
                violations.Add(new RuleViolation(ErrorCode.DependentProductsBasedOnCategory, "Products", categoryId, ErrorMessage.Messages[ErrorCode.DependentProductsBasedOnCategory]));
                throw new ValidationError(violations);
            }
            var children = UnitOfWork.ServiceCategoryRepository.GetAllChildrenNodesOfSelectedNode(categoryId);
            if (children.Any())
            {
                violations.Add(new RuleViolation(ErrorCode.DependentSubCategoriesBasedOnCategory, "SubCategory", categoryId, ErrorMessage.Messages[ErrorCode.DependentProductsBasedOnCategory]));
                throw new ValidationError(violations);
            }
            UnitOfWork.ServiceCategoryRepository.Delete(categoryId);
            UnitOfWork.SaveChanges();
        }

        #endregion

        #region Service Pack
        public IEnumerable<ServicePackInfoDetail> GetServicePacks(ServicePackSearchEntry filter, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = new List<ServicePackInfoDetail>();
            var packages = UnitOfWork.ServicePackRepository.GetServicePacks(filter.ServicePackName, filter.CategoryId, filter.ServiceType, filter.Status, ref recordCount, orderBy, page, pageSize);
            if (packages != null)
            {
                lst.AddRange(from p in packages
                             let serviceTaxRate = p.Tax
                             where serviceTaxRate != null
                             let taxRate = (serviceTaxRate != null && serviceTaxRate.IsPercent) ? (Convert.ToDecimal(p.PackageFee) * serviceTaxRate.TaxRate) / 100 : serviceTaxRate.TaxRate
                             let options = UnitOfWork.ServicePackOptionRepository.GetServicePackOptions(p.PackageId)
                             let discount = p.Discount
                             where discount != null
                             let discountRate = (discount != null && discount.IsPercent) ? (Convert.ToDecimal(p.PackageFee) * discount.DiscountRate) / 100 : discount.DiscountRate
                             select new ServicePackInfoDetail
                             {
                                 PackageId = p.PackageId,
                                 PackageCode = p.PackageCode,
                                 PackageName = p.PackageName,
                                 CategoryId = p.CategoryId,
                                 TypeId = p.TypeId,
                                 AvailableQuantity = p.AvailableQuantity,
                                 Capacity = p.Capacity,
                                 DurationId = p.DurationId,
                                 DiscountId = p.DiscountId,
                                 TaxRateId = p.TaxRateId,
                                 Weight = p.Weight,
                                 PackageFee = p.PackageFee,
                                 TotalFee = p.TotalFee,
                                 TaxRate = taxRate,
                                 DiscountRate = discountRate,
                                 CurrencyCode = p.CurrencyCode,
                                 FileId = p.FileId,
                                 Description = !string.IsNullOrEmpty(p.Description) ? HttpUtility.HtmlDecode(p.Description) : string.Empty,
                                 Specification = !string.IsNullOrEmpty(p.Specification) ? HttpUtility.HtmlDecode(p.Specification) : string.Empty,
                                 FileUrl = (p.FileId != null && p.FileId > 0) ? DocumentService.GetFileInfoDetail(Convert.ToInt32(p.FileId)).FileUrl : GlobalSettings.NotFoundFileUrl,
                                 Rating = p.Rating,
                                 TotalViews = p.TotalViews,
                                 ListOrder = p.ListOrder,
                                 Status = p.Status,
                                 CreatedDate = p.CreatedDate,
                                 LastModifiedDate = p.LastModifiedDate,
                                 Category = p.Category.ToDto<ServiceCategory, ServiceCategoryDetail>(),
                                 Type = p.Type.ToDto<ServicePackType, ServicePackTypeDetail>(),
                                 Period = p.Period.ToDto<ServicePeriod, ServicePeriodDetail>(),
                                 Discount = p.Discount.ToDto<ServiceDiscount, ServiceDiscountDetail>(),
                                 Tax = p.Tax.ToDto<ServiceTaxRate, ServiceTaxRateDetail>(),
                                 Duration = p.Duration.ToDto<ServicePackDuration, ServicePackDurationDetail>(),
                                 Document = DocumentService.GetFileInfoDetail(p.FileId ?? 0),
                                 Options = options.ToDtos<ServicePackOption, ServicePackOptionDetail>(),
                                 Employees = GetServicePackProviders(p.PackageId)
                             });
            }
            return lst;
        }
        public IEnumerable<ServicePackInfoDetail> GetServicePacks(int categoryId, ServicePackStatus? status)
        {
            var lst = new List<ServicePackInfoDetail>();
            var packages = UnitOfWork.ServicePackRepository.GetServicePacks(categoryId, status);
            if (packages != null)
            {
                lst.AddRange(from p in packages
                             let serviceTaxRate = p.Tax
                             where serviceTaxRate != null
                             let taxRate = (serviceTaxRate != null && serviceTaxRate.IsPercent) ? (Convert.ToDecimal(p.PackageFee) * serviceTaxRate.TaxRate) / 100 : serviceTaxRate.TaxRate
                             let options = UnitOfWork.ServicePackOptionRepository.GetServicePackOptions(p.PackageId)
                             let discount = p.Discount
                             where discount != null
                             let discountRate = (discount != null && discount.IsPercent) ? (Convert.ToDecimal(p.PackageFee) * discount.DiscountRate) / 100 : discount.DiscountRate
                             select new ServicePackInfoDetail
                             {
                                 PackageId = p.PackageId,
                                 PackageCode = p.PackageCode,
                                 PackageName = p.PackageName,
                                 CategoryId = p.CategoryId,
                                 TypeId = p.TypeId,
                                 AvailableQuantity = p.AvailableQuantity,
                                 Capacity = p.Capacity,
                                 DurationId = p.DurationId,
                                 DiscountId = p.DiscountId,
                                 TaxRateId = p.TaxRateId,
                                 Weight = p.Weight,
                                 PackageFee = p.PackageFee,
                                 TotalFee = p.TotalFee,
                                 TaxRate = taxRate,
                                 DiscountRate = discountRate,
                                 CurrencyCode = p.CurrencyCode,
                                 FileId = p.FileId,
                                 Description = !string.IsNullOrEmpty(p.Description) ? HttpUtility.HtmlDecode(p.Description) : string.Empty,
                                 Specification = !string.IsNullOrEmpty(p.Specification) ? HttpUtility.HtmlDecode(p.Specification) : string.Empty,
                                 FileUrl = (p.FileId != null && p.FileId > 0) ? DocumentService.GetFileInfoDetail(Convert.ToInt32(p.FileId)).FileUrl : GlobalSettings.NotFoundFileUrl,
                                 Rating = p.Rating,
                                 TotalViews = p.TotalViews,
                                 ListOrder = p.ListOrder,
                                 Status = p.Status,
                                 CreatedDate = p.CreatedDate,
                                 LastModifiedDate = p.LastModifiedDate,
                                 Category = p.Category.ToDto<ServiceCategory, ServiceCategoryDetail>(),
                                 Type = p.Type.ToDto<ServicePackType, ServicePackTypeDetail>(),
                                 Period = p.Period.ToDto<ServicePeriod, ServicePeriodDetail>(),
                                 Discount = p.Discount.ToDto<ServiceDiscount, ServiceDiscountDetail>(),
                                 Tax = p.Tax.ToDto<ServiceTaxRate, ServiceTaxRateDetail>(),
                                 Duration = p.Duration.ToDto<ServicePackDuration, ServicePackDurationDetail>(),
                                 Document = DocumentService.GetFileInfoDetail(p.FileId ?? 0),
                                 Options = options.ToDtos<ServicePackOption, ServicePackOptionDetail>(),
                                 Employees = GetServicePackProviders(p.PackageId),
                                 Ratings = GetServicePackRatings(p.PackageId)
                             });
            }
            return lst;
        }
        public IEnumerable<ServicePackInfoDetail> GetServicePacks(int typeId, int categoryId, ServicePackStatus? status)
        {
            var lst = new List<ServicePackInfoDetail>();
            var packages = UnitOfWork.ServicePackRepository.GetServicePacks(typeId, categoryId, status);
            if (packages != null)
            {
                lst.AddRange(from p in packages
                             let serviceTaxRate = p.Tax
                             where serviceTaxRate != null
                             let taxRate = (serviceTaxRate != null && serviceTaxRate.IsPercent) ? (Convert.ToDecimal(p.PackageFee) * serviceTaxRate.TaxRate) / 100 : serviceTaxRate.TaxRate
                             let options = UnitOfWork.ServicePackOptionRepository.GetServicePackOptions(p.PackageId)
                             let discount = p.Discount
                             where discount != null
                             let discountRate = (discount != null && discount.IsPercent) ? (Convert.ToDecimal(p.PackageFee) * discount.DiscountRate) / 100 : discount.DiscountRate
                             select new ServicePackInfoDetail
                             {
                                 PackageId = p.PackageId,
                                 PackageCode = p.PackageCode,
                                 PackageName = p.PackageName,
                                 CategoryId = p.CategoryId,
                                 TypeId = p.TypeId,
                                 AvailableQuantity = p.AvailableQuantity,
                                 Capacity = p.Capacity,
                                 DurationId = p.DurationId,
                                 DiscountId = p.DiscountId,
                                 TaxRateId = p.TaxRateId,
                                 Weight = p.Weight,
                                 PackageFee = p.PackageFee,
                                 TotalFee = p.TotalFee,
                                 TaxRate = taxRate,
                                 DiscountRate = discountRate,
                                 CurrencyCode = p.CurrencyCode,
                                 FileId = p.FileId,
                                 Description = !string.IsNullOrEmpty(p.Description) ? HttpUtility.HtmlDecode(p.Description) : string.Empty,
                                 Specification = !string.IsNullOrEmpty(p.Specification) ? HttpUtility.HtmlDecode(p.Specification) : string.Empty,
                                 FileUrl = (p.FileId != null && p.FileId > 0) ? DocumentService.GetFileInfoDetail(Convert.ToInt32(p.FileId)).FileUrl : GlobalSettings.NotFoundFileUrl,
                                 Rating = p.Rating,
                                 TotalViews = p.TotalViews,
                                 ListOrder = p.ListOrder,
                                 Status = p.Status,
                                 CreatedDate = p.CreatedDate,
                                 LastModifiedDate = p.LastModifiedDate,
                                 Category = p.Category.ToDto<ServiceCategory, ServiceCategoryDetail>(),
                                 Type = p.Type.ToDto<ServicePackType, ServicePackTypeDetail>(),
                                 Period = p.Period.ToDto<ServicePeriod, ServicePeriodDetail>(),
                                 Discount = p.Discount.ToDto<ServiceDiscount, ServiceDiscountDetail>(),
                                 Tax = p.Tax.ToDto<ServiceTaxRate, ServiceTaxRateDetail>(),
                                 Duration = p.Duration.ToDto<ServicePackDuration, ServicePackDurationDetail>(),
                                 Document = DocumentService.GetFileInfoDetail(p.FileId ?? 0),
                                 Options = options.ToDtos<ServicePackOption, ServicePackOptionDetail>(),
                                 Employees = GetServicePackProviders(p.PackageId),
                                 Ratings = GetServicePackRatings(p.PackageId)
                             });
            }
            return lst;
        }
        public IEnumerable<ServicePackInfoDetail> GetServicePacks(int categoryId, ServicePackStatus? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = new List<ServicePackInfoDetail>();
            var packages = UnitOfWork.ServicePackRepository.GetServicePacks(categoryId, status, ref recordCount, orderBy, page, pageSize).ToList();
            if (packages.Any())
            {
                lst.AddRange(from p in packages
                             let serviceTaxRate = p.Tax
                             where serviceTaxRate != null
                             let taxRate = (serviceTaxRate != null && serviceTaxRate.IsPercent) ? (Convert.ToDecimal(p.PackageFee) * serviceTaxRate.TaxRate) / 100 : serviceTaxRate.TaxRate
                             let options = UnitOfWork.ServicePackOptionRepository.GetServicePackOptions(p.PackageId)
                             let discount = p.Discount
                             where discount != null
                             let discountRate = (discount != null && discount.IsPercent) ? (Convert.ToDecimal(p.PackageFee) * discount.DiscountRate) / 100 : discount.DiscountRate
                             select new ServicePackInfoDetail
                             {
                                 PackageId = p.PackageId,
                                 PackageCode = p.PackageCode,
                                 PackageName = p.PackageName,
                                 CategoryId = p.CategoryId,
                                 TypeId = p.TypeId,
                                 AvailableQuantity = p.AvailableQuantity,
                                 Capacity = p.Capacity,
                                 DurationId = p.DurationId,
                                 DiscountId = p.DiscountId,
                                 TaxRateId = p.TaxRateId,
                                 Weight = p.Weight,
                                 PackageFee = p.PackageFee,
                                 TotalFee = p.TotalFee,
                                 CurrencyCode = p.CurrencyCode,
                                 FileId = p.FileId,
                                 Description = !string.IsNullOrEmpty(p.Description) ? HttpUtility.HtmlDecode(p.Description) : string.Empty,
                                 Specification = !string.IsNullOrEmpty(p.Specification) ? HttpUtility.HtmlDecode(p.Specification) : string.Empty,
                                 FileUrl = (p.FileId != null && p.FileId > 0) ? DocumentService.GetFileInfoDetail(Convert.ToInt32(p.FileId)).FileUrl : GlobalSettings.NotFoundFileUrl,
                                 Rating = p.Rating,
                                 TotalViews = p.TotalViews,
                                 ListOrder = p.ListOrder,
                                 Status = p.Status,
                                 CreatedDate = p.CreatedDate,
                                 LastModifiedDate = p.LastModifiedDate,
                                 Category = p.Category.ToDto<ServiceCategory, ServiceCategoryDetail>(),
                                 Type = p.Type.ToDto<ServicePackType, ServicePackTypeDetail>(),
                                 Period = p.Period.ToDto<ServicePeriod, ServicePeriodDetail>(),
                                 Discount = p.Discount.ToDto<ServiceDiscount, ServiceDiscountDetail>(),
                                 Tax = p.Tax.ToDto<ServiceTaxRate, ServiceTaxRateDetail>(),
                                 Duration = p.Duration.ToDto<ServicePackDuration, ServicePackDurationDetail>(),
                                 Document = DocumentService.GetFileInfoDetail(p.FileId ?? 0),
                                 Options = options.ToDtos<ServicePackOption, ServicePackOptionDetail>(),
                                 Employees = GetServicePackProviders(p.PackageId),
                                 Ratings = GetServicePackRatings(p.PackageId)
                             });
            }
            return lst;
        }
        public IEnumerable<ServicePackInfoDetail> GetDiscountedServicePackages(int? categoryId, ServicePackStatus? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = new List<ServicePackInfoDetail>();
            var packages = UnitOfWork.ServicePackRepository.GetDiscountedServicePacks(categoryId, status, ref recordCount, orderBy, page, pageSize);
            if (packages != null)
            {
                lst.AddRange(from p in packages
                             let serviceTaxRate = p.Tax
                             where serviceTaxRate != null
                             let taxRate = (serviceTaxRate != null && serviceTaxRate.IsPercent) ? (Convert.ToDecimal(p.PackageFee) * serviceTaxRate.TaxRate) / 100 : serviceTaxRate.TaxRate
                             let options = UnitOfWork.ServicePackOptionRepository.GetServicePackOptions(p.PackageId)
                             let discount = p.Discount
                             where discount != null
                             let discountRate = (discount != null && discount.IsPercent) ? (Convert.ToDecimal(p.PackageFee) * discount.DiscountRate) / 100 : discount.DiscountRate
                             select new ServicePackInfoDetail
                             {
                                 PackageId = p.PackageId,
                                 PackageCode = p.PackageCode,
                                 PackageName = p.PackageName,
                                 CategoryId = p.CategoryId,
                                 TypeId = p.TypeId,
                                 AvailableQuantity = p.AvailableQuantity,
                                 Capacity = p.Capacity,
                                 DurationId = p.DurationId,
                                 DiscountId = p.DiscountId,
                                 TaxRateId = p.TaxRateId,
                                 Weight = p.Weight,
                                 PackageFee = p.PackageFee,
                                 TotalFee = p.TotalFee,
                                 TaxRate = taxRate,
                                 DiscountRate = discountRate,
                                 CurrencyCode = p.CurrencyCode,
                                 FileId = p.FileId,
                                 Description = !string.IsNullOrEmpty(p.Description) ? HttpUtility.HtmlDecode(p.Description) : string.Empty,
                                 Specification = !string.IsNullOrEmpty(p.Specification) ? HttpUtility.HtmlDecode(p.Specification) : string.Empty,
                                 FileUrl = (p.FileId != null && p.FileId > 0) ? DocumentService.GetFileInfoDetail(Convert.ToInt32(p.FileId)).FileUrl : GlobalSettings.NotFoundFileUrl,
                                 Rating = p.Rating,
                                 TotalViews = p.TotalViews,
                                 ListOrder = p.ListOrder,
                                 Status = p.Status,
                                 CreatedDate = p.CreatedDate,
                                 LastModifiedDate = p.LastModifiedDate,
                                 Category = p.Category.ToDto<ServiceCategory, ServiceCategoryDetail>(),
                                 Type = p.Type.ToDto<ServicePackType, ServicePackTypeDetail>(),
                                 Period = p.Period.ToDto<ServicePeriod, ServicePeriodDetail>(),
                                 Discount = p.Discount.ToDto<ServiceDiscount, ServiceDiscountDetail>(),
                                 Tax = p.Tax.ToDto<ServiceTaxRate, ServiceTaxRateDetail>(),
                                 Duration = p.Duration.ToDto<ServicePackDuration, ServicePackDurationDetail>(),
                                 Document = DocumentService.GetFileInfoDetail(p.FileId ?? 0),
                                 Options = options.ToDtos<ServicePackOption, ServicePackOptionDetail>(),
                                 Employees = GetServicePackProviders(p.PackageId),
                                 Ratings = GetServicePackRatings(p.PackageId)
                             });
            }
            return lst;
        }
        public ServicePackInfoDetail GetServicePackDetail(int id)
        {
            var entity = UnitOfWork.ServicePackRepository.GetDetail(id);
            if (entity == null) return new ServicePackInfoDetail();

            var taxRate = entity.Tax != null && entity.Tax.IsPercent
                ? Convert.ToDecimal(entity.PackageFee) * entity.Tax.TaxRate / 100
                : entity.Tax.TaxRate;

            var discountRate = entity.Discount != null && entity.Discount.IsPercent
                ? Convert.ToDecimal(entity.PackageFee) * entity.Discount.DiscountRate / 100
                : entity.Discount.DiscountRate;

            var item = new ServicePackInfoDetail
            {
                PackageId = entity.PackageId,
                PackageCode = entity.PackageCode,
                PackageName = entity.PackageName,
                CategoryId = entity.CategoryId,
                TypeId = entity.TypeId,
                AvailableQuantity = entity.AvailableQuantity,
                Capacity = entity.Capacity,
                DurationId = entity.DurationId,
                DiscountId = entity.DiscountId,
                TaxRateId = entity.TaxRateId,
                Weight = entity.Weight,
                PackageFee = entity.PackageFee,
                TotalFee = entity.TotalFee,
                TaxRate = taxRate,
                DiscountRate = discountRate,
                CurrencyCode = entity.CurrencyCode,
                FileId = entity.FileId,
                Description = !string.IsNullOrEmpty(entity.Description) ? HttpUtility.HtmlDecode(entity.Description) : string.Empty,
                Specification = !string.IsNullOrEmpty(entity.Specification) ? HttpUtility.HtmlDecode(entity.Specification) : string.Empty,
                FileUrl = (entity.FileId != null && entity.FileId > 0) ? DocumentService.GetFileInfoDetail(Convert.ToInt32(entity.FileId)).FileUrl : GlobalSettings.NotFoundFileUrl,
                Rating = entity.Rating,
                TotalViews = entity.TotalViews,
                ListOrder = entity.ListOrder,
                Status = entity.Status,
                CreatedDate = entity.CreatedDate,
                LastModifiedDate = entity.LastModifiedDate,
                Category = entity.Category.ToDto<ServiceCategory, ServiceCategoryDetail>(),
                Type = entity.Type.ToDto<ServicePackType, ServicePackTypeDetail>(),
                Period = entity.Period.ToDto<ServicePeriod, ServicePeriodDetail>(),
                Discount = entity.Discount.ToDto<ServiceDiscount, ServiceDiscountDetail>(),
                Tax = entity.Tax.ToDto<ServiceTaxRate, ServiceTaxRateDetail>(),
                Duration = entity.Duration.ToDto<ServicePackDuration, ServicePackDurationDetail>(),
                Options = GeServicePackOptions(entity.PackageId),
                Employees = GetServicePackProviders(entity.PackageId),
                Ratings = GetServicePackRatings(entity.PackageId)
            };

            if (entity.FileId != null && entity.FileId > 0)
            {
                item.Document = DocumentService.GetFileInfoDetail(Convert.ToInt32(entity.FileId));
            }
            return item;
        }
        public SelectList PopulateServicePackStatus(bool? selectedValue = true, bool isShowSelectText = false)
        {
            return UnitOfWork.ServicePackRepository.PopulateServicePackStatus(selectedValue, isShowSelectText);
        }
        public SelectList PopulateServicePackSelectList(ServicePackStatus? status = null, int? selectedValue = null,
            bool? isShowSelectText = true)
        {
            return UnitOfWork.ServicePackRepository.PopulateServicePackSelectList(status, selectedValue, isShowSelectText);
        }
        public SelectList PopulateServicePackSelectListNotCode(ServicePackStatus? status = null, int? selectedValue = null, bool? isShowSelectText = true)
        {
            return UnitOfWork.ServicePackRepository.PopulateServicePackSelectListNotCode(status, selectedValue, isShowSelectText);
        }
        public SelectList PopulateServicePackSelectListByCode(string discountCode, ServicePackStatus? status = null, int? selectedValue = null,
            bool? isShowSelectText = true)
        {
            return UnitOfWork.ServicePackRepository.PopulateServicePackSelectListByCode(discountCode, status, selectedValue, isShowSelectText);
        }
        public SelectList PopulateServicePackSelectListByCateId(int cateId, ServicePackStatus? status = null, int? selectedValue = null,
            bool? isShowSelectText = true)
        {
            return UnitOfWork.ServicePackRepository.PopulateServicePackSelectListByCateId(cateId, status, selectedValue, isShowSelectText);
        }

        public ServicePackDetail InsertServicePack(Guid applicationId, Guid userId, ServicePackEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullServicePackEntry, "ServicePackEntry", null, ErrorMessage.Messages[ErrorCode.NullServicePackEntry]));
                throw new ValidationError(violations);
            }

            var isValidServicePackType = Enum.IsDefined(typeof(ServiceType), entry.TypeId);
            if (!isValidServicePackType)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidServicePackType, "ServicePackType", entry.TypeId, ErrorMessage.Messages[ErrorCode.InvalidServicePackType]));
                throw new ValidationError(violations);
            }

            if (string.IsNullOrEmpty(entry.PackageName))
            {
                violations.Add(new RuleViolation(ErrorCode.NullServicePackName, "ServicePackName", null, ErrorMessage.Messages[ErrorCode.NullServicePackName]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.PackageName.Length > 300)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidServicePackName, "ServicePackName", null, ErrorMessage.Messages[ErrorCode.InvalidServicePackName]));
                    throw new ValidationError(violations);
                }
                else
                {
                    bool isDuplicate = UnitOfWork.ServicePackRepository.HasDataExisted(entry.PackageName);
                    if (isDuplicate)
                    {
                        violations.Add(new RuleViolation(ErrorCode.DuplicateServicePackName, "ServicePackName",
                                entry.PackageName));
                        throw new ValidationError(violations);
                    }
                }
            }

            string currencyCode;
            if (!string.IsNullOrEmpty(entry.CurrencyCode))
            {
                var setting = ApplicationService.GetCurrencySetting(applicationId,
                    SettingKeys.CurrencySettingName);
                currencyCode = setting != null ? setting.Setting.KeyValue : GlobalSettings.DefaultCurrencyCode;
            }
            else
            {
                currencyCode = entry.CurrencyCode;
            }

            var entity = entry.ToEntity<ServicePackEntry, ServicePack>();
            entity.CurrencyCode = currencyCode;
            entity.PackageCode = entry.PackageCode ?? Guid.NewGuid().ToString();
            entity.ListOrder = UnitOfWork.ServicePackRepository.GetNewListOrder();
            entity.CreatedDate = DateTime.UtcNow;

            if (entry.File != null && entry.File.ContentLength > 0)
            {
                int maxContentLength = UnitOfWork.ApplicationSettingRepository.GetAllowedMaxImageContentLength(applicationId);
                string[] allowedFileExtensions = UnitOfWork.ApplicationSettingRepository.GetAllowedImageExtensions(applicationId);

                if (!allowedFileExtensions.Contains(entry.File.FileName.Substring(entry.File.FileName.LastIndexOf('.'))))
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidFileType, "FileUpload", LanguageResource.InvalidFileExtension + " : " + string.Join(", ", allowedFileExtensions)));
                    throw new ValidationError(violations);
                }
                else if (entry.File.ContentLength > maxContentLength)
                {
                    violations.Add(new RuleViolation(ErrorCode.MaximumAllowedSize, "FileUpload", LanguageResource.InvalidFileSize + " , " + LanguageResource.MaximumAllowedSize + maxContentLength + " MB"));
                    throw new ValidationError(violations);
                }
                else
                {
                    var fileInfo = DocumentService.UploadAndSaveDbByFolderId(applicationId, userId, entry.File, (int)FileLocation.ServicePack, StorageType.Local);
                    entity.FileId = fileInfo.FileId;
                }
            }

            UnitOfWork.ServicePackRepository.Insert(entity);
            UnitOfWork.SaveChanges();

            //Insert SelectedProviders
            if (entry.SelectedProviders == null || !entry.SelectedProviders.Any())
            {
                violations.Add(new RuleViolation(ErrorCode.PleaseSelectEmployee, "SelectedProviders",
                                   entry.SelectedProviders, ErrorMessage.Messages[ErrorCode.PleaseSelectEmployee]));
                throw new ValidationError(violations);
            }
            else
            {
                foreach (var employeeId in entry.SelectedProviders)
                {
                    var providerEntry = new ServicePackProviderEntry
                    {
                        PackageId = entity.PackageId,
                        EmployeeId = employeeId
                    };
                    InsertServicePackProvider(providerEntry);
                }
            }

            // insert options
            if (entry.Options != null && entry.Options.Any())
            {
                InsertServicePackOptions(entity.PackageId, entry.Options);
            }

            return entity.ToDto<ServicePack, ServicePackDetail>();
        }

        public void UpdateServicePack(Guid applicationId, Guid userId, ServicePackEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullServicePackEditEntry, "ServicePackEditEntry", null, ErrorMessage.Messages[ErrorCode.NullServicePackEditEntry]));
                throw new ValidationError(violations);
            }

            var isValidServicePackType = Enum.IsDefined(typeof(ServiceType), entry.TypeId);
            if (!isValidServicePackType)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidServicePackType, "ServicePackType", entry.TypeId, ErrorMessage.Messages[ErrorCode.InvalidServicePackType]));
                throw new ValidationError(violations);
            }

            var entity = UnitOfWork.ServicePackRepository.FindById(entry.PackageId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundServicePack, "ServicePack", entry.PackageId, ErrorMessage.Messages[ErrorCode.NotFoundServicePack]));
                throw new ValidationError(violations);
            }

            if (string.IsNullOrEmpty(entry.PackageName))
            {
                violations.Add(new RuleViolation(ErrorCode.NullServicePackName, "ServicePackName", null, ErrorMessage.Messages[ErrorCode.NullServicePackName]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.PackageName.Length > 300)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidServicePackName, "ServicePackName", null, ErrorMessage.Messages[ErrorCode.InvalidServicePackName]));
                    throw new ValidationError(violations);
                }
                else
                {
                    if (entity.PackageName != entry.PackageName)
                    {
                        bool isDuplicate = UnitOfWork.ServicePackRepository.HasDataExisted(entry.PackageName);
                        if (isDuplicate)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicateServicePackName, "ServicePackName",
                                    entry.PackageName, ErrorMessage.Messages[ErrorCode.DuplicateServicePackName]));
                            throw new ValidationError(violations);
                        }
                    }
                }
            }

            if (entry.SelectedProviders == null)
            {
                violations.Add(new RuleViolation(ErrorCode.PleaseSelectEmployee, "SelectedProviders",
                                   entry.SelectedProviders, ErrorMessage.Messages[ErrorCode.PleaseSelectEmployee]));
                throw new ValidationError(violations);
            }

            if (entry.File != null && entry.File.ContentLength > 0)
            {
                if (entity.FileId != null)
                {
                    DocumentService.DeleteFile(Convert.ToInt32(entity.FileId));
                }

                var fileInfo = DocumentService.UploadAndSaveDbByFolderId(applicationId, userId, entry.File, (int)FileLocation.ServicePack, StorageType.Local);
                entity.FileId = fileInfo.FileId;
            }

            // update options
            if (entry.Options != null && entry.Options.Any())
            {
                InsertServicePackOptions(entry.PackageId, entry.Options);
            }

            if (entry.ExistedOptions != null && entry.ExistedOptions.Any())
            {
                UpdateServicePackOption(entry.PackageId, entry.ExistedOptions);
            }

            // update other properties
            string specification = !string.IsNullOrEmpty(entry.Specification)
                ? HttpUtility.HtmlDecode(entry.Specification)
                : string.Empty;

            string currencyCode;
            if (!string.IsNullOrEmpty(entry.CurrencyCode))
            {
                var setting = ApplicationService.GetCurrencySetting(applicationId,
                    SettingKeys.CurrencySettingName);
                currencyCode = setting != null ? setting.Setting.KeyValue : GlobalSettings.DefaultCurrencyCode;
            }
            else
            {
                currencyCode = entry.CurrencyCode;
            }

            entity.CurrencyCode = currencyCode;
            entity.PackageName = entry.PackageName;
            entity.CategoryId = entry.CategoryId;
            entity.TypeId = Convert.ToInt32(entry.TypeId);
            entity.AvailableQuantity = entry.AvailableQuantity;
            entity.Capacity = entry.Capacity;
            entity.DurationId = entry.DurationId;
            entity.DiscountId = entry.DiscountId;
            entity.TaxRateId = entry.TaxRateId;
            entity.Weight = entry.Weight;
            entity.PackageFee = entry.PackageFee;
            entity.TotalFee = entry.TotalFee;
            entity.Description = entry.Description;
            entity.Specification = specification;
            entity.Status = entry.Status;
            entity.LastModifiedDate = DateTime.UtcNow;
            UnitOfWork.ServicePackRepository.Update(entity);
            UnitOfWork.SaveChanges();

            //Update Providers for service pack
            if (entry.SelectedProviders != null && entry.SelectedProviders.Any())
            {
                var servicePackProviderEntries =
                    entry.SelectedProviders.Select(employeeId => new ServicePackProviderEntry
                    {
                        PackageId = entity.PackageId,
                        EmployeeId = employeeId,
                    }).ToList();

                UpdateServicePackProviders(entity.PackageId, servicePackProviderEntries);
            }
            else
            {
                DeleteServicePackProviders(entity.PackageId);
            }
        }

        public void UpdateServicePackProviders(int packageId, List<ServicePackProviderEntry> employeesOfServicePacks)
        {
            var existedProviders = UnitOfWork.ServicePackProviderRepository.GetServicePackProviders(Convert.ToInt32(packageId)).ToList();
            if (existedProviders.Any())
            {
                var previousProviders = existedProviders.Select(x => x.EmployeeId).ToList();
                var latestProviders = employeesOfServicePacks.Select(x => x.EmployeeId).ToList();

                //Get the elements in latest list a but not in previous list - Except
                var exceptLatestList = latestProviders.Except(previousProviders).ToList();
                if (exceptLatestList.Count > 0)
                {
                    foreach (var employeeId in exceptLatestList)
                    {
                        var entry = employeesOfServicePacks.FirstOrDefault(x => x.EmployeeId == employeeId);
                        InsertServicePackProvider(entry);
                    }
                }

                //Get the elements in previous list b but not in latest list a - Except
                var exceptPreviousList = previousProviders.Except(latestProviders).ToList();
                if (exceptPreviousList.Count > 0)
                {
                    foreach (var employeeId in exceptPreviousList)
                    {
                        var entity = UnitOfWork.ServicePackProviderRepository.GetDetails(packageId, employeeId);
                        if (entity == null) return;

                        UnitOfWork.ServicePackProviderRepository.Delete(entity);
                        UnitOfWork.SaveChanges();
                    }
                }
            }
            else
            {
                if (employeesOfServicePacks != null && employeesOfServicePacks.Any())
                {
                    foreach (var employee in employeesOfServicePacks)
                    {
                        var providerEntry = new ServicePackProviderEntry
                        {
                            PackageId = packageId,
                            EmployeeId = employee.EmployeeId
                        };
                        InsertServicePackProvider(providerEntry);
                    }
                }
            }
        }

        private void DeleteServicePackProviders(int packageId)
        {
            var existedProviders =
                UnitOfWork.ServicePackProviderRepository.GetServicePackProviders(Convert.ToInt32(packageId))
                    .ToList();
            if (existedProviders.Any())
            {
                foreach (var provider in existedProviders)
                {
                    UnitOfWork.ServicePackProviderRepository.Delete(provider);
                    UnitOfWork.SaveChanges();
                }
            }
        }

        public void DeleteServicePack(int packageId)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ServicePackRepository.Find(packageId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundServicePack, "ServicePackId", packageId));
                throw new ValidationError(violations);
            }

            if (UnitOfWork.OrderProductTempRepository.HasOrderProductTempExisted(packageId, ItemType.ServicePackage)
                || UnitOfWork.OrderProductRepository.HasOrderProductExisted(packageId, ItemType.ServicePackage))
            {
                violations.Add(new RuleViolation(ErrorCode.ExistedOder, "Service Package", packageId, ErrorMessage.Messages[ErrorCode.ExistedOder]));
                throw new ValidationError(violations);
            }

            if (entity.FileId != null && entity.FileId > 0)
            {
                DocumentService.DeleteFile(Convert.ToInt32(entity.FileId));
            }
            UnitOfWork.ServicePackRepository.Delete(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateServicePackStatus(int id, ServicePackStatus status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ServicePackRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundServicePack, "ServicePack", id, ErrorMessage.Messages[ErrorCode.NotFoundServicePack]));
                throw new ValidationError(violations);
            }

            var isValid = Enum.IsDefined(typeof(ServicePackStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status", status, ErrorMessage.Messages[ErrorCode.InvalidStatus]));
                throw new ValidationError(violations);
            }
            if (entity.Status == status) return;

            entity.Status = status;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.ServicePackRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateServicePackListOrder(int id, int listOrder)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ServicePackRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundServicePack, "ServicePack", id, ErrorMessage.Messages[ErrorCode.NotFoundServicePack]));
                throw new ValidationError(violations);
            }

            entity.ListOrder = listOrder;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.ServicePackRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateServicePackTotalViews(int id)
        {
            var entity = UnitOfWork.ServicePackRepository.FindById(id);
            if (entity == null) return;

            if (entity.TotalViews == null) entity.TotalViews = 0;

            entity.TotalViews = entity.TotalViews + 1;

            UnitOfWork.ServicePackRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region ServicePackType
        public IEnumerable<ServicePackTypeDetail> GetServicePackTypes(ServicePackTypeStatus? status)
        {
            var lst = UnitOfWork.ServicePackTypeRepository.GetServicePackTypes(status);
            return lst.ToDtos<ServicePackType, ServicePackTypeDetail>();
        }
        public SelectList PopulateServicePackTypeSelectList(ServicePackTypeStatus? status, int? selectedValue = null, bool isShowSelectText = false)
        {
            return UnitOfWork.ServicePackTypeRepository.PopulateServicePackTypeSelectList(status, selectedValue, isShowSelectText);
        }
        public ServicePackTypeDetail InsertServicePackType(ServicePackTypeEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullServicePackTypeEntry, "ServicePackTypeEntry", null, ErrorMessage.Messages[ErrorCode.NullServicePackTypeEntry]));
                throw new ValidationError(violations);
            }

            if (string.IsNullOrEmpty(entry.TypeName))
            {
                violations.Add(new RuleViolation(ErrorCode.NullTypeName, "TypeName", null, ErrorMessage.Messages[ErrorCode.NullTypeName]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.TypeName.Length > 300)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidTypeName, "TypeName", null, ErrorMessage.Messages[ErrorCode.InvalidTypeName]));
                    throw new ValidationError(violations);
                }
                else
                {
                    bool hasDuplicateDurationName = UnitOfWork.ServicePackTypeRepository.HasDataExisted(entry.TypeName);
                    if (hasDuplicateDurationName)
                    {
                        violations.Add(new RuleViolation(ErrorCode.DuplicateTypeName, "TypeName",
                            entry.TypeName, ErrorMessage.Messages[ErrorCode.DuplicateTypeName]));
                        throw new ValidationError(violations);
                    }
                }
            }

            var entity = entry.ToEntity<ServicePackTypeEntry, ServicePackType>();

            UnitOfWork.ServicePackTypeRepository.Insert(entity);
            UnitOfWork.SaveChanges();
            return entity.ToDto<ServicePackType, ServicePackTypeDetail>();
        }

        public void UpdateServicePackType(ServicePackTypeEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullServicePackTypeEditEntry, "ServicePackTypeEditEntry", null, ErrorMessage.Messages[ErrorCode.NullServicePackTypeEditEntry]));
                throw new ValidationError(violations);
            }

            var entity = UnitOfWork.ServicePackTypeRepository.FindById(entry.TypeId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundServicePackType, "ServicePackType", entry.TypeId, ErrorMessage.Messages[ErrorCode.NotFoundServicePackType]));
                throw new ValidationError(violations);
            }

            if (string.IsNullOrEmpty(entry.TypeName))
            {
                violations.Add(new RuleViolation(ErrorCode.NullTypeName, "TypeName", null, ErrorMessage.Messages[ErrorCode.NullTypeName]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.TypeName.Length > 300)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidTypeName, "TypeName", null, ErrorMessage.Messages[ErrorCode.InvalidTypeName]));
                    throw new ValidationError(violations);
                }
                else
                {
                    if (entity.TypeName != entry.TypeName)
                    {
                        bool isDuplicate = UnitOfWork.ServicePackTypeRepository.HasDataExisted(entry.TypeName);
                        if (isDuplicate)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicateTypeName, "TypeName",
                                entry.TypeName, ErrorMessage.Messages[ErrorCode.DuplicateTypeName]));
                            throw new ValidationError(violations);
                        }
                    }
                }
            }

            entity.TypeName = entry.TypeName;
            entity.IsOnline = entry.IsOnline;
            entity.IsActive = entry.IsActive;

            UnitOfWork.ServicePackTypeRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateServicePackTypeStatus(int id, ServicePackTypeStatus status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ServicePackTypeRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundServicePackType, "ServicePackType", id, ErrorMessage.Messages[ErrorCode.NotFoundServicePackType]));
                throw new ValidationError(violations);
            }
            if (entity.IsActive == status) return;

            entity.IsActive = status;

            UnitOfWork.ServicePackTypeRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        #endregion

        #region Service Pack Option
        public IEnumerable<ServicePackOptionDetail> GeServicePackOptions(int packageId)
        {
            var lst = UnitOfWork.ServicePackOptionRepository.GetServicePackOptions(packageId);
            return lst.ToDtos<ServicePackOption, ServicePackOptionDetail>();
        }
        public ServicePackOptionDetail GetServicePackOptionDetails(int optionId)
        {
            var entity = UnitOfWork.ServicePackOptionRepository.FindById(optionId);
            return entity.ToDto<ServicePackOption, ServicePackOptionDetail>();
        }
        public void InsertServicePackOption(int packageId, ServicePackOptionEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (packageId <= 0)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidPackageId, "PackageId", packageId, ErrorMessage.Messages[ErrorCode.InvalidPackageId]));
                throw new ValidationError(violations);
            }
            else
            {
                var item = UnitOfWork.ServicePackRepository.FindById(packageId);
                if (item == null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidPackageId, "PackageId", packageId, ErrorMessage.Messages[ErrorCode.InvalidPackageId]));
                    throw new ValidationError(violations);
                }
            }

            if (string.IsNullOrEmpty(entry.OptionName))
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidOptionName, "OptionName", entry.OptionName, ErrorMessage.Messages[ErrorCode.InvalidOptionName]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.OptionName.Length > 200)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidOptionName, "OptionName", entry.OptionName, ErrorMessage.Messages[ErrorCode.InvalidOptionName]));
                    throw new ValidationError(violations);
                }
                else
                {
                    bool isDuplicate = UnitOfWork.ServicePackOptionRepository.HasDataExisted(packageId, entry.OptionName);
                    if (isDuplicate)
                    {
                        violations.Add(new RuleViolation(ErrorCode.DuplicateOptionName, "OptionName", entry.OptionName, ErrorMessage.Messages[ErrorCode.DuplicateOptionName]));
                        throw new ValidationError(violations);
                    }
                }
            }

            if (entry.OptionValue != null && entry.OptionValue <= 0)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidOptionValue, "OptionValue", entry.OptionValue, ErrorMessage.Messages[ErrorCode.InvalidOptionValue]));
                throw new ValidationError(violations);
            }

            var entity = entry.ToEntity<ServicePackOptionEntry, ServicePackOption>();
            entity.PackageId = packageId;
            entity.ListOrder = UnitOfWork.ServicePackOptionRepository.GetNewListOrder();

            UnitOfWork.ServicePackOptionRepository.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void InsertServicePackOptions(int packageId, List<ServicePackOptionEntry> entries)
        {
            if (entries != null && entries.Any())
            {
                foreach (var entry in entries)
                {
                    if (packageId > 0 && !string.IsNullOrEmpty(entry.OptionName) && entry.OptionValue != null && entry.OptionValue > 0)
                    {
                        InsertServicePackOption(packageId, entry);
                    }
                }
            }
        }

        public void UpdateServicePackOption(ServicePackOptionEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ServicePackOptionRepository.FindById(entry.OptionId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundPackageOption, "ServicePackOption", entry.OptionId, ErrorMessage.Messages[ErrorCode.NotFoundPackageOption]));
                throw new ValidationError(violations);
            }

            if (entry.PackageId <= 0)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidPackageId, "PackageId", entry.PackageId, ErrorMessage.Messages[ErrorCode.InvalidPackageId]));
                throw new ValidationError(violations);
            }
            else
            {
                var item = UnitOfWork.ServicePackRepository.FindById(entry.PackageId);
                if (item == null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidPackageId, "PackageId", entry.PackageId, ErrorMessage.Messages[ErrorCode.InvalidPackageId]));
                    throw new ValidationError(violations);
                }
            }

            if (string.IsNullOrEmpty(entry.OptionName))
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidOptionName, "OptionName", null, ErrorMessage.Messages[ErrorCode.InvalidOptionName]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.OptionName.Length > 200)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidOptionName, "OptionName", entry.OptionName.Length, ErrorMessage.Messages[ErrorCode.InvalidOptionName]));
                    throw new ValidationError(violations);
                }
                else
                {
                    if (entity.OptionName != entry.OptionName)
                    {
                        bool isDuplicate = UnitOfWork.ServicePackOptionRepository.HasDataExisted(Convert.ToInt32(entry.PackageId), entry.OptionName);
                        if (isDuplicate)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicateOptionName, "OptionName", entry.OptionName, ErrorMessage.Messages[ErrorCode.DuplicateOptionName]));
                            throw new ValidationError(violations);
                        }
                    }
                }
            }

            if (entry.OptionValue != null && entry.OptionValue < 0)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidOptionValue, "OptionValue", entry.OptionValue, ErrorMessage.Messages[ErrorCode.InvalidOptionValue]));
                throw new ValidationError(violations);
            }

            entity.PackageId = Convert.ToInt32(entry.PackageId);
            entity.OptionName = entry.OptionName;
            entity.OptionValue = entry.OptionValue;
            entity.IsActive = entry.IsActive;

            UnitOfWork.ServicePackOptionRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateServicePackOption(int packageId, List<ServicePackOptionEditEntry> entries)
        {
            if (entries != null && entries.Any())
            {
                foreach (var entry in entries)
                {
                    if (entry.PackageId != null && entry.OptionId != null && entry.PackageId > 0 && entry.OptionId > 0)
                    {
                        UpdateServicePackOption(entry);
                    }
                    else
                    {
                        entry.PackageId = packageId;
                        InsertServicePackOption(packageId, entry);
                    }
                }
            }
        }

        public void UpdateServicePackOptionStatus(int id, ServicePackOptionStatus status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ServicePackOptionRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullPackageOption, "OptionId", id, ErrorMessage.Messages[ErrorCode.NullPackageOption]));
                throw new ValidationError(violations);
            }

            if (entity.IsActive == status) return;

            entity.IsActive = status;
            UnitOfWork.ServicePackOptionRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region Service Pack Duration 
        public IEnumerable<ServicePackDurationDetail> GetServicePackDurations(ServicePackDurationSearchEntry filter, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = UnitOfWork.ServicePackDurationRepository.GetServicePackDurations(filter.DurationName, filter.Status, ref recordCount, orderBy, page, pageSize);
            return lst.ToDtos<ServicePackDuration, ServicePackDurationDetail>();
        }

        public ServicePackDurationDetail GetServicePackDurationDetail(int id)
        {
            var entity = UnitOfWork.ServicePackDurationRepository.FindById(id);
            return entity.ToDto<ServicePackDuration, ServicePackDurationDetail>();
        }

        public ServicePackDurationDetail GetDurationByServicePackId(int packageId)
        {
            var entity = UnitOfWork.ServicePackDurationRepository.GetDurationByServicePackId(packageId);
            return entity.ToDto<ServicePackDuration, ServicePackDurationDetail>();
        }

        public SelectList PopulateServicePackDurationStatus(bool? selectedValue = true, bool isShowSelectText = false)
        {
            return UnitOfWork.ServicePackDurationRepository.PopulateServicePackDurationStatus(selectedValue, isShowSelectText);
        }

        public SelectList PopulateServicePackDurationSelectList(bool? status = null, int? selectedValue = null,
          bool? isShowSelectText = true)
        {
            return UnitOfWork.ServicePackDurationRepository.PopulateServicePackDurationSelectList(status, selectedValue, isShowSelectText);
        }


        public ServicePackDurationDetail InsertServicePackDuration(ServicePackDurationEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullServicePackDurationEntry, "ServicePackDurationEntry", null, ErrorMessage.Messages[ErrorCode.NullServicePackDurationEntry]));
                throw new ValidationError(violations);
            }

            if (string.IsNullOrEmpty(entry.DurationName))
            {
                violations.Add(new RuleViolation(ErrorCode.NullDurationName, "DurationName", null, ErrorMessage.Messages[ErrorCode.NullDurationName]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.DurationName.Length > 300)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidDurationName, "DurationName", null, ErrorMessage.Messages[ErrorCode.InvalidDurationName]));
                    throw new ValidationError(violations);
                }
                else
                {
                    bool hasDuplicateDurationName = UnitOfWork.ServicePackDurationRepository.HasDurationNameExisted(entry.DurationName);
                    if (hasDuplicateDurationName)
                    {
                        violations.Add(new RuleViolation(ErrorCode.DuplicateDurationName, "DurationName",
                                entry.DurationName, ErrorMessage.Messages[ErrorCode.DuplicateDurationName]));
                        throw new ValidationError(violations);
                    }
                }
            }

            bool hasAllotedTimeExisted = UnitOfWork.ServicePackDurationRepository.HasAllotedTimeExisted(entry.AllotedTime);
            if (hasAllotedTimeExisted)
            {
                violations.Add(new RuleViolation(ErrorCode.DuplicateAllotedTime, "AllotedTime",
                        entry.AllotedTime, ErrorMessage.Messages[ErrorCode.DuplicateAllotedTime]));
                throw new ValidationError(violations);
            }

            var entity = entry.ToEntity<ServicePackDurationEntry, ServicePackDuration>();
            entity.CreatedDate = DateTime.UtcNow;

            UnitOfWork.ServicePackDurationRepository.Insert(entity);
            UnitOfWork.SaveChanges();
            return entity.ToDto<ServicePackDuration, ServicePackDurationDetail>();
        }

        public void UpdateServicePackDuration(ServicePackDurationEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullServicePackDurationEditEntry, "ServicePackDurationEditEntry", null, ErrorMessage.Messages[ErrorCode.NullServicePackDurationEditEntry]));
                throw new ValidationError(violations);
            }

            var entity = UnitOfWork.ServicePackDurationRepository.FindById(entry.DurationId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundServicePackDuration, "ServicePackDuration", entry.DurationId, ErrorMessage.Messages[ErrorCode.NotFoundServicePackDuration]));
                throw new ValidationError(violations);
            }

            if (string.IsNullOrEmpty(entry.DurationName))
            {
                violations.Add(new RuleViolation(ErrorCode.NullDurationName, "DurationName", null, ErrorMessage.Messages[ErrorCode.NullDurationName]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.DurationName.Length > 300)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidDurationName, "DurationName", null, ErrorMessage.Messages[ErrorCode.InvalidDurationName]));
                    throw new ValidationError(violations);
                }
                else
                {
                    if (entity.DurationName != entry.DurationName)
                    {
                        bool isDuplicate = UnitOfWork.ServicePackDurationRepository.HasDurationNameExisted(entry.DurationName);
                        if (isDuplicate)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicateDurationName, "DurationName",
                                    entry.DurationName, ErrorMessage.Messages[ErrorCode.DuplicateDurationName]));
                            throw new ValidationError(violations);
                        }
                    }
                }
            }

            if (entity.AllotedTime != entry.AllotedTime)
            {
                bool hasAllotedTimeExisted = UnitOfWork.ServicePackDurationRepository.HasAllotedTimeExisted(entry.AllotedTime);
                if (hasAllotedTimeExisted)
                {
                    violations.Add(new RuleViolation(ErrorCode.DuplicateAllotedTime, "AllotedTime",
                            entry.AllotedTime, ErrorMessage.Messages[ErrorCode.DuplicateAllotedTime]));
                    throw new ValidationError(violations);
                }
            }

            entity.DurationName = entry.DurationName;
            entity.AllotedTime = entry.AllotedTime;
            entity.Unit = entry.Unit;
            entity.Description = entry.Description;
            entity.IsActive = entry.IsActive;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.ServicePackDurationRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateServicePackDurationStatus(int id, bool status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ServicePackDurationRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundServicePackDuration, "ServicePackDuration", id, ErrorMessage.Messages[ErrorCode.NotFoundServicePackDuration]));
                throw new ValidationError(violations);
            }
            if (entity.IsActive == status) return;

            entity.IsActive = status;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.ServicePackDurationRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        #endregion

        #region Service Pack Provider - Employee by Package
        public List<EmployeeInfoDetail> GetServicePackProviders(int packageId, EmployeeStatus? status = null)
        {
            var lst = new List<EmployeeInfoDetail>();
            var employees = UnitOfWork.ServicePackProviderRepository.GetServicePackProviders(packageId, status);
            if (employees == null) return lst;

            foreach (var employee in employees)
            {
                var contactInfo = employee.Contact;
                var contact = new ContactInfoDetail
                {
                    ContactId = contactInfo.ContactId,
                    Title = contactInfo.Title,
                    FirstName = contactInfo.FirstName,
                    LastName = contactInfo.LastName,
                    FullName = contactInfo.FirstName + " " + contactInfo.LastName,
                    DisplayName = contactInfo.DisplayName,
                    Sex = contactInfo.Sex,
                    Dob = contactInfo.Dob,
                    JobTitle = contactInfo.JobTitle,
                    Photo = contactInfo.Photo,
                    LinePhone1 = contactInfo.LinePhone1,
                    LinePhone2 = contactInfo.LinePhone2,
                    Mobile = contactInfo.Mobile,
                    Fax = contactInfo.Fax,
                    Email = contactInfo.Email,
                    Website = contactInfo.Website,
                    IdNo = contactInfo.IdNo,
                    IdIssuedDate = contactInfo.IdIssuedDate,
                    TaxNo = contactInfo.TaxNo,
                    IsActive = contactInfo.IsActive
                };

                if (employee.Contact.Photo != null)
                {
                    var photoInfo = DocumentService.GetFileInfoDetail(Convert.ToInt32(contactInfo.Photo));
                    contact.DocumentInfo = photoInfo;
                    contact.FileUrl = photoInfo.FileUrl;
                }

                var emergencyAddress = employee.EmergencyAddressId != null && employee.EmergencyAddressId > 0 ? ContactService.GetAddressInfoDetail(Convert.ToInt32(employee.EmergencyAddressId)) : null;
                var permanentAddress = employee.PermanentAddressId != null && employee.PermanentAddressId > 0 ? ContactService.GetAddressInfoDetail(Convert.ToInt32(employee.PermanentAddressId)) : null;

                var item = new EmployeeInfoDetail
                {
                    EmployeeId = employee.EmployeeId,
                    EmployeeNo = employee.EmployeeNo,
                    ContactId = employee.ContactId,
                    EmergencyAddressId = employee.EmergencyAddressId,
                    PermanentAddressId = employee.PermanentAddressId,
                    VendorId = employee.VendorId,
                    CompanyId = employee.CompanyId,
                    PositionId = employee.PositionId,
                    JoinedDate = employee.JoinedDate,
                    Status = employee.Status,
                    Contact = contact,
                    EmergencyAddress = emergencyAddress,
                    PermanentAddress = permanentAddress,
                    Company = employee.Company.ToDto<Entities.Business.Companies.Company, CompanyDetail>(),
                    JobPosition = employee.JobPosition.ToDto<JobPosition, JobPositionDetail>()
                };
                lst.Add(item);
            }
            return lst;
        }
        public SelectList PopulateProviderSelectList(int packageId, EmployeeStatus? status = null,
            int? selectedValue = null, bool? isShowSelectText = true)
        {
            return UnitOfWork.ServicePackProviderRepository.PopulateProviderSelectList(packageId, status, selectedValue, isShowSelectText);
        }

        public MultiSelectList PopulateAvailableProviderMultiSelectList(int? packageId = null, EmployeeStatus? status = null, string[] selectedValues = null)
        {
            return UnitOfWork.ServicePackProviderRepository.PopulateAvailableProviderMultiSelectList(packageId, status, selectedValues);
        }

        public MultiSelectList PopulateSelectedProviderMultiSelectList(int? packageId = null, EmployeeStatus? status = null, string[] selectedValues = null)
        {
            return UnitOfWork.ServicePackProviderRepository.PopulateSelectedProviderMultiSelectList(packageId, status, selectedValues);
        }

        public void InsertServicePackProvider(ServicePackProviderEntry entry)
        {
            bool isDuplicate = UnitOfWork.ServicePackProviderRepository.HasDataExisted(entry.PackageId, entry.EmployeeId);
            if (isDuplicate) return;
            var entity = entry.ToEntity<ServicePackProviderEntry, ServicePackProvider>();
            UnitOfWork.ServicePackProviderRepository.Insert(entity);
            UnitOfWork.SaveChanges();
        }

        #endregion

        #region Service Pack Rating
        public int GetDefaultServicePackRating(Guid applicationId)
        {
            var rateSetting = ApplicationService.GetRateSetting(applicationId, RatingSetting.Service);
            int ratings = Convert.ToInt32(rateSetting.Setting.KeyValue);
            return ratings;
        }
        public IEnumerable<ServicePackRatingDetail> GetServicePackRatings(int packageId)
        {
            var lst = UnitOfWork.ServicePackRatingRepository.GetServicePackRatings(packageId);
            return lst.ToDtos<ServicePackRating, ServicePackRatingDetail>();
        }

        public decimal InsertServicePackRating(ServicePackRatingEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry.PackageId <= 0)
            {
                violations.Add(new RuleViolation(ErrorCode.NullServicePackId, "ServicePackId", entry.PackageId));
                throw new ValidationError(violations);
            }
            else
            {
                var servicePack = UnitOfWork.ServicePackRepository.FindById(entry.PackageId);
                if (servicePack == null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidServicePackId, "ServicePackId", entry.PackageId));
                    throw new ValidationError(violations);
                }
            }

            if (entry.Rate <= 0)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidRate, "Rate", entry.Rate));
                throw new ValidationError(violations);
            }

            decimal averageRates = 0;
            var clientIp = NetworkUtils.GetIP4Address();

            var package = UnitOfWork.ServicePackRepository.FindById(entry.PackageId);
            if (package == null) return averageRates;

            var rating = entry.ToEntity<ServicePackRatingEntry, ServicePackRating>();
            rating.CreatedDate = DateTime.UtcNow;
            rating.Ip = clientIp;

            var ratings = UnitOfWork.ServicePackRatingRepository.GetServicePackRatings(entry.PackageId).ToList();
            if (!ratings.Any())
            {
                UnitOfWork.ServicePackRatingRepository.Insert(rating);
                UnitOfWork.SaveChanges();
            }
            else
            {
                var existedRating = ratings.FirstOrDefault(x => x.PackageId == entry.PackageId && x.Ip == clientIp);
                if (existedRating == null)
                {
                    UnitOfWork.ServicePackRatingRepository.Insert(rating);
                    UnitOfWork.SaveChanges();
                }
                else
                {
                    existedRating.Rate = entry.Rate;
                    existedRating.LastModifiedDate = DateTime.UtcNow;
                    UnitOfWork.ServicePackRatingRepository.Update(existedRating);
                    UnitOfWork.SaveChanges();
                }
            }

            ratings = UnitOfWork.ServicePackRatingRepository.GetServicePackRatings(entry.PackageId).ToList();
            var rateSum = ratings.Sum(d => d.Rate);
            averageRates = rateSum / ratings.Count;

            package.Rating = averageRates;
            UnitOfWork.ServicePackRepository.Update(package);
            UnitOfWork.SaveChanges();

            return averageRates;
        }
        #endregion

        #region Service Period
        public IEnumerable<ServicePeriodDetail> GetServicePeriods(bool? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = UnitOfWork.ServicePeriodRepository.GetPeriods(status, ref recordCount, orderBy, page, pageSize);
            return lst.ToDtos<ServicePeriod, ServicePeriodDetail>();
        }
        public ServicePeriodDetail GetServicePeriodDetail(int id)
        {
            var entity = UnitOfWork.ServicePeriodRepository.FindById(id);
            return entity.ToDto<ServicePeriod, ServicePeriodDetail>();
        }
        public SelectList PopulatePeriod(int? selectedValue = null, bool isShowSelectText = true)
        {
            return UnitOfWork.ServicePeriodRepository.PopulatePeriodSelectList(true, selectedValue, isShowSelectText);
        }

        public SelectList PopulateServicePeriodStatus(bool? selectedValue = true, bool isShowSelectText = false)
        {
            return UnitOfWork.ServicePeriodRepository.PopulatePeriodStatus(selectedValue, isShowSelectText);
        }

        public ServicePeriodDetail InsertServicePeriod(ServicePeriodEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullServicePeriodEntry, "ServicePeriodEntry", null, ErrorMessage.Messages[ErrorCode.NullServicePeriodEntry]));
                throw new ValidationError(violations);
            }

            if (string.IsNullOrEmpty(entry.PeriodName))
            {
                violations.Add(new RuleViolation(ErrorCode.NullPeriodName, "PeriodName", entry.PeriodName,
                    ErrorMessage.Messages[ErrorCode.NullPeriodName]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.PeriodName.Length > 250)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidPeriodName, "PeriodName", entry.PeriodName.Length,
                        ErrorMessage.Messages[ErrorCode.InvalidPeriodName]));
                    throw new ValidationError(violations);
                }
                else
                {
                    var isDuplicated = UnitOfWork.ServicePeriodRepository.HasDataExisted(entry.PeriodName);
                    if (isDuplicated)
                    {
                        violations.Add(new RuleViolation(ErrorCode.DuplicatePeriodName, "PeriodName", entry.PeriodName.Length,
                       ErrorMessage.Messages[ErrorCode.DuplicatePeriodName]));
                        throw new ValidationError(violations);
                    }
                }
            }

            var entity = entry.ToEntity<ServicePeriodEntry, ServicePeriod>();
            UnitOfWork.ServicePeriodRepository.Insert(entity);
            UnitOfWork.SaveChanges();
            return entity.ToDto<ServicePeriod, ServicePeriodDetail>();
        }
        public void UpdateServicePeriod(ServicePeriodEditEntry entry)
        {
            //Check validation
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullServicePeriodEditEntry, "ServicePeriodEditEntry", null, ErrorMessage.Messages[ErrorCode.NullServicePeriodEditEntry]));
                throw new ValidationError(violations);
            }

            var entity = UnitOfWork.ServicePeriodRepository.Find(entry.PeriodId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidPeriodId, "ServicePeriodEditEntry", entry.PeriodId, ErrorMessage.Messages[ErrorCode.InvalidPeriodId]));
                throw new ValidationError(violations);
            }

            if (string.IsNullOrEmpty(entry.PeriodName))
            {
                violations.Add(new RuleViolation(ErrorCode.NullPeriodName, "PeriodName", entry.PeriodName,
                    ErrorMessage.Messages[ErrorCode.NullPeriodName]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.PeriodName.Length > 250)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidPeriodName, "PeriodName", entry.PeriodName.Length,
                  ErrorMessage.Messages[ErrorCode.InvalidPeriodName]));
                    throw new ValidationError(violations);
                }
                else
                {
                    if (entity.PeriodName != entry.PeriodName)
                    {
                        var isDuplicated = UnitOfWork.ServicePeriodRepository.HasDataExisted(entry.PeriodName);
                        if (isDuplicated)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicatePeriodName, "PeriodName", entry.PeriodName.Length,
                           ErrorMessage.Messages[ErrorCode.DuplicatePeriodName]));
                            throw new ValidationError(violations);
                        }
                    }
                }
            }

            //Assign data
            entity.PeriodName = entry.PeriodName;
            entity.PeriodValue = entry.PeriodValue;
            entity.Status = entry.Status;

            UnitOfWork.ServicePeriodRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateServicePeriodStatus(int id, bool status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ServicePeriodRepository.Find(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundServicePeriod, "PeriodId", id, ErrorMessage.Messages[ErrorCode.NotFoundServicePeriod]));
                throw new ValidationError(violations);
            }

            if (entity.Status == status) return;

            entity.Status = status;
            UnitOfWork.ServicePeriodRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        #endregion

        #region Service Tax Rate
        public IEnumerable<ServiceTaxRateDetail> GeServiceTaxRates(ServiceTaxRateSearchEntry filter, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = UnitOfWork.ServiceTaxRateRepository.GetList(filter.IsActive, ref recordCount, orderBy, page, pageSize);
            return lst.ToDtos<ServiceTaxRate, ServiceTaxRateDetail>();
        }

        public SelectList PopulateServiceTaxRateSelectList(bool? status = null,
            int? selectedValue = null, bool? isShowSelectText = true)
        {
            return UnitOfWork.ServiceTaxRateRepository.PopulateServiceTaxRateSelectList(status, selectedValue, isShowSelectText);
        }

        public ServiceTaxRateDetail GetServiceTaxRateDetails(int serviceTaxRateId)
        {
            var entity = UnitOfWork.ServiceTaxRateRepository.FindById(serviceTaxRateId);
            return entity.ToDto<ServiceTaxRate, ServiceTaxRateDetail>();
        }
        public void InsertServiceTaxRate(ServiceTaxRateEntry entry)
        {
            ISpecification<ServiceTaxRateEntry> validator = new ServiceTaxRateEntryValidator(UnitOfWork, PermissionLevel.Create, CurrentClaimsIdentity);
            var dataViolations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isDataValid) throw new ValidationError(dataViolations);

            var isExisted = UnitOfWork.ServiceTaxRateRepository.HasDataExisted(entry.TaxRate, entry.IsPercent);
            if (isExisted)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.DuplicateTaxRate, "TaxRate", entry.TaxRate, ErrorMessage.Messages[ErrorCode.DuplicateTaxRate]));
                throw new ValidationError(dataViolations);
            }

            var entity = entry.ToEntity<ServiceTaxRateEntry, ServiceTaxRate>();
            entity.CreatedDate = DateTime.UtcNow;

            UnitOfWork.ServiceTaxRateRepository.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateServiceTaxRate(ServiceTaxRateEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullServiceTaxRateEditEntry, "ServiceTaxRateEditEntry", null, ErrorMessage.Messages[ErrorCode.NullServiceTaxRateEditEntry]));
                throw new ValidationError(violations);
            }


            var entity = UnitOfWork.ServiceTaxRateRepository.FindById(entry.TaxRateId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundServiceTaxRate, "ServiceTaxRate", entry.TaxRateId, ErrorMessage.Messages[ErrorCode.NotFoundServiceTaxRate]));
                throw new ValidationError(violations);
            }

            entity.TaxRate = entry.TaxRate;
            entity.IsPercent = entry.IsPercent;
            entity.Description = entry.Description;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.ServiceTaxRateRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateServiceTaxRateStatus(int id, bool status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ServiceTaxRateRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidTaxRateId, "TaxRateId", id, ErrorMessage.Messages[ErrorCode.InvalidTaxRateId]));
                throw new ValidationError(violations);
            }

            if (entity.IsActive == status) return;

            entity.IsActive = status;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.ServiceTaxRateRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region Service Booking
        
        public List<CalendarEventItem> GetCalendarEvents(int vendorId, ItemType type, DateTime? startDate, DateTime? endDate, OrderProductStatus? status=null)
        {
            var violations = new List<RuleViolation>();
            if (startDate != null && endDate != null)
            {
                if (DateTime.Compare(startDate.Value, endDate.Value) > 0)
                {
                    violations.Add(new RuleViolation(ErrorCode.ToDateMustBeGreaterThanFromDate, "ToDate", $"{startDate.Value}- {endDate.Value}", ErrorMessage.Messages[ErrorCode.ToDateMustBeGreaterThanFromDate]));
                    throw new ValidationError(violations);
                }
            }
            
            var list = new List<CalendarEventItem>();
            var orderProducts =OrderService.GetOrderProducts(vendorId, type, startDate, endDate, status);
            if (orderProducts != null && orderProducts.Count() > 0)
            {
                foreach (var orderProduct in orderProducts)
                {
                    var order = OrderService.GetOrderDetailByOrderNo(orderProduct.OrderNo);
                    var from = orderProduct.StartDate != null
                        ? Convert.ToDateTime(orderProduct.StartDate)
                        : Convert.ToDateTime(orderProduct.CreatedDate);
                    var to = orderProduct.EndDate != null
                        ? Convert.ToDateTime(orderProduct.EndDate)
                        : from;
                    var fromPeriod = orderProduct.FromPeriod != null ? Convert.ToInt32(orderProduct.FromPeriod) : 0;
                    var toPeriod = orderProduct.ToPeriod != null ? Convert.ToInt32(orderProduct.ToPeriod) : 0;
                    var start = from.AddMinutes(fromPeriod);
                    var end = to.AddMinutes(toPeriod);
                    var color = GetOrderProductStatusColor(orderProduct.Status);
                    var title = $"{orderProduct.Item.ItemName} - {order.Customer.FirstName} {order.Customer.LastName}";
                    var url = (type == ItemType.ServicePackage)
                        ? $"/Admin/ServicePack/Edit/{orderProduct.ProductId}" : $"/Admin/Product/Edit/{orderProduct.ProductId}";

                    var calendarEventItem = new CalendarEventItem
                    {
                        Id = orderProduct.OrderProductId.ToString(),
                        SomeKey = Convert.ToString(order.OrderId),
                        Title = title,
                        Start = start,
                        End = end,
                        Color = color,
                        AllDay = orderProduct.EndDate == start.AddHours(8),
                        Url = url,
                        Customer = order.Customer
                    };

                    if (orderProduct.EmployeeId != null && orderProduct.EmployeeId > 0)
                    {
                        calendarEventItem.Employee = EmployeeService.GetEmployeeDetail(Convert.ToInt32(orderProduct.EmployeeId));
                    }

                    list.Add(calendarEventItem);
                }
            }

            return list;
        }

        private string GetOrderProductStatusColor(OrderProductStatus status)
        {
            string color;
            switch (status)
            {
                case OrderProductStatus.InActive:
                    color = "#008000";
                    break;
                case OrderProductStatus.Active:
                    color = "#005B96";
                    break;
                default:
                    color = "#008000";
                    break;
            }
            return color;
        }

        private string GetOrderStatusColor(OrderStatus status)
        {
            string color;
            switch (status)
            {
                case OrderStatus.Pending:
                    color = "#008000";
                    break;
                case OrderStatus.Approved:
                    color = "#005B96";
                    break;
                case OrderStatus.Cancelled:
                    color = "#F59C00";
                    break;
                case OrderStatus.Rejected:
                    color = "#8B0000";
                    break;
                default:
                    color = "#000000";
                    break;
            }
            return color;
        }
        
        public void BookingSinglePackagesToCart(Guid applicationId, BookingSingleKindEntry entry)
        {
            var packages = entry.Packages;
            if (packages != null && packages.Any())
            {
                var cart = ShoppingCart.Instance;
                foreach (var package in packages)
                {
                    var packageEntity = UnitOfWork.ServicePackRepository.FindById(package.PackageId);
                    int allotedTime = 0; //minutes
                    if (packageEntity.DurationId != null)
                    {
                        var packageDurationEntity = UnitOfWork.ServicePackDurationRepository.FindById(packageEntity.DurationId);
                        allotedTime = packageDurationEntity.AllotedTime;
                    }

                    int fromPeriod = entry.FromPeriod ?? 0;
                    int toPeriod = fromPeriod + allotedTime;
                    int periodGroup = Convert.ToInt32(entry.PeriodGroup);

                    cart.Add(applicationId, package.PackageId, 1, ItemType.ServicePackage, package.EmployeeId, periodGroup, fromPeriod, toPeriod, entry.Comment);
                }
            }
        }

        public void BookingFullPackagesToCart(Guid applicationId, BookingPackageKindEntry entry)
        {
            var packages = entry.Packages;
            if (packages != null && packages.Any())
            {
                var cart = ShoppingCart.Instance;
                foreach (var package in packages)
                {
                    var packageEntity = UnitOfWork.ServicePackRepository.FindById(package.PackageId);
                    int allotedTime = 0; //minutes
                    if (packageEntity.DurationId != null)
                    {
                        var packageDurationEntity = UnitOfWork.ServicePackDurationRepository.FindById(packageEntity.DurationId);
                        allotedTime = packageDurationEntity.AllotedTime;
                    }

                    int fromPeriod = entry.FromPeriod ?? 0;
                    int toPeriod = fromPeriod + allotedTime;
                    int periodGroup = Convert.ToInt32(entry.PeriodGroup);

                    cart.Add(applicationId, package.PackageId, 1, ItemType.ServicePackage, package.EmployeeId, periodGroup, fromPeriod, toPeriod, entry.Comment);
                }
            }
        }

        #endregion

        #region Service Discount

        public IEnumerable<ServiceDiscountDetail> GeServiceDiscounts(ServiceDiscountSearchEntry filter, ref int? recordCount,
           string orderBy = null, int? page = null, int? pageSize = null)
        {
            var violations = new List<RuleViolation>();
            if (filter.FromDate != null && filter.ToDate != null)
            {
                if (DateTime.Compare(filter.FromDate.Value, filter.ToDate.Value) > 0)
                {
                    violations.Add(new RuleViolation(ErrorCode.EndDateMustBeGreaterThanStartDate, "EndDate"));
                    throw new ValidationError(violations);
                }
            }

            var lst = UnitOfWork.ServiceDiscountRepository.GetServiceDiscounts(filter.FromDate, filter.ToDate, filter.IsActive, ref recordCount, orderBy, page, pageSize);
            return lst.ToDtos<ServiceDiscount, ServiceDiscountDetail>();
        }
        public SelectList PopulateServiceDiscountSelectList(ServiceDiscountStatus? status = null, int? selectedValue = null, bool? isShowSelectText = true)
        {
            return UnitOfWork.ServiceDiscountRepository.PopulateServiceDiscountSelectList(status, selectedValue, isShowSelectText);
        }
        public SelectList PopulateServiceDiscountSelectList(DiscountType type, ServiceDiscountStatus? status = null, int? selectedValue = null, bool? isShowSelectText = true)
        {
            return UnitOfWork.ServiceDiscountRepository.PopulateServiceDiscountSelectList(type, status, selectedValue, isShowSelectText);
        }
        public ServiceDiscountDetail GetServiceDiscountDetails(int discountId)
        {
            var entity = UnitOfWork.ServiceDiscountRepository.FindById(discountId);
            return entity.ToDto<ServiceDiscount, ServiceDiscountDetail>();
        }
        public void InsertServiceDiscount(ServiceDiscountEntry entry)
        {
            ISpecification<ServiceDiscountEntry> validator = new ServiceDiscountEntryValidator(UnitOfWork, PermissionLevel.Create, CurrentClaimsIdentity);
            var dataViolations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isDataValid) throw new ValidationError(dataViolations);

            bool isDuplicate = UnitOfWork.ServiceDiscountRepository.HasDataExisted(entry.Quantity, entry.DiscountRate, entry.IsPercent);
            if (isDuplicate)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.DuplicateServiceDiscount, "ServiceDiscount",
                    $"{entry.Quantity}-{entry.DiscountRate}-{entry.IsPercent}", ErrorMessage.Messages[ErrorCode.DuplicateServiceDiscount]));
                throw new ValidationError(dataViolations);
            }

            var entity = entry.ToEntity<ServiceDiscountEntry, ServiceDiscount>();
            entity.CreatedDate = DateTime.UtcNow;

            UnitOfWork.ServiceDiscountRepository.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateServiceDiscount(ServiceDiscountEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullServiceDiscountEditEntry, "ServiceDiscountEditEntry", null, ErrorMessage.Messages[ErrorCode.NullServiceDiscountEditEntry]));
                throw new ValidationError(violations);
            }

            if (entry.DiscountRate < 0)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidDiscountRate, "TaxRate", entry.DiscountRate, ErrorMessage.Messages[ErrorCode.InvalidDiscountRate]));
                throw new ValidationError(violations);
            }

            if (entry.StartDate.HasValue && entry.EndDate.HasValue)
            {
                if (DateTime.Compare(entry.EndDate.Value, entry.EndDate.Value) > 0)
                {
                    violations.Add(new RuleViolation(ErrorCode.EndDateMustBeGreaterThanStartDate, "EndDate", entry.EndDate, ErrorMessage.Messages[ErrorCode.InvalidDiscountRate]));
                    throw new ValidationError(violations);
                }
            }

            var entity = UnitOfWork.ServiceDiscountRepository.FindById(entry.DiscountId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundServiceDiscount, "ServiceDiscount", entry.DiscountId, ErrorMessage.Messages[ErrorCode.NotFoundServiceDiscount]));
                throw new ValidationError(violations);
            }

            entity.DiscountCode = entry.DiscountCode;
            entity.DiscountType = entry.DiscountType;
            entity.Quantity = entry.Quantity;
            entity.DiscountRate = entry.DiscountRate;
            entity.IsPercent = entry.IsPercent;
            entity.StartDate = entry.StartDate;
            entity.EndDate = entry.EndDate;
            entity.Description = entry.Description;
            entity.IsActive = entry.IsActive;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.ServiceDiscountRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateServiceDiscountStatus(int id, ServiceDiscountStatus status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ServiceDiscountRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundServiceDiscount, "ServiceDiscount", id, ErrorMessage.Messages[ErrorCode.NotFoundServiceDiscount]));
                throw new ValidationError(violations);
            }

            var isValid = Enum.IsDefined(typeof(ServiceDiscountStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status", status, ErrorMessage.Messages[ErrorCode.InvalidStatus]));
                throw new ValidationError(violations);
            }
            if (entity.IsActive == status) return;

            entity.IsActive = status;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.ServiceDiscountRepository.Update(entity);
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
                    ApplicationService = null;
                    ContactService = null;
                    CurrencyService = null;
                    DocumentService = null;
                    EmployeeService = null;
                    MailService = null;
                    MessageService = null;
                    NotificationService = null;
                    OrderService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}
