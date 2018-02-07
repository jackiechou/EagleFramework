using System.Collections.Generic;
using System.Text.RegularExpressions;
using Eagle.Common.Extensions;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Contents.Articles;
using Eagle.Services.Validations;

namespace Eagle.Services.Contents.Validation
{
    public class NewsCommentEntryValidator : SpecificationBase<NewsCommentEntry>
    {
        private static IUnitOfWork UnitOfWork { get; set; }

        public NewsCommentEntryValidator(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        protected override bool IsSatisfyBy(NewsCommentEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundNewsCommentEntry, "NewsCommentEntry"));
                return false;
            }

            ISpecification<NewsCommentEntry> hasValidNewsId = new HasValidNewsId();
            ISpecification<NewsCommentEntry> hasValidCommentName = new HasValidCommentName();
            ISpecification<NewsCommentEntry> hasValidCommentText = new HasValidCommentText();
            ISpecification<NewsCommentEntry> hasValidEmail = new HasValidEmail();
            return hasValidNewsId.And(hasValidCommentName).And(hasValidCommentText).And(hasValidEmail).IsSatisfyBy(data, violations);
        }
        private class HasValidNewsId : SpecificationBase<NewsCommentEntry>
        {
            protected override bool IsSatisfyBy(NewsCommentEntry data, IList<RuleViolation> violations = null)
            {
                if (data.NewsId > 0 && violations != null)
                {
                    var entity = UnitOfWork.NewsCommentRepository.FindById(data.NewsId);
                    if (entity == null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.NullNewsId, "NewsId", null, ErrorMessage.Messages[ErrorCode.NullNewsId]));
                        return false;
                    }
                }
                return true;
            }
        }
        private class HasValidCommentName : SpecificationBase<NewsCommentEntry>
        {
            protected override bool IsSatisfyBy(NewsCommentEntry data, IList<RuleViolation> violations = null)
            {
                var result = !string.IsNullOrEmpty(data?.CommentName) && data.CommentName.Length > 300;
                if (result && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidCommentName, "CommentName", data.CommentName, ErrorMessage.Messages[ErrorCode.InvalidCommentName]));
                    return false;
                }
                return result;
            }
        }
        private class HasValidCommentText : SpecificationBase<NewsCommentEntry>
        {
            protected override bool IsSatisfyBy(NewsCommentEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.CommentText) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullCommentText, "CommentText", data.CommentText, ErrorMessage.Messages[ErrorCode.NullCommentText]));
                    return false;
                }
                else 
                {
                    if (data.CommentText.Length > 500 && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidCommentText, "CommentText", data.CommentText, ErrorMessage.Messages[ErrorCode.InvalidCommentText]));
                        return false;
                    }
                }
                return true;
            }
        }
        private class HasValidEmail : SpecificationBase<NewsCommentEntry>
        {
            protected override bool IsSatisfyBy(NewsCommentEntry data, IList<RuleViolation> violations = null)
            {
                string _validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
              + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
              + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

                var regex = new Regex(_validEmailPattern, RegexOptions.IgnoreCase);
                if (!regex.IsMatch(data.CreatedByEmail) || data.CreatedByEmail.IsNullOrEmpty() || data.CreatedByEmail.Length > 100)
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidEmail, "Email"));
                        return false;
                    }
                }

                return true;
            }
        }
    }
}
