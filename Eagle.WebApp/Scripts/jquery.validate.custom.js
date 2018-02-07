(function ($) {
    var defaultOptions = {
        validClass: 'has-success',
        errorClass: 'has-error',
        highlight: function (element, errorClass, validClass) {
            $(element).closest(".form-group")
                .removeClass(validClass)
                .addClass('has-error');
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).closest(".form-group")
            .removeClass('has-error')
            .addClass(validClass);
        }
    };
    $.validator.setDefaults(defaultOptions);

    $.validator.unobtrusive.options = {
        errorClass: defaultOptions.errorClass,
        validClass: defaultOptions.validClass
    };

    //$("span.field-validation-valid, span.field-validation-error").addClass('help-block');
    //$("div.form-group").has("span.field-validation-error").addClass('has-error');
    //$("div.validation-summary-errors").has("li:visible").addClass("alert alert-block alert-danger");
});