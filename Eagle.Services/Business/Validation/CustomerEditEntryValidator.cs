using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Eagle.Core.Configuration;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Validations;

namespace Eagle.Services.Business.Validation
{
    public class CustomerEditEntryValidator : SpecificationBase<CustomerEditEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }
        private PermissionLevel PermissionLevel { get; set; }

        public CustomerEditEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel, ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(CustomerEditEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundCustomerEditEntry, "Customer entry", null, ErrorMessage.Messages[ErrorCode.NotFoundCustomerEditEntry]));
                return false;
            }

            //ISpecification<CustomerEditEntry> validPermission = new PermissionValidator<CustomerEditEntry>(CurrentClaimsIdentity, ModuleDefinition.Company, PermissionLevel.View);
            ISpecification<CustomerEditEntry> hasValidCustomerTypeId = new HasValidCustomerTypeId();
            ISpecification<CustomerEditEntry> hasValidCustomerName = new HasValidCustomerName();
            ISpecification<CustomerEditEntry> hasValidCardNo = new HasValidCardNo();
            ISpecification<CustomerEditEntry> hasValidEmail = new HasValidEmail();
            ISpecification<CustomerEditEntry> hasValidHomePhone = new HasValidHomePhone();
            ISpecification<CustomerEditEntry> hasValidWorkPhone = new HasValidWorkPhone();
            ISpecification<CustomerEditEntry> hasValidFax = new HasValidFax();
            ISpecification<CustomerEditEntry> hasValidBirthday = new HasValidBirthday();


            var result = hasValidCustomerTypeId.And(hasValidCustomerName).And(hasValidBirthday).And(hasValidCardNo)
                .And(hasValidEmail).And(hasValidHomePhone).And(hasValidWorkPhone).And(hasValidFax).IsSatisfyBy(data, violations);
            return result;
        }
        private class HasValidCustomerTypeId : SpecificationBase<CustomerEditEntry>
        {
            protected override bool IsSatisfyBy(CustomerEditEntry data, IList<RuleViolation> violations = null)
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
        private class HasValidCustomerName : SpecificationBase<CustomerEditEntry>
        {
            protected override bool IsSatisfyBy(CustomerEditEntry data, IList<RuleViolation> violations = null)
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
                            violations.Add(new RuleViolation(ErrorCode.InvalidLastName, "LastName", data.FirstName, ErrorMessage.Messages[ErrorCode.InvalidLastName]));
                            return false;
                        }
                    }
                }
                return true;
            }
        }
        private class HasValidCardNo : SpecificationBase<CustomerEditEntry>
        {
            protected override bool IsSatisfyBy(CustomerEditEntry data, IList<RuleViolation> violations = null)
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
        private class HasValidEmail : SpecificationBase<CustomerEditEntry>
        {
            string _validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
            + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
            + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            protected override bool IsSatisfyBy(CustomerEditEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.Email)) return true;
                if (!string.IsNullOrEmpty(data.Email) && data.Email.Length > 100)
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidEmail, "Email", data.Email, ErrorMessage.Messages[ErrorCode.InvalidEmail]));
                        return false;
                    }
                }
                else
                {
                    var regex = new Regex(_validEmailPattern, RegexOptions.IgnoreCase);
                    if (!regex.IsMatch(data.Email))
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.WrongEmailFormat, "Email", data.Email, ErrorMessage.Messages[ErrorCode.WrongEmailFormat]));
                            return false;
                        }
                    }
                }
                return true;
            }
        }
        private class HasValidHomePhone : SpecificationBase<CustomerEditEntry>
        {
            protected override bool IsSatisfyBy(CustomerEditEntry data, IList<RuleViolation> violations = null)
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
                    }
                }
                return true;
            }
        }
        private class HasValidWorkPhone : SpecificationBase<CustomerEditEntry>
        {
            protected override bool IsSatisfyBy(CustomerEditEntry data, IList<RuleViolation> violations = null)
            {
                if (!string.IsNullOrEmpty(data.WorkPhone))
                {
                    if (data.WorkPhone.Length > 50 && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidPhone, "Phone", data.WorkPhone, ErrorMessage.Messages[ErrorCode.InvalidPhone]));
                        return false;
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
                    }
                  
                }

                return true;
            }
        }
        private class HasValidFax : SpecificationBase<CustomerEditEntry>
        {
            protected override bool IsSatisfyBy(CustomerEditEntry data, IList<RuleViolation> violations = null)
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
                        violations.Add(new RuleViolation(ErrorCode.InvalidFax, "Fax", fax, ErrorMessage.Messages[ErrorCode.InvalidFax]));
                        return false;
                    }
                }

                return true;
            }
       }
        private class HasValidBirthday : SpecificationBase<CustomerEditEntry>
        {
            protected override bool IsSatisfyBy(CustomerEditEntry data, IList<RuleViolation> violations = null)
            {
                DateTime selectedDate;
                if (DateTime.TryParse(data.BirthDay.ToString(), out selectedDate))
                {
                    DateTime rangeDate = DateTime.UtcNow.AddYears(-100);
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
                        if (selectedDate.CompareTo(rangeDate) < 0)
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
