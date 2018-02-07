using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Validations;

namespace Eagle.Services.Business.Validation
{
    public class CustomerEntryValidator : SpecificationBase<CustomerEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; }
        private static IUnitOfWork UnitOfWork { get; set; }
        private PermissionLevel PermissionLevel { get; }

        public CustomerEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel, ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(CustomerEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundCustomerEntry, "Customer entry", null, ErrorMessage.Messages[ErrorCode.InvalidCustomerTypeId]));
                return false;
            }

            //ISpecification<CustomerEntry> validPermission = new PermissionValidator<CustomerEntry>(CurrentClaimsIdentity, ModuleDefinition.Company, PermissionLevel.View);
            ISpecification<CustomerEntry> hasValidCustomerTypeId = new HasValidCustomerTypeId();
            ISpecification<CustomerEntry> hasValidCustomerName = new HasValidCustomerName();
            ISpecification<CustomerEntry> hasValidCardNo = new HasValidCardNo();
            ISpecification<CustomerEntry> hasValidEmail = new HasValidEmail();
            ISpecification<CustomerEntry> hasValidHomePhone = new HasValidHomePhone();
            ISpecification<CustomerEntry> hasValidWorkPhone = new HasValidWorkPhone();
            ISpecification<CustomerEntry> hasValidFax = new HasValidFax();
            ISpecification<CustomerEntry> hasValidCustomerNo = new HasValidCustomerNo();
            ISpecification<CustomerEntry> hasValidBirthday = new HasValidBirthday();
            
            var result = hasValidCustomerTypeId.And(hasValidCustomerName).And(hasValidCustomerNo).And(hasValidBirthday)
                .And(hasValidCardNo).And(hasValidEmail).And(hasValidHomePhone).And(hasValidWorkPhone).And(hasValidFax).IsSatisfyBy(data, violations);
            return result;
        }
        private class HasValidCustomerTypeId : SpecificationBase<CustomerEntry>
        {
            protected override bool IsSatisfyBy(CustomerEntry data, IList<RuleViolation> violations = null)
            {
                var customerType = UnitOfWork.CustomerTypeRepository.FindById(data.CustomerTypeId);
                if (customerType == null)
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidCustomerTypeId, "CustomerType", data.CustomerTypeId, ErrorMessage.Messages[ErrorCode.InvalidCustomerTypeId]));
                        return false;
                    }
                }
                return true;
            }
        }
        private class HasValidCustomerName : SpecificationBase<CustomerEntry>
        {
            protected override bool IsSatisfyBy(CustomerEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.FirstName))
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.NullFirstName, "FirstName", null, ErrorMessage.Messages[ErrorCode.NullFirstName]));
                        return false;
                    }
                }
                else
                {
                    if (data.FirstName.Length > 250)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.InvalidFirstName, "FirstName", data.FirstName, ErrorMessage.Messages[ErrorCode.InvalidFirstName]));
                            return false;
                        }
                    }
                }

                if (string.IsNullOrEmpty(data.LastName))
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.NullLastName, "LastName", null, ErrorMessage.Messages[ErrorCode.NullLastName]));
                        return false;
                    }
                }
                else
                {
                    if (data.LastName.Length > 250)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.InvalidLastName, "LastName", data.LastName, ErrorMessage.Messages[ErrorCode.InvalidLastName]));
                            return false;
                        }
                    }
                }
                return true;
            }
        }
        private class HasValidCustomerNo : SpecificationBase<CustomerEntry>
        {
            protected override bool IsSatisfyBy(CustomerEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.CustomerNo))
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.NullCustomerNo, "CustomerNo",null, ErrorMessage.Messages[ErrorCode.NullCustomerNo]));
                        return false;
                    }
                }
                else
                {
                    if (data.CustomerNo.Length > 50)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.InvalidCustomerNo, "CustomerNo", data.CustomerNo, ErrorMessage.Messages[ErrorCode.InvalidCustomerNo]));
                            return false;
                        }
                    }
                    else
                    {
                        bool isDuplicate = UnitOfWork.CustomerRepository.HasCustomerNumberExisted(data.CustomerNo);
                        if (isDuplicate)
                        {
                            if (violations != null)
                            {
                                violations.Add(new RuleViolation(ErrorCode.DuplicateCustomerNo, "CustomerNo", data.CustomerNo, ErrorMessage.Messages[ErrorCode.DuplicateCustomerNo]));
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
        }
        private class HasValidCardNo : SpecificationBase<CustomerEntry>
        {
            protected override bool IsSatisfyBy(CustomerEntry data, IList<RuleViolation> violations = null)
            {
                if (!string.IsNullOrEmpty(data.CardNo) && data.CardNo.Length > 20)
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidCardNo, "CardNo", data.CardNo, ErrorMessage.Messages[ErrorCode.InvalidCardNo]));
                        return false;
                    }
                }
                return true;
            }
        }
        private class HasValidEmail : SpecificationBase<CustomerEntry>
        {
            static string _validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                                               + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                                               + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            Regex _regex = new Regex(_validEmailPattern, RegexOptions.IgnoreCase);

            protected override bool IsSatisfyBy(CustomerEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.Email))
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.NullEmail, "Email", null, ErrorMessage.Messages[ErrorCode.NullEmail]));
                        return false;
                    }
                }
                else if (data.Email.Length > 150)
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidEmail, "Email", data.Email, ErrorMessage.Messages[ErrorCode.InvalidEmail]));
                        return false;
                    }
                }
                else if (!_regex.IsMatch(data.Email))
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.WrongEmailFormat, "Email", data.Email, ErrorMessage.Messages[ErrorCode.WrongEmailFormat]));
                        return false;
                    }
                }
                else
                {
                    var isExisted = UnitOfWork.CustomerRepository.HasEmailExisted(data.Email);
                    if (isExisted)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.ExistedEmail, "Email", data.Email, ErrorMessage.Messages[ErrorCode.ExistedEmail]));
                            return false;
                        }
                    }
                }
                return true;
            }
        }
        private class HasValidHomePhone : SpecificationBase<CustomerEntry>
        {
            protected override bool IsSatisfyBy(CustomerEntry data, IList<RuleViolation> violations = null)
            {
                if (!string.IsNullOrEmpty(data.HomePhone))
                {
                    if (data.HomePhone.Length > 50)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.InvalidPhone, "HomePhone", data.HomePhone, ErrorMessage.Messages[ErrorCode.InvalidPhone]));
                            return false;
                        }
                    }
                    else
                    {
                        string phone = data.HomePhone;
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

                        if (phone.Length > 0 && violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.InvalidPhone, "Phone", phone, ErrorMessage.Messages[ErrorCode.InvalidPhone]));
                            return false;
                        }
                        else
                        {
                            var isExisted = UnitOfWork.CustomerRepository.HasPhoneExisted(data.HomePhone);
                            if (isExisted && violations != null)
                            {
                                violations.Add(new RuleViolation(ErrorCode.ExistedHomePhone, "HomePhone", phone, ErrorMessage.Messages[ErrorCode.ExistedHomePhone]));
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
        }
        private class HasValidWorkPhone : SpecificationBase<CustomerEntry>
        {
            protected override bool IsSatisfyBy(CustomerEntry data, IList<RuleViolation> violations = null)
            {
                if (!string.IsNullOrEmpty(data.WorkPhone))
                {
                    if (data.WorkPhone.Length > 50)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.InvalidPhone, "Phone", data.WorkPhone, ErrorMessage.Messages[ErrorCode.InvalidPhone]));
                            return false;
                        }
                    }
                    else
                    {
                        string phone = data.WorkPhone;
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

                        if (phone.Length > 0 && violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.InvalidPhone, "Phone", phone, ErrorMessage.Messages[ErrorCode.InvalidPhone]));
                            return false;
                        }
                        else
                        {
                            var isExisted = UnitOfWork.CustomerRepository.HasPhoneExisted(data.WorkPhone);
                            if (isExisted && violations != null)
                            {
                                violations.Add(new RuleViolation(ErrorCode.ExistedPhone, "Phone", phone, ErrorMessage.Messages[ErrorCode.ExistedPhone]));
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
        }
        private class HasValidFax : SpecificationBase<CustomerEntry>
        {
            protected override bool IsSatisfyBy(CustomerEntry data, IList<RuleViolation> violations = null)
            {
                if (!string.IsNullOrEmpty(data.Fax))
                {
                    string fax = data.Fax;
                    fax = fax.Replace("0", "");
                    fax = fax.Replace("1", "");
                    fax = fax.Replace("2", "");
                    fax = fax.Replace("3", "");
                    fax = fax.Replace("4", "");
                    fax = fax.Replace("5", "");
                    fax = fax.Replace("6", "");
                    fax = fax.Replace("7", "");
                    fax = fax.Replace("8", "");
                    fax = fax.Replace("9", "");
                    fax = fax.Replace("(", "");
                    fax = fax.Replace(")", "");
                    fax = fax.Replace("+", "");
                    fax = fax.Replace("-", "");

                    if (fax.Length > 0 && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidFax, "Fax", fax, ErrorMessage.Messages[ErrorCode.ExistedPhone]));
                        return false;
                    }
                }

                return true;
            }
        }
        private class HasValidBirthday : SpecificationBase<CustomerEntry>
        {
            protected override bool IsSatisfyBy(CustomerEntry data, IList<RuleViolation> violations = null)
            {
                DateTime selectedDate;
                if (DateTime.TryParse(data.BirthDay.ToString(), out selectedDate))
                {
                    //DateTime rangeDate = DateTime.UtcNow.AddYears(-100);
                    //DateTime rangeDate = DateTime.UtcNow.AddYears(-Math.Abs(18));
                    DateTime minDate = new DateTime(1900, 1, 1);
                    
                    if (selectedDate.CompareTo(minDate) <= 0)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.BirthdayMustBeGreaterThanMinDate, "Birthday",
                                data.BirthDay, ErrorMessage.Messages[ErrorCode.BirthdayMustBeGreaterThanMinDate]));
                            return false;
                        }
                    }
                    else
                    {
                        if (selectedDate >= DateTime.UtcNow)
                        {
                            if (violations != null)
                            {
                                violations.Add(new RuleViolation(ErrorCode.InvalidBirthday, "Birthday", data.BirthDay,
                                    ErrorMessage.Messages[ErrorCode.InvalidBirthday]));
                                return false;
                            }
                        }
                    }
                }
                else
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidBirthday, "Birthday", data.BirthDay,
                            ErrorMessage.Messages[ErrorCode.InvalidBirthday]));
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
