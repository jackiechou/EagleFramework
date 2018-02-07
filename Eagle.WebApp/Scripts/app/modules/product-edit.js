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
            //style: [
            //    { text: "Highlight Error", value: "hlError" },
            //    { text: "Highlight OK", value: "hlOK" },
            //    { text: "Inline Code", value: "inlineCode" }
            //],
            //stylesheets: [
            //"~/content/editorStyles.css"
            //],
            //messages: {
            //    directoryNotFound: "Directory not found!",
            //    deleteFile: "Are you sure? This action cannot be undone.",
            //    invalidFileType: "Supported file types are {1}. Please retry your upload.",
            //    overwriteFile: "Do you want to overwrite the file with name '{0}'?"
            //},
            value: data,
            imageBrowser: {
                //fileTypes: ".png,.gif,.jpg,.jpeg",
                path: window.UploadNewsImageFolder,
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
                //fileTypes: "*.zip",
                path: window.UploadNewsFileFolder,
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

        editor.data("kendoEditor").toolbar.element.find(".k-insertImage").parent().click(function () {
            setTimeout(function () {
                var imageBrowser = $(".k-imagebrowser").data("kendoImageBrowser");
                imageBrowser.listView.bind("dataBound", function (e) {
                    if (imageBrowser._path === "/") {
                        imageBrowser.element.find(".k-toolbar-wrap").hide();
                    } else {
                        imageBrowser.element.find(".k-toolbar-wrap").show();
                    }
                });
            });
        });
    }

    setupEditor('Specification', '');
    
    //START DYNAMIC TABLE EXTENSION - OPTION - Dynamic Rows Code
    function handleInputNumber() {
        $('input[type=number]').on('wheel', function (e) {
            e.preventDefault();
        });

        $('input[type=number]').keyup(function (event) {
            // skip for arrow keys
            if (event.which >= 37 && event.which <= 40) return;

            // format number
            $(this).val(function (index, value) {
                return value
                    .replace(/\D/g, "");
            });
        });
    }

    handleInputNumber();
    
    function addOption(index) {
        //var table = $(this).parents('div.panel-option').find('.option-table');
        //var lastRow = table.find("tr").last();
        //var optionName = lastRow.find('[data-name="OptionName"] > input[type="text"]').val();
        //if (optionName === null || optionName === '' || optionName === undefined) {
        // return false;
        //}
        var table = $('.attribute-' + index + '-option-table');
        var tableArray = table.data("array");

        // Get max row id and set new id
        var newid = 0;
        var rowArray = '';
        $.each(table.find("tbody tr"), function () {
            if (parseInt($(this).data("id")) > newid) {
                newid = parseInt($(this).data("id"));
            }
            $(this).addClass('bg-grey').removeClass('active');
            $(this).find('input[type=text],input[type=number], input[type=radio], input[type=checkbox], select, textarea').attr('readonly', true);
            $('.readonly input:checkbox').click(function () { return false; });
            $('.readonly input:checkbox').keydown(function () { return false; });
        });
        newid++;
        rowArray = tableArray + '[' + newid + ']';

        var tr = $("<tr></tr>", {
            id: "optionRow" + newid,
            "data-id": newid,
            "data-array": rowArray,
            "class": "active"
        });

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

                if (c.is('input[type=number]')) {
                    c.attr('value', '');
                    c.bind("change keyup paste", function () {
                        // skip for arrow keys
                        if (event.which >= 37 && event.which <= 40) return;

                        // format number
                        c.val(function (index, value) {
                            return value
                            .replace(/\D/g, "");
                        });
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
        $("<td class='text-center'></td>").append('<button data-title="Edit" data-index="' + index + '" class="btn btn-success glyphicon glyphicon-pencil edit-option"></button> ' +
            '<button data-title="Delete" data-index="' + index + '" class="btn btn-warning glyphicon glyphicon-trash remove-option"></button>').appendTo($(tr));

        //add the new row
        $(tr).appendTo(table);

        //add option events
        bindOptionEvents();

        setupNumber();

        //rebind Validate
        $('form').data('validator', null);
        $.validator.unobtrusive.parse($('form'));

        return false;
    }
    function removeOption(url, id) {
        var params = { id: id };
        $.ajax({
            type: "DELETE",
            url: url,
            data: params,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
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
    function bindOptionEvents() {
        $('.add-option').on("click", function () {
            var opionIndex = $(this).data('index');
            var table = $(this).parents('div.panel-option').find('.option-table');
            var lastRow = table.find("tr").last();
            var optionName = lastRow.find('[data-name="OptionName"] > input[type="text"]').val();
            if (optionName !== null && optionName !== '' && optionName !== undefined) {
                addOption(opionIndex);
            }
            return false;
        });

        $('.edit-option').on("click", function () {
            var id = $(this).data('id');
            $(this).closest("tr").css("background-color", "transparent");
            $(this).closest("tr").find('input[type=text],input[type=number], input[type=radio], input[type=checkbox], select, textarea').attr('readonly', false);
            return false;
        });

        $('.remove-option').on("click", function () {
            var id = $(this).data('id');
            var url = $(this).data('url');

            if (id !== null && id !== undefined && id !== '' && id > 0) {
                removeOption(url, id);
            }

            $(this).closest("tr").remove();
            return false;
        });

        handleInputNumber();
    }
    //END DYNAMIC TABLE

    //START DYNAMIC HTML EXTENSION - ATTRIBUTE
    function addRow() {
        var container = $(".dynamic-container");
        var containerBody = $(".dynamic-container-inner");
        var containerArray = container.data("array");
        var mode = container.data("mode");

        // Get max row id and set new id
        var newid = 0;
        var rowArray = '';
        $.each(container.find(".dynamic-row"), function () {
            if (parseInt($(this).data("id")) > newid) {
                newid = parseInt($(this).data("id"));
            }
            $(this).addClass('bg-grey').removeClass('active');
            $(this).find('input[type=text],input[type=number], input[type=radio], input[type=checkbox], select, textarea').attr('readonly', true);
            $('.readonly input:checkbox').click(function () { return false; });
            $('.readonly input:checkbox').keydown(function () { return false; });
        });
        newid++;
        rowArray = containerArray + '[' + newid + ']';

        var tr = $("<div></div>", {
            id: "row" + newid,
            "data-id": newid,
            "data-array": rowArray,
            "class": "dynamic-row padding-10 active"
        });

        var trBody = $("<div class='dynamic-main-row row'></div>").appendTo($(tr));

        // loop through each column and create new elements with name of newid
        $.each(container.find("div.dynamic-row:nth(0) div.dynamic-col"), function () {
            var currentColumn = $(this);
            var children = currentColumn.children();

            // add new column and element if it has a nane
            if ($(this).data("name") !== undefined && $(this).data("name") !== null && $(this).data("name") !== '') {
                var column = $("<div></div>", { "class": $(currentColumn).attr("class"), "data-name": $(currentColumn).data("name") });

                var c = $(currentColumn).find($(children[0]).prop('tagName')).clone();
                if (!c.hasClass(".ignored")) {
                    c.not(".ignored").val('');
                }

                c.attr({
                    "id": rowArray + '.' + $(currentColumn).data("name"),
                    "name": rowArray + '.' + $(currentColumn).data("name"),
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

                c.appendTo($(column));
                column.appendTo($(trBody));
            }
        });

        //add delete button and column
        $("<div class='text-center'></div>").append('<button type="button" data-title="LoadOptionForm" data-index="' + newid + '" data-mode="' + mode + '" class="btn btn-default glyphicon glyphicon-plus options-load" ></button> ' +
            '<button data-toggle="tooltip" data-title="Edit" data-index="' + newid + '" class="btn btn-success glyphicon glyphicon-pencil row-edit"></button> ' +
            '<button data-toggle="tooltip" data-title="Delete" data-index="' + newid + '" class="btn btn-warning glyphicon glyphicon-trash row-remove"></button>').appendTo($(trBody));

        //add the new option row
        $('<div class="dynamic-sub-row row"><div class="padding-left-right-160 padding-top-bottom-10 attribute-' + newid + '-option-container" id="Attributes[' + newid + '].OptionContainer"></div></div>').appendTo($(tr));

        //add the new row
        $(tr).appendTo(containerBody);

        //rebind Validate
        $('form').data('validator', null);
        $.validator.unobtrusive.parse($('form'));

        //bind row events
        bindRowEvents();
    }

    function removeRow(url, id) {
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

    function populateCreateProductAttribute(container) {
        var url = window.CreateProductAttributeUrl;
        $.ajax({
            type: "GET",
            url: url,
            //global: false,
            //beforeSend: function () {
            //    $.unblockUI();
            //},
            success: function (data) {
                container.html(data);
                bindRowEvents();
                return false;
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
                return false;
            }
        });
    }

    function bindRowEvents() {
        $('.options-load').on("click", function () {
            var index = $(this).data('index');
            var mode = $(this).data('mode');
            var url = window.CreateProductAttributeOptionUrl;
            var params = { "mode": mode, "index": index };
            var optionContainer = $('.attribute-' + index + '-option-container');
            var attributeName = $(this).parent().parent('div.dynamic-main-row').find("input[type=text]").val();

            var table = optionContainer.find('.option-table');
            var existingOptions = table.find("tr[id^='optionRow']");

            if (existingOptions.length > 0) {
                var lastRow = table.find("tr[id^='optionRow']").last();
                var optionName = lastRow.find('[data-name="OptionName"] > input[type="text"]').val();
                if (optionName !== null && optionName !== '' && optionName !== undefined) {
                    addOption(index);
                }
            }
            else {
                if (attributeName !== null && attributeName !== '' && attributeName !== undefined) {
                    $.ajax({
                        type: "GET",
                        url: url,
                        data: params,
                        //global: false,
                        //beforeSend: function () {
                        //    $.unblockUI();
                        //},
                        success: function (data) {
                            optionContainer.html(data);
                            bindOptionEvents();
                        },
                        error: function (jqXhr, textStatus, errorThrown) {
                            handleAjaxErrors(jqXhr, textStatus, errorThrown);
                        }
                    });
                }
            }

            return false;
        });

        $(".row-add").on("click", function () {
            //var rowCount = $('table#myTable tr:last').index() + 1;
            var container = $(this).parents("div.product-attribute-container");
            var divCount = container.find('div.dynamic-container-inner').children().length;
            if (divCount > 0) {
                var lastRow = container.find('div.dynamic-container-inner').children().last();
                var attributeName = lastRow.find('[data-name="AttributeName"] > input[type="text"]').val();
                if (attributeName !== null && attributeName !== '' && attributeName !== undefined) {
                    addRow();
                }
            }
            else {
                populateCreateProductAttribute(container);
            }
            return false;
        });

        $(".row-edit").on("click", function () {
            $(this).closest("div.dynamic-main-row").css("background-color", "transparent");
            $(this).closest("div.dynamic-main-row").find('input:text, input:radio, input:checkbox, select, textarea').attr('readonly', false);
            return false;
        });

        $(".row-remove").on("click", function () {
            var id = $(this).data('id');
            var url = $(this).data('url');

            if (id !== null && id !== undefined && id !== '' && id > 0) {
                removeRow(url, id);
            }

            $(this).parents("div.dynamic-row").remove();
            return false;
        });

        bindOptionEvents();
    }
    bindRowEvents();
    //END DYNAMIC HTML

    //Check and privew photo
    function checkPreviewPhoto() {
        $('#File').checkFile({
            allowedExtensions: ['jpg', 'jpeg', 'png', 'gif', 'bmp'],
            allowedSize: 15 //15MB	
        });
        previewPhoto();
    }

    checkPreviewPhoto();

    //Product Type
    function populateProductTypeSelectBox() {
        var categoryId = $('#CategoryId').val();
        var select = $('#ProductTypeId');
        var url = select.data('url');
        var selectedValue = select.data('value');
        var params = { categoryId: categoryId, selectedValue: selectedValue, isShowSelectText: true };

        select.empty();
        if (categoryId !== null && categoryId !== '' && categoryId !== undefined && categoryId > 0) {
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
                        //select.find('option:first').attr("selected", "selected");
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
        else {
            select.append($('<option/>', { value: 'Null', text: " ---" + window.None + " --- " }));
        }
        return false;
    }

    //Category ComboTree
    function populateCategoryComboTree(categoryId) {
        var select = $("#" + categoryId);
        var url = select.data('url');
        var selectedValue = select.val();

        var params = {
            "selectedId": selectedValue,
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
                        select.combotree('setValue', selectedValue);
                        populateProductTypeSelectBox();
                    },
                    onClick: function (node) {
                        selectedValue = node.id;
                        $(this).val(selectedValue);
                        if (selectedValue !== null && selectedValue !== '') {
                            populateProductTypeSelectBox();
                        }
                    }
                });
            }
        });
    }
    populateCategoryComboTree('CategoryId');

    function calculateGrossPrice() {
        var netFee = $("#NetPrice").val();
        if (netFee !== null && netFee !== '' && netFee !== undefined) {
            var tax = 0, discount = 0;
            var netPrice = parseFloat($("#NetPrice").val().replace(/,/g, ''));
            if ($("#TaxRateId").val() !== '')
                tax = parseFloat($("#TaxRateId").find('option:selected').text().replace('%', '').replace(/,/g, ''));
            if ($("#DiscountId").val() !== '')
                discount = parseFloat($("#DiscountId").find('option:selected').text().replace('%', '').replace(/,/g, ''));

            var total = netPrice + (netPrice * tax / 100) - (netPrice * discount / 100);
            var totalGrossPrice = total.toFixed();
            if (totalGrossPrice !== '0')
                $('#GrossPrice').val(addCommas(totalGrossPrice));
            else
                $('#GrossPrice').val(0);
        } else {
            $("#NetPrice").val(0);
            $('#GrossPrice').val(0);
        }
    }

    $('#NetPrice').on("input", function () {
        calculateGrossPrice();
    });

    $("#DiscountId").change(function () {
        calculateGrossPrice();
    });

    $("#TaxRateId").change(function () {
        calculateGrossPrice();
    });
    
    //set up product gallery of a product
    function populateProductGalleryFiles() {
        $("#ProductGalleryFiles").fileinput({
            uploadUrl: '/file-upload-batch/2',
            maxFilePreviewSize: 10240,
            overwriteInitial: false,
            showUpload: false
        });
    }

    populateProductGalleryFiles();
    
    function getDetails(url, id) {
        $.ajax({
            type: "GET",
            url: url,
            data: { "id": id },
            success: function (data) {
                $('#divEdit').html(data);

                setupEditor('Specification', '');
                populateCategoryComboTree('CategoryId');
                checkPreviewPhoto();
                handleCheckBoxes();
                handleRadios();
                invokeDateTimePicker('dd/MM/yyyy');
                bindRowEvents();
                //  populateProductGalleryFiles();

                $('#NetPrice').on("input", function () {
                    //var dInput = this.text;
                    //console.log(dInput);
                    calculateGrossPrice();
                });

                $("#DiscountId").change(function () {
                    calculateGrossPrice();
                });

                $("#TaxRateId").change(function () {
                    calculateGrossPrice();
                });
                return false;
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
    
    function edit(url, formId, back) {
        //$('.product-attribute-container').children(':input').each(function () {
        //    if ($(this).val() === '' || $(this).val() === null) {
        //        $(this).closest("div.dynamic-row").remove();
        //    } 
        //});

        var form = $("#" + formId);
        var formData = new FormData();
        formData.append("File", $('input[type=file]')[0].files[0]);
        var params = form.serializeArray();
        $.each($('input[name="ProductGalleryFiles"]'), function (i, obj) {
            $.each(obj.files,
                function (j, file) {
                    formData.append('productGalleryFiles[' + j + ']', file);
                });
        });

        $.each(params, function (i, val) {
            if (val.name === 'NetPrice') {
                formData.append('NetPrice', $("#NetPrice").val().replace(/,/g, ""));
            }
            else if (val.name === 'GrossPrice') {
                formData.append('GrossPrice', $("#GrossPrice").val().replace(/,/g, ""));
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
                    //getDetails(url, response.Data.Id);
                    showMessageWithTitle(jqXhr.status, response.Data.Message, "success", 20000);
                    if (back)
                        window.location.href = window.ProductHomehUrl;
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
    
    $(document).on("click", ".edit", function (e) {
        e.preventDefault();

        var url = $(this).data('url');
        var form = $(this).data('form');

        if (!$("#" + form).valid()) { // Not Valid
            return false;
        }
        else {
            edit(url, form, false);
            return false;
        }
    });

    $(document).on("click", ".editBack", function (e) {
        e.preventDefault();

        var url = $(this).data('url');
        var form = $(this).data('form');

        if (!$("#" + form).valid()) { // Not Valid
            return false;
        }
        else {
            edit(url, form, true);
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
})(jQuery);