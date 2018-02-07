(function ($) {

    //function searchEmployee() {
    //    var formUrl = window.EmployeeSearchUrl;
    //    var filterRequest = $("#frmSearchEmployee").serialize().replace(/[^&]+=&/g, '').replace(/&[^&]+=$/g, '');
    //    $.ajax({
    //        type: "GET",
    //        url: formUrl,
    //        data: filterRequest,
    //        ContentType: 'application/json;utf-8',
    //        datatype: 'json',
    //        success: function (data) {
    //            $('#search-result').html(data);
    //            return false;
    //        },
    //        error: function (jqXhr, textStatus, errorThrown) {
    //            $('#search-result').html('<span>' + textStatus + ", " + errorThrown + '</span>');
    //        }
    //    });
    //    return false;
    //}

    //function getEmployeeDetail(url, id) {
    //    $.ajax({
    //        type: "GET",
    //        url: url,
    //        data: { "id": id },
    //        success: function (data) {
    //            $('#EmployeeId').val(data.EmployeeId);
    //            $('#EmployeeName').val(data.Contact.FirstName + ' ' + data.Contact.LastName);
    //            $("form").valid();
    //            bootbox.hideAll();
    //        },
    //        error: function (jqXhr, textStatus, errorThrown) {
    //            handleAjaxErrors(jqXhr, textStatus, errorThrown);
    //        }
    //    });

    //    return false;
    //}

    //function showPopUp(title, content) {
    //    var dialog = bootbox.dialog({
    //        className: "my-modal",
    //        title: title,
    //        message: content,
    //        backdrop: true,
    //        closeButton: false,
    //        onEscape: function () {
    //            dialog.modal('hide');
    //            return false;
    //        }
    //    }).find("div.modal-dialog").css({ "width": "80%" });

    //    dialog.css({
    //        'top': '25%',
    //        'margin-top': function () {
    //            return -($(this).height() / 2);
    //        }
    //    });

    //    return false;
    //}

    //$(document).on("click", ".showEmployeePopUp", function () {
    //    var url = $(this).data('url');

    //    $.ajax({
    //        type: "GET",
    //        dataType: "html",
    //        url: url,
    //        success: function (data, statusCode, xhr) {
    //            var title = window.ChooseEmployee;
    //            showPopUp(title, data);
    //        }, error: function (jqXhr, textStatus, errorThrown) {
    //            handleAjaxErrors(jqXhr, textStatus, errorThrown);
    //        }
    //    });

    //    return false;
    //});

    ////search employee
    //$(document).on("click", ".searchEmployee", function () {
    //    searchEmployee();
    //});

    //$(document).on("click", ".getEmployeeDetail", function (e) {
    //    e.preventDefault();

    //    var id = $(this).data('id');
    //    var url = $(this).data('url');

    //    getEmployeeDetail(url, id);

    //    return false;
    //});

    function poplulateCustomerSelect2() {
        var selectBox = $('#cbxCustomer');
        var requestUrl = selectBox.data('url');
        var hiddenSelectedId = 'CustomerId';
        var hiddenSelectedText = 'CustomerName';
        var requestJsonArrayParams = {}
        
        selectBox.select2({
            width: "100%",
            theme: "classic",
            minimumInputLength: 0,
            allowClear: true,
            closeOnSelect: true,
            multiple: false,
            ajax: {
                url: requestUrl,
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    var query = {
                        'search': params.term || '',
                        'page': params.page || 1
                    }
                    return $.extend({}, query, requestJsonArrayParams);
                },
                processResults: function (data, params) {
                    params.page = params.page || 1;
                    return {
                        results: data.Results,
                        pagination: {
                            more: data.MorePage
                        }
                    };
                },
                cache: true
            },
            escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
            templateResult: function (item) { return item.text; },
            templateSelection: function (item) { return item.text; },
            matcher: function (term, text) {
                return text.toUpperCase().indexOf(term.toUpperCase()) !== -1;
            }
        });


        selectBox.on("select2:select", function (e) {
            var selected = e.params.data;
            if (selected !== undefined && selected !== null && selected !== '') {
                $("[name=" + hiddenSelectedId + "]").val(selected.id);
                $("[name=" + hiddenSelectedText + "]").val(selected.text);
            }
        }).on("select2:unselecting", function (e) {
            $(this).select2('val', '');
            $(this).data('state', 'unselected');
            $("[name=" + hiddenSelectedId + "]").val('');
            $("[name=" + hiddenSelectedText + "]").val('');
        });

        //bindInitialValueToSelect2
        var selectedValue = selectBox.data('id');
        var selectedText = selectBox.data('text');

        if (selectedValue !== null && selectedValue !== '' && selectedValue !== undefined
            && selectedText !== null && selectedText !== '' && selectedText !== undefined) {
            var $option = new Option(selectedText, selectedValue, true, true);
            selectBox.append($option);
            selectBox.trigger('change');
        }
    }

    poplulateCustomerSelect2();
  
    function poplulateEmployeeSelectList() {
        var select = $('#cbxEmployee');
        var url = select.data('url');
        var selectedValue = select.data('id');
        var hiddenSelectedId = 'EmployeeId';
        var hiddenSelectedText = 'EmployeeName';
        var servicePackId = $('#ServicePackId').val();
        var params = { "servicePackId": servicePackId, "selectedValue": selectedValue};

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
                }

                if (selectedValue !== null && selectedValue !== '' && selectedValue !== undefined) {
                    select.find('option').removeAttr('selected').filter('[value=' + selectedValue + ']').attr('selected', true);
                }

                select.select2({ width: '100%' });
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });

        select.on("select2:select", function (e) {
            var selected = e.params.data;
            if (selected !== undefined && selected !== null && selected !== '') {
                $("[name=" + hiddenSelectedId + "]").val(selected.id);
                $("[name=" + hiddenSelectedText + "]").val(selected.text);
            }
        }).on("select2:unselecting", function (e) {
            select.select2('val', '');
            $("[name=" + hiddenSelectedId + "]").val('');
            $("[name=" + hiddenSelectedText + "]").val('');
        });
    }
    

    $(document).on("change", "#ServicePackId", function () {
        poplulateEmployeeSelectList();
    });


    var numberOfMonths = 1;
    var startDate = new Date();
    startDate.setMonth(startDate.getMonth() - numberOfMonths);
    startDate.setDate(1);
    var endDate = new Date(startDate.getFullYear() + 1, startDate.getMonth(), startDate.getDate());

    $('#DateRangePicker').daterangepicker({
        startDate: startDate, // From 
        endDate: endDate, // To 
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
        //daysOfWeek: ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'],
        //monthNames: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
        //firstDay: 1,
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


    function getDetails(url, id) {
        $.ajax({
            type: "GET",
            url: url,
            data: { "id": id },
            success: function (data) {
                $('#divEdit').html(data);
                invokeDateTimePicker('dd/MM/yyyy');
                setupNumber();
                handleRadios();
                poplulateCustomerSelect2();
                poplulateEmployeeSelectList();
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    function search() {
        var formUrl = window.ServiceBookingSearchUrl;
        var filterRequest = $("#frmSearch").serialize().replace(/[^&]+=&/g, '').replace(/&[^&]+=$/g, '');
        $.ajax({
            type: "GET",
            url: formUrl,
            data: filterRequest,
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

    function resetControls(form, mode) {
        var validateObj = $('#' + form);
        validateObj.find('input:text, input:password, input:file, select, textarea').not('.ignored').val('');
        validateObj.find('input:radio, input:checkbox').removeAttr('checked').removeAttr('selected');
        validateObj.find('input[type="number"]').val(0);
        validateObj.find('select').find('option:first').attr('selected', true).siblings().attr('selected', false);
        validateObj.find("[data-valmsg-summary=true]").removeClass("validation-summary-errors").addClass("validation-summary-valid").find("ul").empty();
        validateObj.find("[data-valmsg-replace]").removeClass("field-validation-error").addClass("field-validation-valid").empty();

        if (mode === 'search') {
            search();
        }
    }

    function create(url, form, mode) {
        var formData = $("#" + form).awesomeFormSerializer({
            Deposit: $("#Deposit").val().replace(/,/g, "")
        });

        $.ajax({
            type: "POST",
            url: url,
            data: formData,
            dataType: "json",
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    search();
                    resetControls(form, mode);
                    showMessageWithTitle(jqXhr.status, response.Data.Message, "success", 20000);
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

        return false;
    }

    function edit(url, form) {
        var formData = $("#" + form).awesomeFormSerializer({
            Deposit: $("#Deposit").val().replace(/,/g, "")
        });

        $.ajax({
            type: "PUT",
            url: url,
            data: formData,
            dataType: "json",
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    search();
                    showMessageWithTitle(jqXhr.status, response.Data.Message, "success", 20000);
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

    function updateStatus(url, id, status) {
        var params = { id: id, status: status };
        $.ajax({
            type: "PUT",
            url: url,
            data: params,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    search();
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

    //search
    $(document).on("click", ".search", function () {
        search();
    });

    $(document).on("click", ".create", function (e) {
        e.preventDefault();

        var form = $(this).data('form');
        var url = $(this).data('url');
        var mode = $(this).data('mode');

        if (!$("#" + form).valid()) { // Not Valid
            return false;
        } else {
            create(url, form, mode);
            return false;
        }
    });

    $(document).on("change", ".changeStatus", function (e) {
        e.preventDefault();

        var id = $(this).data('id');
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
    

    $(document).on("click", ".editItem", function (e) {
        e.preventDefault();

        var id = $(this).data('id');
        var url = $(this).data('url');

        getDetails(url, id);

        //Go to top
        $('html, body').animate({ scrollTop: 0 }, 'fast');
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
            resetControls(form, mode);
        }
        return false;
    });

})(jQuery);