(function ($) {
    function search() {
        //var searchText = $("input[name='SearchText']").val();
        //var moduleType = $('input[type="radio"][name="SearchModuleType"]:checked').val();
        //var params = { 'SearchText': searchText, 'SearchModuleType': moduleType, 'Status': '' };

        var formData = $("#frmSearchModule").serialize();
        var formUrl = $(".search").data('url');
        var page = $('#page-containter').attr("data-page");
        if (page !== null && page !== '') {
            formUrl += "?page=" + page;
        }

        $.ajax({
            type: "GET",
            url: formUrl,
            data: formData,
            beforeSend: function () {
                $.unblockUI();
            },
            success: function (data) {
                $('#search-result').html(data);


                $(document).on('change keyup','#SearchText', function() {
                    delay(function () {
                        var search = $.trim(this.value);
                        var regex = new RegExp(search, 'gi');

                        $('ul#list-module').find('li').each(function () {
                            // cache jquery object
                            if (typeof $(this).data('text') !== 'undefined') {
                                var text = $(this).data('text');
                                if (text.match(regex) === null)
                                    $(this).hide();
                                else
                                    $(this).show();
                            } else {
                                $(this).hide();
                            }
                        });
                    }, 1000);
                });

                return false;
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
                return false;
            }
        });
    }

    search();

    $(document).on('change', 'input[type="radio"][name="SearchModuleType"]', function () {
        search();
    });


    $(document).on('click', '.search', function () {
        search();
    });

    $(document).on("click", ".pagination a", function () {
        var url = ($(this).attr('href'));
        var paramName = 'page';
        //var result = (RegExp(paramName + '=' + '(.+?)(&|$)').exec(url) || [, null])[1];
        var result = RegExp(paramName + '=' + '(.+?)(&|$)').exec(url);
        $('#page-containter').attr('data-page', result);
    });

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

    function populateSelectedPagesDropDownList() {
        var select = $('select.selected');
        var moduleId = $('#ModuleId').val();

        if (moduleId !== null && moduleId !== undefined && moduleId !== '') {
            var url = window.PagesByModuleMultiSelectListUrl;
            var params = { 'moduleId': moduleId, 'pageTypeId': $('#ModuleTypeId').val() };

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
    
    function populatePageToDropDownList() {
        var select = $('select.unselected');
        var url = window.PageMultiSelectListUrl;
        var params = {
            'pageTypeId': $('#ModuleTypeId').val(),
            'moduleId': $('#ModuleId').val()
        };

        select.empty();
        $('select.selected').empty();

        $.ajax({
            type: "GET",
            url: url,
            data: params,
            //global: false,
            beforeSend: function () {
                $.unblockUI();
            },
            success: function (data) {
                if (data.length > 0) {
                    //select.append($('<option/>', { value: 'Null', text: " --- "+window.Select+" --- " }));
                    $.each(data, function (index, itemData) {
                        select.append($('<option/>', {
                            value: itemData.Value,
                            text: itemData.Text
                        }));
                    });
                    //select.find('option:first').attr("selected", "selected");
                    populateSelectedPagesDropDownList();
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

    $(document).on("change", "#ModuleTypeId", function () {
        populatePageToDropDownList();
    });

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    
    //All Pages vs Some Pages
    function handleCheckboxBootstrapToggle() {
        $('.chekbox-toggle').bootstrapToggle();
        $('.chekbox-toggle').change(function () {
            //var result = $(this).prop('checked');
            var checkBoxStatus = $(this).is(":checked");
            if (checkBoxStatus)
                $(".page-container").hide();
            else {
                $(".page-container").show();
            }
        });
    }

    handleCheckboxBootstrapToggle();
    
    function handleCheckBoxEvent() {
        //Handle Checkbox
        $('input:checkbox').click(function () {
            var checkBoxStatus = $(this).is(":checked");
            $(this).attr("checked", checkBoxStatus);
            $(this).val(checkBoxStatus);
        });
    }

    //START DYNAMIC TABLE EXTENSION
    $(".add-row").on("click", function () {
        addRow();
        return false;
    });

    $(".row-remove").on("click", function () {
        $(this).closest("tr").remove();
        return false;
    });

    $(".row-edit").on("click", function () {
        $(this).closest("tr").css("background-color", "transparent");
        $(this).closest("tr").find('input:text, input:radio, input:checkbox, select, textarea').attr('readonly', false);
        return false;
    });

    // Sortable Code
    //var fixHelperModified = function (e, tr) {
    //    var $originals = tr.children();
    //    var $helper = tr.clone();

    //    $helper.children().each(function (index) {
    //        $(this).width($originals.eq(index).width());
    //    });

    //    return $helper;
    //};

    //$(".table-sortable tbody").sortable({
    //    helper: fixHelperModified
    //}).disableSelection();

    //$(".table-sortable thead").disableSelection();
    //END DYNAMIC TABLE
    

    //GET - DETAILS
    function getDetails(id, containerId, url) {
        $.ajax({
            type: "GET",
            url: url,
            data: { "id": id },
            success: function (data) {
                $('#' + containerId).html(data);
                handleCheckboxBootstrapToggle();
                invokeDateTimePicker('dd/MM/yyyy');
                populateDualListBoxListeners();
                //populateModuleCapabilities();
                handleCheckBoxEvent();

                $(".add-row").on("click", function () {
                    addRow();
                });

                return false;
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
        return false;
    }

    function resetControls(formId, mode) {
        var form = $('#' + formId);
        if (mode === 'edit') {
            var id = $('.edit').data('id');
            var container = $('.edit').data('container');
            getDetails(id, container);
        } else if (mode === 'search') {
            form.find('input:text, input:password, input:file, select, textarea').not('.ignored').val('');
            form.find('input:radio, input:checkbox').removeAttr('checked').removeAttr('selected');
            form.find('input[type="number"]').val(0);
            form.find('input[type=file]').val('');
            form.find('select').find('option:first').attr('selected', true).siblings().attr('selected', false);
            form.find("[data-valmsg-summary=true]").removeClass("validation-summary-errors").addClass("validation-summary-valid").find("ul").empty();
            form.find("[data-valmsg-replace]").removeClass("field-validation-error").addClass("field-validation-valid").empty();
            search();
        }
        else {
            form.find('input:text, input:password, input:file, select, textarea').not('.ignored').val('');
            form.find('input:radio, input:checkbox').removeAttr('checked').removeAttr('selected');
            form.find('input[type="number"]').val(0);
            form.find('input[type=file]').val('');
            form.find('select').find('option:first').attr('selected', true).siblings().attr('selected', false);
            form.find("[data-valmsg-summary=true]").removeClass("validation-summary-errors").addClass("validation-summary-valid").find("ul").empty();
            form.find("[data-valmsg-replace]").removeClass("field-validation-error").addClass("field-validation-valid").empty();
            populateDualListBoxListeners();
        }
    }


    $(document).on("click", ".reset", function () {
        var mode = $(this).data('mode');
        var form = $(this).data('form');
        resetControls(form, mode);
    });

    //GET - EDIT
    $(document).on("click", ".editItem", function (e) {
        e.preventDefault();
        $(this).addClass('active').siblings().removeClass('active');

        var id = $(this).data('id');
        var container = $(this).data('container');
        var formUrl = $(this).data('url');

        getDetails(id, container, formUrl);
    });
    
    //GET - EDIT Permission
    $(document).on("click", ".editPermissionItem", function (e) {
        var id = $(this).data('id');
        var containerId = $(this).data('container');
        var url = $(this).data('url');

        $.ajax({
            type: "GET",
            url: url,
            data: { "id": id },
            success: function (data) {
                $('#' + containerId).html(data);
                return false;
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
        return false;
    });

  
    //POST - ADD
    $(document).on("click", ".create", function (e) {
        e.preventDefault();

        var url = $(this).data("url");
        var formId = $(this).data("form");
        var mode = $(this).data("mode");
        var form = $("#" + formId);
        var params = form.serialize();

        if (!form.valid()) { // Not Valid
            return false;
        } else {
            $.ajax({
                type: 'POST',
                url: url,
                data: params,
                dataType: "json",
                success: function (response, textStatus, jqXhr) {
                    if (response.Status === 0) {
                        resetControls(formId, mode);
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
    });

    //POST - EDIT
    $(document).on("click", ".edit", function (e) {
        e.preventDefault();

        var mode = $(this).data("mode");
        var url = $(this).data("url");
        var formId = $(this).data("form");
        var form = $("#" + formId);
        var params = form.serialize();

        if (!form.valid()) { // Not Valid
            return false;
        } else {
            $.ajax({
                type: 'POST',
                url: url,
                data: params,
                dataType: "json",
                success: function (response, textStatus, jqXhr) {
                    if (response.Status === 0) {
                        resetControls(formId, mode);
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
    });
    
    $(document).on("change", ".changeStatus", function (e) {
        e.preventDefault();

        var id = $(this).data('id');
        var formUrl = $(this).data('url');
        var status = $(this).is(":checked");
        var formData = { "id": id, "status": status };

        $.ajax({
            type: 'POST',
            url: formUrl,
            data: formData,
            cache: false,
            dataType: "json",
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
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
    });

    $(document).on("change", ".changePermissionStatus", function (e) {
        e.preventDefault();

        var roleId = $(this).data('roleid');
        var moduleId = $(this).data('moduleid');
        var capabilityId = $(this).data('capabilityid');
        var formUrl = $(this).data('url');
        var status = $(this).is(":checked");
        var formData = JSON.stringify({ "roleId": roleId, "moduleId": moduleId, "capabilityId": capabilityId, "status": status });

        $.ajax({
            type: 'POST',
            url: formUrl,
            data: formData,
            cache: false,
            dataType: "json",
            success: function (data) {
                var result = JSON.parse(data);
                if (result.flag === 'true') {
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