(function ($) {
    function loadEditors() {
        var $editors = $("textarea.ckeditor");
        $editors.each(function () {
            var editorID = $(this).attr("id");
            
            var instance = CKEDITOR.instances[editorID];
            if (instance) {
                instance.destroy();
                CKFinder.setupCKEditor(instance, '/ckfinder/');
                $(this).val(instance.getData());
                instance.on('change', function(evt) {
                    instance.updateElement();
                });
            }
            CKEDITOR.replace(editorID, { customConfig: 'configCustom.js' });
            
            
        });
        return false;
    }

    loadEditors();

    //SORTABLE ++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    function invokeSortableList() {
        $("#sortable").sortable();
        $("#sortable").disableSelection();
    }

    function loadPageList() {
        //var pageId = $(cbxPage).select2('data').id;
        var searchText = $("input[name='SearchText']").val();
        var pageTypeId = $("input[name='PageType']:checked").val();
        var params = { 'SearchText': searchText, 'PageType': pageTypeId };
        var formUrl = $(".search").data('url');


        var page = $('#page-containter').attr("data-page");
        if (page !== null && page !== '') {
            formUrl += "?page=" + page;
        }

        $.ajax({
            type: "GET",
            url: formUrl,
            data: params,
            success: function (data) {
                $('#search-result').html(data);
                invokeSortableList();
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
        return false;
    }

    $(document).on("click", ".pagination a", function () {
        var url = ($(this).attr('href'));
        var paramName = 'page';
        var result = (RegExp(paramName + '=' + '(.+?)(&|$)').exec(url) || [, null])[1];
        $('#page-containter').attr('data-page', result);
    });

    loadPageList();

    function resetControls(formId) {
        $('#Keywords').tagsinput('removeAll');

        var validateObj = $('#' + formId);
        validateObj.find('input:text, input:password, input:file, select, textarea').not('.ignored').val('');
        validateObj.find('input:radio, input:checkbox').removeAttr('checked').removeAttr('selected');
        validateObj.find('input[type="number"]').val(0);
        validateObj.find('input[type=file]').val('');
        validateObj.find('select').find('option:first').attr('selected', true).siblings().attr('selected', false);
        validateObj.find("[data-valmsg-summary=true]").removeClass("validation-summary-errors").addClass("validation-summary-valid").find("ul").empty();
        validateObj.find("[data-valmsg-replace]").removeClass("field-validation-error").addClass("field-validation-valid").empty();
    }

    function pickIcon() {
        var icon = 'glyphicon-picture';
        if ($("#IconClass").val() !== null && $("#IconClass").val() !== '') {
            icon = $("#IconClass").val();
        }

        $('.iconpicker').iconpicker({
            align: 'center',
            arrowClass: 'btn-primary',
            arrowPrevIconClass: 'glyphicon glyphicon-chevron-left',
            arrowNextIconClass: 'glyphicon glyphicon-chevron-right',
            iconset: 'glyphicon|fontawesome',
            icon: icon,
            cols: 10,
            rows: 5,
            footer: true,
            header: true,
            search: true,
            searchText: 'Search',
            selectedClass: 'btn-success',
            unselectedClass: '',
            labelHeader: '{0} of {1} pages',
            labelFooter: '{0} - {1} of {2} icons',
            placement: 'top'
        }).on('change', function (e) {
            $("#IconClass").val(e.icon);
        });
    }

    pickIcon();

    //LOAD AVAILABLE MODULES vs SELECTED MODULES
    function populateDualListBoxListeners() {
        /** Simple delay function that can wrap around an existing function and provides a callback. */
        var delay = (function () {
            var timer = 0;
            return function (callback, ms) {
                clearTimeout(timer);
                timer = setTimeout(callback, ms);
            };
        })();

        /** Checks whether or not an element is visible. The default jQuery implementation doesn't work. */
        $.fn.isVisible = function () {
            return !($(this).css('visibility') === 'hidden' || $(this).css('display') === 'none');
        };

        // Sorts options in a select / list box.
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

        //$(document).on("click", ".select-from option", function (e) {
        //    $(this).attr("selected", true);
        //});

        //$('.push-all').click(function () {
        //    $('.select-from option').each(function () {
        //        $('.select-to').append($('<option/>', {
        //            value: $(this).val(),
        //            text: $(this).text()
        //        }));
        //        $(this).remove();
        //    });
        //});

        //$('.push-item').click(function () {
        //    $('.select-from option:selected').each(function () {
        //        $('.select-to').append($('<option/>', {
        //            value: $(this).val(),
        //            text: $(this).text()
        //        })).show();
        //        $(this).remove();
        //    });
        //});

        //$('.pull-item').click(function () {
        //    $('.select-to option:selected').each(function () {
        //        $('.select-from').append($('<option/>', {
        //            value: $(this).val(),
        //            text: $(this).text()
        //        }));
        //        $(this).remove();
        //    });
        //});

        //$('.pull-all').click(function () {
        //    $('.select-to option').each(function () {
        //        $('.select-from').append($('<option/>', {
        //            value: $(this).val(),
        //            text: $(this).text()
        //        }));
        //        $(this).remove();
        //    });
        //});
    }

    populateDualListBoxListeners();

    function disableFirstRowInTable() {
        var firstRow = $("#tbl-page-permission").find("tbody tr:first");
        firstRow.find("input, select").attr('disabled', true);
        firstRow.attr('disabled', true).css("background-color", "#EEEEEE");
    }

    function getDetails(id, containerId, url) {
        $.ajax({
            type: "GET",
            url: url,
            data: { "id": id },
            success: function (data) {
                $('#' + containerId).html(data);
                invokeDateTimePicker('dd/MM/yyyy');
                $('#Keywords').tagsinput();
                pickIcon();
                populateDualListBoxListeners();

                //Handle Checkbox
                $('input:checkbox').click(function () {
                    var checkBoxStatus = $(this).is(":checked");
                    $(this).attr("checked", checkBoxStatus);
                    $(this).val(checkBoxStatus);
                });

                //disable first row of table
                disableFirstRowInTable();

                $('#PageUrl').keyup(function () {
                    var input = $(this);
                    if (input.val().substring(0, 4) === 'www.') { input.val('http://www.' + input.val().substring(4)); }
                    var re = /(http|ftp|https):\/\/[\w-]+(\.[\w-]+)+([\w.,@?^=%&:\/~+#-]*[\w@?^=%&\/~+#-])?/;
                    var isUrl = re.test(input.val());
                    if (isUrl) { input.removeClass("invalid").removeClass("error").addClass("valid"); }
                    else { input.removeClass("valid").addClass("invalid error"); }
                });


                //Checks whether CKEDITOR is defined or not
                loadEditors();

                return false;
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
        return false;
    }

    //AUTO-COMPLETE - SELECT2 +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    function populateAutoCompleteSelect2Single(requestUrl, requestJsonArrayParams, selectBoxId, hiddenSelectedId, hiddenSelectedText, placeholder) {
        var selectBox = $('#' + selectBoxId);

        selectBox.select2({
            placeholder: placeholder,
            width: "100%",
            theme: "classic",
            minimumInputLength: 3,
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


        $('#' + selectBoxId).on("select2:select", function (e) {
            var selected = e.params.data;
            if (selected !== undefined && selected !== null && selected !== '') {
                $("[name=" + hiddenSelectedId + "]").val(selected.id);
                $("[name=" + hiddenSelectedText + "]").val(selected.text);
                loadPageList();
            }
        }).on("select2:unselecting", function (e) {
            $('#' + selectBoxId).select2('val', '');
            $("[name=" + hiddenSelectedId + "]").val('');
            $("[name=" + hiddenSelectedText + "]").val('');
            loadPageList();
        });
    }

    function bindInitialValueToSelect2(requestDetailUrl, requestDetailJsonArrayParams, selectBoxId, selectedValue) {
        if (selectedValue !== null && selectedValue !== '') {
            $.ajax({
                type: 'GET',
                url: requestDetailUrl,
                data: requestDetailJsonArrayParams,
                dataType: "json",
                success: function (data) {
                    //console.log(data);
                    var $option = new Option(data.Text, data.Id, true, true);
                    $('#' + selectBoxId).append($option);
                    $('#' + selectBoxId).trigger('change');
                },
                error: function (jqXhr) {
                    console.log(jqXhr.responseText);
                }
            });
        }
    }

    populateAutoCompleteSelect2Single(window.GetAutoCompletePagesUrl, { 'pageTypeId': $("input[name='PageType']:checked").val() }, 'cbxPage', 'PageId', 'SearchText', window.PleaseInputPageName);

    bindInitialValueToSelect2(window.GetAutoCompleteDetailsUrl, { "id": $('#PageId').val() }, 'cbxPage', $('#PageId').val());

    //END AUTOCOMPLETE - SELECT2  +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    //search
    $(document).on("click", ".search", function () {
        loadPageList();
    });

    //Handle Checkbox
    $('input:checkbox').click(function () {
        var checkBoxStatus = $(this).is(":checked");
        $(this).attr("checked", checkBoxStatus);
        $(this).val(checkBoxStatus);
    });

    $.fn.serializeObject = function () {
        var o = {};
        var a = this.serializeArray();
        $.each(a, function () {
            if (o[this.name]) {
                if (!o[this.name].push) {
                    o[this.name] = [o[this.name]];
                }
                o[this.name].push(this.value || '');
            } else {
                o[this.name] = this.value || '';
            }
        });
        return o;
    };

    $.fn.serializeTableToArray = function () {
        var array = [];
        var headers = [];
        $(this).find('th').each(function (index, item) {
            headers[index] = $(item).data("id").replace(/\n|\r|\s+/g, "");
        });
        $(this).find('tbody > tr').has('td').each(function () {
            var arrayItem = {};
            $('td', $(this)).each(function (index, item) {
                arrayItem[headers[index]] = $(item).find('input,input[type="hidden"],input[type="checkbox"], textarea, select').val();
            });
            array.push(arrayItem);
        });
        //console.log(JSON.stringify(array));
        return array;
    };

    function create(url, formId) {
        //var formData = $("#" + formId).serializeObject();
        //var pagePermissions = { 'PagePermissions': $("#tbl-page-permission").serializeTableToArray() };
        //formData['PageRolePermission'] = pagePermissions;
        //console.log(JSON.stringify(formData));

        var formData = $("#" + formId).serialize();
        $.ajax({
            type: 'POST',
            url: url,
            data: formData,
            dataType: "json",
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    loadPageList();
                    resetControls(formId);
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

    function edit(url, formId) {
        //var data = convertFormToJson("myform");
        //var formData = $('#frmPage').serialize();
        //var token = $('input[name="__RequestVerificationToken"]').val();
        //var headers = {};
        //headers['__RequestVerificationToken'] = token;
        // var verifiedData = $.extend(formData, headers);

        //var selectedModules = [];
        //$('select.selected option').each(function () {
        //    selectedModules.push($(this).val());
        //});
        //var formData = $("#" + formId).serializeObject();
        //var pagePermissions = {
        //    'PagePermissions': $("#tbl-page-permission").serializeTableToArray()
        //};
        //formData['PageRolePermission'] = pagePermissions;
        //formData['SelectedModules'] = selectedModules;
        //console.log(JSON.stringify(formData));

        var formData = $("#" + formId).serialize();

        $.ajax({
            type: 'POST',
            url: url,
            data: formData,
            dataType: "json",
            traditional: true,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    var activeItem = $("ul.list-group li.list-group-item a").find(".active");
                    getDetails(activeItem.data('id'), activeItem.data('container'), activeItem.data('url'));
                    loadPageList();
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
        return true;
    }

    $('#PageUrl').keyup(function () {
        var input = $(this);
        if (input.val().substring(0, 4) === 'www.') { input.val('http://www.' + input.val().substring(4)); }
        var re = /(http|ftp|https):\/\/[\w-]+(\.[\w-]+)+([\w.,@?^=%&:\/~+#-]*[\w@?^=%&\/~+#-])?/;
        var isUrl = re.test(input.val());
        if (isUrl) { input.removeClass("invalid").removeClass("error").addClass("valid"); }
        else { input.removeClass("valid").addClass("invalid error"); }
    });

    //Add and update post action
    $(document).on("click", ".create", function (e) {
        e.preventDefault();

        var formId = $(this).data('form');
        var url = $(this).data('url');
        //var mode = $(this).data('mode');

        //$(this).val(window.Processing);
        //$(this).attr("disabled", true);
        //var dataPageHeadText = CKEDITOR.instances.PageHeadText.getData();

        if (!$("#" + formId).valid()) { // Not Valid
            return false;
        } else {
            create(url, formId);
            return true;
        }
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

    //EDIT
    $(document).on("click", ".edit", function (e) {
        e.preventDefault();

        var formId = $(this).data('form');
        var url = $(this).data('url');
        edit(url, formId);
        return false;
    });

    $(document).on("change click", ".changeStatus", function (e) {
        e.preventDefault();

        var id = $(this).data('id');
        var formUrl = $(this).data('url');
        var status = $(this).is(":checked");
        var formData = { "id": id, "status": status };

        $.ajax({
            type: 'POST',
            url: formUrl,
            data: formData,
            success: function (data) {
                var result = JSON.parse(data);
                if (result.flag === 'true') {
                    loadPageList();
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

    $(document).on("change", ".changePermissionStatus", function (e) {
        e.preventDefault();

        var roleId = $(this).data('roleid');
        var pageId = $(this).data('pageid');
        var allowAccess = $(this).is(":checked");
        var formUrl = $(this).data('url');
        var userId = '';

        var formData = { "pageId": pageId, "roleId": roleId, "allowAccess": allowAccess, "userId": userId };
       
        $.ajax({
            type: 'POST',
            url: formUrl,
            data: formData,
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

    //RADIO
    function handleRadioButton(elementId) {
        var radioSelector = $("input:radio[name=" + elementId + "]");
        radioSelector.change(function () {
            var radioStatus = $(this).is(":checked");
            radioSelector.attr("checked", radioStatus);
            $("input[name='" + $(this).attr("name") + "']:radio").not(this).parent().removeClass('selected');
            $("input[name='" + $(this).attr("name") + "']:radio").not(this).removeAttr("checked");

            populateAutoCompleteSelect2Single(
                window.GetAutoCompletePagesUrl,
                { 'pageTypeId': $("input[name='PageType']:checked").val() },
                'cbxPage',
                'PageId',
                'SearchText',
                window.PleaseInput
            );
            loadPageList();
        });
    }

    handleRadioButton("PageType");

})(jQuery);