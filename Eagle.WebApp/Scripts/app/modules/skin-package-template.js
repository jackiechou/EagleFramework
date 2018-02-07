(function ($) {
    function populatePackageSelectBox(status) {
        var select = $('#PackageId');
        var url = select.data('url');
        var selectedValue = select.data('id');
        var typeId = $('#TypeId').val();
        var params = { "typeId": typeId, "status": status, "selectedValue": selectedValue };

        select.empty();
        $.ajax({
            type: "GET",
            url: url,
            data: params,
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
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    $(document).on("change", "#TypeId", function (e) {
        e.preventDefault();
        var selectedValue = $(this).find(":selected").val();
        $(this).find('option').removeAttr('selected').filter('[value=' + selectedValue + ']').attr('selected', true);
        populatePackageSelectBox(true);
        return false;
    });

    function populateSearchPackageSelectBox(status) {
        var select = $('#SearchPackageId');
        var url = select.data('url');
        var selectedValue = select.data('id');
        var typeId = $('#SearchTypeId').val();
        var params = { "typeId": typeId, "status": status, "selectedValue": selectedValue };

        select.empty();
        $.ajax({
            type: "GET",
            url: url,
            data: params,
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
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    $(document).on("change", "#SearchTypeId", function (e) {
        e.preventDefault();
        var selectedValue = $(this).find(":selected").val();
        $(this).find('option').removeAttr('selected').filter('[value=' + selectedValue + ']').attr('selected', true);
        populateSearchPackageSelectBox(true);
        return false;
    });

    function getDetails(url, id) {
        $.ajax({
            type: "GET",
            url: url,
            data: { "id": id },
            success: function (data) {
                $('#divEdit').html(data);
                handleCheckBoxEvent();
                populatePackageSelectBox(null);
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
        return false;
    }



    function search() {
        var formUrl = window.SkinPackageTemplateSearchUrl;
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

    //search
    $(document).on("click", ".search", function () {
        search();
        return false;
    });

    function create(url, form, mode) {
        var formData = $("#" + form).serialize();

        $.ajax({
            type: "POST",
            url: url,
            data: formData,
            dataType: "json",
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    resetControls(form, mode);
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
            }, error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });

        return false;
    }

    function edit(url, form) {
        var formData = $("#" + form).serialize();

        $.ajax({
            type: "PUT",
            url: url,
            data: formData,
            dataType: "json",
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    getDetails(url, response.Data.Id);
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
                }
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
        return false;
    }

    function changeStatus(url, id, status) {
        var params = { "id": id, "status": status };

        $.ajax({
            type: "PUT",
            url: url,
            data: params,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    search();
                    $(".reset").click();
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
                }
                return false;
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
        return false;
    }

    $(document).on("click", ".create", function (e) {
        e.preventDefault();

        var form = $(this).data('form');
        var mode = $(this).data('mode');
        var url = $(this).data('url');

        if (!$("#" + form).valid()) { // Not Valid
            return false;
        } else {
            create(url, form, mode);
            return false;
        }
    });

    $(document).on("click", ".changeStatus", function (e) {
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
                    changeStatus(url, id, status);
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
            resetControls(form, mode);
        }
        return false;
    });

})(jQuery);