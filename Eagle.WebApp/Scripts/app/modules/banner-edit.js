(function ($) {
    function resetControls() {
        var validateObj = $('#frmBanner');
        validateObj.find('input:text, input:password, input:file, select, textarea').not('.ignored').val('');
        validateObj.find('input:radio, input:checkbox').removeAttr('checked').removeAttr('selected');
        validateObj.find('input[type="number"]').val(0);
        validateObj.find('select').find('option:first').attr('selected', true).siblings().attr('selected', false);
        validateObj.find("[data-valmsg-summary=true]").removeClass("validation-summary-errors").addClass("validation-summary-valid").find("ul").empty();
        validateObj.find("[data-valmsg-replace]").removeClass("field-validation-error").addClass("field-validation-valid").empty();
    }
    
    function reloadGrid() {
        $.ajax({
            type: "GET",
            url: window.BannerListUrl,
            success: function (data) {
                $('#divList').html(data);
            }
        });
    }

    function createData() {
        var url = window.CreateBannerUrl;
        var form = $("#frmBanner");
        var params = form.serializeArray();
        var formData = new FormData();
        formData.append("File", $('input[type=file]')[0].files[0]);

        $('#SelectedPages :selected').each(function (i, val) {
            formData.append('SelectedPages[' + i + ']', val.value);
        });

        $.each(params, function (i, val) {
            if (val.name !== 'SelectedPages') {
                formData.append(val.name, val.value);
            }
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
                    //resetControls();
                    showMessageWithTitle(jqXhr.status, response.Data, "success", 20000);
                    window.location.href = '/admin/banner/index';
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

    function editData() {
        var url = window.EditBannerUrl;
        var form = $("#frmBanner");
        var params = form.serializeArray();
        var formData = new FormData();
        formData.append("File", $('input[type=file]')[0].files[0]);

        $('#SelectedPages :selected').each(function (i, val) {
            formData.append('SelectedPages[' + i + ']', val.value);
        });

        $.each(params, function (i, val) {
            if (val.name !== 'SelectedPages') {
                formData.append(val.name, val.value);
            }
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
                    //getBannerDetails(url, response.Data.Id);
                    showMessageWithTitle(jqXhr.status, response.Data.Message, "success", 20000);
                    window.location.href = '/admin/banner/index';
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

    function deleteData(url, id) {
        var params = { id: id };
        $.ajax({
            type: 'DELETE',
            url: url,
            data: params,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    reloadGrid();
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

    function updateStatus(url, id, status) {
        var params = { id: id, status: status };
        $.ajax({
            type: "PUT",
            url: url,
            data: params,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    reloadGrid();
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
    }

    function createStepWizard() {
        // Step Wizard
        var form = $("#frmBanner");
        form.validate();

        $("#wizard").steps({
            headerTag: "h3",
            bodyTag: "section",
            transitionEffect: "fade",
            autoFocus: true,
            saveState: true,
            labels: {
                current: "current step:",
                pagination: "Pagination",
                finish: window.Finish,
                next: window.Next,
                previous: window.Previous,
                loading: window.Loading
            },
            onStepChanging: function (event, currentIndex, newIndex) {
                //var fv = form.data('formValidation'), // FormValidation instance
                // The current step container
                //var container = form.find('section[data-step="' + currentIndex + '"]');

                invokeDateTimePicker('dd/MM/yyyy');

                // Allways allow previous action even if the current form is not valid!
                if (currentIndex > newIndex) {
                    return true;
                }

                form.validate();
                if (form.valid() === false || form.valid() === null) {
                    // Do not jump to the next step
                    return false;
                }

                // Needed in some cases if the user went back (clean up)
                if (currentIndex < newIndex) {
                    // To remove error styles
                    form.find(".body:eq(" + newIndex + ") label.error").remove();
                    form.find(".body:eq(" + newIndex + ") .error").removeClass("error");
                }

                //change color of the Go button
                $('.actions > ul > li:last-child a').css('background-color', '#f89406');

                form.validate().settings.ignore = ":disabled,:hidden";
                return form.valid();
            },
            onStepChanged: function (event, currentIndex, priorIndex) {
                
            },
            onFinishing: function (event, currentIndex) {
                form.validate().settings.ignore = ":disabled";
                return form.valid();
            },
            onFinished: function (event, currentIndex) {
                if (!form.valid()) { // Not Valid
                    return false;
                } else {
                    //alert("Submitted!");
                    var mode = form.data('mode');
                  
                    if (mode === 'create') {
                        createData();
                    }

                    if (mode === 'edit') {
                        editData();
                    }
                    return false;
                }
            }
        });
    }

    createStepWizard();

    var action;
    $(".number-spinner button").mousedown(function () {
        var btn = $(this);
        var input = btn.closest('.number-spinner').find('input');
        btn.closest('.number-spinner').find('button').prop("disabled", false);

        if (btn.attr('data-dir') === 'up') {
            action = setInterval(function () {
                if (input.attr('max') === undefined || parseInt(input.val()) < parseInt(input.attr('max'))) {
                    input.val(parseInt(input.val()) + 1);
                } else {
                    btn.prop("disabled", true);
                    clearInterval(action);
                }
            }, 50);
        } else {
            action = setInterval(function () {
                if (input.attr('min') === undefined || parseInt(input.val()) > parseInt(input.attr('min'))) {
                    input.val(parseInt(input.val()) - 1);
                } else {
                    btn.prop("disabled", true);
                    clearInterval(action);
                }
            }, 50);
        }
    }).mouseup(function () {
        clearInterval(action);
    });

    $('input[type="number"]').restrictNumber();
    //$('#Tags').tagsinput();
    //$('#Tags').tagsInput({ width: 'auto', style: 'bootstrap' });
    //$('.spinner .btn:first-of-type').on('click', function () {
    //    var btn = $(this);
    //    var input = btn.closest('.spinner').find('input');
    //    if (input.attr('max') == undefined || parseInt(input.val()) < parseInt(input.attr('max'))) {
    //        input.val(parseInt(input.val(), 10) + 1);
    //    } else {
    //        btn.next("disabled", true);
    //    }
    //});
    //$('.spinner .btn:last-of-type').on('click', function () {
    //    var btn = $(this);
    //    var input = btn.closest('.spinner').find('input');
    //    if (input.attr('min') == undefined || parseInt(input.val()) > parseInt(input.attr('min'))) {
    //        input.val(parseInt(input.val(), 10) - 1);
    //    } else {
    //        btn.prev("disabled", true);
    //    }
    //});


    $('#SelectedPages').multipleSelect({
        isOpen: true,
        keepOpen: true,
        multiple: true,
        filter: true,
        //height: 'auto',
        //buttonClass: 'form-control',
        multipleWidth: 255,
        width: '100%'
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

    $(document).on("click", ".deleteItem", function () {
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
    
    $(document).on("click", ".reset", function () {
        resetControls();
        return false;
    });

    $(document).on("click change", "input[type=radio][name='ScopeId']", function (e) {
        e.preventDefault();

        var selectedScopeId = $(this).val();
        switch (selectedScopeId) {
            case "2":
                $('#DateTimeScope').removeClass('hide');
                $('#ClickScope').addClass('hide');
                $('#ImpressionScope').addClass('hide');
                break;
            case "3":
                $('#ClickScope').removeClass('hide');
                $('#DateTimeScope').addClass('hide');
                $('#ImpressionScope').addClass('hide');
                break;
            case "4":
                $('#ImpressionScope').removeClass('hide');
                $('#DateTimeScope').addClass('hide');
                $('#ClickScope').addClass('hide');
                break;
            default:
                $('#ImpressionScope').addClass('hide');
                $('#DateTimeScope').addClass('hide');
                $('#ClickScope').addClass('hide');
                break;
        }
    });

    $(document).on("click change", "input[type=radio][name='TypeId']", function (e) {
        e.preventDefault();

        var selectedTypeId = $(this).val();
        switch (selectedTypeId) {
        case "1":
            $('#FileContainer').removeClass('hide');
            $('#BannerContentContainer').addClass('hide');
            break;
        case "2":
            $('#FileContainer').removeClass('hide');
            $('#BannerContentContainer').removeClass('hide');
            break;
        case "3":
            $('#FileContainer').addClass('hide');
            $('#BannerContentContainer').removeClass('hide');
            break;
        default:
            $('#FileContainer').removeClass('hide');
            $('#BannerContentContainer').addClass('hide');
            break;
        }
    });

    previewPhoto();
})(jQuery);