using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Validations;

namespace Eagle.Services.Business.Validation
{
    public class CustomerRegisterEntryValidator : SpecificationBase<CustomerRegisterEntry>
    {
        private static IUnitOfWork UnitOfWork { get; set; }
        private PermissionLevel PermissionLevel { get; set; }

        public CustomerRegisterEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
        }

        protected override bool IsSatisfyBy(CustomerRegisterEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullCustomerRegisterEntry, "Customer entry", null, ErrorMessage.Messages[ErrorCode.InvalidCustomerTypeId]));
                return false;
            }

            ISpecification<CustomerRegisterEntry> hasValidCustomerName = new HasValidCustomerName();
            ISpecification<CustomerRegisterEntry> hasValidEmail = new HasValidEmail();
            ISpecification<CustomerRegisterEntry> hasValidPhone = new HasValidPhone();
            ISpecification<CustomerRegisterEntry> hasValidCountryId = new HasValidCountryId();
            ISpecification<CustomerRegisterEntry> hasValidBirthday = new HasValidBirthday();

            var result = hasValidCustomerName.And(hasValidEmail).And(hasValidPhone).And(hasValidCountryId).And(hasValidBirthday).IsSatisfyBy(data, violations);
            return result;
        }
       
        private class HasValidCustomerName : SpecificationBase<CustomerRegisterEntry>
        {
            protected override bool IsSatisfyBy(CustomerRegisterEntry data, IList<RuleViolation> violations = null)
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

                //if (!string.IsNullOrEmpty(data.FirstName) && !string.IsNullOrEmpty(data.LastName))
                //{
                //    bool isDuplicate = UnitOfWork.CustomerRepository.HasCustomerNameExisted(data.FirstName, data.LastName);
                //    if (isDuplicate)
                //    {
                //        if (violations != null)
                //        {
                //            violations.Add(new RuleViolation(ErrorCode.DuplicateCustomerName, "CustomerName", string.Format("{0} {1}", data.FirstName, data.LastName), ErrorMessage.Messages[ErrorCode.DuplicateCustomerName]));
                //            return false;
                //        }
                //    }
                //}
               
                return true;
            }
        }
        private class HasValidEmail : SpecificationBase<CustomerRegisterEntry>
        {
           static string _validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                                        + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                                        + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            Regex regex = new Regex(_validEmailPattern, RegexOptions.IgnoreCase);

            protected override bool IsSatisfyBy(CustomerRegisterEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.EmailAddress))
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.NullEmail, "Email",null, ErrorMessage.Messages[ErrorCode.NullEmail]));
                        return false;
                    }
                }else if (data.EmailAddress.Length > 150)
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidEmail, "Email", data.EmailAddress, ErrorMessage.Messages[ErrorCode.InvalidEmail]));
                        return false;
                    }
                }
                else if (!regex.IsMatch(data.EmailAddress))
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.WrongEmailFormat, "Email", data.EmailAddress, ErrorMessage.Messages[ErrorCode.WrongEmailFormat]));
                        return false;
                    }
                }
                else
                {
                    var isExisted = UnitOfWork.CustomerRepository.HasEmailExisted(data.EmailAddress);
                    if (isExisted)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.ExistedEmail, "Email", data.EmailAddress, ErrorMessage.Messages[ErrorCode.ExistedEmail]));
                            return false;
                        }
                    }
                }
                return true;
            }
        }

        private class HasValidCountryId : SpecificationBase<CustomerRegisterEntry>
        {
            protected override bool IsSatisfyBy(CustomerRegisterEntry data, IList<RuleViolation> violations = null)
            {
                var entity = UnitOfWork.CountryRepository.FindById(data.Address.CountryId);
                if (entity == null && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidCountryId, "CountryId", data.Address.CountryId, ErrorMessage.Messages[ErrorCode.InvalidCountryId]));
                    return false;
                }
                return true;
            }
        }

        private class HasValidPhone : SpecificationBase<CustomerRegisterEntry>
        {
            protected override bool IsSatisfyBy(CustomerRegisterEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.Phone) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidPhone, "Phone", data.Phone,
                        ErrorMessage.Messages[ErrorCode.InvalidPhone]));
                    return false;
                }
                else
                {
                    if (data.Phone.Length > 50)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.InvalidPhone, "HomePhone", data.Phone, ErrorMessage.Messages[ErrorCode.InvalidPhone]));
                            return false;
                        }
                    }
                    else
                    {
                        string phone = data.Phone;
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
                            var isExisted = UnitOfWork.CustomerRepository.HasPhoneExisted(data.Phone);
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

        private class HasValidBirthday : SpecificationBase<CustomerRegisterEntry>
        {
            protected override bool IsSatisfyBy(CustomerRegisterEntry data, IList<RuleViolation> violations = null)
            {
                if (data.BirthDay == null) return true;

                DateTime selectedDate;
                if (DateTime.TryParse(data.BirthDay.ToString(), out selectedDate))
                {
                    //DateTime rangeDate = DateTime.UtcNow.AddYears(-18);
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
        //private class HasValidAddress : SpecificationBase<CustomerRegisterEntry>
        //{
        //    protected override bool IsSatisfyBy(CustomerRegisterEntry data, IList<RuleViolation> violations = null)
        //    {
        //        if (data.Address != null)
        //        {
        //            var country = UnitOfWork.CountryRepository.FindById(data.Address.CountryId);
        //            if (country == null && violations != null)
        //            {
        //                violations.Add(new RuleViolation(ErrorCode.InvalidCountryId, "CountryId", data.Address.CountryId, ErrorMessage.Messages[ErrorCode.InvalidCountryId]));
        //                return false;
        //            }

        //            var province = UnitOfWork.ProvinceRepository.FindById(data.Address.ProvinceId);
        //            if (province == null && violations != null)
        //            {
        //                violations.Add(new RuleViolation(ErrorCode.InvalidProvinceId, "ProvinceId", data.Address.ProvinceId, ErrorMessage.Messages[ErrorCode.InvalidProvinceId]));
        //                return false;
        //            }

        //            var region = UnitOfWork.RegionRepository.FindById(data.Address.RegionId);
        //            if (region == null && violations != null)
        //            {
        //                violations.Add(new RuleViolation(ErrorCode.InvalidRegionId, "RegionId", data.Address.RegionId, ErrorMessage.Messages[ErrorCode.InvalidRegionId]));
        //                return false;
        //            }
        //        }
        //        return true;
        //    }
        //}

    }
}
