using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Resources;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.SystemManagement.Identity;
using Eagle.Services.Validations;

namespace Eagle.Services.SystemManagement.Validation
{
    public class UserEntryValidator : SpecificationBase<UserEntry>
    {
        private static IUnitOfWork UnitOfWork { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        public UserEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
        }

        protected override bool IsSatisfyBy(UserEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundUserEntry, "UserEntry",null, ErrorMessage.Messages[ErrorCode.NotFoundUserEntry]));
                return false;
            }

            ISpecification<UserEntry> hasValidUserName = new HasValidUserName();
            ISpecification<UserEntry> hasValidEmail = new HasValidEmail();
            ISpecification<UserEntry> isExistingEmail = new IsExistingEmail();
            ISpecification<UserEntry> hasValidPasswordSalt = new HasValidPasswordSalt();
            ISpecification<UserEntry> hasValidDate = new HasValidDate();

            ISpecification<UserEntry> hasValidFirstName = new HasValidFirstName();
            ISpecification<UserEntry> hasValidLastName = new HasValidLastName();
            ISpecification<UserEntry> hasExitedEmail = new HasExitedEmail();
            ISpecification<UserEntry> hasValidMobile = new HasValidMobile();


            return hasValidUserName.
                And(hasValidEmail).And(isExistingEmail).And(hasValidPasswordSalt)
                .And(hasValidDate).And(hasValidFirstName).And(hasValidLastName).And(hasExitedEmail)
                .And(hasValidMobile).IsSatisfyBy(data, violations);
        }

        //internal class HasValidRoleId : SpecificationBase<UserEntry>
        //{
        //    protected override bool IsSatisfyBy(UserEntry data, IList<RuleViolation> violations = null)
        //    {
        //        var entity = UnitOfWork.RoleRepository.FindById(data.RoleId);
        //        if (entity == null)
        //        {
        //            if (violations != null)
        //            {
        //                violations.Add(new RuleViolation(ErrorCodeType.NotFoundForApplication, "ApplicationId"));
        //            }
        //            return false;
        //        }
        //        return true;
        //    }
        //}
        internal class HasValidUserName : SpecificationBase<UserEntry>
        {
            protected override bool IsSatisfyBy(UserEntry data, IList<RuleViolation> violations = null)
            {
                if ((string.IsNullOrEmpty(data.UserName) || data.UserName.Length > 256) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidUserName, "Username", null, LanguageResource.InvalidUserName));
                    return false;
                }

                var user = UnitOfWork.UserRepository.FindByUserName(data.UserName);
                if (user != null && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.DuplicateUserName, "Username", data.UserName, LanguageResource.DuplicateUserName));
                    return false;
                }
                return true;
            }
        }
        internal class HasValidPasswordSalt : SpecificationBase<UserEntry>
        {
            protected override bool IsSatisfyBy(UserEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.PasswordSalt) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullPasswordSalt, "PasswordSalt", null, ErrorMessage.Messages[ErrorCode.NullPasswordSalt]));
                    return false;
                }
                else
                {
                    if (data.PasswordSalt.Length > 128 && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidPasswordSalt, "PasswordSalt", data.PasswordSalt, ErrorMessage.Messages[ErrorCode.InvalidPasswordSalt]));
                        return false;
                    }
                }
                return true;
            }
        }

        internal class HasValidEmail : SpecificationBase<UserEntry>
        {
            string _validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
            + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
            + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            protected override bool IsSatisfyBy(UserEntry data, IList<RuleViolation> violations = null)
            {
                var regex = new Regex(_validEmailPattern, RegexOptions.IgnoreCase);
                if (!regex.IsMatch(data.Contact.Email) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidEmail, "Email", data.Contact.Email, ErrorMessage.Messages[ErrorCode.InvalidEmail]));
                    return false;
                }

                return true;
            }
        }
        private class IsExistingEmail : SpecificationBase<UserEntry>
        {
            protected override bool IsSatisfyBy(UserEntry data, IList<RuleViolation> violations = null)
            {
                var isExisted = UnitOfWork.UserRepository.IsEmailExisted(data.Contact.Email);
                if (isExisted && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.ExistedEmail, "Email", data.Contact.Email, ErrorMessage.Messages[ErrorCode.ExistedEmail]));
                    return false;
                }
                return true;
            }
        }
        internal class HasValidDate : SpecificationBase<UserEntry>
        {
            protected override bool IsSatisfyBy(UserEntry data, IList<RuleViolation> violations = null)
            {
                if (data.StartDate != null && data.StartDate <= DateTime.MinValue.ToUniversalTime() && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidStartDate, "StartDate", data.StartDate, ErrorMessage.Messages[ErrorCode.InvalidStartDate]));
                    return false;
                }

                if (data.ExpiredDate != null && data.ExpiredDate <= DateTime.MinValue.ToUniversalTime() && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidExpiredDate, "ExpiredDate", data.ExpiredDate, ErrorMessage.Messages[ErrorCode.InvalidExpiredDate]));
                    return false;
                }

                if (data.ExpiredDate < data.StartDate && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidExpiredDate, "ExpiredDate", data.ExpiredDate, ErrorMessage.Messages[ErrorCode.InvalidExpiredDate]));
                    return false;
                }
                return true;
            }
        }

        private class HasValidFirstName : SpecificationBase<UserEntry>
        {
            protected override bool IsSatisfyBy(UserEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.Contact.FirstName) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidFirstName, "FirstName", data.Contact.FirstName, ErrorMessage.Messages[ErrorCode.InvalidFirstName]));
                    return false;
                }

                return true;
            }
        }
        private class HasValidLastName : SpecificationBase<UserEntry>
        {
            protected override bool IsSatisfyBy(UserEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.Contact.LastName) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidLastName, "LastName", null, ErrorMessage.Messages[ErrorCode.InvalidLastName]));
                    return false;
                }

                return true;
            }
        }

        private class HasExitedEmail : SpecificationBase<UserEntry>
        {
            protected override bool IsSatisfyBy(UserEntry data, IList<RuleViolation> violations = null)
            {
                if (!string.IsNullOrEmpty(data.Contact.Email))
                {
                    var isExisted = UnitOfWork.UserRepository.IsEmailExisted(data.Contact.Email);
                    if (isExisted && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.ExistedEmail, "Email", null, ErrorMessage.Messages[ErrorCode.ExistedEmail]));
                        return false;
                    }
                }
                return true;
            }
        }
        private class HasValidMobile : SpecificationBase<UserEntry>
        {
            protected override bool IsSatisfyBy(UserEntry data, IList<RuleViolation> violations = null)
            {
                if (!string.IsNullOrEmpty(data.Contact.Mobile))
                {
                    if (data.Contact.Mobile.Length > 50 || data.Contact.Mobile.Length <= 0)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.InvalidMobile, "Mobile", data.Contact.Mobile, ErrorMessage.Messages[ErrorCode.InvalidMobile]));
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
                                violations.Add(new RuleViolation(ErrorCode.InvalidFormatMobile, "Mobile", data.Contact.Mobile, ErrorMessage.Messages[ErrorCode.InvalidFormatMobile]));
                                return false;
                            }

                        }
                        else
                        {
                            var isExisted = UnitOfWork.UserRepository.IsMobileExisted(data.Contact.Mobile);
                            if (isExisted)
                            {
                                if (violations != null)
                                {
                                    violations.Add(new RuleViolation(ErrorCode.ExistedMobile, "Mobile", data.Contact.Mobile, ErrorMessage.Messages[ErrorCode.ExistedMobile]));
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
    }
}
