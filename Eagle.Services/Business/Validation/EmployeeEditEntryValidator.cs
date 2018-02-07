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
    public class EmployeeEditEntryValidator : SpecificationBase<EmployeeEditEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }
        private PermissionLevel PermissionLevel { get; set; }

        public EmployeeEditEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel, ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(EmployeeEditEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullEmployeeEditEntry, "Employee entry"));
                return false;
            }

            //ISpecification<EmployeeEditEntry> validPermission = new PermissionValidator<EmployeeEditEntry>(CurrentClaimsIdentity, ModuleDefinition.Company, PermissionLevel.View);
            ISpecification<EmployeeEditEntry> hasValidCompanyId = new HasValidCompanyId();
            ISpecification<EmployeeEditEntry> hasValidPositionId = new HasValidPositionId();
            ISpecification<EmployeeEditEntry> hasValidEmployeeNo = new HasValidEmployeeNo();
            ISpecification<EmployeeEditEntry> hasValidFirstName = new HasValidFirstName();
            ISpecification<EmployeeEditEntry> hasValidLastName = new HasValidLastName();
            ISpecification<EmployeeEditEntry> hasValidEmail = new HasValidEmail();
            ISpecification<EmployeeEditEntry> hasValidPhone = new HasValidPhone();
            ISpecification<EmployeeEditEntry> hasValidFax = new HasValidFax();
            ISpecification<EmployeeEditEntry> hasValidPasswordSalt = new HasValidPasswordSalt();
            ISpecification<EmployeeEditEntry> hasValiJoinedDate = new HasValiJoinedDate();

            var result = hasValidCompanyId.And(hasValidPositionId).And(hasValidEmployeeNo).And(hasValidFirstName).And(hasValidLastName)
                .And(hasValidEmail).And(hasValidPhone).And(hasValidFax).And(hasValidPasswordSalt).And(hasValiJoinedDate).IsSatisfyBy(data, violations);
            return result;
        }

        private class HasValidCompanyId : SpecificationBase<EmployeeEditEntry>
        {
            protected override bool IsSatisfyBy(EmployeeEditEntry data, IList<RuleViolation> violations = null)
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
                return true;
            }
        }

        private class HasValidPositionId : SpecificationBase<EmployeeEditEntry>
        {
            protected override bool IsSatisfyBy(EmployeeEditEntry data, IList<RuleViolation> violations = null)
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
        
        private class HasValidEmployeeNo : SpecificationBase<EmployeeEditEntry>
        {
            protected override bool IsSatisfyBy(EmployeeEditEntry data, IList<RuleViolation> violations = null)
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
                }
                return true;
            }
        }

        private class HasValidFirstName : SpecificationBase<EmployeeEditEntry>
        {
            protected override bool IsSatisfyBy(EmployeeEditEntry data, IList<RuleViolation> violations = null)
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

        private class HasValidLastName : SpecificationBase<EmployeeEditEntry>
        {
            protected override bool IsSatisfyBy(EmployeeEditEntry data, IList<RuleViolation> violations = null)
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

        private class HasValidEmail : SpecificationBase<EmployeeEditEntry>
        {
            string _validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
            + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
            + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            protected override bool IsSatisfyBy(EmployeeEditEntry data, IList<RuleViolation> violations = null)
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
                }
                return true;
            }
        }

        private class HasValidPhone : SpecificationBase<EmployeeEditEntry>
        {
            protected override bool IsSatisfyBy(EmployeeEditEntry data, IList<RuleViolation> violations = null)
            {
                if (!string.IsNullOrEmpty(data.Contact.LinePhone1) && data.Contact.LinePhone1.Length > 50)
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
                }

                if (!string.IsNullOrEmpty(data.Contact.LinePhone2) && data.Contact.LinePhone2.Length > 50)
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
                }
                return true;
            }
        }

        private class HasValidFax : SpecificationBase<EmployeeEditEntry>
        {
            protected override bool IsSatisfyBy(EmployeeEditEntry data, IList<RuleViolation> violations = null)
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

        internal class HasValidPasswordSalt : SpecificationBase<EmployeeEditEntry>
        {
            protected override bool IsSatisfyBy(EmployeeEditEntry data, IList<RuleViolation> violations = null)
            {
                if (!string.IsNullOrEmpty(data.PasswordSalt) && data.PasswordSalt.Length > 128 && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidPasswordSalt, "PasswordSalt", data.PasswordSalt, ErrorMessage.Messages[ErrorCode.InvalidPasswordSalt]));
                    return false;
                }

                return true;
            }
        }
        private class HasValiJoinedDate : SpecificationBase<EmployeeEditEntry>
        {
            protected override bool IsSatisfyBy(EmployeeEditEntry data, IList<RuleViolation> violations = null)
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
