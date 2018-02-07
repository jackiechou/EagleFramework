using Eagle.Common.Utilities;
using Eagle.Core.Permission;
using Eagle.Core.Settings;
using Eagle.Entities.Contents.Banners;
using Eagle.Repositories;
using Eagle.Resources;
using Eagle.Services.Contents.Validation;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Contents.Banners;
using Eagle.Services.EntityMapping.Common;
using Eagle.Services.Exceptions;
using Eagle.Services.SystemManagement;
using Eagle.Services.Validations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace Eagle.Services.Contents
{
    public class BannerService : BaseService, IBannerService
    {
        #region Contruct
        private IPageService PageService { get; set; }
        private IDocumentService DocumentService { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BannerService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="pageService"></param>
        /// <param name="documentService"></param>
        public BannerService(IUnitOfWork unitOfWork, IPageService pageService, IDocumentService documentService) : base(unitOfWork)
        {
            PageService = pageService;
            DocumentService = documentService;
        }

        #endregion

        #region Banner Position

        public IEnumerable<BannerPositionDetail> GetBannerPositions(BannerPositionStatus? status, ref int? recordCount, int? page, int? pageSize)
        {
            var entityList = UnitOfWork.BannerPositionRepository.GetList(status, ref recordCount, page, pageSize);
            return entityList.ToDtos<BannerPosition, BannerPositionDetail>();
        }
        public IEnumerable<BannerPositionDetail> GetBannerPositions(BannerPositionStatus? status)
        {
            var entityList = UnitOfWork.BannerPositionRepository.GetList(status);
            return entityList.ToDtos<BannerPosition, BannerPositionDetail>();
        }
        public BannerPositionDetail GetBannerPositionDetail(int id)
        {
            var entity = UnitOfWork.BannerPositionRepository.Find(id);
            return entity.ToDto<BannerPosition, BannerPositionDetail>();
        }
        public SelectList PoplulateBannerPositionSelectList(bool? isShowSelectText = null, int? selectedValue = null)
        {
            var listItems = new List<SelectListItem>();
            var lst = UnitOfWork.BannerPositionRepository.GetList(BannerPositionStatus.Active).ToList();
            if (lst.Any())
            {
                listItems = lst.Select(p => new SelectListItem { Text = p.PositionName, Value = p.PositionId.ToString() }).ToList();
                if (isShowSelectText != null && isShowSelectText == true)
                    listItems.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.Select} ---", Value = "" });
            }
            else
            {
                listItems.Insert(0, new SelectListItem { Text = $"-- {LanguageResource.None} --", Value = "-1" });
            }
            return new SelectList(listItems, "Value", "Text", selectedValue);
        }

        public MultiSelectList PopulateBannerPositionMultiSelectList(int bannerId, bool? isShowSelectText = null, BannerPositionStatus? status = null)
        {
            var selectedValues = UnitOfWork.BannerZoneRepository.GetListByBannerId(bannerId).Select(x => x.PositionId).ToArray();
            return UnitOfWork.BannerPositionRepository.PopulateBannerPositionMultiSelectList(isShowSelectText, status, selectedValues);
        }

        public BannerPositionDetail InsertBannerPosition(BannerPositionEntry entry)
        {
            ISpecification<BannerPositionEntry> validator = new BannerPositionEntryValidator(CurrentClaimsIdentity, PermissionLevel.Edit);
            var violations = new List<RuleViolation>();
            if (!validator.IsSatisfyBy(entry, violations)) throw new ValidationError(violations);

            var isDataDuplicate = UnitOfWork.BannerPositionRepository.HasDataExisted(entry.PositionName);
            if (isDataDuplicate)
            {
                violations.Add(new RuleViolation(ErrorCode.DuplicateBannerPositionName, "PositionName", entry.PositionName));
                throw new ValidationError(violations);
            }

            var listOrder = UnitOfWork.BannerPositionRepository.GetLastListOrder() + 1;

            var entity = entry.ToEntity<BannerPositionEntry, BannerPosition>();
            entity.ListOrder = Convert.ToInt32(listOrder);
            UnitOfWork.BannerPositionRepository.Insert(entity);
            UnitOfWork.SaveChanges();
            return entity.ToDto<BannerPosition, BannerPositionDetail>();
        }
        public void UpdateBannerPosition(BannerPositionEditEntry entry)
        {
            //Check validation
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundBannerPositionEntry, "BannerPositionEntry"));
                throw new ValidationError(violations);
            }

            var entity = UnitOfWork.BannerPositionRepository.Find(entry.PositionId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundBannerPositionEntry, "BannerPositionEntry"));
                throw new ValidationError(violations);
            }

            if (!string.IsNullOrEmpty(entry.PositionName) && entry.PositionName.Length > 250)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidName, "PositionName"));
                throw new ValidationError(violations);
            }

            if (!string.IsNullOrEmpty(entry.Description) && entry.Description.Length > 500)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidDescription, "Description"));
                throw new ValidationError(violations);
            }

            if (!Enum.IsDefined(typeof(BannerPositionStatus), entry.Status))
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status"));
                throw new ValidationError(violations);
            }

            //Assign data
            entity.PositionName = entry.PositionName;
            entity.Description = entry.Description;
            entity.Status = entry.Status;
            UnitOfWork.BannerPositionRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateBannerPositionStatus(int id, BannerPositionStatus status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.BannerPositionRepository.Find(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundBannerPostion, "BannerPostion"));
                throw new ValidationError(violations);
            }

            var isValid = Enum.IsDefined(typeof(BannerPositionStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status", null,
                    ErrorMessage.Messages[ErrorCode.InvalidStatus]));
                throw new ValidationError(violations);
            }
            if (entity.Status == status) return;

            entity.Status = status;
            UnitOfWork.BannerPositionRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateBannerPositionListOrder(int id, int listOrder)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.BannerPositionRepository.Find(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundBannerPostion, "BannerPostion"));
                throw new ValidationError(violations);
            }

            entity.ListOrder = listOrder;
            UnitOfWork.BannerPositionRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void DeleteBannerPosition(int id)
        {
            var entity = UnitOfWork.BannerPositionRepository.Find(id);
            if (entity == null) throw new BaseException($"Cannot find item for id: {id}");
            UnitOfWork.BannerPositionRepository.Delete(entity);
            UnitOfWork.SaveChanges();
        }

        #endregion

        #region Banner Type
        public IEnumerable<BannerTypeDetail> GetActiveBannerTypes()
        {
            var entityList = UnitOfWork.BannerTypeRepository.GetActiveList();
            return entityList.ToDtos<BannerType, BannerTypeDetail>().AsEnumerable();
        }
        public IEnumerable<BannerTypeDetail> GetBannerTypes(ref int? recordCount, int? page, int? pageSize)
        {
            var entityList = UnitOfWork.BannerTypeRepository.GetList(ref recordCount, page, pageSize);
            return entityList.ToDtos<BannerType, BannerTypeDetail>().AsEnumerable();
        }
        public BannerTypeDetail GetBannerTypeDetails(int id)
        {
            var entity = UnitOfWork.BannerTypeRepository.Find(id);
            return entity.ToDto<BannerType, BannerTypeDetail>();
        }
        public SelectList PopulateBannerTypeSelectList(int? selectedValue, bool? isShowSelectText)
        {
            return UnitOfWork.BannerTypeRepository.PopulateBannerTypeSelectList(selectedValue, isShowSelectText);
        }
        public BannerTypeDetail InsertBannerType(BannerTypeEntry entry)
        {
            ISpecification<BannerTypeEntry> validator = new BannerTypeEntryValidator(CurrentClaimsIdentity, PermissionLevel.Edit);
            var violations = new List<RuleViolation>();
            if (!validator.IsSatisfyBy(entry, violations)) throw new ValidationError(violations);

            var isDataDuplicate = UnitOfWork.BannerTypeRepository.HasDataExisted(entry.TypeName);
            if (isDataDuplicate) throw new DuplicateNameException();

            var entity = entry.ToEntity<BannerTypeEntry, BannerType>();
            UnitOfWork.BannerTypeRepository.Insert(entity);
            UnitOfWork.SaveChanges();
            return entity.ToDto<BannerType, BannerTypeDetail>();
        }
        public void UpdateBannerType(BannerTypeEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundBannerTypeEditEntry, "BannerTypeEditEntry"));
                throw new ValidationError(violations);
            }

            var entity = UnitOfWork.BannerTypeRepository.Find(entry.TypeId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidBannerTypeId, "TypeId", entry.TypeId));
                throw new ValidationError(violations);
            }

            if (!string.IsNullOrEmpty(entry.TypeName) && entry.TypeName.Length > 250)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidName, "TypeName"));
                throw new ValidationError(violations);
            }

            if (!string.IsNullOrEmpty(entry.Description) && entry.Description.Length > 500)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidDescription, "Description"));
                throw new ValidationError(violations);
            }

            if (!Enum.IsDefined(typeof(BannerTypeStatus), entry.Status))
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status"));
                throw new ValidationError(violations);
            }

            entity.TypeName = entry.TypeName;
            entity.Description = entry.Description;
            entity.Status = entry.Status;

            UnitOfWork.BannerTypeRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateBannerTypeStatus(int id, BannerTypeStatus status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.BannerTypeRepository.Find(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundBannerType, "Id", id, ErrorMessage.GetValue(ErrorCode.NotFoundBannerType)));
                throw new ValidationError(violations);
            }

            var isValid = Enum.IsDefined(typeof(BannerTypeStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status", null,
                    ErrorMessage.Messages[ErrorCode.InvalidStatus]));
                throw new ValidationError(violations);
            }
            if (entity.Status == status) return;

            entity.Status = status;
            UnitOfWork.BannerTypeRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void DeleteBannerType(int id)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.BannerTypeRepository.Find(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundBanner, "Banner", id));
                throw new ValidationError(violations);
            }
            UnitOfWork.BannerTypeRepository.Delete(entity);
            UnitOfWork.SaveChanges();
        }

        #endregion

        #region Banner Scope
        public IEnumerable<BannerScopeDetail> GetActiveBannerScopes()
        {
            var entityList = UnitOfWork.BannerScopeRepository.GetActiveList();
            return entityList.ToDtos<BannerScope, BannerScopeDetail>().AsEnumerable();
        }
        public IEnumerable<BannerScopeDetail> GetBannerScopes(ref int? recordCount, int? page, int? pageSize)
        {
            var entityList = UnitOfWork.BannerScopeRepository.GetList(ref recordCount, page, pageSize);
            return entityList.ToDtos<BannerScope, BannerScopeDetail>().AsEnumerable();
        }
        public BannerScopeDetail GetBannerScopeDetails(int id)
        {
            var entity = UnitOfWork.BannerScopeRepository.Find(id);
            return entity.ToDto<BannerScope, BannerScopeDetail>();
        }
        public SelectList PopulateBannerScopeSelectList(int? selectedValue, bool? isShowSelectText)
        {
            return UnitOfWork.BannerScopeRepository.PopulateBannerScopeSelectList(selectedValue, isShowSelectText);
        }
        public BannerScopeDetail InsertBannerScope(BannerScopeEntry entry)
        {
            ISpecification<BannerScopeEntry> validator = new BannerScopeEntryValidator(CurrentClaimsIdentity, PermissionLevel.Edit);
            var violations = new List<RuleViolation>();
            if (!validator.IsSatisfyBy(entry, violations)) throw new ValidationError(violations);

            var isDataDuplicate = UnitOfWork.BannerScopeRepository.HasDataExisted(entry.ScopeName);
            if (isDataDuplicate) throw new DuplicateNameException();

            var entity = entry.ToEntity<BannerScopeEntry, BannerScope>();
            UnitOfWork.BannerScopeRepository.Insert(entity);
            UnitOfWork.SaveChanges();
            return entity.ToDto<BannerScope, BannerScopeDetail>();
        }
        public void UpdateBannerScope(BannerScopeEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundBannerScopeEditEntry, "BannerScopeEditEntry"));
                throw new ValidationError(violations);
            }

            var entity = UnitOfWork.BannerScopeRepository.Find(entry.ScopeId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidBannerScopeId, "ScopeId", entry.ScopeId));
                throw new ValidationError(violations);
            }

            if (!string.IsNullOrEmpty(entry.ScopeName) && entry.ScopeName.Length > 250)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidName, "ScopeName"));
                throw new ValidationError(violations);
            }

            if (!string.IsNullOrEmpty(entry.Description) && entry.Description.Length > 500)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidDescription, "Description"));
                throw new ValidationError(violations);
            }

            if (!Enum.IsDefined(typeof(BannerScopeStatus), entry.Status))
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status"));
                throw new ValidationError(violations);
            }

            entity.ScopeName = entry.ScopeName;
            entity.Description = entry.Description;
            entity.Status = entry.Status;

            UnitOfWork.BannerScopeRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateBannerScopeStatus(int id, BannerScopeStatus status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.BannerScopeRepository.Find(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundBannerScope, "Id", id, ErrorMessage.GetValue(ErrorCode.NotFoundBannerScope)));
                throw new ValidationError(violations);
            }

            var isValid = Enum.IsDefined(typeof(BannerScopeStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status", null,
                    ErrorMessage.Messages[ErrorCode.InvalidStatus]));
                throw new ValidationError(violations);
            }
            if (entity.Status == status) return;

            entity.Status = status;
            UnitOfWork.BannerScopeRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void DeleteBannerScope(int id)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.BannerScopeRepository.Find(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundBannerScope, "BannerScopeId", id));
                throw new ValidationError(violations);
            }
            UnitOfWork.BannerScopeRepository.Delete(entity);
            UnitOfWork.SaveChanges();
        }

        #endregion

        #region Banner Page
        public IEnumerable<BannerPageDetail> GetBannerPagesByBannerId(int bannerId)
        {
            var entityList = UnitOfWork.BannerPageRepository.GetListByBannerId(bannerId);
            return entityList.ToDtos<BannerPage, BannerPageDetail>();
        }

        public IEnumerable<BannerPageDetail> GetBannerPagesByPageId(int pageId)
        {
            var entityList = UnitOfWork.BannerPageRepository.GetListByPageId(pageId);
            return entityList.ToDtos<BannerPage, BannerPageDetail>();
        }
        public MultiSelectList PopulateBannerPageMultiSelectList(int bannerId)
        {
            var selectedValues = UnitOfWork.BannerPageRepository.GetListByBannerId(bannerId).Select(x => x.PageId.ToString()).ToList();
            return PageService.PopulatePageMultiSelectList(PageType.Site, null, selectedValues);
        }
        public void CreateBannerPages(int bannerId, List<int> pageIds)
        {
            if (pageIds != null && bannerId > 0)
            {
                foreach (var pageId in pageIds)
                {
                    bool isDuplicated = UnitOfWork.BannerPageRepository.HasDataExisted(bannerId, pageId);
                    if (!isDuplicated)
                    {
                        var bannerPage = new BannerPage
                        {
                            BannerId = bannerId,
                            PageId = pageId
                        };
                        UnitOfWork.BannerPageRepository.Insert(bannerPage);
                    }
                }
                UnitOfWork.SaveChanges();
            }
        }

        public void DeleteBannerPages(int bannerId, List<int> pageIds)
        {
            if (pageIds != null && bannerId > 0)
            {
                foreach (var pageId in pageIds)
                {
                    var entity = UnitOfWork.BannerPageRepository.GetDetails(bannerId, pageId);
                    if (entity != null)
                    {
                        UnitOfWork.BannerPageRepository.Delete(entity);
                    }
                }
                UnitOfWork.SaveChanges();
            }
        }
        #endregion

        #region Banner Zone - Position
        public IEnumerable<BannerZoneInfoDetail> GetBannerZonesByBannerId(int bannerId, BannerStatus? status = null)
        {
            var lst = new List<BannerZoneInfoDetail>();
            var zones = UnitOfWork.BannerZoneRepository.GetListByBannerId(bannerId, status);
            if (zones != null)
            {
                foreach (var zone in zones)
                {
                    var position = UnitOfWork.BannerPositionRepository.FindById(zone.PositionId);
                    var zoneInfo = new BannerZoneInfoDetail
                    {
                        BannerId = zone.BannerId,
                        PositionId = zone.PositionId,
                        Position = position.ToDto<BannerPosition, BannerPositionDetail>()
                    };
                    lst.Add(zoneInfo);
                }
            }
            return lst;
        }

        public IEnumerable<BannerZoneDetail> GetBannerZonesByPositionId(int positionId, BannerStatus? status=null)
        {
            var entityList = UnitOfWork.BannerZoneRepository.GetListByPositionId(positionId, status);
            return entityList.ToDtos<BannerZone, BannerZoneDetail>();
        }

        public void CreateBannerZones(int bannerId, List<int> positionIds)
        {
            if (positionIds == null || bannerId <= 0) return;

            foreach (var positionId in positionIds)
            {
                bool isDuplicated = UnitOfWork.BannerZoneRepository.HasDataExisted(bannerId, positionId);
                if (!isDuplicated)
                {
                    var bannerZone = new BannerZone
                    {
                        BannerId = bannerId,
                        PositionId = positionId
                    };
                    UnitOfWork.BannerZoneRepository.Insert(bannerZone);
                }
            }
            UnitOfWork.SaveChanges();
        }

        public void DeleteBannerZones(int bannerId, List<int> positionIds)
        {
            if (positionIds == null || bannerId <= 0) return;

            foreach (var positionId in positionIds)
            {
                var entity = UnitOfWork.BannerZoneRepository.GetDetails(bannerId, positionId);
                if (entity != null)
                {
                    UnitOfWork.BannerZoneRepository.Delete(entity);
                }
            }
            UnitOfWork.SaveChanges();
        }

        #endregion

        #region Banner
        public SelectList PopulateLinkTargets(string selectedValue, bool isShowSelectText = false)
        {
            List<SelectListItem> lst = new List<SelectListItem>
            {
                new SelectListItem {Text = LanguageResource.LoadInANewWindow, Value = "_blank"},
                new SelectListItem {Text = LanguageResource.LoadInTheSameFrameAsItWasClicked, Value = "_self"},
                new SelectListItem {Text = LanguageResource.LoadInTheParentFrameset, Value = "_parent"},
                new SelectListItem {Text = LanguageResource.LoadInTheFullBodyOfTheWindow, Value = "_top"}
            };
            if (isShowSelectText)
                lst.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.Select} ---", Value = "" });
            return new SelectList(lst, "Value", "Text", selectedValue);
        }
        public IEnumerable<BannerInfoDetail> Search(int vendorId, string languageCode, BannerSearchEntry entry, out int recordCount, string order = null, int? page = null, int? pageSize = null)
        {
            var lst = new List<BannerInfoDetail>();
            var banners = UnitOfWork.BannerRepository.Search(out recordCount, vendorId, languageCode, entry.BannerName, entry.Advertiser,
                entry.BannerTypeId, entry.BannerPositionId, entry.Status, order, page, pageSize).ToList();
            if (banners.Any())
            {
                foreach (var banner in banners)
                {
                    var scope = UnitOfWork.BannerScopeRepository.FindById(banner.ScopeId);
                    var type = UnitOfWork.BannerTypeRepository.FindById(banner.TypeId);
                    var zones = GetBannerZonesByBannerId(banner.BannerId);

                    var item = new BannerInfoDetail
                    {
                        BannerId = banner.BannerId,
                        VendorId = banner.VendorId,
                        ScopeId = banner.ScopeId,
                        TypeId = banner.TypeId,
                        BannerTitle = banner.BannerTitle,
                        BannerContent = banner.BannerContent,
                        AltText = banner.AltText,
                        Link = banner.Link,
                        Description = banner.Description,
                        Advertiser = banner.Advertiser,
                        Tags = banner.Tags,
                        ClickThroughs = banner.ClickThroughs,
                        Impressions = banner.Impressions,
                        Width = banner.Width,
                        Height = banner.Height,
                        ListOrder = banner.ListOrder,
                        Target = banner.Target,
                        StartDate = banner.StartDate,
                        EndDate = banner.EndDate,
                        Status = banner.Status,
                        FileId = banner.FileId,
                        Scope = scope.ToDto<BannerScope, BannerScopeDetail>(),
                        Type = type.ToDto<BannerType, BannerTypeDetail>(),
                        Zones = zones
                    };

                    if (banner.FileId != null && banner.FileId > 0)
                    {
                        item.Document = DocumentService.GetFileInfoDetail((int)banner.FileId);
                        if (item.Document != null)
                        {
                            item.FileUrl = item.Document.FileUrl;
                        }
                    }

                    lst.Add(item);
                }
            }
            return lst;
        }

        public IEnumerable<BannerInfoDetail> GetBanners(int vendorId, BannerTypeSetting type, BannerPositionSetting position, int? quantity, BannerStatus? status)
        {
            var lst = new List<BannerInfoDetail>();
            var banners = UnitOfWork.BannerRepository.GetBanners(vendorId, type, position, quantity, status).ToList();
            if (banners.Any())
            {
                foreach (var banner in banners)
                {
                    var item = new BannerInfoDetail
                    {
                        BannerId = banner.BannerId,
                        VendorId = banner.VendorId,
                        ScopeId = banner.ScopeId,
                        TypeId = banner.TypeId,
                        BannerTitle = banner.BannerTitle,
                        BannerContent = banner.BannerContent,
                        AltText = banner.AltText,
                        Link = banner.Link,
                        Description = banner.Description,
                        Advertiser = banner.Advertiser,
                        Tags = banner.Tags,
                        ClickThroughs = banner.ClickThroughs,
                        Impressions = banner.Impressions,
                        Width = banner.Width,
                        Height = banner.Height,
                        ListOrder = banner.ListOrder,
                        Target = banner.Target,
                        StartDate = banner.StartDate,
                        EndDate = banner.EndDate,
                        Status = banner.Status,
                        FileId = banner.FileId,

                    };

                    if (banner.FileId != null && banner.FileId > 0)
                    {
                        item.Document = DocumentService.GetFileInfoDetail((int)banner.FileId);
                        if (item.Document != null)
                        {
                            item.FileUrl = item.Document.FileUrl;
                        }
                    }

                    lst.Add(item);
                }
            }
            return lst;
        }

        public BannerDetail GetBannerDetails(int id)
        {
            var entity = UnitOfWork.BannerRepository.FindById(id);
            return entity.ToDto<Banner, BannerDetail>();
        }
        public BannerDetail Insert(Guid applicationId, Guid userId, int vendorId, string languageCode, BannerEntry entry)
        {
            ISpecification<BannerEntry> validator = new BannerEntryValidator(UnitOfWork, PermissionLevel.Create, CurrentClaimsIdentity);
            var violations = new List<RuleViolation>();
            if (!validator.IsSatisfyBy(entry, violations)) throw new ValidationError(violations);

            var listOrder = UnitOfWork.BannerRepository.GetNewListOrder();
            var entity = entry.ToEntity<BannerEntry, Banner>();

            if (entry.File != null && entry.File.ContentLength > 0)
            {
                var fileInfo = DocumentService.UploadAndSaveDbByFolderId(applicationId, userId, entry.File, (int)FileLocation.Banner, StorageType.Local);
                entity.FileId = fileInfo.FileId;
            }

            entity.VendorId = vendorId;
            entity.ListOrder = listOrder;
            entity.LanguageCode = languageCode;
            entity.Ip = NetworkUtils.GetIP4Address();
            entity.CreatedByUserId = userId;
            entity.CreatedDate = DateTime.UtcNow;

            UnitOfWork.BannerRepository.Insert(entity);
            UnitOfWork.SaveChanges();

            //Save selected positions
            if (entry.SelectedPositions.Any())
            {
                CreateBannerZones(entity.BannerId, entry.SelectedPositions);
            }

            //Save selected pages
            if (entry.SelectedPages != null)
            {
                CreateBannerPages(entity.BannerId, entry.SelectedPages);
            }

            return entity.ToDto<Banner, BannerDetail>();
        }
        public void Update(Guid applicationId, Guid userId, string languageCode, BannerEditEntry entry)
        {
            ISpecification<BannerEditEntry> validator = new BannerEditEntryValidator(UnitOfWork, PermissionLevel.Edit, CurrentClaimsIdentity);
            var violations = new List<RuleViolation>();
            if (!validator.IsSatisfyBy(entry, violations)) throw new ValidationError(violations);

            var entity = UnitOfWork.BannerRepository.Find(entry.BannerId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullBanner, "Banner"));
                throw new ValidationError(violations);
            }

            if (entry.File != null && entry.File.ContentLength > 0)
            {
                var existedFileId = entity.FileId;
                if (existedFileId != null)
                {
                    DocumentService.DeleteFile(Convert.ToInt32(existedFileId));
                }

                var fileInfo = DocumentService.UploadAndSaveDbByFolderId(applicationId, userId, entry.File, (int)FileLocation.Banner, StorageType.Local);
                entity.FileId = fileInfo.FileId;
            }

            entity.ScopeId = entry.ScopeId;
            entity.TypeId = entry.TypeId;
            entity.BannerTitle = entry.BannerTitle;
            entity.AltText = entry.AltText;
            entity.BannerContent = entry.BannerContent;
            entity.Advertiser = entry.Advertiser;
            entity.Link = entry.Link;
            entity.Target = entry.Target;
            entity.Description = entry.Description;
            entity.Tags = entry.Tags;
            entity.ClickThroughs = entry.ClickThroughs;
            entity.Width = entry.Width;
            entity.Height = entry.Height;
            entity.StartDate = entry.StartDate;
            entity.EndDate = entry.EndDate;
            entity.Status = entry.Status;
            entity.LanguageCode = languageCode;
            entity.LastModifiedByUserId = userId;
            entity.LastModifiedDate = DateTime.UtcNow;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();

            UnitOfWork.BannerRepository.Update(entity);
            UnitOfWork.SaveChanges();

            //Save selected positions
            if (entry.SelectedPositions.Any())
            {
                var selectedPositionIds = entry.SelectedPositions;
                var existedPositionIds = UnitOfWork.BannerZoneRepository.GetListByBannerId(entry.BannerId).Select(x => x.PositionId).ToList();
                var differentPositionIds = existedPositionIds.Except(selectedPositionIds).ToList();
                if (differentPositionIds.Any())
                {
                    DeleteBannerZones(entry.BannerId, differentPositionIds);
                }

                var newPositionIds = selectedPositionIds.Except(existedPositionIds).ToList();
                if (newPositionIds.Any())
                {
                    CreateBannerZones(entry.BannerId, newPositionIds);
                }
            }

            //Save selected pages
            if (entry.SelectedPages.Any())
            {
                var selectedPageIds = entry.SelectedPages;
                var existedPageIds = UnitOfWork.BannerPageRepository.GetListByBannerId(entry.BannerId).Select(x => x.PageId).ToList();
                var differentPageIds = existedPageIds.Except(selectedPageIds).ToList();
                if (differentPageIds.Any())
                {
                    DeleteBannerPages(entry.BannerId, differentPageIds);
                }

                var newPageIds = selectedPageIds.Except(existedPageIds).ToList();
                if (newPageIds.Any())
                {
                    CreateBannerPages(entry.BannerId, newPageIds);
                }
            }
        }
        public void UpdateBannerStatus(Guid userId, int id, BannerStatus status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.BannerRepository.Find(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullBanner, "Banner"));
                throw new ValidationError(violations);
            }

            var isValid = Enum.IsDefined(typeof(BannerStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status", null,
                    ErrorMessage.Messages[ErrorCode.InvalidStatus]));
                throw new ValidationError(violations);
            }
            if (entity.Status == status) return;

            entity.Status = status;
            entity.LastModifiedDate = DateTime.UtcNow;
            entity.LastModifiedByUserId = userId;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();

            UnitOfWork.BannerRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void DeleteBanner(int id)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.BannerRepository.Find(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullBanner, "Banner"));
                throw new ValidationError(violations);
            }

            //Delete all pages that contain the selected banner
            var existedPageIds = UnitOfWork.BannerPageRepository.GetListByBannerId(id).Select(x => x.PageId).ToList();            
            if (existedPageIds.Any())
            {
                DeleteBannerPages(id, existedPageIds);
            }

            //Delete all positions that belongs to the selected banner
            var existedPositionIds = UnitOfWork.BannerZoneRepository.GetListByBannerId(id).Select(x => x.PositionId).ToList();            
            if (existedPositionIds.Any())
            {
                DeleteBannerZones(id, existedPositionIds);
            }

            if (entity.FileId != null && entity.FileId > 0)
            {
                DocumentService.DeleteFile(Convert.ToInt32(entity.FileId));
            }
            UnitOfWork.BannerRepository.Delete(entity);
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
                    PageService = null;
                    DocumentService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}
