(function ($) {    
    function search() {
        var formUrl = window.SearchRoleUrl;
        var filterRequest = $("#frmSearchRole").serialize().replace(/[^&]+=&/g, '').replace(/&[^&]+=$/g, '');
        $.ajax({
            type: "GET",
            url: formUrl,
            data: filterRequest,
            ContentType: 'application/json;utf-8',
            datatype: 'json',
            success: function (data) {
                $('#dataGrid').html(data);
                return false;
            },
            error: function (jqXhr, textStatus, errorThrown) {
                $("#dataGrid").html('<span>' + textStatus + ", " + errorThrown + '</span>');
            }
        });
        return false;
    }

    search();

    function populateDualListBoxListeners() {
        //Simple delay function that can wrap around an existing function and provides a callback.
        var delay = (function () {
            var timer = 0;
            return function (callback, ms) {
                clearTimeout(timer);
                timer = setTimeout(callback, ms);
            };
        })();

        //Checks whether or not an element is visible. The default jQuery implementation doesn't work.
        $.fn.isVisible = function () {
            return !($(this).css('visibility') === 'hidden' || $(this).css('display') === 'none');
        };

        //Sorts options in a select / list box.
        $.fn.sortOptions = function () {
            return this.each(function () {
                $(this).append($(this).find('option').remove().sort(function (a, b) {
                    var at = $(a).text(), bt = $(b).text();
                    return (at > bt) ? 1 : ((at < bt) ? -1 : 0);
                }));
            });
        };

        $.fn.filterByText = function (textBox) {
            return this.each(function () {
                var select = this;
                var options = [];
                var timeout = 10;

                $(select).find('option').each(function () {
                    options.push({ value: $(this).val(), text: $(this).text() });
                });

                $(select).data('options', options);

                $(textBox).bind('change keyup', function () {
                    delay(function () {
                        var options = $(select).scrollTop(0).data('options');
                        var search = $.trim($(textBox).val());
                        var regex = new RegExp(search, 'gi');

                        $.each(options, function (i) {
                            var option = options[i];
                            if (option.text.match(regex) === null) {
                                $(select).find($('option[value="' + option.value + '"]')).hide();
                            } else {
                                $(select).find($('option[value="' + option.value + '"]')).show();
                            }
                        });
                    }, timeout);
                });
            });
        };

        var unselected = $('select.unselected');
        var selected = $('select.selected');
        var optParentElement = $('.dual-list-box');

        optParentElement.find('button').bind('click', function () {
            switch ($(this).data('type')) {
                case 'str': /* Selected to the right. */
                    unselected.find('option:selected').appendTo(selected);
                    selected.find('option').each(function () {
                        if ($(this).isVisible()) {
                            $(this).attr('selected', true);
                        }
                    });
                    //$(this).prop('disabled', true);
                    break;
                case 'atr': /* All to the right. */
                    unselected.find('option').each(function () {
                        if ($(this).isVisible()) {
                            $(this).remove().appendTo(selected);
                            $(this).attr('selected', true);
                        }
                    });
                    break;
                case 'stl': /* Selected to the left. */
                    selected.find('option:selected').remove().appendTo(unselected);
                    //$(this).prop('disabled', true);
                    selected.find('option').each(function () {
                        if ($(this).isVisible()) {
                            $(this).prop('selected', true);
                        }
                    });
                    break;
                case 'atl': /* All to the left. */
                    selected.find('option').each(function () {
                        if ($(this).isVisible()) {
                            $(this).remove().appendTo(unselected);
                        }
                    });
                    break;
                default:
                    break;
            }

            unselected.filterByText($('input.filter-unselected')).scrollTop(0).sortOptions();
            selected.filterByText($('input.filter-selected')).scrollTop(0).sortOptions();
        });

        optParentElement.closest('form').submit(function () {
            selected.find('option').prop('selected', true);
        });

        optParentElement.find('input[type="text"]').keypress(function (e) {
            if (e.which === 13) {
                event.preventDefault();
            }
        });

        selected.find('option').each(function () {
            if ($(this).isVisible()) {
                $(this).prop('selected', true);
            }
        });
        selected.filterByText($('input.filter-selected')).scrollTop(0).sortOptions();
        unselected.filterByText($('input.filter-unselected')).scrollTop(0).sortOptions();
    }

    populateDualListBoxListeners();

    function populateSelectedGroupsDropDownList() {
        var select = $('select.selected');
        var url = select.data('url');
        var roleId = $('#RoleId').val();

        if (roleId !== null && roleId !== undefined && roleId !== '') {
            var params = { 'roleId': roleId };

            select.empty();
            $.getJSON(url, params,
                function (classesData) {
                    if (classesData.length > 0) {
                        $.each(classesData, function (index, itemData) {
                            select.append($('<option/>', {
                                value: itemData.Value,
                                text: itemData.Text
                            }));
                        });
                    }
                });
        }
    }

    function populateGroupToDropDownList() {
        var select = $('select.unselected');
        var url = select.data('url');

        select.empty();
        $('select.selected').empty();

        $.ajax({
            type: "GET",
            url: url,
            data: null,
            success: function (data) {
                if (data.length > 0) {
                    $.each(data, function (index, itemData) {
                        select.append($('<option/>', {
                            value: itemData.Value,
                            text: itemData.Text
                        }));
                    });
                    populateSelectedGroupsDropDownList();
                    populateDualListBoxListeners();
                } else {
                    select.append($('<option/>', { value: 'Null', text: " ---" + window.None + " --- " }));
                }
                return false;
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
        return false;
    }

    populateGroupToDropDownList();


    //function invokeDateTimePicker(dateFormat) {
    //    var formatDate = dateFormat.toLowerCase();
    //    $('.datepicker').datetimepicker({
    //        format: formatDate,
    //        weekStart: 1,
    //        todayBtn: 1,
    //        autoclose: 1,
    //        todayHighlight: 1,
    //        startView: 2,
    //        minView: 2,
    //        forceParse: 0,
    //        pickTime: false,
    //        minDate: '1/1/1900'
    //    }).on('changeDate', function (e) {
    //        var selectedDate = '';
    //        var result = $(this).val();
    //        var arr = result.split("/");
    //        if (dateFormat === 'dd/MM/yyyy' || dateFormat === 'dd-MM-yyyy') {
    //            selectedDate = arr[1] + '/' + arr[0] + '/' + arr[2];
    //        }
    //        if (dateFormat === 'MM/dd/yyyy' || dateFormat === 'MM-dd-yyyy') {
    //            selectedDate = arr[0] + '/' + arr[1] + '/' + arr[2];
    //        }
    //        if (dateFormat === 'yyyy/MM/dd' || dateFormat === 'yyyy-MM-dd') {
    //            selectedDate = arr[2] + '/' + arr[1] + '/' + arr[0];
    //        }

    //        $(this).siblings('input').val(selectedDate);
    //        $(this).datetimepicker('hide');
    //    });
    //}
    //invokeDateTimePicker('dd/MM/yyyy');

    function getDetails(id, url, containerId) {
        $.ajax({
            type: "GET",
            url: url,
            data: { "id": id },
            success: function (data) {
                $('#' + containerId).html(data);
                populateGroupToDropDownList();
                invokeDateTimePicker('dd/MM/yyyy');
                return false;
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
        return false;
    }

    function resetControls(formId, mode) {
        if (mode === 'edit') {
            var id = $('.edit').data('id');
            var url = $('.edit').data('url');
            var container = $('.edit').data('container');
            getDetails(id, url, container);
        }
        else {
            var formObj = $('#' + formId);
            formObj.find('input:text, input:password, input:file, select, textarea').not('.ignored').val('');
            formObj.find('input:radio, input:checkbox').removeAttr('checked').removeAttr('selected');
            formObj.find('input[type="number"]').val(0);
            formObj.find('input[type=file]').val('');
            formObj.find('select').find('option:first').attr('selected', true).siblings().attr('selected', false);
            formObj.find("[data-valmsg-summary=true]").removeClass("validation-summary-errors").addClass("validation-summary-valid").find("ul").empty();
            formObj.find("[data-valmsg-replace]").removeClass("field-validation-error").addClass("field-validation-valid").empty();
            search();
        }
    }

    function reloadData(data, formId, mode) {
        var titleSuccess = "", titleFailure = "";
        if (mode === 0) {
            titleSuccess = window.CreateSuccess;
            titleFailure = window.CreateFailure;
        } else if (mode === 1) {
            titleSuccess = window.UpdateSuccess;
            titleFailure = window.UpdateFailure;
        } else if (mode === 2) {
            titleSuccess = window.DeleteSuccess;
            titleFailure = window.DeleteFailure;
        } else {
            titleSuccess = window.UpdateSuccess;
            titleFailure = window.UpdateFailure;
        }

        var result = JSON.parse(data);
        if (result.flag === 'true') {
            search();
            resetControls(formId, mode);
            showMessageWithTitle(titleSuccess, result.message, "success", 10000);
        } else {
            showMessageWithTitle(titleFailure, result.message, "error", 10000);
        }
    }


    //ADD
    $(document).on("click", ".create", function (e) {
        e.preventDefault();

        var formId = $(this).data('form');
        var mode = $(this).data('mode');
        var url = $(this).data('url');
        var formData = $("#" + formId).serialize();

        if (!$('#' + formId).valid()) { // Not Valid
            return false;
        } else {
            $(this).val('Processing...');
            $(this).attr("disabled", true);

            $.ajax({
                type: 'POST',
                url: url,
                data: formData,
                dataType: "json",
                success: function (data) {
                    $(".create").val('Save');
                    $(".create").prop("disabled", false);
                    reloadData(data, formId, mode);
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    handleAjaxErrors(jqXhr, textStatus, errorThrown);
                    $(".create").val('Save');
                    $(".create").prop("disabled", false);
                }
            });
            return true;
        }
    });

    //GET - EDIT
    $(document).on("click", ".editItem", function (e) {
        e.preventDefault();

        var id = $(this).data('id');
        var container = $(this).data('container');
        var url = $(this).data('url');
        getDetails(id, url, container);
    });

    //EDIT
    $(document).on("click", ".edit", function (e) {
        e.preventDefault();

        var mode = $(this).data('mode');
        var formId = $(this).data('form');
        var formUrl = $(this).data('url');
        var formData = $("#" + formId).serialize();

        if (!$('#' + formId).valid()) { // Not Valid
            return false;
        } else {
            $(this).val('Processing...');
            $(this).attr("disabled", true);

            $.ajax({
                type: 'POST',
                url: formUrl,
                data: formData,
                cache: false,
                dataType: "json",
                success: function (data) {
                    $(".edit").val('Save');
                   //$(".edit").prop("disabled", false);
                    $(".edit").removeAttr('disabled');

                    reloadData(data, formId, mode);
                    return false;
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    handleAjaxErrors(jqXhr, textStatus, errorThrown);
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
        search();
    });

    $(document).on("click", ".load-back-form", function () {
        var url = $(this).data('url');
        window.location.href = url;
    });

    //removeItem
    $(document).on("click", ".removeItem", function () {
        var groupId = $(this).data('groupid');
        var roleId = $(this).data('roleid');
        var url = $(this).data('url');
        var params = JSON.stringify({ "roleId": roleId, "groupId": groupId });

        $.ajax({
            type: 'POST',
            url: url,
            data: params,
            cache: false,
            dataType: "json",
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

    $(document).on("change", ".changeStatus", function () {
        var id = $(this).data('id');
        var url = $(this).data('url');
        var status = $(this).is(":checked");
        var params = JSON.stringify({ "id": id, "status": status });

        $.ajax({
            type: 'POST',
            url: url,
            data: params,
            cache: false,
            dataType: "json",
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

})(jQuery);