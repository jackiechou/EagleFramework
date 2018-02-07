(function ($) {
    function getDetails(id, url) {
        $.ajax({
            type: "GET",
            url: url,
            data: { "id": id },
            success: function (data) {
                $('#divEdit').html(data);
                return false;
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
        return false;
    }
    
    function search() {
        var formUrl = window.SearchRoleGroupUrl;
        var filterRequest = $("#frmSearch").serialize().replace(/[^&]+=&/g, '').replace(/&[^&]+=$/g, '');
        $.ajax({
            type: "GET",
            url: formUrl,
            data: filterRequest,
            ContentType: 'application/json;utf-8',
            datatype: 'json',
            success: function (data) {
                $("#SearchResult").html(data);
                return false;
            },
            error: function (jqXhr, textStatus, errorThrown) {
                $("#SearchResult").html('<span>' + textStatus + ", " + errorThrown + '</span>');
            }
        });
        return false;
    }

    function resetControls(formId, mode) {
        if (mode === 'edit') {
            var id = $('.edit').data('id');
            var url = $('.edit').data('url');
            getDetails(id, url);
        } else {
            var validateObj = $('#' + formId);
            validateObj.find('input:text, input:password, input:file, select, textarea').not('.ignored').val('');
            validateObj.find('input:radio, input:checkbox').removeAttr('checked').removeAttr('selected');
            validateObj.find('input[type="number"]').val(0);
            validateObj.find('input[type=file]').val('');
            validateObj.find('select').find('option:first').attr('selected', true).siblings().attr('selected', false);
            validateObj.find("[data-valmsg-summary=true]").removeClass("validation-summary-errors").addClass("validation-summary-valid").find("ul").empty();
            validateObj.find("[data-valmsg-replace]").removeClass("field-validation-error").addClass("field-validation-valid").empty();
            search();
        }
    }

    function reloadData(data, formId, mode) {
        var titleSuccess = "", titleFailure = "";
        if (mode === 'create') {
            titleSuccess = window.CreateSuccess;
            titleFailure = window.CreateFailure;
        } else if (mode === 'edit') {
            titleSuccess = window.UpdateSuccess;
            titleFailure = window.UpdateFailure;
        } else if (mode === 'delete') {
            titleSuccess = window.DeleteSuccess;
            titleFailure = window.DeleteFailure;
        } else {
            titleSuccess = window.UpdateSuccess;
            titleFailure = window.UpdateFailure;
        }

        if (data.flag === true) {
            search();
            //resetControls(formId, mode);
            showMessageWithTitle(titleSuccess, data.message, "success", 10000);
        } else {
            showMessageWithTitle(titleFailure, data.message, "error", 10000);
        }
    }

    $(document).on("click", ".checkbox", function () {
        var checkBoxStatus = $(this).is(":checked");
        $(this).attr("checked", checkBoxStatus);
        $(this).val(checkBoxStatus);
    });

    $(document).on("change", ".changeStatus", function (e) {
        e.preventDefault();

        var id = $(this).data('id');
        var formUrl = $(this).data('url');
        var status = $(this).is(":checked");
        var formData = { "id": id, "status": status };

        ////Setup Ajax Request Verification Token vs Anti Forgery Token
        //$.ajaxPrefilter(function (options, originalOptions, jqXhr) {
        //    var verificationToken = $("meta[name='__AjaxRequestVerificationToken']").attr('content');
        //    if (verificationToken) {
        //        jqXhr.setRequestHeader("X-Request-Verification-Token", verificationToken);
        //    }
        //});

        $.ajax({
            type: 'POST',
            url: formUrl,
            data: formData,
            success: function (data) {
                var result = JSON.parse(data);
                if (result.flag === 'true') {
                    search();
                    showMessageWithTitle(window.UpdateStatus, result.message, "success", 10000);
                } else {
                    showMessageWithTitle(window.UpdateStatus, result.message, "error", 10000);
                }
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
        return false;
    });

    //GET - EDIT
    $(document).on("click", ".editItem", function (e) {
        e.preventDefault();

        var id = $(this).data('id');
        var url = $(this).data('url');
        getDetails(id, url);
        return false;
    });

    //EDIT
    $(document).on("click", ".edit", function (e) {
        e.preventDefault();
        var mode = $(this).data('mode');
        var formId = $(this).data('form');
        var formUrl = $(this).data('url');

        if (!$('#' + formId).valid()) { // Not Valid
            return false;
        } else {
            $(this).attr("disabled", true);
            $(this).val('Processing...');

            var formData = $('#frmRoleGroup').serialize();
            var token = $('input[name="__RequestVerificationToken"]').val();
            var headers = {};
            headers['__RequestVerificationToken'] = token;
            // var verifiedData = $.extend(formData, headers);

            $.ajax({
                type: 'POST',
                url: formUrl,
                data: formData,
                headers: headers,
                dataType: "json",
                success: function (data) {
                    $(".edit").val('Save');
                    $(".edit").prop("disabled", false);
                    reloadData(data, formId, mode);
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    handleAjaxErrors(jqXhr, textStatus, errorThrown);
                    $(".edit").val('Save');
                    $(".edit").prop("disabled", false);
                }
            });
            return true;
        }
    });


    // Reset form
    $(document).on("click", ".reset", function () {
        var formId = $(this).data('form');
        var mode = $(this).data('mode');
        resetControls(formId, mode);
    });

    //search
    $(document).on("click", ".search", function () {
        //var formId = $(this).data('form');
        //var formUrl = $(this).data('url');
        //var container = $(this).data('container');
        search();
    });

})(jQuery);

//CREATE - AJAX FORM
function onBegin(xhr) {
    $.validator.unobtrusive.parse("frmRoleGroup");

    if (!$('#frmRoleGroup').valid()) { // Not Valid
        alert("duoc chu ha?");
        return false;
    } else {
        $('#loadingDisplay').show();
        $('#loadProgressBar').css('width', '50%').attr("aria-valuenow", 50);
        $('#formMessage').hide();
        return true;
    }
}

function onSuccess(data, status, xhr) {
    if (data.flag === true) {
        showMessageWithTitle(data.status, data.message, "success", 10000);
        $(".search").trigger('click');
    } else {
        showMessageWithTitle(data.status, data.message, "error", 10000);
    }
}

function onComplete(status, xhr) {
    $('#loadProgressBar').css('width', '100%').attr("aria-valuenow", 100);
    $("#loadingDisplay").delay(500).fadeOut(20).queue(function (next) {
        $('#loadProgressBar').delay(1200).css('width', '0%').attr("aria-valuenow", 0);
        next();
    });

    $(".formMessage").addClass("alert-success");
    $(".formMessage").html(status + ":" + xhr.responseText);
    $('.formMessage').show();
}

function onFailure(xhr, status, error) {
    $(".formMessage").addClass("alert-danger");
    $('.formMessage').html(status + ": [ " + xhr.ErrorCode + "] " + xhr.responseText);
    $('.formMessage').show();
}