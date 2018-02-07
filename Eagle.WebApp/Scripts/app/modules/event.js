(function ($) {
    $.fn.checkFile = function (options) {
        var defaults = {
            allowedExtensions: ['jpg', 'jpeg', 'png', 'gif'],
            allowedSize: 15, //15MB	
            success: function () { }
        };

        options = $.extend(defaults, options);

        if ($(this).value === "") {
            return;
        }

        // get the file name, possibly with path (depends on browser)
        var fileName = $(this).val();
        var fileNameLower = fileName.toLowerCase();
        var extension = fileNameLower.substr((fileNameLower.lastIndexOf('.') + 1));

        var fileSize = $(this)[0].files[0].size; //size in kb
        fileSize = fileSize / 1048576; //size in mb  

        if ($.inArray(extension, options.allowedExtensions) === -1) {
            if (fileSize > options.allowedSize) {
                showNotification('error', 'Wrong extension type! You can upload only ' + options.allowedExtensions + ' extension file, and file size is less than ' + options.allowedSize + ' MB');
            } else {
                showNotification('error', 'Wrong extension type! You can upload only ' + options.allowedExtensions + ' extension file');
            }
            $(this).focus();
        } else {
            if (fileSize > options.allowedSize) {
                showNotification('error', 'You can only upload file up to ' + options.allowedSize + ' MB');
                $(this).focus();
            } else {
                hideMessage();
                options.success();
            }
        }
    };

    function previewImage() {
        $("input[type=file][name=File]").on('change', function () {
            if (typeof (FileReader) !== "undefined") {
                var imageHolder = $("#image-holder");
                imageHolder.empty();

                var file = $(this)[0].files[0];
                if (file !== null) {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $("<img />", {
                            "width": 150,
                            "height": 150,
                            "src": e.target.result,
                            "class": "thumb-image"
                        }).appendTo(imageHolder);
                    };

                    imageHolder.show();
                    reader.readAsDataURL(file);
                }

                var imageContainer = $("#image-container");
                imageContainer.hide();

                //Check file
                if ($(this).val() !== '' && $(this).val() !== null) {

                    $(this).checkFile();
                }
            } else {
                console.log("This browser does not support FileReader.");
            }
        });
    }

    previewImage();

    function resetImage() {
        $(document).on("click", ".resetImage", function () {
            $('#File').val('');

            var imageHolder = $("#image-holder");
            imageHolder.hide();

            var imageContainer = $("#image-container");
            imageContainer.show();
        });
    }

    resetImage();

    //Event Type ComboTree
    function populateEventTypeComboTree() {
        var select = $("#TypeId");
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

   populateEventTypeComboTree();

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
                path: window.UploadEventImageFolder,
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
                path: window.UploadEventFileFolder,
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

        //editor.data("kendoEditor").toolbar.element.find(".k-insertImage").parent().click(function () {
        //    setTimeout(function () {
        //        var imageBrowser = $(".k-imagebrowser").data("kendoImageBrowser");
        //        imageBrowser.listView.bind("dataBound", function (e) {
        //            if (imageBrowser._path === "/") {
        //                imageBrowser.element.find(".k-toolbar-wrap").hide();
        //            } else {
        //                imageBrowser.element.find(".k-toolbar-wrap").show();
        //            }
        //        });
        //    });
        //});
    }

    setupEditor('EventMessage', '');

    function getDetails(url, id) {
        $.ajax({
            type: "GET",
            url: url,
            data: { "id": id },
            success: function (data) {
                $('#divEdit').html(data);
                populateEventTypeComboTree();
                handleCheckBoxEvent();
                handleRadios();
                previewImage();
                resetImage();
                invokeDateTimePicker('dd/MM/yyyy');
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    function resetControls(formId, mode) {
        var form = $("#" + formId);

        if (mode === 'create') {
            form.find('input:text, input:password, input:file, select, textarea').not('.ignored').val('');
            form.find('input:radio, input:checkbox').removeAttr('checked').removeAttr('selected');
            form.find('input[type="number"]').val(0);
            form.find('input[type=file]').val('');
            form.find('select').find('option:first').attr('selected', true).siblings().attr('selected', false);
            form.find("[data-valmsg-summary=true]").removeClass("validation-summary-errors").addClass("validation-summary-valid").find("ul").empty();
            form.find("[data-valmsg-replace]").removeClass("field-validation-error").addClass("field-validation-valid").empty();

            var editor = $("#EventMessage").data("kendoEditor");
            if (editor !== null && editor !== undefined) {
                editor.value("");
            }
        }
       else {
            form.find('input:text, input:password, input:file, select, textarea').not('.ignored').val('');
            form.find('input:radio, input:checkbox').removeAttr('checked').removeAttr('selected');
            form.find('input[type="number"]').val(0);
            form.find('input[type=file]').val('');
            form.find('select').find('option:first').attr('selected', true).siblings().attr('selected', false);
            form.find("[data-valmsg-summary=true]").removeClass("validation-summary-errors").addClass("validation-summary-valid").find("ul").empty();
            form.find("[data-valmsg-replace]").removeClass("field-validation-error").addClass("field-validation-valid").empty();
        }
    }
    
    ////PAGINATION
    function search() {
        var formUrl = window.SearchEventUrl;
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
        var formData = new FormData();
        formData.append("File", $('input[type=file]')[0].files[0]);
        var params = form.serializeArray();
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
                    resetControls(formId, 'create');
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
            }, error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
                   
            }
        });
        
        return false;
    }

    function edit(url, formId) {
        var form = $("#" + formId);
        var formData = new FormData();
        formData.append("File", $('input[type=file]')[0].files[0]);
        var params = form.serializeArray();
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

    $(document).on("click", ".generate-code", function () {
        var url = $(this).data('url');
        $.ajax({
            type: "GET",
            url: url,
            beforeSend: function () {
                $.unblockUI();
            },
            success: function (data) {
                $('#EventCode').html(data);
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
        return false;
    });

    //SEARCH
    $(document).on("click", ".search", function () {
        search();
    });

    //RESET
    $(document).on("click", ".reset", function () {
        var formId = $(this).data('form');
        var mode = $(this).data('mode');
        resetControls(formId, mode);
    });

    //DETAILS
    $(document).on("click", ".editItem", function (e) {
        e.preventDefault();

        var id = $(this).data('id');
        var url = $(this).data('url');

        getDetails(url, id);
        return false;
    });

    //CREATE
    $(document).on("click", ".create", function (e) {
        e.preventDefault();

        var form = $(this).data('form');
        var url = $(this).data('url');

        if (!$("#" + form).valid()) { // Not Valid
            return false;
        } else {
            if ($("#TypeId").val() === '' || $("#TypeId").val() === '0') {
                showMessageWithTitle('500', 'Please select Event Type', "error", 20000);
                return false;
            } else {
                $("#" + form).valid();
                create(url, form);
                return false;
            }
        }
    });

    //EDIT
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

    //UPDATE
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
})(jQuery);