using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Business.Personnel;
using Eagle.Services.Dtos.Business.Roster;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Validations;

namespace Eagle.Services.Business.Validation
{
    public class EmployeeEntryValidator : SpecificationBase<EmployeeEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }
        private PermissionLevel PermissionLevel { get; set; }

        public EmployeeEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel, ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(EmployeeEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullEmployeeEntry, "Employee entry"));
                return false;
            }
          
            //ISpecification<EmployeeEntry> validPermission = new PermissionValidator<EmployeeEntry>(CurrentClaimsIdentity, ModuleDefinition.Company, PermissionLevel.View);
            ISpecification<EmployeeEntry> hasValidCompanyId = new HasValidCompanyId();
            ISpecification<EmployeeEntry> hasValidPositionId = new HasValidPositionId();
            ISpecification<EmployeeEntry> hasValidEmployeeNo = new HasValidEmployeeNo();
            ISpecification<EmployeeEntry> hasValidFirstName = new HasValidFirstName();
            ISpecification<EmployeeEntry> hasValidLastName = new HasValidLastName();
            ISpecification<EmployeeEntry> hasValidEmail = new HasValidEmail();
            ISpecification<EmployeeEntry> hasValidPhone = new HasValidPhone();
            ISpecification<EmployeeEntry> hasValidFax = new HasValidFax();
            ISpecification<EmployeeEntry> hasValidPasswordSalt = new HasValidPasswordSalt();
            ISpecification<EmployeeEntry> hasValiJoinedDate = new HasValiJoinedDate();
            
            var result = hasValidCompanyId.And(hasValidPositionId).And(hasValidEmployeeNo).And(hasValidFirstName).And(hasValidLastName)
                .And(hasValidEmail).And(hasValidPhone).And(hasValidFax).And(hasValidPasswordSalt).And(hasValiJoinedDate).IsSatisfyBy(data, violations);
            return result;
        }

        private class HasValidCompanyId : SpecificationBase<EmployeeEntry>
        {
            protected override bool IsSatisfyBy(EmployeeEntry data, IList<RuleViolation> violations = null)
            {
                if (data.CompanyId > 0)
                {
                    var company = UnitOfWork.CompanyRepository.FindById(data.CompanyId);
                    if (company == null)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.InvalidCompanyId, "CompanyId", data.CompanyId,
                                ErrorMessage.Messages[ErrorCode.InvalidCompanyId]));
                            return false;
                        }
                    }
                }
                else
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.NullCompanyId, "CompanyId", null,
                            ErrorMessage.Messages[ErrorCode.NullCompanyId]));
                        return false;
                    }
                }
                return true;
            }
        }

        private class HasValidPositionId : SpecificationBase<EmployeeEntry>
        {
            protected override bool IsSatisfyBy(EmployeeEntry data, IList<RuleViolation> violations = null)
            {
                if (data.PositionId > 0)
                {
                    var position = UnitOfWork.JobPositionRepository.FindById(data.PositionId);
                    if (position == null)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.InvalidPositionId, "PositionId", data.CompanyId, ErrorMessage.Messages[ErrorCode.InvalidPositionId]));
                            return false;
                        }
                    }
                }
                else
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.NullPositionId, "PositionId", null,
                            ErrorMessage.Messages[ErrorCode.NullPositionId]));
                        return false;
                    }
                }
                return true;
            }
        }

        private class HasValidEmployeeNo : SpecificationBase<EmployeeEntry>
        {
            protected override bool IsSatisfyBy(EmployeeEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.EmployeeNo))
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.NullEmployeeNo, "EmployeeNo", null, ErrorMessage.Messages[ErrorCode.NullEmployeeNo]));
                        return false;
                    }
                }
                else
                {
                    if (data.EmployeeNo.Length > 50)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.InvalidEmployeeNo, "EmployeeNo", data.EmployeeNo, ErrorMessage.Messages[ErrorCode.InvalidEmployeeNo]));
                            return false;
                        }
                    }
                    else
                    {
                        bool isDuplicate = UnitOfWork.EmployeeRepository.HasEmployeeNumberExisted(data.EmployeeNo);
                        if (isDuplicate)
                        {
                            if (violations != null)
                            {
                                violations.Add(new RuleViolation(ErrorCode.DuplicateEmployeeNo, "EmployeeNo", data.EmployeeNo, ErrorMessage.Messages[ErrorCode.InvalidEmployeeNo]));
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
        }

        private class HasValidFirstName : SpecificationBase<EmployeeEntry>
        {
            protected override bool IsSatisfyBy(EmployeeEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.Contact.FirstName))
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.NullFirstName, "FirstName", null, ErrorMessage.Messages[ErrorCode.NullFirstName]));
                        return false;
                    }
                }
                else
                {
                    if (data.Contact.FirstName.Length > 256)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.InvalidFirstName, "FirstName", data.Contact.FirstName, ErrorMessage.Messages[ErrorCode.InvalidFirstName]));
                            return false;
                        }
                    }
                }
                return true;
            }
        }

        private class HasValidLastName : SpecificationBase<EmployeeEntry>
        {
            protected override bool IsSatisfyBy(EmployeeEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.Contact.FirstName))
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.NullLastName, "LastName", null, ErrorMessage.Messages[ErrorCode.NullLastName]));
                        return false;
                    }
                }
                else
                {
                    if (data.Contact.FirstName.Length > 256)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.InvalidLastName, "LastName", data.Contact.LastName, ErrorMessage.Messages[ErrorCode.InvalidLastName]));
                            return false;
                        }
                    }
                }
                return true;
            }
        }

        private class HasValidEmail : SpecificationBase<EmployeeEntry>
        {
            string _validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
            + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
            + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            protected override bool IsSatisfyBy(EmployeeEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.Contact.Email)) return true;
                if (!string.IsNullOrEmpty(data.Contact.Email) && data.Contact.Email.Length > 100)
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidEmail, "Email", data.Contact.Email, ErrorMessage.Messages[ErrorCode.InvalidEmail]));
                        return false;
                    }
                }
                else
                {
                    var regex = new Regex(_validEmailPattern, RegexOptions.IgnoreCase);
                    if (!regex.IsMatch(data.Contact.Email))
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.WrongEmailFormat, "Email", data.Contact.Email, ErrorMessage.Messages[ErrorCode.WrongEmailFormat]));
                            return false;
                        }
                    }
                    else
                    {
                        var isExisted = UnitOfWork.EmployeeRepository.HasEmailExisted(data.Contact.Email);
                        if (isExisted)
                        {
                            if (violations != null)
                            {
                                violations.Add(new RuleViolation(ErrorCode.ExistedEmail, "Email", data.Contact.Email, ErrorMessage.Messages[ErrorCode.ExistedEmail]));
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
        }

        private class HasValidPhone : SpecificationBase<EmployeeEntry>
        {
            protected override bool IsSatisfyBy(EmployeeEntry data, IList<RuleViolation> violations = null)
            {
                if (!string.IsNullOrEmpty(data.Contact.LinePhone1))
                {
                    if (data.Contact.LinePhone1.Length > 50 && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidPhone, "Phone", data.Contact.LinePhone1.Length,
                            ErrorMessage.Messages[ErrorCode.InvalidPhone]));
                        return false;
                    }
                    else
                    {
                        string phone = data.Contact.LinePhone1;
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
                            var isExisted = UnitOfWork.EmployeeRepository.HasPhoneExisted(phone);
                            if (isExisted)
                            {
                                if (violations != null)
                                {
                                    violations.Add(new RuleViolation(ErrorCode.ExistedPhone, "Phone", phone,
                                ErrorMessage.Messages[ErrorCode.ExistedPhone]));
                                    return false;
                                }
                            }
                        }
                    }
                }
               

                if (!string.IsNullOrEmpty(data.Contact.LinePhone2))
                {
                    if (data.Contact.LinePhone2.Length > 50 && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidPhone, "Phone", data.Contact.LinePhone2.Length,
                            ErrorMessage.Messages[ErrorCode.InvalidPhone]));
                        return false;
                    }
                    else
                    {
                        string phone = data.Contact.LinePhone2;
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
                            violations.Add(new RuleViolation(ErrorCode.InvalidPhone, "Phone", phone,
                                ErrorMessage.Messages[ErrorCode.InvalidPhone]));
                        }
                        else
                        {
                            var isExisted = UnitOfWork.EmployeeRepository.HasPhoneExisted(phone);
                            if (isExisted)
                            {
                                if (violations != null)
                                {
                                    violations.Add(new RuleViolation(ErrorCode.ExistedPhone, "Phone", phone,
                                        ErrorMessage.Messages[ErrorCode.ExistedPhone]));
                                    return false;
                                }
                            }
                        }
                    }
                }
                return true;
            }
        }

        private class HasValidFax : SpecificationBase<EmployeeEntry>
        {
            protected override bool IsSatisfyBy(EmployeeEntry data, IList<RuleViolation> violations = null)
            {
                if (!string.IsNullOrEmpty(data.Contact.Fax))
                {
                    string fax = data.Contact.Fax;
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

        internal class HasValidPasswordSalt : SpecificationBase<EmployeeEntry>
        {
            protected override bool IsSatisfyBy(EmployeeEntry data, IList<RuleViolation> violations = null)
            {
                if (!string.IsNullOrEmpty(data.PasswordSalt) && data.PasswordSalt.Length > 128 && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidPasswordSalt, "PasswordSalt", data.PasswordSalt, ErrorMessage.Messages[ErrorCode.InvalidPasswordSalt]));
                    return false;
                }
                
                return true;
            }
        }
        private class HasValiJoinedDate : SpecificationBase<EmployeeEntry>
        {
            protected override bool IsSatisfyBy(EmployeeEntry data, IList<RuleViolation> violations = null)
            {
                if (data.JoinedDate == null && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidJoinedDate, "JoinedDate", data.JoinedDate, ErrorMessage.Messages[ErrorCode.InvalidJoinedDate]));
                    return false;
                }
                else
                {
                    if (data.JoinedDate <= DateTime.MinValue.ToUniversalTime() && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidJoinedDate, "JoinedDate", data.JoinedDate, ErrorMessage.Messages[ErrorCode.InvalidJoinedDate]));
                        return false;
                    }
                }
                return true;
            }
        }

    }
}
