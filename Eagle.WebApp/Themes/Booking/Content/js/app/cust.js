(function ($) {
    function populateRegionDropDownList() {
        var select = $('.Address-RegionId');
        //var selectedValue = select.data('id');
        var url = select.data('url');
        var provinceId = $('.Address-ProvinceId').val();

        var params = { 'provinceId': provinceId, 'selectedValue': null, 'isShowSelectText': true };
        select.empty();

        $.ajax({
            type: "GET",
            url: url,
            data: params,
            beforeSend: function () {
                $.unblockUI();
            },
            success: function (data) {
                if (data.length > 0) {
                    $.each(data, function (index, itemData) {
                        select.append($('<option/>', {
                            value: itemData.Value,
                            text: itemData.Text
                        }));
                    });

                    //if (selectedValue !== null && selectedValue !== '' && selectedValue !== undefined) {
                    //    select.find('option').removeAttr('selected').filter('[value=' + selectedValue + ']').attr('selected', true);
                    //}

                    //select.select2({ width: '100%' });
                } else {
                    select.append($('<option/>', {
                        value: '',
                        text: 'None'
                    }));
                }
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });

        return false;
    }

    function populateProvinceDropDownList() {
        var select = $('.Address-ProvinceId');
        var selectedValue = select.data('id');
        var url = select.data('url');
        var countryId = $('.Address-CountryId').val();

        if (countryId !== null && countryId !== undefined && countryId !== '') {
            var params = { 'countryId': countryId, 'selectedValue': selectedValue, 'isShowSelectText': true };

            select.empty();
            $.ajax({
                type: "GET",
                url: url,
                data: params,
                beforeSend: function () {
                    $.unblockUI();
                },
                success: function (data) {
                    if (data.length > 0) {
                        $.each(data, function (index, itemData) {
                            select.append($('<option/>', {
                                value: itemData.Value,
                                text: itemData.Text
                            }));
                        });

                        if (selectedValue !== null && selectedValue !== '' && selectedValue !== undefined) {
                            select.find('option').removeAttr('selected').filter('[value=' + selectedValue + ']').attr('selected', true);
                        }

                        populateRegionDropDownList();

                        select.select2({ width: '100%' });
                    } else {
                        select.append($('<option/>', {
                            value: '',
                            text: 'None'
                        }));
                    }
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    handleAjaxErrors(jqXhr, textStatus, errorThrown);
                }
            });
        }
    }

    function populateCountryDropDownList() {
        var select = $('.Address-CountryId');
        var selectedValue = select.data('id');
        var url = select.data('url');
        var params = { "selectedValue": selectedValue, "isShowSelectText": true };

        select.empty();

        $.getJSON(url, params,
            function (classesData) {
                if (classesData.length > 0) {
                    $.each(classesData, function (index, itemData) {
                        select.append($('<option/>', {
                            value: itemData.Value,
                            text: itemData.Text
                        }));
                    });
                }

                if (selectedValue !== null && selectedValue !== '' && selectedValue !== undefined) {
                    select.find('option').removeAttr('selected').filter('[value=' + selectedValue + ']').attr('selected', true);
                }

                populateProvinceDropDownList();

                select.select2({ width: '100%' });
                //select.on('change', function () {
                //    $(this).trigger('blur');
                //});
               // select.rules('add', 'required');
            });
    }

    $(document).on("change", ".Address-CountryId", function () {
        populateProvinceDropDownList();
    });

    $(document).on("change", ".Address-ProvinceId", function () {
        populateRegionDropDownList();
    });
    populateCountryDropDownList();

    function resetControls(form) {
        var validateObj = $('#' + form);
        validateObj.find('input:text, input:password, input:file, select, textarea').not('.ignored').val('');
        validateObj.find('input:radio, input:checkbox').removeAttr('checked').removeAttr('selected');
        validateObj.find('input[type="number"]').val(0);
        validateObj.find('select').find('option:first').attr('selected', true).siblings().attr('selected', false);
        validateObj.find("[data-valmsg-summary=true]").removeClass("validation-summary-errors").addClass("validation-summary-valid").find("ul").empty();
        validateObj.find("[data-valmsg-replace]").removeClass("field-validation-error").addClass("field-validation-valid").empty();
    }

    function login(url, formId) {
        var formData = $("#" + formId).serialize();

        $.ajax({
            type: "POST",
            url: url,
            data: formData,
            dataType: "json",
            beforeSend: function () {
                $.unblockUI();
            },
            success: function (response) {
                if (response.Status === 0) {
                    //console.log("login successfully");
                    window.location.href = response.Data.ReturnedUrl;
                    return false;
                } else {
                    if (response.Errors !== null) {
                        var result = '';
                        $.each(response.Errors, function (i, obj) {
                            result += obj.ErrorMessage + '<br/>';
                        });

                        showMessageWithTitleByContainerId('alertMessageBox-loginform', 500, result, "error", 50000);
                    }
                    return false;
                }
            }, error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrorMessages(formId, jqXhr, textStatus, errorThrown);
                return false;
            }
        });
    }

    function signUp(url, formId) {
        var formData = $("#" + formId).serialize();
        $.ajax({
            type: "POST",
            url: url,
            data: formData,
            dataType: "json",
            beforeSend: function () {
                $.unblockUI();
            },
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    showMessageWithTitleByContainerId("alertMessageBox-frmCreateCustomer", jqXhr.status, response.Data.Message, "success", 20000);

                    $(this).attr('disabled', true);

                    var returnedUrl = getParameterByName('desiredUrl');
                    if (returnedUrl === null || returnedUrl === '' || returnedUrl === 'undefined') {
                        returnedUrl = response.Data.ReturnedUrl;
                    }             
                    
                    //console.log(returnedUrl);
                    window.location.href = returnedUrl;
                } else {
                    if (response.Errors !== null) {
                        var result = '';
                        $.each(response.Errors, function (i, obj) {
                            result += obj.ErrorMessage + '<br/>';
                        });
                        
                        showMessageWithTitleByContainerId("alertMessageBox-frmCreateCustomer", 500, result, "error", 50000);
                    }
                }
            }, error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrorMessages(formId, jqXhr, textStatus, errorThrown);
            }
        });
        return false;
    }

    function changePassword(url, form) {
        var formData = $("#" + form).serialize();
        $.ajax({
            type: "POST",
            url: url,
            data: formData,
            beforeSend: function () {
                $.unblockUI();
            },
            success: function (data) {
                $('.preload').remove();
                $('#ForgetOld').remove();
                $('#ForgetPassword').html(data);
            }
        });
    }

    $(document).on("click", ".registerCustomer", function (e) {
        e.preventDefault();

        var formId = $(this).data('form');
        var url = $(this).data('url');

        if (!$("#" + formId).valid()) { // Not Valid
            return false;
        } else {
            signUp(url, formId);
            return false;
        }
    });

    $(document).on("click", ".reset", function () {
        var form = $(this).data('form');
        resetControls(form);
        return false;
    });

    $(document).on("click", ".login", function () {
        var url = $(this).data("url");
        var formId = $(this).data("form");

        if (!$("#" + formId).valid()) { // Not Valid
            return false;
        } else {
            login(url, formId);
        }
        return false;
    });
    $('#UserName').focus();

    $(document).on("click", "#changePassword", function () {
        var url = $(this).data("url");
        var formId = $(this).data("form");

        if (!$("#" + formId).valid()) { // Not Valid
            return false;
        } else {
            changePassword(url, formId);
        }
        return false;
    });

    $(document).keypress(function (event) {
        var keycode = (event.keyCode ? event.keyCode : event.which);
        if (keycode === '13') {
            if ($('#UserName').val() !== '' && $('#Password') !== '')
                $('#btnLogin').click();
        }
    });
})(jQuery);