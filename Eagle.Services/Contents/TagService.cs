using System;
using System.Collections.Generic;
using System.Linq;
using Eagle.Core.Settings;
using Eagle.Entities.Contents.Tags;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Contents.Tags;
using Eagle.Services.EntityMapping.Common;
using Eagle.Services.Validations;

namespace Eagle.Services.Contents
{
    public class TagService : BaseService, ITagService
    {
        public TagService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        #region TAG
        public IEnumerable<TagDetail> GetTags(SearchTagEntry searchTagEntry, ref int? recordCount, int? page = null, int? pageSize = null)
        {
            var lst = UnitOfWork.TagRepository.GetTags(searchTagEntry.TagName, searchTagEntry.TagTypeId, ref recordCount, page, pageSize);
            return lst.ToDtos<Tag, TagDetail>();
        }
        public void CreateTag(TagEntry entry)
        {
            if (entry == null) return;
            var violations = new List<RuleViolation>();
            if (string.IsNullOrEmpty(entry.TagName))
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidTagName, "TagName"));
                throw new ValidationError(violations);
            }

            if (entry.TagName.Trim().Length == 0 || entry.TagName.Length > 20)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidTagName, "TagName"));
                throw new ValidationError(violations);
            }

            var tag = UnitOfWork.TagRepository.GetTagsByName(entry.TagType, entry.TagName);
            if (tag != null)
            {
                violations.Add(new RuleViolation(ErrorCode.DuplicateName, "TagName"));
                throw new ValidationError(violations);
            }

            var entity = entry.ToEntity<TagEntry, Tag>();
            UnitOfWork.TagRepository.Insert(entity);
            UnitOfWork.SaveChanges();
        }

        public void EditTag(int tagId, TagEntry entry)
        {
            if (entry == null) return;

            var violations = new List<RuleViolation>();
            if (string.IsNullOrEmpty(entry.TagName))
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidTagName, "TagName"));
                throw new ValidationError(violations);
            }

            if (entry.TagName.Trim().Length == 0 || entry.TagName.Length > 20)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidTagName, "TagName"));
                throw new ValidationError(violations);
            }

            var tag = UnitOfWork.TagRepository.FindById(tagId);
            if (tag == null)
            {
                violations.Add(new RuleViolation (ErrorCode.NotFoundTag, "TagName"));
                throw new ValidationError(violations);
            }

            if (entry.TagType != tag.TagType)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidTagType, "TagType", entry.TagType));
                throw new ValidationError(violations);
            }

            if (!tag.TagName.Equals(entry.TagName))
            {
                var entity = UnitOfWork.TagRepository.GetTagsByName(entry.TagType, entry.TagName);
                if (entity != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.DuplicateName, "TagName"));
                    throw new ValidationError(violations);
                }
            }

            tag.TagName = entry.TagName;
            UnitOfWork.TagRepository.Update(tag);
            UnitOfWork.SaveChanges();
        }

        public void DeleteTagByTagId(int tagId)
        {
            var violations = new List<RuleViolation>();
            var tag = UnitOfWork.TagRepository.FindById(tagId);
            if (tag == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundTag, "TagId", tagId));
                throw new ValidationError(violations);
            }
            UnitOfWork.TagRepository.Delete(tag);
            UnitOfWork.SaveChanges();
        }

        #endregion

        #region TAG INTEGRATION
        public IEnumerable<TagDetail> GetTagsByTagKey(int tagKey)
        {
            var tagIntegrations = UnitOfWork.TagIntegrationRepository.GetTagIntegrationsByKey(tagKey).ToList();
            if (!tagIntegrations.Any()) return null;

            var tagIds = tagIntegrations.Select(x => x.TagId).ToList();
            if (!tagIds.Any()) return null;

            var tags = UnitOfWork.TagRepository.GetTagsByTagIds(tagIds);
            return tags.ToDtos<Tag, TagDetail>();
        }

        public void DeleteTagsByTagKey(int tagKey)
        {
            var violations = new List<RuleViolation>();
            var tagIntegrations = UnitOfWork.TagIntegrationRepository.GetTagIntegrationsByKey(tagKey).ToList();
            if (!tagIntegrations.Any())
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundTag, "TagKey", tagKey));
                throw new ValidationError(violations);
            }

            var tagIds = tagIntegrations.Select(x => x.TagId).ToList();
            if (!tagIds.Any()) return;

            //Delete all selected items in tag integration
            foreach (var tagIntegration in tagIntegrations)
            {
                UnitOfWork.TagIntegrationRepository.Delete(tagIntegration);
            }

            var tags = UnitOfWork.TagRepository.GetTagsByTagIds(tagIds);
            if (tags == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundTag, "Tag"));
                throw new ValidationError(violations);
            }

            //Delete all selected items in tag
            foreach (var tag in tags)
            {
                UnitOfWork.TagRepository.Delete(tag);
            }

            UnitOfWork.SaveChanges();
        }

        public void UpdateTagIntegrationStatus(int tagId, TagStatus status)
        {
            var violations = new List<RuleViolation>();
            var tagIntegration = UnitOfWork.TagIntegrationRepository.GetTagIntegrationByTagId(tagId);
            if (tagIntegration == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundTag, "TagName"));
                throw new ValidationError(violations);
            }
            tagIntegration.TagStatus = status;
            tagIntegration.LastModifiedDate = DateTime.UtcNow;
            UnitOfWork.TagIntegrationRepository.Update(tagIntegration);
        }

        #endregion
    }
}
