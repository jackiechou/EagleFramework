﻿(function ($) {
    //Check and privew photo
    function checkPreviewPhoto() {
        $('#File').checkFile({
            allowedExtensions: ['jpg', 'jpeg', 'png', 'gif', 'bmp'],
            allowedSize: 15 //15MB	
        });
        previewPhoto();
    }

    checkPreviewPhoto();

    function populateRegionDropDownList(provinceControlId, regionControlId, isShowSelectText) {
        var select = $(regionControlId);
        var selectedValue = select.data('id');
        var url = select.data('url');
        var provinceId = $(provinceControlId).val();

        if (provinceId !== null && provinceId !== undefined && provinceId !== '') {
            var params = { 'provinceId': provinceId, 'isShowSelectText': isShowSelectText };

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

                        if (selectedValue !== null && selectedValue !== '' && selectedValue !== undefined) {
                            select.find('option').removeAttr('selected').filter('[value=' + selectedValue + ']').attr('selected', true);
                        }

                        select.select2({ width: '100%' });
                    } else {
                        select.append($('<option/>', {
                            value: '',
                            text: 'None'
                        }));
                    }
                });
        }
    }

    function populateProvinceDropDownList(countryControlId, provinceControlId, regionControlId, isShowSelectText) {
        var select = $(provinceControlId);
        var selectedValue = select.data('id');
        var url = select.data('url');
        var countryId = $(countryControlId).val();

        if (countryId !== null && countryId !== undefined && countryId !== '') {
            var params = { 'countryId': countryId, 'isShowSelectText': isShowSelectText };

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

                        if (selectedValue !== null && selectedValue !== '' && selectedValue !== undefined) {
                            select.find('option').removeAttr('selected').filter('[value=' + selectedValue + ']').attr('selected', true);
                        }

                        populateRegionDropDownList(provinceControlId, regionControlId, false);

                        select.select2({ width: '100%' });
                    } else {
                        select.append($('<option/>', {
                            value: '',
                            text: 'None'
                        }));
                    }
                });
        }
    }

    function populateCountryDropDownList(countryControlId, provinceControlId, regionControlId, isShowSelectText) {
        var select = $(countryControlId);
        var selectedValue = select.data('id');
        var url = select.data('url');
        var params = { "selectedValue": selectedValue, "isShowSelectText": isShowSelectText };

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

                if (selectedValue !== null && selectedValue !== '' && selectedValue !== undefined) {
                    select.find('option').removeAttr('selected').filter('[value=' + selectedValue + ']').attr('selected', true);
                }

                populateProvinceDropDownList(countryControlId, provinceControlId, regionControlId, false);

                select.select2({ width: '100%' });
            });
    }

    populateCountryDropDownList('.Address-CountryId', '.Address-ProvinceId', '.Address-RegionId', true);

    $(document).on("change", ".Address-CountryId", function () {
        populateProvinceDropDownList('.Address-CountryId', '.Address-ProvinceId', '.Address-RegionId', true);
    });

    $(document).on("change", ".Address-ProvinceId", function () {
        populateRegionDropDownList('.Address-ProvinceId', '.Address-RegionId', true);
    });


    function setupEditor(editorId, data) {
        // create Editor from textarea HTML element with default set of tools
        var editor = $("#" + editorId);
        // var value = editor.data("kendoEditor").value();
        editor.kendoEditor({
            encoded: false,
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
                path: window.UploadCompanyImageFolder,
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
            }
        });
    }
    setupEditor('Description', '');

    //Company Parent ComboTree
    function populateParentComboTree() {
        var select = $("#ParentId");
        var url = select.data('url');
        var selectedValue = select.val();

        var params = {
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

    populateParentComboTree();

    //Create company tree
    function createTree(data) {
        var $tree = $("#tree");
        $tree.tree({
            lines: true,
            animate: true,
            data: data,
            formatter:function(node){
                var s = node.text;
                if (node.children){
                    s += '&nbsp;<span style=\'color:blue\'>(' + node.children.length + ')</span>';
                }
                return s;
            },
            onClick: function (node) {
                var url = window.EditCompanyUrl;
                getDetails(url, node.id);
            },
            //onDblClick: function (node) {
            //    var url = window.EditCompanyUrl;
            //    getDetails(url, node.id);
            //},
        });
    }

    //function createTreeGrid() {
    //    $('.easyui-treegrid').treegrid({
    //        //url: window.GetCompanyTreeUrl,
    //        //method: 'get',
    //        idField: 'id',
    //        treeField: 'name',
    //        //columns: [[
    //        //   //{ field: 'id', title: 'No', width: 180 },
    //        //   { field: 'text', title: 'CompanyName', width: 180 }
    //        //]],
    //        collapsible: false,
    //        singleSelect: true,
    //        lines: true,
    //        rownumbers: true,
    //        remoteFilter: false,
    //        pagination: true,
    //        showFooter: true,
    //        pageSize: 10,
    //        pageList: [2,10,20],
    //        fitColumns: true,
    //        animate: true
    //    });
    //}

    //createTreeGrid();

    function loadTree() {
        var selectedId = $('#ParentId').val();
        $.ajax({
            type: "GET",
            url: window.GetCompanyTreeUrl,
            data: { 'selectedId': selectedId, 'isRootShowed': true },
            success: function (data) {
                createTree(data);
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
        return false;
    }

    loadTree();
    
    function getDetails(url, id) {
        $.ajax({
            type: "GET",
            url: url,
            data: { "id": id },
            success: function (data) {
                $('#divEdit').html(data);
                handleCheckBoxEvent();
                handleRadios();
                loadTree();
                populateParentComboTree();
                checkPreviewPhoto();
                setupEditor('Description', '');
                populateCountryDropDownList('.Address-CountryId', '.Address-ProvinceId', '.Address-RegionId', true);
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
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

    function reloadGrid() {
        $.ajax({
            type: "GET",
            url: window.CompanyListUrl,
            success: function (data) {
                $('#divList').html(data);
            }
        });
    }

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
                    reloadGrid();
                    resetControls(form);
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
                    reloadGrid();
                    getDetails(url, response.Data.Id);
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

    function deleteData(url, id) {
        var params = { id: id };
        $.ajax({
            type: "DELETE",
            url: url,
            data: params,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    reloadGrid();
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

    $(document).on("click", ".editItem", function (e) {
        e.preventDefault();

        var id = $(this).data('id');
        var url = $(this).data('url');

        getDetails(url, id);

        //Go to top
        $('html, body').animate({ scrollTop: 0 }, 'fast');
        return false;
    });

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
})(jQuery);