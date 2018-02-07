(function ($) {
    function populatePackageSelectBox() {
        var select = $('#PackageId');
        var url = select.data('url');
        var selectedValue = select.data('id');
        var typeId = $('#TypeId').val();
        var params = { "typeId": typeId, "selectedValue": selectedValue };

        select.empty();
        $.ajax({
            type: "GET",
            url: url,
            data: params,
            success: function(data) {
                if (data.length > 0) {
                    $.each(data, function(index, itemData) {
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
            error: function(jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    $(document).on("change", "#TypeId", function (e) {
        e.preventDefault();
        var selectedValue = $(this).find(":selected").val();
        $(this).find('option').removeAttr('selected').filter('[value=' + selectedValue + ']').attr('selected', true);
        populatePackageSelectBox();
        return false;
    });

    function populateSearchPackageSelectBox() {
        var select = $('#SearchPackageId');
        var url = select.data('url');
        var selectedValue = select.data('id');
        var typeId = $('#SearchTypeId').val();
        var params = { "typeId": typeId, "selectedValue": selectedValue };

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
        populateSearchPackageSelectBox();
        return false;
    });

    //Check and privew photo
    function checkPreviewPhoto() {
        $('#File').checkFile({
            allowedExtensions: ['jpg', 'jpeg', 'png', 'gif', 'bmp'],
            allowedSize: 15 //15MB	
        });
        previewPhoto();
    }

    checkPreviewPhoto();

    function toggle() {
        $('#IsExternalLink').bootstrapToggle();
        $('#IsExternalLink').change(function () {
            var tartget = $(this).data('target');
            $('#' + tartget).toggle();
            if ($('#IsExternalLink').is(':checked') === false) {
                $('#BackgroundLink').removeClass("invalid").removeClass("error").addClass("valid").val('');
            } else {
                checkUrl('BackgroundLink');
            }
        });
    }

    toggle();

    function getDetails(url, id) {
        $.ajax({
            type: "GET",
            url: url,
            data: { "id": id },
            success: function (data) {
                $('#divEdit').html(data);
                toggle();
                handleCheckBoxEvent();
                checkPreviewPhoto();
                checkUrl('BackgroundLink');
                populatePackageSelectBox();

                $(document).on("change", "#TypeId", function (e) {
                    e.preventDefault();
                    var selectedValue = $(this).find(":selected").val();
                    $(this).find('option').removeAttr('selected').filter('[value=' + selectedValue + ']').attr('selected', true);
                    populatePackageSelectBox();
                    return false;
                });
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
        return false;
    }

    function search() {
        var formUrl = window.SkinPackageBackgroundSearchUrl;
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

    function resetControls(formId) {
        var validateObj = $('#' + formId);
        validateObj.find('input:text, input:password, input:file, select, textarea').not('.ignored').val('');
        validateObj.find('input:radio, input:checkbox').removeAttr('checked').removeAttr('selected');
        validateObj.find('input[type="number"]').val(0);
        validateObj.find('select').find('option:first').attr('selected', true).siblings().attr('selected', false);
        validateObj.find("[data-valmsg-summary=true]").removeClass("validation-summary-errors").addClass("validation-summary-valid").find("ul").empty();
        validateObj.find("[data-valmsg-replace]").removeClass("field-validation-error").addClass("field-validation-valid").empty();

        search();
        return false;
    }

    //search
    $(document).on("click", ".search", function () {
        search();
        return false;
    });

    function create(url, formId) {
        var form = $("#" + formId);
        var params = form.serializeArray();
        var formData = new FormData();
        formData.append("File", $('input[type=file]')[0].files[0]);
        $.each(params, function (i, val) {
            formData.append(val.name, val.value);
        });

        $.ajax({
            type: "POST",
            url: url,
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    resetControls(formId);
                    showMessageWithTitle(jqXhr.status, response.Data.Message, "success", 20000);
                } else {
                    if (response.Errors !== null) {
                        var result = '';
                        $.each(response.Errors, function (i, obj) {
                            result += obj.ErrorMessage + '<br/>';
                        });
                        showMessageWithTitle(500, result, "error", 500000);
                    }
                }
            }, error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });

        return false;
    }

    function edit(url, formId) {
        var form = $("#" + formId);
        var params = form.serializeArray();
        var formData = new FormData();
        formData.append("File", $('input[type=file]')[0].files[0]);
        $.each(params, function (i, val) {
            formData.append(val.name, val.value);
        });

        $.ajax({
            type: "PUT",
            url: url,
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
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
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
        return false;
    }

    $(document).on("click", ".create", function (e) {
        e.preventDefault();

        var formId = $(this).data('form');
        var mode = $(this).data('mode');
        var url = $(this).data('url');

        if (!$("#" + formId).valid()) { // Not Valid
            return false;
        } else {
            if ($('input[type=file]').get(0).files.length === 0 && $('input[type=text][name="BackgroundLink"]').val() === '') {
                showMessageWithTitle(500, "Please select photo or input link", "error", 50000);
                return false;
            } else {
                create(url, formId);
            }
        }
        return false;
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
        var formId = $(this).data('form');

        if (!$("#" + formId).valid()) { // Not Valid
            return false;
        }
        else {
            var flag = false;
            if ($('input[type=file]')[0].files[0] !== null) {
                flag = true;
            } else {
                if ($('input[name="File"]').val() !== null && $('input[name="File"]').val() !== '') {
                    flag = true;
                } else {
                    if ($('input[type=file]').get(0).files.length === 0 && $('input[name="BackgroundLink"]').val() === '' && $('input[name="BackgroundFile"]').val() === '') {
                        showMessageWithTitle(500, "Please select photo or input link", "error", 50000);
                        return false;
                    }
                }
            }

            if (flag) {
                edit(url, formId);
            } 
        }
        return false;
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