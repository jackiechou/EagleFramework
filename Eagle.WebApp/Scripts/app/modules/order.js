(function ($) {
    function setDateRange() {
        $('input[name="DateRangePicker"]').daterangepicker({
            autoUpdateInput: false,
            locale: {
                cancelLabel: 'Clear',
                format: 'DD/MM/YYYY'
            },
            format: 'DD/MM/YYYY',
            separator: ' - ', // From To 
            applyLabel: 'Apply',
            cancelLabel: 'Cancel',
            fromLabel: 'From',
            toLabel: 'To',
            customRangeLabel: 'Custom',
            "opens": "left",
            "drops": "down",
            "buttonClasses": "btn btn-sm",
            "applyClass": "btn-success",
            "cancelClass": "btn-default"
        },
       function (start, end) {
           if (start) {
               $('#FromDate').val(start.format('MM/DD/YYYY'));
               $('#ToDate').val(end.format('MM/DD/YYYY'));
           }
           return false;
       });


        $('input[name="DateRangePicker"]').on('apply.daterangepicker', function (ev, picker) {
            $(this).val(picker.startDate.format('MM/DD/YYYY') + ' - ' + picker.endDate.format('MM/DD/YYYY'));
            $('#FromDate').val(picker.startDate.format('MM/DD/YYYY'));
            $('#ToDate').val(picker.endDate.format('MM/DD/YYYY'));
        });

        $('input[name="DateRangePicker"]').on('cancel.daterangepicker', function (ev, picker) {
            $(this).val('');
            $('#FromDate').val('');
            $('#ToDate').val('');
        });

        return false;
    }

    setDateRange();

   
    //START DYNAMIC TABLE EXTENSION - OPTION
    // Dynamic Rows Code
    function handleInputNumber() {
        $('input[type=number]').on('wheel', function (e) {
            e.preventDefault();
        });

        $('input[type=number]').keyup(function (event) {
            // skip for arrow keys
            if (event.which >= 37 && event.which <= 40) return;

            // format number
            $(this).val(function (index, value) {
                return value
                .replace(/\D/g, "");
            });
        });
    }

    handleInputNumber();


    //function calculateGrossPrice() {
    //    var tax = 0, discount = 0, total = 0;
    //    var netPrice = parseFloat($("#NetPrice").val().replace(/,/g, ''));
    //    if ($("#TaxRateId").val() !== '')
    //        tax = parseFloat($("#TaxRateId").find('option:selected').text().replace('%', '').replace(/,/g, ''));
    //    if ($("#DiscountId").val() !== '')
    //        discount = parseFloat($("#DiscountId").find('option:selected').text().replace('%', '').replace(/,/g, ''));

    //    total = netPrice + (netPrice * tax / 100) - (netPrice * discount / 100);
    //    var totalGrossPrice = total.toFixed();
    //    if (totalGrossPrice !== '0')
    //        $('#GrossPrice').val(addCommas(totalGrossPrice));
    //    else
    //        $('#GrossPrice').val(0);
    //}

    //$('#NetPrice').on("input", function () {
    //    calculateGrossPrice();
    //});

    //$("#DiscountId").change(function () {
    //    calculateGrossPrice();
    //});

    //$("#TaxRateId").change(function () {
    //    calculateGrossPrice();
    //});

    function getDetails(url, id) {
        $.ajax({
            type: "GET",
            url: url,
            data: { "id": id },
            success: function (data) {
                $('#divEdit').html(data);
                //setupEditor('Specification', '');
                //populateCategoryComboTree('CategoryId');
                //checkPreviewPhoto();
                //handleCheckBoxes();
                //invokeDateTimePicker('dd/MM/yyyy');
                //bindRowEvents();

                //$('#NetPrice').on("input", function () {
                //    //var dInput = this.text;
                //    //console.log(dInput);
                //    calculateGrossPrice();
                //});

                //$("#DiscountId").change(function () {
                //    calculateGrossPrice();
                //});

                //$("#TaxRateId").change(function () {
                //    calculateGrossPrice();
                //});
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
        return false;
    }

    function resetControls(form) {
        var validateObj = $('#' + form);
        validateObj.find('input:text, input:password, input:file, select, textarea').not('.ignored').val('');
        validateObj.find('input:radio, input:checkbox').removeAttr('checked').removeAttr('selected');
        validateObj.find('input[type="number"]').val(0);
        validateObj.find('select').find('option:first').attr('selected', true).siblings().attr('selected', false);
        validateObj.find("[data-valmsg-summary=true]").removeClass("validation-summary-errors").addClass("validation-summary-valid").find("ul").empty();
        validateObj.find("[data-valmsg-replace]").removeClass("field-validation-error").addClass("field-validation-valid").empty();
    }

    function search(formId, url) {
        var filterRequest = $("#" + formId).serialize().replace(/[^&]+=&/g, '').replace(/&[^&]+=$/g, '');
        $.ajax({
            type: "GET",
            url: url,
            data: filterRequest,
            ContentType: 'application/json;utf-8',
            datatype: 'json',
            success: function (data) {
                $('#search-result').html(data);
                return false;
            },
            error: function (jqXhr, textStatus, errorThrown) {
                $('#search-result').html('<span>' + textStatus + ", " + errorThrown + '</span>');
            }
        });
        return false;
    }

    //function create(url, formId) {
    //    var form = $("#" + formId);
    //    var formData = new FormData();
    //    formData.append("File", $('input[type=file]')[0].files[0]);
    //    var params = form.serializeArray();
    //    $.each(params, function (i, val) {
    //        if (val.name === 'NetPrice') {
    //            formData.append('NetPrice', $("#NetPrice").val().replace(/,/g, ""));
    //        }
    //        else if (val.name === 'GrossPrice') {
    //            formData.append('GrossPrice', $("#GrossPrice").val().replace(/,/g, ""));
    //        } else {
    //            formData.append(val.name, val.value);
    //        }
    //    });

    //    $.ajax({
    //        type: "POST",
    //        url: url,
    //        data: formData,
    //        cache: false,
    //        contentType: false,
    //        processData: false,
    //        success: function (response, textStatus, jqXhr) {
    //            if (response.Status === 0) {
    //                search();
    //                resetControls(formId);
    //                showMessageWithTitle(jqXhr.status, response.Data, "success", 20000);
    //            } else {
    //                if (response.Errors !== null) {
    //                    var result = '';
    //                    $.each(response.Errors, function (i, obj) {
    //                        result += obj.ErrorMessage + '<br/>';
    //                    });
    //                    showMessageWithTitle(500, result, "error", 50000);
    //                }
    //            }
    //        }, error: function (jqXhr, textStatus, errorThrown) {
    //            handleAjaxErrors(jqXhr, textStatus, errorThrown);
    //        }
    //    });
    //}

    //function edit(url, formId) {
    //    //$('.product-attribute-container').children(':input').each(function () {
    //    //    if ($(this).val() === '' || $(this).val() === null) {
    //    //        $(this).closest("div.dynamic-row").remove();
    //    //    } 
    //    //});

    //    var form = $("#" + formId);
    //    var formData = new FormData();
    //    formData.append("File", $('input[type=file]')[0].files[0]);
    //    var params = form.serializeArray();
    //    $.each(params, function (i, val) {
    //        if (val.name === 'NetPrice') {
    //            formData.append('NetPrice', $("#NetPrice").val().replace(/,/g, ""));
    //        }
    //        else if (val.name === 'GrossPrice') {
    //            formData.append('GrossPrice', $("#GrossPrice").val().replace(/,/g, ""));
    //        }
    //        else {
    //            formData.append(val.name, val.value);
    //        }
    //    });


    //    $.ajax({
    //        type: "PUT",
    //        url: url,
    //        data: formData,
    //        cache: false,
    //        contentType: false,
    //        processData: false,
    //        success: function (response, textStatus, jqXhr) {
    //            if (response.Status === 0) {
    //                search();
    //                getDetails(url, response.Data.Id);
    //                showMessageWithTitle(jqXhr.status, response.Data.Message, "success", 20000);
    //            } else {
    //                if (response.Errors !== null) {
    //                    var result = '';
    //                    $.each(response.Errors, function (i, obj) {
    //                        result += obj.ErrorMessage + '<br/>';
    //                    });
    //                    showMessageWithTitle(500, result, "error", 50000);
    //                }
    //            }
    //        },
    //        error: function (jqXhr, textStatus, errorThrown) {
    //            handleAjaxErrors(jqXhr, textStatus, errorThrown);
    //        }
    //    });
    //}

    function updateStatus(url, orderNo, status) {
        var params = { "orderNo": orderNo, "status": status };
       
        $.ajax({
            type: "PUT",
            url: url,
            data: params,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
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
    $(document).on("click", ".search", function () {
        var formId = $(this).data('form');
        var url = $(this).data('url');

        search(formId, url);
        return false;
    });

    $(document).on("click", ".create", function (e) {
        e.preventDefault();

        var form = $(this).data('form');
        var url = $(this).data('url');

        if (!$("#" + form).valid()) { // Not Valid
            return false;
        } else {
            create(url, form);
            return false;
        }
    });

    $(document).on("change", ".changeStatus", function (e) {
        e.preventDefault();

        var orderNo = $(this).data('orderno');
        var url = $(this).data('url');
        var warning = $(this).data('warning');
        var status = $(this).find('input[type="radio"]:checked').val();

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
                    updateStatus(url, orderNo, status);
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

    $(document).on("click", ".editItem", function (e) {
        e.preventDefault();

        var id = $(this).data('id');
        var url = $(this).data('url');

        getDetails(url, id);
        return false;
    });

    $(document).on("click", ".edit", function (e) {
        e.preventDefault();

        var url = $(this).data('url');
        var form = $(this).data('form');

        if (!$("#" + form).valid()) { // Not Valid
            return false;
        }
        else {
            edit(url, form);
            return false;
        }
    });

    $(document).on("click", ".reset", function () {
        var form = $(this).data('form');
        var mode = $(this).data('mode');

        if (mode === 'edit') {
            var id = $(this).data('id');
            var url = $(this).data('url');
            getDetails(url, id);
        } else {
            resetControls(form);
        }
        return false;
    });
})(jQuery);
