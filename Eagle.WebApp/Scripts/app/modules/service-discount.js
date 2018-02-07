(function ($) {
    var startDate = new Date();
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

    function setDateRange(startDate, endDate) {
        $('input[name="DateRange"]').daterangepicker({
            locale: {
                cancelLabel: 'Clear',
                format: 'DD/MM/YYYY'
            },
            format: 'DD/MM/YYYY',
            startDate: startDate, // From 
            endDate: endDate, // To 
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
               $('#StartDate').val(start.format('MM/DD/YYYY'));
               $('#EndDate').val(end.format('MM/DD/YYYY'));
           }
       });


        $('input[name="DateRange"]').on('apply.daterangepicker', function (ev, picker) {
            $(this).val(picker.startDate.format('MM/DD/YYYY') + ' - ' + picker.endDate.format('MM/DD/YYYY'));
            $('#StartDate').val(picker.startDate.format('MM/DD/YYYY'));
            $('#EndDate').val(picker.endDate.format('MM/DD/YYYY'));
        });


        $('input[name="DateRange"]').on('cancel.daterangepicker', function (ev, picker) {
            $(this).val('');
            $('#StartDate').val('');
            $('#EndDate').val('');
        });
    }

    setDateRange(startDate, endDate);

    function search() {
        var formUrl = window.ServiceDiscountSearchUrl;
        var filterRequest = $("#frmSearch").serialize().replace(/[^&]+=&/g, '').replace(/&[^&]+=$/g, '');
        $.ajax({
            type: "GET",
            url: formUrl,
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

    function getDetails(url, id) {
        $.ajax({
            type: "GET",
            url: url,
            data: { "id": id },
            success: function (data) {
                $('#divEdit').html(data);
                handleRadios();
                handleCheckBoxes();
                setupNumber();

                var startDate = '';
                var endDate = '';

                if ($('#StartDate').val() !== null && $('#StartDate').val() !== '' && $('#EndDate').val() !== null && $('#EndDate').val() !== '') {
                    var date = $('#StartDate').val();
                    var dateArray = date.split("/");
                    startDate = dateArray[1] + '/' + dateArray[0] + '/' + dateArray[2];

                    var dateEnd = $('#EndDate').val();
                    var dateEndArray = dateEnd.split("/");
                    endDate = dateEndArray[1] + '/' + dateEndArray[0] + '/' + dateEndArray[2];

                    setDateRange(startDate, endDate);
                } else {
                    $('input[name="DateRange"]').daterangepicker({
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
                           $('#StartDate').val(start.format('MM/DD/YYYY'));
                           $('#EndDate').val(end.format('MM/DD/YYYY'));
                       }
                   });

                    $('input[name="DateRange"]').on('apply.daterangepicker', function (ev, picker) {
                        $(this).val(picker.startDate.format('MM/DD/YYYY') + ' - ' + picker.endDate.format('MM/DD/YYYY'));
                        $('#StartDate').val(picker.startDate.format('MM/DD/YYYY'));
                        $('#EndDate').val(picker.endDate.format('MM/DD/YYYY'));
                    });


                    $('input[name="DateRange"]').on('cancel.daterangepicker', function (ev, picker) {
                        $(this).val('');
                        $('#StartDate').val('');
                        $('#EndDate').val('');
                    });
                }
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
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

    function create(url, form) {
        var formData = $("#" + form).awesomeFormSerializer({
            Quantity: $("#Quantity").val().replace(/,/g, ""),
            DiscountRate: $("#DiscountRate").val().replace(/,/g, "")
        });

        $.ajax({
            type: "POST",
            url: url,
            data: formData,
            //dataType: "json",
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    search();
                    resetControls(form);
                    showMessageWithTitle(jqXhr.status, response.Data, "success", 20000);
                    return false;
                } else {
                    if (response.Errors !== null) {
                        var result = '';
                        $.each(response.Errors, function (i, obj) {
                            result += obj.ErrorMessage + '<br/>';
                        });
                        showMessageWithTitle(500, result, "error", 50000);
                    }
                    return false;
                }
            }, error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
        return false;
    }

    function edit(url, form) {
        var formData = $("#" + form).awesomeFormSerializer({
            Quantity: $("#Quantity").val().replace(/,/g, ""),
            DiscountRate: $("#DiscountRate").val().replace(/,/g, "")
        });

        $.ajax({
            type: "PUT",
            url: url,
            data: formData,
            //dataType: "json",
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    search();
                    showMessageWithTitle(jqXhr.status, response.Data.Message, "success", 20000);
                    return false;
                } else {
                    if (response.Errors !== null) {
                        var result = '';
                        $.each(response.Errors, function (i, obj) {
                            result += obj.ErrorMessage + '<br/>';
                        });
                        showMessageWithTitle(500, result, "error", 50000);
                    }
                    return false;
                }
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });

        return false;
    }

    function deleteData(url, id) {
        var params = { id: id };
        $.ajax({
            type: "DELETE",
            url: url,
            data: params,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    search();
                    showMessageWithTitle(jqXhr.status, response.Data, "success", 20000);
                    return false;
                } else {
                    if (response.Errors !== null) {
                        var result = '';
                        $.each(response.Errors, function (i, obj) {
                            result += obj.ErrorMessage + '<br/>';
                        });
                        showMessageWithTitle(500, result, "error", 50000);
                    }
                    return false;
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

        if (!$("#" + form).valid()) { // Not Valid
            return false;
        } else {
            create(url, form);
            return false;
        }
    });

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

    $(document).on("click", ".deleteItem", function (e) {
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
                    deleteData(url, id);
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
            resetControls(form);
        }
        return false;
    });

})(jQuery);