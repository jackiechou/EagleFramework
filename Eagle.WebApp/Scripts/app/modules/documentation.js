(function ($) {
    invokeDateTimePicker('dd/MM/yyyy');

    function resetControls() {
        var validateObj = $('#frmSearch');
        validateObj.find('input:text, input:password, input:file, select, textarea').not('.ignored').val('');
        validateObj.find('input:radio, input:checkbox').removeAttr('checked').removeAttr('selected');
        validateObj.find('input[type="number"]').val(0);
        validateObj.find('input[type=file]').val('');
    }

    function deleteData(id) {
        var params = { id: id };
        $.ajax({
            type: "DELETE",
            url: window.DeleteDocumentationUrl,
            data: params,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    search();
                    resetControls();
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

    function resetFile() {
        $(document).on("click", ".resetFile", function () {
            $('#File').val('');
        });
    }

    resetFile();

    ////PAGINATION
    function search() {
        var filterRequest = $("#frmSearch").serialize().replace(/[^&]+=&/g, '').replace(/&[^&]+=$/g, '');
        $.ajax({
            type: 'GET',
            url: window.SearchDocumentationUrl,
            data: filterRequest,
            ContentType: 'application/json;utf-8',
            datatype: 'json',
            success: function (data) {
                $("#gridcontainer").html(data);
            },
            error: function (jqXhr, textStatus, errorThrown) {
                $("#gridcontainer").html('<span>' + textStatus + ", " + errorThrown + '</span>');
            }
        });
    }

    $(document).on("click", ".search", function () {
        search();
    });

    function addData() {
        var form = $("#frmDocumentation");
        var params = form.serializeArray();
        var formData = new FormData();
        formData.append("File", $('input[type=file]')[0].files[0]);
        $.each(params, function (i, val) {
            formData.append(val.name, val.value);
        });

        $.ajax({
            type: 'POST',
            url: window.CreateDocumentationUrl,
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    window.location.href = window.IndexDocumentationUrl;
                    //resetControls();
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
            }, error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
        return false;
    }

    function editData() {
        var form = $("#frmDocumentation");
        var params = form.serializeArray();
        var formData = new FormData();
        formData.append("File", $('input[type=file]')[0].files[0]);
        $.each(params, function (i, val) {
            formData.append(val.name, val.value);
        });

        $.ajax({
            type: 'POST',
            url: window.EditDocumentationUrl,
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
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

    //UPDATE STATUS
    function updateStatus(url, id, status) {
        var params = { id: id, status: status };
        $.ajax({
            type: "PUT",
            url: url,
            data: params,
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
    }

    //ADD
    $(document).on("click", ".createDocumentation", function (e) {
        e.preventDefault();

        var form = $('#frmDocumentation');
        form.validate();
        if (!form.valid()) { // Not Valid
            return false;
        }
        else {
            addData();
            return false;
        }
    });

    //EDIT
    $(document).on("click", ".editDocumentation", function (e) {
        e.preventDefault();

        var form = $('#frmDocumentation');
        form.validate();
        if (!form.valid()) { // Not Valid
            return false;
        }
        else {
            editData();
            return false;
        }
    });

    $(document).on("click", ".updateDocumentation", function (e) {
        e.preventDefault();

        var form = $('#frmDocumentation');
        form.validate();
        if (!form.valid()) { // Not Valid
            return false;
        } else {
            var params = form.serializeArray();
            var formData = new FormData();
            formData.append("File", $('input[type=file]')[0].files[0]);
            $.each(params, function (i, val) {
                formData.append(val.name, val.value);
            });

            $.ajax({
                type: 'POST',
                url: window.EditDocumentationUrl,
                data: formData,
                cache: false,
                contentType: false,
                processData: false,
                success: function (response, textStatus, jqXhr) {
                    if (response.Status === 0) {
                        window.location.href = window.IndexDocumentationUrl;
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
    });

    //UPDATE STATUS
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

    //DELETE
    $(document).on("click", ".deleteItem", function (e) {
        e.preventDefault();

        var id = $(this).data('id');
        var box = bootbox.confirm({
            className: "my-modal",
            size: 'small',
            title: window.WarningWarning,
            message: window.DoYouWantToContinue,
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
                    deleteData(id);
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

    //RESET
    $(document).on("click", ".reset", function (e) {
        e.preventDefault();
        resetControls();
    });

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
            'top': '25%',
            'margin-top': function () {
                return -($(this).height() / 2);
            }
        });

        return false;
    }

    $(document).on("click", ".showDocumentationPopUp", function () {
        var url = $(this).data('url');

        $.ajax({
            type: "GET",
            dataType: "html",
            url: url,
            success: function (data, statusCode, xhr) {
                //var title = window.ChooseDocumentation;
                var title = 'Documentation detail';
                showPopUp(title, data);
            }, error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });

        return false;
    });

})(jQuery);