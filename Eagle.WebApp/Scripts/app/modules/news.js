(function ($) {
    invokeDateTimePicker('dd/MM/yyyy');

    function createTree(data) {
        var selectedValue = $('#CategoryId').val();
        var $tree = $("#tree");
        $tree.tree({
            lines: true,
            animate: true,
            data: data,
            formatter: function (node) {
                var s = node.text;
                if (node.children) {
                    s += '&nbsp;<span style=\'color:blue\'>(' + node.children.length + ')</span>';
                }
                return s;
            },
            onLoadSuccess: function () {
                if (selectedValue !== null) {
                    var node = $tree.tree('find', selectedValue);
                    if (node) {
                        $tree.tree('select', node.target);
                    }
                }
            },
            onClick: function (node) {
                $('#CategoryId').val(node.id);
            }
        });
    }

    function loadTree() {
        var selectedId = $('#CategoryId').val();
        $.ajax({
            type: "GET",
            url: window.GetNewsCategoryTreeUrl,
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

    setupEditor('MainText', '');
    
    function resetControls() {
        var validateObj = $('#frmNews');
        validateObj.find('input:text, input:password, input:file, select, textarea').not('.ignored').val('');
        validateObj.find('input:radio, input:checkbox').removeAttr('checked').removeAttr('selected');
        validateObj.find('input[type="number"]').val(0);
        validateObj.find('input[type=file]').val('');
        validateObj.find('select').find('option:first').attr('selected', true).siblings().attr('selected', false);
        validateObj.find("[data-valmsg-summary=true]").removeClass("validation-summary-errors").addClass("validation-summary-valid").find("ul").empty();
        validateObj.find("[data-valmsg-replace]").removeClass("field-validation-error").addClass("field-validation-valid").empty();

        var editor = $("#MainText").data("kendoEditor");
        if (editor !== null) {
            editor.value("");
        }
    }
    
    //function updateListOrders() {
    //    var positions = [];
    //    for (var i = 0; i < $('.Ids').length; i++) {
    //        positions.push({ 'NewsId': $('.Ids')[i].id, 'ListOrder': i+1 });
    //    }

    //    $.ajax({
    //        type: "PUT",
    //        url: window.UpdateNewsListOrderUrl,
    //        data: { 'listOrderEntry': JSON.stringify(positions) },
    //        success: function (data) {
    //            var result = JSON.parse(data);
    //            if (result.flag === 'true') {
    //                reloadData(data, 2);
    //                showMessageWithTitle(window.UpdateSuccessResource, result.message, "success", 20000);
    //            } else {
    //                showMessageWithTitle(window.UpdateFailure, result.message, "error", 20000);
    //            }
    //        },
    //        error: function (jqXhr, textStatus, errorThrown) {
    //            handleAjaxErrors(jqXhr, textStatus, errorThrown);
    //        }
    //    });
    //    return false;
    //}

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

    //var loadFile = function (event) {
    //    var output = document.getElementById('output');
    //    output.style.display = "block";
    //    output.src = URL.createObjectURL(event.target.files[0]);
    //};
    //var loadFile = function (event) {
    //    var reader = new FileReader();
    //    reader.onload = function () {
    //        var output = document.getElementById('output');
    //        output.src = reader.result;
    //    };
    //    reader.readAsDataURL(event.target.files[0]);
    //};

    ////PAGINATION
    function search() {
        var filterRequest = $("#frmSearch").serialize().replace(/[^&]+=&/g, '').replace(/&[^&]+=$/g, '');
        $.ajax({
            type: 'GET',
            url: window.SearchNewsUrl,
            data: filterRequest,
            ContentType: 'application/json;utf-8',
            datatype: 'json',
            success: function (data) {
                $("#gridcontainer").html(data);
            },
            error: function (jqXhr, textStatus, errorThrown) {
                $("#gridcontainer").html('<span>' + textStatus + ", " + errorThrown + '</span>');
            }
        });
    }

    $(document).on("click", ".search", function () {
        search();
    });

    function addData() {
        var form = $("#frmNews");
        var params = form.serializeArray();
        var formData = new FormData();
        formData.append("File", $('input[type=file]')[0].files[0]);
        $.each(params, function (i, val) {
            formData.append(val.name, val.value);
        });

        $.ajax({
            type: 'POST',
            url: window.CreateNewsUrl,
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    window.location.href = window.IndexNewsUrl;
                    //resetControls();
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
        return false;
    }
    
    function editData() {
        var form = $("#frmNews");
        var params = form.serializeArray();
        var formData = new FormData();
        formData.append("File", $('input[type=file]')[0].files[0]);
        $.each(params, function (i, val) {
            formData.append(val.name, val.value);
        });

        $.ajax({
            type: 'POST',
            url: window.EditNewsUrl,
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
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

    function deleteData(url, id) {
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

    //UPDATE STATUS
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


    //ADD
    $(document).on("click", ".createNews", function (e) {
        e.preventDefault();

        var form = $('#frmNews');
        form.validate();
        if (!form.valid()) { // Not Valid
            return false;
        }
        else {
            addData();
            return false;
        }
    });

    //EDIT
    $(document).on("click", ".editNews", function (e) {
        e.preventDefault();

        var form = $('#frmNews');
        form.validate();
        if (!form.valid()) { // Not Valid
            return false;
        }
        else {
            editData();
            return false;
        }
    });

    $(document).on("click", ".updateNews", function (e) {
        e.preventDefault();

        var form = $('#frmNews');
        form.validate();
        if (!form.valid()) { // Not Valid
            return false;
        }else {
            var params = form.serializeArray();
            var formData = new FormData();
            formData.append("File", $('input[type=file]')[0].files[0]);
            $.each(params, function (i, val) {
                formData.append(val.name, val.value);
            });

            $.ajax({
                type: 'POST',
                url: window.EditNewsUrl,
                data: formData,
                cache: false,
                contentType: false,
                processData: false,
                success: function (response, textStatus, jqXhr) {
                    if (response.Status === 0) {
                        window.location.href = window.IndexNewsUrl;
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
    
    $('#Summary').keypress(function () {
        if (this.value.length >= 4000) {
            return false;
        }
        return true;
    });

    //UPDATE STATUS
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

    //DELETE
    $(document).on("click", ".deleteItem", function (e) {
        e.preventDefault();

        var id = $(this).data('id');
        var url = $(this).data('url');

        var box = bootbox.confirm({
            className: "my-modal",
            size: 'small',
            title: window.WarningWarning,
            message: window.DoYouWantToContinue,
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

    //RESET
    $(document).on("click", ".reset", function (e) {
        e.preventDefault();
        resetControls();
    });
})(jQuery);