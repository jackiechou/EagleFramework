using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Contents.Feedbacks;
using Eagle.Services.Validations;

namespace Eagle.Services.Contents.Validation
{
    public class FeedbackEntryValidator : SpecificationBase<FeedbackEntry>
    {
        private static IUnitOfWork UnitOfWork { get; set; }

        public FeedbackEntryValidator(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        protected override bool IsSatisfyBy(FeedbackEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullFeedbackEntry, "FeedbackEntry"));
                return false;
            }

            ISpecification<FeedbackEntry> hasValidEmail = new HasValidEmail();
            ISpecification<FeedbackEntry> hasValidSenderName = new HasValidSenderName();
            ISpecification<FeedbackEntry> hasValidSubject = new HasValidSubject();
            ISpecification<FeedbackEntry> hasValidBody = new HasValidBody();
            

            return hasValidEmail.And(hasValidSenderName).And(hasValidSubject).And(hasValidBody).IsSatisfyBy(data, violations);
        }
        private class HasValidEmail : SpecificationBase<FeedbackEntry>
        {
            protected override bool IsSatisfyBy(FeedbackEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.Email) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullEmail, "Email", null,
                        ErrorMessage.Messages[ErrorCode.NullEmail]));
                    return false;
                }
                else
                {
                    string _validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                                                + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                                                + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

                    var regex = new Regex(_validEmailPattern, RegexOptions.IgnoreCase);
                    if (!regex.IsMatch(data.Email) || data.Email.Length > 150)
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
        private class HasValidSenderName : SpecificationBase<FeedbackEntry>
        {
            protected override bool IsSatisfyBy(FeedbackEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.SenderName) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullSenderName, "SenderName", null,
                        ErrorMessage.Messages[ErrorCode.NullSenderName]));
                    return false;
                }
                else
                {
                    if (data.SenderName.Length > 250 && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidSenderName, "SenderName", data.SenderName,
                        ErrorMessage.Messages[ErrorCode.InvalidSenderName]));
                        return false;
                    }
                    return true;
                }
            }
        }
        private class HasValidSubject : SpecificationBase<FeedbackEntry>
        {
            protected override bool IsSatisfyBy(FeedbackEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.Subject) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullSubject, "Subject", null,
                        ErrorMessage.Messages[ErrorCode.NullSubject]));
                    return false;
                }
                else
                {
                    if (data.Subject.Length > 250 && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidSubject, "Subject", data.Subject,
                        ErrorMessage.Messages[ErrorCode.InvalidSubject]));
                        return false;
                    }
                    return true;
                }
            }
        }
        private class HasValidBody : SpecificationBase<FeedbackEntry>
        {
            protected override bool IsSatisfyBy(FeedbackEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.Body) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullBody, "Body", null,
                        ErrorMessage.Messages[ErrorCode.NullBody]));
                    return false;
                }
                else
                {
                    if (data.Body.Length > 500 && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidBody, "Body", data.Body,
                        ErrorMessage.Messages[ErrorCode.InvalidBody]));
                        return false;
                    }
                    return true;
                }
            }
        }
        
    }
}
