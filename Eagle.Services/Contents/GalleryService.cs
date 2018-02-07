using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Common.Utilities;
using Eagle.Core.Extension;
using Eagle.Core.Permission;
using Eagle.Core.Settings;
using Eagle.Entities.Common;
using Eagle.Entities.Contents.Galleries;
using Eagle.Repositories;
using Eagle.Services.Contents.Validation;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Contents.Galleries;
using Eagle.Services.EntityMapping.Common;
using Eagle.Services.Exceptions;
using Eagle.Services.SystemManagement;
using Eagle.Services.Validations;

namespace Eagle.Services.Contents
{
    public class GalleryService : BaseService, IGalleryService
    {
        private IDocumentService DocumentService { get; set; }

        public GalleryService(IUnitOfWork unitOfWork, IDocumentService documentService) : base(unitOfWork)
        {
            DocumentService = documentService;
        }

        #region Gallery Topic

        public IEnumerable<TreeNode> GetGalleryTopicTreeNode(GalleryTopicStatus? status, int? selectedId,
            bool? isRootShowed = false)
        {
            return UnitOfWork.GalleryTopicRepository.GetGalleryTopicTreeNode(status, selectedId, isRootShowed);
        }

        public IEnumerable<TreeGrid> GetGalleryTopicTreeGrid(GalleryTopicStatus? status, int? selectedId, bool? isRootShowed)
        {
            return UnitOfWork.GalleryTopicRepository.GetGalleryTopicTreeGrid(status, selectedId, isRootShowed);
        }

        public IEnumerable<TreeDetail> GetGalleryTopicSelectTree(GalleryTopicStatus? status, int? selectedId, bool? isRootShowed = false)
        {
            var lst = UnitOfWork.GalleryTopicRepository.GetGalleryTopicSelectTree(status, selectedId, isRootShowed);
            return lst.ToDtos<TreeEntity, TreeDetail>();
        }

        public GalleryTopicDetail GetGalleryTopicDetail(int id)
        {
            var entity = UnitOfWork.GalleryTopicRepository.FindById(id);
            return entity.ToDto<GalleryTopic, GalleryTopicDetail>();
        }

        public void InsertGalleryTopic(Guid userId, GalleryTopicEntry entry)
        {
            ISpecification<GalleryTopicEntry> validator = new GalleryTopicEntryValidator(UnitOfWork, PermissionLevel.Create, CurrentClaimsIdentity);
            var dataViolations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isDataValid) throw new ValidationError(dataViolations);

            var entity = entry.ToEntity<GalleryTopicEntry, GalleryTopic>();
            entity.HasChild = false;
            entity.TopicAlias = StringUtils.ConvertTitle2Alias(entry.TopicName);
            entity.TopicCode = entry.TopicCode;
            entity.ListOrder = UnitOfWork.NewsRepository.GetNewListOrder();
            entity.CreatedByUserId = userId;
            entity.Ip = NetworkUtils.GetIP4Address();

            UnitOfWork.GalleryTopicRepository.Insert(entity);
            UnitOfWork.SaveChanges();

            if (entry.ParentId != null && entry.ParentId > 0)
            {
                var parentEntity = UnitOfWork.GalleryTopicRepository.FindById(Convert.ToInt32(entry.ParentId));
                if (parentEntity == null) return;

                parentEntity.HasChild = true;
                UnitOfWork.GalleryTopicRepository.Update(parentEntity);

                var lineage = $"{parentEntity.Lineage},{entity.TopicId}";
                entity.Lineage = lineage;
                entity.Depth = lineage.Split(',').Count();
                entity.ParentId = entry.ParentId;
            }
            else
            {
                entity.ParentId = 0;
                entity.Lineage = $"{entity.TopicId}";
                entity.Depth = 1;
            }

            UnitOfWork.GalleryTopicRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateGalleryTopic(Guid userId, GalleryTopicEditEntry entry)
        {
            ISpecification<GalleryTopicEditEntry> validator = new GalleryTopicEditEntryValidator(UnitOfWork, PermissionLevel.Edit, CurrentClaimsIdentity);
            var dataViolations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isDataValid) throw new ValidationError(dataViolations);

            var entity = UnitOfWork.GalleryTopicRepository.FindById(entry.TopicId);
            if (entity == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NullGalleryTopic, "GalleryTopic"));
                throw new ValidationError(dataViolations);
            }

            if (string.IsNullOrEmpty(entry.TopicName))
            {
                dataViolations.Add(new RuleViolation(ErrorCode.InvalidTopicName, "TopicName"));
                throw new ValidationError(dataViolations);
            }
            else
            {
                if (entity.TopicName != entry.TopicName)
                {
                    bool isDuplicate = UnitOfWork.GalleryTopicRepository.HasTopicNameExisted(entry.TopicName, entry.ParentId);
                    if (isDuplicate)
                    {
                        dataViolations.Add(new RuleViolation(ErrorCode.DuplicateTopicName, "TopicName", entry.TopicName));
                        throw new ValidationError(dataViolations);
                    }
                }
            }

            if (string.IsNullOrEmpty(entry.TopicCode))
            {
                dataViolations.Add(new RuleViolation(ErrorCode.InvalidTopicCode, "TopicCode",null));
                throw new ValidationError(dataViolations);
            }
            else
            {
                if (entity.TopicCode != entry.TopicCode)
                {
                    bool isDuplicate = UnitOfWork.GalleryTopicRepository.HasTopicCodeExisted(entry.TopicCode);
                    if (isDuplicate)
                    {
                        dataViolations.Add(new RuleViolation(ErrorCode.DuplicateTopicCode, "TopicCode", entry.TopicCode));
                        throw new ValidationError(dataViolations);
                    }
                }
            }

            if (entry.ParentId != entity.ParentId && entry.ParentId != entity.TopicId)
            {
                if (entry.ParentId != null && entry.ParentId > 0)
                {
                    var children = UnitOfWork.GalleryTopicRepository.GetAllChildrenNodesOfSelectedNode(Convert.ToInt32(entry.TopicId)).ToList();
                    if (children.Any())
                    {
                        var parentIds = children.Select(x => x.TopicId).ToList();
                        if (parentIds.Contains(Convert.ToInt32(entry.ParentId)))
                        {
                            dataViolations.Add(new RuleViolation(ErrorCode.InvalidParentId, "ParentId", entry.ParentId, ErrorMessage.Messages[ErrorCode.InvalidParentId]));
                            throw new ValidationError(dataViolations);
                        }
                    }

                    //Update parent entry
                    var parentEntryEntity = UnitOfWork.GalleryTopicRepository.FindById(Convert.ToInt32(entry.ParentId));
                    if (parentEntryEntity == null)
                    {
                        dataViolations.Add(new RuleViolation(ErrorCode.NotFoundParentId, "ParentId"));
                        throw new ValidationError(dataViolations);
                    }
                    else
                    {
                        if (parentEntryEntity.HasChild == null || parentEntryEntity.HasChild == false)
                        {
                            parentEntryEntity.HasChild = true;
                            UnitOfWork.GalleryTopicRepository.Update(parentEntryEntity);
                        }
                    }

                    //Update parent entity
                    var parentEntity = UnitOfWork.GalleryTopicRepository.FindById(entity.ParentId);
                    if (parentEntity != null)
                    {
                        var childList = UnitOfWork.GalleryTopicRepository.GetAllChildrenNodesOfSelectedNode(entity.ParentId).ToList();
                        if (childList.Any())
                        {
                            childList = childList.Where(x => (x.TopicId != entity.ParentId) && (x.TopicId != entity.TopicId)).ToList();
                            parentEntity.HasChild = childList.Any();
                            UnitOfWork.GalleryTopicRepository.Update(parentEntity);
                        }
                    }

                    var lineage = $"{parentEntryEntity.Lineage},{entry.TopicId}";
                    entity.Lineage = lineage;
                    entity.Depth = lineage.Split(',').Count();
                    entity.ParentId = entry.ParentId;
                }
                else
                {
                    entity.ParentId = 0;
                    entity.Lineage = $"{entry.TopicId}";
                    entity.Depth = 1;
                }
            }

            //Update entity
            var hasChild = UnitOfWork.GalleryTopicRepository.HasChild(entity.TopicId);
            entity.HasChild = hasChild;
            entity.TopicCode = entry.TopicCode;
            entity.TopicAlias = StringUtils.GenerateFriendlyString(entry.TopicName);
            entity.Description = entry.Description;
            entity.ListOrder = UnitOfWork.GalleryTopicRepository.GetNewListOrder();
            entity.Status = entry.Status;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedByUserId = userId;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.GalleryTopicRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateGalleryTopicStatus(Guid userId, int id, GalleryTopicStatus status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.GalleryTopicRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullGalleryTopic, "GalleryTopic",null, ErrorMessage.Messages[ErrorCode.NullGalleryTopic]));
                throw new ValidationError(violations);
            }
          
            var isValid = Enum.IsDefined(typeof(GalleryTopicStatus), status);
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

            UnitOfWork.GalleryTopicRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        #endregion

        #region Gallery Collection

        public IEnumerable<GalleryCollectionInfoDetail> Search(GalleryCollectionSearchEntry filter, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = new List<GalleryCollectionInfoDetail>();
            var collections = UnitOfWork.GalleryCollectionRepository.GetGalleryCollections(filter.SearchCollectionName, filter.SearchTopicId, filter.SearchStatus,ref recordCount, orderBy,page, pageSize);
            if (collections != null)
            {
                lst.AddRange(collections.Select(collection => new GalleryCollectionInfoDetail
                {
                    TopicId = collection.TopicId,
                    CollectionId = collection.CollectionId,
                    CollectionName = collection.CollectionName,
                    Description = collection.Description,
                    IconFile = collection.IconFile,
                    ListOrder = collection.ListOrder,
                    Status = collection.Status,
                    GalleryTopic = collection.GalleryTopic.ToDto<GalleryTopic, GalleryTopicDetail>(),
                    GalleryFiles = GetGalleryFiles(collection.CollectionId, (filter.SearchStatus!=null && filter.SearchStatus == GalleryCollectionStatus.Active)?GalleryFileStatus.Active : GalleryFileStatus.InActive)
                }));
            }
            return lst;
        }
        public List<GalleryCollectionInfoDetail> SearchGalleryCollections(GallerySearchEntry filter)
        {
            var collectionSummaries = new List<GalleryCollectionInfoDetail>();

            if (string.IsNullOrEmpty(filter.SearchTopicCode))
            {
                if (filter.SearchCollectionId == null || filter.SearchCollectionId <= 0) return null;
                var collection = UnitOfWork.GalleryCollectionRepository.GetDetail(Convert.ToInt32(filter.SearchCollectionId));
                if (collection != null)
                {
                    collectionSummaries.Add(new GalleryCollectionInfoDetail
                    {
                        TopicId = collection.TopicId,
                        CollectionId = collection.CollectionId,
                        CollectionName = collection.CollectionName,
                        Description = collection.Description,
                        IconFile = collection.IconFile,
                        ListOrder = collection.ListOrder,
                        Status = collection.Status,
                        GalleryTopic = collection.GalleryTopic.ToDto<GalleryTopic, GalleryTopicDetail>(),
                        GalleryFiles = GetGalleryFiles(collection.CollectionId, filter.SearchStatus)
                    });
                }
                return collectionSummaries;
            }
            else
            {
                var topic = UnitOfWork.GalleryTopicRepository.GetDetailByCode(filter.SearchTopicCode);
                if (filter.SearchCollectionId != null && filter.SearchCollectionId > 0)
                {
                    var collection = UnitOfWork.GalleryCollectionRepository.FindById(Convert.ToInt32(filter.SearchCollectionId));
                    if (collection != null)
                    {
                        collectionSummaries.Add(new GalleryCollectionInfoDetail
                        {
                            TopicId = collection.TopicId,
                            CollectionId = collection.CollectionId,
                            CollectionName = collection.CollectionName,
                            Description = collection.Description,
                            IconFile = collection.IconFile,
                            ListOrder = collection.ListOrder,
                            Status = collection.Status,
                            GalleryTopic = topic.ToDto<GalleryTopic, GalleryTopicDetail>(),
                            GalleryFiles = GetGalleryFiles(collection.CollectionId, filter.SearchStatus)
                        });
                    }
                    return collectionSummaries;
                }
                else
                {
                    if (topic == null) return null;
                    var collections = UnitOfWork.GalleryCollectionRepository.GetGalleryCollections(filter.SearchTopicCode, Convert.ToInt32(topic.TopicId));
                    if (collections!=null && collections.Count() > 0)
                    {
                        collectionSummaries.AddRange(collections.Select(collection => new GalleryCollectionInfoDetail
                        {
                            TopicId = collection.TopicId,
                            CollectionId = collection.CollectionId,
                            CollectionName = collection.CollectionName,
                            Description = collection.Description,
                            IconFile = collection.IconFile,
                            ListOrder = collection.ListOrder,
                            Status = collection.Status,
                            GalleryTopic = topic.ToDto<GalleryTopic, GalleryTopicDetail>(),
                            GalleryFiles = GetGalleryFiles(collection.CollectionId, filter.SearchStatus)
                        }));
                    }
                    return collectionSummaries;
                }
            }
        }
        public List<GalleryCollectionInfoDetail> GetGalleryCollections(GalleryFileSearchEntry filter)
        {
            var collectionSummaries = new List<GalleryCollectionInfoDetail>();
            if (filter.SearchCollectionId != null && filter.SearchCollectionId > 0)
            {
                var collection =
                    UnitOfWork.GalleryCollectionRepository.FindById(Convert.ToInt32(filter.SearchCollectionId));
                if (collection != null)
                {
                    var galleryFiles = GetGalleryFiles(collection.CollectionId, filter.SearchStatus);

                    collectionSummaries.Add(new GalleryCollectionInfoDetail
                    {
                        TopicId = collection.TopicId,
                        CollectionId = collection.CollectionId,
                        CollectionName = collection.CollectionName,
                        Description = collection.Description,
                        IconFile = collection.IconFile,
                        ListOrder = collection.ListOrder,
                        Status = collection.Status,
                        GalleryFiles = galleryFiles
                    });
                }
                return collectionSummaries;
            }
            else
            {
                if (filter.SearchTopicId != null && filter.SearchTopicId > 0)
                {
                    var collections = UnitOfWork.GalleryCollectionRepository.GetGalleryCollections(null, Convert.ToInt32(filter.SearchTopicId)).ToList();
                    if (collections.Any())
                    {
                        collectionSummaries.AddRange(collections.Select(collection => new GalleryCollectionInfoDetail
                        {
                            TopicId = collection.TopicId,
                            CollectionId = collection.CollectionId,
                            CollectionName = collection.CollectionName,
                            Description = collection.Description,
                            IconFile = collection.IconFile,
                            ListOrder = collection.ListOrder,
                            Status = collection.Status,
                            GalleryFiles = GetGalleryFiles(collection.CollectionId, filter.SearchStatus)
                        }));
                    }
                }
                else
                {
                    var collections =
                        UnitOfWork.GalleryCollectionRepository.GetGalleryCollections(null, null);

                    if (collections!=null && collections.Count()>0)
                    {
                        collectionSummaries.AddRange(collections.Select(collection => new GalleryCollectionInfoDetail
                        {
                            TopicId = collection.TopicId,
                            CollectionId = collection.CollectionId,
                            CollectionName = collection.CollectionName,
                            Description = collection.Description,
                            IconFile = collection.IconFile,
                            ListOrder = collection.ListOrder,
                            Status = collection.Status,
                            GalleryFiles = GetGalleryFiles(collection.CollectionId, filter.SearchStatus)
                        }));
                    }
                }

                return collectionSummaries;
            }
        }
        public IEnumerable<GalleryCollectionInfoDetail> GetGalleryCollections(int? topicId, GalleryCollectionStatus? status = null)
        {
            var collectionSummaries = new List<GalleryCollectionInfoDetail>();
            var collections = UnitOfWork.GalleryCollectionRepository.GetGalleryCollections(null,topicId, status).ToList();
            if (!collections.Any()) return collectionSummaries;

            collectionSummaries.AddRange(collections.Select(collection => new GalleryCollectionInfoDetail
            {
                TopicId = collection.TopicId,
                CollectionId = collection.CollectionId,
                CollectionName = collection.CollectionName,
                Description = collection.Description,
                IconFile = collection.IconFile,
                ListOrder = collection.ListOrder,
                Status = collection.Status
            }));
            return collectionSummaries;
        }
        public GalleryCollectionInfoDetail GetLatestGalleryCollection()
        {
            var collection = UnitOfWork.GalleryCollectionRepository.GetLatestGalleryCollection();
            if (collection == null) return null;

            var galleryFiles = GetGalleryFiles(collection.CollectionId, GalleryFileStatus.Active);
            var item = new GalleryCollectionInfoDetail
            {
                TopicId = collection.TopicId,
                CollectionId = collection.CollectionId,
                CollectionName = collection.CollectionName,
                Description = collection.Description,
                IconFile = collection.IconFile,
                ListOrder = collection.ListOrder,
                Status = collection.Status,
                GalleryFiles = galleryFiles
            };
            return item;
        }
        public List<SelectListItem> GetGalleryCollectionSelectList(int topicId, GalleryCollectionStatus? status = null, int? selectedValue = null, bool? isShowSelectText = false)
        {
            return UnitOfWork.GalleryCollectionRepository.GetGalleryCollectionSelectList(topicId, status, selectedValue, isShowSelectText);
        }
        public SelectList PoplulateGalleryCollectionSelectList(int topicId, GalleryCollectionStatus? status = null, int? selectedValue = null, bool? isShowSelectText = false)
        {
            return UnitOfWork.GalleryCollectionRepository.PopulateGalleryCollectionSelectList(topicId, status, selectedValue, isShowSelectText);
        }
        public GalleryCollectionDetail GetGalleryCollectionDetail(int id)
        {
            var entity = UnitOfWork.GalleryCollectionRepository.FindById(id);
            return entity.ToDto<GalleryCollection, GalleryCollectionDetail>();
        }
        public GalleryCollectionDetail InsertGalleryCollection(Guid userId, GalleryCollectionEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullGalleryCollectionEntry, "GalleryCollectionEntry"));
                throw new ValidationError(violations);
            }
            else
            {
                if (string.IsNullOrEmpty(entry.CollectionName))
                {
                    violations.Add(new RuleViolation(ErrorCode.NullCollectionName, "CollectionName",
                        entry.CollectionName));
                    throw new ValidationError(violations);
                }
                else
                {
                    if (entry.CollectionName.Length > 250)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidCollectionName, "CollectionName",
                            entry.CollectionName));
                        throw new ValidationError(violations);
                    }
                    else
                    {
                        var isDuplicate = UnitOfWork.GalleryCollectionRepository.HasDataExisted(entry.CollectionName);
                        if (isDuplicate)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicateCollectionName, "CollectionName",
                                entry.CollectionName));
                            throw new ValidationError(violations);
                        }
                    }
                }
            }

            var entity = entry.ToEntity<GalleryCollectionEntry, GalleryCollection>();
            entity.ListOrder = UnitOfWork.GalleryCollectionRepository.GetNewListOrder();
            entity.Ip = NetworkUtils.GetIP4Address();
            entity.CreatedByUserId = userId;
            entity.CreatedDate = DateTime.UtcNow;

            UnitOfWork.GalleryCollectionRepository.Insert(entity);
            UnitOfWork.SaveChanges();
            return entity.ToDto<GalleryCollection, GalleryCollectionDetail>();
        }
        public void UpdateGalleryCollection(Guid userId, GalleryCollectionEditEntry entry)
        {
            //Check validation
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullReferenceGalleryCollectionEditEntry,
                    "GalleryCollectionEntry"));
                throw new ValidationError(violations);
            }

            var entity = UnitOfWork.GalleryCollectionRepository.Find(entry.CollectionId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundGalleryCollectionEditEntry, "GalleryCollectionEntry"));
                throw new ValidationError(violations);
            }

            if (string.IsNullOrEmpty(entry.CollectionName))
            {
                violations.Add(new RuleViolation(ErrorCode.NullCollectionName, "CollectionName",
                    entry.CollectionName));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.CollectionName.Length > 250)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidCollectionName, "CollectionName",
                        entry.CollectionName));
                    throw new ValidationError(violations);
                }
                else
                {
                    if (entity.CollectionName != entry.CollectionName)
                    {
                        var isDuplicate = UnitOfWork.GalleryCollectionRepository.HasDataExisted(entry.CollectionName);
                        if (isDuplicate)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicateCollectionName, "CollectionName",
                                entry.CollectionName));
                            throw new ValidationError(violations);
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(entry.Description) && entry.Description.Length > 500)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidDescription, "Description"));
                throw new ValidationError(violations);
            }

            if (!Enum.IsDefined(typeof(GalleryCollectionStatus), entry.Status))
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status"));
                throw new ValidationError(violations);
            }

            //Assign data
            entity.CollectionName = entry.CollectionName;
            entity.IconFile = entry.IconFile;
            entity.Description = entry.Description;
            entity.Status = entry.Status;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedByUserId = userId;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.GalleryCollectionRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateGalleryCollectionStatus(Guid userId, int id, GalleryCollectionStatus status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.GalleryCollectionRepository.Find(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidCollectionId, "CollectionId"));
                throw new ValidationError(violations);
            }

            var isValid = Enum.IsDefined(typeof(GalleryCollectionStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status", null,
                    ErrorMessage.Messages[ErrorCode.InvalidStatus]));
                throw new ValidationError(violations);
            }
            if (entity.Status == status) return;

            entity.Status = status;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedByUserId = userId;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.GalleryCollectionRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateGalleryCollectionListOrder(Guid userId, int id, int listOrder)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.GalleryCollectionRepository.Find(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidCollectionId, "CollectionId"));
                throw new ValidationError(violations);
            }

            entity.ListOrder = listOrder;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedByUserId = userId;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.GalleryCollectionRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void DeleteGalleryCollection(Guid userId, int id)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.GalleryCollectionRepository.Find(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidCollectionId, "CollectionId"));
                throw new ValidationError(violations);
            }

            entity.Status = GalleryCollectionStatus.InActive;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedByUserId = userId;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.GalleryCollectionRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
      
        #endregion

        #region Gallery File
        public IEnumerable<GalleryFileInfoDetail> GetGalleryFiles(int? collectionId, GalleryFileStatus? status, out int recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = new List<GalleryFileInfoDetail>();
            var gallerFiles = UnitOfWork.GalleryFileRepository.GetGalleryFileList(collectionId, status, out recordCount, orderBy, page, pageSize);
            if (!gallerFiles.Any()) return lst;

            lst.AddRange(gallerFiles.Select(gallerFile => new GalleryFileInfoDetail
            {
                CollectionId = gallerFile.CollectionId,
                FileId = gallerFile.FileId,
                ListOrder = gallerFile.ListOrder,
                Status = gallerFile.Status,
                GalleryTopic = gallerFile.GalleryTopic.ToDto<GalleryTopic, GalleryTopicDetail>(),
                GalleryCollection = gallerFile.GalleryCollection.ToDto<GalleryCollection, GalleryCollectionDetail>(),
                File = DocumentService.GetFileInfoDetail(gallerFile.FileId)
            }));
            return lst.AsEnumerable();
        }

        public IEnumerable<GalleryFileInfoDetail> GetGalleryFiles(int? collectionId, GalleryFileStatus? status = null)
        {
            var lst = new List<GalleryFileInfoDetail>();
            var gallerFiles = UnitOfWork.GalleryFileRepository.GetGalleryFiles(Convert.ToInt32(collectionId), status).ToList();
            if (!gallerFiles.Any()) return lst;

            lst.AddRange(gallerFiles.Select(gallerFile => new GalleryFileInfoDetail
            {
                CollectionId = gallerFile.CollectionId,
                FileId = gallerFile.FileId,
                ListOrder = gallerFile.ListOrder,
                Status = gallerFile.Status,
                GalleryTopic = gallerFile.GalleryTopic.ToDto<GalleryTopic, GalleryTopicDetail>(),
                GalleryCollection = gallerFile.GalleryCollection.ToDto<GalleryCollection, GalleryCollectionDetail>(),
                File = DocumentService.GetFileInfoDetail(gallerFile.FileId)
            }));
            return lst.AsEnumerable();
        }
        public List<GalleryFileInfoDetail> GetGalleryFilesFromLatestCollection(GalleryFileStatus? status = null)
        {
            var collection = UnitOfWork.GalleryCollectionRepository.GetLatestGalleryCollection();
            if (collection == null) return null;

            var gallerFiles = UnitOfWork.GalleryFileRepository.GetGalleryFiles(collection.CollectionId, status).ToList();
            if (!gallerFiles.Any()) return new List<GalleryFileInfoDetail>();

            return gallerFiles.Select(gallerFile => new GalleryFileInfoDetail
            {
                CollectionId = gallerFile.CollectionId,
                FileId = gallerFile.FileId,
                ListOrder = gallerFile.ListOrder,
                Status = gallerFile.Status,
                GalleryTopic = gallerFile.GalleryTopic.ToDto<GalleryTopic, GalleryTopicDetail>(),
                GalleryCollection = gallerFile.GalleryCollection.ToDto<GalleryCollection, GalleryCollectionDetail>(),
                File = DocumentService.GetFileInfoDetail(gallerFile.FileId)
            }).ToList();
        }
        public GalleryFileDetail GetGalleryFileDetail(int collectionId, int fileId)
        {
            var entity = UnitOfWork.GalleryFileRepository.GetDetails(collectionId, fileId);
            return entity.ToDto<GalleryFile, GalleryFileDetail>();
        }

        public void InsertGalleryFile(Guid applicationId, Guid userId, GalleryFileEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullGalleryFileEntry, "GalleryFileEntry"));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.CollectionId <= 0)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidCollectionId, "CollectionId"));
                    throw new ValidationError(violations);
                }

                if (entry.File != null && entry.File.ContentLength == 0)
                {
                    violations.Add(new RuleViolation(ErrorCode.NotFoundHttpPostedFile, "File"));
                    throw new ValidationError(violations);
                }

                var fileInfo = DocumentService.UploadAndSaveDbByFolderId(applicationId, userId, entry.File, (int)FileLocation.Gallery, StorageType.Local);
                if (fileInfo != null)
                {
                    var listOrder = UnitOfWork.GalleryFileRepository.GetNewListOrder();
                    var entity = new GalleryFile
                    {
                        CollectionId = entry.CollectionId,
                        FileId = fileInfo.FileId,
                        ListOrder = Convert.ToInt32(listOrder),
                        Status = entry.Status,
                        Ip = NetworkUtils.GetIP4Address(),
                        CreatedByUserId = userId,
                        CreatedDate = DateTime.UtcNow
                    };
                    UnitOfWork.GalleryFileRepository.Insert(entity);
                    UnitOfWork.SaveChanges();
                }
            }
        }

        public void UpdateGalleryFile(Guid applicationId, Guid userId, GalleryFileEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullGalleryFileEditEntry, "GalleryFileEditEntry", null, ErrorMessage.Messages[ErrorCode.NullGalleryFileEditEntry]));
                throw new ValidationError(violations);
            }

            if (entry.CollectionId <= 0)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidCollectionId, "CollectionId", null, ErrorMessage.Messages[ErrorCode.InvalidCollectionId]));
                throw new ValidationError(violations);
            }

            var entity = UnitOfWork.GalleryFileRepository.FindById(entry.GalleryFileId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundGalleryFile, "GalleryFile", $"{entry.CollectionId} - {entry.FileId}", ErrorMessage.Messages[ErrorCode.NotFoundGalleryFile]));
                throw new ValidationError(violations);
            }

            var prevFileId = entity.FileId;
            if (entry.File == null || entry.File.ContentLength == 0)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundHttpPostedFile, "File", null,
                         ErrorMessage.Messages[ErrorCode.NotFoundHttpPostedFile]));
                throw new ValidationError(violations);
            }
            else
            {
                var fileInfo = DocumentService.UploadAndSaveDbByFolderId(applicationId, userId, entry.File, (int)FileLocation.Gallery,
                    StorageType.Local);
                if (fileInfo != null)
                {
                    entity.FileId = fileInfo.FileId;
                   
                }
            }

            entity.CollectionId = entry.CollectionId;
            entity.Status = entry.Status;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedByUserId = userId;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.GalleryFileRepository.Update(entity);
            UnitOfWork.SaveChanges();

            DocumentService.DeleteFile(prevFileId);
        }

        public void UpdateGalleryFileStatus(Guid applicationId, Guid userId, int collectionId, int fileId, bool status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.GalleryFileRepository.GetDetails(collectionId, fileId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidCollectionId, "CollectionId"));
                throw new ValidationError(violations);
            }

            entity.Status = status ? GalleryFileStatus.Active : GalleryFileStatus.InActive;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedByUserId = userId;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.GalleryFileRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateGalleryFileListOrder(Guid userId, int collectionId, int fileId, int listOrder)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.GalleryFileRepository.GetDetails(collectionId, fileId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundGalleryFile, "CollectionId - FileId"));
                throw new ValidationError(violations);
            }

            entity.ListOrder = listOrder;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedByUserId = userId;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.GalleryFileRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateGalleryFileStatus(Guid applicationId, Guid userId, int id, bool status)
        {
            var entity = UnitOfWork.GalleryFileRepository.FindById(id);
            if (entity == null) throw new NotFoundDataException();

            var violations = new List<RuleViolation>();
            var isValid = Enum.IsDefined(typeof(GalleryFileStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status"));
                throw new ValidationError(violations);
            }

            entity.Status = status ? GalleryFileStatus.Active : GalleryFileStatus.InActive;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedByUserId = userId;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.GalleryFileRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void DeleteGalleryFile(int collectionId, int fileId)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.GalleryFileRepository.GetDetails(collectionId, fileId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundGalleryFile, "CollectionId - FileId"));
                throw new ValidationError(violations);
            }

            DocumentService.DeleteFile(entity.FileId);
            UnitOfWork.GalleryFileRepository.Delete(entity);
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
                    DocumentService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}
