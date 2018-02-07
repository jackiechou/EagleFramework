(function ($) {
    //============================================================
    //ORDER ======================================================
    //============================================================
    function getOrderSettings(url) {
        $.ajax({
            type: "GET",
            url: url,
            success: function (data) {
                $('#order-container').html(data);
                return false;
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    function editOrderSetting(url, formId) {
        var params = $("#" + formId).serialize();
        var container = $("#alertMessageBox-" + formId);

        $.ajax({
            type: "PUT",
            url: url,
            data: params,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    searchSetting();
                    showMessageWithTitleByContainer(container, jqXhr.status, response.Data.Message, "success", 20000);
                } else {
                    if (response.Errors !== null) {
                        var result = '';
                        $.each(response.Errors, function (i, obj) {
                            result += obj.ErrorMessage + '<br/>';
                        });
                        showMessageWithTitleByContainer(container, 500, result, "error", 50000);
                    }
                }
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    $(document).on("click", ".editOrderSetting", function (e) {
        e.preventDefault();
        var formId = $(this).data('form');
        var url = $(this).data('url');

        if (!$("#" + formId).valid()) { // Not Valid
            return false;
        } else {
            editOrderSetting(url, formId);
            return false;
        }
    });
    
    $(document).on("click", ".resetOrderSetting", function () {
        var url = $(this).data('url');
        getOrderSettings(url);
        return false;
    });
    
    //============================================================
    //GOOGLE RECAPTCHA ===========================================
    //============================================================
    function getRecaptchaSettings(url) {
        $.ajax({
            type: "GET",
            url: url,
            success: function (data) {
                $('#recaptcha-container').html(data);
                return false;
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    function editRecaptchaSetting(url, formId) {
        var params = $("#" + formId).serialize();
        var container = $("#alertMessageBox-" + formId);

        $.ajax({
            type: "PUT",
            url: url,
            data: params,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    searchSetting();
                    showMessageWithTitleByContainer(container, jqXhr.status, response.Data.Message, "success", 20000);
                } else {
                    if (response.Errors !== null) {
                        var result = '';
                        $.each(response.Errors, function (i, obj) {
                            result += obj.ErrorMessage + '<br/>';
                        });
                        showMessageWithTitleByContainer(container, 500, result, "error", 50000);
                    }
                }
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    $(document).on("click", ".editRecaptchaSetting", function (e) {
        e.preventDefault();
        var formId = $(this).data('form');
        var url = $(this).data('url');

        if (!$("#" + formId).valid()) { // Not Valid
            return false;
        } else {
            editRecaptchaSetting(url, formId);
            return false;
        }
    });
    
    $(document).on("click", ".resetRecaptchaSetting", function () {
        var url = $(this).data('url');
        getRecaptchaSettings(url);
        return false;
    });
    
   
    //=====================================================================
    //PAYGATE =============================================================
    //=====================================================================
    function getPayGates() {
        var url = '/admin/application/GetPayGateSettings';
        $.ajax({
            type: "GET",
            url: url,
            data: null,
            ContentType: 'application/json;utf-8',
            datatype: 'json',
            success: function (data) {
                $('#paygate-container').html(data);
                return false;
            },
            error: function (jqXhr, textStatus, errorThrown) {
                $('#paygate-container').html('<span>' + textStatus + ", " + errorThrown + '</span>');
            }
        });
        return false;
    }

    function selectPayGate(url, id) {
        var params = { "id": id };

        $.ajax({
            type: "PUT",
            url: url,
            data: params,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    getPayGates();
                    showMessageWithTitle(jqXhr.status, response.Data, "success", 20000);
                } else {
                    if (response.Errors !== null) {
                        var result = '';
                        $.each(response.Errors, function (i, obj) {
                            result += obj.ErrorMessage + '<br/>';
                        });
                        showMessageWithTitle(500, result, "error", 50000);
                    }
                }
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    function updateStatus(url, id, status) {
        var params = { "id": id, "status": status };

        $.ajax({
            type: "PUT",
            url: url,
            data: params,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    getPayGates();
                    showMessageWithTitle(jqXhr.status, response.Data, "success", 20000);
                } else {
                    if (response.Errors !== null) {
                        var result = '';
                        $.each(response.Errors, function (i, obj) {
                            result += obj.ErrorMessage + '<br/>';
                        });
                        showMessageWithTitle(500, result, "error", 50000);
                    }
                }
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }


    $(document).on("change", ".changeStatus", function (e) {
        e.preventDefault();

        var id = $(this).data('id');
        var url = $(this).data('url');
        var warning = $(this).data('warning');
        var status = $(this).is(":checked");

        var box = bootbox.confirm({
            className: "my-modal",
            size: 'small',
            title: window.Warning,
            message: warning,
            buttons: {
                confirm: {
                    label: window.Ok,
                    className: 'confirm-button-class'
                },
                cancel: {
                    label: window.Cancel,
                    className: 'cancel-button-class'
                }
            },
            callback: function (result) {
                if (result) {
                    updateStatus(url, id, status);
                    box.modal('hide');
                }
            },
            onEscape: function () { return false; }
        });

        box.css({
            'top': '50%',
            'margin-top': function () {
                return -(box.height() / 2);
            }
        });
        return false;
    });


    $(document).on("click", ".selectPayGate", function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var url = $(this).data('url');
        var warning = $(this).data('warning');

        var box = bootbox.confirm({
            className: "my-modal",
            size: 'small',
            title: window.Warning,
            message: warning,
            buttons: {
                confirm: {
                    label: window.Ok,
                    className: 'confirm-button-class'
                },
                cancel: {
                    label: window.Cancel,
                    className: 'cancel-button-class'
                }
            },
            callback: function (result) {
                if (result) {
                    selectPayGate(url, id);
                    box.modal('hide');
                }
            },
            onEscape: function () { return false; }
        });

        box.css({
            'top': '50%',
            'margin-top': function () {
                return -(box.height() / 2);
            }
        });
        return false;
    });

    //$(document).on("click", ".resetPayGate", function () {
    //    var url = $(this).data('url');
    //    getPayGates(url);
    //    return false;
    //});

    //=====================================================================
    //PAYPAL =============================================================
    //=====================================================================
    //function toggleGateMode() {
    //    $('#chkGateMode').bootstrapToggle('off');
    //    $('#chkGateMode').change(function () {
    //        var checkBoxStatus = $(this).is(":checked");                   
    //        console.log(checkBoxStatus);
    //    });
    //}
    //toggleGateMode();

    function getPayPalSettings(url, mode) {
        $.ajax({
            type: "GET",
            url: url,
            data: { "mode": mode },
            success: function (data) {
                $('#'+mode+'-container').html(data);
                return false;
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    function editPayPalSetting(url, formId) {
        var params = $("#" + formId).serialize();
        var container = $("#alertMessageBox-" + formId);

        $.ajax({
            type: "PUT",
            url: url,
            data: params,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    searchSetting();
                    showMessageWithTitleByContainer(container, jqXhr.status, response.Data.Message, "success", 20000);
                } else {
                    if (response.Errors !== null) {
                        var result = '';
                        $.each(response.Errors, function (i, obj) {
                            result += obj.ErrorMessage + '<br/>';
                        });
                        showMessageWithTitleByContainer(container, 500, result, "error", 50000);
                    }
                }
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    function editPayPalMode(url, mode) {
        var params = { "mode": mode };
        var container = $("#alertMessageBox-frmPayPal" + mode);

        $.ajax({
            type: "PUT",
            url: url,
            data: params,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    searchSetting();
                    showMessageWithTitleByContainer(container, jqXhr.status, response.Data.Message, "success", 20000);
                } else {
                    if (response.Errors !== null) {
                        var result = '';
                        $.each(response.Errors, function (i, obj) {
                            result += obj.ErrorMessage + '<br/>';
                        });
                        showMessageWithTitleByContainer(container, 500, result, "error", 50000);
                    }
                }
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    $("input[type='radio'][name='payPalMode']").change(function () {
        //var mode = $("input[type='radio'][name='payPalMode']:checked").val();
        var mode = $(this).val();
        var url = $(this).data('url');
        editPayPalMode(url, mode);
        return false;
    });

    $(document).on("click", ".editPayPal", function (e) {
        e.preventDefault();
        var formId = $(this).data('form');
        var url = $(this).data('url');

        if (!$("#" + formId).valid()) { // Not Valid
            return false;
        } else {
            editPayPalSetting(url, formId);
            return false;
        }
    });

    $(document).on("click", ".resetPayPal", function () {
        var mode = $(this).data('mode');
        var url = $(this).data('url');
        getPayPalSettings(url, mode);
        return false;
    });

    $('#tabstrip a').click(function(e) {
        e.preventDefault();
        var tabId = $(this).attr("href").substr(1);
        var url = $(this).data("url");
        $("#" + tabId).load(url);
        $(this).tab('show');
        return false;
    });

    //===================================================================
    //SMTP - NOTIFICATION================================================
    //===================================================================
    function getSenders() {
        var select = $(".notification-sender-select");
        var url = '/admin/application/PopulateNotificationSenderSelectList';
        var mailServerProviderId = $(".mail-server-provider-select").val();

        var params = {
            "mailServerProviderId": mailServerProviderId,
            "selectedValue": null
        };
        select.empty();
        $.ajax({
            async: false,
            cache: false,
            type: 'GET',
            dataType: "json",
            url: url,
            data: params,
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            },
            success: function (data) {
                if (data.length > 0) {
                    $.each(data, function (index, itemData) {
                        select.append($('<option/>', {
                            value: itemData.Value,
                            text: itemData.Text,
                            selected: itemData.Selected
                        }));
                    });
                } else {
                    select.append($('<option/>', { value: 'Null', text: " ---" + window.Select + " --- " }));
                }
                return false;
            }
        });
    }

    $(document).on("change", ".mail-server-provider-select", function (e) {
        e.preventDefault();
        var selectedValue = $(this).find(":selected").val();
        $(this).find('option').removeAttr('selected').filter('[value=' + selectedValue + ']').attr('selected', true);

      // console.log(selectedValue);
        getSenders();
        return false;
    });

    function getSmtpSettings(url) {
        $.ajax({
            type: "GET",
            url: url,
            success: function (data) {
                $('#smtp-container').html(data);
                return false;
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    function editSmtpSetting(url, formId) {
        var params = $("#" + formId).serialize();
        var container = $("#alertMessageBox-" + formId);

        $.ajax({
            type: "PUT",
            url: url,
            data: params,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    searchSetting();
                    showMessageWithTitleByContainer(container, jqXhr.status, response.Data.Message, "success", 20000);
                } else {
                    if (response.Errors !== null) {
                        var result = '';
                        $.each(response.Errors, function (i, obj) {
                            result += obj.ErrorMessage + '<br/>';
                        });
                        showMessageWithTitleByContainer(container, 500, result, "error", 50000);
                    }
                }
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    $(document).on("click", ".editSmtpSetting", function (e) {
        e.preventDefault();
        var formId = $(this).data('form');
        var url = $(this).data('url');

        if (!$("#" + formId).valid()) { // Not Valid
            return false;
        } else {
            editSmtpSetting(url, formId);
            return false;
        }
    });

    $(document).on("click", ".resetSmtpSetting", function () {
        var url = $(this).data('url');
        getSmtpSettings(url);
        return false;
    });


    //===================================================================
    //ADDRESS ================================================
    //===================================================================
    function getProvinces() {
        var select = $(".province-select");
        var url = '/admin/application/PopulateProvinceSelectList';
        var countryId = $(".country-select").val();

        var params = {
            "countryId": countryId,
            "selectedValue": null
        };
        select.empty();
        $.ajax({
            async: false,
            cache: false,
            type: 'GET',
            dataType: "json",
            url: url,
            data: params,
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            },
            success: function (data) {
                if (data.length > 0) {
                    $.each(data, function (index, itemData) {
                        select.append($('<option/>', {
                            value: itemData.Value,
                            text: itemData.Text,
                            selected: itemData.Selected
                        }));
                    });
                } else {
                    select.append($('<option/>', { value: 'Null', text: " ---" + window.Select + " --- " }));
                }
                return false;
            }
        });
    }

    $(document).on("change", ".country-select", function (e) {
        e.preventDefault();
        var selectedValue = $(this).find(":selected").val();
        $(this).find('option').removeAttr('selected').filter('[value=' + selectedValue + ']').attr('selected', true);

        // console.log(selectedValue);
        getProvinces();
        return false;
    });

    function getAddressSettings(url) {
        $.ajax({
            type: "GET",
            url: url,
            success: function (data) {
                $('#address-container').html(data);
                return false;
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    function editAddressSetting(url, formId) {
        var params = $("#" + formId).serialize();
        var container = $("#alertMessageBox-" + formId);

        $.ajax({
            type: "PUT",
            url: url,
            data: params,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    searchSetting();
                    showMessageWithTitleByContainer(container, jqXhr.status, response.Data.Message, "success", 20000);
                } else {
                    if (response.Errors !== null) {
                        var result = '';
                        $.each(response.Errors, function (i, obj) {
                            result += obj.ErrorMessage + '<br/>';
                        });
                        showMessageWithTitleByContainer(container, 500, result, "error", 50000);
                    }
                }
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    $(document).on("click", ".editAddressSetting", function (e) {
        e.preventDefault();
        var formId = $(this).data('form');
        var url = $(this).data('url');

        if (!$("#" + formId).valid()) { // Not Valid
            return false;
        } else {
            editAddressSetting(url, formId);
            return false;
        }
    });

    $(document).on("click", ".resetAddressSetting", function () {
        var url = $(this).data('url');
        getAddressSettings(url);
        return false;
    });


    //====================================================================
    //RATINGS ============================================================
    //====================================================================
    function getRatings(url) {
        $.ajax({
            type: "GET",
            url: url,
            success: function (data) {
                $('#rating-container').html(data);
                return false;
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    function editRating(url, formId) {
        var params = $("#" + formId).serialize();
        var container = $("#alertMessageBox-" + formId);

        $.ajax({
            type: "PUT",
            url: url,
            data: params,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    searchSetting();
                    showMessageWithTitleByContainer(container, jqXhr.status, response.Data.Message, "success", 20000);
                } else {
                    if (response.Errors !== null) {
                        var result = '';
                        $.each(response.Errors, function (i, obj) {
                            result += obj.ErrorMessage + '<br/>';
                        });
                        showMessageWithTitleByContainer(container, 500, result, "error", 50000);
                    }
                }
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    $(document).on("click", ".editRating", function (e) {
        e.preventDefault();
        var formId = $(this).data('form');
        var url = $(this).data('url');

        if (!$("#" + formId).valid()) { // Not Valid
            return false;
        } else {
            editRating(url, formId);
            return false;
        }
    });

    $(document).on("click", ".resetRating", function () {
        var url = $(this).data('url');
        getRatings(url);
        return false;
    });

    //====================================================================
    //PRODUCT FILTERED PRICE RANGE========================================
    //====================================================================
    function getProductFilteredPriceRangeSettings(url) {
        $.ajax({
            type: "GET",
            url: url,
            success: function (data) {
                $('#product-filtered-price-range-container').html(data);
                return false;
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    function editProductFilteredPriceRangeSetting(url, formId) {
        var params = $("#" + formId).serialize();
        var container = $("#alertMessageBox-" + formId);

        $.ajax({
            type: "PUT",
            url: url,
            data: params,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    searchSetting();
                    showMessageWithTitleByContainer(container, jqXhr.status, response.Data.Message, "success", 20000);
                } else {
                    if (response.Errors !== null) {
                        var result = '';
                        $.each(response.Errors, function (i, obj) {
                            result += obj.ErrorMessage + '<br/>';
                        });
                        showMessageWithTitleByContainer(container, 500, result, "error", 50000);
                    }
                }
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    $(document).on("click", ".editProductFilteredPriceRangeSetting", function (e) {
        e.preventDefault();
        var formId = $(this).data('form');
        var url = $(this).data('url');

        if (!$("#" + formId).valid()) { // Not Valid
            return false;
        } else {
            editProductFilteredPriceRangeSetting(url, formId);
            return false;
        }
    });

    $(document).on("click", ".resetProductFilteredPriceRangeSetting", function () {
        var url = $(this).data('url');
        getProductFilteredPriceRangeSettings(url);
        return false;
    });
   
    //====================================================================
    //PRODUCT PRICE RANGE=================================================
    //====================================================================
    function getProductPriceRangeSettings(url) {
        $.ajax({
            type: "GET",
            url: url,
            success: function (data) {
                $('#product-price-range-container').html(data);
                return false;
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    function editProductPriceRangeSetting(url, formId) {
        var params = $("#" + formId).serialize();
        var container = $("#alertMessageBox-" + formId);

        $.ajax({
            type: "PUT",
            url: url,
            data: params,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    searchSetting();
                    showMessageWithTitleByContainer(container, jqXhr.status, response.Data.Message, "success", 20000);
                } else {
                    if (response.Errors !== null) {
                        var result = '';
                        $.each(response.Errors, function (i, obj) {
                            result += obj.ErrorMessage + '<br/>';
                        });
                        showMessageWithTitleByContainer(container, 500, result, "error", 50000);
                    }
                }
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    $(document).on("click", ".editProductPriceRangeSetting", function (e) {
        e.preventDefault();
        var formId = $(this).data('form');
        var url = $(this).data('url');

        if (!$("#" + formId).valid()) { // Not Valid
            return false;
        } else {
            editProductPriceRangeSetting(url, formId);
            return false;
        }
    });
    
    $(document).on("click", ".resetProductPriceRangeSetting", function () {
        var url = $(this).data('url');
        getProductPriceRangeSettings(url);
        return false;
    });
    

    //====================================================================
    //CACHE ==============================================================
    //====================================================================
    function clearCache(url, formId) {
        var params = $("#" + formId).serialize();
        var container = $("#alertMessageBox-" + formId);

        $.ajax({
            type: "POST",
            url: url,
            data: params,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    showMessageWithTitleByContainer(container, jqXhr.status, response.Data.Message, "success", 20000);
                } else {
                    if (response.Errors !== null) {
                        var result = '';
                        $.each(response.Errors, function (i, obj) {
                            result += obj.ErrorMessage + '<br/>';
                        });
                        showMessageWithTitleByContainer(container, 500, result, "error", 50000);
                    }
                }
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    $(document).on("click", ".clear-cache", function () {
        var formId = $(this).data('form');
        var url = $(this).data('url');
        clearCache(url, formId);
        return false;
    });

    //====================================================================
    //FILE CONFIGURATION =================================================
    //====================================================================
    function getFileConfigSettings(url) {
        $.ajax({
            type: "GET",
            url: url,
            success: function (data) {
                $('#file-config-container').html(data);
                return false;
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    function editFileConfigSetting(url, formId) {
        var params = $("#" + formId).serialize();
        var container = $("#alertMessageBox-" + formId);

        $.ajax({
            type: "PUT",
            url: url,
            data: params,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    searchSetting();
                    showMessageWithTitleByContainer(container, jqXhr.status, response.Data.Message, "success", 20000);
                } else {
                    if (response.Errors !== null) {
                        var result = '';
                        $.each(response.Errors, function (i, obj) {
                            result += obj.ErrorMessage + '<br/>';
                        });
                        showMessageWithTitleByContainer(container, 500, result, "error", 50000);
                    }
                }
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    $(document).on("click", ".editFileConfigSetting", function (e) {
        e.preventDefault();
        var formId = $(this).data('form');
        var url = $(this).data('url');

        if (!$("#" + formId).valid()) { // Not Valid
            return false;
        } else {
            editFileConfigSetting(url, formId);
            return false;
        }
    });

    $(document).on("click", ".resetFileConfigSetting", function () {
        var url = $(this).data('url');
        getFileConfigSettings(url);
        return false;
    });

    //====================================================================
    //PAGE CONFIGURATION =================================================
    //====================================================================
    function getPageConfigSettings(url) {
        $.ajax({
            type: "GET",
            url: url,
            success: function (data) {
                $('#page-config-container').html(data);
                return false;
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    function editPageConfigSetting(url, formId) {
        var params = $("#" + formId).serialize();
        var container = $("#alertMessageBox-" + formId);

        $.ajax({
            type: "PUT",
            url: url,
            data: params,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    searchSetting();
                    showMessageWithTitleByContainer(container, jqXhr.status, response.Data.Message, "success", 20000);
                } else {
                    if (response.Errors !== null) {
                        var result = '';
                        $.each(response.Errors, function (i, obj) {
                            result += obj.ErrorMessage + '<br/>';
                        });
                        showMessageWithTitleByContainer(container, 500, result, "error", 50000);
                    }
                }
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    $(document).on("click", ".editPageConfigSetting", function (e) {
        e.preventDefault();
        var formId = $(this).data('form');
        var url = $(this).data('url');

        if (!$("#" + formId).valid()) { // Not Valid
            return false;
        } else {
            editPageConfigSetting(url, formId);
            return false;
        }
    });

    $(document).on("click", ".resetPageConfigSetting", function () {
        var url = $(this).data('url');
        getPageConfigSettings(url);
        return false;
    });

    //====================================================================
    //DATE-TIME CONFIGURATION =================================================
    //====================================================================
    function getDateTimeConfigSettings(url) {
        $.ajax({
            type: "GET",
            url: url,
            success: function (data) {
                $('#date-time-config-container').html(data);
                return false;
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    function editDateTimeConfigSetting(url, formId) {
        var params = $("#" + formId).serialize();
        var container = $("#alertMessageBox-" + formId);

        $.ajax({
            type: "PUT",
            url: url,
            data: params,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    searchSetting();
                    showMessageWithTitleByContainer(container, jqXhr.status, response.Data.Message, "success", 20000);
                } else {
                    if (response.Errors !== null) {
                        var result = '';
                        $.each(response.Errors, function (i, obj) {
                            result += obj.ErrorMessage + '<br/>';
                        });
                        showMessageWithTitleByContainer(container, 500, result, "error", 50000);
                    }
                }
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    $(document).on("click", ".editDateTimeConfigSetting", function (e) {
        e.preventDefault();
        var formId = $(this).data('form');
        var url = $(this).data('url');

        if (!$("#" + formId).valid()) { // Not Valid
            return false;
        } else {
            editDateTimeConfigSetting(url, formId);
            return false;
        }
    });

    $(document).on("click", ".resetDateTimeConfigSetting", function () {
        var url = $(this).data('url');
        getDateTimeConfigSettings(url);
        return false;
    });


    //====================================================================
    //FACEBOOK============================================================
    //====================================================================
    function getFacebookSettings(url) {
        $.ajax({
            type: "GET",
            url: url,
            success: function (data) {
                $('#facebook-container').html(data);
                return false;
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    function editFacebookSetting(url, formId) {
        var params = $("#" + formId).serialize();
        var container = $("#alertMessageBox-" + formId);

        $.ajax({
            type: "PUT",
            url: url,
            data: params,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    searchSetting();
                    showMessageWithTitleByContainer(container, jqXhr.status, response.Data.Message, "success", 20000);
                } else {
                    if (response.Errors !== null) {
                        var result = '';
                        $.each(response.Errors, function (i, obj) {
                            result += obj.ErrorMessage + '<br/>';
                        });
                        showMessageWithTitleByContainer(container, 500, result, "error", 50000);
                    }
                }
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    $(document).on("click", ".editFacebookSetting", function (e) {
        e.preventDefault();
        var formId = $(this).data('form');
        var url = $(this).data('url');

        if (!$("#" + formId).valid()) { // Not Valid
            return false;
        } else {
            editFacebookSetting(url, formId);
            return false;
        }
    });
    
    $(document).on("click", ".resetFacebookSetting", function () {
        var url = $(this).data('url');
        getFacebookSettings(url);
        return false;
    });

    //=====================================================================
    //Twitter==============================================================
    //=====================================================================
    function getTwitterSettings(url) {
        $.ajax({
            type: "GET",
            url: url,
            success: function (data) {
                $('#twitter-container').html(data);
                return false;
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    function editTwitterSetting(url, formId) {
        var params = $("#" + formId).serialize();
        var container = $("#alertMessageBox-" + formId);

        $.ajax({
            type: "PUT",
            url: url,
            data: params,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    searchSetting();
                    showMessageWithTitleByContainer(container, jqXhr.status, response.Data.Message, "success", 20000);
                } else {
                    if (response.Errors !== null) {
                        var result = '';
                        $.each(response.Errors, function (i, obj) {
                            result += obj.ErrorMessage + '<br/>';
                        });
                        showMessageWithTitleByContainer(container, 500, result, "error", 50000);
                    }
                }
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    $(document).on("click", ".editTwitterSetting", function (e) {
        e.preventDefault();
        var formId = $(this).data('form');
        var url = $(this).data('url');

        if (!$("#" + formId).valid()) { // Not Valid
            return false;
        } else {
            editTwitterSetting(url, formId);
            return false;
        }
    });
    
    $(document).on("click", ".resetTwitterSetting", function () {
        var url = $(this).data('url');
        getTwitterSettings(url);
        return false;
    });
    
    //=======================================================================
    //Currency===============================================================
    //=======================================================================
    function getCurrencySettings(url) {
        $.ajax({
            type: "GET",
            url: url,
            success: function (data) {
                $('#currency-container').html(data);
                return false;
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    function editCurrencySetting(url, formId) {
        var params = $("#" + formId).serialize();
        var container = $("#alertMessageBox-" + formId);

        $.ajax({
            type: "PUT",
            url: url,
            data: params,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    searchSetting();
                    showMessageWithTitleByContainer(container, jqXhr.status, response.Data.Message, "success", 20000);
                } else {
                    if (response.Errors !== null) {
                        var result = '';
                        $.each(response.Errors, function (i, obj) {
                            result += obj.ErrorMessage + '<br/>';
                        });
                        showMessageWithTitleByContainer(container, 500, result, "error", 50000);
                    }
                }
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    $(document).on("click", ".editCurrencySetting", function (e) {
        e.preventDefault();
        var formId = $(this).data('form');
        var url = $(this).data('url');

        if (!$("#" + formId).valid()) { // Not Valid
            return false;
        } else {
            editCurrencySetting(url, formId);
            return false;
        }
    });
    
    $(document).on("click", ".resetCurrencySetting", function () {
        var url = $(this).data('url');
        getCurrencySettings(url);
        return false;
    });

    //=======================================================================
    //Language===============================================================
    //=======================================================================
    function getLanguageSettings(url) {
        $.ajax({
            type: "GET",
            url: url,
            success: function (data) {
                $('#language-container').html(data);
                return false;
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    function editLanguageSetting(url, formId) {
        var params = $("#" + formId).serialize();
        var container = $("#alertMessageBox-" + formId);

        $.ajax({
            type: "PUT",
            url: url,
            data: params,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    searchSetting();
                    showMessageWithTitleByContainer(container, jqXhr.status, response.Data.Message, "success", 20000);
                } else {
                    if (response.Errors !== null) {
                        var result = '';
                        $.each(response.Errors, function (i, obj) {
                            result += obj.ErrorMessage + '<br/>';
                        });
                        showMessageWithTitleByContainer(container, 500, result, "error", 50000);
                    }
                }
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    $(document).on("click", ".editLanguageSetting", function (e) {
        e.preventDefault();
        var formId = $(this).data('form');
        var url = $(this).data('url');

        if (!$("#" + formId).valid()) { // Not Valid
            return false;
        } else {
            editLanguageSetting(url, formId);
            return false;
        }
    });

    $(document).on("click", ".resetLanguageSetting", function () {
        var url = $(this).data('url');
        getLanguageSettings(url);
        return false;
    });


    //=======================================================================
    //Delivery===============================================================
    //=======================================================================
    function getDeliverySettings(url) {
        $.ajax({
            type: "GET",
            url: url,
            success: function (data) {
                $('#delivery-container').html(data);
                return false;
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    function editDeliverySetting(url, formId) {
        var params = $("#" + formId).serialize();
        var container = $("#alertMessageBox-" + formId);

        $.ajax({
            type: "PUT",
            url: url,
            data: params,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    searchSetting();
                    showMessageWithTitleByContainer(container, jqXhr.status, response.Data.Message, "success", 20000);
                } else {
                    if (response.Errors !== null) {
                        var result = '';
                        $.each(response.Errors, function (i, obj) {
                            result += obj.ErrorMessage + '<br/>';
                        });
                        showMessageWithTitleByContainer(container, 500, result, "error", 50000);
                    }
                }
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    function updateDeliverySettingStatus(url, id, status) {
        var params = { "id": id, "status": status };
        var container = $("#alertMessageBox-frmDeliverySetting");

        $.ajax({
            type: "PUT",
            url: url,
            data: params,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    searchSetting();
                    showMessageWithTitleByContainer(container, jqXhr.status, response.Data.Message, "success", 20000);
                } else {
                    if (response.Errors !== null) {
                        var result = '';
                        $.each(response.Errors, function (i, obj) {
                            result += obj.ErrorMessage + '<br/>';
                        });
                        showMessageWithTitle(500, result, "error", 50000);
                    }
                }
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }


    $(document).on("click", ".editDeliverySetting", function (e) {
        e.preventDefault();
        var formId = $(this).data('form');
        var url = $(this).data('url');

        if (!$("#" + formId).valid()) { // Not Valid
            return false;
        } else {
            editDeliverySetting(url, formId);
            return false;
        }
    });

    $(document).on("click", ".resetDeliverySetting", function () {
        var url = $(this).data('url');
        getDeliverySettings(url);
        return false;
    });
    
    $(document).on("change", ".changeDeliverySettingStatus", function (e) {
        e.preventDefault();

        var id = $(this).data('id');
        var url = $(this).data('url');
        var warning = $(this).data('warning');
        var status = $(this).val();

        var box = bootbox.confirm({
            className: "my-modal",
            size: 'small',
            title: window.Warning,
            message: warning,
            buttons: {
                confirm: {
                    label: window.Ok,
                    className: 'confirm-button-class'
                },
                cancel: {
                    label: window.Cancel,
                    className: 'cancel-button-class'
                }
            },
            callback: function (result) {
                if (result) {
                    updateDeliverySettingStatus(url, id, status);
                    box.modal('hide');
                }
            },
            onEscape: function () { return false; }
        });

        box.css({
            'top': '50%',
            'margin-top': function () {
                return -(box.height() / 2);
            }
        });
        return false;
    });

    //=======================================================================
    //Notification ===============================================================
    //=======================================================================
    function getNotificationSettings(url) {
        $.ajax({
            type: "GET",
            url: url,
            success: function (data) {
                $('#notificaion-container').html(data);
                return false;
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    function editNotificationSetting(url, formId) {
        var params = $("#" + formId).serialize();
        var container = $("#alertMessageBox-" + formId);

        $.ajax({
            type: "PUT",
            url: url,
            data: params,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    searchSetting();
                    showMessageWithTitleByContainer(container, jqXhr.status, response.Data.Message, "success", 20000);
                } else {
                    if (response.Errors !== null) {
                        var result = '';
                        $.each(response.Errors, function (i, obj) {
                            result += obj.ErrorMessage + '<br/>';
                        });
                        showMessageWithTitleByContainer(container, 500, result, "error", 50000);
                    }
                }
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    function updateNotificationSettingStatus(url, id, status) {
        var params = { "id": id, "status": status };
        var container = $("#alertMessageBox-frmNotificationSetting");

        $.ajax({
            type: "PUT",
            url: url,
            data: params,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    searchSetting();
                    showMessageWithTitleByContainer(container, jqXhr.status, response.Data.Message, "success", 20000);
                } else {
                    if (response.Errors !== null) {
                        var result = '';
                        $.each(response.Errors, function (i, obj) {
                            result += obj.ErrorMessage + '<br/>';
                        });
                        showMessageWithTitle(500, result, "error", 50000);
                    }
                }
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }


    $(document).on("click", ".editNotificationSetting", function (e) {
        e.preventDefault();
        var formId = $(this).data('form');
        var url = $(this).data('url');

        if (!$("#" + formId).valid()) { // Not Valid
            return false;
        } else {
            editNotificationSetting(url, formId);
            return false;
        }
    });

    $(document).on("click", ".resetNotificationSetting", function () {
        var url = $(this).data('url');
        getNotificationSettings(url);
        return false;
    });

    $(document).on("change", ".changeNotificationSettingStatus", function (e) {
        e.preventDefault();

        var id = $(this).data('id');
        var url = $(this).data('url');
        var warning = $(this).data('warning');
        var status = $(this).val();

        var box = bootbox.confirm({
            className: "my-modal",
            size: 'small',
            title: window.Warning,
            message: warning,
            buttons: {
                confirm: {
                    label: window.Ok,
                    className: 'confirm-button-class'
                },
                cancel: {
                    label: window.Cancel,
                    className: 'cancel-button-class'
                }
            },
            callback: function (result) {
                if (result) {
                    updateNotificationSettingStatus(url, id, status);
                    box.modal('hide');
                }
            },
            onEscape: function () { return false; }
        });

        box.css({
            'top': '50%',
            'margin-top': function () {
                return -(box.height() / 2);
            }
        });
        return false;
    });


    //=======================================================================
    //POPULATE POPUP ========================================================
    //=======================================================================
    function showPopUp(title, content) {
        var dialog = bootbox.dialog({
            className: "my-modal",
            title: title,
            message: content,
            backdrop: true,
            closeButton: true,
            onEscape: true,
        }).find("div.modal-dialog").css({ "width": "80%" });

        dialog.css({
            'top': '10%',
            'margin-top': function () {
                return -($(this).height() / 2);
            }
        });

        return false;
    }

    $(document).on("click", ".populateCreateSettingFormPopup", function () {
        var title = $(this).data('title');
        var url = $(this).data('url');

        $.ajax({
            type: "GET",
            dataType: "html",
            url: url,
            success: function (data) {              
                showPopUp(title, data);
            }, error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });

        return false;
    });

    $(document).on("click", ".populateEditSettingFormPopup", function () {
        var id = $(this).data('id');
        var title = $(this).data('title');
        var url = $(this).data('url');

        $.ajax({
            type: "GET",
            data: { "id": id },
            dataType: "html",
            url: url,
            success: function (data) {
                showPopUp(title, data);
                handleCheckBoxEvent();
                handleRadios();
            }, error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });

        return false;
    });

    //=======================================================================
    //SETTING ===============================================================
    //=======================================================================
    function searchSetting() {
        var formUrl = window.GetAdvancedSettingsUrl;
        var filterRequest = $("#frmSearch").serialize().replace(/[^&]+=&/g, '').replace(/&[^&]+=$/g, '');
        $.ajax({
            type: "GET",
            url: formUrl,
            data: filterRequest,
            ContentType: 'application/json;utf-8',
            datatype: 'json',
            success: function (data) {
                $('#advanced-setting-container').html(data);
                return false;
            },
            error: function (jqXhr, textStatus, errorThrown) {
                $('#advanced-setting-container').html('<span>' + textStatus + ", " + errorThrown + '</span>');
            }
        });
        return false;
    }

    function createSetting(url, form) {
        var formData = $("#" + form).serialize();

        $.ajax({
            type: "POST",
            url: url,
            data: formData,
            dataType: "json",
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    showMessageWithTitle(jqXhr.status, response.Data, "success", 20000);
                    window.location.href = window.ApplicationHomePage;
                } else {
                    if (response.Errors !== null) {
                        var result = '';
                        $.each(response.Errors, function (i, obj) {
                            result += obj.ErrorMessage + '<br/>';
                        });
                        showMessageWithTitle(500, result, "error", 50000);
                    }
                }
            }, error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    function editSetting(url, form) {
        var formData = $("#" + form).serialize();

        $.ajax({
            type: "PUT",
            url: url,
            data: formData,
            dataType: "json",
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    showMessageWithTitle(jqXhr.status, response.Data.Message, "success", 20000);
                    window.location.href = window.ApplicationHomePage;
                } else {
                    if (response.Errors !== null) {
                        var result = '';
                        $.each(response.Errors, function (i, obj) {
                            result += obj.ErrorMessage + '<br/>';
                        });
                        showMessageWithTitle(500, result, "error", 50000);
                    }
                }
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    function deleteSetting(url, id) {
        var params = { id: id };
        $.ajax({
            type: "DELETE",
            url: url,
            data: params,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    searchSetting();
                    showMessageWithTitle(jqXhr.status, response.Data, "success", 20000);
                } else {
                    if (response.Errors !== null) {
                        var result = '';
                        $.each(response.Errors, function (i, obj) {
                            result += obj.ErrorMessage + '<br/>';
                        });
                        showMessageWithTitle(500, result, "error", 50000);
                    }
                }
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
        return false;
    }
 
    function updateSettingStatus(url, id, status) {
        var params = { "id": id, "status": status };

        $.ajax({
            type: "PUT",
            url: url,
            data: params,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    searchSetting();
                    showMessageWithTitle(jqXhr.status, response.Data, "success", 20000);
                } else {
                    if (response.Errors !== null) {
                        var result = '';
                        $.each(response.Errors, function (i, obj) {
                            result += obj.ErrorMessage + '<br/>';
                        });
                        showMessageWithTitle(500, result, "error", 50000);
                    }
                }
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    function updateSettingSecure(url, id, isSecured) {
        var params = { "id": id, "isSecured": isSecured };

        $.ajax({
            type: "PUT",
            url: url,
            data: params,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    searchSetting();
                    showMessageWithTitle(jqXhr.status, response.Data, "success", 20000);
                } else {
                    if (response.Errors !== null) {
                        var result = '';
                        $.each(response.Errors, function (i, obj) {
                            result += obj.ErrorMessage + '<br/>';
                        });
                        showMessageWithTitle(500, result, "error", 50000);
                    }
                }
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    //search
    $(document).on("click", ".searchSetting", function () {
        searchSetting();
    });

    $(document).on("click", ".createSetting", function (e) {
        e.preventDefault();

        var form = $(this).data('form');
        var url = $(this).data('url');

        if (!$("#" + form).valid()) { // Not Valid
            return false;
        } else {
            createSetting(url, form);
            return false;
        }
    });
    
    $(document).on("click", ".deleteSetting", function (e) {
        e.preventDefault();

        var id = $(this).data('id');
        var url = $(this).data('url');
        var warning = $(this).data('warning');

        var box = bootbox.confirm({
            className: "my-modal",
            size: 'small',
            title: window.Warning,
            message: warning,
            buttons: {
                confirm: {
                    label: window.Ok,
                    className: 'confirm-button-class'
                },
                cancel: {
                    label: window.Cancel,
                    className: 'cancel-button-class'
                }
            },
            callback: function (result) {
                if (result) {
                    deleteSetting(url, id);
                    box.modal('hide');
                }
            },
            onEscape: function () { return false; }
        });

        box.css({
            'top': '50%',
            'margin-top': function () {
                return -(box.height() / 2);
            }
        });
        return false;
    });

    $(document).on("click", ".editSetting", function (e) {
        e.preventDefault();

        var url = $(this).data('url');
        var form = $(this).data('form');

        if (!$("#" + form).valid()) { // Not Valid
            return false;
        }
        else {
            editSetting(url, form);
            return false;
        }
    });
    
    $(document).on("change", ".changeSettingStatus", function (e) {
        e.preventDefault();

        var id = $(this).data('id');
        var url = $(this).data('url');
        var warning = $(this).data('warning');
        var status = $(this).val();

        var box = bootbox.confirm({
            className: "my-modal",
            size: 'small',
            title: window.Warning,
            message: warning,
            buttons: {
                confirm: {
                    label: window.Ok,
                    className: 'confirm-button-class'
                },
                cancel: {
                    label: window.Cancel,
                    className: 'cancel-button-class'
                }
            },
            callback: function (result) {
                if (result) {
                    updateSettingStatus(url, id, status);
                    box.modal('hide');
                }
            },
            onEscape: function () { return false; }
        });

        box.css({
            'top': '50%',
            'margin-top': function () {
                return -(box.height() / 2);
            }
        });
        return false;
    });

    $(document).on("change", ".changeSettingSecure", function (e) {
        e.preventDefault();

        var id = $(this).data('id');
        var url = $(this).data('url');
        var warning = $(this).data('warning');
        var isSecured = $(this).is(":checked");

        var box = bootbox.confirm({
            className: "my-modal",
            size: 'small',
            title: window.Warning,
            message: warning,
            buttons: {
                confirm: {
                    label: window.Ok,
                    className: 'confirm-button-class'
                },
                cancel: {
                    label: window.Cancel,
                    className: 'cancel-button-class'
                }
            },
            callback: function (result) {
                if (result) {
                    updateSettingSecure(url, id, isSecured);
                    box.modal('hide');
                }
            },
            onEscape: function () { return false; }
        });

        box.css({
            'top': '50%',
            'margin-top': function () {
                return -(box.height() / 2);
            }
        });
        return false;
    });
    
    //function resetControls(form) {
    //    var validateObj = $('#' + form);
    //    validateObj.find('input:text, input:password, input:file, select, textarea').not('.ignored').val('');
    //    validateObj.find('input:radio, input:checkbox').removeAttr('checked').removeAttr('selected');
    //    validateObj.find('input[type="number"]').val(0);
    //    validateObj.find('select').find('option:first').attr('selected', true).siblings().attr('selected', false);
    //    validateObj.find("[data-valmsg-summary=true]").removeClass("validation-summary-errors").addClass("validation-summary-valid").find("ul").empty();
    //    validateObj.find("[data-valmsg-replace]").removeClass("field-validation-error").addClass("field-validation-valid").empty();
    //    handleRadios();
    //    handleCheckBoxes();
    //}

    
    //$(document).on("click", ".reset", function () {
    //    var form = $(this).data('form');
    //    var mode = $(this).data('mode');

    //    resetControls(form);
    //    return false;
    //});
 
})(jQuery);