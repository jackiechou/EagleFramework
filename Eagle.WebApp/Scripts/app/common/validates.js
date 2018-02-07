/****************************************************************
FILE: RegExpValidate.js

DESCRIPTION: This file contains a library of validation functions
  using javascript regular expressions.  Library also contains 
  functions that reformat fields for display or for storage.


  VALIDATION FUNCTIONS:

  validateEmail - checks format of email address
    validateUSPhone - checks format of US phone number
    validateNumeric - checks for valid numeric value
  validateInteger - checks for valid integer value
    validateNotEmpty - checks for blank form field
  validateUSZip - checks for valid US zip code
  validateUSDate - checks for valid date in US format
  validateValue - checks a string against supplied pattern

  FORMAT FUNCTIONS:

  rightTrim - removes trailing spaces from a string
  leftTrim - removes leading spaces from a string
  trimAll - removes leading and trailing spaces from a string
  removeCurrency - removes currency formatting characters (), $
  addCurrency - inserts currency formatting characters
  removeCommas - removes comma separators from a number
  addCommas - adds comma separators to a number
  removeCharacters - removes characters from a string that match 
  passed pattern


  Common expressions

Date
   /^\d{1,2}(\-|\/|\.)\d{1,2}\1\d{4}$/     mm/dd/yyyy
   
US zip code
  /(^\d{5}$)|(^\d{5}-\d{4}$)/             99999 or 99999-9999
  
Canadian postal code
  /^\D{1}\d{1}\D{1}\-?\d{1}\D{1}\d{1}$/   Z5Z-5Z5 orZ5Z5Z5
  
Time
  /^([1-9]|1[0-2]):[0-5]\d(:[0-5]\d(\.\d{1,3})?)?$/   HH:MM or HH:MM:SS or HH:MM:SS.mmm
  
IP Address(no check for alid values (0-255))
  /^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$/ 999.999.999.999
  
Dollar Amount
  /^((\$\d*)|(\$\d*\.\d{2})|(\d*)|(\d*\.\d{2}))$/ 100, 100.00, $100 or $100.00
  
Social Security Number
  /^\d{3}\-?\d{2}\-?\d{4}$/   999-99-9999 or999999999
  
Canadian Social Insurance Number
  /^\d{9}$/ 999999999



AUTHOR: Karen Gayda

DATE: 03/24/2000
*******************************************************************/

//Very import function
function setupValidation() {
    $('.input-validation-error').parents('.form-group').addClass('has-error');
    $('.field-validation-error').addClass('text-danger');

    $("span.field-validation-valid, span.field-validation-error").addClass('help-block');
    $("div.form-group").has("span.field-validation-error").addClass('has-error');
    $("div.validation-summary-errors").has("li:visible").addClass("alert alert-block alert-danger");

    window.customValidation = window.customValidation || 
   {
       relatedControlValidationCalled: function (event) {
           if (!customValidation.activeValidator) {
               customValidation.formValidator = $(event.data.source).closest('form').data('validator');
           }
           customValidation.formValidator.element($(event.data.target));
       },
       relatedControlCollection: [],
       formValidator: undefined,
       addDependatControlValidaitonHandler: function (element, dependentPropertyName) {
           var id = $(element).attr('id');
           if ($.inArray(id, customValidation.relatedControlCollection) < 0) {
               customValidation.relatedControlCollection.push(id);
               $(element).on('blur', { source: $(element), target: $('#' + dependentPropertyName) }, customValidation.relatedControlValidationCalled);
           }
       }
   };

    //$.validator.unobtrusive.parseDynamicContent('[select to the target element for your jax post-back]');
    $.validator.unobtrusive.parseDynamicContent = function (selector) {
        //use the normal unobstrusive.parse method
        $.validator.unobtrusive.parse(selector);

        //get the relevant form
        var form = $(selector).first().closest('form');

        //Modify 2 new lines  - R remove the validator data so that the validator picks up new validation.
        form.removeData('validator'); /* added by the raw jquery.validate plugin */
        form.removeData("unobtrusiveValidation"); /* added by the jquery unobtrusive plugin*/
        //form.data("unobtrusiveValidation", null);
        //form.data("validator", null);

        //get the collections of unobstrusive validators, and jquery validators
        //and compare the two
        var unobtrusiveValidation = form.data('unobtrusiveValidation');
        var validator = form.validate();

        $.each(unobtrusiveValidation.options.rules, function (elname, elrules) {
            if (validator.settings.rules[elname] === undefined) {
                var args = {};
                $.extend(args, elrules);
                args.messages = unobtrusiveValidation.options.messages[elname];
                //edit:use quoted strings for the name selector
                $("[name='" + elname + "']").rules("add", args);
            } else {
                $.each(elrules, function (rulename, data) {
                    if (validator.settings.rules[elname][rulename] === undefined) {
                        var args = {};
                        args[rulename] = data;
                        args.messages = unobtrusiveValidation.options.messages[elname][rulename];
                        //edit:use quoted strings for the name selector
                        $("[name='" + elname + "']").rules("add", args);
                    }
                });
            }
        });
    }
    
    $.validator.unobtrusive.adapters.add('comparedates', ['otherpropertyname', 'allowequality'], function (options) {
        options.rules['comparedates'] = options.params;
        if (options.message) {
            options.messages['comparedates'] = options.message;
        }
    });
    $.validator.addMethod('comparedates', function (value, element, params) {
        var otherFieldValue = $('input[name="' + params.otherpropertyname + '"]').val();
        if (otherFieldValue && value) {
            var currentValue = Date.parse(value);
            var otherValue = Date.parse(otherFieldValue);
            if ($(element).attr('name').toLowerCase().indexOf('begin') >= 0) {
                if (params.allowequality) {
                    if (currentValue > otherValue) {
                        return false;
                    }
                } else {
                    if (currentValue >= otherValue) {
                        return false;
                    }
                }
            } else {
                if (params.allowequality) {
                    if (currentValue < otherValue) {
                        return false;
                    }
                } else {
                    if (currentValue <= otherValue) {
                        return false;
                    }
                }
            }
        }
        customValidation.addDependatControlValidaitonHandler(element, params.otherpropertyname);
        return true;
    }, '');

    $.validator.unobtrusive.adapters.add('comparenumbers', ['otherpropertyname', 'allowequality'], function (options) {
        options.rules['comparenumbers'] = options.params;
        if (options.message) {
            options.messages['comparenumbers'] = options.message;
        }
    });
    $.validator.addMethod('comparenumbers', function (value, element, params) {
        var otherFieldValue = $('input[name="' + params.otherpropertyname + '"]').val();
        if (otherFieldValue && value) {
            var currentValue = parseFloat(value);
            var otherValue = parseFloat(otherFieldValue);
            if ($(element).attr('name').toLowerCase().indexOf('begin') >= 0) {
                if (params.allowequality) {
                    if (currentValue > otherValue) {
                        return false;
                    }
                } else {
                    if (currentValue >= otherValue) {
                        return false;
                    }
                }
            } else {
                if (params.allowequality) {
                    if (currentValue < otherValue) {
                        return false;
                    };
                } else {
                    if (currentValue <= otherValue) {
                        return false;
                    };
                }
            }
        }
        window.customValidation.addDependatControlValidaitonHandler(element, params.otherpropertyname);
        return true;
    }, '');

    $.validator.addMethod('validbirthdate', function (value, element, params) {
        var currentDate = new Date();
        if (Date.parse(value) > currentDate) {
            return false;
        }
        return true;
    }, '');
    $.validator.unobtrusive.adapters.add('validbirthdate', function (options) {
        options.rules['validbirthdate'] = {};
        options.messages['validbirthdate'] = options.message;
    });

    $.validator.methods["date"] = function (value, element) { return true; };
    $.validator.addMethod('accept', function () { return true; });
    $.validator.setDefaults({ ignore: '' });
}

function validateEmail(strValue) {
    /************************************************
    DESCRIPTION: Validates that a string contains a
      valid email pattern.
    
     PARAMETERS:
       strValue - String to be tested for validity
    
    RETURNS:
       True if valid, otherwise false.
    
    REMARKS: Accounts for email with country appended
      does not validate that email contains valid URL
      type (.com, .gov, etc.) or valid country suffix.
    *************************************************/
    var objRegExp =
     /(^[a-z]([a-z_\.]*)@([a-z_\.]*)([.][a-z]{3})$)|(^[a-z]([a-z_\.]*)@([a-z_\.]*)(\.[a-z]{3})(\.[a-z]{2})*$)/i;
    //var objRegExp = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
    if (strValue.indexOf(" ") > -1)
        return false;

    //check for valid email
    return objRegExp.test(strValue);
}

function validateUSPhone(strValue) {
    /************************************************
    DESCRIPTION: Validates that a string contains valid
      US phone pattern.
      Ex. (999) 999-9999 or (999)999-9999
    
    PARAMETERS:
       strValue - String to be tested for validity
    
    RETURNS:
       True if valid, otherwise false.
    *************************************************/
    var objRegExp = /^\([1-9]\d{2}\)\s?\d{3}\-\d{4}$/;

    //check for valid us phone with or without space between
    //area code
    return objRegExp.test(strValue);
}

// Validation Phone: Chi duoc nhap So, Dau Gach.
function validatePhone(x) {
    var validateChar = /^[0-9\-]+$/;

    if (x.trim() === "") {
        return true;
    }

    if (x.match(validateChar)) {
        return true;
    }
    else {
        return false;
    }
}

function validateTelephone(x) {
    var validateChar = /^[0-9\-\(\)]+$/;

    if (x.trim() === "") {
        return true;
    }

    if (x.match(validateChar)) {
        return true;
    }
    else {
        return false;
    }
}

function validatePhoneLength(oSrc, args) {
    args.IsValid = (args.Value.length >= 8);
}

function validateNumeric(strValue) {
    /*****************************************************************
    DESCRIPTION: Validates that a string contains only valid numbers.
    
    PARAMETERS:
       strValue - String to be tested for validity
    
    RETURNS:
       True if valid, otherwise false.
    ******************************************************************/
    var objRegExp = /(^-?\d\d*\.\d*$)|(^-?\d\d*$)|(^-?\.\d\d*$)/;

    //check for numeric characters
    return objRegExp.test(strValue);
}

function validateInteger(strValue) {
    /************************************************
    DESCRIPTION: Validates that a string contains only
        valid integer number.
    
    PARAMETERS:
       strValue - String to be tested for validity
    
    RETURNS:
       True if valid, otherwise false.
    **************************************************/
    var objRegExp = /(^-?\d\d*$)/;

    //check for integer characters
    return objRegExp.test(strValue);
}

function validateNotEmpty(strValue) {
    /************************************************
    DESCRIPTION: Validates that a string is not all
      blank (whitespace) characters.
    
    PARAMETERS:
       strValue - String to be tested for validity
    
    RETURNS:
       True if valid, otherwise false.
    *************************************************/
    var strTemp = strValue;
    strTemp = trimAll(strTemp);
    if (strTemp.length > 0) {
        return true;
    }
    return false;
}

function validateUSZip(strValue) {
    /************************************************
    DESCRIPTION: Validates that a string a United
      States zip code in 5 digit format or zip+4
      format. 99999 or 99999-9999
    
    PARAMETERS:
       strValue - String to be tested for validity
    
    RETURNS:
       True if valid, otherwise false.
    
    *************************************************/
    var objRegExp = /(^\d{5}$)|(^\d{5}-\d{4}$)/;

    //check for valid US Zipcode
    return objRegExp.test(strValue);
}

function validateUSDate(strValue) {
    /************************************************
    DESCRIPTION: Validates that a string contains only
        valid dates with 2 digit month, 2 digit day,
        4 digit year. Date separator can be ., -, or /.
        Uses combination of regular expressions and
        string parsing to validate date.
        Ex. mm/dd/yyyy or mm-dd-yyyy or mm.dd.yyyy
    
    PARAMETERS:
       strValue - String to be tested for validity
    
    RETURNS:
       True if valid, otherwise false.
    
    REMARKS:
       Avoids some of the limitations of the Date.parse()
       method such as the date separator character.
    *************************************************/
    var objRegExp = /^\d{1,2}(\-|\/|\.)\d{1,2}\1\d{4}$/;

    //check to see if in correct format
    if (!objRegExp.test(strValue))
        return false; //doesn't match pattern, bad date
    else {
        var strSeparator = strValue.substring(2, 3);
        var arrayDate = strValue.split(strSeparator);
        //create a lookup for months not equal to Feb.
        var arrayLookup = {
            '01': 31,
            '03': 31,
            '04': 30,
            '05': 31,
            '06': 30,
            '07': 31,
            '08': 31,
            '09': 30,
            '10': 31,
            '11': 30,
            '12': 31
        };
        var intDay = parseInt(arrayDate[1], 10);

        //check if month value and day value agree
        if (arrayLookup[arrayDate[0]] !== null) {
            if (intDay <= arrayLookup[arrayDate[0]] && intDay !== 0)
                return true; //found in lookup table, good date
        }

        //check for February (bugfix 20050322)
        //bugfix  for parseInt kevin
        //bugfix  biss year  O.Jp Voutat
        var intMonth = parseInt(arrayDate[0], 10);
        if (intMonth === 2) {
            var intYear = parseInt(arrayDate[2]);
            if (intDay > 0 && intDay < 29) {
                return true;
            }
            else if (intDay === 29) {
                if ((intYear % 4 === 0) && (intYear % 100 !== 0) ||
                    (intYear % 400 === 0)) {
                    // year div by 4 and ((not div by 100) or div by 400) ->ok
                    return true;
                }
            }
        }
    }
    return false; //any other values, bad date
}

function validateValue(strValue, strMatchPattern) {
    /************************************************
    DESCRIPTION: Validates that a string a matches
      a valid regular expression value.
    
    PARAMETERS:
       strValue - String to be tested for validity
       strMatchPattern - String containing a valid
          regular expression match pattern.
    
    RETURNS:
       True if valid, otherwise false.
    *************************************************/
    var objRegExp = new RegExp(strMatchPattern);

    //check if string matches pattern
    return objRegExp.test(strValue);
}


function rightTrim(strValue) {
    /************************************************
    DESCRIPTION: Trims trailing whitespace chars.
    
    PARAMETERS:
       strValue - String to be trimmed.
    
    RETURNS:
       Source string with right whitespaces removed.
    *************************************************/
    var objRegExp = /^([\w\W]*)(\b\s*)$/;

    if (objRegExp.test(strValue)) {
        //remove trailing a whitespace characters
        strValue = strValue.replace(objRegExp, '$1');
    }
    return strValue;
}

function leftTrim(strValue) {
    /************************************************
    DESCRIPTION: Trims leading whitespace chars.
    
    PARAMETERS:
       strValue - String to be trimmed
    
    RETURNS:
       Source string with left whitespaces removed.
    *************************************************/
    var objRegExp = /^(\s*)(\b[\w\W]*)$/;

    if (objRegExp.test(strValue)) {
        //remove leading a whitespace characters
        strValue = strValue.replace(objRegExp, '$2');
    }
    return strValue;
}

function trimAll(strValue) {
    /************************************************
    DESCRIPTION: Removes leading and trailing spaces.
    
    PARAMETERS: Source string from which spaces will
      be removed;
    
    RETURNS: Source string with whitespaces removed.
    *************************************************/
    var objRegExp = /^(\s*)$/;

    //check for all spaces
    if (objRegExp.test(strValue)) {
        strValue = strValue.replace(objRegExp, '');
        if (strValue.length === 0)
            return strValue;
    }

    //check for leading & trailing spaces
    objRegExp = /^(\s*)([\W\w]*)(\b\s*$)/;
    if (objRegExp.test(strValue)) {
        //remove leading and trailing whitespace characters
        strValue = strValue.replace(objRegExp, '$2');
    }
    return strValue;
}

function removeCurrency(strValue) {
    /************************************************
    DESCRIPTION: Removes currency formatting from
      source string.
    
    PARAMETERS:
      strValue - Source string from which currency formatting
         will be removed;
    
    RETURNS: Source string with commas removed.
    *************************************************/
    var objRegExp = /\(/;
    var strMinus = '';

    //check if negative
    if (objRegExp.test(strValue)) {
        strMinus = '-';
    }

    objRegExp = /\)|\(|[,]/g;
    strValue = strValue.replace(objRegExp, '');
    if (strValue.indexOf('$') >= 0) {
        strValue = strValue.substring(1, strValue.length);
    }
    return strMinus + strValue;
}

function addCurrency(strValue) {
    /************************************************
    DESCRIPTION: Formats a number as currency.
    
    PARAMETERS:
      strValue - Source string to be formatted
    
    REMARKS: Assumes number passed is a valid
      numeric value in the rounded to 2 decimal
      places.  If not, returns original value.
    *************************************************/
    var objRegExp = /-?[0-9]+\.[0-9]{2}$/;

    if (objRegExp.test(strValue)) {
        objRegExp.compile('^-');
        strValue = addCommas(strValue);
        if (objRegExp.test(strValue)) {
            strValue = '(' + strValue.replace(objRegExp, '') + ')';
        }
        return '$' + strValue;
    }
    else
        return strValue;
}

function removeCommas(strValue) {
    /************************************************
    DESCRIPTION: Removes commas from source string.
    
    PARAMETERS:
      strValue - Source string from which commas will
        be removed;
    
    RETURNS: Source string with commas removed.
    *************************************************/
    var objRegExp = /,/g; //search for commas globally

    //replace all matches with empty strings
    return strValue.replace(objRegExp, '');
}

function addCommas(strValue) {
    /************************************************
    DESCRIPTION: Inserts commas into numeric string.
    
    PARAMETERS:
      strValue - source string containing commas.
    
    RETURNS: String modified with comma grouping if
      source was all numeric, otherwise source is
      returned.
    
    REMARKS: Used with integers or numbers with
      2 or less decimal places.
    *************************************************/
    var objRegExp = new RegExp('(-?[0-9]+)([0-9]{3})');

    //check for match to search criteria
    while (objRegExp.test(strValue)) {
        //replace original string with first group match,
        //a comma, then second group match
        strValue = strValue.replace(objRegExp, '$1,$2');
    }
    return strValue;
}

function removeCharacters(strValue, strMatchPattern) {
    /************************************************
    DESCRIPTION: Removes characters from a source string
      based upon matches of the supplied pattern.
    
    PARAMETERS:
      strValue - source string containing number.
    
    RETURNS: String modified with characters
      matching search pattern removed
    
    USAGE:  strNoSpaces = removeCharacters( ' sfdf  dfd',
                                    '\s*')
    *************************************************/
    var objRegExp = new RegExp(strMatchPattern, 'gi');

    //replace passed pattern matches with blanks
    return strValue.replace(objRegExp, '');
}

// Validate Date Format
function isDateFormat(value) {
    try {
        //Change the below values to determine which format of date you wish to check. It is set to dd/mm/yyyy by default.
        var dayIndex = 0;
        var monthIndex = 1;
        var yearIndex = 2;

        value = value.replace(/-/g, "/").replace(/\./g, "/");
        var splitValue = value.split("/");
        var ret = true;
        if (splitValue.length > 3) {
            ret = false;
        }
        if (!(splitValue[dayIndex].length === 1 || splitValue[dayIndex].length === 2)) {
            ret = false;
        }
        if (ret && !(splitValue[monthIndex].length === 1 || splitValue[monthIndex].length === 2)) {
            ret = false;
        }
        if (ret && splitValue[yearIndex].length !== 4) {
            ret = false;
        }
        if (ret) {
            var day = parseInt(splitValue[dayIndex], 10);
            var month = parseInt(splitValue[monthIndex], 10);
            var year = parseInt(splitValue[yearIndex], 10);

            if (((year > 1900) && (year < 3000))) {
                if ((month <= 12 && month > 0)) {

                    var leapYear = (((year % 4) === 0) && ((year % 100) !== 0) || ((year % 400) === 0));

                    if (day > 0) {
                        if (month === 2) {
                            ret = leapYear ? day <= 29 : day <= 28;
                        }
                        else {
                            if ((month === 4) || (month === 6) || (month === 9) || (month === 11)) {
                                ret = day <= 30;
                            }
                            else {
                                ret = day <= 31;
                            }
                        }
                    }
                }
            }
        }
        return ret;
    }
    catch (e) {
        return false;
    }
}

function convertToInt(obj) {
    var iResult = 0;
    try {
        if (isNaN(parseInt(obj)) === false) {
            iResult = parseInt(obj);
        }
    } catch (e) {
        iResult = 0;
    }
    return iResult;
}

function parseDateTime(value) {
    var dayIndex = 0;
    var monthIndex = 1;
    var yearIndex = 2;

    value = value.replace(/-/g, "/").replace(/\./g, "/");
    var splitValue = value.split("/");

    var day = parseInt(splitValue[dayIndex], 10);
    var month = parseInt(splitValue[monthIndex], 10);
    var year = parseInt(splitValue[yearIndex], 10);

    return new Date(year, month - 1, day);
}

function daysBetween(date1, date2) {
    //Get 1 day in milliseconds
    var oneDay = 1000 * 60 * 60 * 24;

    // Convert both dates to milliseconds
    var date1Ms = date1.getTime();
    var date2Ms = date2.getTime();

    // Calculate the difference in milliseconds
    var differenceMs = date2Ms - date1Ms;

    // Convert back to days and return
    return Math.round(differenceMs / oneDay);
}

function compareDate(startDate, endDate) {
    try {
        var ret = false;
        if (isDateFormat(startDate) && isDateFormat(endDate)) {
            if (ParseDateTime(startDate) <= ParseDateTime(endDate)) {
                ret = true;
            }
            else {
                ret = false;
            }
        }
        else {
            ret = false;
        }
        return ret;
    } catch (e) {
        return false;
    }
}

function checkDate(field) {
    var allowBlank = true;
    var minYear = 1902;
    var maxYear = (new Date()).getFullYear();
    var errorMsg = "";
    var regs = [];
    // regular expression to match required date format 
    var re = /^(\d{1,2})\/(\d{1,2})\/(\d{4})$/;
    if (field.value !== '') {
        if (regs === field.value.match(re)) {
            if (regs[1] < 1 || regs[1] > 31) { errorMsg = "Invalid value for day: " + regs[1]; }
            else if (regs[2] < 1 || regs[2] > 12) { errorMsg = "Invalid value for month: " + regs[2]; }
            else if (regs[3] < minYear || regs[3] > maxYear) { errorMsg = "Invalid value for year: " + regs[3] + " - must be between " + minYear + " and " + maxYear; }
        } else { errorMsg = "Invalid date format: " + field.value; }
    } else if (!allowBlank) {
        errorMsg = "Empty date not allowed!";
    } if (errorMsg !== "") {
        //alert(errorMsg);
        field.focus();
        return false;
    }
    return true;
}

function isValidDateWithFormatMMDDYYYY(dateStr) {
    // Checks for the following valid date formats:
    // MM/DD/YY   MM/DD/YYYY   MM-DD-YY   MM-DD-YYYY
    // Also separates date into month, day, and year variables

    var datePat = /^(\d{1,2})(\/|-)(\d{1,2})\2(\d{2}|\d{4})$/;

    // To require a 4 digit year entry, use this line instead:
    // var datePat = /^(\d{1,2})(\/|-)(\d{1,2})\2(\d{4})$/;

    var matchArray = dateStr.match(datePat); // is the format ok?
    if (matchArray === null) {
        alert("Date is not in a valid format.");
        return false;
    }
    var month = matchArray[1]; // parse date into variables
    var day = matchArray[3];
    var year = matchArray[4];
    if (month < 1 || month > 12) { // check month range
        alert("Month must be between 1 and 12.");
        return false;
    }
    if (day < 1 || day > 31) {
        alert("Day must be between 1 and 31.");
        return false;
    }
    if ((month === 4 || month === 6 || month === 9 || month === 11) && day === 31) {
        alert("Month " + month + " doesn't have 31 days!");
        return false;
    }
    if (month === 2) { // check for february 29th
        var isleap = (year % 4 === 0 && (year % 100 !== 0 || year % 400 === 0));
        if (day > 29 || (day === 29 && !isleap)) {
            alert("February " + year + " doesn't have " + day + " days!");
            return false;
        }
    }
    return true;  // date is valid
}

function formatPhone(obj) {
    var strvalue;
    if (eval(obj))
        strvalue = eval(obj).value;
    else
        strvalue = obj;
    var num;
    num = strvalue.toString().replace(/\$|\,/g, '');

    if (isNaN(num))
        num = "";
    eval(obj).value = num;
}

function isDouble(x) {
    x = unformatCurrency(x);
    var validateSpecialChar = /^[\d\.]+$/;
    if (x.match(validateSpecialChar)) {
        return true;
    }
    else {
        return false;
    }
}

function isInt(x) {
    var validateSpecialChar = /^[\d]+$/;
    if (x.match(validateSpecialChar)) {
        return true;
    }
    else {
        return false;
    }
}

function isNumKey(e) {
    var text = String.fromCharCode(e.which);
    var validateSpecialChar = /^[0-9\.]+$/;
    if (text.match(validateSpecialChar) || (e.keyCode === 8 || e.keyCode === 46 || (e.keyCode === 37 && text !== "%") || (e.keyCode === 39 && text !== "'"))) {
        return true;
    }
    else {
        return false;
    }
}

function isNum(x) {
    var validateSpecialChar = /^[0-9\.]+$/;
    if (x.match(validateSpecialChar)) {
        return true;
    }
    else {
        return false;
    }
}

function validateBirthDay(dateValue) {
    var flag = false;

    var currentDate = new Date();
    var minDate = 1900;
    var maxDate = parseInt(currentDate.getFullYear()) - 18;
    var dteDate;
    var dteDate2;
    var months = new Array('JAN', 'FEB', 'MAR', 'APR', 'MAY', 'JUN', 'JUL', 'AUG', 'SEP', 'OCT', 'NOV', 'DEC');

    //dateValue is dd/MM/yyyy
    if (dateValue !== undefined && dateValue !== '' && dateValue !== null) {
        var obj1 = dateValue.split("/");
        var obj2 = new Array();

        //day
        obj2[0] = parseInt(obj1[1], 10);

        //month
        obj2[1] = parseInt(obj1[0], 10) - 1;

        //year
        obj2[2] = parseInt(obj1[2], 10);

        dteDate = new Date(obj2[2], obj2[1], obj2[0]);

        var indexOfM = -1;
        for (var i = 0; i < months.length; i++) {
            if (months[i] === obj1[2].toUpperCase()) {
                indexOfM = i;
            }
        }

        dteDate2 = new Date(obj2[2], indexOfM, obj2[0]);
        if (
          (
               (obj2[0] === dteDate.getDate()) &&
               (obj2[1] === dteDate.getMonth()) &&
               (obj2[2] === dteDate.getFullYear()) &&
               (dteDate.getFullYear() > minDate) &&
               (dteDate.getFullYear() < maxDate)
           ) ||
          (
               (obj1[0] === dteDate2.getDate()) &&
               (indexOfM === dteDate2.getMonth()) &&
               (obj1[2] === dteDate2.getFullYear()) &&
               (dteDate2.getFullYear() > minDate) &&
               (dteDate2.getFullYear() < maxDate)
           )
        )
            flag = true;
        else
            flag = false;
    }
    else
        flag = false;
    return flag;
}

function validateBirthDate(controlId) {
    var flag = false;

    var currentDate = new Date();
    var minDate = 1900;
    var maxDate = parseInt(currentDate.getFullYear()) - 18;
    var dteDate;
    var dteDate2;
    var months = new Array('JAN', 'FEB', 'MAR', 'APR', 'MAY', 'JUN', 'JUL', 'AUG', 'SEP', 'OCT', 'NOV', 'DEC');

    var dateValue = document.getElementById(controlId).value;

    if (dateValue !== '') {
        var obj1 = document.getElementById(controlId).value.split("/");
        var obj2 = new Array();

        obj2[0] = parseInt(obj1[0], 10);
        obj2[1] = parseInt(obj1[1], 10) - 1;
        obj2[2] = parseInt(obj1[2], 10);

        dteDate = new Date(obj2[2], obj2[1], obj2[0]);

        var indexOfM = -1;
        for (var i = 0; i < months.length; i++) {
            if (months[i] === obj1[1].toUpperCase()) {
                indexOfM = i;
            }
        }

        dteDate2 = new Date(obj2[2], indexOfM, obj2[0]);
        if (
          (
               (obj2[0] === dteDate.getDate()) &&
               (obj2[1] === dteDate.getMonth()) &&
               (obj2[2] === dteDate.getFullYear()) &&
               (dteDate.getFullYear() > minDate) &&
               (dteDate.getFullYear() < maxDate)
           ) ||
          (
               (obj1[0] === dteDate2.getDate()) &&
               (indexOfM === dteDate2.getMonth()) &&
               (obj1[2] === dteDate2.getFullYear()) &&
               (dteDate2.getFullYear() > minDate) &&
               (dteDate2.getFullYear() < maxDate)
           )
        )
            flag = true;
        else
            flag = false;
    }
    else
        flag = false;
    return flag;
}

// Validation Code: Chi duoc nhap So, Dau Gach va dau gach duoi.
function validateCode(x) {

    var validateSpecialChar = /^[0-9\_\-]+$/;

    if (x.match(validateSpecialChar)) {
        return true;
    }
    else {
        return false;
    }
}

// Validation cac ki tu dac biet: Chi duoc nhap cac ki tu: chu (Hoa, thuong), So, ki tu gach duoi _
function validateSpecialCharacter(x) {
    var validateSpecialChar = /^[A-Za-z0-9\_\-]+$/;

    if (x.match(validateSpecialChar)) {
        return true;
    }
    else {
        return false;
    }
}