﻿(function ($) {
    function resetControls(form) {
        var validateObj = $('#' + form);
        validateObj.find('input:text, input:password, input:file, select, textarea').not('.ignored').val('');
        validateObj.find('input:radio, input:checkbox').removeAttr('checked').removeAttr('selected');
        validateObj.find('input[type="number"]').val(0);
        validateObj.find('select').find('option:first').attr('selected', true).siblings().attr('selected', false);
        validateObj.find("[data-valmsg-summary=true]").removeClass("validation-summary-errors").addClass("validation-summary-valid").find("ul").empty();
        validateObj.find("[data-valmsg-replace]").removeClass("field-validation-error").addClass("field-validation-valid").empty();
    }

    function search() {
        var formUrl = window.ServicePackSearchUrl;
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

    //search
    $(document).on("click", ".search", function () {
        search();
    });

    $(document).on("click", ".reset", function () {
        var form = $(this).data('form');
        resetControls(form);
        return false;
    });

    $('#ServiceType').on('change', function () {
        populateServiceCategoryComboTree();
        search();
    });

    //Service Category ComboTree
    function populateServiceCategoryComboTree() {
        var select = $("#CategoryId");
        var url = select.data('url');
        var params = {
            "typeId": $('#ServiceType').val(),
            "selectedId": "0",
            "isRootShowed": true
        };

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
                select.combotree({
                    data: data,
                    valueField: 'id',
                    textField: 'text',
                    required: false,
                    editable: false,
                    method: 'get',
                    panelHeight: 'auto',
                    checkbox: true,
                    children: 'children',
                    onLoadSuccess: function (row, data) {
                        $(this).tree("expandAll");
                        //select.combotree('setValues', [0]);
                    },
                    onClick: function (node) {
                        var selectedValue = node.id;
                        $(this).val(selectedValue);
                        search();
                    }
                });
            }
        });
    }

    populateServiceCategoryComboTree();
    
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

})(jQuery);