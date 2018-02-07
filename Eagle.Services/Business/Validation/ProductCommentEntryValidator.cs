using System.Collections.Generic;
using System.Security.Claims;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Validations;

namespace Eagle.Services.Business.Validation
{
    public class ProductCommentEntryValidator : SpecificationBase<ProductCommentEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }
        private PermissionLevel PermissionLevel { get; set; }

        public ProductCommentEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel,
            ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(ProductCommentEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundProductCommentEntry, "ProductCommentEntry",null, ErrorMessage.Messages[ErrorCode.NotFoundProductCommentEntry]));
                return false;
            }
            ISpecification<ProductCommentEntry> hasValidCommentName = new HasValidCommentName();
            ISpecification<ProductCommentEntry> hasValidCommentEmail = new HasValidCommentEmail();
            ISpecification<ProductCommentEntry> hasValidMobile = new HasValidMobile();

            return hasValidCommentName.And(hasValidCommentEmail).And(hasValidMobile).IsSatisfyBy(data, violations);
        }

        private class HasValidCommentName : SpecificationBase<ProductCommentEntry>
        {
            protected override bool IsSatisfyBy(ProductCommentEntry data, IList<RuleViolation> violations = null)
            {
                if (!string.IsNullOrEmpty(data.CommentName) && data.CommentName.Length > 250)
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidCommentName, "CommentName"));
                        return false;
                    }
                }
                return true;
            }
        }

        private class HasValidCommentEmail : SpecificationBase<ProductCommentEntry>
        {
            protected override bool IsSatisfyBy(ProductCommentEntry data, IList<RuleViolation> violations = null)
            {
                if (!string.IsNullOrEmpty(data.CommentEmail) && data.CommentEmail.Length > 150)
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidEmail, "CommentEmail", data.CommentEmail, ErrorMessage.Messages[ErrorCode.InvalidEmail]));
                        return false;
                    }
                }
                
                return true;
            }
        }
        private class HasValidMobile : SpecificationBase<ProductCommentEntry>
        {
            protected override bool IsSatisfyBy(ProductCommentEntry data, IList<RuleViolation> violations = null)
            {
                if (!string.IsNullOrEmpty(data.CommentMobile) )
                {
                    if (data.CommentMobile.Length > 20 & violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidMobile, "Mobile", data.CommentMobile,
                            ErrorMessage.Messages[ErrorCode.InvalidMobile]));
                        return false;
                    }
                    else
                    {
                        string phone = data.CommentMobile;
                        phone = phone.Replace("0", "");
                        phone = phone.Replace("1", "");
                        phone = phone.Replace("2", "");
                        phone = phone.Replace("3", "");
                        phone = phone.Replace("4", "");
                        phone = phone.Replace("5", "");
                        phone = phone.Replace("6", "");
                        phone = phone.Replace("7", "");
                        phone = phone.Replace("8", "");
                        phone = phone.Replace("9", "");
                        phone = phone.Replace("+", "");
                        phone = phone.Replace("-", "");
                        phone = phone.Replace(")", "");
                        phone = phone.Replace("(", "");
                        phone = phone.Replace(" ", "");

                        if (phone.Length > 0 && violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.InvalidPhone, "Phone", phone, ErrorMessage.Messages[ErrorCode.InvalidPhone]));
                            return false;
                        }
                    }
                }
                return true;
            }
        }
    }
}
