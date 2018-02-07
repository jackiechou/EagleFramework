using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.SystemManagement.Identity;
using Eagle.Services.Validations;

namespace Eagle.Services.SystemManagement.Validation
{
    public class UserEditEntryValidator : SpecificationBase<UserEditEntry>
    {
        private static IUnitOfWork UnitOfWork { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        public UserEditEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
        }

        protected override bool IsSatisfyBy(UserEditEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullUserEditEntry, "UserEditEntry"));
                return false;
            }

            ISpecification<UserEditEntry> hasValidUserName = new HasValidUserName();
            ISpecification<UserEditEntry> hasValidEmail = new HasValidEmail();
            ISpecification<UserEditEntry> hasValidPasswordSalt = new HasValidPasswordSalt();
            ISpecification<UserEditEntry> hasValidPassword = new HasValidPassword();
            ISpecification<UserEditEntry> hasValidDate = new HasValidDate();
            ISpecification<UserEditEntry> hasValidFirstName = new HasValidFirstName();
            ISpecification<UserEditEntry> hasValidLastName = new HasValidLastName();
            ISpecification<UserEditEntry> hasValidMobile = new HasValidMobile();

            return hasValidUserName.And(hasValidEmail).And(hasValidPasswordSalt).And(hasValidPassword)
                .And(hasValidDate).And(hasValidFirstName).And(hasValidLastName)
                .And(hasValidMobile).IsSatisfyBy(data, violations);
        }

        internal class HasValidUserName : SpecificationBase<UserEditEntry>
        {
            protected override bool IsSatisfyBy(UserEditEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.UserName) || data.UserName.Length > 256)
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidUserName));
                    }
                    return false;
                }

                var user = UnitOfWork.UserRepository.FindByUserName(data.UserName);
                if (user == null)
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidUserName));
                    }
                    return false;
                }
                return true;
            }
        }
        internal class HasValidPasswordSalt : SpecificationBase<UserEditEntry>
        {
            protected override bool IsSatisfyBy(UserEditEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.PasswordSalt) || data.PasswordSalt.Length > 128)
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidPasswordSalt));
                    }
                    return false;
                }
                return true;
            }
        }
        internal class HasValidPassword : SpecificationBase<UserEditEntry>
        {
            protected override bool IsSatisfyBy(UserEditEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.PasswordSalt) || data.PasswordSalt.Length > 128)
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidPassword));
                    }
                    return false;
                }
                return true;
            }
        }
        internal class HasValidEmail : SpecificationBase<UserEditEntry>
        {
            string _validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
            + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
            + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            protected override bool IsSatisfyBy(UserEditEntry data, IList<RuleViolation> violations = null)
            {
                var regex = new Regex(_validEmailPattern, RegexOptions.IgnoreCase);
                if (!regex.IsMatch(data.Contact.Email))
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidEmailFormat, "Email"));
                        return false;
                    }
                }
                else
                {
                    var profile = UnitOfWork.UserProfileRepository.GetProfile(data.UserId);
                    if (profile.Contact != null)
                    {
                        var contact = profile.Contact;
                        if (!string.IsNullOrEmpty(contact.Email) && contact.Email != data.Contact.Email)
                        {
                            var isExisted = UnitOfWork.UserRepository.IsEmailExisted(data.Contact.Email);
                            if (isExisted)
                            {
                                if (violations != null)
                                {
                                    violations.Add(new RuleViolation(ErrorCode.ExistedEmail, "Email"));
                                    return false;
                                }
                            }
                        }
                        return true;
                    }
                }
                return true;
            }
        }
        internal class HasValidDate : SpecificationBase<UserEditEntry>
        {
            protected override bool IsSatisfyBy(UserEditEntry data, IList<RuleViolation> violations = null)
            {
                if (data.StartDate != null && data.StartDate <= DateTime.MinValue.ToUniversalTime())
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidStartDate));
                        return false;
                    }
                }

                if (data.ExpiredDate != null && data.ExpiredDate <= DateTime.MinValue.ToUniversalTime())
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidExpiredDate));
                        return false;
                    }
                }

                if (data.ExpiredDate < data.StartDate)
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidExpiredDate));
                        return false;
                    }
                }
                return true;
            }
        }
        internal class HasValidFirstName : SpecificationBase<UserEditEntry>
        {
            protected override bool IsSatisfyBy(UserEditEntry data, IList<RuleViolation> violations = null)
            {
                var isValid = !string.IsNullOrEmpty(data.Contact.FirstName);
                if (!isValid)
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidFirstName, "FirstName"));
                        return false;
                    }
                }
                return true;
            }
        }
        private class HasValidLastName : SpecificationBase<UserEditEntry>
        {
            protected override bool IsSatisfyBy(UserEditEntry data, IList<RuleViolation> violations = null)
            {
                var isValid = !string.IsNullOrEmpty(data.Contact.LastName);
                if (!isValid)
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidLastName, "LastName"));
                        return false;
                    }
                }

                return true;
            }
        }
        private class HasValidMobile : SpecificationBase<UserEditEntry>
        {
            protected override bool IsSatisfyBy(UserEditEntry data, IList<RuleViolation> violations = null)
            {
                if (!string.IsNullOrEmpty(data.Contact.Mobile))
                {
                    if (data.Contact.Mobile.Length > 50 || data.Contact.Mobile.Length <= 0)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.InvalidMobile, "Mobile"));
                            return false;
                        }
                    }
                    else
                    {
                        string phone = data.Contact.Mobile;
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

                        if (phone.Length > 0)
                        {
                            if (violations != null)
                            {
                                violations.Add(new RuleViolation(ErrorCode.InvalidFormatMobile, "Mobile"));
                                return false;
                            }
                        }
                        else
                        {
                            var profile = UnitOfWork.UserProfileRepository.GetProfile(data.UserId);
                            if (profile.Contact != null)
                            {
                                var contact = profile.Contact;
                                if (!string.IsNullOrEmpty(contact.Mobile) && contact.Mobile != data.Contact.Mobile)
                                {
                                    var isExisted = UnitOfWork.UserRepository.IsMobileExisted(data.Contact.Mobile);
                                    if (isExisted)
                                    {
                                        if (violations != null)
                                        {
                                            violations.Add(new RuleViolation(ErrorCode.ExistedMobile, "Mobile"));
                                            return false;
                                        }
                                    }
                                }
                                return true;
                            }
                        }
                        return true;
                    }
                }
                return true;
            }
        }
    }
}
