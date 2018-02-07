using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace Eagle.Services.Dtos.Validation
{
    /// <summary>
    /// Checks if the value is not null and is a bool and is equal to true
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MustBeTrue : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return value != null && value is bool && (bool)value;
        }
    }

    /// <summary>
    /// Checks if the value is equal to null or is an empty string
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MustBeEmpty : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return value == null || (value is string && (string)value == "");
        }
    }

    /// <summary>
    /// Checks if the value is not null and is an integer which is greater than zero
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class GreaterThanZero : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return value != null && value is int && (int)value > 0;
        }
    }

    /// <summary>
    /// Checks if their 18th birthday is not in future.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class OverEighteenYearsOld : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return value != null && value is DateTime && ((DateTime)value).AddYears(18).Date <= DateTime.UtcNow.Date;
        }
    }

    /// <summary>
    /// Checks if it the string is a valid UK date
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ValidStringAsUkDate : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime testDate;
            return value == null || (value is string &&
                   DateTime.TryParse((string)value, new System.Globalization.CultureInfo("en-GB"),
                       System.Globalization.DateTimeStyles.None, out testDate));
        }
    }

    ///<summary>
    ///Compares two dates to each other, ensuring that one is larger than the other
    ///[CompareNumbers("EndQuantity", ErrorMessage = "Min cannot be more than max")]
    /// public int BeginQuantity { get; set; }
    ///[CompareNumbers("BeginQuantity", ErrorMessage = "Min cannot be more than max")]
    /// public int EndQuantity { get; set; }
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CompareNumbersAttribute : ValidationAttribute, IClientValidatable
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="CompareNumbersAttribute"/> class.
        /// </summary>
        /// <param name="otherPropertyName">Name of the compare to date property.</param>
        /// <param name="allowEquality">if set to <c>true</c> equal dates are allowed.</param>
        public CompareNumbersAttribute(string otherPropertyName, bool allowEquality = true)
        {
            AllowEquality = allowEquality;
            OtherPropertyName = otherPropertyName;
        }

        #region Properties

        /// <summary>
        /// Gets the name of the  property to compare to
        /// </summary>
        public string OtherPropertyName { get; private set; }

        /// <summary>
        /// Gets a value indicating whether dates could be the same
        /// </summary>
        public bool AllowEquality { get; private set; }


        #endregion

        /// <summary>
        /// Validates the specified value with respect to the current validation attribute.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>
        /// An instance of the <see cref="T:System.ComponentModel.DataAnnotations.ValidationResult"/> class.
        /// </returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var result = ValidationResult.Success;
            var otherValue = validationContext.ObjectType.GetProperty(OtherPropertyName).GetValue(validationContext.ObjectInstance, null);
            if (value != null)
            {
                decimal currentDecimalValue;
                if (decimal.TryParse(value.ToString(), out currentDecimalValue))
                {

                    if (otherValue != null)
                    {
                        decimal otherDecimalValue;
                        if (decimal.TryParse(otherValue.ToString(), out otherDecimalValue))
                        {
                            if (!OtherPropertyName.ToLower().Contains("begin"))
                            {
                                if (currentDecimalValue > otherDecimalValue)
                                {
                                    result = new ValidationResult(ErrorMessage);
                                }
                            }
                            else
                            {
                                if (currentDecimalValue < otherDecimalValue)
                                {
                                    result = new ValidationResult(ErrorMessage);
                                }
                            }
                            if (currentDecimalValue == otherDecimalValue && !AllowEquality)
                            {
                                result = new ValidationResult(ErrorMessage);
                            }
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// When implemented in a class, returns client validation rules for that class.
        /// </summary>
        /// <param name="metadata">The model metadata.</param>
        /// <param name="context">The controller context.</param>
        /// <returns>
        /// The client validation rules for this validator.
        /// </returns>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ErrorMessage,
                ValidationType = "comparenumbers"
            };
            rule.ValidationParameters["otherpropertyname"] = OtherPropertyName;
            rule.ValidationParameters["allowequality"] = AllowEquality ? "true" : "";
            yield return rule;
        }
    }

    /// <summary>
    /// Compares two dates to each other, ensuring that one is larger than the other
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CompareDatesAttribute : ValidationAttribute, IClientValidatable
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="CompareDatesAttribute"/> class.
        /// </summary>
        /// <param name="otherPropertyName">Name of the compare to date property.</param>
        /// <param name="allowEquality">if set to <c>true</c> equal dates are allowed.</param>
        public CompareDatesAttribute(string otherPropertyName, bool allowEquality = true)
        {
            AllowEquality = allowEquality;
            OtherPropertyName = otherPropertyName;
        }

        #region Properties

        /// <summary>
        /// Gets the name of the  property to compare to
        /// </summary>
        public string OtherPropertyName { get; private set; }

        /// <summary>
        /// Gets a value indicating whether dates could be the same
        /// </summary>
        public bool AllowEquality { get; private set; }


        #endregion

        /// <summary>
        /// Validates the specified value with respect to the current validation attribute.
        ///[Display(Name = "Start Date")]
        //[DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        //[CompareDates("EndDate", ErrorMessage = "Start date cannot be before end date")]
        //public DateTime? BeginDate { get; set; }
        //[Display(Name = "End Date")]
        //[DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        //[CompareDates("BeginDate", ErrorMessage = "Start date cannot be before end date")]
        //public DateTime? EndDate { get; set; }
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>
        /// An instance of the <see cref="T:System.ComponentModel.DataAnnotations.ValidationResult"/> class.
        /// </returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var result = ValidationResult.Success;
            var otherValue = validationContext.ObjectType.GetProperty(OtherPropertyName)
                .GetValue(validationContext.ObjectInstance, null);
            if (value != null)
            {
                if (value is DateTime)
                {

                    if (otherValue != null)
                    {
                        if (otherValue is DateTime)
                        {
                            if (!OtherPropertyName.ToLower().Contains("begin"))
                            {
                                if ((DateTime)value > (DateTime)otherValue)
                                {
                                    result = new ValidationResult(ErrorMessage);
                                }
                            }
                            else
                            {
                                if ((DateTime)value < (DateTime)otherValue)
                                {
                                    result = new ValidationResult(ErrorMessage);
                                }
                            }
                            if ((DateTime)value == (DateTime)otherValue && !AllowEquality)
                            {
                                result = new ValidationResult(ErrorMessage);
                            }
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// When implemented in a class, returns client validation rules for that class.
        /// </summary>
        /// <param name="metadata">The model metadata.</param>
        /// <param name="context">The controller context.</param>
        /// <returns>
        /// The client validation rules for this validator.
        /// </returns>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ErrorMessage,
                ValidationType = "comparedates"
            };
            rule.ValidationParameters["otherpropertyname"] = OtherPropertyName;
            rule.ValidationParameters["allowequality"] = AllowEquality ? "true" : "";
            yield return rule;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ValidBirthDate : ValidationAttribute, IClientValidatable
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime birthJoin = Convert.ToDateTime(value);
                if (birthJoin > DateTime.UtcNow)
                {
                    return new ValidationResult("Birth date can not be greater than current date.");
                }
            }
            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule mvr = new ModelClientValidationRule
            {
                ErrorMessage = "Birth Date can not be greater than current date",
                ValidationType = "validbirthdate"
            };
            return new[] { mvr };
        }
    }

    /// <summary>
    /// [MinimumElements(1, ErrorMessage = "At least a person is required")]
    /// public List<YourClass/> YourClassList { get; private set; }
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MinimumElementsAttribute : ValidationAttribute
    {
        private readonly int _minElements;
        public MinimumElementsAttribute(int minElements)
        {
            _minElements = minElements;
        }

        public override bool IsValid(object value)
        {
            var list = value as IList;
            if (list != null)
            {
                return list.Count >= _minElements;
            }
            return false;
        }
    }

    /// <summary>
    /// [ListRequired(ErrorMessage = "Required.")]
    /// public IEnumerable<YourClass/> YourClassList { get; set; }
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ListRequiredAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            var list = value as IEnumerable;
            return list != null && list.GetEnumerator().MoveNext();
        }
    }


    /// <summary>
    /// [CheckBoxListRequired(ErrorMessage = "Required.")]
    /// public SelectList CheckBoxRequiredList { get; set; }
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class CheckBoxListRequiredAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            bool result = false;

            var list = value as SelectList;
            if (list != null && list.GetEnumerator().MoveNext())
            {
                if (list.Any(item => item.Selected))
                {
                    result = true;
                }
            }

            return result;
        }
    }


}
