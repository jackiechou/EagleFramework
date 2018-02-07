using System.Web.Mvc;
using Eagle.Core.Attributes;

namespace Eagle.WebApp.Attributes
{
    public class ValidDecimalValidator : DataAnnotationsModelValidator<ValidDecimal>
    {
        public ValidDecimalValidator(ModelMetadata metadata, ControllerContext context, ValidDecimal attribute)
            : base(metadata, context, attribute)
        {
            if (!attribute.IsValid(context.HttpContext.Request.Form[metadata.PropertyName]))
            {
                var propertyName = metadata.PropertyName;
                context.Controller.ViewData.ModelState[propertyName].Errors.Clear();
                context.Controller.ViewData.ModelState[propertyName].Errors.Add(attribute.ErrorMessage);
            }
        }
    }

    public class ValidIntegerValidator : DataAnnotationsModelValidator<ValidInteger>
    {
        public ValidIntegerValidator(ModelMetadata metadata, ControllerContext context, ValidInteger attribute)
            : base(metadata, context, attribute)
        {
            if (!attribute.IsValid(context.HttpContext.Request.Form[metadata.PropertyName]))
            {
                var propertyName = metadata.PropertyName;
                context.Controller.ViewData.ModelState[propertyName].Errors.Clear();
                context.Controller.ViewData.ModelState[propertyName].Errors.Add(attribute.ErrorMessage);
            }
        }
    }
}