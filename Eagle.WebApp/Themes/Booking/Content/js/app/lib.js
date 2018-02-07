function handleAjaxErrorMessages(formId, jqXhr, textStatus, errorThrown) {
    var title = '', message = '';

    if (errorThrown !== null && errorThrown !== '') {
        switch (errorThrown) {
            case 'timeout':
                title = "Time-out Error \n";
                message = "Time out error.";
                break;
            case 'error':
                title = "Ajax Exception \n";
                message = "Error Ajax request";
                break;
            case 'abort':
                title = "Abort Exception \n";
                message = "Ajax request aborted";
                break;
            case 'parsererror':
                title = "Json Parse Error \n";
                message = "Requested JSON parse failed.";
                break;
            default:
                title = jqXhr.status + 'Uncaught Error.\n';
                message = jqXhr.responseText;
                break;
        }
    }
    else {
        if (jqXhr.status !== null && jqXhr.status !== '' && jqXhr.status !== undefined) {
            switch (jqXhr.status) {
                case 0:
                    title = jqXhr.status + " Network Error\n";
                    message = "Not connect.n Verify Network.";
                    break;
                case 401:
                    title = jqXhr.status + " Unauthorized !";
                    message = "Forbidden - Unauthorized Error";
                    break;
                case 403:
                    title = jqXhr.status + " Forbidden!";
                    message = "Sorry, your session has expired.";
                    break;
                case 404:
                    title = jqXhr.status + " Page Not Found !";
                    message = 'Requested page not found. [404]';
                    break;
                case 500:
                    title = jqXhr.status + " Internal Server Error !";
                    message = 'Internal Server Error';
                    break;
                case 503:
                    title = jqXhr.status + " Service Unavailable !";
                    message = 'Service Unavailable';
                    break;
                case 590:
                    title = jqXhr.status + " Unexpected time-out !";
                    message = 'Unexpected time-out';
                    break;
                default:
                    title = jqXhr.status + ' Unexpected error.';
                    message = jqXhr.responseText;
                    break;
            }
        }
    }

    showAlertMessageBox(formId, title, message, "error", 10000);
}
function handleAjaxErrors(jqXhr, textStatus, errorThrown) {
    var title = '', message = '';

    if (errorThrown !== null && errorThrown !== '') {
        switch (errorThrown) {
            case 'timeout':
                title = "Time-out Error \n";
                message = "Time out error.";
                break;
            case 'error':
                title = "Ajax Exception \n";
                message = "Error Ajax request";
                break;
            case 'abort':
                title = "Abort Exception \n";
                message = "Ajax request aborted";
                break;
            case 'parsererror':
                title = "Json Parse Error \n";
                message = "Requested JSON parse failed.";
                break;
            default:
                title = jqXhr.status + 'Uncaught Error.\n';
                message = jqXhr.responseText;
                break;
        }
    }
    else {
        if (jqXhr.status !== null && jqXhr.status !== '' && jqXhr.status !== undefined) {
            switch (jqXhr.status) {
                case 0:
                    title = jqXhr.status + " Network Error\n";
                    message = "Not connect.n Verify Network.";
                    break;
                case 401:
                    title = jqXhr.status + " Unauthorized !";
                    message = "Forbidden - Unauthorized Error";
                    break;
                case 403:
                    title = jqXhr.status + " Forbidden!";
                    message = "Sorry, your session has expired.";
                    break;
                case 404:
                    title = jqXhr.status + " Page Not Found !";
                    message = 'Requested page not found. [404]';
                    break;
                case 500:
                    title = jqXhr.status + " Internal Server Error !";
                    message = 'Internal Server Error';
                    break;
                case 503:
                    title = jqXhr.status + " Service Unavailable !";
                    message = 'Service Unavailable';
                    break;
                case 590:
                    title = jqXhr.status + " Unexpected time-out !";
                    message = 'Unexpected time-out';
                    break;
                default:
                    title = jqXhr.status + ' Unexpected error.';
                    message = jqXhr.responseText;
                    break;
            }
        }
    }

    showMessageWithTitle(title, message, "error", 10000);
}

function showHiveDiv(selectedValue, divBox1, divBox2) {
    var divContainer1 = $("#" + divBox1);
    var divContainer2 = $("#" + divBox2);
    if (selectedValue === "1") {
        divContainer1.css("display", "none");
        divContainer2.css("display", "block");
    } else {
        divContainer1.css("display", "block");
        divContainer2.css("display", "none");
    }
    return false;
}

function createCookie(name, value, days) {
    var expires='';
    if (days)
    {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = '; expires=' + date.toGMTString();
    }
    document.cookie = name + '=' + encodeURIComponent(value) + expires + '; path=/';
}

function readCookie(name) {
    var nameEq = name + '=';
    var ca = document.cookie.split(';');

    for (var i = 0; i < ca.length; i++) {
        var c = decodeURIComponent(ca[i]);

        while (c.charAt(0) === ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEq) === 0) return c.substring(nameEq.length, c.length);
    }

    return null;
}

/*== DATE TIME =================================*/
function ddMMyyyyToMMddyyyy(str) {
    try {
        var split = str.split('/');
        var day = split[0];
        var month = split[1];
        var year = split[2];
        return year + '/' + month + '/' + day;
    } catch (e) {
        return null;
    }
}

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

function HandleBootstrapCheckBox() {
    var checkBoxSelector = $('input[type=checkbox][class="check-box"]');
    var hiddenSelector = $("input:hidden[name=" + checkBoxSelector.attr("name") + "]");
    checkBoxSelector.click(function () {
        var checkBoxStatus = $(this).is(":checked");
        checkBoxSelector.attr("checked", checkBoxStatus);
        checkBoxSelector.val(checkBoxStatus);
        hiddenSelector.val(checkBoxStatus);
    });
}

function handleRadios() {
    $('input:radio').change(function () {
        var radioName = $(this).attr("name");
        $("input:radio[name=" + radioName + "]").attr('checked', false);
       // $("input:radio[name=" + radioName + "]").parent('label').removeClass('active');
        $(this).attr("checked", true);
        $(this).parent('label').removeClass('btn-default').addClass('active btn-success');
        $(this).parent('label').siblings().removeClass('active btn-success').addClass('btn-default');
    });
}

function handleRadioButton(radioName) {
    var radioSelector = $("input:radio[name=" + radioName + "]");
    var hiddenSelector = $("input:hidden[name=" + radioName + "]");
    radioSelector.change(function () {
        var radioStatus = $(this).is(":checked");
        radioSelector.attr("checked", radioStatus);
        radioSelector.val(radioStatus);
        hiddenSelector.val(radioStatus);
    });
}

function handleRadioButtionSwitch() {
    $(document).on("click change", ".btn-toggle", function () {
        $(this).children('label').removeClass('btn-success').addClass('btn-default');

        if ($(this).find('.btn').hasClass('active')) {
            $(this).find('.btn').toggleClass('btn-success');
            $(this).children('input').attr("checked", true);
        } else {
            $(this).find('.btn').removeClass('active');
            $(this).children('input').attr("checked", false);
        }
    });
}

function handleCheckBox(checkFieldId) {
    $("input:checkbox[name=" + checkFieldId + "]").click(function () {
        var checkBoxStatus = $(this).is(":checked");
        $(this).attr("checked", checkBoxStatus);
        $(this).val(checkBoxStatus);
        $("input:hidden[name=" + checkFieldId + "]").val(checkBoxStatus);
    });
}

function handleCheckBoxEvent() {
    //Handle Checkbox
    $('input:checkbox').click(function () {
        var checkBoxStatus = $(this).is(":checked");
        $(this).attr("checked", checkBoxStatus);
        $(this).val(checkBoxStatus);
    });
}

function handleCheckBoxes() {
    $('input:checkbox').click(function () {
        var checkBoxStatus = $(this).is(":checked");
        $(this).attr("checked", checkBoxStatus);
        $(this).val(checkBoxStatus);
        $("input:hidden[name=" + $(this).attr("name") + "]").val(checkBoxStatus);
    });
}

function handleCheckBoxesWithStatus(status) {
    $('input:checkbox').attr("checked", status);
    $('input:checkbox').val(status);
    $("input:hidden[name=" + $(this).attr("name") + "]").val(status);

    $('input:checkbox').click(function () {
        var checkBoxStatus = $(this).is(":checked");
        $(this).attr("checked", checkBoxStatus);
        $(this).val(checkBoxStatus);
        $("input:hidden[name=" + $(this).attr("name") + "]").val(checkBoxStatus);
    });
}

function handleCheckBoxStatus(checkFieldId, chkStatus) {
    var checkBoxSelector = $("input:checkbox[name=" + checkFieldId + "]");
    var hiddenSelector = $("input:hidden[name=" + checkFieldId + "]");

    checkBoxSelector.attr("checked", chkStatus);
    checkBoxSelector.val(chkStatus);
    hiddenSelector.val(chkStatus);

    checkBoxSelector.click(function () {
        var checkBoxStatus = $(this).is(":checked");
        $(this).attr("checked", checkBoxStatus);
        $(this).val(checkBoxStatus);
        hiddenSelector.val(checkBoxStatus);
    });
}

function handleAccordion() {
    $(document).on('click', 'div.accordion-heading a', function () {
        var nextElement = $(this).parent('.accordion-heading').parent('.accordion-group').siblings();
        nextElement.find('.accordion-body').removeClass("in").css("height", "0");
        nextElement.find('.accordion-heading').find(".icon-chevron-up").removeClass("glyphicon-chevron-up").addClass("glyphicon-chevron-down");
    });

    $('div.accordion-body').on('shown', function () {
        $(this).parent("div").find(".glyphicon-chevron-down")
               .removeClass("glyphicon-chevron-down").addClass("glyphicon-chevron-up");
    });

    $('div.accordion-body').on('hidden', function () {
        $(this).parent("div").find(".glyphicon-chevron-up")
               .removeClass("glyphicon-chevron-up").addClass("glyphicon-chevron-down");
    });
}

function loadContent(url, container) {
    $.ajax({
        type: "GET",
        dataType: "html",
        url: url,
        success: function (data) {
            $(container).html(data);
        }, error: function (jqXhr, textStatus, errorThrown) {
            handleAjaxErrors(jqXhr, textStatus, errorThrown);
        }
    });
}


/// <reference path="spin.js" />
function replaceButtonText(buttonId, text) {
    if (document.getElementById) {
        var button = document.getElementById(buttonId);
        if (button) {
            if (button.childNodes[0]) {
                button.childNodes[0].nodeValue = text;
            }
            else if (button.value) {
                button.value = text;
            }
            else //if (button.innerHTML)
            {
                button.innerHTML = text;
            }
        }
    }
}

function commaFormatted(amount) {
    var delimiter = "."; // replace comma if desired
    amount = new String(amount);
    var a = amount.split('.', 2);
    var d = a[1];
    var i = parseInt(a[0]);
    if (isNaN(i)) { return ''; }
    var minus = '';
    if (i < 0) { minus = '-'; }
    i = Math.abs(i);
    var n = new String(i);
    a = [];
    while (n.length > 3) {
        var nn = n.substr(n.length - 3);
        a.unshift(nn);
        n = n.substr(0, n.length - 3);
    }
    if (n.length > 0) { a.unshift(n); }
    n = a.join(delimiter);
    if (d.length < 1) { amount = n; }
    else { amount = n + '.' + d; }
    amount = minus + amount;
    return amount;
}

//// Doan code chan nut Enter
//document.onkeypress = stopRKey;

//function stopRKey(evt) {
//    evt = (evt) ? evt : ((event) ? event : null);
//    var node = (evt.target) ? evt.target : ((evt.srcElement) ? evt.srcElement : null);
//    if ((evt.keyCode === 13) && (node.type === "text")) {
//        return false;
//    } else {
//        return true;
//    }
//}

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

function addDot(nStr) {
    nStr += '';
    var x = nStr.split('.');
    var x1 = x[0];
    var x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + '.' + '$2');
    }
    return x1 + x2;
}

function addCommas(nStr) {
    nStr += '';
    var x = nStr.split('.');
    var x1 = x[0];
    var x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;
}

function convertFormToJSON(formId) {
    var array = $("#" + formId).serializeArray();
    var json = {};
    $.each(array, function () {
        json[this.name] = this.value || '';
    });
    return JSON.stringify(json);
}

function formatNumber(val) {
    if (isNaN(val))
        return val.replace(',', '.');
    else
        return val;
}


//Check whether key down is number
function isNumberFromKeyDown(e) {
    if ((48 <= e.keyCode && e.keyCode <= 57)
        || (33 < e.keyCode && e.keyCode < 40)
        || e.keyCode === 0
        || e.keyCode === 8
        || e.keyCode === 32
        || e.keyCode === 45
        || e.keyCode === 46)
        return true;
    return false;
}

function removeFormatNumber(cellvalue) {
    return cellvalue.replace(/\$|\./g, '').replace(',', '.');
}

function multiLineHtmlEncode(value) {
    var lines = value.split(/\r\n|\r|\n/);
    for (var i = 0; i < lines.length; i++) {
        lines[i] = htmlEncode(lines[i]);
    }
    return lines.join('\r\n');
}

function htmlEncode(value) {
    if (value) {
        return jQuery('<div/>').text(value).html();
    } else {
        return '';
    }
}


// Show dialog with OK button: Information, warning, Error - START
function showContentPopUp(title, msg) {
    $(".modal-header").html(title);
    $(".modal-body").html(msg);
    $(".modal-popup").modal('show');
    $('html, body').animate({ scrollTop: 80 }, 'slow');
}

function hideContentPopUp() {
    $('.modal-body').html('');
    $(".modal-popup").modal('hide');


}

function scrollToElementInDiv(elementId, divContainerId) {
    $('#' + divContainerId).animate({
        scrollTop: $(this).parent().scrollTop()
    }, {
        duration: 1000,
        specialEasing: {
            width: 'linear',
            height: 'easeOutBounce'
        },
        complete: function (e) {
            console.log("animation completed");
        }
    });
}

function scrollToTopInPopUp() {
    $('.modal-body').animate({ scrollTop: $(this).parent().scrollTop() }, 'slow');
}

//NUMER INTERACTION ========================================
function toInt(obj) {
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

function toFloat(obj) {
    var iResult = 0;
    try {
        if (isNaN(parseFloat(obj)) === false) {
            iResult = parseFloat(obj);
        }
    } catch (e) {
        iResult = 0;
    }
    return iResult;
}

function addDotToNumber(nStr) {
    try {
        return parseFloat(nStr).formatMoney(2, ',', '.');
    } catch (e) {
        return 0.00;
    }
}

function unformatNumber(cellvalue) {
    try {
        return cellvalue.replace(/\$|\./g, '').replace(',', '.');
    } catch (e) {
        return 0;
    }
}

 function isNumber(e) {
    if ((48 <= e.keyCode && e.keyCode <= 57)
        || (33 < e.keyCode && e.keyCode < 40)
        || e.keyCode === 0
        || e.keyCode === 8
        || e.keyCode === 9
        || e.keyCode === 32
        || e.keyCode === 45
        || e.keyCode === 46)
        return true;
    return false;
 }

 function setupNumber() {
     //$('.number').each(function () {
     //    if ($(this).is('input'))
     //        $(this).val(addDotToNumber($(this).val()));
     //    else if (!isNaN($(this).html().replace(',', '.')))
     //        $(this).html(addDotToNumber($(this).html().replace(',', '.')));
     //});

     //$('input.number').each(function () {
     //    this.value = this.value.replace(/,/g, "");
     //}).val(function (index, value) {
     //    return value
     //    .replace(/\D/g, "")
     //    .replace(/\B(?=(\d{3})+(?!\d))/g, ",");
     //});

     $('input.number').keyup(function (event) {
         // skip for arrow keys
         if (event.which >= 37 && event.which <= 40) return;

         // format number
         $(this).val(function (index, value) {
             return value
                 .replace(/\D/g, "")
                 .replace(/\B(?=(\d{3})+(?!\d))/g, ",");
         });
     });
 }

 $.fn.trim = function (length) {
     return this.length > length ? this.substring(0, length) + "..." : this;
 }

 //$.fn.returnPressNumber = function (x) {
 //    return this.each(function () {
 //        $(this)
 //        .attr('type', 'text')
 //        .keypress(function (e) {
 //            console.log(e.keyCode);
 //            if (isNumber(e) === true) {
 //                if (x != undefined) {
 //                    return x(this);
 //                }
 //                else {
 //                    return true;
 //                }
 //            }
 //            else {
 //                return false;
 //            }
 //        })
 //        .keyup(function () {
 //            // Format Number
 //            var num = unformatNumber($(this).val());
 //            $(this).val(addDotToNumber(num));
 //        });
 //    });
 //};


//$.fn.formatNumber = function() {
//    return $(this).val(addDotToNumber($(this).val()));
//};

//$.fn.returnPressNumber = function (x) {
//    return this.each(function () {
//        $(this)
//        .attr('type', 'text')
//        .keypress(function (e) {
//            //console.log(e.keyCode);

//            if (isNumber(e) === true) {
//                if (x !== undefined) {
//                    return x(this);
//                }
//                else {
//                    return true;
//                }
//            }
//            else {
//                return false;
//            }
//        })
//        .keyup(function () {
//            //Format Number
//            var num = unformatNumber($(this).val());
//            $(this).val(addDotToNumber(num));
//        });
//    });
//};

$.fn.restrictNumber = function () {
    return this.each(function () {
        if (this.type && 'number' === this.type.toLowerCase()) {
            $(this).on('input', function () {
                var v = parseFloat(this.value),
                min = parseFloat(this.min),
                max = parseFloat(this.max);

                if (v < min) {
                    this.value = min;
                } else if (v > max) {
                    this.value = max;
                }
                else {
                    this.value = v;
                }
            });
        }
    });
};

/**End NUMER INTERACTION =========================================================*/

//Start FORM-MANAGEMENT ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

function convertFormToJSON(formId) {
    var array = $("#" + formId).serializeArray();
    var json = {};
    $.each(array, function () {
        json[this.name] = this.value || '';
    });
    return JSON.stringify(json);
}

$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};

$.fn.serializeTableToArray = function () {
    var array = [];
    var headers = [];
    $(this).find('th').each(function (index, item) {
        if ($(item).attr("id") !== "" && typeof $(item).attr("id") !== 'undefined') {
            headers[index] = $(item).data("id").replace(/\n|\r|\s+/g, "");
        }
    });
    $(this).find('tbody > tr').has('td').each(function () {
        var arrayItem = {};
        $('td', $(this)).each(function (index, item) {
            arrayItem[headers[index]] = $(item).find('input,input[type="hidden"],input[type="checkbox"], textarea, select').val();
        });
        array.push(arrayItem);
    });
    //console.log(JSON.stringify(array));
    return array;
};

$.fn.serializeFormJSON = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
}

// Pass an object of key/vals to override
//var formData = $("#" + form).awesomeFormSerializer({
//    Quantity: $("#Quantity").val().replace('.', ''),
//    DiscountRate: $("#DiscountRate").val().replace('.', '')
//});
$.fn.awesomeFormSerializer = function (overrides) {
    // Get the parameters as an array
    var newParams = this.serializeArray();

    for (var key in overrides) {
        if (overrides.hasOwnProperty(key)) {
            var newVal = overrides[key];
            var index;
            // Find and replace `content` if there
            for (index = 0; index < newParams.length; ++index) {
                if (newParams[index].name === key) {
                    newParams[index].value = newVal;
                    break;
                }
            }

            // Add it if it wasn't there
            if (index >= newParams.length) {
                newParams.push({
                    name: key,
                    value: newVal
                });
            }
        }
    }

    // Convert to URL-encoded string
    return $.param(newParams);
};

//$("#formId").clearValidation();
$.fn.clearValidation = function() {
    var validator = $(this).validate();
    //$(this).data('validator').resetForm();
    validator.hideErrors();
    validator.resetForm();
    validator.reset();

    //$('.form-group').each(function () { $(this).removeClass('has-success'); });
    //$('.form-group').each(function () { $(this).removeClass('has-error'); });
    //$('.form-group').each(function () { $(this).removeClass('has-feedback'); });
    //$('.help-block').each(function () { $(this).remove(); });
    //$('.form-control-feedback').each(function () { $(this).remove(); });

    $("label.error").hide();
    $(".error").removeClass("error");
    $('.input-validation-error').removeClass('input-validation-error');
    $('.validation-summary-errors').html();
    $('.validation-summary-errors').addClass('validation-summary-valid');
    $('.validation-summary-valid').removeClass('validation-summary-errors');
};

//This can be used like this: 
//var form = element.closest("form");
//form.initValidation(); or $("#SomeForm").initValidation();
$.fn.initValidation = function () {        
    $(this).removeData("validator");
    $(this).removeData("unobtrusiveValidation");
    $.validator.unobtrusive.parse(this);        
    return this;
};


function resetFormValidator(formId) {
    $(formId).removeData('validator');
    $(formId).removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse(formId);
}

//End FORM-MANAGEMENT ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


$.fn.flash = function (duration) {
    this.animate({ opacity: 0 }, duration).animate({ opacity: 100 }, duration);
};

///* Checks whether or not an element is visible. The default jQuery implementation doesn't work. */
$.fn.isVisible = function () {
    return !($(this).css('visibility') === 'hidden' || $(this).css('display') === 'none');
};

///** Sorts options in a select / list box. */
$.fn.sortOptions = function () {
    return this.each(function () {
        $(this).append($(this).find('option').remove().sort(function (a, b) {
            var at = $(a).text(), bt = $(b).text();
            return (at > bt) ? 1 : ((at < bt) ? -1 : 0);
        }));
    });
};

/** Simple delay function that can wrap around an existing function and provides a callback. */
var delay = (function () {
    var timer = 0;
    return function (callback, ms) {
        clearTimeout(timer);
        timer = setTimeout(callback, ms);
    };
})();

$.fn.filterByText = function (textBox) {
    return this.each(function () {
        var select = this;
        var options = [];
        var timeout = 10;

        $(select).find('option').each(function () {
            options.push({ value: $(this).val(), text: $(this).text() });
        });

        $(select).data('options', options);

        $(textBox).bind('change keyup', function () {
            delay(function () {
                var options = $(select).data('options');
                var search = $.trim($(this).val());
                var regex = new RegExp(search, 'gi');

                $.each(options, function (i) {
                    if (options[i].text.match(regex) === null) {
                        $(select).find($('option[value="' + options[i].value + '"]')).hide();
                    } else {
                        $(select).find($('option[value="' + options[i].value + '"]')).show();
                    }
                });
            }, timeout);
        });
    });
};

////USAGE: $("#form").serializefiles();
$.fn.serializefiles = function () {
    var obj = $(this);
    /* ADD FILE TO PARAM AJAX */
    var formData = new FormData();
    $.each($(obj).find("input[type='file']"), function (i, tag) {
        $.each($(tag)[0].files, function (i, file) {
            formData.append(tag.name, file);
        });
    });
    var params = $(obj).serializeArray();
    $.each(params, function (i, val) {
        formData.append(val.name, val.value);
    });
    return formData;
};

/*== CURRENCY =================================*/
function formatCurrency(num) {
    if (!num) return "￥0";
    num = num.toString().replace(/\$|\,/g, '');
    if (isNaN(num))
        num = "0";
    var sign = (num === (num = Math.abs(num)));
    num = Math.floor(num * 100 + 0.50000000001);
    var cents = num % 100;
    num = Math.floor(num / 100).toString();
    if (cents < 10)
        cents = "0" + cents;
    for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3) ; i++)
        num = num.substring(0, num.length - (4 * i + 3)) + ',' +
              num.substring(num.length - (4 * i + 3));
    if (parseFloat(cents) === 0) {
        return (((sign) ? '' : '-') + '￥' + num);
    }
    else {
        return (((sign) ? '' : '-') + '￥' + num + '.' + cents);
    }
}

function currencyFmatter(el, cellval, opts) {
    if (el !== null && !isNaN(el)) {
        var current = formatCurrency(el);
        return current.replace('.00', '');
    }
    return "￥0";
}

function unformatCurrency(cellvalue) {
    return cellvalue.replace("￥", "").replace(/\$|\,/g, '');
}

$.fn.formatMoney = function (c, d, t) {
    c = isNaN(c = Math.abs(c)) ? 2 : c;
    d = d === undefined ? "." : d;
    t = t === undefined ? "," : t;

    var n = this,
        s = n < 0 ? "-" : "",
        i = parseInt(n = Math.abs(+n || 0).toFixed(c)) + "",
        j = (j = i.length) > 3 ? j % 3 : 0;
    var result = s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");
    if (parseFloat(result.substring(result.length - c, result.length)) === 0) {
        result = result.split(d)[0];
    }
    return result;
};

function backToTop() {
    $(window).scroll(function () {
        if ($(this).scrollTop() > 100) {
            $('.scroll-up').fadeIn();
        } else {
            $('.scroll-up').fadeOut();
        }
    });

    $('.scroll-up').click(function () {
        $("html, body").animate({
            scrollTop: 0
        }, 600);
        return false;
    });
}

// Dynamic Rows Code
function addRow() {
    var table = $(".dynamic-table");
    var tableArray = table.data("array");

    // Get max row id and set new id
    var newid = 0;
    var rowArray = '';
    $.each(table.find("tbody tr"), function () {
        if (parseInt($(this).data("id")) > newid) {
            newid = parseInt($(this).data("id"));
        }
        //$(this).addClass('active').siblings().removeClass('active');
        $(this).addClass('bg-grey').removeClass('active');
        $(this).find('input:text, input:radio, input:checkbox, select, textarea').attr('readonly', true);
        $('.readonly input:checkbox').click(function () { return false; });
        $('.readonly input:checkbox').keydown(function () { return false; });
    });
    newid++;
    rowArray = tableArray + '[' + newid + ']';

    var tr = $("<tr></tr>", {
        id: "row" + newid,
        "data-id": newid,
        "data-array": rowArray,
        "class": "active"
    });

    //if (table.find('tbody tr:nth(0) td:first-child')) {
    //    $("<td></td>", {
    //        'class': 'text-center',
    //        'text': table.find('tr').length
    //    }).prependTo($(tr));
    //}

    // loop through each td and create new elements with name of newid
    $.each(table.find("tbody tr:nth(0) td"), function () {
        var curTd = $(this);
        var children = curTd.children();

        // add new td and element if it has a nane
        if ($(this).data("name") !== undefined && $(this).data("name") !== null && $(this).data("name") !== '') {
            var td = $("<td></td>", { "class": $(curTd).attr("class"), "data-name": $(curTd).data("name") });

            var c = $(curTd).find($(children[0]).prop('tagName')).clone();
            if (!c.hasClass(".ignored")) {
                c.not(".ignored").val('');
            }

            c.attr({
                "id": rowArray + '.' + $(curTd).data("name"),
                "name": rowArray + '.' + $(curTd).data("name"),
                'readonly': false
            });

            if (c.is('input:text:not(".ignored")')) {
                c.attr('value', '');
                c.bind("change keyup paste", function () {
                    c.attr('value', c.val());
                });
            }

            if (c.is('textarea')) {
                c.text('');
                c.bind("change keyup paste", function () {
                    c.html(c.val());
                });
            }

            if (c.is('input:checkbox:not(".ignored")')) {
                c.removeAttr('checked');
                c.val('false');
                c.bind("click", function () {
                    c.attr({
                        "checked": c.is(":checked"),
                        "value": c.is(":checked")
                    });
                });
            }

            if (c.is('select')) {
                c.removeAttr('selected');
            }

            c.appendTo($(td));
            td.appendTo($(tr));
        }
    });

    //add delete button and td
    $("<td class='text-center'></td>").append("<button data-toggle='tooltip' data-title='Edit' class='btn btn-success glyphicon glyphicon-pencil row-edit'></button> <button data-toggle='tooltip' data-title='Delete' class='btn btn-warning glyphicon glyphicon-trash row-remove'></button>").appendTo($(tr));

    //add the new row
    $(tr).appendTo(table);

    //rebind Validate
    $('form').data('validator', null);
    $.validator.unobtrusive.parse($('form'));


    $(".row-remove").on("click", function () {
        $(this).closest("tr").remove();
        return false;
    });

    $(".row-edit").on("click", function () {
        $(this).closest("tr").css("background-color", "transparent");
        $(this).closest("tr").find('input:text, input:radio, input:checkbox, select, textarea').attr('readonly', false);
        return false;
    });
}

function setReadOnly() {
    $('.readonly input').attr('readonly', 'readonly');
    $('.readonly textarea').attr('readonly', 'readonly');
    $('.readonly input:checkbox').click(function () { return false; });
    $('.readonly input:checkbox').keydown(function () { return false; });
}

function removeReadOnly() {
    $('.readonly input').removeAttribute('readonly');
    $('.readonly textarea').removeAttribute('readonly');
    $('.readonly input:checkbox').click(function () { return true; });
    $('.readonly input:checkbox').keydown(function () { return true; });
}

function updateProgress(percentage) {
    var $progress = $('.progress');
    var $progressBar = $('.progress-bar');
   
    $progress.css('display', 'block');
    $progressBar.css('width', percentage + '%');
    $progressBar.html(percentage + '%');

    if (percentage > 99.999) {
        $progressBar.animate({ width: "100%" }, 100);
        $progress.delay(1000).fadeOut(1000);
    }
}

function createProgress() {
    var $progress = $('.progress');
    var $progressBar = $('.progress-bar');
  
    setTimeout(function () {
        $progressBar.css('width', '10%');
        setTimeout(function () {
            $progressBar.css('width', '30%');
            setTimeout(function () {
                $progressBar.css('width', '100%');
                setTimeout(function () {
                    $progress.css('display', 'none');
                    showMessageWithTitle("Done", "Done", "success", 500);
                }, 500); // WAIT 5 milliseconds
            }, 2000); // WAIT 2 seconds
        }, 1000); // WAIT 1 seconds
    }, 1000); // WAIT 1 second
}

//PHOTO-MANAGEMENT START ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//get file size
function getFileSize(fileid) {
    var fileSize = 0; //for IE
    fileSize = $("#" + fileid)[0].files[0].size; //size in kb
    fileSize = fileSize / 1048576; //size in mb 
    return fileSize;
}

//get file path from client system
function getNameFromPath(strFilepath) {
    var objRe = new RegExp(/([^\/\\]+)$/);
    var strName = objRe.exec(strFilepath);

    if (strName === null) {
        return null;
    }
    else {
        return strName[0];
    }
}

$.fn.checkFile = function (options) {
    var defaults = {
        allowedExtensions: [
            'jpg', 'jpeg', 'png', 'gif', 'bmp', 'doc', 'docx', 'txt', 'xls', 'xlsx', 'csv', 'pdf', 'zip', 'rar', 'xml', 'swf', 'htm', 'html', 'js'],
        allowedSize: 150, //150MB	
        success: function () { }
    };

    options = $.extend(defaults, options);

    if ($(this).get(0).files.length === 0) {
        return;
    }

    // get the file name, possibly with path (depends on browser)
    var fileName = $(this).val();
    var fileNameLower = fileName.toLowerCase();
    var extension = fileNameLower.substr((fileNameLower.lastIndexOf('.') + 1));

    //var fileSize = $(this)[0].files[0].size; //size in kb
    var fileSize = $(this).get(0).files.size; //size in kb
    fileSize = fileSize / 1048576; //size in mb  

    if ($.inArray(extension, options.allowedExtensions) === -1) {
        if (fileSize > options.allowedSize) {
            showNotification('error', 'Wrong extension type! You can upload only ' + options.allowedExtensions + ' extension file, and file size is less than ' + options.allowedSize + ' MB');
        } else {
            showNotification('error', 'Wrong extension type! You can upload only ' + options.allowedExtensions + ' extension file');
        }
        $(this).focus();
    } else {
        if (fileSize > options.allowedSize) {
            showNotification('error', 'You can only upload file up to ' + options.allowedSize + ' MB');
            $(this).focus();
        } else {
            hideMessage();
            options.success();
        }
    }
};

$.fn.checkFileSize = function (options) {
    var defaults = {
        allowedSize: 15, //15MB
        success: function () { },
        error: function () {
            showNotification('error', 'You can only upload file up to 15 MB');
        }
    };
    options = $.extend(defaults, options);

    return this.each(function () {
        $(this).on('change', function () {
            var id = $(this).attr('id'),
				size = getFileSize(id);

            if (size > options.allowedSize) {
                options.error();
                $(this).focus();
            } else {
                options.success();
            }
        });
    });
};

$.fn.checkFileType = function (options) {
    var defaults = {
        allowedExtensions: ['jpg', 'jpeg', 'png', 'gif'],
        success: function () { },
        error: function () {
            showNotification('error', 'You can upload only jpg,jpeg,png,gif extension file');
        }
    };
    options = $.extend(defaults, options);

    if ($(this).get(0).files.length === 0) {
        showNotification('error', 'File has not been found !');
        return false;
    }

    return this.each(function () {
        $(this).on('change', function () {
            var value = $(this).val(),
                file = value.toLowerCase(),
                extension = file.substring(file.lastIndexOf('.') + 1);

            if ($.inArray(extension, options.allowedExtensions) === -1) {
                options.error();
                $(this).focus();
            } else {
                options.success();
            }
        });
    });
};

//$('#image').checkFileType({
//    allowedExtensions: ['jpg', 'jpeg'],
//    success: function() {
//        alert('Success');
//    },
//    error: function() {
//        alert('Error');
//    }
//});

function previewPhoto() {
    //PREVIEW IMAGE
    $("input[type=file][name=File]").on('change', function () {
        if (typeof (FileReader) !== "undefined") {
            var imageHolder = $("#image-holder");
            imageHolder.empty();

            var file = $(this)[0].files[0];
            if (file !== null) {
                var imageExtensions = ['jpg', 'jpeg', 'png', 'gif', 'bmp'];
                var fileName = $(this).val();
                var fileNameLower = fileName.toLowerCase();
                var extension = fileNameLower.substr((fileNameLower.lastIndexOf('.') + 1));
                if ($.inArray(extension, imageExtensions) !== -1) {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $("<img />", {
                            "width": 150,
                            "height": 150,
                            "src": e.target.result,
                            "class": "thumb-image"
                        }).appendTo(imageHolder);
                    };

                    imageHolder.show();
                    reader.readAsDataURL(file);
                    $(this).focus();
                }
            }

            var imageContainer = $("#image-container");
            imageContainer.hide();

            //Check file
            if ($(this).val() !== '' && $(this).val() !== null) {
                $(this).checkFile();
            }
        } else {
            console.log("This browser does not support FileReader.");
        }
    });

    //RESET IMAGE
    $(document).on("click", ".resetPhoto", function () {
        $('#Photo').val('');

        var imageHolder = $("#image-holder");
        imageHolder.hide();

        var imageContainer = $("#image-container");
        imageContainer.show();
    });

    $(document).on("click", ".resetFile", function () {
        $('#File').val('');

        var imageHolder = $("#image-holder");
        imageHolder.hide();

        var imageContainer = $("#image-container");
        imageContainer.show();
    });

    $(document).on("click", ".resetVideo", function () {
        $('.video').val('');

        var videoHolder = $(".video-holder");
        videoHolder.hide();

        var videoContainer = $(".video-container");
        videoContainer.show();
    });
}
//PHOTO-MANAGEMENT END ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

function checkUrl(controlId) {
    $('#' + controlId).keyup(function () {
        var input = $(this);
        if (input.val() !== '') {
            if (input.val().substring(0, 4) === 'www.') {
                input.val('http://www.' + input.val().substring(4));
            }
            var re = /(http|ftp|https):\/\/[\w-]+(\.[\w-]+)+([\w.,@?^=%&:\/~+#-]*[\w@?^=%&\/~+#-])?/;
            var isUrl = re.test(input.val());
            if (isUrl) {
                input.removeClass("invalid").removeClass("error").addClass("valid");
            } else {
                input.removeClass("valid").addClass("invalid error");
            }
        } else {
            input.removeClass("invalid").removeClass("error").addClass("valid");
        }
    });

    $('form').valid();
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

function invokeDateTimePicker(dateFormat) {
    var formatDate = dateFormat.toLowerCase();
    $('.datepicker').datetimepicker({
        format: formatDate,
        weekStart: 1,
        todayBtn: 1,
        clearBtn: 1,
        autoclose: 1,
        todayHighlight: 1,
        startView: 2,
        minView: 2,
        forceParse: 0,
        pickTime: false,
        minDate: '1/1/1900'
    }).on('changeDate', function (e) {
        var selectedDate = '';
        if ($(this).val() !== undefined && $(this).val() !== '') {
            var result = $(this).val();

            var arr = result.split("/");
            if (dateFormat === 'dd/MM/yyyy' || dateFormat === 'dd-MM-yyyy') {
                selectedDate = arr[1] + '/' + arr[0] + '/' + arr[2];
            }
            if (dateFormat === 'MM/dd/yyyy' || dateFormat === 'MM-dd-yyyy') {
                selectedDate = arr[0] + '/' + arr[1] + '/' + arr[2];
            }
            if (dateFormat === 'yyyy/MM/dd' || dateFormat === 'yyyy-MM-dd') {
                selectedDate = arr[2] + '/' + arr[1] + '/' + arr[0];
            }
        }

        $(this).siblings('input').val(selectedDate);
        $(this).datetimepicker('hide');
    });
}