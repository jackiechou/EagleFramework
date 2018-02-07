using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Entities.SystemManagement;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.SystemManagement;
using Eagle.Services.EntityMapping.Common;
using Eagle.Services.Exceptions;
using Eagle.Services.Validations;

namespace Eagle.Services.SystemManagement
{
    public class ContentService : BaseService, IContentService
    {
        public ContentService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        #region Content Type----------------------------------------------------------------------------------------------
        public IEnumerable<ContentTypeDetail> GetContentTypes(out int recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var result = UnitOfWork.ContentTypeRepository.Get(out recordCount, null, orderBy,null, page,pageSize);
            return result.ToDtos<ContentType, ContentTypeDetail>();
        }
        public ContentTypeDetail GetContentTypeDetails(int id)
        {
            var entity = UnitOfWork.ContentTypeRepository.FindById(id);
            return entity.ToDto<ContentType, ContentTypeDetail>();
        }
        public void InsertContentType(ContentTypeEntry entry)
        {
            var violations = new List<RuleViolation>();
            bool isDuplicate = UnitOfWork.ContentTypeRepository.HasDataExisted(entry.ContentTypeName);
            if (isDuplicate)
            {
                violations.Add(new RuleViolation(ErrorCode.DuplicateContentTypeName, "ContentTypeName", entry.ContentTypeName));
                throw new ValidationError(violations);
            }

            var entity = new ContentType
            {
                ContentTypeName = entry.ContentTypeName
            };

            UnitOfWork.ContentTypeRepository.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateContentType(int id, ContentTypeEntry entry)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ContentTypeRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullContentTypeEntry, "ContentTypeEntry"));
                throw new ValidationError(violations);
            }
            entity.ContentTypeName = entry.ContentTypeName;

            UnitOfWork.ContentTypeRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void DeleteContentType(int id)
        {
            var entity = UnitOfWork.ContentTypeRepository.FindById(id);
            if (entity == null) return;
            UnitOfWork.ContentTypeRepository.Delete(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion ----------------------------------------------------------------------------------------------

        #region Content Items----------------------------------------------------------------------------------------------
        public SelectList PopulateContentItemsByPageToDropDownList(string selectedValue, bool isShowSelectText = false)
        {
            return UnitOfWork.ContentItemRepository.PopulateContentItemsByPageToDropDownList(selectedValue, isShowSelectText);
        }

        public SelectList PopulateContentItemsByModuleToDropDownList(string selectedValue, bool isShowSelectText = false)
        {
            return UnitOfWork.ContentItemRepository.PopulateContentItemsByModuleToDropDownList(selectedValue, isShowSelectText);
        }

        public IEnumerable<ContentItemDetail> GetContentItems(int contentTypeId)
        {
            var result = UnitOfWork.ContentItemRepository.GetList(contentTypeId);
            return result.ToDtos<ContentItem, ContentItemDetail>();
        }

        public ContentItemDetail GetContentItemDetails(int id)
        {
            var entity = UnitOfWork.ContentItemRepository.FindById(id);
            return entity.ToDto<ContentItem, ContentItemDetail>();
        }

        public List<RuleViolation> GetRuleViolations(ContentItemEntry data)
        {
            List<RuleViolation> validationIssues = new List<RuleViolation>();
            if (data == null)
            {
                validationIssues.Add(new RuleViolation(ErrorCode.NullContentItemEntry));
            }
            else
            {
                if (string.IsNullOrEmpty(data.ItemKey))
                {
                    validationIssues.Add(new RuleViolation(ErrorCode.InvalidItemKey, "ItemKey", data.ItemKey, ErrorMessage.Messages[ErrorCode.InvalidItemKey]));
                }

                if (string.IsNullOrEmpty(data.ItemContent))
                {
                    validationIssues.Add(new RuleViolation(ErrorCode.InvalidItemKey, "ItemKey", data.ItemKey, ErrorMessage.Messages[ErrorCode.InvalidItemKey]));
                }
            }
            return validationIssues;
        }

        public void Validate(ContentItemEntry data)
        {
            var validationIssues = GetRuleViolations(data);
            if (validationIssues.Count > 0)
                throw new RuleViolationException("Validation Issues", validationIssues);
        }

        public void InsertContentItem(ContentItemEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullContentItemEntry, "ContentItemEntry"));
                throw new ValidationError(violations);
            }

            bool isDuplicate = UnitOfWork.ContentItemRepository.HasDataExisted(entry.ContentTypeId, entry.ItemContent);
            if (isDuplicate)
            {
                violations.Add(new RuleViolation(ErrorCode.DuplicateContentItemName, "ContentItemName"));
                throw new ValidationError(violations);
            }

            var entity = new ContentItem
            {
                ContentTypeId = entry.ContentTypeId,
                ItemKey = entry.ItemKey,
                ItemText = entry.ItemContent
            };

            UnitOfWork.ContentItemRepository.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateContentItem(int id, ContentItemEntry entry)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ContentItemRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullContentItem, "ContentItemEntry", id));
                throw new ValidationError(violations);
            }

            entity.ContentTypeId = entry.ContentTypeId;
            entity.ItemKey = entry.ItemKey;
            entity.ItemText = entry.ItemContent;

            UnitOfWork.ContentItemRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void DeleteContentItem(int id)
        {
            var entity = UnitOfWork.ContentItemRepository.FindById(id);
            if (entity == null) return;
            UnitOfWork.ContentItemRepository.Delete(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion ------------------------------------------------------------------------------------------------------

    }
}
