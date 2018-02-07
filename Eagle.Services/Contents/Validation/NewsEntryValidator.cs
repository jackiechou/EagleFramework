using System.Collections.Generic;
using System.Security.Claims;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Contents.Articles;
using Eagle.Services.Validations;

namespace Eagle.Services.Contents.Validation
{
    public class NewsEntryValidator : SpecificationBase<NewsEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        public NewsEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel, ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(NewsEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundNewsEntry,"NewsEntry"));
                return false;
            }

            ISpecification<NewsEntry> isValidCategoryId = new IsValidCategoryId();
            ISpecification<NewsEntry> isValidTitle = new IsValidTitle();
            ISpecification<NewsEntry> isValidHeadline = new IsValidHeadline();

            return isValidCategoryId.And(isValidTitle).And(isValidHeadline).IsSatisfyBy(data, violations);
        }

        private class IsValidTitle : SpecificationBase<NewsEntry>
        {
            protected override bool IsSatisfyBy(NewsEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.Title) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullTitle, "Title", null,ErrorMessage.Messages[ErrorCode.NullTitle]));
                    return false;
                }
                else
                {
                    if (data.Title.Length > 300 && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidTitle, "Title", data.Title, ErrorMessage.Messages[ErrorCode.InvalidTitle]));
                        return false;
                    }

                    var isDuplicated = UnitOfWork.NewsRepository.HasDataExisted(data.CategoryId, data.Title);
                    if(isDuplicated && violations != null)
                    {
                        violations?.Add(new RuleViolation(ErrorCode.DuplicateTitle, "Title", data.Title, ErrorMessage.Messages[ErrorCode.DuplicateTitle]));
                        return false;
                    }
                    return true;
                }
            }
        }
        private class IsValidHeadline : SpecificationBase<NewsEntry>
        {
            protected override bool IsSatisfyBy(NewsEntry data, IList<RuleViolation> violations = null)
            {
                var result = !string.IsNullOrEmpty(data.Headline) && data.Headline.Length > 300;
                if (result && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvaliHeadline,"Headline", data.Headline, ErrorMessage.Messages[ErrorCode.InvaliHeadline]));
                    return false;
                }
                return true;
            }
        }
        private class IsValidCategoryId : SpecificationBase<NewsEntry>
        {
            protected override bool IsSatisfyBy(NewsEntry data, IList<RuleViolation> violations = null)
            {
                var result = data.CategoryId <= 0;
                if (result && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidCategoryId, "CategoryId", data.CategoryId, ErrorMessage.Messages[ErrorCode.InvalidCategoryId]));
                    return false;
                }
                return true;
            }
        }
    }
}
