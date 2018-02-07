using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using Eagle.Common.Services.Mail;
using Eagle.Common.Services.Parse;
using Eagle.Common.Utilities;
using Eagle.Core.Common;
using Eagle.Core.Configuration;
using Eagle.Core.Extension;
using Eagle.Core.Pagination;
using Eagle.Core.Permission;
using Eagle.Core.Search;
using Eagle.Core.Settings;
using Eagle.Entities.Contents.Articles;
using Eagle.Entities.Contents.Feedbacks;
using Eagle.Entities.Services.Messaging;
using Eagle.Repositories;
using Eagle.Resources;
using Eagle.Services.Contents.Validation;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Contents.Articles;
using Eagle.Services.Dtos.Contents.Feedbacks;
using Eagle.Services.Dtos.Services.Message;
using Eagle.Services.Dtos.SystemManagement.FileStorage;
using Eagle.Services.EntityMapping.Common;
using Eagle.Services.Exceptions;
using Eagle.Services.Messaging;
using Eagle.Services.SystemManagement;
using Eagle.Services.Validations;

namespace Eagle.Services.Contents
{
    public class NewsService : BaseService, INewsService
    {
        public IApplicationService ApplicationService { get; set; }
        public ICommonService CommonService { get; set; }
        public IDocumentService DocumentService { get; set; }
        public IMailService MailService { get; set; }
        public IMessageService MessageService { get; set; }
        public INotificationService NotificationService { get; set; }
       
        public NewsService(IUnitOfWork unitOfWork, IApplicationService applicationService, ICommonService commonService, IDocumentService documentService, IMessageService messageService,
            INotificationService notificationService, IMailService mailService) : base(unitOfWork)
        {
            ApplicationService = applicationService;
            CommonService = commonService;
            DocumentService = documentService;
            MailService = mailService;
            MessageService = messageService;
            NotificationService = notificationService;
        }

        #region NEWS CATEGORY
        public IEnumerable<TreeGrid> GetNewsCategoryTreeGrid(string languageCode, NewsCategoryStatus? status, out int recordCount, string orderBy = null, int? page = null, int? pageSize = null, int? selectedId = null, bool? isRootShowed = false)
        {
            return UnitOfWork.NewsCategoryRepository.GetNewsCategoryTreeGrid(languageCode, status, 
                out recordCount, orderBy, page, pageSize, selectedId, isRootShowed);
        }
        public SearchDataResult<NewsCategoryDetail> GetNewsCategories(string languageCode, string searchText, NewsCategoryStatus? status, out int recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var list = UnitOfWork.NewsCategoryRepository.GetTreeNodes(languageCode, searchText, null, status, out recordCount, orderBy, page, pageSize);
            return new SearchDataResult<NewsCategoryDetail>
            {
                List = list.ToDtos<NewsCategory, NewsCategoryDetail>(),
                PaginatedList = new PaginatedList
                {
                    PageIndex = page ?? 0,
                    PageSize = pageSize ?? GlobalSettings.DefaultPageSize,
                    TotalItemCount = recordCount
                }
            };
        }

        public SearchDataResult<TreeGrid> GetNewsCategoryList(string languageCode, int? page, int? pageSize)
        {
            int recordCount;
            var list = GetNewsCategoryTreeGrid(languageCode, null, out recordCount, null, page, pageSize).ToList();
            return new SearchDataResult<TreeGrid>
            {
                List = list,
                PaginatedList = new PaginatedList
                {
                    PageIndex = page ?? 0,
                    PageSize = pageSize ?? GlobalSettings.DefaultPageSize,
                    TotalItemCount = recordCount
                }
            };
        }

        public string GenerateCategoryCode(int num)
        {
            return UnitOfWork.NewsCategoryRepository.GenerateCategoryCode(num);
        }

        public NewsCategoryDetail GetNewsCategoryByCode(string categoryCode)
        {
            var entity = UnitOfWork.NewsCategoryRepository.GetDetailsByCode(categoryCode);
            return entity.ToDto<NewsCategory, NewsCategoryDetail>();
        }

        public NewsCategoryDetail GetNewsCategoryDetail(int id)
        {
            var violations = new List<RuleViolation>();
            if (id <= 0)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundCategory, "CategoryId", id, ErrorMessage.Messages[ErrorCode.NotFoundCategory]));
                throw new ValidationError(violations);
            }

            var entity = UnitOfWork.NewsCategoryRepository.FindById(id);
            return entity.ToDto<NewsCategory, NewsCategoryDetail>();
        }

        public IEnumerable<NewsCategoryDetail> GetNewsCategoryParentNodes()
        {
            var lst = UnitOfWork.NewsCategoryRepository.GetParentNodes();
            return lst.ToDtos<NewsCategory, NewsCategoryDetail>();
        }
        public IEnumerable<TreeNodeDetail> GetChildren(int? parentId, string languageCode)
        {
            var recursiveObjects = new List<TreeNodeDetail>();
            if (parentId != null && parentId != 0)
            {
                var list = UnitOfWork.NewsCategoryRepository.GetListByLanguageCode(languageCode).Select(x => new TreeNodeDetail
                {
                    Id = x.CategoryId,
                    Text = x.CategoryName,
                    ParentId = x.ParentId
                });

                recursiveObjects = CommonService.RecursiveFillTreeNodes(list, parentId).ToList();
            }
            return recursiveObjects;
        }
        public IEnumerable<TreeNodeDetail> PopulateHierachicalNewsCategoryDropDownList(string languageCode)
        {
            var list = UnitOfWork.NewsCategoryRepository.GetListByLanguageCode(languageCode).Select(x => new TreeNodeDetail
            {
                Id = x.CategoryId,
                Text = x.CategoryName,
                ParentId = x.ParentId
            }).AsEnumerable();

            return CommonService.RecursiveFillTreeNodes(list, null);
        }
        public IEnumerable<TreeDetail> GetNewsCategoryTree(string languageCode, NewsCategoryStatus? status = null, int? selectedId=null, bool? isRootShowed=true)
        {
            var list = UnitOfWork.NewsCategoryRepository.GetNewsCategories(languageCode, status).Select(p => new TreeDetail
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
                tooltip = p.Description
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

        public void InsertNewsCategory(Guid userId, string languageCode, NewsCategoryEntry entry)
        {
            ISpecification<NewsCategoryEntry> validator = new NewsCategoryEntryValidator(UnitOfWork, PermissionLevel.Create, CurrentClaimsIdentity);
            var dataViolations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isDataValid) throw new ValidationError(dataViolations);

            var entity = entry.ToEntity<NewsCategoryEntry, NewsCategory>();
            entity.CategoryCode = Guid.NewGuid().ToString();
            entity.HasChild = false;
            entity.LanguageCode = languageCode;
            entity.Alias = StringUtils.ConvertTitle2Alias(entry.CategoryName);
            entity.ListOrder = UnitOfWork.NewsRepository.GetNewListOrder();
            entity.CreatedByUserId = userId;
            entity.Ip = NetworkUtils.GetIP4Address();

            UnitOfWork.NewsCategoryRepository.Insert(entity);
            UnitOfWork.SaveChanges();

            if (entry.ParentId != null && entry.ParentId > 0)
            {
                var parentEntity = UnitOfWork.NewsCategoryRepository.FindById(Convert.ToInt32(entry.ParentId));
                if (parentEntity == null) return;

                parentEntity.HasChild = true;
                UnitOfWork.NewsCategoryRepository.Update(parentEntity);

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

            UnitOfWork.NewsCategoryRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateNewsCategory(Guid userId, string languageCode, NewsCategoryEditEntry entry)
        {
            ISpecification<NewsCategoryEditEntry> validator = new NewsCategoryEditEntryValidator(UnitOfWork, PermissionLevel.Edit, CurrentClaimsIdentity);
            var violations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, violations);
            if (!isDataValid) throw new ValidationError(violations);

            var entity = UnitOfWork.NewsCategoryRepository.FindById(entry.CategoryId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundCategory, "CategoryId", entry.CategoryId, ErrorMessage.Messages[ErrorCode.NotFoundCategory]));
                throw new ValidationError(violations);
            }

            if (entity.CategoryName != entry.CategoryName)
            {
                bool isDuplicate = UnitOfWork.NewsCategoryRepository.HasDataExisted(entry.CategoryName, entry.ParentId);
                if (isDuplicate)
                {
                    violations.Add(new RuleViolation(ErrorCode.DuplicateCategoryName, "CategoryName", entry.CategoryName));
                    throw new ValidationError(violations);
                }
            }

            if (entry.ParentId != entity.ParentId && entry.ParentId != entity.CategoryId)
            {
                if (entry.ParentId != null && entry.ParentId > 0)
                {
                    var children = UnitOfWork.NewsCategoryRepository.GetAllChildrenNodesOfSelectedNode(languageCode, null, Convert.ToInt32(entry.CategoryId)).ToList();
                    if (children.Any())
                    {
                        var parentIds = children.Select(x => x.CategoryId).ToList();
                        if (parentIds.Contains(Convert.ToInt32(entry.ParentId)))
                        {
                            violations.Add(new RuleViolation(ErrorCode.InvalidParentId, "ParentId", entry.ParentId, ErrorMessage.Messages[ErrorCode.InvalidParentId]));
                            throw new ValidationError(violations);
                        }
                    }

                    //Update parent entry
                    var parentEntryEntity = UnitOfWork.NewsCategoryRepository.FindById(Convert.ToInt32(entry.ParentId));
                    if (parentEntryEntity == null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.NotFoundParentId, "ParentId"));
                        throw new ValidationError(violations);
                    }
                    else
                    {
                        if (parentEntryEntity.HasChild == null || parentEntryEntity.HasChild == false)
                        {
                            parentEntryEntity.HasChild = true;
                            UnitOfWork.NewsCategoryRepository.Update(parentEntryEntity);
                        }
                    }

                    //Update parent entity
                    var parentEntity = UnitOfWork.NewsCategoryRepository.FindById(entity.ParentId);
                    if (parentEntity != null)
                    {
                        var childList = UnitOfWork.NewsCategoryRepository.GetAllChildrenNodesOfSelectedNode(languageCode, null, entity.ParentId, null).ToList();
                        if (childList.Any())
                        {
                            childList = childList.Where(x => (x.CategoryId != entity.ParentId) && (x.CategoryId != entity.CategoryId)).ToList();
                            parentEntity.HasChild = childList.Any();
                            UnitOfWork.NewsCategoryRepository.Update(parentEntity);
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
            var hasChild = UnitOfWork.NewsCategoryRepository.HasChild(entity.CategoryId);
            entity.CategoryName = entry.CategoryName;
            entity.HasChild = hasChild;
            entity.Alias = StringUtils.GenerateFriendlyString(entry.CategoryName);
            entity.CategoryImage = entry.CategoryImage;
            entity.Description = entry.Description;
            entity.NavigateUrl = entry.NavigateUrl;
            entity.LanguageCode = languageCode;
            entity.Status = entry.Status;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedByUserId = userId;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.NewsCategoryRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateNewsCategoryListOrder(Guid userId, int currentCategoryId, bool isUp)
        {
            var violations = new List<RuleViolation>();
            var currentEntity = UnitOfWork.NewsCategoryRepository.FindById(currentCategoryId);
            if (currentEntity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundForNewsCategory, "NewsCategory", currentCategoryId));
                throw new ValidationError(violations);
            }

            if (isUp)
            {
                var nextEntity = UnitOfWork.NewsCategoryRepository.GetNextCategory(currentCategoryId);
                if (nextEntity == null) return;

                nextEntity.ListOrder = currentCategoryId;
                nextEntity.LastModifiedByUserId = userId;
                nextEntity.LastUpdatedIp = NetworkUtils.GetIP4Address();
                nextEntity.LastModifiedDate = DateTime.UtcNow;
                UnitOfWork.NewsCategoryRepository.Update(nextEntity);

                currentEntity.ListOrder = nextEntity.CategoryId;
                currentEntity.LastModifiedByUserId = userId;
                currentEntity.LastUpdatedIp = NetworkUtils.GetIP4Address();
                currentEntity.LastModifiedDate = DateTime.UtcNow;
                UnitOfWork.NewsCategoryRepository.Update(currentEntity);
            }
            else
            {
                var prevEntity = UnitOfWork.NewsCategoryRepository.GetPreviousCategory(currentCategoryId);
                if (prevEntity == null) return;

                prevEntity.ListOrder = currentCategoryId;
                prevEntity.LastModifiedByUserId = userId;
                prevEntity.LastUpdatedIp = NetworkUtils.GetIP4Address();
                prevEntity.LastModifiedDate = DateTime.UtcNow;
                UnitOfWork.NewsCategoryRepository.Update(prevEntity);

                currentEntity.ListOrder = prevEntity.CategoryId;
                currentEntity.LastUpdatedIp = NetworkUtils.GetIP4Address();
                currentEntity.LastModifiedDate = DateTime.UtcNow;
                currentEntity.LastModifiedByUserId = userId;
                UnitOfWork.NewsCategoryRepository.Update(currentEntity);
            }
            UnitOfWork.SaveChanges();
        }

        public void UpdateNewsCategoryListOrder(Guid userId, NewsCategorySortOrderEntry entry)
        {
            var violations = new List<RuleViolation>();
            var currentEntity = UnitOfWork.NewsCategoryRepository.FindById(entry.CategoryId);
            if (currentEntity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundForNewsCategory, "NewsCategory", entry.CategoryId));
                throw new ValidationError(violations);
            }

            currentEntity.ParentId = entry.ParentId;
            currentEntity.ListOrder = entry.ListOrder;
            currentEntity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            currentEntity.LastModifiedByUserId = userId;
            currentEntity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.NewsCategoryRepository.Update(currentEntity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateNewsCategoryListOrders(Guid userId, NewsCategoryListOrderEntry entry)
        {
            if (entry.ListOrders == null) return;
            foreach (var item in entry.ListOrders)
            {
                UpdateNewsCategoryListOrder(userId, item);
            }
        }

        public void UpdateNewsCategoryStatus(Guid userId, int id, NewsCategoryStatus status)
        {
            var entity = UnitOfWork.NewsCategoryRepository.FindById(id);
            if (entity == null) throw new NotFoundDataException();

            var violations = new List<RuleViolation>();
            var isValid = Enum.IsDefined(typeof(NewsCategoryStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidNewsCategoryStatus, "Status"));
                throw new ValidationError(violations);
            }
            if (entity.Status == status) return;

            entity.Status = status;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedByUserId = userId;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.NewsCategoryRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region NEWS
        public IEnumerable<NewsInfoDetail> Search(NewsSearchEntry searchEntry, out int recordCount, string orderBy = null, int? page = 1, int? pageSize = null)
        {
            var news= UnitOfWork.NewsRepository.Search(out recordCount, searchEntry.Keywords, searchEntry.Authors,
                searchEntry.CategoryId, searchEntry.FromDate, searchEntry.ToDate, searchEntry.Status, orderBy, page, pageSize).ToList();

            if (!news.Any()) return null;

            var lst = news.Select(x => new NewsInfoDetail
            {
                NewsId = x.NewsId,
                Title = x.Title,
                Headline = x.Headline,
                Alias = x.Alias,
                Summary = x.Summary,
                FrontImage = x.FrontImage,
                MainImage = x.MainImage,
                MainText = !string.IsNullOrEmpty(x.MainText)? HttpUtility.HtmlDecode(x.MainText):string.Empty,
                NavigateUrl = x.NavigateUrl,
                Authors = x.Authors,
                ListOrder = x.ListOrder,
                Tags = x.Tags,
                Source = x.Source,
                TotalRates = x.TotalRates,
                TotalViews = x.TotalViews,
                PostedDate = x.PostedDate,
                Status = x.Status,
                CreatedDate = x.CreatedDate,
                LastModifiedDate = x.LastModifiedDate,
                CreatedByUserId = x.CreatedByUserId,
                LastModifiedByUserId = x.LastModifiedByUserId,
                Ip = x.Ip,
                LastUpdatedIp = x.LastUpdatedIp,
                CategoryId = x.CategoryId,
                VendorId = x.CategoryId,
                CategoryName = x.CategoryName,
                FullName = x.FullName,
                FrontImageUrl = (x.FrontImage != null && x.FrontImage > 0) ? DocumentService.GetFileInfoDetail(Convert.ToInt32(x.FrontImage)).FileUrl : GlobalSettings.NotFoundFileUrl,
                MainImageUrl = (x.MainImage != null && x.FrontImage > 0) ? DocumentService.GetFileInfoDetail(Convert.ToInt32(x.MainImage)).FileUrl : GlobalSettings.NotFoundFileUrl,
                Category = new NewsCategoryDetail
                {
                    CategoryId = x.Category.CategoryId,
                    CategoryCode = x.Category.CategoryCode,
                    CategoryName = x.Category.CategoryName,
                    Alias = x.Category.Alias,
                    ParentId = x.Category.ParentId,
                    Depth = x.Category.Depth,
                    Lineage = x.Category.Lineage,
                    HasChild = x.Category.HasChild,
                    CategoryImage = x.Category.CategoryImage,
                    Description = x.Category.Description,
                    NavigateUrl = x.Category.NavigateUrl,
                    ListOrder = x.Category.ListOrder,
                    Status = x.Category.Status,
                    LanguageCode = x.Category.LanguageCode
                }, 
                Comments = GetNewsComments(x.NewsId, NewsCommentStatus.Active).ToList()
            }).ToList();
            return lst;
        }
        public IEnumerable<NewsInfoDetail> GetNewsList(string searchText, NewsStatus? status, out int recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var news = UnitOfWork.NewsRepository.GetList(searchText, status, out recordCount, orderBy, page, pageSize);
            var lst = news.Select(x => new NewsInfoDetail
            {
                NewsId = x.NewsId,
                Title = x.Title,
                Headline = x.Headline,
                Alias = x.Alias,
                Summary = x.Summary,
                FrontImage = x.FrontImage,
                MainImage = x.MainImage,
                MainText = !string.IsNullOrEmpty(x.MainText) ? HttpUtility.HtmlDecode(x.MainText) : string.Empty,
                NavigateUrl = x.NavigateUrl,
                Authors = x.Authors,
                ListOrder = x.ListOrder,
                Tags = x.Tags,
                Source = x.Source,
                TotalRates = x.TotalRates,
                TotalViews = x.TotalViews,
                PostedDate = x.PostedDate,
                Status = x.Status,
                CreatedDate = x.CreatedDate,
                LastModifiedDate = x.LastModifiedDate,
                CreatedByUserId = x.CreatedByUserId,
                LastModifiedByUserId = x.LastModifiedByUserId,
                Ip = x.Ip,
                LastUpdatedIp = x.LastUpdatedIp,
                CategoryId = x.CategoryId,
                CategoryName = x.CategoryName,
                FullName = x.FullName,
                FrontImageUrl = (x.FrontImage != null && x.FrontImage > 0) ? DocumentService.GetFileInfoDetail(Convert.ToInt32(x.FrontImage)).FileUrl : GlobalSettings.NotFoundFileUrl,
                MainImageUrl = (x.MainImage != null && x.FrontImage > 0) ? DocumentService.GetFileInfoDetail(Convert.ToInt32(x.MainImage)).FileUrl : GlobalSettings.NotFoundFileUrl,
                Category = new NewsCategoryDetail
                {
                    CategoryId = x.Category.CategoryId,
                    CategoryCode = x.Category.CategoryCode,
                    CategoryName = x.Category.CategoryName,
                    Alias = x.Category.Alias,
                    ParentId = x.Category.ParentId,
                    Depth = x.Category.Depth,
                    Lineage = x.Category.Lineage,
                    HasChild = x.Category.HasChild,
                    CategoryImage = x.Category.CategoryImage,
                    Description = x.Category.Description,
                    NavigateUrl = x.Category.NavigateUrl,
                    ListOrder = x.Category.ListOrder,
                    Status = x.Category.Status,
                    LanguageCode = x.Category.LanguageCode
                },
                Comments = GetNewsComments(x.NewsId, NewsCommentStatus.Active).ToList()
            }).ToList();
            return lst;
        }
        public IEnumerable<NewsInfoDetail> GetNewsByCategoryId(int categoryId, NewsStatus? status, out int recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var news = UnitOfWork.NewsRepository.GetListByCategoryId(categoryId, status, out recordCount, orderBy, page, pageSize);
            var lst = news.Select(x => new NewsInfoDetail
            {
                NewsId = x.NewsId,
                Title = x.Title,
                Headline = x.Headline,
                Alias = x.Alias,
                Summary = x.Summary,
                FrontImage = x.FrontImage,
                MainImage = x.MainImage,
                MainText = !string.IsNullOrEmpty(x.MainText) ? HttpUtility.HtmlDecode(x.MainText) : string.Empty,
                NavigateUrl = x.NavigateUrl,
                Authors = x.Authors,
                ListOrder = x.ListOrder,
                Tags = x.Tags,
                Source = x.Source,
                TotalRates = x.TotalRates,
                TotalViews = x.TotalViews,
                PostedDate = x.PostedDate,
                Status = x.Status,
                CreatedDate = x.CreatedDate,
                LastModifiedDate = x.LastModifiedDate,
                CreatedByUserId = x.CreatedByUserId,
                LastModifiedByUserId = x.LastModifiedByUserId,
                Ip = x.Ip,
                LastUpdatedIp = x.LastUpdatedIp,
                CategoryId = x.CategoryId,
                CategoryName = x.CategoryName,
                FullName = x.FullName,
                FrontImageUrl = (x.FrontImage != null && x.FrontImage > 0) ? DocumentService.GetFileInfoDetail(Convert.ToInt32(x.FrontImage)).FileUrl : GlobalSettings.NotFoundFileUrl,
                MainImageUrl = (x.MainImage != null && x.FrontImage > 0) ? DocumentService.GetFileInfoDetail(Convert.ToInt32(x.MainImage)).FileUrl : GlobalSettings.NotFoundFileUrl,
                Category = new NewsCategoryDetail
                {
                    CategoryId = x.Category.CategoryId,
                    CategoryCode = x.Category.CategoryCode,
                    CategoryName = x.Category.CategoryName,
                    Alias = x.Category.Alias,
                    ParentId = x.Category.ParentId,
                    Depth = x.Category.Depth,
                    Lineage = x.Category.Lineage,
                    HasChild = x.Category.HasChild,
                    CategoryImage = x.Category.CategoryImage,
                    Description = x.Category.Description,
                    NavigateUrl = x.Category.NavigateUrl,
                    ListOrder = x.Category.ListOrder,
                    Status = x.Category.Status,
                    LanguageCode = x.Category.LanguageCode
                },
                Comments = GetNewsComments(x.NewsId, NewsCommentStatus.Active).ToList()
            }).ToList();
            return lst;
        }
        public IEnumerable<NewsInfoDetail> GetNews(int number)
        {
            var news = UnitOfWork.NewsRepository.GetNews(number);
            var lst = news.Select(x => new NewsInfoDetail
            {
                NewsId = x.NewsId,
                Title = x.Title,
                Headline = x.Headline,
                Alias = x.Alias,
                Summary = x.Summary,
                FrontImage = x.FrontImage,
                MainImage = x.MainImage,
                MainText = !string.IsNullOrEmpty(x.MainText) ? HttpUtility.HtmlDecode(x.MainText) : string.Empty,
                NavigateUrl = x.NavigateUrl,
                Authors = x.Authors,
                ListOrder = x.ListOrder,
                Tags = x.Tags,
                Source = x.Source,
                TotalRates = x.TotalRates,
                TotalViews = x.TotalViews,
                PostedDate = x.PostedDate,
                Status = x.Status,
                CreatedDate = x.CreatedDate,
                LastModifiedDate = x.LastModifiedDate,
                CreatedByUserId = x.CreatedByUserId,
                LastModifiedByUserId = x.LastModifiedByUserId,
                Ip = x.Ip,
                LastUpdatedIp = x.LastUpdatedIp,
                CategoryId = x.CategoryId,
                CategoryName = x.CategoryName,
                FullName = x.FullName,
                FrontImageUrl = (x.FrontImage != null && x.FrontImage > 0) ? DocumentService.GetFileInfoDetail(Convert.ToInt32(x.FrontImage)).FileUrl : GlobalSettings.NotFoundFileUrl,
                MainImageUrl = (x.MainImage != null && x.FrontImage > 0) ? DocumentService.GetFileInfoDetail(Convert.ToInt32(x.MainImage)).FileUrl : GlobalSettings.NotFoundFileUrl,
                Category = new NewsCategoryDetail
                {
                    CategoryId = x.Category.CategoryId,
                    CategoryCode = x.Category.CategoryCode,
                    CategoryName = x.Category.CategoryName,
                    Alias = x.Category.Alias,
                    ParentId = x.Category.ParentId,
                    Depth = x.Category.Depth,
                    Lineage = x.Category.Lineage,
                    HasChild = x.Category.HasChild,
                    CategoryImage = x.Category.CategoryImage,
                    Description = x.Category.Description,
                    NavigateUrl = x.Category.NavigateUrl,
                    ListOrder = x.Category.ListOrder,
                    Status = x.Category.Status,
                    LanguageCode = x.Category.LanguageCode
                },
                Comments = GetNewsComments(x.NewsId, NewsCommentStatus.Active).ToList()
            }).ToList();
            return lst;
        }
        public IEnumerable<NewsInfoDetail> GetListByTotalViews(int recordCount)
        {
            var news = UnitOfWork.NewsRepository.GetListByTotalViews(recordCount);
            var lst = news.Select(x => new NewsInfoDetail
            {
                NewsId = x.NewsId,
                Title = x.Title,
                Headline = x.Headline,
                Alias = x.Alias,
                Summary = x.Summary,
                FrontImage = x.FrontImage,
                MainImage = x.MainImage,
                MainText = !string.IsNullOrEmpty(x.MainText) ? HttpUtility.HtmlDecode(x.MainText) : string.Empty,
                NavigateUrl = x.NavigateUrl,
                Authors = x.Authors,
                ListOrder = x.ListOrder,
                Tags = x.Tags,
                Source = x.Source,
                TotalRates = x.TotalRates,
                TotalViews = x.TotalViews,
                PostedDate = x.PostedDate,
                Status = x.Status,
                CreatedDate = x.CreatedDate,
                LastModifiedDate = x.LastModifiedDate,
                CreatedByUserId = x.CreatedByUserId,
                LastModifiedByUserId = x.LastModifiedByUserId,
                Ip = x.Ip,
                LastUpdatedIp = x.LastUpdatedIp,
                CategoryId = x.CategoryId,
                CategoryName = x.CategoryName,
                FullName = x.FullName,
                FrontImageUrl = (x.FrontImage != null && x.FrontImage > 0) ? DocumentService.GetFileInfoDetail(Convert.ToInt32(x.FrontImage)).FileUrl : GlobalSettings.NotFoundFileUrl,
                MainImageUrl = (x.MainImage != null && x.FrontImage > 0) ? DocumentService.GetFileInfoDetail(Convert.ToInt32(x.MainImage)).FileUrl : GlobalSettings.NotFoundFileUrl,
                Category = new NewsCategoryDetail
                {
                    CategoryId = x.Category.CategoryId,
                    CategoryCode = x.Category.CategoryCode,
                    CategoryName = x.Category.CategoryName,
                    Alias = x.Category.Alias,
                    ParentId = x.Category.ParentId,
                    Depth = x.Category.Depth,
                    Lineage = x.Category.Lineage,
                    HasChild = x.Category.HasChild,
                    CategoryImage = x.Category.CategoryImage,
                    Description = x.Category.Description,
                    NavigateUrl = x.Category.NavigateUrl,
                    ListOrder = x.Category.ListOrder,
                    Status = x.Category.Status,
                    LanguageCode = x.Category.LanguageCode
                },
                Comments = GetNewsComments(x.NewsId, NewsCommentStatus.Active).ToList()
            }).ToList();
            return lst;
        }
        public NewsInfoDetail GeNewsDetail(int id)
        {
            var item = UnitOfWork.NewsRepository.GetDetails(id);
            var model = new NewsInfoDetail
            {
                NewsId = item.NewsId,
                Title = item.Title,
                Headline = item.Headline,
                Alias = item.Alias,
                Summary = item.Summary,
                FrontImage = item.FrontImage,
                MainImage = item.MainImage,
                MainText = !string.IsNullOrEmpty(item.MainText) ? HttpUtility.HtmlDecode(item.MainText) : string.Empty,
                NavigateUrl = item.NavigateUrl,
                Authors = item.Authors,
                ListOrder = item.ListOrder,
                Tags = item.Tags,
                Source = item.Source,
                TotalRates = item.TotalRates,
                TotalViews = item.TotalViews,
                PostedDate = item.PostedDate,
                Status = item.Status,
                CreatedDate = item.CreatedDate,
                LastModifiedDate = item.LastModifiedDate,
                CreatedByUserId = item.CreatedByUserId,
                LastModifiedByUserId = item.LastModifiedByUserId,
                Ip = item.Ip,
                LastUpdatedIp = item.LastUpdatedIp,
                CategoryId = item.CategoryId,
                CategoryName = item.CategoryName,
                FullName = item.FullName,
                FrontImageUrl = (item.FrontImage != null && item.FrontImage > 0) ? DocumentService.GetFileInfoDetail(Convert.ToInt32(item.FrontImage)).FileUrl : GlobalSettings.NotFoundFileUrl,
                MainImageUrl = (item.MainImage != null && item.FrontImage > 0) ? DocumentService.GetFileInfoDetail(Convert.ToInt32(item.MainImage)).FileUrl : GlobalSettings.NotFoundFileUrl,
                Category = new NewsCategoryDetail
                {
                    CategoryId = item.Category.CategoryId,
                    CategoryCode = item.Category.CategoryCode,
                    CategoryName = item.Category.CategoryName,
                    Alias = item.Category.Alias,
                    ParentId = item.Category.ParentId,
                    Depth = item.Category.Depth,
                    Lineage = item.Category.Lineage,
                    HasChild = item.Category.HasChild,
                    CategoryImage = item.Category.CategoryImage,
                    Description = item.Category.Description,
                    NavigateUrl = item.Category.NavigateUrl,
                    ListOrder = item.Category.ListOrder,
                    Status = item.Category.Status,
                    LanguageCode = item.Category.LanguageCode
                },
                Comments = GetNewsComments(item.NewsId, NewsCommentStatus.Active).ToList()
            };
            
            var documentInfos = new List<DocumentInfoDetail>();
            if (model.FrontImage != null && model.FrontImage > 0)
            {
                var frontImageInfo = DocumentService.GetFileInfoDetail(Convert.ToInt32(model.FrontImage));
                documentInfos.Add(frontImageInfo);
            }
            if (model.MainImage != null && model.MainImage > 0)
            {
                var mainImageInfo = DocumentService.GetFileInfoDetail(Convert.ToInt32(model.MainImage));
                documentInfos.Add(mainImageInfo);
            }
            model.DocumentInfos = documentInfos;
            return model;
        }
        public NewsDetail InsertNews(Guid applicationId, Guid userId, int? vendorId, NewsEntry entry)
        {
            ISpecification<NewsEntry> validator = new NewsEntryValidator(UnitOfWork, PermissionLevel.Create, CurrentClaimsIdentity);
            var dataViolations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isDataValid) throw new ValidationError(dataViolations);

            int listOrder = UnitOfWork.NewsRepository.GetNewListOrder();
            var entity = entry.ToEntity<NewsEntry, News>();
            entity.Alias = StringUtils.GenerateFriendlyString(entity.Headline);
            entity.MainText = StringUtils.UTF8_Encode(entry.MainText);
            entity.Tags = StringUtils.CreateTags(entry.Headline);
            entity.PostedDate = DateTime.UtcNow;
            entity.Ip = NetworkUtils.GetIP4Address();
            entity.ListOrder = listOrder;
            entity.Status = NewsStatus.Published;
            entity.CreatedByUserId = userId;
            entity.VendorId = vendorId;

            UnitOfWork.NewsRepository.Insert(entity);
            UnitOfWork.SaveChanges();

            if (entry.File != null && entry.File.ContentLength > 0)
            {
                var fileIds = DocumentService.UploadAndSaveWithThumbnail(applicationId, userId, entry.File, (int)FileLocation.News, StorageType.Local);
                entity.MainImage = fileIds[0].FileId;
                entity.FrontImage = fileIds[1].FileId;
            }

            UnitOfWork.NewsRepository.Update(entity);
            UnitOfWork.SaveChanges();
            return entity.ToDto<News, NewsDetail>();
        }
        public List<RuleViolation> ValidateEntry(NewsEntry entry)
        {
            var violations = new List<RuleViolation>();
         
            //Check Title
            if (string.IsNullOrEmpty(entry.Title))
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidTitle, "Title"));
            }
            else
            {
                if (entry.Title.Length > 300)
                {
                    violations?.Add(new RuleViolation(ErrorCode.InvalidTitle, "Title"));
                }
            }

            //Check Headline
            if (!string.IsNullOrEmpty(entry.Headline) && entry.Headline.Length > 300)
            {
                violations.Add(new RuleViolation(ErrorCode.InvaliHeadline, "Headline"));
            }

            if (entry.CategoryId <= 0)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidCategoryId, "CategoryId"));
            }

            return violations;
        }
        public void UpdateNews(Guid applicationId, Guid userId, int id, NewsEntry entry)
        {
            var entity = UnitOfWork.NewsRepository.FindById(id);
            if (entity == null) return;

            var dataViolations = ValidateEntry(entry);
            if (dataViolations.Any()) throw new ValidationError(dataViolations);

            entity.CategoryId = entry.CategoryId;
            entity.Title = entry.Title;
            entity.Headline = entry.Headline;
            entity.Alias = StringUtils.GenerateFriendlyString(entry.Headline);
            entity.Authors = entry.Authors;
            entity.NavigateUrl = entry.NavigateUrl;
            entity.Source = entry.Source;
            entity.Summary = entry.Summary;
            entity.MainText = StringUtils.UTF8_Encode(entry.MainText);
            entity.Tags = StringUtils.CreateTags(entry.Headline);
            entity.PostedDate = entry.PostedDate;
            entity.LastModifiedDate = DateTime.UtcNow;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedByUserId = userId;

            UnitOfWork.NewsRepository.Update(entity);
            UnitOfWork.SaveChanges();

            if (entry.File != null && entry.File.ContentLength > 0)
            {
                if (entity.FrontImage != null)
                {
                    DocumentService.DeleteFile(Convert.ToInt32(entity.FrontImage));
                }

                if (entity.MainImage != null)
                {
                    DocumentService.DeleteFile(Convert.ToInt32(entity.MainImage));
                }

                var fileIds = DocumentService.UploadAndSaveWithThumbnail(applicationId, userId, entry.File, (int)FileLocation.News, StorageType.Local);
                entity.MainImage = fileIds[0].FileId;
                entity.FrontImage = fileIds[1].FileId;
            }

            UnitOfWork.NewsRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateNewsListOrder(Guid userId, NewsSortOrderEntry entry)
        {
            var entity = UnitOfWork.NewsRepository.FindById(entry.NewsId);
            if (entity == null) return;

            entity.ListOrder = entry.ListOrder;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedByUserId = userId;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.NewsRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateNewsListOrders(Guid userId, NewsListOrderEntry entry)
        {
            if (entry.ListOrders == null) return;
            foreach (var item in entry.ListOrders)
            {
                UpdateNewsListOrder(userId, item);
            }
            UnitOfWork.SaveChanges();
        }
        public void UpdateNewsTotalViews(int id)
        {
            var entity = UnitOfWork.NewsRepository.FindById(id);
            if (entity == null) return;

            if (entity.TotalViews == null) entity.TotalViews = 0;

            entity.TotalViews = entity.TotalViews + 1;

            UnitOfWork.NewsRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateNewsTotalView(Guid userId, int id)
        {
            var entity = UnitOfWork.NewsRepository.FindById(id);
            if (entity == null) return;

            entity.TotalViews = entity.TotalViews + 1;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedByUserId = userId;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.NewsRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateNewsTotalView(Guid userId, int id, int totalview)
        {
            var entity = UnitOfWork.NewsRepository.FindById(id);
            if (entity == null) return;

            entity.TotalViews = totalview;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedByUserId = userId;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.NewsRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateNewsStatus(Guid userId, int id, NewsStatus status)
        {
            var violations = new List<RuleViolation>();
            var isValid = Enum.IsDefined(typeof(NewsStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidNewsStatus, "Status", status));
                throw new ValidationError(violations);
            }

            var entity = UnitOfWork.NewsRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundForNews, "News", id));
                throw new ValidationError(violations);
            }
            if (entity.Status == status) return;

            entity.Status = status;
            entity.LastModifiedDate = DateTime.UtcNow;
            entity.LastModifiedByUserId = userId;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            UnitOfWork.NewsRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void DeleteNews(int id)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.NewsRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundForNews, "News", id));
                throw new ValidationError(violations);
            }

            UnitOfWork.NewsRepository.Delete(entity);
            UnitOfWork.SaveChanges();

            if (entity.FrontImage != null)
            {
                DocumentService.DeleteFile(Convert.ToInt32(entity.FrontImage));
            }

            if (entity.MainImage != null)
            {
                DocumentService.DeleteFile(Convert.ToInt32(entity.MainImage));
            }
        }

        #endregion

        #region News Comment
        public IEnumerable<NewsCommentInfoDetail> GetNewsComments(NewsCommentStatus? status, ref int? recordCount,
           string orderBy = null, int? page = null, int? pageSize = null)
        {
            var comments = UnitOfWork.NewsCommentRepository.GetNewsComments(status, ref recordCount, orderBy, page,
                pageSize);
            var lst = comments.Select(x => new NewsCommentInfoDetail
            {
                NewsId = x.NewsId,
                CommentId = x.CommentId,
                CommentName = x.CommentName,
                CommentText = x.CommentText,
                CreatedByEmail = x.CreatedByEmail,
                IsReplied = x.IsReplied,
                IsPublished = x.IsPublished,
                Ip = x.Ip,
                CreatedDate = x.CreatedDate,
                News = x.News.ToDto<News, NewsDetail>()
            });
            return lst;
        }

        public IEnumerable<NewsCommentDetail> GetNewsComments(int newsId, NewsCommentStatus? status, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = UnitOfWork.NewsCommentRepository.GetNewsComments(newsId, status, ref recordCount, orderBy, page,
                pageSize);
            return lst.ToDtos<NewsComment, NewsCommentDetail>();
        }

        public IEnumerable<NewsCommentDetail> GetNewsComments(int newsId, NewsCommentStatus? status)
        {
            var lst = UnitOfWork.NewsCommentRepository.GetNewsComments(newsId, status);
            return lst.ToDtos<NewsComment, NewsCommentDetail>();
        }

        public NewsCommentDetail GetNewsCommentDetail(int id)
        {
            var entity = UnitOfWork.NewsCommentRepository.FindById(id);
            return entity.ToDto<NewsComment, NewsCommentDetail>();
        }

        public void InsertNewsComment(NewsCommentEntry entry)
        {
            ISpecification<NewsCommentEntry> validator = new NewsCommentEntryValidator(UnitOfWork);
            var dataViolations = new List<RuleViolation>();
            var isValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isValid) throw new ValidationError(dataViolations);

            var entity = entry.ToEntity<NewsCommentEntry, NewsComment>();
            entity.Ip = NetworkUtils.GetIP4Address();
            entity.CreatedDate = DateTime.UtcNow;

            UnitOfWork.NewsCommentRepository.Insert(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateNewsComment(int id, NewsCommentEntry entry)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.NewsCommentRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundForNewsComment, "NewsComment", id));
                throw new ValidationError(violations);
            }

            ISpecification<NewsCommentEntry> validator = new NewsCommentEntryValidator(UnitOfWork);
            var dataViolations = new List<RuleViolation>();
            var isValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isValid) throw new ValidationError(dataViolations);

            entity.NewsId = entry.NewsId;
            entity.IsReplied = entry.IsReplied;
            entity.IsPublished = entry.IsPublished;
            entity.CommentName = entry.CommentName;
            entity.CreatedByEmail = entry.CreatedByEmail;

            UnitOfWork.NewsCommentRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateNewsCommentStatus(int id, NewsCommentStatus status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.NewsCommentRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundForNewsComment, "NewsComment", id));
                throw new ValidationError(violations);
            }

            var isValid = Enum.IsDefined(typeof(NewsCommentStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status", status, ErrorMessage.Messages[ErrorCode.InvalidStatus]));
                throw new ValidationError(violations);
            }
            if (entity.IsPublished == status) return;

            entity.IsPublished = status;
            UnitOfWork.NewsCommentRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void DeleteNewsComment(int id)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.NewsCommentRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundForNewsComment, "NewsComment", id));
                throw new ValidationError(violations);
            }

            UnitOfWork.NewsCommentRepository.Delete(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region News Rating
        public int GetDefaultNewsRating(Guid applicationId)
        {
            var rateSetting = ApplicationService.GetRateSetting(applicationId, RatingSetting.News);
            int ratings = Convert.ToInt32(rateSetting.Setting.KeyValue);
            return ratings;
        }
        public IEnumerable<NewsRatingDetail> GetNewsRatings(int newsId)
        {
            var lst = UnitOfWork.NewsRatingRepository.GetNewsRatings(newsId);
            return lst.ToDtos<NewsRating, NewsRatingDetail>();
        }

        public decimal InsertNewsRating(NewsRatingEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry.NewsId <= 0)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidNewsId, "NewsId", entry.NewsId));
                throw new ValidationError(violations);
            }
       
            if (entry.Rate <= 0)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidRate, "Rate", entry.Rate));
                throw new ValidationError(violations);
            }

            decimal averageRates = 0;
            var clientIp = NetworkUtils.GetIP4Address();

            var newsEntity = UnitOfWork.NewsRepository.FindById(entry.NewsId);
            if (newsEntity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullNewsId, "NewsId", entry.NewsId));
                throw new ValidationError(violations);
            }

            var rating = entry.ToEntity<NewsRatingEntry, NewsRating>();
            rating.CreatedDate = DateTime.UtcNow;
            rating.Ip = clientIp;

            var ratings = UnitOfWork.NewsRatingRepository.GetNewsRatings(entry.NewsId).ToList();
            if (!ratings.Any())
            {
                UnitOfWork.NewsRatingRepository.Insert(rating);
                UnitOfWork.SaveChanges();
            }
            else
            {
                var existedRating = ratings.FirstOrDefault(x => x.NewsId == entry.NewsId && x.Ip == clientIp);
                if (existedRating == null)
                {
                    UnitOfWork.NewsRatingRepository.Insert(rating);
                    UnitOfWork.SaveChanges();
                }
                else
                {
                    existedRating.Rate = entry.Rate;
                    existedRating.LastModifiedDate = DateTime.UtcNow;
                    UnitOfWork.NewsRatingRepository.Update(existedRating);
                    UnitOfWork.SaveChanges();
                }
            }

            ratings = UnitOfWork.NewsRatingRepository.GetNewsRatings(entry.NewsId).ToList();
            var rateSum = ratings.Sum(d => d.Rate);
            averageRates = rateSum / ratings.Count;

            newsEntity.TotalRates = averageRates;
            UnitOfWork.NewsRepository.Update(newsEntity);
            UnitOfWork.SaveChanges();


            return averageRates;
        }
        #endregion

        #region Feedback
        public IEnumerable<FeedbackDetail> GetFeedbacks(bool? status, ref int? recordCount,
           string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = UnitOfWork.FeedbackRepository.GetFeedbacks(ref recordCount, status, orderBy, page,
                pageSize);
            return lst.ToDtos<Feedback, FeedbackDetail>();
        }
        public void InsertFeedback(Guid applicationId, FeedbackEntry entry)
        {
            ISpecification<FeedbackEntry> validator = new FeedbackEntryValidator(UnitOfWork);
            var dataViolations = new List<RuleViolation>(); 
            var isValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isValid) throw new ValidationError(dataViolations);

            var entity = entry.ToEntity<FeedbackEntry, Feedback>();
            entity.CreatedDate = DateTime.UtcNow;
            UnitOfWork.FeedbackRepository.Insert(entity);
            UnitOfWork.SaveChanges();

            if (entity.FeedbackId > 0)
            {
                SendMailNotification(applicationId, NotificationTypeSetting.CreateFeedback, entity.FeedbackId);
            }
        }
        public void UpdateFeedbackStatus(int id, bool status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.FeedbackRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundFeedback, "Feedback", id, $"{LanguageResource.NotFoundFeedback} with ID : {id}"));
                throw new ValidationError(violations);
            }
           
            if (entity.IsActive == status) return;

            entity.IsActive = status;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.FeedbackRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region Contact us - Feedback Notification

        public void SendMailNotification(Guid applicationId, NotificationTypeSetting notificationTypeSetting, int feedbackId, DateTime? predefinedDate = null, string targetId = null, string bcc = null, string cc = null)
        {
            var violations = new List<RuleViolation>();
            int messageTypeId = Convert.ToInt32(MessageTypeSetting.Email);
            int notificationTypeId = Convert.ToInt32(notificationTypeSetting);

            var notificationType = UnitOfWork.NotificationTypeRepository.FindById(notificationTypeId);
            if (notificationType == null) return;

            //Get Template 
            var template = MessageService.GetMessageTemplateDetail(notificationTypeId, messageTypeId);
            if (template == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundForMessageTemplate, "MessageTemplate", null, ErrorMessage.Messages[ErrorCode.NotFoundForMessageTemplate]));
                throw new ValidationError(violations);
            }

            //Get Mail Settings
            var mailSettings = MailService.GetDefaultSmtpInfo(applicationId);
            if (mailSettings == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundNotificationSender, "NotificationSender", null, ErrorMessage.Messages[ErrorCode.NotFoundNotificationSender]));
                throw new ValidationError(violations);
            }

            //Get Sender Information 
            var sender = NotificationService.GetNotificationSenderDetail(notificationType.NotificationSenderTypeId);

            //Get Receivers
            var receivers = NotificationService.GetNotificationTargets(notificationTypeId, targetId);
            if (receivers.Any())
            {
                var eventInfo = UnitOfWork.FeedbackRepository.FindById(feedbackId);

                //string receiverMails = string.Join(",", receivers.Select(x => x.MailAddress).ToArray());
                string subject = template.TemplateSubject;

                foreach (var receiver in receivers)
                {
                    var templateVariables = new Hashtable
                    {
                        //Bind Feedback
                        { "FeedbackId", eventInfo.FeedbackId },
                        { "SenderName", eventInfo.SenderName },
                        { "Email",eventInfo.Email },
                        { "Mobile", eventInfo.Mobile },
                        { "Subject", eventInfo.Subject },
                        { "Body", eventInfo.Body },
                        { "IsReplied", eventInfo.IsReplied },
                        { "IsActive", eventInfo.IsActive },
                        { "CreatedDate", eventInfo.CreatedDate },
                        { "LastModifiedDate", eventInfo.LastModifiedDate },
                    };

                    string body = ParseTemplateHandler.ParseTemplate(templateVariables, template.TemplateBody);

                    //Create message queue
                    var messageQueueEntry = new MessageQueueEntry
                    {
                        From = sender.MailAddress,
                        To = receiver.MailAddress,
                        Subject = subject,
                        Body = body,
                        Bcc = bcc,
                        Cc = cc,
                        PredefinedDate = predefinedDate
                    };
                    var messageQueue = MessageService.CreateMessageQueue(messageQueueEntry);

                    //Create notification message
                    var extraInfo = (from string key in templateVariables.Keys
                                     select new SerializableKeyValuePair<string, string>
                                     {
                                         Key = key,
                                         Value = Convert.ToString(templateVariables[key])
                                     }).ToList();

                    var messsageInfo = new MessageDetail
                    {
                        MessageTypeId = MessageTypeSetting.Email,
                        NotificationTypeId = notificationTypeSetting,
                        TemplateId = template.TemplateId,
                        WebsiteUrl = string.Empty,
                        WebsiteUrlBase = string.Empty,
                        ExtraInfo = extraInfo,
                        Version = "1.0"
                    };

                    var notificationMessageEntry = new NotificationMessageEntry
                    {
                        Message = messsageInfo,
                        PublishDate = predefinedDate ?? DateTime.UtcNow,
                        SentStatus = NotificationSentStatus.Ready
                    };
                    var notificationMessage = NotificationService.InsertNotificationMessage(notificationMessageEntry);

                    //Send Mail Manually
                    string result;
                    var isEmailSent = MailHandler.SendMail(mailSettings.SmtpmEmail, receiver.MailAddress, sender.SenderName, receiver.TargetName, cc, bcc, null, MailPriority.Normal, subject, true, Encoding.UTF8, body, null,
                        mailSettings.SmtpServer, SmtpAuthentication.Basic, mailSettings.SmtpUsername, mailSettings.SmtpPassword, mailSettings.EnableSsl, out result);

                    //Update status in message queue
                    var messageQueueEditEntry = new MessageQueueEditEntry
                    {
                        QueueId = messageQueue.QueueId,
                        From = messageQueue.From,
                        To = messageQueue.To,
                        Subject = messageQueue.Subject,
                        Body = messageQueue.Body,
                        Bcc = messageQueue.Bcc,
                        Cc = messageQueue.Cc,
                        Status = isEmailSent,
                        ResponseStatus = isEmailSent ? 1 : 0,
                        ResponseMessage = result,
                        SentDate = DateTime.UtcNow
                    };
                    MessageService.UpdateMessageQueue(messageQueueEditEntry);


                    //Update status in notification message
                    var sentStatus = isEmailSent ? NotificationSentStatus.Sent : NotificationSentStatus.Failed;
                    NotificationService.UpdateNotificationMessageStatus(notificationMessage.NotificationMessageId,
                        messsageInfo, sentStatus);
                }
            }
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
                    ApplicationService = null;
                    CommonService = null;
                    DocumentService = null;
                    MailService = null;
                    MessageService = null;
                    NotificationService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}
