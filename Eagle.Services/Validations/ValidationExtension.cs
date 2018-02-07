using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Eagle.Common.Extensions;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services.Dtos.Common;

namespace Eagle.Services.Validations
{
    public static class ValidationExtension
    {
        public static string FlattenException(Exception exception)
        {
            var stringBuilder = new StringBuilder();

            while (exception != null)
            {
                stringBuilder.AppendLine(exception.Message);
                stringBuilder.AppendLine(exception.StackTrace);

                exception = exception.InnerException;
            }

            return stringBuilder.ToString();
        }

        public static string ConvertValidateErrorToString(ValidationError ex)
        {
            var errorExtraInfos = ex?.Data["ValidationErrors"] as List<RuleViolation>;
            if (errorExtraInfos == null) return string.Empty;

            StringBuilder sb = new StringBuilder();
            foreach (var error in errorExtraInfos)
            {
                sb.AppendLine($"{error.PropertyName} - {error.ErrorMessage}");
            }
            return sb.ToString();
        }
        public static void AddRuleViolations(this ModelStateDictionary modelState, IEnumerable<RuleViolation> errors)
        {
            foreach (RuleViolation issue in errors)
            {
                modelState.AddModelError(issue.PropertyName, issue.ErrorMessage);
            }
        }
        /// <summary>
        /// Returns a Key/Value pair with all the errors in the model
        /// according to the data annotation properties.
        /// </summary>
        /// <param name="errDictionary"></param>
        /// <returns>
        /// Key: Name of the property
        /// Value: The error message returned from data annotation
        /// </returns>
        public static Dictionary<string, string> GetModelErrors(this ModelStateDictionary errDictionary)
        {
            var errors = new Dictionary<string, string>();
            errDictionary.Where(k => k.Value.Errors.Count > 0).ForEach(i =>
            {
                var er = string.Join(", ", i.Value.Errors.Select(e => e.ErrorMessage).ToArray());
                errors.Add(i.Key, er);
            });
            return errors;
        }
        public static List<Error> GetModelStateErrors(this ModelStateDictionary modelStateDictionary)
        {
            var errors = new List<Error>();
            var modelStateErrors = modelStateDictionary.Values.SelectMany(v => v.Errors).ToList();
            if (modelStateErrors.Any())
            {
                errors.AddRange(modelStateErrors.Select(modelStateError => new Error
                {
                    ErrorMessage = modelStateError.ErrorMessage
                }));
            }
            return errors;
        }
        public static List<Error> GetException(ValidationError ex)
        {
            var errorLst = new List<Error>();
            var errorExtraInfos = ex.Data["ValidationErrors"] as List<RuleViolation>;
            if (errorExtraInfos != null)
            {
                errorLst.AddRange(errorExtraInfos.Select(error => new Error
                {
                    ErrorCode = error.ErrorCode,
                    ErrorMessage = $"{error.PropertyName} - {error.ErrorMessage}",
                    ExtraInfos = new List<RuleViolation>{
                            new RuleViolation(error.ErrorCode, error.PropertyName, error.PropertyValue, error.ErrorMessage)
                    }
                }));
            }
            return errorLst;
        }
        public static void ThrowException(this ControllerBase controllerBase, ValidationError ex,
            AlertMessageType? messageType = AlertMessageType.Error)
        {
            var errorExtraInfos = ex.Data["ValidationErrors"] as List<RuleViolation>;
            if (errorExtraInfos == null) return;

            string cssClass, sortMessage;
            switch (messageType)
            {
                case AlertMessageType.Warning:
                    cssClass = "alert alert-warning";
                    sortMessage = LanguageResource.Warning;
                    break;
                case AlertMessageType.Error:
                    cssClass = "alert alert-danger";
                    sortMessage = LanguageResource.Error;
                    break;
                case AlertMessageType.Info:
                    cssClass = "alert alert-info";
                    sortMessage = LanguageResource.Info;
                    break;
                default:
                    cssClass = "alert alert-danger";
                    sortMessage = LanguageResource.Error;
                    break;
            }

            StringBuilder sb = new StringBuilder();
            foreach (var error in errorExtraInfos)
            {
                if (!string.IsNullOrEmpty(error.ErrorMessage))
                {
                    sb.AppendLine(error.ErrorMessage);
                }
                else
                {
                    sb.AppendLine(error.PropertyValue != null
                       ? $"{error.PropertyName} : {error.PropertyValue}"
                       : $"{error.PropertyName} : {error.ErrorCode}");
                }
            }
            var errorMessage = sb.ToString();
            controllerBase.ViewBag.HasErrorMessage = true;
            controllerBase.ViewBag.AlertCssClass = cssClass + " alert-dismissible show";
            controllerBase.ViewBag.AlertSortMessage = sortMessage;
            controllerBase.ViewBag.AlertMessage = errorMessage;
        }
        public static void ShowException(this ControllerBase controllerBase, ValidationError ex,
            AlertMessageType? messageType = AlertMessageType.Error)
        {
            var errorExtraInfos = ex.Data["ValidationErrors"] as List<RuleViolation>;
            if (errorExtraInfos == null) return;

            string cssClass, sortMessage;
            switch (messageType)
            {
                case AlertMessageType.Warning:
                    cssClass = "alert alert-warning";
                    sortMessage = LanguageResource.Warning;
                    break;
                case AlertMessageType.Error:
                    cssClass = "alert alert-danger";
                    sortMessage = LanguageResource.Error;
                    break;
                case AlertMessageType.Info:
                    cssClass = "alert alert-info";
                    sortMessage = LanguageResource.Info;
                    break;
                default:
                    cssClass = "alert alert-danger";
                    sortMessage = LanguageResource.Error;
                    break;
            }

            StringBuilder sb = new StringBuilder();
            foreach (var error in errorExtraInfos)
            {
                if (!string.IsNullOrEmpty(error.ErrorMessage))
                {
                    sb.AppendLine(error.ErrorMessage);
                }
                else
                {
                    sb.AppendLine(error.PropertyValue != null
                       ? $"{error.PropertyName} : {error.PropertyValue}"
                       : $"{error.PropertyName} : {error.ErrorCode}");
                }
            }
            var errorMessage = sb.ToString();
            controllerBase.ViewBag.DisplayErrorMessage = true;
            controllerBase.ViewBag.CssClass = cssClass + " alert-dismissible show";
            controllerBase.ViewBag.SortMessage = sortMessage;
            controllerBase.ViewBag.Message = errorMessage;
        }
        public static void ShowMessage(this ControllerBase controllerBase, string message, AlertMessageType? messageType = AlertMessageType.Info)
        {
            string cssClass, sortMessage;
            switch (messageType)
            {
                case AlertMessageType.Warning:
                    cssClass = "alert alert-warning";
                    sortMessage = LanguageResource.Warning;
                    break;
                case AlertMessageType.Error:
                    cssClass = "alert alert-danger";
                    sortMessage = LanguageResource.Error;
                    break;
                case AlertMessageType.Info:
                    cssClass = "alert alert-info";
                    sortMessage = LanguageResource.Info;
                    break;
                case AlertMessageType.Success:
                    cssClass = "alert alert-success";
                    sortMessage = LanguageResource.Success;
                    break;
                default:
                    cssClass = "alert alert-info";
                    sortMessage = LanguageResource.Info;
                    break;
            }

            controllerBase.ViewBag.DisplayErrorMessage = true;
            controllerBase.ViewBag.CssClass = cssClass + " alert-dismissible show";
            controllerBase.ViewBag.SortMessage = sortMessage;
            controllerBase.ViewBag.Message = message;
        }
        public static void ShowModelState(this ControllerBase controllerBase, ModelStateDictionary modelState)
        {
            var errorsBuilder = new StringBuilder();
            var errors = modelState.GetModelErrors();
            errors.ForEach(key => errorsBuilder.AppendFormat("{0}: {1} -", key.Key, key.Value));
            var errorMessage = errorsBuilder.ToString();

            controllerBase.ViewBag.DisplayErrorMessage = true;
            controllerBase.ViewBag.CssClass = " alert alert-danger alert-dismissible show";
            controllerBase.ViewBag.SortMessage = LanguageResource.Error;
            controllerBase.ViewBag.Message = errorMessage;
        }


    }
}
