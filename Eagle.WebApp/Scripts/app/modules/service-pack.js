(function ($) {

    function setupEditor(editorId, data) {
        // create Editor from textarea HTML element with default set of tools
        var editor = $("#" + editorId);
        // var value = editor.data("kendoEditor").value();
        editor.kendoEditor({
            tools: [
                "bold",
                "italic",
                "underline",
                "strikethrough",
                "justifyLeft",
                "justifyCenter",
                "justifyRight",
                "justifyFull",
                "insertUnorderedList",
                "insertOrderedList",
                "indent",
                "outdent",
                "createLink",
                "unlink",
                "insertImage",
                "insertFile",
                "subscript",
                "superscript",
                "createTable",
                "addRowAbove",
                "addRowBelow",
                "addColumnLeft",
                "addColumnRight",
                "deleteRow",
                "deleteColumn",
                "viewHtml",
                "formatting",
                "cleanFormatting",
                "fontName",
                "fontSize",
                "foreColor",
                "backColor",
                "print"
            ],
            value: data,
            imageBrowser: {
                path: window.UploadServicePackImageFolder,
                messages: {
                    dropFilesHere: "Drop files here"
                },
                transport: {
                    read: {
                        url: window.ReadImageBrowserUrl,
                        dataType: "json",
                        type: "POST"
                    },
                    destroy: {
                        url: window.DestroyImageBrowserUrl,
                        type: "POST"
                    },
                    create: {
                        url: window.CreateImageBrowserDirectoryUrl,
                        type: "POST"
                    },
                    thumbnailUrl: window.ThumbnailImageBrowserUrl,
                    uploadUrl: window.UploadImageBrowserUrl,
                    imageUrl: '/{0}'
                }
            },
            fileBrowser: {
                path: window.UploadServicePackFileFolder,
                messages: {
                    dropFilesHere: "Drop files here"
                },
                transport: {
                    read: window.ReadFileBrowserUrl,
                    destroy: {
                        url: window.DestroyFileBrowserUrl,
                        type: "POST"
                    },
                    create: {
                        url: window.CreateFileBrowserDirectoryUrl,
                        type: "POST"
                    },
                    uploadUrl: window.UploadFileBrowserUrl,
                    fileUrl: '/{0}'
                }
            }
        });
    }

    setupEditor('Specification', '');

    //Check and privew photo
    function checkPreviewPhoto() {
        $('#File').checkFile({
            allowedExtensions: ['jpg', 'jpeg', 'png', 'gif', 'bmp'],
            allowedSize: 15 //15MB	
        });
        previewPhoto();
    }

    checkPreviewPhoto();

    //Service Category ComboTree
    function populateServiceCategoryComboTree() {
        var select = $("#CategoryId");
        var url = select.data('url');
        var selectedValue = select.val();

        var params = {
            "typeId": $('#TypeId').val(),
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
                        selectedValue = node.id;
                        $(this).val(selectedValue);
                    }
                });
            }
        });


    }

    populateServiceCategoryComboTree();

    $('#TypeId').on('change', function () {
        populateServiceCategoryComboTree();
    });

    function poplulateServicePackDurationSelectList(controlId, status) {
        var select = $('#' + controlId);
        var selectedValue = select.data('id');
        var url = select.data('url');
        var params = { "status": status, "selectedValue": selectedValue, "isShowSelectText": true };

        select.empty();
        $.ajax({
            type: "GET",
            url: url,
            data: params,
            beforeSend: function () {
                $.unblockUI();
            },
            success: function (data) {
                if (data.length > 0) {
                    $.each(data, function (index, itemData) {
                        select.append($('<option/>', {
                            value: itemData.Value,
                            text: itemData.Text
                        }));
                    });
                }

                if (selectedValue !== null && selectedValue !== '' && selectedValue !== undefined) {
                    select.find('option').removeAttr('selected').filter('[value=' + selectedValue + ']').attr('selected', true);
                }

                select.select2({ width: '100%' });
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    poplulateServicePackDurationSelectList('DurationId', true);

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
    }

    populateDualListBoxListeners();


    function calculateGrossPrice() {
        var servicePackFee = $("#PackageFee").val();
        if (servicePackFee !== null && servicePackFee !== '' && servicePackFee !== undefined) {
            var tax = 0, discount = 0, total = 0;
            var netPrice = parseFloat($("#PackageFee").val().replace(/,/g, ''));
            if ($("#DiscountId").val() !== '') {
                discount = parseFloat($("#DiscountId").find('option:selected').text().replace('%', '').replace(/,/g, ''));
            }

            if ($("#TaxRateId").val() !== '') {
                tax = parseFloat($("#TaxRateId").find('option:selected').text().replace('%', '').replace(/,/g, ''));
            }

            total = netPrice + (netPrice * (tax / 100)) - (netPrice * (discount / 100));
            var totalGrossPrice = total.toFixed();
            if (totalGrossPrice !== '0')
                $('#TotalFee').val(addCommas(totalGrossPrice));
            else
                $('#TotalFee').val(0);
        } else {
            $('#PackageFee').val('');
            $('#TotalFee').val('');
        }
    }

    $('#PackageFee').on("input", function () {
        calculateGrossPrice();
    });

    //Handle Discount
    $(document).on("change", "#DiscountId", function () {
        $(this).find("option[value=" + $(this).val() + "]").attr('selected', true).siblings().attr('selected', false);
        //var selectedValue = $(this).val();
        calculateGrossPrice();
    });

    //Handle Tax Rate
    $(document).on("change", "#TaxRateId", function () {
        $(this).find("option[value=" + $(this).val() + "]").attr('selected', true).siblings().attr('selected', false);
        //var selectedValue = $(this).val();
        calculateGrossPrice();
    });

    function getDetails(url, id) {
        $.ajax({
            type: "GET",
            url: url,
            data: { "id": id },
            success: function (data) {
                $('#divEdit').html(data);
                setupEditor('Specification', '');
                handleCheckBoxEvent(); // lib.js
                handleRadios(); // lib.js
                setupNumber(); // lib.js
                populateServiceCategoryComboTree();
                poplulateServicePackDurationSelectList('DurationId', null);
                populateDualListBoxListeners();
                checkPreviewPhoto();


                $('#PackageFee').on("input", function () {
                    calculateGrossPrice();
                });

                $('#TotalFee').on("input", function () {
                    calculateGrossPrice();
                });

                $("#DiscountId").change(function () {
                    calculateGrossPrice();
                });
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
        return false;
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

    function create(url, formId) {
        var form = $("#" + formId);

        var params = form.serializeArray();
        var formData = new FormData();
        formData.append("File", $('input[type=file]')[0].files[0]);
        $.each(params, function (i, val) {
            if (val.name === 'PackageFee') {
                formData.append('PackageFee', $("#PackageFee").val().replace(/,/g, ""));
            }
            else if (val.name === 'TotalFee') {
                formData.append('TotalFee', $("#TotalFee").val().replace(/,/g, ""));
            }
            else {
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
                    //resetControls(form);
                    var homePage = window.ServicePackIndexUrl;
                    showMessageWithTitle(jqXhr.status, response.Data, "success", 20000);
                    window.location.href = '/Admin/ServicePack/Index';
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

    function edit(url, formId) {
        var form = $("#" + formId);
        var params = form.serializeArray();
        var formData = new FormData();
        formData.append("File", $('input[type=file]')[0].files[0]);
        $.each(params, function (i, val) {
            if (val.name === 'PackageFee') {
                formData.append('PackageFee', $("#PackageFee").val().replace(/,/g, ""));
            }
            else if (val.name === 'TotalFee') {
                formData.append('TotalFee', $("#TotalFee").val().replace(/,/g, ""));
            }
            else {
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
                    //search();
                    showMessageWithTitle(jqXhr.status, response.Data.Message, "success", 20000);
                    // window.location.href = window.ServicePackIndexUrl;
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

    //function deleteData(url, id) {
    //    var params = { id: id };
    //    $.ajax({
    //        type: "DELETE",
    //        url: url,
    //        data: params,
    //        success: function (response, textStatus, jqXhr) {
    //            if (response.Status === 0) {
    //                search();
    //                showMessageWithTitle(jqXhr.status, response.Data, "success", 20000);
    //            } else {
    //                if (response.Errors !== null) {
    //                    var result = '';
    //                    $.each(response.Errors, function (i, obj) {
    //                        result += obj.ErrorMessage + '<br/>';
    //                    });
    //                    showMessageWithTitle(500, result, "error", 50000);
    //                }
    //            }
    //        },
    //        error: function (jqXhr, textStatus, errorThrown) {
    //            handleAjaxErrors(jqXhr, textStatus, errorThrown);
    //        }
    //    });
    //    return false;
    //}

    //function updateStatus(url, id, status) {
    //    var params = { id: id, status: status };
    //    $.ajax({
    //        type: "PUT",
    //        url: url,
    //        data: params,
    //        success: function (response, textStatus, jqXhr) {
    //            if (response.Status === 0) {
    //                search();
    //                showMessageWithTitle(jqXhr.status, response.Data, "success", 20000);
    //            } else {
    //                if (response.Errors !== null) {
    //                    var result = '';
    //                    $.each(response.Errors, function (i, obj) {
    //                        result += obj.ErrorMessage + '<br/>';
    //                    });
    //                    showMessageWithTitle(500, result, "error", 50000);
    //                }
    //            }
    //        },
    //        error: function (jqXhr, textStatus, errorThrown) {
    //            handleAjaxErrors(jqXhr, textStatus, errorThrown);
    //        }
    //    });
    //    return false;
    //}

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

    //$(document).on("change", ".changeStatus", function (e) {
    //    e.preventDefault();

    //    var id = $(this).data('id');
    //    var url = $(this).data('url');
    //    var warning = $(this).data('warning');
    //    var status = $(this).is(":checked");

    //    var box = bootbox.confirm({
    //        className: "my-modal",
    //        size: 'small',
    //        title: window.Warning,
    //        message: warning,
    //        buttons: {
    //            confirm: {
    //                label: window.Ok,
    //                className: 'confirm-button-class'
    //            },
    //            cancel: {
    //                label: window.Cancel,
    //                className: 'cancel-button-class'
    //            }
    //        },
    //        callback: function (result) {
    //            if (result) {
    //                updateStatus(url, id, status);
    //                box.modal('hide');
    //            }
    //        },
    //        onEscape: function () { return false; }
    //    });

    //    box.css({
    //        'top': '50%',
    //        'margin-top': function () {
    //            return -(box.height() / 2);
    //        }
    //    });
    //    return false;
    //});

    //$(document).on("click", ".deleteItem", function (e) {
    //    e.preventDefault();

    //    var id = $(this).data('id');
    //    var url = $(this).data('url');
    //    var warning = $(this).data('warning');

    //    var box = bootbox.confirm({
    //        className: "my-modal",
    //        size: 'small',
    //        title: window.Warning,
    //        message: warning,
    //        buttons: {
    //            confirm: {
    //                label: window.Ok,
    //                className: 'confirm-button-class'
    //            },
    //            cancel: {
    //                label: window.Cancel,
    //                className: 'cancel-button-class'
    //            }
    //        },
    //        callback: function (result) {
    //            if (result) {
    //                deleteData(url, id);
    //                box.modal('hide');
    //            }
    //        },
    //        onEscape: function () { return false; }
    //    });

    //    box.css({
    //        'top': '50%',
    //        'margin-top': function () {
    //            return -(box.height() / 2);
    //        }
    //    });
    //    return false;
    //});

    //$(document).on("click", ".editItem", function (e) {
    //    e.preventDefault();

    //    var id = $(this).data('id');
    //    var url = $(this).data('url');

    //    getDetails(url, id);

    //    //Go to top
    //    $('html, body').animate({ scrollTop: 0 }, 'fast');
    //    return false;
    //});

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

    // Dynamic Rows Code
    function addRow() {
        var table = $(".dynamic-table");
        var tableArray = table.data("array");

        // Get max row id and set new id
        var newid = 0;
        var rowArray = '';
        $.each(table.find("tbody tr"), function () {
            if (parseInt($(this).data("id")) > newid) {
                newid = parseInt($(this).data("id"));
            }
            //$(this).addClass('active').siblings().removeClass('active');
            $(this).addClass('bg-grey').removeClass('active');
            $(this).find('input:text, input:radio, input:checkbox, select, textarea').attr('readonly', true);
            $('.readonly input:checkbox').click(function () { return false; });
            $('.readonly input:checkbox').keydown(function () { return false; });
        });
        newid++;
        rowArray = tableArray + '[' + newid + ']';

        var tr = $("<tr></tr>", {
            id: "row" + newid,
            "data-id": newid,
            "data-array": rowArray,
            "class": "active"
        });

        //if (table.find('tbody tr:nth(0) td:first-child')) {
        //    $("<td></td>", {
        //        'class': 'text-center',
        //        'text': table.find('tr').length
        //    }).prependTo($(tr));
        //}

        // loop through each td and create new elements with name of newid
        $.each(table.find("tbody tr:nth(0) td"), function () {
            var curTd = $(this);
            var children = curTd.children();

            // add new td and element if it has a nane
            if ($(this).data("name") !== undefined && $(this).data("name") !== null && $(this).data("name") !== '') {
                var td = $("<td></td>", { "class": $(curTd).attr("class"), "data-name": $(curTd).data("name") });

                var c = $(curTd).find($(children[0]).prop('tagName')).clone();
                if (!c.hasClass(".ignored")) {
                    c.not(".ignored").val('');
                }

                c.attr({
                    "id": rowArray + '.' + $(curTd).data("name"),
                    "name": rowArray + '.' + $(curTd).data("name"),
                    'readonly': false
                });

                if (c.is('input:text:not(".ignored")')) {
                    c.attr('value', '');
                    c.bind("change keyup paste", function () {
                        c.attr('value', c.val());
                    });
                }

                if (c.is('textarea')) {
                    c.text('');
                    c.bind("change keyup paste", function () {
                        c.html(c.val());
                    });
                }

                if (c.is('input:checkbox:not(".ignored")')) {
                    c.removeAttr('checked');
                    c.val('false');
                    c.bind("click", function () {
                        c.attr({
                            "checked": c.is(":checked"),
                            "value": c.is(":checked")
                        });
                    });
                }

                if (c.is('select')) {
                    c.removeAttr('selected');
                }

                c.appendTo($(td));
                td.appendTo($(tr));
            }
        });

        //add delete button and td
        $("<td class='text-center'></td>").append("<button data-toggle='tooltip' data-title='Edit' class='btn btn-success glyphicon glyphicon-pencil row-edit'></button> <button data-toggle='tooltip' data-title='Delete' class='btn btn-warning glyphicon glyphicon-trash row-remove'></button>").appendTo($(tr));

        //add the new row
        $(tr).appendTo(table);

        setupNumber();

        //rebind Validate
        $('form').data('validator', null);
        $.validator.unobtrusive.parse($('form'));


        $(".row-remove").on("click", function () {
            $(this).closest("tr").remove();
            return false;
        });

        $(".row-edit").on("click", function () {
            $(this).closest("tr").css("background-color", "transparent");
            $(this).closest("tr").find('input:text, input:radio, input:checkbox, select, textarea').attr('readonly', false);
            return false;
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
})(jQuery);